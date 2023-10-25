// Decompiled with JetBrains decompiler
// Type: NodePoseSyncStateManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class NodePoseSyncStateManager : 
  MultiplayerSyncStateManager<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable, NodePoseSyncStateNetSerializable, NodePoseSyncStateDeltaNetSerializable>,
  INodePoseSyncStateManager,
  INodePoseSyncStateManager<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable, NodePoseSyncStateNetSerializable, NodePoseSyncStateDeltaNetSerializable>
{
  protected override float deltaUpdateFrequency => 0.01f;

  protected override float fullStateUpdateFrequency => 0.1f;

  protected override int localBufferSize => 32;

  protected override int remoteBufferSize => 256;

  protected override IPacketPool<NodePoseSyncStateNetSerializable> serializablePool => (IPacketPool<NodePoseSyncStateNetSerializable>) NodePoseSyncStateNetSerializable.pool;

  protected override IPacketPool<NodePoseSyncStateDeltaNetSerializable> deltaSerializablePool => NodePoseSyncStateDeltaNetSerializable.pool;

  protected override MultiplayerSessionManager.MessageType messageType => MultiplayerSessionManager.MessageType.NodePoseSyncState;

  protected override MultiplayerSessionManager.MessageType deltaMessageType => MultiplayerSessionManager.MessageType.NodePoseSyncStateDelta;

  protected override PoseSerializable Interpolate(
    PoseSerializable prev,
    float prevTime,
    PoseSerializable curr,
    float currTime,
    float time)
  {
    return PosePrediction.PredictPoseSerializable(prev, prevTime, curr, currTime, time);
  }

  protected override PoseSerializable Smooth(PoseSerializable a, PoseSerializable b, float smooth) => PosePrediction.InterpolatePoseSerializable(a, b, smooth);
}
