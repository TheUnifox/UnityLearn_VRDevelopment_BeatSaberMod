using System;

// Token: 0x02000002 RID: 2
public abstract class BeatmapDataItem : IComparable<BeatmapDataItem>
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    public float time { get; }

    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
    public int executionOrder { get; }

    // Token: 0x17000003 RID: 3
    // (get) Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
    public virtual int subtypeGroupIdentifier
    {
        get
        {
            return this.subtypeIdentifier;
        }
    }

    // Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
    protected BeatmapDataItem(float time, int executionOrder, int subtypeIdentifier, BeatmapDataItem.BeatmapDataItemType type)
    {
        this.time = time;
        this.executionOrder = executionOrder;
        this.subtypeIdentifier = subtypeIdentifier;
        this.type = type;
    }

    // Token: 0x06000005 RID: 5
    public abstract BeatmapDataItem GetCopy();

    // Token: 0x06000006 RID: 6 RVA: 0x00002090 File Offset: 0x00000290
    public int CompareTo(BeatmapDataItem b)
    {
        if (this.time < b.time)
        {
            return -1;
        }
        if (this.time > b.time)
        {
            return 1;
        }
        if (this.executionOrder < b.executionOrder)
        {
            return -1;
        }
        if (this.executionOrder > b.executionOrder)
        {
            return 1;
        }
        return 0;
    }

    // Token: 0x04000003 RID: 3
    public readonly int subtypeIdentifier;

    // Token: 0x04000004 RID: 4
    public readonly BeatmapDataItem.BeatmapDataItemType type;

    // Token: 0x02000003 RID: 3
    public enum BeatmapDataItemType
    {
        // Token: 0x04000006 RID: 6
        BeatmapObject,
        // Token: 0x04000007 RID: 7
        BeatmapEvent
    }
}
