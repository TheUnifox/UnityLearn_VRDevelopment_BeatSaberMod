using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000038 RID: 56
public struct ColorSerializable : INetSerializable, IEquatable<ColorSerializable>
{
    // Token: 0x06000135 RID: 309 RVA: 0x0000651A File Offset: 0x0000471A
    public ColorSerializable(Color color)
    {
        this._color = color;
    }

    // Token: 0x06000136 RID: 310 RVA: 0x00006524 File Offset: 0x00004724
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._color.r);
        writer.Put(this._color.g);
        writer.Put(this._color.b);
        writer.Put(this._color.a);
    }

    // Token: 0x06000137 RID: 311 RVA: 0x00006578 File Offset: 0x00004778
    public void Deserialize(NetDataReader reader)
    {
        this._color.r = reader.GetFloat();
        this._color.g = reader.GetFloat();
        this._color.b = reader.GetFloat();
        this._color.a = reader.GetFloat();
    }

    // Token: 0x06000138 RID: 312 RVA: 0x000065C9 File Offset: 0x000047C9
    public static implicit operator Color(ColorSerializable c)
    {
        return c._color;
    }

    // Token: 0x06000139 RID: 313 RVA: 0x000065D1 File Offset: 0x000047D1
    public static implicit operator ColorSerializable(Color c)
    {
        return new ColorSerializable(c);
    }

    // Token: 0x0600013A RID: 314 RVA: 0x000065DC File Offset: 0x000047DC
    public bool Equals(ColorSerializable other)
    {
        return Mathf.Approximately(this._color.r, other._color.r) && Mathf.Approximately(this._color.g, other._color.g) && Mathf.Approximately(this._color.b, other._color.b) && Mathf.Approximately(this._color.a, other._color.a);
    }

    // Token: 0x0600013B RID: 315 RVA: 0x00006660 File Offset: 0x00004860
    public override bool Equals(object obj)
    {
        if (obj is ColorSerializable)
        {
            ColorSerializable other = (ColorSerializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x0600013C RID: 316 RVA: 0x00006687 File Offset: 0x00004887
    public override int GetHashCode()
    {
        return this._color.GetHashCode();
    }

    // Token: 0x0600013D RID: 317 RVA: 0x0000669A File Offset: 0x0000489A
    public override string ToString()
    {
        return this._color.ToString();
    }

    // Token: 0x04000102 RID: 258
    private Color _color;
}
