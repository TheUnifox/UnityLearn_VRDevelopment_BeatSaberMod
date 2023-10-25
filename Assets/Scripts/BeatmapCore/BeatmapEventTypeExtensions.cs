using System;

// Token: 0x0200000D RID: 13
public static class BeatmapEventTypeExtensions
{
    // Token: 0x06000024 RID: 36 RVA: 0x0000248E File Offset: 0x0000068E
    public static bool IsCoreLightIntensityChangeEvent(this BasicBeatmapEventType basicBeatmapEventType)
    {
        return basicBeatmapEventType == BasicBeatmapEventType.Event0 || basicBeatmapEventType == BasicBeatmapEventType.Event1 || basicBeatmapEventType == BasicBeatmapEventType.Event2 || basicBeatmapEventType == BasicBeatmapEventType.Event3 || basicBeatmapEventType == BasicBeatmapEventType.Event4;
    }

    // Token: 0x0400004D RID: 77
    public const BasicBeatmapEventType kLights1 = BasicBeatmapEventType.Event0;

    // Token: 0x0400004E RID: 78
    public const BasicBeatmapEventType kLights2 = BasicBeatmapEventType.Event1;

    // Token: 0x0400004F RID: 79
    public const BasicBeatmapEventType kLights3 = BasicBeatmapEventType.Event2;

    // Token: 0x04000050 RID: 80
    public const BasicBeatmapEventType kLights4 = BasicBeatmapEventType.Event3;

    // Token: 0x04000051 RID: 81
    public const BasicBeatmapEventType kLights5 = BasicBeatmapEventType.Event4;

    // Token: 0x04000052 RID: 82
    public const BasicBeatmapEventType kColorBoost = BasicBeatmapEventType.Event5;

    // Token: 0x04000053 RID: 83
    public const BasicBeatmapEventType kRotateRings = BasicBeatmapEventType.Event8;

    // Token: 0x04000054 RID: 84
    public const BasicBeatmapEventType kCompressExpand = BasicBeatmapEventType.Event9;

    // Token: 0x04000055 RID: 85
    public const BasicBeatmapEventType kLegacyEarlySpawnRotation = BasicBeatmapEventType.Event14;

    // Token: 0x04000056 RID: 86
    public const BasicBeatmapEventType kLegacyLateSpawnRotation = BasicBeatmapEventType.Event15;
}
