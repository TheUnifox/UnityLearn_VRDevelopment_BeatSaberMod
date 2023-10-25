using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public readonly struct NoteCutInfo
{
    // Token: 0x17000023 RID: 35
    // (get) Token: 0x060000AB RID: 171 RVA: 0x0000456C File Offset: 0x0000276C
    public bool allIsOK
    {
        get
        {
            return this.speedOK && this.directionOK && this.saberTypeOK && !this.wasCutTooSoon;
        }
    }

    // Token: 0x17000024 RID: 36
    // (get) Token: 0x060000AC RID: 172 RVA: 0x00004591 File Offset: 0x00002791
    public bool allExceptSaberTypeIsOK
    {
        get
        {
            return this.speedOK && this.directionOK && !this.wasCutTooSoon;
        }
    }

    // Token: 0x17000025 RID: 37
    // (get) Token: 0x060000AD RID: 173 RVA: 0x000045AE File Offset: 0x000027AE
    public NoteCutInfo.FailReason failReason
    {
        get
        {
            if (this.wasCutTooSoon)
            {
                return NoteCutInfo.FailReason.TooSoon;
            }
            if (!this.saberTypeOK)
            {
                return NoteCutInfo.FailReason.WrongColor;
            }
            if (!this.speedOK)
            {
                return NoteCutInfo.FailReason.CutHarder;
            }
            if (!this.directionOK)
            {
                return NoteCutInfo.FailReason.WrongDirection;
            }
            return NoteCutInfo.FailReason.None;
        }
    }

    // Token: 0x060000AE RID: 174 RVA: 0x000045DC File Offset: 0x000027DC
    public NoteCutInfo(NoteData noteData, bool speedOK, bool directionOK, bool saberTypeOK, bool wasCutTooSoon, float saberSpeed, Vector3 saberDir, SaberType saberType, float timeDeviation, float cutDirDeviation, Vector3 cutPoint, Vector3 cutNormal, float cutDistanceToCenter, float cutAngle, Quaternion worldRotation, Quaternion inverseWorldRotation, Quaternion noteRotation, Vector3 notePosition, ISaberMovementData saberMovementData)
    {
        this.noteData = noteData;
        this.speedOK = speedOK;
        this.directionOK = directionOK;
        this.saberTypeOK = saberTypeOK;
        this.wasCutTooSoon = wasCutTooSoon;
        this.saberSpeed = saberSpeed;
        this.saberDir = saberDir.normalized;
        this.saberType = saberType;
        this.cutPoint = cutPoint;
        this.cutNormal = cutNormal;
        this.cutDistanceToCenter = cutDistanceToCenter;
        this.cutAngle = cutAngle;
        this.timeDeviation = timeDeviation;
        this.cutDirDeviation = cutDirDeviation;
        this.worldRotation = worldRotation;
        this.inverseWorldRotation = inverseWorldRotation;
        this.noteRotation = noteRotation;
        this.notePosition = notePosition;
        this.saberMovementData = saberMovementData;
    }

    // Token: 0x04000085 RID: 133
    public readonly NoteData noteData;

    // Token: 0x04000086 RID: 134
    public readonly bool speedOK;

    // Token: 0x04000087 RID: 135
    public readonly bool directionOK;

    // Token: 0x04000088 RID: 136
    public readonly bool saberTypeOK;

    // Token: 0x04000089 RID: 137
    public readonly bool wasCutTooSoon;

    // Token: 0x0400008A RID: 138
    public readonly float saberSpeed;

    // Token: 0x0400008B RID: 139
    public readonly Vector3 saberDir;

    // Token: 0x0400008C RID: 140
    public readonly SaberType saberType;

    // Token: 0x0400008D RID: 141
    public readonly float timeDeviation;

    // Token: 0x0400008E RID: 142
    public readonly float cutDirDeviation;

    // Token: 0x0400008F RID: 143
    public readonly Vector3 cutPoint;

    // Token: 0x04000090 RID: 144
    public readonly Vector3 cutNormal;

    // Token: 0x04000091 RID: 145
    public readonly float cutAngle;

    // Token: 0x04000092 RID: 146
    public readonly float cutDistanceToCenter;

    // Token: 0x04000093 RID: 147
    public readonly Quaternion worldRotation;

    // Token: 0x04000094 RID: 148
    public readonly Quaternion inverseWorldRotation;

    // Token: 0x04000095 RID: 149
    public readonly Quaternion noteRotation;

    // Token: 0x04000096 RID: 150
    public readonly Vector3 notePosition;

    // Token: 0x04000097 RID: 151
    public readonly ISaberMovementData saberMovementData;

    // Token: 0x02000025 RID: 37
    public enum FailReason
    {
        // Token: 0x04000099 RID: 153
        None,
        // Token: 0x0400009A RID: 154
        TooSoon,
        // Token: 0x0400009B RID: 155
        WrongColor,
        // Token: 0x0400009C RID: 156
        CutHarder,
        // Token: 0x0400009D RID: 157
        WrongDirection
    }
}
