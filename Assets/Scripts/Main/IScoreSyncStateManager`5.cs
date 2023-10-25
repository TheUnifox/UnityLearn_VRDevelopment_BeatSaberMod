// Decompiled with JetBrains decompiler
// Type: IScoreSyncStateManager`5
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using LiteNetLib.Utils;
using System;

public interface IScoreSyncStateManager<TStateTable, TType, TState, TSerializable, TDeltaSerializable>
  where TStateTable : struct, IStateTable<TStateTable, TType, TState>, INetSerializable, IEquatableByReference<TStateTable>
  where TType : struct, IConvertible
  where TState : struct
  where TSerializable : INetSerializable, ISyncStateSerializable<TStateTable>, IPoolablePacket
  where TDeltaSerializable : INetSerializable, ISyncStateDeltaSerializable<TStateTable>, IPoolablePacket
{
  float syncTime { get; }

  int connectedPlayerCount { get; }

  LocalMultiplayerSyncState<TStateTable, TType, TState> localState { get; }

  MultiplayerSyncState<TStateTable, TType, TState> GetSyncStateForPlayer(IConnectedPlayer player);

  MultiplayerSyncState<TStateTable, TType, TState> GetSyncState(int i);
}
