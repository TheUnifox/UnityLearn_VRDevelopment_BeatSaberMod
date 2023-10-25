// Decompiled with JetBrains decompiler
// Type: MultiplayerSyncStateManager`5
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class MultiplayerSyncStateManager<TStateTable, TType, TState, TSerializable, TDeltaSerializable> : 
  MonoBehaviour
  where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable>
  where TType : struct, IConvertible
  where TState : struct
  where TSerializable : INetSerializable, ISyncStateSerializable<TStateTable>, IPoolablePacket
  where TDeltaSerializable : INetSerializable, ISyncStateDeltaSerializable<TStateTable>, IPoolablePacket
{
  [Inject]
  private readonly IMultiplayerSessionManager _multiplayerSessionManager;
  private LocalMultiplayerSyncState<TStateTable, TType, TState> _localState;
  private readonly List<RemoteMultiplayerSyncState<TStateTable, TType, TState>> _connectedPlayerStates = new List<RemoteMultiplayerSyncState<TStateTable, TType, TState>>();
  private readonly List<RemoteMultiplayerSyncState<TStateTable, TType, TState>> _disconnectedPlayerStates = new List<RemoteMultiplayerSyncState<TStateTable, TType, TState>>();

  protected IMultiplayerSessionManager multiplayerSessionManager => this._multiplayerSessionManager;

  public LocalMultiplayerSyncState<TStateTable, TType, TState> localState => this._localState;

  public int connectedPlayerCount => this._connectedPlayerStates.Count + (this._localState != null ? 1 : 0);

  public int disconnectedPlayerCount => this._disconnectedPlayerStates.Count;

  public virtual float syncTime => this._multiplayerSessionManager.syncTime;

  protected abstract float deltaUpdateFrequency { get; }

  protected abstract float fullStateUpdateFrequency { get; }

  protected abstract int localBufferSize { get; }

  protected abstract int remoteBufferSize { get; }

  protected abstract IPacketPool<TSerializable> serializablePool { get; }

  protected abstract IPacketPool<TDeltaSerializable> deltaSerializablePool { get; }

  protected abstract MultiplayerSessionManager.MessageType messageType { get; }

  protected abstract MultiplayerSessionManager.MessageType deltaMessageType { get; }

  protected void Start()
  {
    this.serializablePool.Fill();
    this.deltaSerializablePool.Fill();
    this._multiplayerSessionManager.RegisterCallback<TSerializable>(this.messageType, new System.Action<TSerializable, IConnectedPlayer>(this.HandleSyncPacket), new Func<TSerializable>(this.serializablePool.Obtain));
    this._multiplayerSessionManager.RegisterCallback<TDeltaSerializable>(this.deltaMessageType, new System.Action<TDeltaSerializable, IConnectedPlayer>(this.HandleSyncDeltaPacket), new Func<TDeltaSerializable>(this.deltaSerializablePool.Obtain));
    this._multiplayerSessionManager.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.HandlePlayerConnected);
    this._multiplayerSessionManager.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
    this.TryCreateLocalState();
    for (int index = 0; index < this._multiplayerSessionManager.connectedPlayerCount; ++index)
      this._connectedPlayerStates.Add(new RemoteMultiplayerSyncState<TStateTable, TType, TState>(this._multiplayerSessionManager.GetConnectedPlayer(index), this.remoteBufferSize, new StateBuffer<TStateTable, TType, TState>.InterpolationDelegate(this.Interpolate), new StateBuffer<TStateTable, TType, TState>.SmoothingDelegate(this.Smooth)));
    this._localState?.SetDirty();
  }

  protected void LateUpdate()
  {
    if (!this._multiplayerSessionManager.isConnectingOrConnected || !this._multiplayerSessionManager.isSyncTimeInitialized)
      return;
    if (this._localState == null)
    {
      this.TryCreateLocalState();
    }
    else
    {
      this._localState.SetCurrentTime(this.syncTime);
      TSerializable serializable1;
      if (this._localState.TryGetSerializedState<TSerializable>(this.serializablePool, out serializable1))
        this._multiplayerSessionManager.Send<TSerializable>(serializable1);
      TDeltaSerializable serializable2;
      if (!this._localState.TryGetSerializedStateDelta<TDeltaSerializable>(this.deltaSerializablePool, out serializable2))
        return;
      this._multiplayerSessionManager.SendUnreliable<TDeltaSerializable>(serializable2);
    }
  }

  protected void OnDestroy()
  {
    if (this._multiplayerSessionManager == null)
      return;
    this._multiplayerSessionManager.UnregisterCallback<TSerializable>(this.messageType);
    this._multiplayerSessionManager.UnregisterCallback<TDeltaSerializable>(this.deltaMessageType);
    this._multiplayerSessionManager.playerConnectedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerConnected);
    this._multiplayerSessionManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandlePlayerDisconnected);
  }

  protected abstract TState Interpolate(TState a, float timeA, TState b, float timeB, float time);

  protected virtual TState Smooth(TState a, TState b, float smoooth) => b;

  public void ClearBufferedStates()
  {
    this._localState?.ClearBufferedStates();
    foreach (MultiplayerSyncState<TStateTable, TType, TState> connectedPlayerState in this._connectedPlayerStates)
      connectedPlayerState.ClearBufferedStates();
  }

  private void TryCreateLocalState()
  {
    if (this._localState != null || this._multiplayerSessionManager.localPlayer == null || this._multiplayerSessionManager.isSpectating)
      return;
    this._localState = new LocalMultiplayerSyncState<TStateTable, TType, TState>(this._multiplayerSessionManager.localPlayer, this.fullStateUpdateFrequency, this.deltaUpdateFrequency, this.localBufferSize, new StateBuffer<TStateTable, TType, TState>.InterpolationDelegate(this.Interpolate));
  }

  private void HandlePlayerConnected(IConnectedPlayer player)
  {
    if (this._localState == null && this._multiplayerSessionManager.localPlayer != null && !this._multiplayerSessionManager.isSpectating)
      this._localState = new LocalMultiplayerSyncState<TStateTable, TType, TState>(this._multiplayerSessionManager.localPlayer, this.fullStateUpdateFrequency, this.deltaUpdateFrequency, this.localBufferSize, new StateBuffer<TStateTable, TType, TState>.InterpolationDelegate(this.Interpolate));
    this._localState?.SetDirty();
    for (int index = 0; index < this._disconnectedPlayerStates.Count; ++index)
    {
      if (this._disconnectedPlayerStates[index].player.userId == player.userId)
      {
        this._disconnectedPlayerStates.RemoveAt(index);
        break;
      }
    }
    this._connectedPlayerStates.InsertSorted<RemoteMultiplayerSyncState<TStateTable, TType, TState>>(new RemoteMultiplayerSyncState<TStateTable, TType, TState>(player, this.remoteBufferSize, new StateBuffer<TStateTable, TType, TState>.InterpolationDelegate(this.Interpolate)), (Func<RemoteMultiplayerSyncState<TStateTable, TType, TState>, int>) (s => s.player.sortIndex));
  }

  private void HandlePlayerDisconnected(IConnectedPlayer player)
  {
    for (int index = 0; index < this._connectedPlayerStates.Count; ++index)
    {
      if (this._connectedPlayerStates[index].player == player)
      {
        this._disconnectedPlayerStates.Add(this._connectedPlayerStates[index]);
        this._connectedPlayerStates.RemoveAt(index);
        break;
      }
    }
  }

  private void HandleSyncPacket(TSerializable packet, IConnectedPlayer player)
  {
    for (int index = 0; index < this._connectedPlayerStates.Count; ++index)
    {
      if (this._connectedPlayerStates[index].player == player)
      {
        this._connectedPlayerStates[index].UpdateState<TSerializable>(packet);
        break;
      }
    }
    packet.Release();
  }

  private void HandleSyncDeltaPacket(TDeltaSerializable packet, IConnectedPlayer player)
  {
    for (int index = 0; index < this._connectedPlayerStates.Count; ++index)
    {
      if (this._connectedPlayerStates[index].player == player)
      {
        this._connectedPlayerStates[index].UpdateDelta<TDeltaSerializable>(packet);
        break;
      }
    }
    packet.Release();
  }

  public MultiplayerSyncState<TStateTable, TType, TState> GetSyncState(int i)
  {
    if (this._localState != null)
    {
      if (i == 0)
        return (MultiplayerSyncState<TStateTable, TType, TState>) this._localState;
      --i;
    }
    return (MultiplayerSyncState<TStateTable, TType, TState>) this._connectedPlayerStates[i];
  }

  public MultiplayerSyncState<TStateTable, TType, TState> GetSyncStateForPlayer(
    IConnectedPlayer player)
  {
    for (int index = 0; index < this._connectedPlayerStates.Count; ++index)
    {
      if (this._connectedPlayerStates[index].player == player)
        return (MultiplayerSyncState<TStateTable, TType, TState>) this._connectedPlayerStates[index];
    }
    return player.isMe ? (MultiplayerSyncState<TStateTable, TType, TState>) this._localState : (MultiplayerSyncState<TStateTable, TType, TState>) null;
  }

  public MultiplayerSyncState<TStateTable, TType, TState> GetDisconnectedSyncState(int i) => (MultiplayerSyncState<TStateTable, TType, TState>) this._disconnectedPlayerStates[i];
}
