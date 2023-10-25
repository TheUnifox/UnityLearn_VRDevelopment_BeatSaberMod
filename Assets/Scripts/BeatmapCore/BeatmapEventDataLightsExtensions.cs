using System;

// Token: 0x0200000A RID: 10
public static class BeatmapEventDataLightsExtensions
{
    // Token: 0x0600001F RID: 31 RVA: 0x000023E0 File Offset: 0x000005E0
    public static EnvironmentColorType LightColorTypeFromEventDataValue(this BasicBeatmapEventData basicBeatmapEventData)
    {
        return BeatmapEventDataLightsExtensions.GetLightColorTypeFromEventDataValue(basicBeatmapEventData.value);
    }

    // Token: 0x06000020 RID: 32 RVA: 0x000023F0 File Offset: 0x000005F0
    public static EnvironmentColorType GetLightColorTypeFromEventDataValue(int beatmapEventValue)
    {
        if (beatmapEventValue == 1 || beatmapEventValue == 2 || beatmapEventValue == 3 || beatmapEventValue == 4 || beatmapEventValue == 0 || beatmapEventValue == -1)
        {
            return EnvironmentColorType.Color0;
        }
        if (beatmapEventValue == 5 || beatmapEventValue == 6 || beatmapEventValue == 7 || beatmapEventValue == 8)
        {
            return EnvironmentColorType.Color1;
        }
        if (beatmapEventValue == 9 || beatmapEventValue == 10 || beatmapEventValue == 11 || beatmapEventValue == 12)
        {
            return EnvironmentColorType.ColorW;
        }
        return EnvironmentColorType.Color0;
    }

    // Token: 0x06000021 RID: 33 RVA: 0x0000243F File Offset: 0x0000063F
    public static bool HasLightFadeEventDataValue(this BasicBeatmapEventData basicBeatmapEventData)
    {
        return basicBeatmapEventData.value == 4 || basicBeatmapEventData.value == 8 || basicBeatmapEventData.value == 12;
    }

    // Token: 0x06000022 RID: 34 RVA: 0x0000245F File Offset: 0x0000065F
    public static bool HasFixedDurationLightSwitchEventDataValue(this BasicBeatmapEventData basicBeatmapEventData)
    {
        return BeatmapEventDataLightsExtensions.HasFixedDurationLightSwitchEventDataValue(basicBeatmapEventData.value);
    }

    // Token: 0x06000023 RID: 35 RVA: 0x0000246C File Offset: 0x0000066C
    private static bool HasFixedDurationLightSwitchEventDataValue(int beatmapEventValue)
    {
        return beatmapEventValue == 2 || beatmapEventValue == 6 || beatmapEventValue == 10 || beatmapEventValue == 3 || beatmapEventValue == 7 || beatmapEventValue == 11 || beatmapEventValue == -1;
    }

    // Token: 0x0200000B RID: 11
    public static class LightSwitchEventEffectDataValues
    {
        // Token: 0x04000022 RID: 34
        public const int kOff = 0;

        // Token: 0x04000023 RID: 35
        public const int kOnA = 1;

        // Token: 0x04000024 RID: 36
        public const int kOnB = 5;

        // Token: 0x04000025 RID: 37
        public const int kOnW = 9;

        // Token: 0x04000026 RID: 38
        public const int kHighlightA = 2;

        // Token: 0x04000027 RID: 39
        public const int kHighlightB = 6;

        // Token: 0x04000028 RID: 40
        public const int kHighlightW = 10;

        // Token: 0x04000029 RID: 41
        public const int kFlashAndFadeToBlack = -1;

        // Token: 0x0400002A RID: 42
        public const int kFlashAndFadeToBlackA = 3;

        // Token: 0x0400002B RID: 43
        public const int kFlashAndFadeToBlackB = 7;

        // Token: 0x0400002C RID: 44
        public const int kFlashAndFadeToBlackW = 11;

        // Token: 0x0400002D RID: 45
        public const int kFadeA = 4;

        // Token: 0x0400002E RID: 46
        public const int kFadeB = 8;

        // Token: 0x0400002F RID: 47
        public const int kFadeW = 12;
    }
}
