using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000036 RID: 54
public struct QuaternionSerializable : INetSerializable, IEquatable<QuaternionSerializable>
{
    // Token: 0x17000032 RID: 50
    // (get) Token: 0x06000117 RID: 279 RVA: 0x00005E3A File Offset: 0x0000403A
    public static QuaternionSerializable identity
    {
        get
        {
            return new QuaternionSerializable(Quaternion.identity);
        }
    }

    // Token: 0x06000118 RID: 280 RVA: 0x00005E48 File Offset: 0x00004048
    private static void ToSmallest(Quaternion q, out int sa, out int sb, out int sc)
    {
        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;
        float num = q.x;
        float num2 = q.y;
        float num3 = q.z;
        float num4 = q.w;
        float num5 = Mathf.Abs(q.x);
        float num6 = Mathf.Abs(q.y);
        float num7 = Mathf.Abs(q.z);
        float num8 = Mathf.Abs(q.w);
        if (num8 < num5 || num8 < num6 || num8 < num7)
        {
            if (num5 >= num6 && num5 >= num7 && num5 >= num8)
            {
                flag = true;
                num = q.w;
                num4 = q.x;
            }
            else if (num6 >= num5 && num6 >= num7 && num6 >= num8)
            {
                flag2 = true;
                num2 = q.w;
                num4 = q.y;
            }
            else if (num7 >= num5 && num7 >= num6 && num7 >= num8)
            {
                flag3 = true;
                num3 = q.w;
                num4 = q.z;
            }
        }
        sa = Mathf.RoundToInt(num * 11584.53f);
        sb = Mathf.RoundToInt(num2 * 11584.53f);
        sc = Mathf.RoundToInt(num3 * 11584.53f);
        if (num4 < 0f)
        {
            flag = !flag;
            flag2 = !flag2;
            flag3 = !flag3;
        }
        sa <<= 1;
        sb <<= 1;
        sc <<= 1;
        if (flag)
        {
            sa |= 1;
        }
        if (flag2)
        {
            sb |= 1;
        }
        if (flag3)
        {
            sc |= 1;
        }
    }

    // Token: 0x06000119 RID: 281 RVA: 0x00005FA0 File Offset: 0x000041A0
    private static Quaternion FromSmallest(int sa, int sb, int sc)
    {
        bool flag = (sa & 1) == 1;
        bool flag2 = (sb & 1) == 1;
        bool flag3 = (sc & 1) == 1;
        float num = (float)(sa >> 1) * 8.632201E-05f;
        float num2 = (float)(sb >> 1) * 8.632201E-05f;
        float num3 = (float)(sc >> 1) * 8.632201E-05f;
        float num4 = Mathf.Sqrt(1f - num * num - num2 * num2 - num3 * num3);
        if ((flag && flag2) || (flag && flag3) || (flag2 && flag3))
        {
            flag = !flag;
            flag2 = !flag2;
            flag3 = !flag3;
            num4 = -num4;
        }
        return new Quaternion(flag ? num4 : num, flag2 ? num4 : num2, flag3 ? num4 : num3, flag ? num : (flag2 ? num2 : (flag3 ? num3 : num4)));
    }

    // Token: 0x0600011A RID: 282 RVA: 0x00006059 File Offset: 0x00004259
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._a);
        writer.Put(this._b);
        writer.Put(this._c);
    }

    // Token: 0x0600011B RID: 283 RVA: 0x0000607F File Offset: 0x0000427F
    public void Deserialize(NetDataReader reader)
    {
        this._a = reader.GetInt();
        this._b = reader.GetInt();
        this._c = reader.GetInt();
    }

    // Token: 0x0600011C RID: 284 RVA: 0x000060A5 File Offset: 0x000042A5
    public bool Equals(QuaternionSerializable other)
    {
        return this._a == other._a && this._b == other._b && this._c == other._c;
    }

    // Token: 0x0600011D RID: 285 RVA: 0x000060D4 File Offset: 0x000042D4
    public override bool Equals(object obj)
    {
        if (obj is QuaternionSerializable)
        {
            QuaternionSerializable other = (QuaternionSerializable)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x0600011E RID: 286 RVA: 0x000060FB File Offset: 0x000042FB
    public bool Approximately(QuaternionSerializable other)
    {
        return QuantizedMathf.Approximately(this, other);
    }

    // Token: 0x0600011F RID: 287 RVA: 0x00006114 File Offset: 0x00004314
    public override int GetHashCode()
    {
        return (this._a << 24 | (this._a >> 8 & 16777215)) ^ (this._b << 16 | (this._b >> 16 & 65535)) ^ (this._c << 8 | (this._c >> 24 & 255));
    }

    // Token: 0x06000120 RID: 288 RVA: 0x0000616C File Offset: 0x0000436C
    public int GetSize()
    {
        return VarIntExtensions.GetSize(this._a) + VarIntExtensions.GetSize(this._b) + VarIntExtensions.GetSize(this._c);
    }

    // Token: 0x06000121 RID: 289 RVA: 0x00006194 File Offset: 0x00004394
    public QuaternionSerializable(Quaternion q)
    {
        if (Math.Abs(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w - 1f) > Mathf.Epsilon)
        {
            q.Normalize();
        }
        QuaternionSerializable.ToSmallest(q, out this._a, out this._b, out this._c);
    }

    // Token: 0x06000122 RID: 290 RVA: 0x0000620C File Offset: 0x0000440C
    public QuaternionSerializable(NetDataReader reader)
    {
        this._a = (this._b = (this._c = 0));
        this.Deserialize(reader);
    }

    // Token: 0x06000123 RID: 291 RVA: 0x0000623C File Offset: 0x0000443C
    public override string ToString()
    {
        return QuaternionSerializable.FromSmallest(this._a, this._b, this._c).ToString();
    }

    // Token: 0x06000124 RID: 292 RVA: 0x0000626E File Offset: 0x0000446E
    public static implicit operator Quaternion(QuaternionSerializable q)
    {
        return QuaternionSerializable.FromSmallest(q._a, q._b, q._c);
    }

    // Token: 0x06000125 RID: 293 RVA: 0x00006287 File Offset: 0x00004487
    public static implicit operator QuaternionSerializable(Quaternion q)
    {
        return new QuaternionSerializable(q);
    }

    // Token: 0x06000126 RID: 294 RVA: 0x00006290 File Offset: 0x00004490
    public static QuaternionSerializable operator +(QuaternionSerializable a, QuaternionSerializable b)
    {
        return new QuaternionSerializable
        {
            _a = a._a + b._a,
            _b = a._b + b._b,
            _c = a._c + b._c
        };
    }

    // Token: 0x06000127 RID: 295 RVA: 0x000062E4 File Offset: 0x000044E4
    public static QuaternionSerializable operator -(QuaternionSerializable a, QuaternionSerializable b)
    {
        return new QuaternionSerializable
        {
            _a = a._a - b._a,
            _b = a._b - b._b,
            _c = a._c - b._c
        };
    }

    // Token: 0x040000F9 RID: 249
    private int _a;

    // Token: 0x040000FA RID: 250
    private int _b;

    // Token: 0x040000FB RID: 251
    private int _c;

    // Token: 0x040000FC RID: 252
    private const float kSqrtTwo = 1.4142135f;

    // Token: 0x040000FD RID: 253
    private const float kOneOverSqrtTwo = 0.70710677f;

    // Token: 0x040000FE RID: 254
    private const float kScale = 11584.53f;

    // Token: 0x040000FF RID: 255
    private const float kInvScale = 8.632201E-05f;
}
