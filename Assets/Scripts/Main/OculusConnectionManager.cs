// Decompiled with JetBrains decompiler
// Type: OculusConnectionManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BGNet.Core;
using BGNet.Logging;
using LiteNetLib.Utils;
using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class OculusConnectionManager : IConnectionManager, IPollable, IDisposable
{
  protected readonly List<OculusConnectionManager.OculusConnection> _connections = new List<OculusConnectionManager.OculusConnection>();
  protected readonly NetDataReader _dataReader = new NetDataReader();
  protected OculusNetworkPlayerModel _oculusNetworkPlayerModel;
  protected OculusConnectionManager.NetworkMode _mode;
  protected bool _connectionEstablished;
  [CompilerGenerated]
  protected bool m_CisDisconnecting;
  protected byte[] _buffer = new byte[2048];

  public event System.Action onInitializedEvent;

  public event System.Action onConnectedEvent;

  public event System.Action<DisconnectedReason> onDisconnectedEvent;

  public event System.Action<ConnectionFailedReason> onConnectionFailedEvent;

  public event System.Action<IConnection> onConnectionConnectedEvent;

  public event System.Action<IConnection, DisconnectedReason> onConnectionDisconnectedEvent;

  public event System.Action<IConnection, NetDataReader, DeliveryMethod> onReceivedDataEvent;

  public string userId => this._oculusNetworkPlayerModel.localPlayer.userId;

  public string userName => this._oculusNetworkPlayerModel.localPlayer.userName;

  public bool isConnected
  {
    get
    {
      if (this._mode == OculusConnectionManager.NetworkMode.Server)
        return true;
      return this._mode == OculusConnectionManager.NetworkMode.Client && this._connectionEstablished;
    }
  }

  public bool isConnecting => this._mode == OculusConnectionManager.NetworkMode.Client && !this._connectionEstablished;

  public bool isDisconnecting
  {
    get => this.m_CisDisconnecting;
    private set => this.m_CisDisconnecting = value;
  }

  public bool isDisposed => this._mode == OculusConnectionManager.NetworkMode.None;

  public bool isConnectionOwner => this._mode == OculusConnectionManager.NetworkMode.Server;

  public bool isServer => this._mode == OculusConnectionManager.NetworkMode.Server;

  public bool isClient => this._mode == OculusConnectionManager.NetworkMode.Client;

  public int connectionCount => this._connections.Count;

  public virtual void SendToAll(NetDataWriter writer, DeliveryMethod deliveryMethod) => this.SendToAll(writer, deliveryMethod, (IConnection) null);

  public virtual void SendToAll(
    NetDataWriter writer,
    DeliveryMethod deliveryMethod,
    IConnection excludingConnection)
  {
    for (int index = 0; index < this._connections.Count; ++index)
    {
      if (!object.Equals((object) this._connections[index], (object) excludingConnection))
        this._connections[index].Send(writer, deliveryMethod);
    }
  }

  public virtual void PollUpdate()
  {
    Packet packet;
    while ((packet = Net.ReadPacket()) != null)
    {
      OculusConnectionManager.OculusConnection connection = this.GetConnection(packet.SenderID);
      byte[] numArray = this.AcquireBuffer((int) packet.Size);
      int maxSize = (int) packet.ReadBytes(numArray);
      this._dataReader.SetSource(numArray, 0, maxSize);
      System.Action<IConnection, NetDataReader, DeliveryMethod> receivedDataEvent = this.onReceivedDataEvent;
      if (receivedDataEvent != null)
        receivedDataEvent((IConnection) connection, this._dataReader, OculusConnectionManager.SendPolicyToDeliveryMethod(packet.Policy));
      packet.Dispose();
      this.ReleaseBuffer(numArray);
    }
  }

  public virtual bool Init<T>(IConnectionInitParams<T> initParams) where T : IConnectionManager
  {
    if (this._mode != OculusConnectionManager.NetworkMode.None)
      return false;
    switch (initParams)
    {
      case OculusConnectionManager.StartServerParams startServerParams:
        this._mode = OculusConnectionManager.NetworkMode.Server;
        this._oculusNetworkPlayerModel = startServerParams.oculusNetworkPlayerModel;
        Net.SetPeerConnectRequestCallback(new Message<NetworkingPeer>.Callback(this.HandlePeerConnectionRequest));
        Net.SetConnectionStateChangedCallback(new Message<NetworkingPeer>.Callback(this.HandleConnectionStateChanged));
        System.Action initializedEvent1 = this.onInitializedEvent;
        if (initializedEvent1 != null)
          initializedEvent1();
        System.Action onConnectedEvent = this.onConnectedEvent;
        if (onConnectedEvent != null)
          onConnectedEvent();
        return true;
      case OculusConnectionManager.ConnectToServerParams connectToServerParams:
        this._mode = OculusConnectionManager.NetworkMode.Client;
        this._oculusNetworkPlayerModel = connectToServerParams.oculusNetworkPlayerModel;
        Net.SetConnectionStateChangedCallback(new Message<NetworkingPeer>.Callback(this.HandleConnectionStateChanged));
        Net.Connect(connectToServerParams.serverId);
        System.Action initializedEvent2 = this.onInitializedEvent;
        if (initializedEvent2 != null)
          initializedEvent2();
        return true;
      default:
        return false;
    }
  }

  public virtual void Disconnect(DisconnectedReason disconnectedReason = DisconnectedReason.UserInitiated) => this.DisconnectInternal(disconnectedReason, ConnectionFailedReason.ConnectionCanceled);

  public virtual void DisconnectInternal(
    DisconnectedReason disconnectedReason = DisconnectedReason.Unknown,
    ConnectionFailedReason connectionFailedReason = ConnectionFailedReason.Unknown)
  {
    if (this._mode == OculusConnectionManager.NetworkMode.None)
      return;
    bool isServer = this.isServer;
    bool flag = !this._connectionEstablished && this.isClient;
    this.isDisconnecting = true;
    this._mode = OculusConnectionManager.NetworkMode.None;
    Net.SetPeerConnectRequestCallback(new Message<NetworkingPeer>.Callback(OculusConnectionManager.VoidHandler));
    Net.SetConnectionStateChangedCallback(new Message<NetworkingPeer>.Callback(OculusConnectionManager.VoidHandler));
    for (int index = this._connections.Count - 1; index >= 0; --index)
    {
      OculusConnectionManager.OculusConnection connection = this._connections[index];
      this._connections.RemoveAt(index);
      connection.Disconnect();
      System.Action<IConnection, DisconnectedReason> disconnectedEvent = this.onConnectionDisconnectedEvent;
      if (disconnectedEvent != null)
        disconnectedEvent((IConnection) connection, disconnectedReason);
    }
    if (isServer)
      Net.CloseForCurrentRoom();
    this.isDisconnecting = false;
    if (flag)
    {
      System.Action<ConnectionFailedReason> connectionFailedEvent = this.onConnectionFailedEvent;
      if (connectionFailedEvent == null)
        return;
      connectionFailedEvent(connectionFailedReason);
    }
    else
    {
      System.Action<DisconnectedReason> disconnectedEvent = this.onDisconnectedEvent;
      if (disconnectedEvent == null)
        return;
      disconnectedEvent(disconnectedReason);
    }
  }

  public virtual void Dispose() => this.Disconnect(DisconnectedReason.UserInitiated);

  public virtual Task DisposeAsync()
  {
    this.Dispose();
    return Task.CompletedTask;
  }

  public virtual IConnection GetConnection(int index) => (IConnection) this._connections[index];

  public virtual async void HandlePeerConnectionRequest(Message<NetworkingPeer> message)
  {
    if (message.IsError)
      return;
    Debug.Log("Received Peer Connection Request " + (object) message.Data.ID);
    if (!await this._oculusNetworkPlayerModel.ShouldAcceptConnectionFromPlayer(message.Data.ID))
      return;
    Net.Accept(message.Data.ID);
  }

  public virtual void HandleConnectionStateChanged(Message<NetworkingPeer> message)
  {
    if (message.IsError)
      return;
    switch (message.Data.State)
    {
      case PeerConnectionState.Connected:
        if (this.isClient && !this._connectionEstablished)
        {
          this._connectionEstablished = true;
          System.Action onConnectedEvent = this.onConnectedEvent;
          if (onConnectedEvent != null)
            onConnectedEvent();
        }
        this.GetConnection(message.Data.ID);
        break;
      case PeerConnectionState.Timeout:
        this.RemoveConnection(message.Data.ID, DisconnectedReason.Timeout);
        break;
      case PeerConnectionState.Closed:
        this.RemoveConnection(message.Data.ID, this.isClient ? DisconnectedReason.ServerConnectionClosed : DisconnectedReason.UserInitiated);
        break;
    }
  }

  public virtual OculusConnectionManager.OculusConnection GetConnection(ulong user)
  {
    for (int index = 0; index < this._connections.Count; ++index)
    {
      if ((long) this._connections[index].id == (long) user)
        return this._connections[index];
    }
    OculusConnectionManager.OculusConnection connection = new OculusConnectionManager.OculusConnection(user, this._oculusNetworkPlayerModel.GetUserName(user), !this.isServer);
    this._connections.Add(connection);
    System.Action<IConnection> connectionConnectedEvent = this.onConnectionConnectedEvent;
    if (connectionConnectedEvent != null)
      connectionConnectedEvent((IConnection) connection);
    return connection;
  }

  public virtual void RemoveConnection(ulong id, DisconnectedReason reason)
  {
    for (int index = 0; index < this._connections.Count; ++index)
    {
      OculusConnectionManager.OculusConnection connection = this._connections[index];
      if ((long) connection.id == (long) id)
      {
        this._connections.RemoveAt(index);
        System.Action<IConnection, DisconnectedReason> disconnectedEvent = this.onConnectionDisconnectedEvent;
        if (disconnectedEvent != null)
        {
          disconnectedEvent((IConnection) connection, reason);
          break;
        }
        break;
      }
    }
    if (!this.isClient || this._connections.Count != 0)
      return;
    this.DisconnectInternal(DisconnectedReason.ServerConnectionClosed, ConnectionFailedReason.ServerUnreachable);
  }

  public virtual byte[] AcquireBuffer(int size)
  {
    if (size > this._buffer.Length)
      this._buffer = new byte[size];
    return this._buffer;
  }

  public virtual void ReleaseBuffer(byte[] buffer)
  {
  }

  private static SendPolicy DeliveryMethodToSendPolicy(DeliveryMethod deliveryMethod) => deliveryMethod == DeliveryMethod.ReliableOrdered ? SendPolicy.Reliable : SendPolicy.Unreliable;

  private static DeliveryMethod SendPolicyToDeliveryMethod(SendPolicy sendPolicy) => sendPolicy == SendPolicy.Reliable ? DeliveryMethod.ReliableOrdered : DeliveryMethod.Unreliable;

  private static void VoidHandler(Message<NetworkingPeer> message)
  {
  }

  public enum NetworkMode
  {
    None,
    Client,
    Server,
  }

  public class OculusConnection : IConnection, IEquatable<OculusConnectionManager.OculusConnection>
  {
    protected readonly ulong _id;
    protected readonly string _userId;
    protected readonly string _userName;
    protected readonly bool _isConnectionOwner;

    public ulong id => this._id;

    public string userId => this._userId;

    public string userName => this._userName;

    public bool isConnectionOwner => this._isConnectionOwner;

    public OculusConnection(ulong id, string userName, bool isConnectionOwner)
    {
      this._id = id;
      this._userId = NetworkUtility.GetHashedUserId(id.ToString(), AuthenticationToken.Platform.OculusRift);
      this._userName = userName;
      this._isConnectionOwner = isConnectionOwner;
    }

    public virtual void Send(NetDataWriter writer, DeliveryMethod deliveryMethod)
    {
      if (!Oculus.Platform.Core.IsInitialized())
        return;
      CAPI.ovr_Net_SendPacket(this._id, (UIntPtr) (ulong) writer.Length, writer.Data, OculusConnectionManager.DeliveryMethodToSendPolicy(deliveryMethod));
    }

    public virtual void Disconnect() => Net.Close(this._id);

    public virtual bool Equals(OculusConnectionManager.OculusConnection other)
    {
      if (other == null)
        return false;
      return this == other || (long) this._id == (long) other._id;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((OculusConnectionManager.OculusConnection) obj);
    }

    public override int GetHashCode() => this._id.GetHashCode();

    public override string ToString() => "[OculusConnection id=" + (object) this._id + "]";
  }

  public class StartServerParams : IConnectionInitParams<OculusConnectionManager>
  {
    public OculusNetworkPlayerModel oculusNetworkPlayerModel;
  }

  public class ConnectToServerParams : IConnectionInitParams<OculusConnectionManager>
  {
    public OculusNetworkPlayerModel oculusNetworkPlayerModel;
    public ulong serverId;
  }
}
