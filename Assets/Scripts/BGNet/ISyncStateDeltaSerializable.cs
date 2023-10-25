using System;

// Token: 0x02000083 RID: 131
public interface ISyncStateDeltaSerializable<T>
{
    // Token: 0x170000DC RID: 220
    // (get) Token: 0x06000565 RID: 1381
    // (set) Token: 0x06000566 RID: 1382
    SyncStateId baseId { get; set; }

    // Token: 0x170000DD RID: 221
    // (get) Token: 0x06000567 RID: 1383
    // (set) Token: 0x06000568 RID: 1384
    int timeOffsetMs { get; set; }

    // Token: 0x170000DE RID: 222
    // (get) Token: 0x06000569 RID: 1385
    // (set) Token: 0x0600056A RID: 1386
    T delta { get; set; }
}
