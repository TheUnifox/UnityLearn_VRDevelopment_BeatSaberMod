using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public abstract class RankModel
{
    // Token: 0x060000EF RID: 239 RVA: 0x000053E4 File Offset: 0x000035E4
    public static string GetRankName(RankModel.Rank rank)
    {
        switch (rank)
        {
            case RankModel.Rank.E:
                return "E";
            case RankModel.Rank.D:
                return "D";
            case RankModel.Rank.C:
                return "C";
            case RankModel.Rank.B:
                return "B";
            case RankModel.Rank.A:
                return "A";
            case RankModel.Rank.S:
                return "S";
            case RankModel.Rank.SS:
                return "SS";
            case RankModel.Rank.SSS:
                return "SSS";
            default:
                return "XXX";
        }
    }

    // Token: 0x060000F0 RID: 240 RVA: 0x00005450 File Offset: 0x00003650
    public static RankModel.Rank GetRankForScore(int multipliedScore, int modifiedScore, int maxMultipliedScore, int maxModifiedScore)
    {
        if (maxModifiedScore <= 0)
        {
            return RankModel.Rank.E;
        }
        if (maxMultipliedScore == 0)
        {
            return RankModel.Rank.SS;
        }
        int num = Mathf.Max(maxMultipliedScore, maxModifiedScore);
        float num2 = (float)modifiedScore / (float)num;
        if (multipliedScore == maxMultipliedScore && modifiedScore >= multipliedScore)
        {
            return RankModel.Rank.SSS;
        }
        if (num2 > 0.9f)
        {
            return RankModel.Rank.SS;
        }
        if (num2 > 0.8f)
        {
            return RankModel.Rank.S;
        }
        if (num2 > 0.65f)
        {
            return RankModel.Rank.A;
        }
        if (num2 > 0.5f)
        {
            return RankModel.Rank.B;
        }
        if (num2 > 0.35f)
        {
            return RankModel.Rank.C;
        }
        if (num2 > 0.2f)
        {
            return RankModel.Rank.D;
        }
        return RankModel.Rank.E;
    }

    // Token: 0x02000030 RID: 48
    public enum Rank
    {
        // Token: 0x040000E2 RID: 226
        E,
        // Token: 0x040000E3 RID: 227
        D,
        // Token: 0x040000E4 RID: 228
        C,
        // Token: 0x040000E5 RID: 229
        B,
        // Token: 0x040000E6 RID: 230
        A,
        // Token: 0x040000E7 RID: 231
        S,
        // Token: 0x040000E8 RID: 232
        SS,
        // Token: 0x040000E9 RID: 233
        SSS
    }
}
