using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000035 RID: 53
public struct Vector4Serializable : INetSerializable, IEquatable<Vector4Serializable>
{
    // Token: 0x06000109 RID: 265 RVA: 0x00005A4E File Offset: 0x00003C4E
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._x);
        writer.Put(this._y);
        writer.Put(this._z);
        writer.Put(this._w);
    }

    // Token: 0x0600010A RID: 266 RVA: 0x00005A80 File Offset: 0x00003C80
    public void Deserialize(NetDataReader reader)
    {
        this._x = reader.GetInt();
        this._y = reader.GetInt();
        this._z = reader.GetInt();
        this._w = reader.GetInt();
    }

    // Token: 0x0600010B RID: 267 RVA: 0x00005AB2 File Offset: 0x00003CB2
    public bool Equals(Vector4Serializable other)
    {
        return this._x == other._x && this._y == other._y && this._z == other._z && this._w == other._w;
    }

    // Token: 0x0600010C RID: 268 RVA: 0x00005AF0 File Offset: 0x00003CF0
    public bool Approximately(Vector4Serializable other)
    {
        return QuantizedMathf.Approximately(this._x, other._x, 1) && QuantizedMathf.Approximately(this._y, other._y, 1) && QuantizedMathf.Approximately(this._z, other._z, 1) && QuantizedMathf.Approximately(this._w, other._w, 1);
    }

    // Token: 0x0600010D RID: 269 RVA: 0x00005B50 File Offset: 0x00003D50
    public override bool Equals(object obj)
    {
        if (obj is Vector4Serializable)
        {
            Vector4Serializable other = (Vector4Serializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x0600010E RID: 270 RVA: 0x00005B78 File Offset: 0x00003D78
    public override int GetHashCode()
    {
        return (this._x << 24 | (this._x >> 8 & 16777215)) ^ (this._y << 16 | (this._y >> 16 & 65535)) ^ (this._z << 8 | (this._z >> 24 & 255)) ^ this._w;
    }

    // Token: 0x0600010F RID: 271 RVA: 0x00005BD8 File Offset: 0x00003DD8
    public override string ToString()
    {
        return string.Concat(new string[]
        {
            "(",
            QuantizedMathf.QuantizedVectorComponentToString(this._x),
            ", ",
            QuantizedMathf.QuantizedVectorComponentToString(this._y),
            ", ",
            QuantizedMathf.QuantizedVectorComponentToString(this._z),
            ", ",
            QuantizedMathf.QuantizedVectorComponentToString(this._w),
            ")"
        });
    }

    // Token: 0x06000110 RID: 272 RVA: 0x00005C51 File Offset: 0x00003E51
    public int GetSize()
    {
        return VarIntExtensions.GetSize(this._x) + VarIntExtensions.GetSize(this._y) + VarIntExtensions.GetSize(this._z) + VarIntExtensions.GetSize(this._w);
    }

    // Token: 0x06000111 RID: 273 RVA: 0x00005C84 File Offset: 0x00003E84
    public Vector4Serializable(Vector4 v)
    {
        this._x = Mathf.RoundToInt(v.x * 1000f);
        this._y = Mathf.RoundToInt(v.y * 1000f);
        this._z = Mathf.RoundToInt(v.z * 1000f);
        this._w = Mathf.RoundToInt(v.w * 1000f);
    }

    // Token: 0x06000112 RID: 274 RVA: 0x00005CF0 File Offset: 0x00003EF0
    public Vector4Serializable(NetDataReader reader)
    {
        this._x = (this._y = (this._z = (this._w = 0)));
        this.Deserialize(reader);
    }

    // Token: 0x06000113 RID: 275 RVA: 0x00005D26 File Offset: 0x00003F26
    public static implicit operator Vector4(Vector4Serializable v)
    {
        return new Vector4((float)v._x / 1000f, (float)v._y / 1000f, (float)v._z / 1000f, (float)v._w / 1000f);
    }

    // Token: 0x06000114 RID: 276 RVA: 0x00005D61 File Offset: 0x00003F61
    public static implicit operator Vector4Serializable(Vector4 v)
    {
        return new Vector4Serializable(v);
    }

    // Token: 0x06000115 RID: 277 RVA: 0x00005D6C File Offset: 0x00003F6C
    public static Vector4Serializable operator +(Vector4Serializable a, Vector4Serializable b)
    {
        return new Vector4Serializable
        {
            _x = a._x + b._x,
            _y = a._y + b._y,
            _z = a._z + b._z,
            _w = a._w + b._w
        };
    }

    // Token: 0x06000116 RID: 278 RVA: 0x00005DD4 File Offset: 0x00003FD4
    public static Vector4Serializable operator -(Vector4Serializable a, Vector4Serializable b)
    {
        return new Vector4Serializable
        {
            _x = a._x - b._x,
            _y = a._y - b._y,
            _z = a._z - b._z,
            _w = a._w - b._w
        };
    }

    // Token: 0x040000F5 RID: 245
    private int _x;

    // Token: 0x040000F6 RID: 246
    private int _y;

    // Token: 0x040000F7 RID: 247
    private int _z;

    // Token: 0x040000F8 RID: 248
    private int _w;
}
