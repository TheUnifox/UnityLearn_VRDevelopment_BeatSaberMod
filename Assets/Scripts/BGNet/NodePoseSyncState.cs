using System;
using LiteNetLib.Utils;

// Token: 0x02000085 RID: 133
public struct NodePoseSyncState : IStateTable<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable>, INetSerializable, IEquatableByReference<NodePoseSyncState>
{
    // Token: 0x06000571 RID: 1393 RVA: 0x0000ECE1 File Offset: 0x0000CEE1
    public void Serialize(NetDataWriter writer)
    {
        this._head.Serialize(writer);
        this._leftController.Serialize(writer);
        this._rightController.Serialize(writer);
    }

    // Token: 0x06000572 RID: 1394 RVA: 0x0000ED07 File Offset: 0x0000CF07
    public void Deserialize(NetDataReader reader)
    {
        this._head.Deserialize(reader);
        this._leftController.Deserialize(reader);
        this._rightController.Deserialize(reader);
    }

    // Token: 0x06000573 RID: 1395 RVA: 0x0000ED2D File Offset: 0x0000CF2D
    public void SetState(NodePoseSyncState.NodePose nodePose, PoseSerializable pose)
    {
        switch (nodePose)
        {
            case NodePoseSyncState.NodePose.Head:
                this._head = pose;
                return;
            case NodePoseSyncState.NodePose.LeftController:
                this._leftController = pose;
                return;
            case NodePoseSyncState.NodePose.RightController:
                this._rightController = pose;
                return;
            default:
                return;
        }
    }

    // Token: 0x06000574 RID: 1396 RVA: 0x0000ED5C File Offset: 0x0000CF5C
    public PoseSerializable GetState(NodePoseSyncState.NodePose nodePose)
    {
        switch (nodePose)
        {
            case NodePoseSyncState.NodePose.Head:
                return this._head;
            case NodePoseSyncState.NodePose.LeftController:
                return this._leftController;
            case NodePoseSyncState.NodePose.RightController:
                return this._rightController;
            default:
                return default(PoseSerializable);
        }
    }

    // Token: 0x06000575 RID: 1397 RVA: 0x0000ED9B File Offset: 0x0000CF9B
    public bool Equals(in NodePoseSyncState other)
    {
        return this._head.Equals(other._head) && this._leftController.Equals(other._leftController) && this._rightController.Equals(other._rightController);
    }

    // Token: 0x06000576 RID: 1398 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
    public NodePoseSyncState GetDelta(in NodePoseSyncState latest)
    {
        return new NodePoseSyncState
        {
            _head = latest._head - this._head,
            _leftController = latest._leftController - this._leftController,
            _rightController = latest._rightController - this._rightController
        };
    }

    // Token: 0x06000577 RID: 1399 RVA: 0x0000EE38 File Offset: 0x0000D038
    public NodePoseSyncState ApplyDelta(in NodePoseSyncState delta)
    {
        return new NodePoseSyncState
        {
            _head = delta._head + this._head,
            _leftController = delta._leftController + this._leftController,
            _rightController = delta._rightController + this._rightController
        };
    }

    // Token: 0x06000578 RID: 1400 RVA: 0x0000EE96 File Offset: 0x0000D096
    public int GetSize()
    {
        return this._head.GetSize() + this._leftController.GetSize() + this._rightController.GetSize();
    }

    // Token: 0x06000579 RID: 1401 RVA: 0x0000EEBB File Offset: 0x0000D0BB
    NodePoseSyncState IStateTable<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable>.GetDelta(in NodePoseSyncState stateTable)
    {
        return this.GetDelta(stateTable);
    }

    // Token: 0x0600057A RID: 1402 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
    NodePoseSyncState IStateTable<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable>.ApplyDelta(in NodePoseSyncState delta)
    {
        return this.ApplyDelta(delta);
    }

    // Token: 0x0600057B RID: 1403 RVA: 0x0000EECD File Offset: 0x0000D0CD
    bool IEquatableByReference<NodePoseSyncState>.Equals(in NodePoseSyncState other)
    {
        return this.Equals(other);
    }

    // Token: 0x04000220 RID: 544
    private PoseSerializable _head;

    // Token: 0x04000221 RID: 545
    private PoseSerializable _leftController;

    // Token: 0x04000222 RID: 546
    private PoseSerializable _rightController;

    // Token: 0x0200015C RID: 348
    public enum NodePose
    {
        // Token: 0x04000469 RID: 1129
        Head,
        // Token: 0x0400046A RID: 1130
        LeftController,
        // Token: 0x0400046B RID: 1131
        RightController,
        // Token: 0x0400046C RID: 1132
        Count
    }
}
