using System;

// Token: 0x0200000F RID: 15
public abstract class BeatmapEventData : BeatmapDataItem
{
    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000026 RID: 38 RVA: 0x000024AD File Offset: 0x000006AD
    // (set) Token: 0x06000027 RID: 39 RVA: 0x000024B5 File Offset: 0x000006B5
    public BeatmapEventData previousSameTypeEventData { get; private set; }

    // Token: 0x17000008 RID: 8
    // (get) Token: 0x06000028 RID: 40 RVA: 0x000024BE File Offset: 0x000006BE
    // (set) Token: 0x06000029 RID: 41 RVA: 0x000024C6 File Offset: 0x000006C6
    public BeatmapEventData nextSameTypeEventData { get; private set; }

    // Token: 0x0600002A RID: 42 RVA: 0x000024CF File Offset: 0x000006CF
    protected BeatmapEventData(float time, int executionOrder, int subtypeIdentifier) : base(time, executionOrder, subtypeIdentifier, BeatmapDataItem.BeatmapDataItemType.BeatmapEvent)
    {
    }

    // Token: 0x0600002B RID: 43 RVA: 0x000024DB File Offset: 0x000006DB
    public void __ConnectWithPreviousSameTypeEventData(BeatmapEventData newPreviousSameTypeEvent)
    {
        this.previousSameTypeEventData = newPreviousSameTypeEvent;
        if (this.previousSameTypeEventData != null)
        {
            this.previousSameTypeEventData.nextSameTypeEventData = this;
        }
    }

    // Token: 0x0600002C RID: 44 RVA: 0x000024F8 File Offset: 0x000006F8
    public void __ConnectWithNextSameTypeEventData(BeatmapEventData newNextSameTypeEvent)
    {
        this.nextSameTypeEventData = newNextSameTypeEvent;
        if (this.nextSameTypeEventData != null)
        {
            this.nextSameTypeEventData.previousSameTypeEventData = this;
        }
    }

    // Token: 0x0600002D RID: 45 RVA: 0x00002515 File Offset: 0x00000715
    public void __ResetConnections()
    {
        this.previousSameTypeEventData = null;
        this.nextSameTypeEventData = null;
    }

    // Token: 0x0600002E RID: 46
    protected abstract BeatmapEventData GetDefault();

    // Token: 0x0600002F RID: 47 RVA: 0x00002525 File Offset: 0x00000725
    public BeatmapEventData GetDefault(BeatmapEventData nextData)
    {
        BeatmapEventData @default = this.GetDefault();
        @default.nextSameTypeEventData = nextData;
        return @default;
    }
}
