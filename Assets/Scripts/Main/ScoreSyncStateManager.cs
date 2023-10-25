// Decompiled with JetBrains decompiler
// Type: ScoreSyncStateManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ScoreSyncStateManager : 
  MultiplayerSyncStateManager<StandardScoreSyncState, StandardScoreSyncState.Score, int, StandardScoreSyncStateNetSerializable, StandardScoreSyncStateDeltaNetSerializable>,
  IScoreSyncStateManager,
  IScoreSyncStateManager<StandardScoreSyncState, StandardScoreSyncState.Score, int, StandardScoreSyncStateNetSerializable, StandardScoreSyncStateDeltaNetSerializable>
{
  protected override float deltaUpdateFrequency => 0.02f;

  protected override float fullStateUpdateFrequency => 0.5f;

  protected override int localBufferSize => 256;

  protected override int remoteBufferSize => 128;

  protected override IPacketPool<StandardScoreSyncStateNetSerializable> serializablePool => (IPacketPool<StandardScoreSyncStateNetSerializable>) StandardScoreSyncStateNetSerializable.pool;

  protected override IPacketPool<StandardScoreSyncStateDeltaNetSerializable> deltaSerializablePool => StandardScoreSyncStateDeltaNetSerializable.pool;

  protected override MultiplayerSessionManager.MessageType messageType => MultiplayerSessionManager.MessageType.ScoreSyncState;

  protected override MultiplayerSessionManager.MessageType deltaMessageType => MultiplayerSessionManager.MessageType.ScoreSyncStateDelta;

  protected override int Interpolate(
    int prev,
    float prevTime,
    int curr,
    float currTime,
    float time)
  {
    if (Mathf.Abs(curr - prev) <= 40 && (double) Mathf.Abs(currTime - prevTime) < 0.059999998658895493 && (double) currTime > (double) prevTime)
      return (int) Mathf.Lerp((float) prev, (float) curr, (float) (((double) time - (double) prevTime) / ((double) currTime - (double) prevTime)));
    return (double) time < (double) currTime ? prev : curr;
  }
}
