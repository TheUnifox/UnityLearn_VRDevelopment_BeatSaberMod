using System;
using LiteNetLib.Utils;
using Newtonsoft.Json;

// Token: 0x02000008 RID: 8
public readonly struct BeatmapLevelSelectionMask : IEquatable<BeatmapLevelSelectionMask>
{
    // Token: 0x06000022 RID: 34 RVA: 0x000024D6 File Offset: 0x000006D6
    [JsonConstructor]
    public BeatmapLevelSelectionMask(BeatmapDifficultyMask difficulties, GameplayModifierMask modifiers, SongPackMask songPacks)
    {
        this.difficulties = difficulties;
        this.modifiers = modifiers;
        this.songPacks = songPacks;
    }

    // Token: 0x06000023 RID: 35 RVA: 0x000024F0 File Offset: 0x000006F0
    public override bool Equals(object obj)
    {
        if (obj is BeatmapLevelSelectionMask)
        {
            BeatmapLevelSelectionMask other = (BeatmapLevelSelectionMask)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x06000024 RID: 36 RVA: 0x00002518 File Offset: 0x00000718
    public bool Equals(BeatmapLevelSelectionMask other)
    {
        return this.difficulties.Equals(other.difficulties) && this.modifiers.Equals(other.modifiers) && this.songPacks.Equals(other.songPacks);
    }

    // Token: 0x06000025 RID: 37 RVA: 0x00002574 File Offset: 0x00000774
    public override int GetHashCode()
    {
        return (int)((GameplayModifierMask)((int)this.modifiers << 5) | (GameplayModifierMask)this.difficulties) ^ this.songPacks.GetHashCode();
    }

    // Token: 0x06000026 RID: 38 RVA: 0x00002597 File Offset: 0x00000797
    public void Serialize(NetDataWriter writer, uint version = 0U)
    {
        writer.Put((byte)this.difficulties);
        writer.Put((uint)this.modifiers);
        this.songPacks.Serialize(writer);
    }

    // Token: 0x06000027 RID: 39 RVA: 0x000025BD File Offset: 0x000007BD
    public static BeatmapLevelSelectionMask Deserialize(NetDataReader reader, uint version = 0U)
    {
        return new BeatmapLevelSelectionMask((BeatmapDifficultyMask)reader.GetByte(), (GameplayModifierMask)reader.GetUInt(), SongPackMask.Deserialize(reader));
    }

    // Token: 0x06000028 RID: 40 RVA: 0x000025D7 File Offset: 0x000007D7
    public static bool operator ==(BeatmapLevelSelectionMask l, BeatmapLevelSelectionMask r)
    {
        return l.difficulties == r.difficulties && l.modifiers == r.modifiers && l.songPacks == r.songPacks;
    }

    // Token: 0x06000029 RID: 41 RVA: 0x00002608 File Offset: 0x00000808
    public static bool operator !=(BeatmapLevelSelectionMask l, BeatmapLevelSelectionMask r)
    {
        return l.difficulties != r.difficulties || l.modifiers != r.modifiers || l.songPacks != r.songPacks;
    }

    // Token: 0x0400000C RID: 12
    [JsonProperty("difficulties")]
    public readonly BeatmapDifficultyMask difficulties;

    // Token: 0x0400000D RID: 13
    [JsonProperty("modifiers")]
    public readonly GameplayModifierMask modifiers;

    // Token: 0x0400000E RID: 14
    [JsonProperty("song_packs")]
    public readonly SongPackMask songPacks;
}
