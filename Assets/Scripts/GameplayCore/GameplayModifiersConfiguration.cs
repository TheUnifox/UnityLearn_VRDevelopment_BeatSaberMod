using System;

// Token: 0x02000011 RID: 17
public abstract class GameplayModifiersConfiguration
{
    // Token: 0x02000012 RID: 18
    public static class SongSpeed
    {
        // Token: 0x04000047 RID: 71
        public const float kSlower = 0.85f;

        // Token: 0x04000048 RID: 72
        public const float kNormal = 1f;

        // Token: 0x04000049 RID: 73
        public const float kFaster = 1.2f;

        // Token: 0x0400004A RID: 74
        public const float kSuperFast = 1.5f;
    }

    // Token: 0x02000013 RID: 19
    public static class CutAngleTolerance
    {
        // Token: 0x0400004B RID: 75
        public const float kDefault = 60f;

        // Token: 0x0400004C RID: 76
        public const float kStrict = 40f;
    }

    // Token: 0x02000014 RID: 20
    public static class NoteUniformScale
    {
        // Token: 0x0400004D RID: 77
        public const float kDefault = 1f;

        // Token: 0x0400004E RID: 78
        public const float kSmall = 0.5f;
    }
}
