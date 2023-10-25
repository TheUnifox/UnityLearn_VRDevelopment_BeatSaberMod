using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class MockPlayerLobbyPoseGeneratorAI : MockPlayerLobbyPoseGenerator
{
    // Token: 0x0600006C RID: 108 RVA: 0x00003ABC File Offset: 0x00001CBC
    public MockPlayerLobbyPoseGeneratorAI(IMultiplayerSessionManager multiplayerSessionManager) : base(multiplayerSessionManager)
    {
    }

    // Token: 0x0600006D RID: 109 RVA: 0x00003AC8 File Offset: 0x00001CC8
    public override void Init()
    {
        this._headPose = new Pose(new Vector3(0f, 1.5f, 0f), Quaternion.identity);
        this._leftHandPose = new Pose(new Vector3(-0.2f, 1f, 0.2f), Quaternion.identity);
        this._rightHandPose = new Pose(new Vector3(0.2f, 1f, 0.2f), Quaternion.identity);
        this._random = new System.Random(Guid.NewGuid().GetHashCode());
        this._lastHeadPoseTarget = this._headPose;
        this._lastLeftHandPoseTarget = this._leftHandPose;
        this._lastRightHandPoseTarget = this._rightHandPose;
        this._lastTargetTime = 0f;
        this._headPoseTarget = this._headPose;
        this._leftHandPoseTarget = this._leftHandPose;
        this._rightHandPoseTarget = this._rightHandPose;
        this._nextTargetTime = this.multiplayerSessionManager.syncTime;
    }

    // Token: 0x0600006E RID: 110 RVA: 0x00003BC4 File Offset: 0x00001DC4
    public override void Tick()
    {
        if (this._nextTargetTime < this.multiplayerSessionManager.syncTime)
        {
            this._lastTargetTime = this._nextTargetTime;
            this._lastHeadPoseTarget = this._headPoseTarget;
            this._lastLeftHandPoseTarget = this._leftHandPoseTarget;
            this._lastRightHandPoseTarget = this._rightHandPoseTarget;
            this._nextTargetTime = this.multiplayerSessionManager.syncTime + 2f * (float)this._random.NextDouble();
            this._headPoseTarget.position.x = 0.5f * (float)this._random.NextDouble() - 0.25f;
            this._headPoseTarget.position.y = 0.25f * (float)this._random.NextDouble() + 1.25f;
            this._headPoseTarget.position.z = 0.5f * (float)this._random.NextDouble() - 0.25f;
            Vector3 euler = new Vector3
            {
                x = 30f * (float)this._random.NextDouble() - 15f,
                y = 60f * (float)this._random.NextDouble() - 30f,
                z = 20f * (float)this._random.NextDouble() - 10f
            };
            this._headPoseTarget.rotation = Quaternion.Euler(euler);
            this._leftHandPoseTarget.position.x = this._headPoseTarget.position.x + 0.5f * (float)this._random.NextDouble() - 0.5f;
            this._leftHandPoseTarget.position.y = 0.5f * (float)this._random.NextDouble() + 0.75f;
            this._leftHandPoseTarget.position.z = this._headPoseTarget.position.z + 0.5f * (float)this._random.NextDouble();
            euler.x = 180f * (float)this._random.NextDouble() - 90f;
            euler.y = 180f * (float)this._random.NextDouble() - 90f;
            euler.z = 90f * (float)this._random.NextDouble() - 45f;
            this._leftHandPoseTarget.rotation = Quaternion.Euler(euler);
            this._rightHandPoseTarget.position.x = this._headPoseTarget.position.x + 0.5f * (float)this._random.NextDouble();
            this._rightHandPoseTarget.position.y = 0.5f * (float)this._random.NextDouble() + 0.75f;
            this._rightHandPoseTarget.position.z = this._headPoseTarget.position.z + 0.5f * (float)this._random.NextDouble();
            euler.x = 180f * (float)this._random.NextDouble() - 90f;
            euler.y = 180f * (float)this._random.NextDouble() - 90f;
            euler.z = 90f * (float)this._random.NextDouble() - 45f;
            this._rightHandPoseTarget.rotation = Quaternion.Euler(euler);
        }
        Pose curr = PosePrediction.PredictPose(this._lastHeadPoseTarget, this._lastTargetTime, this._headPoseTarget, this._nextTargetTime, this.multiplayerSessionManager.syncTime);
        Pose curr2 = PosePrediction.PredictPose(this._lastLeftHandPoseTarget, this._lastTargetTime, this._leftHandPoseTarget, this._nextTargetTime, this.multiplayerSessionManager.syncTime);
        Pose curr3 = PosePrediction.PredictPose(this._lastRightHandPoseTarget, this._lastTargetTime, this._rightHandPoseTarget, this._nextTargetTime, this.multiplayerSessionManager.syncTime);
        this._headPose = PosePrediction.InterpolatePose(this._headPose, curr, 0.5f);
        this._leftHandPose = PosePrediction.InterpolatePose(this._leftHandPose, curr2, 0.5f);
        this._rightHandPose = PosePrediction.InterpolatePose(this._rightHandPose, curr3, 0.5f);
        this.mockNodePoseSyncStateSender.SendPose(this._headPose, this._leftHandPose, this._rightHandPose);
    }

    // Token: 0x0400004A RID: 74
    private System.Random _random;

    // Token: 0x0400004B RID: 75
    private Pose _headPose;

    // Token: 0x0400004C RID: 76
    private Pose _leftHandPose;

    // Token: 0x0400004D RID: 77
    private Pose _rightHandPose;

    // Token: 0x0400004E RID: 78
    private Pose _lastHeadPoseTarget;

    // Token: 0x0400004F RID: 79
    private Pose _lastLeftHandPoseTarget;

    // Token: 0x04000050 RID: 80
    private Pose _lastRightHandPoseTarget;

    // Token: 0x04000051 RID: 81
    private float _lastTargetTime;

    // Token: 0x04000052 RID: 82
    private Pose _headPoseTarget;

    // Token: 0x04000053 RID: 83
    private Pose _leftHandPoseTarget;

    // Token: 0x04000054 RID: 84
    private Pose _rightHandPoseTarget;

    // Token: 0x04000055 RID: 85
    private float _nextTargetTime;
}
