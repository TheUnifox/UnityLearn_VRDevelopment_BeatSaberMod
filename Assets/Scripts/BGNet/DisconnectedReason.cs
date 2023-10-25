using System;

// Token: 0x02000016 RID: 22
public enum DisconnectedReason
{
    // Token: 0x04000073 RID: 115
    Unknown = 1,
    // Token: 0x04000074 RID: 116
    UserInitiated,
    // Token: 0x04000075 RID: 117
    Timeout,
    // Token: 0x04000076 RID: 118
    Kicked,
    // Token: 0x04000077 RID: 119
    ServerAtCapacity,
    // Token: 0x04000078 RID: 120
    ServerConnectionClosed,
    // Token: 0x04000079 RID: 121
    MasterServerUnreachable,
    // Token: 0x0400007A RID: 122
    ClientConnectionClosed,
    // Token: 0x0400007B RID: 123
    NetworkDisconnected,
    // Token: 0x0400007C RID: 124
    ServerTerminated
}
