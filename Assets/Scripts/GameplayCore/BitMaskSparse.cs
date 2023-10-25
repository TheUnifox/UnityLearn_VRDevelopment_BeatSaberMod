using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib.Utils;

// Token: 0x02000007 RID: 7
public class BitMaskSparse : IBitMask<BitMaskSparse>, IEquatable<BitMaskSparse>, INetSerializable
{
    // Token: 0x17000009 RID: 9
    // (get) Token: 0x0600004C RID: 76 RVA: 0x00002C99 File Offset: 0x00000E99
    public int bitCount { get; }

    // Token: 0x0600004D RID: 77 RVA: 0x00002CA1 File Offset: 0x00000EA1
    public BitMaskSparse(int bitCount)
    {
        this.bitCount = bitCount;
    }

    // Token: 0x0600004E RID: 78 RVA: 0x00002CBB File Offset: 0x00000EBB
    public virtual bool Equals(BitMaskSparse other)
    {
        return other != null && this._sparseSet.SetEquals(other._sparseSet);
    }

    // Token: 0x0600004F RID: 79 RVA: 0x00002CD3 File Offset: 0x00000ED3
    public virtual BitMaskSparse SetBits(int offset, ulong bits)
    {
        while (bits > 0UL)
        {
            if ((bits & 1UL) == 1UL && offset >= 0 && offset < this.bitCount)
            {
                this._sparseSet.Add((uint)offset);
            }
            offset++;
            bits >>= 1;
        }
        return this;
    }

    // Token: 0x06000050 RID: 80 RVA: 0x00002D0C File Offset: 0x00000F0C
    public virtual ulong GetBits(int offset, int count)
    {
        ulong num = 0UL;
        for (int i = 0; i < count; i++)
        {
            if (offset + i >= 0 && offset + i < this.bitCount && this._sparseSet.Contains((uint)(offset + i)))
            {
                num |= 1UL << i;
            }
        }
        return num;
    }

    // Token: 0x06000051 RID: 81 RVA: 0x00002D54 File Offset: 0x00000F54
    public override string ToString()
    {
        return string.Join(";", from i in this._sparseSet
                                select i.ToString());
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00002D8C File Offset: 0x00000F8C
    public virtual void Serialize(NetDataWriter writer)
    {
        uint num = 0U;
        foreach (uint num2 in this._sparseSet)
        {
            writer.PutVarUInt(num2 - num);
            num = num2;
        }
        writer.PutVarUInt((uint)((long)this.bitCount - (long)((ulong)num)));
    }

    // Token: 0x06000053 RID: 83 RVA: 0x00002DF8 File Offset: 0x00000FF8
    public virtual void Deserialize(NetDataReader reader)
    {
        uint num = 0U;
        for (; ; )
        {
            uint varUInt = reader.GetVarUInt();
            num += varUInt;
            if ((ulong)num >= (ulong)((long)this.bitCount))
            {
                break;
            }
            this._sparseSet.Add(num);
        }
    }

    // Token: 0x04000013 RID: 19
    protected readonly SortedSet<uint> _sparseSet = new SortedSet<uint>();
}
