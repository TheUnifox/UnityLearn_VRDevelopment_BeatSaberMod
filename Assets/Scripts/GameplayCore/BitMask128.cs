using System;
using LiteNetLib.Utils;

// Token: 0x02000004 RID: 4
public readonly struct BitMask128 : IBitMask<BitMask128>, IEquatable<BitMask128>, INetImmutableSerializable<BitMask128>
{
    // Token: 0x17000004 RID: 4
    // (get) Token: 0x0600001A RID: 26 RVA: 0x00002304 File Offset: 0x00000504
    public int bitCount
    {
        get
        {
            return 128;
        }
    }

    // Token: 0x17000005 RID: 5
    // (get) Token: 0x0600001B RID: 27 RVA: 0x0000230B File Offset: 0x0000050B
    public static BitMask128 maxValue
    {
        get
        {
            return new BitMask128(ulong.MaxValue, ulong.MaxValue);
        }
    }

    // Token: 0x0600001C RID: 28 RVA: 0x00002316 File Offset: 0x00000516
    public BitMask128(ulong d0, ulong d1)
    {
        this._d0 = d0;
        this._d1 = d1;
    }

    // Token: 0x0600001D RID: 29 RVA: 0x00002326 File Offset: 0x00000526
    public BitMask128(ulong value)
    {
        this._d0 = 0UL;
        this._d1 = value;
    }

    // Token: 0x0600001E RID: 30 RVA: 0x00002338 File Offset: 0x00000538
    public BitMask128 SetBits(int offset, ulong bits)
    {
        ulong d = this._d0;
        int num = offset - 64;
        return new BitMask128(d | bits.ShiftLeft(num), this._d1 | bits.ShiftLeft(offset));
    }

    // Token: 0x0600001F RID: 31 RVA: 0x00002370 File Offset: 0x00000570
    public ulong GetBits(int offset, int count)
    {
        ulong num = (1UL << count) - 1UL;
        int num2 = offset - 64;
        return (this._d0.ShiftRight(num2) | this._d1.ShiftRight(offset)) & num;
    }

    // Token: 0x06000020 RID: 32 RVA: 0x000023AA File Offset: 0x000005AA
    public static BitMask128 operator |(in BitMask128 a, in BitMask128 b)
    {
        return new BitMask128(a._d0 | b._d0, a._d1 | b._d1);
    }

    // Token: 0x06000021 RID: 33 RVA: 0x000023CB File Offset: 0x000005CB
    public static BitMask128 operator &(in BitMask128 a, in BitMask128 b)
    {
        return new BitMask128(a._d0 & b._d0, a._d1 & b._d1);
    }

    // Token: 0x06000022 RID: 34 RVA: 0x000023EC File Offset: 0x000005EC
    public static BitMask128 operator ^(in BitMask128 a, in BitMask128 b)
    {
        return new BitMask128(a._d0 ^ b._d0, a._d1 ^ b._d1);
    }

    // Token: 0x06000023 RID: 35 RVA: 0x0000240D File Offset: 0x0000060D
    public static BitMask128 operator <<(in BitMask128 a, int bits)
    {
        if (bits < 0)
        {
            return a >> -bits;
        }
        return new BitMask128(a._d0 << bits | a._d1 >> 64 - bits, a._d1 << bits);
    }

    // Token: 0x06000024 RID: 36 RVA: 0x00002446 File Offset: 0x00000646
    public static BitMask128 operator >>(in BitMask128 a, int bits)
    {
        if (bits < 0)
        {
            return a << -bits;
        }
        return new BitMask128(a._d0 >> bits, a._d0 << 64 - bits | a._d1 >> bits);
    }

    // Token: 0x06000025 RID: 37 RVA: 0x0000247F File Offset: 0x0000067F
    public static bool operator ==(in BitMask128 a, in BitMask128 b)
    {
        return a._d0 == b._d0 && a._d1 == b._d1;
    }

    // Token: 0x06000026 RID: 38 RVA: 0x0000249F File Offset: 0x0000069F
    public static bool operator !=(in BitMask128 a, in BitMask128 b)
    {
        return a._d0 != b._d0 || a._d1 != b._d1;
    }

    // Token: 0x06000027 RID: 39 RVA: 0x000024C2 File Offset: 0x000006C2
    public static implicit operator BitMask128(ulong value)
    {
        return new BitMask128(value);
    }

    // Token: 0x06000028 RID: 40 RVA: 0x000024CA File Offset: 0x000006CA
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._d0);
        writer.Put(this._d1);
    }

    // Token: 0x06000029 RID: 41 RVA: 0x000024E4 File Offset: 0x000006E4
    public BitMask128 CreateFromSerializedData(NetDataReader reader)
    {
        return BitMask128.Deserialize(reader);
    }

    // Token: 0x0600002A RID: 42 RVA: 0x000024EC File Offset: 0x000006EC
    public static BitMask128 Deserialize(NetDataReader reader)
    {
        return new BitMask128(reader.GetULong(), reader.GetULong());
    }

    // Token: 0x0600002B RID: 43 RVA: 0x00002500 File Offset: 0x00000700
    public override string ToString()
    {
        return this._d0.ToString("x16") + this._d1.ToString("x16");
    }

    // Token: 0x0600002C RID: 44 RVA: 0x0000247F File Offset: 0x0000067F
    public bool Equals(BitMask128 other)
    {
        return this._d0 == other._d0 && this._d1 == other._d1;
    }

    // Token: 0x0600002D RID: 45 RVA: 0x00002538 File Offset: 0x00000738
    public override bool Equals(object obj)
    {
        if (obj is BitMask128)
        {
            BitMask128 bitMask = (BitMask128)obj;
            return this == bitMask;
        }
        return false;
    }

    // Token: 0x0600002E RID: 46 RVA: 0x00002560 File Offset: 0x00000760
    public override int GetHashCode()
    {
        return this._d0.GetHashCode() ^ this._d1.GetHashCode();
    }

    // Token: 0x0400000A RID: 10
    private readonly ulong _d0;

    // Token: 0x0400000B RID: 11
    private readonly ulong _d1;
}
