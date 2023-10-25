using System;

// Token: 0x02000006 RID: 6
[Flags]
public enum BeatmapDifficultyMask : byte
{
    // Token: 0x04000016 RID: 22
    Easy = 1,
    // Token: 0x04000017 RID: 23
    Normal = 2,
    // Token: 0x04000018 RID: 24
    Hard = 4,
    // Token: 0x04000019 RID: 25
    Expert = 8,
    // Token: 0x0400001A RID: 26
    ExpertPlus = 16,
    // Token: 0x0400001B RID: 27
    All = 31
}
