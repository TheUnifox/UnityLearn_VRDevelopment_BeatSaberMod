using System;

// Token: 0x02000084 RID: 132
public interface ISyncStateSerializable<T>
{
    // Token: 0x170000DF RID: 223
    // (get) Token: 0x0600056B RID: 1387
    // (set) Token: 0x0600056C RID: 1388
    SyncStateId id { get; set; }

    // Token: 0x170000E0 RID: 224
    // (get) Token: 0x0600056D RID: 1389
    // (set) Token: 0x0600056E RID: 1390
    float time { get; set; }

    // Token: 0x170000E1 RID: 225
    // (get) Token: 0x0600056F RID: 1391
    // (set) Token: 0x06000570 RID: 1392
    T state { get; set; }
}
