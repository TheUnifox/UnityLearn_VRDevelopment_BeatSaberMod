using System;

// Token: 0x02000006 RID: 6
public class MockNodePoseSyncStateSender : IDisposable
{
    // Token: 0x06000012 RID: 18 RVA: 0x0000213E File Offset: 0x0000033E
    public MockNodePoseSyncStateSender(IMultiplayerSessionManager msm)
    {
        this._multiplayerSessionManager = msm;
        this._multiplayerSessionManager.RegisterCallback<NodePoseSyncStateNetSerializable>(MultiplayerSessionManager.MessageType.NodePoseSyncState, new Action<NodePoseSyncStateNetSerializable, IConnectedPlayer>(this.HandleNodePoseSyncStateUpdate), new Func<NodePoseSyncStateNetSerializable>(NodePoseSyncStateNetSerializable.pool.Obtain));
    }

    // Token: 0x06000013 RID: 19 RVA: 0x00002176 File Offset: 0x00000376
    public void Dispose()
    {
        this._multiplayerSessionManager.UnregisterCallback<NodePoseSyncStateNetSerializable>(MultiplayerSessionManager.MessageType.NodePoseSyncState);
    }

    // Token: 0x06000014 RID: 20 RVA: 0x00002184 File Offset: 0x00000384
    public void SendPose(PoseSerializable headPose, PoseSerializable leftHandPose, PoseSerializable rightHandPose)
    {
        NodePoseSyncState state = default(NodePoseSyncState);
        state.SetState(NodePoseSyncState.NodePose.Head, headPose);
        state.SetState(NodePoseSyncState.NodePose.LeftController, leftHandPose);
        state.SetState(NodePoseSyncState.NodePose.RightController, rightHandPose);
        NodePoseSyncStateNetSerializable nodePoseSyncStateNetSerializable = NodePoseSyncStateNetSerializable.pool.Obtain();
        nodePoseSyncStateNetSerializable.state = state;
        nodePoseSyncStateNetSerializable.time = this._multiplayerSessionManager.syncTime;
        this._multiplayerSessionManager.SendUnreliable<NodePoseSyncStateNetSerializable>(nodePoseSyncStateNetSerializable);
    }

    // Token: 0x06000015 RID: 21 RVA: 0x000021E3 File Offset: 0x000003E3
    private void HandleNodePoseSyncStateUpdate(NodePoseSyncStateNetSerializable nodePose, IConnectedPlayer connectedPlayer)
    {
    }

    // Token: 0x0400000B RID: 11
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;
}
