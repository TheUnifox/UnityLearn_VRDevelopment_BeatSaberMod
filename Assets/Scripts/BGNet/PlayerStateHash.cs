using System;
using System.Collections.Generic;
using LiteNetLib.Utils;

// Token: 0x0200006F RID: 111
public readonly struct PlayerStateHash : INetImmutableSerializable<PlayerStateHash>, IEquatable<PlayerStateHash>
{
    // Token: 0x060004CB RID: 1227 RVA: 0x0000CDD9 File Offset: 0x0000AFD9
    private PlayerStateHash(BitMask128 bloomFilter)
    {
        this._bloomFilter = bloomFilter;
    }

    // Token: 0x060004CC RID: 1228 RVA: 0x0000CDE2 File Offset: 0x0000AFE2
    public PlayerStateHash(string state)
    {
        this._bloomFilter = state.ToBloomFilter<BitMask128>(3, 8);
    }

    // Token: 0x060004CD RID: 1229 RVA: 0x0000CDF2 File Offset: 0x0000AFF2
    public PlayerStateHash(HashSet<string> stateHashSet)
    {
        this._bloomFilter = stateHashSet.ToBloomFilter<BitMask128>(3, 8);
    }

    // Token: 0x060004CE RID: 1230 RVA: 0x0000CE02 File Offset: 0x0000B002
    public bool Contains(string state)
    {
        return this._bloomFilter.ContainsBloomFilterEntry(state, 3, 8);
    }

    // Token: 0x060004CF RID: 1231 RVA: 0x0000CE12 File Offset: 0x0000B012
    public PlayerStateHash AddState(string state)
    {
        return new PlayerStateHash(this._bloomFilter.AddBloomFilterEntry(state, 3, 8));
    }

    // Token: 0x060004D0 RID: 1232 RVA: 0x0000CE27 File Offset: 0x0000B027
    public void Serialize(NetDataWriter writer)
    {
        this._bloomFilter.Serialize(writer);
    }

    // Token: 0x060004D1 RID: 1233 RVA: 0x0000CE35 File Offset: 0x0000B035
    public PlayerStateHash CreateFromSerializedData(NetDataReader reader)
    {
        return PlayerStateHash.Deserialize(reader);
    }

    // Token: 0x060004D2 RID: 1234 RVA: 0x0000CE3D File Offset: 0x0000B03D
    public static PlayerStateHash Deserialize(NetDataReader reader)
    {
        return new PlayerStateHash(BitMask128.Deserialize(reader));
    }

    // Token: 0x060004D3 RID: 1235 RVA: 0x0000CE4A File Offset: 0x0000B04A
    public override string ToString()
    {
        return "[PlayerStateMask " + this._bloomFilter + "]";
    }

    // Token: 0x060004D4 RID: 1236 RVA: 0x0000CE66 File Offset: 0x0000B066
    public string ToShortString()
    {
        return this._bloomFilter.ToShortString<BitMask128>();
    }

    // Token: 0x060004D5 RID: 1237 RVA: 0x0000CE73 File Offset: 0x0000B073
    public byte[] ToBytes()
    {
        return this._bloomFilter.ToBytes<BitMask128>();
    }

    // Token: 0x060004D6 RID: 1238 RVA: 0x0000CE80 File Offset: 0x0000B080
    public override int GetHashCode()
    {
        return this._bloomFilter.GetHashCode();
    }

    // Token: 0x060004D7 RID: 1239 RVA: 0x0000CE93 File Offset: 0x0000B093
    public bool Equals(PlayerStateHash other)
    {
        return this._bloomFilter == other._bloomFilter;
    }

    // Token: 0x060004D8 RID: 1240 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
    public override bool Equals(object obj)
    {
        if (obj is PlayerStateHash)
        {
            PlayerStateHash other = (PlayerStateHash)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x060004D9 RID: 1241 RVA: 0x0000CED0 File Offset: 0x0000B0D0
    public static bool TryParse(string stringSerializedMask, out PlayerStateHash playerStateHash)
    {
        BitMask128 bloomFilter;
        if (IBitMaskUtil.TryParse<BitMask128>(stringSerializedMask, out bloomFilter))
        {
            playerStateHash = new PlayerStateHash(bloomFilter);
            return true;
        }
        if (stringSerializedMask.StartsWith("[PlayerStateMask ") && stringSerializedMask.EndsWith("]") && IBitMaskUtil.TryParse<BitMask128>(stringSerializedMask, "[PlayerStateMask ".Length, stringSerializedMask.Length - "[PlayerStateMask ".Length - "]".Length, out bloomFilter))
        {
            playerStateHash = new PlayerStateHash(bloomFilter);
            return true;
        }
        playerStateHash = default(PlayerStateHash);
        return false;
    }

    // Token: 0x060004DA RID: 1242 RVA: 0x0000CF58 File Offset: 0x0000B158
    public static PlayerStateHash Parse(string stringSerializedMask)
    {
        PlayerStateHash result;
        if (PlayerStateHash.TryParse(stringSerializedMask, out result))
        {
            return result;
        }
        throw new ParseException("Invalid PlayerStateMask " + stringSerializedMask);
    }

    // Token: 0x060004DB RID: 1243 RVA: 0x0000CF81 File Offset: 0x0000B181
    public static PlayerStateHash FromBytes(byte[] bytes, int offset = 0)
    {
        return new PlayerStateHash(IBitMaskUtil.FromBytes<BitMask128>(bytes, offset));
    }

    // Token: 0x040001D9 RID: 473
    private readonly BitMask128 _bloomFilter;

    // Token: 0x040001DA RID: 474
    private const string kToStringPrefix = "[PlayerStateMask ";

    // Token: 0x040001DB RID: 475
    private const string kToStringSuffix = "]";
}
