using System;

// Token: 0x02000005 RID: 5
public static class BeatmapDifficultySerializedMethods
{
    // Token: 0x06000007 RID: 7 RVA: 0x000020DE File Offset: 0x000002DE
    public static string SerializedName(this BeatmapDifficulty difficulty)
    {
        if (difficulty == BeatmapDifficulty.Easy)
        {
            return "Easy";
        }
        if (difficulty == BeatmapDifficulty.Normal)
        {
            return "Normal";
        }
        if (difficulty == BeatmapDifficulty.Hard)
        {
            return "Hard";
        }
        if (difficulty == BeatmapDifficulty.Expert)
        {
            return "Expert";
        }
        if (difficulty == BeatmapDifficulty.ExpertPlus)
        {
            return "ExpertPlus";
        }
        return "Unknown";
    }

    // Token: 0x06000008 RID: 8 RVA: 0x00002118 File Offset: 0x00000318
    public static bool BeatmapDifficultyFromSerializedName(this string name, out BeatmapDifficulty difficulty)
    {
        if (name == "Easy")
        {
            difficulty = BeatmapDifficulty.Easy;
            return true;
        }
        if (name == "Normal")
        {
            difficulty = BeatmapDifficulty.Normal;
            return true;
        }
        if (name == "Hard")
        {
            difficulty = BeatmapDifficulty.Hard;
            return true;
        }
        if (name == "Expert")
        {
            difficulty = BeatmapDifficulty.Expert;
            return true;
        }
        if (name == "Expert+" || name == "ExpertPlus")
        {
            difficulty = BeatmapDifficulty.ExpertPlus;
            return true;
        }
        difficulty = BeatmapDifficulty.Normal;
        return false;
    }

    // Token: 0x0400000E RID: 14
    private const string kDifficultyEasySerializedName = "Easy";

    // Token: 0x0400000F RID: 15
    private const string kDifficultyNormalSerializedName = "Normal";

    // Token: 0x04000010 RID: 16
    private const string kDifficultyHardSerializedName = "Hard";

    // Token: 0x04000011 RID: 17
    private const string kDifficultyExpertSerializedName = "Expert";

    // Token: 0x04000012 RID: 18
    private const string kDifficultyExpertPlusNameSerializedLegacy = "Expert+";

    // Token: 0x04000013 RID: 19
    private const string kDifficultyExpertPlusSerializedName = "ExpertPlus";

    // Token: 0x04000014 RID: 20
    private const string kDifficultyUnknownSerializedName = "Unknown";
}
