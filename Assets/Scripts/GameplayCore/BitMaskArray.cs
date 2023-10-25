using System;
using System.Linq;
using LiteNetLib.Utils;

// Token: 0x02000006 RID: 6
public class BitMaskArray : IBitMask<BitMaskArray>, IEquatable<BitMaskArray>, INetSerializable
{
    // Token: 0x17000008 RID: 8
    // (get) Token: 0x06000044 RID: 68 RVA: 0x00002A14 File Offset: 0x00000C14
    public int bitCount { get; }

    // Token: 0x06000045 RID: 69 RVA: 0x00002A1C File Offset: 0x00000C1C
    public BitMaskArray(int bitCount)
    {
        this.bitCount = bitCount;
        this._data = new ulong[(bitCount + 63) / 64];
    }

    // Token: 0x06000046 RID: 70 RVA: 0x00002A3D File Offset: 0x00000C3D
    public virtual bool Equals(BitMaskArray other)
    {
        return other != null && this._data.SequenceEqual(other._data);
    }

    // Token: 0x06000047 RID: 71 RVA: 0x00002A58 File Offset: 0x00000C58
    public virtual BitMaskArray SetBits(int offset, ulong bits)
    {
        if (offset / 64 >= 0)
        {
            ulong[] data = this._data;
            int num = offset / 64;
            ulong num2 = data[num];
            int num3 = offset % 64;
            data[num] = (num2 | bits.ShiftLeft(num3));
        }
        if (offset / 64 + 1 < this._data.Length)
        {
            ulong[] data2 = this._data;
            int num4 = offset / 64 + 1;
            ulong num5 = data2[num4];
            int num3 = 64 - offset % 64;
            data2[num4] = (num5 | bits.ShiftRight(num3));
        }
        return this;
    }

    // Token: 0x06000048 RID: 72 RVA: 0x00002AC4 File Offset: 0x00000CC4
    public virtual ulong GetBits(int offset, int count)
    {
        ulong num = 0UL;
        if (offset / 64 >= 0)
        {
            ulong num2 = num;
            ulong[] data = this._data;
            int num3 = offset / 64;
            int num4 = offset % 64;
            num = (num2 | data[num3].ShiftRight(num4));
        }
        if (offset / 64 + 1 < this._data.Length)
        {
            ulong num5 = num;
            ulong[] data2 = this._data;
            int num6 = offset / 64 + 1;
            int num4 = 64 - offset % 64;
            num = (num5 | data2[num6].ShiftLeft(num4));
        }
        return num & (ulong)((long)((1 << count) - 1));
    }

    // Token: 0x06000049 RID: 73 RVA: 0x00002B3C File Offset: 0x00000D3C
    public override string ToString()
    {
        char[] array = new char[this.bitCount / 4];
        for (int i = 0; i < this.bitCount / 4; i++)
        {
            ulong bits = this.GetBits(this.bitCount - i * 4 - 4, 4);
            array[i] = (char)((bits >= 10UL) ? (65UL + bits - 10UL) : (48UL + bits));
        }
        return new string(array);
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00002BA0 File Offset: 0x00000DA0
    public virtual void Serialize(NetDataWriter writer)
    {
        for (int i = 0; i < this._data.Length; i++)
        {
            byte b = 0;
            for (int j = 0; j < 8; j++)
            {
                if ((this._data[i] >> 8 * j & 255UL) != 0UL)
                {
                    b |= (byte)(1 << j);
                }
            }
            writer.Put(b);
            for (int k = 0; k < 8; k++)
            {
                if ((this._data[i] >> 8 * k & 255UL) != 0UL)
                {
                    writer.Put((byte)(this._data[i] >> 8 * k & 255UL));
                }
            }
        }
    }

    // Token: 0x0600004B RID: 75 RVA: 0x00002C3C File Offset: 0x00000E3C
    public virtual void Deserialize(NetDataReader reader)
    {
        for (int i = 0; i < this._data.Length; i++)
        {
            byte @byte = reader.GetByte();
            for (int j = 0; j < 8; j++)
            {
                if (((int)@byte & 1 << j) != 0)
                {
                    byte byte2 = reader.GetByte();
                    this._data[i] |= (ulong)byte2 << j * 8;
                }
            }
        }
    }

    // Token: 0x04000011 RID: 17
    protected readonly ulong[] _data;
}
