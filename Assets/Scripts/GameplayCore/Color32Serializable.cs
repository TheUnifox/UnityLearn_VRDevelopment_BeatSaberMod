using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000039 RID: 57
public struct Color32Serializable : INetSerializable, IEquatable<Color32Serializable>
{
    // Token: 0x0600013E RID: 318 RVA: 0x000066AD File Offset: 0x000048AD
    public Color32Serializable(Color32 color)
    {
        this._color = color;
    }

    // Token: 0x0600013F RID: 319 RVA: 0x000066B8 File Offset: 0x000048B8
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._color.r);
        writer.Put(this._color.g);
        writer.Put(this._color.b);
        writer.Put(this._color.a);
    }

    // Token: 0x06000140 RID: 320 RVA: 0x0000670C File Offset: 0x0000490C
    public void Deserialize(NetDataReader reader)
    {
        this._color.r = reader.GetByte();
        this._color.g = reader.GetByte();
        this._color.b = reader.GetByte();
        this._color.a = reader.GetByte();
    }

    // Token: 0x06000141 RID: 321 RVA: 0x0000675D File Offset: 0x0000495D
    public static implicit operator Color32(Color32Serializable c)
    {
        return c._color;
    }

    // Token: 0x06000142 RID: 322 RVA: 0x00006765 File Offset: 0x00004965
    public static implicit operator Color32Serializable(Color32 c)
    {
        return new Color32Serializable(c);
    }

    // Token: 0x06000143 RID: 323 RVA: 0x00006770 File Offset: 0x00004970
    public bool Equals(Color32Serializable other)
    {
        return this._color.r == other._color.r && this._color.g == other._color.g && this._color.b == other._color.b && this._color.a == other._color.a;
    }

    // Token: 0x06000144 RID: 324 RVA: 0x000067E0 File Offset: 0x000049E0
    public override bool Equals(object obj)
    {
        if (obj is Color32Serializable)
        {
            Color32Serializable other = (Color32Serializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x06000145 RID: 325 RVA: 0x00006807 File Offset: 0x00004A07
    public override int GetHashCode()
    {
        return this._color.GetHashCode();
    }

    // Token: 0x06000146 RID: 326 RVA: 0x0000681A File Offset: 0x00004A1A
    public override string ToString()
    {
        return this._color.ToString();
    }

    // Token: 0x04000103 RID: 259
    private Color32 _color;
}
