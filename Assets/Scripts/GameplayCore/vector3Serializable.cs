using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000034 RID: 52
public struct Vector3Serializable : INetSerializable, IEquatable<Vector3Serializable>
{
    // Token: 0x060000FB RID: 251 RVA: 0x0000572D File Offset: 0x0000392D
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._x);
        writer.Put(this._y);
        writer.Put(this._z);
    }

    // Token: 0x060000FC RID: 252 RVA: 0x00005753 File Offset: 0x00003953
    public void Deserialize(NetDataReader reader)
    {
        this._x = reader.GetInt();
        this._y = reader.GetInt();
        this._z = reader.GetInt();
    }

    // Token: 0x060000FD RID: 253 RVA: 0x00005779 File Offset: 0x00003979
    public bool Equals(Vector3Serializable other)
    {
        return this._x == other._x && this._y == other._y && this._z == other._z;
    }

    // Token: 0x060000FE RID: 254 RVA: 0x000057A7 File Offset: 0x000039A7
    public bool Approximately(Vector3Serializable other)
    {
        return QuantizedMathf.Approximately(this._x, other._x, 1) && QuantizedMathf.Approximately(this._y, other._y, 1) && QuantizedMathf.Approximately(this._z, other._z, 1);
    }

    // Token: 0x060000FF RID: 255 RVA: 0x000057E8 File Offset: 0x000039E8
    public override bool Equals(object obj)
    {
        if (obj is Vector3Serializable)
        {
            Vector3Serializable other = (Vector3Serializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x06000100 RID: 256 RVA: 0x00005810 File Offset: 0x00003A10
    public override int GetHashCode()
    {
        return (this._x << 24 | (this._x >> 8 & 255)) ^ (this._y << 16 | (this._y >> 16 & 65535)) ^ (this._z << 8 | (this._z >> 24 & 16777215));
    }

    // Token: 0x06000101 RID: 257 RVA: 0x00005868 File Offset: 0x00003A68
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
            ")"
        });
    }

    // Token: 0x06000102 RID: 258 RVA: 0x000058CA File Offset: 0x00003ACA
    public int GetSize()
    {
        return VarIntExtensions.GetSize(this._x) + VarIntExtensions.GetSize(this._y) + VarIntExtensions.GetSize(this._z);
    }

    // Token: 0x06000103 RID: 259 RVA: 0x000058F0 File Offset: 0x00003AF0
    public Vector3Serializable(Vector3 v)
    {
        this._x = Mathf.RoundToInt(v.x * 1000f);
        this._y = Mathf.RoundToInt(v.y * 1000f);
        this._z = Mathf.RoundToInt(v.z * 1000f);
    }

    // Token: 0x06000104 RID: 260 RVA: 0x00005944 File Offset: 0x00003B44
    public Vector3Serializable(NetDataReader reader)
    {
        this._x = (this._y = (this._z = 0));
        this.Deserialize(reader);
    }

    // Token: 0x06000105 RID: 261 RVA: 0x00005971 File Offset: 0x00003B71
    public static implicit operator Vector3(Vector3Serializable v)
    {
        return new Vector3((float)v._x / 1000f, (float)v._y / 1000f, (float)v._z / 1000f);
    }

    // Token: 0x06000106 RID: 262 RVA: 0x0000599F File Offset: 0x00003B9F
    public static implicit operator Vector3Serializable(Vector3 v)
    {
        return new Vector3Serializable(v);
    }

    // Token: 0x06000107 RID: 263 RVA: 0x000059A8 File Offset: 0x00003BA8
    public static Vector3Serializable operator +(Vector3Serializable a, Vector3Serializable b)
    {
        return new Vector3Serializable
        {
            _x = a._x + b._x,
            _y = a._y + b._y,
            _z = a._z + b._z
        };
    }

    // Token: 0x06000108 RID: 264 RVA: 0x000059FC File Offset: 0x00003BFC
    public static Vector3Serializable operator -(Vector3Serializable a, Vector3Serializable b)
    {
        return new Vector3Serializable
        {
            _x = a._x - b._x,
            _y = a._y - b._y,
            _z = a._z - b._z
        };
    }

    // Token: 0x040000F2 RID: 242
    private int _x;

    // Token: 0x040000F3 RID: 243
    private int _y;

    // Token: 0x040000F4 RID: 244
    private int _z;
}
