using System;
using LiteNetLib.Utils;

// Token: 0x02000005 RID: 5
public readonly struct BitMask256 : IBitMask<BitMask256>, IEquatable<BitMask256>, INetImmutableSerializable<BitMask256>
{
    // Token: 0x17000006 RID: 6
    // (get) Token: 0x0600002F RID: 47 RVA: 0x0000258A File Offset: 0x0000078A
    public int bitCount
    {
        get
        {
            return 256;
        }
    }

    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000030 RID: 48 RVA: 0x00002591 File Offset: 0x00000791
    public static BitMask256 maxValue
    {
        get
        {
            return new BitMask256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue);
        }
    }

    // Token: 0x06000031 RID: 49 RVA: 0x000025A0 File Offset: 0x000007A0
    public BitMask256(ulong d0, ulong d1, ulong d2, ulong d3)
    {
        this._d0 = d0;
        this._d1 = d1;
        this._d2 = d2;
        this._d3 = d3;
    }

    // Token: 0x06000032 RID: 50 RVA: 0x000025C0 File Offset: 0x000007C0
    public BitMask256(ulong value)
    {
        this._d0 = (this._d1 = (this._d2 = 0UL));
        this._d3 = value;
    }

    // Token: 0x06000033 RID: 51 RVA: 0x000025F0 File Offset: 0x000007F0
    public BitMask256 SetBits(int offset, ulong bits)
    {
        ulong d = this._d0;
        int num = offset - 192;
        ulong d2 = d | bits.ShiftLeft(num);
        ulong d3 = this._d1;
        int num2 = offset - 128;
        ulong d4 = d3 | bits.ShiftLeft(num2);
        ulong d5 = this._d2;
        int num3 = offset - 64;
        return new BitMask256(d2, d4, d5 | bits.ShiftLeft(num3), this._d3 | bits.ShiftLeft(offset));
    }

    // Token: 0x06000034 RID: 52 RVA: 0x00002658 File Offset: 0x00000858
    public ulong GetBits(int offset, int count)
    {
        ulong num = (1UL << count) - 1UL;
        int num2 = offset - 192;
        ulong num3 = this._d0.ShiftRight(num2);
        int num4 = offset - 128;
        ulong num5 = num3 | this._d1.ShiftRight(num4);
        int num6 = offset - 64;
        return (num5 | this._d2.ShiftRight(num6) | this._d3.ShiftRight(offset)) & num;
    }

    // Token: 0x06000035 RID: 53 RVA: 0x000026BE File Offset: 0x000008BE
    public static BitMask256 operator |(in BitMask256 a, in BitMask256 b)
    {
        return new BitMask256(a._d0 | b._d0, a._d1 | b._d1, a._d2 | b._d2, a._d3 | b._d3);
    }

    // Token: 0x06000036 RID: 54 RVA: 0x000026F9 File Offset: 0x000008F9
    public static BitMask256 operator &(in BitMask256 a, in BitMask256 b)
    {
        return new BitMask256(a._d0 & b._d0, a._d1 & b._d1, a._d2 & b._d2, a._d3 & b._d3);
    }

    // Token: 0x06000037 RID: 55 RVA: 0x00002734 File Offset: 0x00000934
    public static BitMask256 operator ^(in BitMask256 a, in BitMask256 b)
    {
        return new BitMask256(a._d0 ^ b._d0, a._d1 ^ b._d1, a._d2 ^ b._d2, a._d3 ^ b._d3);
    }

    // Token: 0x06000038 RID: 56 RVA: 0x00002770 File Offset: 0x00000970
    public static BitMask256 operator <<(in BitMask256 a, int bits)
    {
        if (bits < 0)
        {
            return a >> -bits;
        }
        return new BitMask256(a._d0 << bits | a._d1 >> 64 - bits, a._d1 << bits | a._d2 >> 64 - bits, a._d2 << bits | a._d3 >> 64 - bits, a._d3 << bits);
    }

    // Token: 0x06000039 RID: 57 RVA: 0x000027E8 File Offset: 0x000009E8
    public static BitMask256 operator >>(in BitMask256 a, int bits)
    {
        if (bits < 0)
        {
            return a << -bits;
        }
        return new BitMask256(a._d0 >> bits, a._d0 << 64 - bits | a._d1 >> bits, a._d1 << 64 - bits | a._d2 >> bits, a._d2 << 64 - bits | a._d3 >> bits);
    }

    // Token: 0x0600003A RID: 58 RVA: 0x00002860 File Offset: 0x00000A60
    public static bool operator ==(in BitMask256 a, in BitMask256 b)
    {
        return a._d0 == b._d0 && a._d1 == b._d1 && a._d2 == b._d2 && a._d3 == b._d3;
    }

    // Token: 0x0600003B RID: 59 RVA: 0x0000289C File Offset: 0x00000A9C
    public static bool operator !=(in BitMask256 a, in BitMask256 b)
    {
        return a._d0 != b._d0 || a._d1 != b._d1 || a._d2 != b._d2 || a._d3 != b._d3;
    }

    // Token: 0x0600003C RID: 60 RVA: 0x000028DB File Offset: 0x00000ADB
    public static implicit operator BitMask256(ulong value)
    {
        return new BitMask256(value);
    }

    // Token: 0x0600003D RID: 61 RVA: 0x000028E3 File Offset: 0x00000AE3
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._d0);
        writer.Put(this._d1);
        writer.Put(this._d2);
        writer.Put(this._d3);
    }

    // Token: 0x0600003E RID: 62 RVA: 0x00002915 File Offset: 0x00000B15
    public BitMask256 CreateFromSerializedData(NetDataReader reader)
    {
        return BitMask256.Deserialize(reader);
    }

    // Token: 0x0600003F RID: 63 RVA: 0x0000291D File Offset: 0x00000B1D
    public static BitMask256 Deserialize(NetDataReader reader)
    {
        return new BitMask256(reader.GetULong(), reader.GetULong(), reader.GetULong(), reader.GetULong());
    }

    // Token: 0x06000040 RID: 64 RVA: 0x0000293C File Offset: 0x00000B3C
    public override string ToString()
    {
        return this._d0.ToString("x16") + this._d1.ToString("x16") + this._d2.ToString("x16") + this._d3.ToString("x16");
    }

    // Token: 0x06000041 RID: 65 RVA: 0x0000299A File Offset: 0x00000B9A
    public bool Equals(BitMask256 other)
    {
        return this == other;
    }

    // Token: 0x06000042 RID: 66 RVA: 0x000029A4 File Offset: 0x00000BA4
    public override bool Equals(object obj)
    {
        if (obj is BitMask256)
        {
            BitMask256 bitMask = (BitMask256)obj;
            return this == bitMask;
        }
        return false;
    }

    // Token: 0x06000043 RID: 67 RVA: 0x000029CC File Offset: 0x00000BCC
    public override int GetHashCode()
    {
        return this._d0.GetHashCode() ^ this._d1.GetHashCode() ^ this._d2.GetHashCode() ^ this._d3.GetHashCode();
    }

    // Token: 0x0400000C RID: 12
    private readonly ulong _d0;

    // Token: 0x0400000D RID: 13
    private readonly ulong _d1;

    // Token: 0x0400000E RID: 14
    private readonly ulong _d2;

    // Token: 0x0400000F RID: 15
    private readonly ulong _d3;
}
