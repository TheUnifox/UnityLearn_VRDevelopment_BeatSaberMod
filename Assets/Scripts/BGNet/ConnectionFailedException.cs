using System;

// Token: 0x02000010 RID: 16
public class ConnectionFailedException : Exception
{
    // Token: 0x06000099 RID: 153 RVA: 0x0000444D File Offset: 0x0000264D
    public ConnectionFailedException(ConnectionFailedReason reason)
    {
        this.reason = reason;
    }

    // Token: 0x0600009A RID: 154 RVA: 0x0000445C File Offset: 0x0000265C
    public ConnectionFailedException(ConnectionFailedReason reason, string message) : base(message)
    {
        this.reason = reason;
    }

    // Token: 0x0400004C RID: 76
    public readonly ConnectionFailedReason reason;
}
