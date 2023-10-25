using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using Newtonsoft.Json;

// Token: 0x0200003C RID: 60
[JsonConverter(typeof(SongPackMaskConverter))]
public readonly struct SongPackMask : INetImmutableSerializable<SongPackMask>, IEquatable<SongPackMask>
{
    // Token: 0x06000155 RID: 341 RVA: 0x00006DCD File Offset: 0x00004FCD
    public SongPackMask(string packId)
    {
        this._bloomFilter = packId.ToBloomFilter<BitMask128>(2, 13);
    }

    // Token: 0x06000156 RID: 342 RVA: 0x00006DDE File Offset: 0x00004FDE
    private SongPackMask(BitMask128 bloomFilter)
    {
        this._bloomFilter = bloomFilter;
    }

    // Token: 0x06000157 RID: 343 RVA: 0x00006DE7 File Offset: 0x00004FE7
    public SongPackMask(IEnumerable<string> packs)
    {
        this._bloomFilter = packs.ToBloomFilter<BitMask128>(2, 13);
    }

    // Token: 0x17000034 RID: 52
    // (get) Token: 0x06000158 RID: 344 RVA: 0x00006DF8 File Offset: 0x00004FF8
    public static SongPackMask all
    {
        get
        {
            return new SongPackMask(BitMask128.maxValue);
        }
    }

    // Token: 0x06000159 RID: 345 RVA: 0x00006E04 File Offset: 0x00005004
    public static SongPackMask operator |(SongPackMask a, SongPackMask b)
    {
        return new SongPackMask(a._bloomFilter | b._bloomFilter);
    }

    // Token: 0x0600015A RID: 346 RVA: 0x00006E1E File Offset: 0x0000501E
    public static SongPackMask operator &(SongPackMask a, SongPackMask b)
    {
        return new SongPackMask(a._bloomFilter & b._bloomFilter);
    }

    // Token: 0x0600015B RID: 347 RVA: 0x00006E38 File Offset: 0x00005038
    public static bool operator ==(SongPackMask a, SongPackMask b)
    {
        return a._bloomFilter == b._bloomFilter;
    }

    // Token: 0x0600015C RID: 348 RVA: 0x00006E4D File Offset: 0x0000504D
    public static bool operator !=(SongPackMask a, SongPackMask b)
    {
        return a._bloomFilter != b._bloomFilter;
    }

    // Token: 0x0600015D RID: 349 RVA: 0x00006E62 File Offset: 0x00005062
    public static implicit operator SongPackMask(string id)
    {
        return new SongPackMask(id);
    }

    // Token: 0x0600015E RID: 350 RVA: 0x00006E6A File Offset: 0x0000506A
    public bool Contains(SongPackMask other)
    {
        return (this & other) == other;
    }

    // Token: 0x0600015F RID: 351 RVA: 0x00006E7E File Offset: 0x0000507E
    public int DifferenceFrom(SongPackMask other)
    {
        return (this._bloomFilter ^ other._bloomFilter).NumberOfSetBits<BitMask128>();
    }

    // Token: 0x06000160 RID: 352 RVA: 0x00006E97 File Offset: 0x00005097
    public void Serialize(NetDataWriter writer)
    {
        this._bloomFilter.Serialize(writer);
    }

    // Token: 0x06000161 RID: 353 RVA: 0x00006EA5 File Offset: 0x000050A5
    public SongPackMask CreateFromSerializedData(NetDataReader reader)
    {
        return SongPackMask.Deserialize(reader);
    }

    // Token: 0x06000162 RID: 354 RVA: 0x00006EAD File Offset: 0x000050AD
    public static SongPackMask Deserialize(NetDataReader reader)
    {
        return new SongPackMask(BitMask128.Deserialize(reader));
    }

    // Token: 0x06000163 RID: 355 RVA: 0x00006EBA File Offset: 0x000050BA
    public override string ToString()
    {
        return "[SongPackMask " + this._bloomFilter + "]";
    }

    // Token: 0x06000164 RID: 356 RVA: 0x00006ED6 File Offset: 0x000050D6
    public string ToShortString()
    {
        return this._bloomFilter.ToShortString<BitMask128>();
    }

    // Token: 0x06000165 RID: 357 RVA: 0x00006EE3 File Offset: 0x000050E3
    public byte[] ToBytes()
    {
        return this._bloomFilter.ToBytes<BitMask128>();
    }

    // Token: 0x06000166 RID: 358 RVA: 0x00006EF0 File Offset: 0x000050F0
    public override int GetHashCode()
    {
        return this._bloomFilter.GetHashCode();
    }

    // Token: 0x06000167 RID: 359 RVA: 0x00006F03 File Offset: 0x00005103
    public bool Equals(SongPackMask other)
    {
        return this._bloomFilter == other._bloomFilter;
    }

    // Token: 0x06000168 RID: 360 RVA: 0x00006F18 File Offset: 0x00005118
    public override bool Equals(object obj)
    {
        if (obj is SongPackMask)
        {
            SongPackMask b = (SongPackMask)obj;
            return this == b;
        }
        return false;
    }

    // Token: 0x06000169 RID: 361 RVA: 0x00006F44 File Offset: 0x00005144
    public static bool TryParse(string stringSerializedMask, out SongPackMask songPackMask)
    {
        BitMask128 bloomFilter;
        if (IBitMaskUtil.TryParse<BitMask128>(stringSerializedMask, out bloomFilter))
        {
            songPackMask = new SongPackMask(bloomFilter);
            return true;
        }
        if (stringSerializedMask.StartsWith("[SongPackMask ") && stringSerializedMask.EndsWith("]") && IBitMaskUtil.TryParse<BitMask128>(stringSerializedMask, "[SongPackMask ".Length, stringSerializedMask.Length - "[SongPackMask ".Length - "]".Length, out bloomFilter))
        {
            songPackMask = new SongPackMask(bloomFilter);
            return true;
        }
        songPackMask = default(SongPackMask);
        return false;
    }

    // Token: 0x0600016A RID: 362 RVA: 0x00006FCC File Offset: 0x000051CC
    public static SongPackMask Parse(string stringSerializedMask)
    {
        SongPackMask result;
        if (SongPackMask.TryParse(stringSerializedMask, out result))
        {
            return result;
        }
        throw new ParseException("Invalid SongPackMask " + stringSerializedMask);
    }

    // Token: 0x0600016B RID: 363 RVA: 0x00006FF5 File Offset: 0x000051F5
    public static SongPackMask FromBytes(byte[] bytes, int offset = 0)
    {
        return new SongPackMask(IBitMaskUtil.FromBytes<BitMask128>(bytes, offset));
    }

    // Token: 0x04000125 RID: 293
    private const int kHashCount = 2;

    // Token: 0x04000126 RID: 294
    private const int kHashBits = 13;

    // Token: 0x04000127 RID: 295
    private readonly BitMask128 _bloomFilter;

    // Token: 0x04000128 RID: 296
    private const string kToStringPrefix = "[SongPackMask ";

    // Token: 0x04000129 RID: 297
    private const string kToStringSuffix = "]";
}
