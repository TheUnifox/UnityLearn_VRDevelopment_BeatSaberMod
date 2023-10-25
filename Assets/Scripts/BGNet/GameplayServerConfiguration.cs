using System;
using LiteNetLib.Utils;
using Newtonsoft.Json;

// Token: 0x02000023 RID: 35
public readonly struct GameplayServerConfiguration : IEquatable<GameplayServerConfiguration>, INetImmutableSerializable<GameplayServerConfiguration>
{
    // Token: 0x06000166 RID: 358 RVA: 0x00006791 File Offset: 0x00004991
    [JsonConstructor]
    public GameplayServerConfiguration(int maxPlayerCount, DiscoveryPolicy discoveryPolicy, InvitePolicy invitePolicy, GameplayServerMode gameplayServerMode, SongSelectionMode songSelectionMode, GameplayServerControlSettings gameplayServerControlSettings)
    {
        this.maxPlayerCount = maxPlayerCount;
        this.discoveryPolicy = discoveryPolicy;
        this.invitePolicy = invitePolicy;
        this.gameplayServerMode = gameplayServerMode;
        this.songSelectionMode = songSelectionMode;
        this.gameplayServerControlSettings = gameplayServerControlSettings;
    }

    // Token: 0x06000167 RID: 359 RVA: 0x000067C0 File Offset: 0x000049C0
    public bool Equals(in GameplayServerConfiguration other)
    {
        return this.maxPlayerCount == other.maxPlayerCount && this.discoveryPolicy == other.discoveryPolicy && this.invitePolicy == other.invitePolicy && this.gameplayServerMode == other.gameplayServerMode && this.songSelectionMode == other.songSelectionMode && this.gameplayServerControlSettings == other.gameplayServerControlSettings;
    }

    // Token: 0x06000168 RID: 360 RVA: 0x00006823 File Offset: 0x00004A23
    public bool Equals(GameplayServerConfiguration other)
    {
        return this.Equals(other);
    }

    // Token: 0x06000169 RID: 361 RVA: 0x00006830 File Offset: 0x00004A30
    public override bool Equals(object obj)
    {
        if (obj is GameplayServerConfiguration)
        {
            GameplayServerConfiguration gameplayServerConfiguration = (GameplayServerConfiguration)obj;
            return this.Equals(gameplayServerConfiguration);
        }
        return false;
    }

    // Token: 0x0600016A RID: 362 RVA: 0x00006858 File Offset: 0x00004A58
    public override int GetHashCode()
    {
        return ((((this.maxPlayerCount * 397 ^ (int)this.discoveryPolicy) * 397 ^ (int)this.invitePolicy) * 397 ^ (int)this.gameplayServerMode) * 397 ^ (int)this.songSelectionMode) * 397 ^ (int)this.gameplayServerControlSettings;
    }

    // Token: 0x0600016B RID: 363 RVA: 0x000068AC File Offset: 0x00004AAC
    public static bool operator ==(in GameplayServerConfiguration a, in GameplayServerConfiguration b)
    {
        return a.Equals(b);
    }

    // Token: 0x0600016C RID: 364 RVA: 0x000068B5 File Offset: 0x00004AB5
    public static bool operator !=(in GameplayServerConfiguration a, in GameplayServerConfiguration b)
    {
        return !a.Equals(b);
    }

    // Token: 0x0600016D RID: 365 RVA: 0x000068C4 File Offset: 0x00004AC4
    public void Serialize(NetDataWriter writer)
    {
        writer.PutVarInt(this.maxPlayerCount);
        writer.PutVarInt((int)this.discoveryPolicy);
        writer.PutVarInt((int)this.invitePolicy);
        writer.PutVarInt((int)this.gameplayServerMode);
        writer.PutVarInt((int)this.songSelectionMode);
        writer.PutVarInt((int)this.gameplayServerControlSettings);
    }

    // Token: 0x0600016E RID: 366 RVA: 0x00006919 File Offset: 0x00004B19
    public GameplayServerConfiguration CreateFromSerializedData(NetDataReader reader)
    {
        return GameplayServerConfiguration.Deserialize(reader);
    }

    // Token: 0x0600016F RID: 367 RVA: 0x00006921 File Offset: 0x00004B21
    public static GameplayServerConfiguration Deserialize(NetDataReader reader)
    {
        return new GameplayServerConfiguration(reader.GetVarInt(), (DiscoveryPolicy)reader.GetVarInt(), (InvitePolicy)reader.GetVarInt(), (GameplayServerMode)reader.GetVarInt(), (SongSelectionMode)reader.GetVarInt(), (GameplayServerControlSettings)reader.GetVarInt());
    }

    // Token: 0x06000170 RID: 368 RVA: 0x0000694D File Offset: 0x00004B4D
    public GameplayServerConfiguration WithMaxPlayerCount(int maxPlayerCount)
    {
        return new GameplayServerConfiguration(maxPlayerCount, this.discoveryPolicy, this.invitePolicy, this.gameplayServerMode, this.songSelectionMode, this.gameplayServerControlSettings);
    }

    // Token: 0x040000DA RID: 218
    [JsonProperty("max_player_count")]
    public readonly int maxPlayerCount;

    // Token: 0x040000DB RID: 219
    [JsonProperty("discovery_policy")]
    public readonly DiscoveryPolicy discoveryPolicy;

    // Token: 0x040000DC RID: 220
    [JsonProperty("invite_policy")]
    public readonly InvitePolicy invitePolicy;

    // Token: 0x040000DD RID: 221
    [JsonProperty("gameplay_server_mode")]
    public readonly GameplayServerMode gameplayServerMode;

    // Token: 0x040000DE RID: 222
    [JsonProperty("song_selection_mode")]
    public readonly SongSelectionMode songSelectionMode;

    // Token: 0x040000DF RID: 223
    [JsonProperty("gameplay_server_control_settings")]
    public readonly GameplayServerControlSettings gameplayServerControlSettings;
}
