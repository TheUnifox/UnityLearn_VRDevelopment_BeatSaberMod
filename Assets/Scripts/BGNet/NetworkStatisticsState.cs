using System;

// Token: 0x02000066 RID: 102
public readonly struct NetworkStatisticsState
{
    // Token: 0x06000486 RID: 1158 RVA: 0x0000BA0C File Offset: 0x00009C0C
    public NetworkStatisticsState(long packetsSent, long packetsReceived, long bytesSent, long bytesReceived, long packetsLost, long packetsSentEncrypted, long packetsSentPlaintext, long packetsSentRejected, long packetsReceivedEncrypted, long packetsReceivedPlaintext, long packetsReceivedRejected, long encryptionProcessingTime, long decryptionProcessingTime)
    {
        this.packetsSent = packetsSent;
        this.packetsReceived = packetsReceived;
        this.bytesSent = bytesSent;
        this.bytesReceived = bytesReceived;
        this.packetsLost = packetsLost;
        this.packetsSentEncrypted = packetsSentEncrypted;
        this.packetsSentPlaintext = packetsSentPlaintext;
        this.packetsSentRejected = packetsSentRejected;
        this.packetsReceivedEncrypted = packetsReceivedEncrypted;
        this.packetsReceivedPlaintext = packetsReceivedPlaintext;
        this.packetsReceivedRejected = packetsReceivedRejected;
        this.encryptionProcessingTime = encryptionProcessingTime;
        this.decryptionProcessingTime = decryptionProcessingTime;
    }

    // Token: 0x06000487 RID: 1159 RVA: 0x0000BA80 File Offset: 0x00009C80
    public static NetworkStatisticsDelta operator -(in NetworkStatisticsState a, in NetworkStatisticsState b)
    {
        return new NetworkStatisticsDelta(a.packetsSent - b.packetsSent, a.packetsReceived - b.packetsReceived, a.bytesSent - b.bytesSent, a.bytesReceived - b.bytesReceived, a.packetsLost - b.packetsLost, a.packetsSentEncrypted - b.packetsSentEncrypted, a.packetsSentPlaintext - b.packetsSentPlaintext, a.packetsSentRejected - b.packetsSentRejected, a.packetsReceivedEncrypted - b.packetsReceivedEncrypted, a.packetsReceivedPlaintext - b.packetsReceivedPlaintext, a.packetsReceivedRejected - b.packetsReceivedRejected, a.encryptionProcessingTime - b.encryptionProcessingTime, a.decryptionProcessingTime - b.decryptionProcessingTime);
    }

    // Token: 0x0400019F RID: 415
    public readonly long packetsSent;

    // Token: 0x040001A0 RID: 416
    public readonly long packetsReceived;

    // Token: 0x040001A1 RID: 417
    public readonly long bytesSent;

    // Token: 0x040001A2 RID: 418
    public readonly long bytesReceived;

    // Token: 0x040001A3 RID: 419
    public readonly long packetsLost;

    // Token: 0x040001A4 RID: 420
    public readonly long packetsSentEncrypted;

    // Token: 0x040001A5 RID: 421
    public readonly long packetsSentPlaintext;

    // Token: 0x040001A6 RID: 422
    public readonly long packetsSentRejected;

    // Token: 0x040001A7 RID: 423
    public readonly long packetsReceivedEncrypted;

    // Token: 0x040001A8 RID: 424
    public readonly long packetsReceivedPlaintext;

    // Token: 0x040001A9 RID: 425
    public readonly long packetsReceivedRejected;

    // Token: 0x040001AA RID: 426
    public readonly long encryptionProcessingTime;

    // Token: 0x040001AB RID: 427
    public readonly long decryptionProcessingTime;

    // Token: 0x0200013A RID: 314
    // (Invoke) Token: 0x0600080C RID: 2060
    public delegate void NetworkStatisticsUpdateDelegate(in NetworkStatisticsState statisticsState);
}
