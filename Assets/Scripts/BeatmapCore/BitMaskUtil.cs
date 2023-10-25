using System;
using System.Runtime.CompilerServices;

// Token: 0x0200003A RID: 58
public static class BitMaskUtil
{
    // Token: 0x06000126 RID: 294 RVA: 0x00004BC5 File Offset: 0x00002DC5
    public static uint NumberOfSetBits(ulong i)
    {
        return BitMaskUtil.NumberOfSetBits((uint)(i >> 32)) + BitMaskUtil.NumberOfSetBits((uint)i);
    }

    // Token: 0x06000127 RID: 295 RVA: 0x00004BD9 File Offset: 0x00002DD9
    public static uint NumberOfSetBits(uint i)
    {
        i -= (i >> 1 & 1431655765U);
        i = (i & 858993459U) + (i >> 2 & 858993459U);
        return (i + (i >> 4) & 252645135U) * 16843009U >> 24;
    }

    // Token: 0x06000128 RID: 296 RVA: 0x00004C10 File Offset: 0x00002E10
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ShiftLeft(this ulong value, in int shift)
    {
        if (shift < 0)
        {
            int num = -shift;
            return value.ShiftRight(num);
        }
        if (shift < 64)
        {
            return value << shift;
        }
        return 0UL;
    }

    // Token: 0x06000129 RID: 297 RVA: 0x00004C40 File Offset: 0x00002E40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ShiftRight(this ulong value, in int shift)
    {
        if (shift < 0)
        {
            int num = -shift;
            return value.ShiftLeft(num);
        }
        if (shift < 64)
        {
            return value >> shift;
        }
        return 0UL;
    }
}
