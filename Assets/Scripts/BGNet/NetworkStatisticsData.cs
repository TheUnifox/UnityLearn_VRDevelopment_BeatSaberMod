using System;

// Token: 0x02000067 RID: 103
public readonly struct NetworkStatisticsDelta
{
    // Token: 0x06000488 RID: 1160 RVA: 0x0000BB3C File Offset: 0x00009D3C
    public NetworkStatisticsDelta(long packetsSentDelta, long packetsReceivedDelta, long bytesSentDelta, long bytesReceivedDelta, long packetsLostDelta, long packetsSentEncryptedDelta, long packetsSentPlaintextDelta, long packetsSentRejectedDelta, long packetsReceivedEncryptedDelta, long packetsReceivedPlaintextDelta, long packetsReceivedRejectedDelta, long encryptionProcessingTimeDelta, long decryptionProcessingTimeDelta)
    {
        this.packetsSentDelta = packetsSentDelta;
        this.packetsReceivedDelta = packetsReceivedDelta;
        this.bytesSentDelta = bytesSentDelta;
        this.bytesReceivedDelta = bytesReceivedDelta;
        this.packetsLostDelta = packetsLostDelta;
        this.packetsSentEncryptedDelta = packetsSentEncryptedDelta;
        this.packetsSentPlaintextDelta = packetsSentPlaintextDelta;
        this.packetsSentRejectedDelta = packetsSentRejectedDelta;
        this.packetsReceivedEncryptedDelta = packetsReceivedEncryptedDelta;
        this.packetsReceivedPlaintextDelta = packetsReceivedPlaintextDelta;
        this.packetsReceivedRejectedDelta = packetsReceivedRejectedDelta;
        this.encryptionProcessingTimeDelta = encryptionProcessingTimeDelta;
        this.decryptionProcessingTimeDelta = decryptionProcessingTimeDelta;
    }

    // Token: 0x040001AC RID: 428
    public readonly long packetsSentDelta;

    // Token: 0x040001AD RID: 429
    public readonly long packetsReceivedDelta;

    // Token: 0x040001AE RID: 430
    public readonly long bytesSentDelta;

    // Token: 0x040001AF RID: 431
    public readonly long bytesReceivedDelta;

    // Token: 0x040001B0 RID: 432
    public readonly long packetsLostDelta;

    // Token: 0x040001B1 RID: 433
    public readonly long packetsSentEncryptedDelta;

    // Token: 0x040001B2 RID: 434
    public readonly long packetsSentPlaintextDelta;

    // Token: 0x040001B3 RID: 435
    public readonly long packetsSentRejectedDelta;

    // Token: 0x040001B4 RID: 436
    public readonly long packetsReceivedEncryptedDelta;

    // Token: 0x040001B5 RID: 437
    public readonly long packetsReceivedPlaintextDelta;

    // Token: 0x040001B6 RID: 438
    public readonly long packetsReceivedRejectedDelta;

    // Token: 0x040001B7 RID: 439
    public readonly long encryptionProcessingTimeDelta;

    // Token: 0x040001B8 RID: 440
    public readonly long decryptionProcessingTimeDelta;
}
