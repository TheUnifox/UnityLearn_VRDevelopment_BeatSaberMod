// Decompiled with JetBrains decompiler
// Type: LocalNetworkDiscoveryManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LocalNetworkDiscoveryManager : MonoBehaviour, INetEventListener
{
  protected const float kBroadcastInterval = 5f;
  protected const uint kSignature = 3014087859;
  protected NetManager _netManager;
  protected readonly NetworkPacketSerializer<LocalNetworkDiscoveryManager.PacketType, IPEndPoint> _packetSerializer = new NetworkPacketSerializer<LocalNetworkDiscoveryManager.PacketType, IPEndPoint>();
  protected readonly NetDataWriter _netDataWriter = new NetDataWriter();
  protected readonly LocalNetworkDiscoveryManager.BroadcastPacket _broadcastPacket = new LocalNetworkDiscoveryManager.BroadcastPacket()
  {
    version = 8
  };
  protected int _discoveryPort;
  protected bool _initialized;
  protected bool _enableBroadcasting;
  protected float _lastBroadcastTime;

  public event LocalNetworkDiscoveryManager.PeerUpdatedDelegate peerUpdatedEvent;

  public event LocalNetworkDiscoveryManager.JoinRequestedDelegate joinRequestedEvent;

  public event LocalNetworkDiscoveryManager.JoinRespondedDelegate joinRespondedEvent;

  public event LocalNetworkDiscoveryManager.InviteRequestedDelegate inviteRequestedEvent;

  public event LocalNetworkDiscoveryManager.InviteRespondedDelegate inviteRespondedEvent;

  public string userId => this._broadcastPacket.userId;

  public string userName => this._broadcastPacket.userName;

  public bool isPartyOwner
  {
    get => this._broadcastPacket.isPartyOwner;
    set
    {
      if (this._broadcastPacket.isPartyOwner == value)
        return;
      this._broadcastPacket.isPartyOwner = value;
      this._lastBroadcastTime = -5f;
    }
  }

  public int currentPartySize
  {
    get => this._broadcastPacket.currentPartySize;
    set
    {
      if (this._broadcastPacket.currentPartySize == value)
        return;
      this._broadcastPacket.currentPartySize = value;
      this._lastBroadcastTime = -5f;
    }
  }

  public GameplayServerConfiguration configuration
  {
    get => this._broadcastPacket.configuration;
    set
    {
      if (!(this._broadcastPacket.configuration != value))
        return;
      this._broadcastPacket.configuration = value;
      this._lastBroadcastTime = -5f;
    }
  }

  public BeatmapLevelSelectionMask selectionMask
  {
    get => this._broadcastPacket.selectionMask;
    set
    {
      if (!(this._broadcastPacket.selectionMask != value))
        return;
      this._broadcastPacket.selectionMask = value;
      this._lastBroadcastTime = -5f;
    }
  }

  public bool enableBroadcasting
  {
    get => this._enableBroadcasting;
    set => this._enableBroadcasting = value;
  }

  public virtual void Init(int discoveryPort, string initUserId, string initUserName)
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._discoveryPort = discoveryPort;
    this._broadcastPacket.userId = initUserId;
    this._broadcastPacket.userName = initUserName;
    this._packetSerializer.RegisterCallback<LocalNetworkDiscoveryManager.BroadcastPacket>(LocalNetworkDiscoveryManager.PacketType.Broadcast, (System.Action<LocalNetworkDiscoveryManager.BroadcastPacket, IPEndPoint>) ((packet, endPoint) =>
    {
      if (packet.version != 8U || !(packet.userId != this._broadcastPacket.userId))
        return;
      LocalNetworkDiscoveryManager.PeerUpdatedDelegate peerUpdatedEvent = this.peerUpdatedEvent;
      if (peerUpdatedEvent == null)
        return;
      peerUpdatedEvent(packet.userId, endPoint.Address, packet.userName, packet.currentPartySize, packet.isPartyOwner, packet.selectionMask, packet.configuration);
    }));
    this._packetSerializer.RegisterCallback<LocalNetworkDiscoveryManager.JoinRequestPacket>(LocalNetworkDiscoveryManager.PacketType.JoinRequest, (System.Action<LocalNetworkDiscoveryManager.JoinRequestPacket, IPEndPoint>) ((packet, endPoint) =>
    {
      LocalNetworkDiscoveryManager.JoinRequestedDelegate joinRequestedEvent = this.joinRequestedEvent;
      if (joinRequestedEvent == null)
        return;
      joinRequestedEvent(packet.userId, endPoint.Address, packet.userName);
    }));
    this._packetSerializer.RegisterCallback<LocalNetworkDiscoveryManager.JoinResponsePacket>(LocalNetworkDiscoveryManager.PacketType.JoinResponse, (System.Action<LocalNetworkDiscoveryManager.JoinResponsePacket, IPEndPoint>) ((packet, endPoint) =>
    {
      LocalNetworkDiscoveryManager.JoinRespondedDelegate joinRespondedEvent = this.joinRespondedEvent;
      if (joinRespondedEvent == null)
        return;
      joinRespondedEvent(packet.userId, packet.secret, packet.multiplayerPort, packet.blocked, packet.isPartyOwner, packet.selectionMask, packet.configuration);
    }));
    this._packetSerializer.RegisterCallback<LocalNetworkDiscoveryManager.InviteRequestPacket>(LocalNetworkDiscoveryManager.PacketType.InviteRequest, (System.Action<LocalNetworkDiscoveryManager.InviteRequestPacket, IPEndPoint>) ((packet, endPoint) =>
    {
      LocalNetworkDiscoveryManager.InviteRequestedDelegate inviteRequestedEvent = this.inviteRequestedEvent;
      if (inviteRequestedEvent == null)
        return;
      inviteRequestedEvent(packet.userId, endPoint.Address, packet.userName, packet.secret, packet.multiplayerPort, packet.isPartyOwner, packet.selectionMask, packet.configuration);
    }));
    this._packetSerializer.RegisterCallback<LocalNetworkDiscoveryManager.InviteResponsePacket>(LocalNetworkDiscoveryManager.PacketType.InviteResponse, (System.Action<LocalNetworkDiscoveryManager.InviteResponsePacket, IPEndPoint>) ((packet, endPoint) =>
    {
      LocalNetworkDiscoveryManager.InviteRespondedDelegate inviteRespondedEvent = this.inviteRespondedEvent;
      if (inviteRespondedEvent == null)
        return;
      inviteRespondedEvent(packet.userId, packet.accepted, packet.blocked);
    }));
    this._netManager = new NetManager((INetEventListener) this)
    {
      BroadcastReceiveEnabled = true,
      UnconnectedMessagesEnabled = true
    };
    this._netManager.Start(this._discoveryPort);
    this._netManager.SendBroadcast(new byte[1], this._discoveryPort);
    this._lastBroadcastTime = Time.timeSinceLevelLoad;
  }

  public virtual void OnEnable() => this._netManager?.Start();

  public virtual void OnDisable() => this._netManager?.Stop();

  public virtual void Update()
  {
    if (this._netManager == null)
      return;
    this._netManager.PollEvents();
    if (!this._enableBroadcasting || (double) Time.timeSinceLevelLoad - (double) this._lastBroadcastTime < 5.0)
      return;
    this._lastBroadcastTime = Time.timeSinceLevelLoad;
    this._netManager.SendBroadcast(this.WritePacket<LocalNetworkDiscoveryManager.BroadcastPacket>(this._broadcastPacket), this._discoveryPort);
  }

  public virtual void SendJoinRequest(IPAddress ip)
  {
    IPEndPoint remoteEndPoint = new IPEndPoint(ip, this._discoveryPort);
    this._netManager.SendUnconnectedMessage(this.WritePacket<LocalNetworkDiscoveryManager.JoinRequestPacket>(new LocalNetworkDiscoveryManager.JoinRequestPacket()
    {
      userId = this.userId,
      userName = this.userName
    }), remoteEndPoint);
  }

  public virtual void SendJoinResponse(
    IPAddress ip,
    string secret,
    int multiplayerPort,
    bool blocked,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    IPEndPoint remoteEndPoint = new IPEndPoint(ip, this._discoveryPort);
    this._netManager.SendUnconnectedMessage(this.WritePacket<LocalNetworkDiscoveryManager.JoinResponsePacket>(new LocalNetworkDiscoveryManager.JoinResponsePacket()
    {
      userId = this.userId,
      secret = secret,
      multiplayerPort = multiplayerPort,
      blocked = blocked,
      isPartyOwner = isPartyOwner,
      selectionMask = selectionMask,
      configuration = configuration
    }), remoteEndPoint);
  }

  public virtual void SendInviteRequest(
    IPAddress ip,
    string secret,
    int multiplayerPort,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    IPEndPoint remoteEndPoint = new IPEndPoint(ip, this._discoveryPort);
    this._netManager.SendUnconnectedMessage(this.WritePacket<LocalNetworkDiscoveryManager.InviteRequestPacket>(new LocalNetworkDiscoveryManager.InviteRequestPacket()
    {
      userId = this.userId,
      userName = this.userName,
      secret = secret,
      multiplayerPort = multiplayerPort,
      isPartyOwner = isPartyOwner,
      selectionMask = selectionMask,
      configuration = configuration
    }), remoteEndPoint);
  }

  public virtual void SendInviteResponse(IPAddress ip, bool accepted, bool blocked)
  {
    IPEndPoint remoteEndPoint = new IPEndPoint(ip, this._discoveryPort);
    this._netManager.SendUnconnectedMessage(this.WritePacket<LocalNetworkDiscoveryManager.InviteResponsePacket>(new LocalNetworkDiscoveryManager.InviteResponsePacket()
    {
      userId = this.userId,
      accepted = accepted,
      blocked = blocked
    }), remoteEndPoint);
  }

  public virtual NetDataWriter WritePacket<T>(T packet) where T : INetSerializable, new()
  {
    this._netDataWriter.Reset();
    this._netDataWriter.Put(3014087859U);
    this._packetSerializer.SerializePacket(this._netDataWriter, (INetSerializable) packet);
    return this._netDataWriter;
  }

  void INetEventListener.OnNetworkReceiveUnconnected(
    IPEndPoint remoteEndPoint,
    NetPacketReader reader,
    UnconnectedMessageType messageType)
  {
    uint result;
    if (!reader.TryGetUInt(out result) || result != 3014087859U)
      return;
    this._packetSerializer.ProcessAllPackets((NetDataReader) reader, remoteEndPoint);
  }

  void INetEventListener.OnConnectionRequest(ConnectionRequest request) => request.Reject();

  void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode) => Debug.Log((object) ("[DM] error " + (object) socketErrorCode));

  void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
  {
  }

  void INetEventListener.OnPeerConnected(NetPeer peer)
  {
  }

  void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
  {
  }

  void INetEventListener.OnNetworkReceive(
    NetPeer peer,
    NetPacketReader reader,
    DeliveryMethod deliveryMethod)
  {
  }

  [CompilerGenerated]
  public virtual void m_CInitm_Eb__55_0(
    LocalNetworkDiscoveryManager.BroadcastPacket packet,
    IPEndPoint endPoint)
  {
    if (packet.version != 8U || !(packet.userId != this._broadcastPacket.userId))
      return;
    LocalNetworkDiscoveryManager.PeerUpdatedDelegate peerUpdatedEvent = this.peerUpdatedEvent;
    if (peerUpdatedEvent == null)
      return;
    peerUpdatedEvent(packet.userId, endPoint.Address, packet.userName, packet.currentPartySize, packet.isPartyOwner, packet.selectionMask, packet.configuration);
  }

  [CompilerGenerated]
  public virtual void m_CInitm_Eb__55_1(
    LocalNetworkDiscoveryManager.JoinRequestPacket packet,
    IPEndPoint endPoint)
  {
    LocalNetworkDiscoveryManager.JoinRequestedDelegate joinRequestedEvent = this.joinRequestedEvent;
    if (joinRequestedEvent == null)
      return;
    joinRequestedEvent(packet.userId, endPoint.Address, packet.userName);
  }

  [CompilerGenerated]
  public virtual void m_CInitm_Eb__55_2(
    LocalNetworkDiscoveryManager.JoinResponsePacket packet,
    IPEndPoint endPoint)
  {
    LocalNetworkDiscoveryManager.JoinRespondedDelegate joinRespondedEvent = this.joinRespondedEvent;
    if (joinRespondedEvent == null)
      return;
    joinRespondedEvent(packet.userId, packet.secret, packet.multiplayerPort, packet.blocked, packet.isPartyOwner, packet.selectionMask, packet.configuration);
  }

  [CompilerGenerated]
  public virtual void m_CInitm_Eb__55_3(
    LocalNetworkDiscoveryManager.InviteRequestPacket packet,
    IPEndPoint endPoint)
  {
    LocalNetworkDiscoveryManager.InviteRequestedDelegate inviteRequestedEvent = this.inviteRequestedEvent;
    if (inviteRequestedEvent == null)
      return;
    inviteRequestedEvent(packet.userId, endPoint.Address, packet.userName, packet.secret, packet.multiplayerPort, packet.isPartyOwner, packet.selectionMask, packet.configuration);
  }

  [CompilerGenerated]
  public virtual void m_CInitm_Eb__55_4(
    LocalNetworkDiscoveryManager.InviteResponsePacket packet,
    IPEndPoint endPoint)
  {
    LocalNetworkDiscoveryManager.InviteRespondedDelegate inviteRespondedEvent = this.inviteRespondedEvent;
    if (inviteRespondedEvent == null)
      return;
    inviteRespondedEvent(packet.userId, packet.accepted, packet.blocked);
  }

  public enum PacketType : byte
  {
    Broadcast,
    JoinRequest,
    JoinResponse,
    InviteRequest,
    InviteResponse,
  }

  public class BroadcastPacket : INetSerializable
  {
    public uint version;
    public string userId;
    public string userName;
    public int currentPartySize;
    public bool isPartyOwner;
    public BeatmapLevelSelectionMask selectionMask;
    public GameplayServerConfiguration configuration;

    public virtual void Serialize(NetDataWriter writer)
    {
      writer.PutVarUInt(this.version);
      writer.Put(this.userId);
      writer.Put(this.userName);
      writer.PutVarInt(this.currentPartySize);
      writer.Put(this.isPartyOwner);
      this.selectionMask.Serialize(writer, this.version);
      this.configuration.Serialize(writer);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
      this.version = reader.GetVarUInt();
      this.userId = reader.GetString();
      this.userName = reader.GetString();
      this.currentPartySize = reader.GetVarInt();
      this.isPartyOwner = reader.GetBool();
      this.selectionMask = BeatmapLevelSelectionMask.Deserialize(reader, this.version);
      this.configuration = GameplayServerConfiguration.Deserialize(reader);
    }
  }

  public class JoinRequestPacket : INetSerializable
  {
    public string userId;
    public string userName;

    public virtual void Serialize(NetDataWriter writer)
    {
      writer.Put(this.userId);
      writer.Put(this.userName);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
      this.userId = reader.GetString();
      this.userName = reader.GetString();
    }
  }

  public class JoinResponsePacket : INetSerializable
  {
    public string userId;
    public string secret;
    public int multiplayerPort;
    public bool blocked;
    public bool isPartyOwner;
    public BeatmapLevelSelectionMask selectionMask;
    public GameplayServerConfiguration configuration;

    public virtual void Serialize(NetDataWriter writer)
    {
      writer.Put(this.userId);
      writer.Put(this.secret);
      writer.Put(this.multiplayerPort);
      writer.Put(this.blocked);
      writer.Put(this.isPartyOwner);
      this.selectionMask.Serialize(writer, 8U);
      this.configuration.Serialize(writer);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
      this.userId = reader.GetString();
      this.secret = reader.GetString();
      this.multiplayerPort = reader.GetInt();
      this.blocked = reader.GetBool();
      this.isPartyOwner = reader.GetBool();
      this.selectionMask = BeatmapLevelSelectionMask.Deserialize(reader, 8U);
      this.configuration = GameplayServerConfiguration.Deserialize(reader);
    }
  }

  public class InviteRequestPacket : INetSerializable
  {
    public string userId;
    public string userName;
    public string secret;
    public int multiplayerPort;
    public bool isPartyOwner;
    public BeatmapLevelSelectionMask selectionMask;
    public GameplayServerConfiguration configuration;

    public virtual void Serialize(NetDataWriter writer)
    {
      writer.Put(this.userId);
      writer.Put(this.userName);
      writer.Put(this.secret);
      writer.Put(this.multiplayerPort);
      writer.Put(this.isPartyOwner);
      this.selectionMask.Serialize(writer, 8U);
      this.configuration.Serialize(writer);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
      this.userId = reader.GetString();
      this.userName = reader.GetString();
      this.secret = reader.GetString();
      this.multiplayerPort = reader.GetInt();
      this.isPartyOwner = reader.GetBool();
      this.selectionMask = BeatmapLevelSelectionMask.Deserialize(reader, 8U);
      this.configuration = GameplayServerConfiguration.Deserialize(reader);
    }
  }

  public class InviteResponsePacket : INetSerializable
  {
    public string userId;
    public bool accepted;
    public bool blocked;

    public virtual void Serialize(NetDataWriter writer)
    {
      writer.Put(this.userId);
      writer.Put(this.accepted);
      writer.Put(this.blocked);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
      this.userId = reader.GetString();
      this.accepted = reader.GetBool();
      this.blocked = reader.GetBool();
    }
  }

  public delegate void PeerUpdatedDelegate(
    string userId,
    IPAddress remoteEndPoint,
    string userName,
    int currentPartySize,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration);

  public delegate void JoinRequestedDelegate(
    string userId,
    IPAddress remoteEndPoint,
    string userName);

  public delegate void JoinRespondedDelegate(
    string userId,
    string secret,
    int multiplayerPort,
    bool blocked,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration);

  public delegate void InviteRequestedDelegate(
    string userId,
    IPAddress remoteEndPoint,
    string userName,
    string secret,
    int multiplayerPort,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration);

  public delegate void InviteRespondedDelegate(string userId, bool accepted, bool blocked);
}
