using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x0200003A RID: 58
public struct ColorNoAlphaSerializable : INetSerializable, IEquatable<ColorNoAlphaSerializable>
{
    // Token: 0x06000147 RID: 327 RVA: 0x0000682D File Offset: 0x00004A2D
    public ColorNoAlphaSerializable(Color color)
    {
        this._color = color;
    }

    // Token: 0x06000148 RID: 328 RVA: 0x00006836 File Offset: 0x00004A36
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._color.r);
        writer.Put(this._color.g);
        writer.Put(this._color.b);
    }

    // Token: 0x06000149 RID: 329 RVA: 0x0000686C File Offset: 0x00004A6C
    public void Deserialize(NetDataReader reader)
    {
        this._color.r = reader.GetFloat();
        this._color.g = reader.GetFloat();
        this._color.b = reader.GetFloat();
        this._color.a = 1f;
    }

    // Token: 0x0600014A RID: 330 RVA: 0x000068BC File Offset: 0x00004ABC
    public static implicit operator Color(ColorNoAlphaSerializable c)
    {
        return c._color;
    }

    // Token: 0x0600014B RID: 331 RVA: 0x000068C4 File Offset: 0x00004AC4
    public static implicit operator ColorNoAlphaSerializable(Color c)
    {
        return new ColorNoAlphaSerializable(c);
    }

    // Token: 0x0600014C RID: 332 RVA: 0x000068CC File Offset: 0x00004ACC
    public bool Equals(ColorNoAlphaSerializable other)
    {
        return Mathf.Approximately(this._color.r, other._color.r) && Mathf.Approximately(this._color.g, other._color.g) && Mathf.Approximately(this._color.b, other._color.b);
    }

    // Token: 0x0600014D RID: 333 RVA: 0x00006930 File Offset: 0x00004B30
    public override bool Equals(object obj)
    {
        if (obj is ColorNoAlphaSerializable)
        {
            ColorNoAlphaSerializable other = (ColorNoAlphaSerializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x0600014E RID: 334 RVA: 0x00006957 File Offset: 0x00004B57
    public override int GetHashCode()
    {
        return this._color.GetHashCode();
    }

    // Token: 0x0600014F RID: 335 RVA: 0x0000696A File Offset: 0x00004B6A
    public override string ToString()
    {
        return this._color.ToString();
    }

    // Token: 0x04000104 RID: 260
    private Color _color;
}
