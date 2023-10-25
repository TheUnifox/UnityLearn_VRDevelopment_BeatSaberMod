using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public abstract class NoteBasicCutInfoHelper
{
    // Token: 0x060000AA RID: 170 RVA: 0x0000448C File Offset: 0x0000268C
    public static void GetBasicCutInfo(Transform noteTransform, ColorType colorType, NoteCutDirection cutDirection, SaberType saberType, float saberBladeSpeed, Vector3 cutDirVec, float cutAngleTolerance, out bool directionOK, out bool speedOK, out bool saberTypeOK, out float cutDirDeviation, out float cutDirAngle)
    {
        cutDirVec = noteTransform.InverseTransformVector(cutDirVec);
        bool flag = Mathf.Abs(cutDirVec.z) > Mathf.Abs(cutDirVec.x) * 10f && Mathf.Abs(cutDirVec.z) > Mathf.Abs(cutDirVec.y) * 10f;
        cutDirAngle = Mathf.Atan2(cutDirVec.y, cutDirVec.x) * 57.29578f;
        directionOK = ((!flag && cutDirAngle > -90f - cutAngleTolerance && cutDirAngle < -90f + cutAngleTolerance) || cutDirection == NoteCutDirection.Any);
        speedOK = (saberBladeSpeed > 2f);
        saberTypeOK = saberType.MatchesColorType(colorType);
        cutDirDeviation = (flag ? 90f : (cutDirAngle + 90f));
        if (cutDirDeviation > 180f)
        {
            cutDirDeviation -= 360f;
        }
    }

    // Token: 0x04000084 RID: 132
    private const float kMinBladeSpeedForCut = 2f;
}
