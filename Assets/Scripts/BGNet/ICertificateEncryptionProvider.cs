using System;

// Token: 0x02000031 RID: 49
public interface ICertificateEncryptionProvider : IDisposable
{
    // Token: 0x06000193 RID: 403
    byte[] SignData(byte[] data, int offset, int length);
}
