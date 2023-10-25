using System;

// Token: 0x02000008 RID: 8
public class BPMChangeBeatmapEventData : BeatmapEventData
{
    // Token: 0x06000011 RID: 17 RVA: 0x000022CF File Offset: 0x000004CF
    public BPMChangeBeatmapEventData(float time, float bpm) : base(time, -1001, 0)
    {
        this.bpm = bpm;
    }

    // Token: 0x06000012 RID: 18 RVA: 0x000022E5 File Offset: 0x000004E5
    public override BeatmapDataItem GetCopy()
    {
        return new BPMChangeBeatmapEventData(base.time, this.bpm);
    }

    // Token: 0x06000013 RID: 19 RVA: 0x000022F8 File Offset: 0x000004F8
    protected override BeatmapEventData GetDefault()
    {
        return null;
    }

    // Token: 0x0400001C RID: 28
    public readonly float bpm;
}
