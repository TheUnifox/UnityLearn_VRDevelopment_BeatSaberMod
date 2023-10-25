using System;

// Token: 0x02000015 RID: 21
public interface IBitMask<T> : IEquatable<T>
{
    // Token: 0x1700001C RID: 28
    // (get) Token: 0x0600007D RID: 125
    int bitCount { get; }

    // Token: 0x0600007E RID: 126
    T SetBits(int offset, ulong bits);

    // Token: 0x0600007F RID: 127
    ulong GetBits(int offset, int count);
}
