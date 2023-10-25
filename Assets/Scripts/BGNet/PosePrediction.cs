using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class PosePrediction
{
    // Token: 0x06000592 RID: 1426 RVA: 0x0000F053 File Offset: 0x0000D253
    public static Pose PredictPose(Pose prev, float prevTime, Pose curr, float currTime, float time)
    {
        if (currTime <= prevTime)
        {
            return curr;
        }
        if (time > currTime)
        {
            time = currTime + Mathf.Log(time - currTime + 1f);
        }
        return PosePrediction.InterpolatePose(prev, curr, (time - prevTime) / (currTime - prevTime));
    }

    // Token: 0x06000593 RID: 1427 RVA: 0x0000F084 File Offset: 0x0000D284
    public static Pose InterpolatePose(Pose prev, Pose curr, float t)
    {
        Quaternion rotation = Quaternion.SlerpUnclamped(prev.rotation, curr.rotation, t);
        Vector3 position = Vector3.LerpUnclamped(prev.position, curr.position, t);
        Vector3 forward = prev.forward;
        Vector3 forward2 = curr.forward;
        Vector3 rhs = prev.position - curr.position;
        float num = Vector3.Dot(forward, forward);
        float num2 = Vector3.Dot(forward, forward2);
        float num3 = Vector3.Dot(forward2, forward2);
        float num4 = Vector3.Dot(forward, rhs);
        float num5 = Vector3.Dot(forward2, rhs);
        float num6 = num * num3 - num2 * num2;
        if (num6 > 0.01f)
        {
            float num7 = (num2 * num5 - num3 * num4) / num6;
            float num8 = (num * num5 - num2 * num4) / num6;
            if (Mathf.Abs(num7) < 1f && Mathf.Abs(num8) < 1f && Mathf.Abs(num7 - num8) < 0.5f)
            {
                Vector3 a = prev.position + forward * num7;
                Vector3 b = curr.position + forward2 * num8;
                position = Vector3.LerpUnclamped(a, b, t) - rotation * Vector3.forward * Mathf.LerpUnclamped(num7, num8, t);
            }
        }
        return new Pose(position, rotation);
    }

    // Token: 0x06000594 RID: 1428 RVA: 0x0000F1C6 File Offset: 0x0000D3C6
    public static PoseSerializable PredictPoseSerializable(PoseSerializable prev, float prevTime, PoseSerializable curr, float currTime, float time)
    {
        return PosePrediction.PredictPose(prev, prevTime, curr, currTime, time);
    }

    // Token: 0x06000595 RID: 1429 RVA: 0x0000F1E2 File Offset: 0x0000D3E2
    public static PoseSerializable InterpolatePoseSerializable(PoseSerializable a, PoseSerializable b, float t)
    {
        return PosePrediction.InterpolatePose(a, b, t);
    }
}
