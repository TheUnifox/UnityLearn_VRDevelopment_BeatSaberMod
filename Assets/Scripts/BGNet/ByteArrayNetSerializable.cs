using System;
using LiteNetLib.Utils;

// Token: 0x02000009 RID: 9
public class ByteArrayNetSerializable : INetSerializable
{
    // Token: 0x0600002A RID: 42 RVA: 0x00002639 File Offset: 0x00000839
    public ByteArrayNetSerializable(string name, int minLength = 0, int maxLength = 32767, bool allowNull = false)
    {
        this._name = name;
        this._allowNull = allowNull;
        this._minLength = minLength;
        this._maxLength = maxLength;
    }

    // Token: 0x0600002B RID: 43 RVA: 0x0000265E File Offset: 0x0000085E
    public ByteArrayNetSerializable(string name, int size, bool allowNull = false) : this(name, size, size, allowNull)
    {
    }

    // Token: 0x17000005 RID: 5
    // (get) Token: 0x0600002C RID: 44 RVA: 0x0000266A File Offset: 0x0000086A
    // (set) Token: 0x0600002D RID: 45 RVA: 0x00002674 File Offset: 0x00000874
    public byte[] data
    {
        get
        {
            return this._data;
        }
        set
        {
            if (value == null)
            {
                if (this._allowNull)
                {
                    throw new ArgumentException(this._name + " is not allowed to be null");
                }
                this._data = null;
                return;
            }
            else
            {
                if (value.Length < this._minLength || value.Length > this._maxLength)
                {
                    throw new ArgumentException(string.Format("{0} must be between {1} and {2} long, given an array of length {3}", new object[]
                    {
                        this._name,
                        this._minLength,
                        this._maxLength,
                        value.Length
                    }));
                }
                this._data = value;
                return;
            }
        }
    }

    // Token: 0x0600002E RID: 46 RVA: 0x00002710 File Offset: 0x00000910
    public void Serialize(NetDataWriter writer)
    {
        if (this._data == null && !this._allowNull)
        {
            throw new ArgumentException(this._name + " is not allowed to be null");
        }
        int num = this._minLength;
        if (this._minLength != this._maxLength || this._allowNull)
        {
            byte[] data = this._data;
            num = ((data != null) ? data.Length : 0);
            writer.PutVarUInt((uint)num);
        }
        if (num > 0)
        {
            writer.Put(this._data);
        }
    }

    // Token: 0x0600002F RID: 47 RVA: 0x00002788 File Offset: 0x00000988
    public void Deserialize(NetDataReader reader)
    {
        int num = this._minLength;
        if (this._minLength != this._maxLength || this._allowNull)
        {
            num = (int)reader.GetVarUInt();
            if (num < this._minLength || num > this._maxLength)
            {
                throw new ArgumentException(string.Format("{0} must be between {1} and {2} long, given an array of length {3}", new object[]
                {
                    this._name,
                    this._minLength,
                    this._maxLength,
                    num
                }));
            }
        }
        this._data = new byte[num];
        reader.GetBytes(this._data, 0, this._data.Length);
    }

    // Token: 0x06000030 RID: 48 RVA: 0x00002830 File Offset: 0x00000A30
    public void Clear()
    {
        this._data = null;
    }

    // Token: 0x06000031 RID: 49 RVA: 0x00002839 File Offset: 0x00000A39
    public static implicit operator byte[](ByteArrayNetSerializable byteArrayNetSerializable)
    {
        return byteArrayNetSerializable.data;
    }

    // Token: 0x0400000F RID: 15
    private byte[] _data;

    // Token: 0x04000010 RID: 16
    private readonly string _name;

    // Token: 0x04000011 RID: 17
    private readonly bool _allowNull;

    // Token: 0x04000012 RID: 18
    private readonly int _minLength;

    // Token: 0x04000013 RID: 19
    private readonly int _maxLength;
}
