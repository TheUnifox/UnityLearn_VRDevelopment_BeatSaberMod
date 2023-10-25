using System;
using UnityEngine.XR;

// Token: 0x02000032 RID: 50
public static class SaberTypeExtensions
{
    // Token: 0x060000F1 RID: 241 RVA: 0x000054BD File Offset: 0x000036BD
    public static bool MatchesColorType(this SaberType saberType, ColorType colorType)
    {
        return (saberType == SaberType.SaberA && colorType == ColorType.ColorA) || (saberType == SaberType.SaberB && colorType == ColorType.ColorB);
    }

    // Token: 0x060000F2 RID: 242 RVA: 0x000054D1 File Offset: 0x000036D1
    public static XRNode Node(this SaberType saberType)
    {
        if (saberType != SaberType.SaberA)
        {
            return XRNode.RightHand;
        }
        return XRNode.LeftHand;
    }

    // Token: 0x060000F3 RID: 243 RVA: 0x000054D9 File Offset: 0x000036D9
    public static SaberType MainSaber(bool leftHanded)
    {
        if (!leftHanded)
        {
            return SaberType.SaberB;
        }
        return SaberType.SaberA;
    }

    // Token: 0x060000F4 RID: 244 RVA: 0x000054E1 File Offset: 0x000036E1
    public static SaberType ToSaberType(this ColorType colorType)
    {
        if (colorType == ColorType.ColorA)
        {
            return SaberType.SaberA;
        }
        if (colorType != ColorType.ColorB)
        {
            throw new ArgumentOutOfRangeException("colorType", colorType, null);
        }
        return SaberType.SaberB;
    }
}
