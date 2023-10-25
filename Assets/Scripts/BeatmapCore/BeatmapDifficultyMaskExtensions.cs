using System;

// Token: 0x02000007 RID: 7
public static class BeatmapDifficultyMaskExtensions
{
    // Token: 0x06000009 RID: 9 RVA: 0x00002190 File Offset: 0x00000390
    public static BeatmapDifficultyMask ToMask(this BeatmapDifficulty difficulty)
    {
        return (BeatmapDifficultyMask)(1 << (int)difficulty);
    }

    // Token: 0x0600000A RID: 10 RVA: 0x00002199 File Offset: 0x00000399
    public static BeatmapDifficulty FromMask(this BeatmapDifficultyMask mask)
    {
        switch (mask)
        {
            case BeatmapDifficultyMask.Easy:
                return BeatmapDifficulty.Easy;
            case BeatmapDifficultyMask.Normal:
                return BeatmapDifficulty.Normal;
            case BeatmapDifficultyMask.Easy | BeatmapDifficultyMask.Normal:
                break;
            case BeatmapDifficultyMask.Hard:
                return BeatmapDifficulty.Hard;
            default:
                if (mask == BeatmapDifficultyMask.Expert)
                {
                    return BeatmapDifficulty.Expert;
                }
                if (mask == BeatmapDifficultyMask.ExpertPlus)
                {
                    return BeatmapDifficulty.ExpertPlus;
                }
                break;
        }
        return BeatmapDifficulty.Hard;
    }

    // Token: 0x0600000B RID: 11 RVA: 0x000021CC File Offset: 0x000003CC
    public static string LocalizedKey(this BeatmapDifficultyMask mask)
    {
        if (mask <= BeatmapDifficultyMask.Expert)
        {
            switch (mask)
            {
                case BeatmapDifficultyMask.Easy:
                    return "DIFFICULTY_EASY";
                case BeatmapDifficultyMask.Normal:
                    return "DIFFICULTY_NORMAL";
                case BeatmapDifficultyMask.Easy | BeatmapDifficultyMask.Normal:
                    break;
                case BeatmapDifficultyMask.Hard:
                    return "DIFFICULTY_HARD";
                default:
                    if (mask == BeatmapDifficultyMask.Expert)
                    {
                        return "DIFFICULTY_EXPERT";
                    }
                    break;
            }
        }
        else
        {
            if (mask == BeatmapDifficultyMask.ExpertPlus)
            {
                return "DIFFICULTY_EXPERT_PLUS";
            }
            if (mask == BeatmapDifficultyMask.All)
            {
                return "BEATMAP_DIFFICULTY_ALL";
            }
        }
        return "DIFFICULTY_HARD";
    }

    // Token: 0x0600000C RID: 12 RVA: 0x00002230 File Offset: 0x00000430
    public static string ShortLocalizedKey(this BeatmapDifficultyMask mask)
    {
        if (mask <= BeatmapDifficultyMask.Expert)
        {
            switch (mask)
            {
                case BeatmapDifficultyMask.Easy:
                    return "DIFFICULTY_EASY_SHORT";
                case BeatmapDifficultyMask.Normal:
                    return "DIFFICULTY_NORMAL_SHORT";
                case BeatmapDifficultyMask.Easy | BeatmapDifficultyMask.Normal:
                    break;
                case BeatmapDifficultyMask.Hard:
                    return "DIFFICULTY_HARD_SHORT";
                default:
                    if (mask == BeatmapDifficultyMask.Expert)
                    {
                        return "DIFFICULTY_EXPERT_SHORT";
                    }
                    break;
            }
        }
        else
        {
            if (mask == BeatmapDifficultyMask.ExpertPlus)
            {
                return "DIFFICULTY_EXPERT_PLUS_SHORT";
            }
            if (mask == BeatmapDifficultyMask.All)
            {
                return "BEATMAP_DIFFICULTY_ALL";
            }
        }
        return "DIFFICULTY_HARD_SHORT";
    }

    // Token: 0x0600000D RID: 13 RVA: 0x00002294 File Offset: 0x00000494
    public static bool Contains(this BeatmapDifficultyMask mask, BeatmapDifficulty difficulty)
    {
        return mask.Contains(difficulty.ToMask());
    }

    // Token: 0x0600000E RID: 14 RVA: 0x000022A2 File Offset: 0x000004A2
    public static bool Contains(this BeatmapDifficultyMask mask, BeatmapDifficultyMask other)
    {
        return (mask & other) == other;
    }

    // Token: 0x0600000F RID: 15 RVA: 0x000022AA File Offset: 0x000004AA
    public static int DifferenceFrom(this BeatmapDifficultyMask mask, BeatmapDifficultyMask other)
    {
        return (int)BitMaskUtil.NumberOfSetBits((uint)(mask & other));
    }

    // Token: 0x06000010 RID: 16 RVA: 0x000022B4 File Offset: 0x000004B4
    public static string ToHexString(this BeatmapDifficultyMask mask)
    {
        byte b = (byte)mask;
        return b.ToString("x2");
    }
}
