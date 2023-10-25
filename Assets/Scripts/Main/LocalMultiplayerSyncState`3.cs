// Decompiled with JetBrains decompiler
// Type: LocalMultiplayerSyncState`3
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using LiteNetLib.Utils;
using System;
using UnityEngine;

public class LocalMultiplayerSyncState<TStateTable, TType, TState> : 
  MultiplayerSyncState<TStateTable, TType, TState>
  where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable>
  where TType : struct, IConvertible
  where TState : struct
{
  protected readonly IConnectedPlayer _player;
  protected readonly LocalStateBuffer<TStateTable, TType, TState> _stateBuffer;

  protected override StateBuffer<TStateTable, TType, TState> stateBuffer => (StateBuffer<TStateTable, TType, TState>) this._stateBuffer;

  public override IConnectedPlayer player => this._player;

  public LocalMultiplayerSyncState(
    IConnectedPlayer player,
    float fullStateUpdateFrequency,
    float deltaUpdateFrequency,
    int size,
    StateBuffer<TStateTable, TType, TState>.InterpolationDelegate interpolator,
    StateBuffer<TStateTable, TType, TState>.SmoothingDelegate smoother = null)
  {
    this._player = player;
    this._stateBuffer = new LocalStateBuffer<TStateTable, TType, TState>(fullStateUpdateFrequency, deltaUpdateFrequency, size, interpolator, smoother);
  }

  public virtual bool TryGetSerializedState<T>(IPacketPool<T> pool, out T serializable) where T : IPoolablePacket, ISyncStateSerializable<TStateTable>
  {
    serializable = pool.Obtain();
    TStateTable state;
    float time;
    SyncStateId id;
    if (this._stateBuffer.TryGetSerializedState(out state, out time, out id))
    {
      serializable.id = id;
      serializable.time = time;
      serializable.state = state;
      return true;
    }
    serializable.Release();
    serializable = default (T);
    return false;
  }

  public virtual bool TryGetSerializedStateDelta<T>(IPacketPool<T> pool, out T serializable) where T : IPoolablePacket, ISyncStateDeltaSerializable<TStateTable>
  {
    serializable = pool.Obtain();
    TStateTable delta;
    float timeOffset;
    SyncStateId baseId;
    if (this._stateBuffer.TryGetSerializedStateDelta(out delta, out timeOffset, out baseId))
    {
      serializable.baseId = baseId;
      serializable.timeOffsetMs = Mathf.RoundToInt(1000f * timeOffset);
      serializable.delta = delta;
      return true;
    }
    serializable.Release();
    serializable = default (T);
    return false;
  }

  public virtual void SetDirty() => this._stateBuffer.MarkDirty();

  public virtual void SetCurrentTime(float time) => this._stateBuffer.SetTime(time);

  public virtual void SetState(TType type, TState state) => this._stateBuffer.SetState(type, state);

  public virtual TState GetState(TType type) => this._stateBuffer.GetState(type);

  public TState this[TType t]
  {
    get => this.GetState(t);
    set => this.SetState(t, value);
  }
}
