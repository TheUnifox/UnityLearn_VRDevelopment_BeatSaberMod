using System;
using System.Collections.Generic;
using LiteNetLib.Utils;

// Token: 0x02000003 RID: 3
public class BeatmapLevelMask : INetSerializable, IEquatable<BeatmapLevelMask>
{
    // Token: 0x0600000F RID: 15 RVA: 0x000021B9 File Offset: 0x000003B9
    public BeatmapLevelMask()
    {
    }

    // Token: 0x06000010 RID: 16 RVA: 0x000021D1 File Offset: 0x000003D1
    public BeatmapLevelMask(string level)
    {
        this._bloomFilter.AddBloomFilterEntry(level, 1, 14);
    }

    // Token: 0x06000011 RID: 17 RVA: 0x000021FC File Offset: 0x000003FC
    public BeatmapLevelMask(HashSet<string> levelSet)
    {
        foreach (string value in levelSet)
        {
            this._bloomFilter.AddBloomFilterEntry(value, 1, 14);
        }
    }

    // Token: 0x06000012 RID: 18 RVA: 0x0000226C File Offset: 0x0000046C
    public virtual bool Contains(string state)
    {
        return this._bloomFilter.ContainsBloomFilterEntry(state, 1, 14);
    }

    // Token: 0x06000013 RID: 19 RVA: 0x0000227D File Offset: 0x0000047D
    public virtual void AddLevel(string state)
    {
        this._bloomFilter.AddBloomFilterEntry(state, 1, 14);
    }

    // Token: 0x06000014 RID: 20 RVA: 0x0000228F File Offset: 0x0000048F
    public virtual void Serialize(NetDataWriter writer)
    {
        this._bloomFilter.Serialize(writer);
    }

    // Token: 0x06000015 RID: 21 RVA: 0x0000229D File Offset: 0x0000049D
    public virtual void Deserialize(NetDataReader reader)
    {
        this._bloomFilter.Deserialize(reader);
    }

    // Token: 0x06000016 RID: 22 RVA: 0x000022AB File Offset: 0x000004AB
    public override string ToString()
    {
        return "[BeatmapLevelMask " + this._bloomFilter + "]";
    }

    // Token: 0x06000017 RID: 23 RVA: 0x000022C2 File Offset: 0x000004C2
    public override int GetHashCode()
    {
        return this._bloomFilter.GetHashCode();
    }

    // Token: 0x06000018 RID: 24 RVA: 0x000022CF File Offset: 0x000004CF
    public virtual bool Equals(BeatmapLevelMask other)
    {
        return other != null && this._bloomFilter == other._bloomFilter;
    }

    // Token: 0x06000019 RID: 25 RVA: 0x000022E4 File Offset: 0x000004E4
    public override bool Equals(object obj)
    {
        BeatmapLevelMask other;
        return (other = (obj as BeatmapLevelMask)) != null && this.Equals(other);
    }

    // Token: 0x04000004 RID: 4
    protected const int kBitCount = 16384;

    // Token: 0x04000005 RID: 5
    protected const int kHashCount = 1;

    // Token: 0x04000006 RID: 6
    protected const int kHashBits = 14;

    // Token: 0x04000007 RID: 7
    protected readonly BitMaskSparse _bloomFilter = new BitMaskSparse(16384);

    // Token: 0x04000008 RID: 8
    protected const string kToStringPrefix = "[BeatmapLevelMask ";

    // Token: 0x04000009 RID: 9
    protected const string kToStringSuffix = "]";
}
