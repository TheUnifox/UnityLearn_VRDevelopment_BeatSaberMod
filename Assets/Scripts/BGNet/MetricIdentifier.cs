using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

// Token: 0x0200005B RID: 91
public readonly struct MetricIdentifier : IComparable<MetricIdentifier>, IEquatable<MetricIdentifier>
{
    // Token: 0x170000AD RID: 173
    // (get) Token: 0x0600040D RID: 1037 RVA: 0x00009FE0 File Offset: 0x000081E0
    public int tagCount
    {
        get
        {
            return ((this._tag0.Item1 == null) ? 0 : 1) + ((this._tag1.Item1 == null) ? 0 : 1) + ((this._tag2.Item1 == null) ? 0 : 1) + ((this._tag3.Item1 == null) ? 0 : 1);
        }
    }

    // Token: 0x0600040E RID: 1038 RVA: 0x0000A034 File Offset: 0x00008234
    public MetricIdentifier(string metricName, 
        (string key, string value) tag0 = default(ValueTuple<string, string>), 
        (string key, string value) tag1 = default(ValueTuple<string, string>), 
        (string key, string value) tag2 = default(ValueTuple<string, string>), 
        (string key, string value) tag3 = default(ValueTuple<string, string>))
    {
        this.metricName = metricName;
        this._tag0 = (string.IsNullOrEmpty(tag0.Item1) ? default(ValueTuple<string, string>) : tag0);
        this._tag1 = (string.IsNullOrEmpty(tag1.Item1) ? default(ValueTuple<string, string>) : tag1);
        this._tag2 = (string.IsNullOrEmpty(tag2.Item1) ? default(ValueTuple<string, string>) : tag2);
        this._tag3 = (string.IsNullOrEmpty(tag3.Item1) ? default(ValueTuple<string, string>) : tag3);
    }

    // Token: 0x0600040F RID: 1039 RVA: 0x0000A0C8 File Offset: 0x000082C8
    public int CompareTo(MetricIdentifier other)
    {
        int num = string.Compare(this.metricName, other.metricName, StringComparison.Ordinal);
        if (num != 0)
        {
            return num;
        }
        int num2 = string.Compare(this._tag0.Item2, other._tag0.Item2, StringComparison.Ordinal);
        if (num2 != 0)
        {
            return num2;
        }
        int num3 = string.Compare(this._tag1.Item2, other._tag1.Item2, StringComparison.Ordinal);
        if (num3 != 0)
        {
            return num3;
        }
        int num4 = string.Compare(this._tag2.Item2, other._tag2.Item2, StringComparison.Ordinal);
        if (num4 != 0)
        {
            return num4;
        }
        return string.Compare(this._tag3.Item2, other._tag3.Item2, StringComparison.Ordinal);
    }

    // Token: 0x06000410 RID: 1040 RVA: 0x0000A16F File Offset: 0x0000836F
    public bool Equals(MetricIdentifier other)
    {
        return this.CompareTo(other) == 0;
    }

    // Token: 0x06000411 RID: 1041 RVA: 0x0000A17C File Offset: 0x0000837C
    public override bool Equals(object obj)
    {
        if (obj is MetricIdentifier)
        {
            MetricIdentifier other = (MetricIdentifier)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x06000412 RID: 1042 RVA: 0x0000A1A4 File Offset: 0x000083A4
    public override int GetHashCode()
    {
        return (((((this.metricName != null) ? this.metricName.GetHashCode() : 0) * 397 ^ ((this._tag0.Item2 != null) ? this._tag0.Item2.GetHashCode() : 0)) * 397 ^ ((this._tag1.Item2 != null) ? this._tag1.Item2.GetHashCode() : 0)) * 397 ^ ((this._tag2.Item2 != null) ? this._tag2.Item2.GetHashCode() : 0)) * 397 ^ ((this._tag3.Item2 != null) ? this._tag3.Item2.GetHashCode() : 0);
    }

    // Token: 0x06000413 RID: 1043 RVA: 0x0000A264 File Offset: 0x00008464
    public override string ToString()
    {
        if (this.tagCount == 0)
        {
            return this.metricName;
        }
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(this.metricName);
        stringBuilder.Append("(");
        foreach (ValueTuple<string, string> valueTuple in this.GetTags())
        {
            stringBuilder.Append(valueTuple.Item1);
            stringBuilder.Append(" = ");
            stringBuilder.Append(valueTuple.Item2);
            stringBuilder.Append(", ");
        }
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
        stringBuilder.Append(")");
        return stringBuilder.ToString();
    }

    // Token: 0x06000414 RID: 1044 RVA: 0x0000A32C File Offset: 0x0000852C
    public IEnumerable<(string key, string value)> GetTags()
    {
        if (this._tag0.Item1 != null)
        {
            yield return this._tag0;
        }
        if (this._tag1.Item1 != null)
        {
            yield return this._tag1;
        }
        if (this._tag2.Item1 != null)
        {
            yield return this._tag2;
        }
        if (this._tag3.Item1 != null)
        {
            yield return this._tag3;
        }
        yield break;
    }

    // Token: 0x06000415 RID: 1045 RVA: 0x0000A344 File Offset: 0x00008544
    public static implicit operator MetricIdentifier(string metricName)
    {
        return new MetricIdentifier(metricName, default(ValueTuple<string, string>), default(ValueTuple<string, string>), default(ValueTuple<string, string>), default(ValueTuple<string, string>));
    }

    // Token: 0x04000164 RID: 356
    public readonly string metricName;

    // Token: 0x04000165 RID: 357
    private readonly (string key, string value) _tag0;
    private readonly (string key, string value) _tag1;
    private readonly (string key, string value) _tag2;
    private readonly (string key, string value) _tag3;
}
