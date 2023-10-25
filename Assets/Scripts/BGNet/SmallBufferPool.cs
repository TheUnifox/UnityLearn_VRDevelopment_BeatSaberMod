using System;
using System.Collections.Generic;

// Token: 0x0200007D RID: 125
public class SmallBufferPool
{
    // Token: 0x0600053F RID: 1343 RVA: 0x0000E514 File Offset: 0x0000C714
    public byte[] GetBuffer(int length)
    {
        if (length > 4096)
        {
            return new byte[length];
        }
        List<byte[]> list = (length > 2048) ? this._cacheMax : ((length > 1024) ? this._cacheLarge : ((length > 512) ? this._cacheMedium : this._cacheSmall));
        List<byte[]> obj = list;
        lock (obj)
        {
            if (list.Count > 0)
            {
                byte[] result = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return result;
            }
        }
        return new byte[(length > 2048) ? 4096 : ((length > 1024) ? 2048 : ((length > 512) ? 1024 : 512))];
    }

    // Token: 0x06000540 RID: 1344 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
    public void ReleaseBuffer(byte[] buffer)
    {
        int num = buffer.Length;
        List<byte[]> list;
        int num2;
        if (num <= 1024)
        {
            if (num != 512)
            {
                if (num != 1024)
                {
                    return;
                }
                list = this._cacheMedium;
                num2 = 32;
            }
            else
            {
                list = this._cacheSmall;
                num2 = 128;
            }
        }
        else if (num != 2048)
        {
            if (num != 4096)
            {
                return;
            }
            list = this._cacheMax;
            num2 = 8;
        }
        else
        {
            list = this._cacheLarge;
            num2 = 16;
        }
        List<byte[]> obj = list;
        lock (obj)
        {
            if (list.Count < num2)
            {
                list.Add(buffer);
            }
        }
    }

    // Token: 0x04000201 RID: 513
    private const int kCacheSmallSize = 512;

    // Token: 0x04000202 RID: 514
    private const int kCacheMediumSize = 1024;

    // Token: 0x04000203 RID: 515
    private const int kCacheLargeSize = 2048;

    // Token: 0x04000204 RID: 516
    private const int kCacheMaxSize = 4096;

    // Token: 0x04000205 RID: 517
    private const int kCacheSmallMaxCapacity = 128;

    // Token: 0x04000206 RID: 518
    private const int kCacheMediumMaxCapacity = 32;

    // Token: 0x04000207 RID: 519
    private const int kCacheLargeMaxCapacity = 16;

    // Token: 0x04000208 RID: 520
    private const int kCacheMaxMaxCapacity = 8;

    // Token: 0x04000209 RID: 521
    private readonly List<byte[]> _cacheSmall = new List<byte[]>(128);

    // Token: 0x0400020A RID: 522
    private readonly List<byte[]> _cacheMedium = new List<byte[]>(32);

    // Token: 0x0400020B RID: 523
    private readonly List<byte[]> _cacheLarge = new List<byte[]>(16);

    // Token: 0x0400020C RID: 524
    private readonly List<byte[]> _cacheMax = new List<byte[]>(8);
}
