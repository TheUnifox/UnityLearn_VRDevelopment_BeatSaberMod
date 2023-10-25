// Decompiled with JetBrains decompiler
// Type: MockPlayerLobbyPoseGeneratorMirror
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class MockPlayerLobbyPoseGeneratorMirror : MockPlayerLobbyPoseGenerator
{
  protected readonly NodePoseSyncStateManager _nodePoseSyncStateManager;

  public MockPlayerLobbyPoseGeneratorMirror(
    IMultiplayerSessionManager multiplayerSessionManager,
    NodePoseSyncStateManager nodePoseSyncStateManager)
    : base(multiplayerSessionManager)
  {
    this._nodePoseSyncStateManager = nodePoseSyncStateManager;
  }

  public override void Init()
  {
  }

  public override void Tick()
  {
    LocalMultiplayerSyncState<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable> localState = this._nodePoseSyncStateManager.localState;
    if (localState == null)
      return;
    this.mockNodePoseSyncStateSender.SendPose(localState.GetState(NodePoseSyncState.NodePose.Head), localState.GetState(NodePoseSyncState.NodePose.LeftController), localState.GetState(NodePoseSyncState.NodePose.RightController));
  }
}
