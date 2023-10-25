using System;

// Token: 0x02000011 RID: 17
public enum ConnectionFailedReason
{
    // Token: 0x0400004E RID: 78
    None,
    // Token: 0x0400004F RID: 79
    Unknown,
    // Token: 0x04000050 RID: 80
    ConnectionCanceled,
    // Token: 0x04000051 RID: 81
    ServerUnreachable,
    // Token: 0x04000052 RID: 82
    ServerAlreadyExists,
    // Token: 0x04000053 RID: 83
    ServerDoesNotExist,
    // Token: 0x04000054 RID: 84
    ServerAtCapacity,
    // Token: 0x04000055 RID: 85
    VersionMismatch,
    // Token: 0x04000056 RID: 86
    InvalidPassword,
    // Token: 0x04000057 RID: 87
    MultiplayerApiUnreachable,
    // Token: 0x04000058 RID: 88
    AuthenticationFailed,
    // Token: 0x04000059 RID: 89
    NetworkNotConnected,
    // Token: 0x0400005A RID: 90
    CertificateValidationFailed,
    // Token: 0x0400005B RID: 91
    ServerIsTerminating,
    // Token: 0x0400005C RID: 92
    Timeout,
    // Token: 0x0400005D RID: 93
    FailedToFindMatch
}
