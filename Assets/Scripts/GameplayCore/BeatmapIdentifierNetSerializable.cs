using System;
using LiteNetLib.Utils;
using UnityEngine.Scripting;

// Token: 0x02000002 RID: 2
[Preserve]
public class BeatmapIdentifierNetSerializable : INetSerializable, IEquatable<BeatmapIdentifierNetSerializable>
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    // (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
    public string levelID { get; private set; }

    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
    // (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
    public string beatmapCharacteristicSerializedName { get; private set; }

    // Token: 0x17000003 RID: 3
    // (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
    // (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
    public BeatmapDifficulty difficulty { get; private set; }

    // Token: 0x06000007 RID: 7 RVA: 0x00002083 File Offset: 0x00000283
    public BeatmapIdentifierNetSerializable()
    {
    }

    // Token: 0x06000008 RID: 8 RVA: 0x0000208B File Offset: 0x0000028B
    public BeatmapIdentifierNetSerializable(string levelID, string beatmapCharacteristicSerializedName, BeatmapDifficulty difficulty)
    {
        this.levelID = levelID;
        this.beatmapCharacteristicSerializedName = beatmapCharacteristicSerializedName;
        this.difficulty = difficulty;
    }

    // Token: 0x06000009 RID: 9 RVA: 0x000020A8 File Offset: 0x000002A8
    void INetSerializable.Deserialize(NetDataReader reader)
    {
        this.levelID = reader.GetString();
        this.beatmapCharacteristicSerializedName = reader.GetString();
        this.difficulty = (BeatmapDifficulty)reader.GetUInt();
    }

    // Token: 0x0600000A RID: 10 RVA: 0x000020CE File Offset: 0x000002CE
    void INetSerializable.Serialize(NetDataWriter writer)
    {
        writer.Put(this.levelID);
        writer.Put(this.beatmapCharacteristicSerializedName);
        writer.Put((uint)this.difficulty);
    }

    // Token: 0x0600000B RID: 11 RVA: 0x000020F4 File Offset: 0x000002F4
    public virtual bool Equals(BeatmapIdentifierNetSerializable other)
    {
        return other != null && (this == other || (this.levelID == other.levelID && this.beatmapCharacteristicSerializedName == other.beatmapCharacteristicSerializedName && this.difficulty == other.difficulty));
    }

    // Token: 0x0600000C RID: 12 RVA: 0x00002142 File Offset: 0x00000342
    public override bool Equals(object obj)
    {
        return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((BeatmapIdentifierNetSerializable)obj)));
    }

    // Token: 0x0600000D RID: 13 RVA: 0x00002170 File Offset: 0x00000370
    public override int GetHashCode()
    {
        return (this.levelID.GetHashCode() * 37 ^ this.beatmapCharacteristicSerializedName.GetHashCode()) * 37 ^ (int)this.difficulty;
    }

    // Token: 0x0600000E RID: 14 RVA: 0x00002196 File Offset: 0x00000396
    public override string ToString()
    {
        return string.Format("[BeatmapIdentifierNetSerializable levelID: {0}, difficulty: {1}, characteristic: {2}]", this.levelID, this.difficulty, this.beatmapCharacteristicSerializedName);
    }
}
