using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class MathfExtra
{
    // Token: 0x0600000A RID: 10 RVA: 0x000021C7 File Offset: 0x000003C7
    public static float Mod(float value, float mod)
    {
        return value - mod * Mathf.Floor(value / mod);
    }

    // Token: 0x0600000B RID: 11 RVA: 0x000021D8 File Offset: 0x000003D8
    public static float Round(float value, int decimals)
    {
        int num = (int)Mathf.Pow(10f, (float)decimals);
        return (float)Mathf.RoundToInt(value * (float)num) / (float)num;
    }

    // Token: 0x0600000C RID: 12 RVA: 0x00002100 File Offset: 0x00000300
    public MathfExtra()
    {
    }
}
