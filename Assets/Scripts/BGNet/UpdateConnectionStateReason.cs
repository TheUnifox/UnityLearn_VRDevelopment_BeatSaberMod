using System;

// Token: 0x02000093 RID: 147
public enum UpdateConnectionStateReason
{
    // Token: 0x0400024E RID: 590
    Init,
    // Token: 0x0400024F RID: 591
    PlayerOrderChanged,
    // Token: 0x04000250 RID: 592
    Connected,
    // Token: 0x04000251 RID: 593
    StartSession,
    // Token: 0x04000252 RID: 594
    StartDedicatedServer,
    // Token: 0x04000253 RID: 595
    SyncTimeInitialized,
    // Token: 0x04000254 RID: 596
    ManualDisconnect,
    // Token: 0x04000255 RID: 597
    RemoteDisconnect,
    // Token: 0x04000256 RID: 598
    ConnectionFailed,
    // Token: 0x04000257 RID: 599
    OnDestroy
}
