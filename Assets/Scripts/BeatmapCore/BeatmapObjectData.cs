using System;

// Token: 0x02000022 RID: 34
public abstract class BeatmapObjectData : BeatmapDataItem
{
    // Token: 0x06000079 RID: 121 RVA: 0x00003027 File Offset: 0x00001227
    protected BeatmapObjectData(float time, int subtypeIdentifier) : base(time, 100, subtypeIdentifier, BeatmapDataItem.BeatmapDataItemType.BeatmapObject)
    {
    }

    // Token: 0x0600007A RID: 122
    public abstract void Mirror(int lineCount);
}
