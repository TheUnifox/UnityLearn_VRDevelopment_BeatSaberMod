using System;

// Token: 0x02000011 RID: 17
public class ColorBoostBeatmapEventData : BeatmapEventData
{
    // Token: 0x06000030 RID: 48 RVA: 0x00002534 File Offset: 0x00000734
    public ColorBoostBeatmapEventData(float time, bool boostColorsAreOn) : base(time, -100, 0)
    {
        this.boostColorsAreOn = boostColorsAreOn;
    }

    // Token: 0x06000031 RID: 49 RVA: 0x00002547 File Offset: 0x00000747
    public override BeatmapDataItem GetCopy()
    {
        return new ColorBoostBeatmapEventData(base.time, this.boostColorsAreOn);
    }

    // Token: 0x06000032 RID: 50 RVA: 0x0000255A File Offset: 0x0000075A
    protected override BeatmapEventData GetDefault()
    {
        return ColorBoostBeatmapEventData._defaultCopy;
    }

    // Token: 0x04000062 RID: 98
    public readonly bool boostColorsAreOn;

    // Token: 0x04000063 RID: 99
    [DoesNotRequireDomainReloadInit]
    protected static readonly ColorBoostBeatmapEventData _defaultCopy = new ColorBoostBeatmapEventData(0f, false);
}
