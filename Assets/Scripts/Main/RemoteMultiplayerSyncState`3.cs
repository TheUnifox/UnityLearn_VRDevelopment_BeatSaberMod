// Decompiled with JetBrains decompiler
// Type: RemoteMultiplayerSyncState`3
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using LiteNetLib.Utils;
using System;

public class RemoteMultiplayerSyncState<TStateTable, TType, TState> : 
  MultiplayerSyncState<TStateTable, TType, TState>
  where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable>
  where TType : struct, IConvertible
  where TState : struct
{
  protected readonly IConnectedPlayer _player;
  protected readonly RemoteStateBuffer<TStateTable, TType, TState> _stateBuffer;

  protected override StateBuffer<TStateTable, TType, TState> stateBuffer => (StateBuffer<TStateTable, TType, TState>) this._stateBuffer;

  public override IConnectedPlayer player => this._player;

  public virtual void UpdateState<T>(T serializable) where T : ISyncStateSerializable<TStateTable> => this._stateBuffer.PushState(serializable.id, serializable.state, serializable.time);

  public virtual void UpdateDelta<T>(T serializable) where T : ISyncStateDeltaSerializable<TStateTable> => this._stateBuffer.PushDelta(serializable.baseId, serializable.delta, (float) serializable.timeOffsetMs / 1000f);

  public RemoteMultiplayerSyncState(
    IConnectedPlayer player,
    int size,
    StateBuffer<TStateTable, TType, TState>.InterpolationDelegate interpolator,
    StateBuffer<TStateTable, TType, TState>.SmoothingDelegate smoother = null)
  {
    this._player = player;
    this._stateBuffer = new RemoteStateBuffer<TStateTable, TType, TState>(size, interpolator, smoother);
  }
}
