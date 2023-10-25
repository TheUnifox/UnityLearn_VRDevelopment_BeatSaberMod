// Decompiled with JetBrains decompiler
// Type: MultiplayerSyncState`3
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using LiteNetLib.Utils;
using System;

public abstract class MultiplayerSyncState<TStateTable, TType, TState>
  where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable>
  where TType : struct, IConvertible
  where TState : struct
{
  protected abstract StateBuffer<TStateTable, TType, TState> stateBuffer { get; }

  public abstract IConnectedPlayer player { get; }

  public float GetLatestTime() => this.stateBuffer.GetLatestTime();

  public TState GetLatestState(TType type) => this.stateBuffer.GetLatestState(type);

  public virtual TState GetState(TType type, float time) => this.stateBuffer.GetState(type, time);

  public void ClearBufferedStates() => this.stateBuffer.Clear();
}
