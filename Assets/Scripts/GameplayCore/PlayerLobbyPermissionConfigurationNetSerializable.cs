using System;
using LiteNetLib.Utils;

// Token: 0x0200002A RID: 42
public class PlayerLobbyPermissionConfigurationNetSerializable : PoolableSerializable
{
    // Token: 0x1700002A RID: 42
    // (get) Token: 0x060000CD RID: 205 RVA: 0x00004F01 File Offset: 0x00003101
    public string userId
    {
        get
        {
            return this._userId;
        }
    }

    // Token: 0x1700002B RID: 43
    // (get) Token: 0x060000CE RID: 206 RVA: 0x00004F09 File Offset: 0x00003109
    public bool isServerOwner
    {
        get
        {
            return this._isServerOwner;
        }
    }

    // Token: 0x1700002C RID: 44
    // (get) Token: 0x060000CF RID: 207 RVA: 0x00004F11 File Offset: 0x00003111
    public bool hasRecommendBeatmapsPermission
    {
        get
        {
            return this._hasRecommendBeatmapsPermission;
        }
    }

    // Token: 0x1700002D RID: 45
    // (get) Token: 0x060000D0 RID: 208 RVA: 0x00004F19 File Offset: 0x00003119
    public bool hasRecommendGameplayModifiersPermission
    {
        get
        {
            return this._hasRecommendGameplayModifiersPermission;
        }
    }

    // Token: 0x1700002E RID: 46
    // (get) Token: 0x060000D1 RID: 209 RVA: 0x00004F21 File Offset: 0x00003121
    public bool hasKickVotePermission
    {
        get
        {
            return this._hasKickVotePermission;
        }
    }

    // Token: 0x1700002F RID: 47
    // (get) Token: 0x060000D2 RID: 210 RVA: 0x00004F29 File Offset: 0x00003129
    public bool hasInvitePermission
    {
        get
        {
            return this._hasInvitePermission;
        }
    }

    // Token: 0x060000D3 RID: 211 RVA: 0x00004F31 File Offset: 0x00003131
    public static PlayerLobbyPermissionConfigurationNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<PlayerLobbyPermissionConfigurationNetSerializable>();
    }

    // Token: 0x060000D4 RID: 212 RVA: 0x00004F38 File Offset: 0x00003138
    public virtual PlayerLobbyPermissionConfigurationNetSerializable Init(string userId, bool isServerOwner, bool hasRecommendBeatmapsPermission, bool hasRecommendGameplayModifiersPermission, bool hasKickVotePermission, bool hasInvitePermission)
    {
        this._userId = userId;
        this._isServerOwner = isServerOwner;
        this._hasRecommendBeatmapsPermission = hasRecommendBeatmapsPermission;
        this._hasRecommendGameplayModifiersPermission = hasRecommendGameplayModifiersPermission;
        this._hasKickVotePermission = hasKickVotePermission;
        this._hasInvitePermission = hasInvitePermission;
        return this;
    }

    // Token: 0x060000D6 RID: 214 RVA: 0x00004F68 File Offset: 0x00003168
    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(this._userId);
        int num = (this._isServerOwner ? 1 : 0) | (this._hasRecommendBeatmapsPermission ? 2 : 0) | (this._hasRecommendGameplayModifiersPermission ? 4 : 0) | (this._hasKickVotePermission ? 8 : 0) | (this._hasInvitePermission ? 16 : 0);
        writer.Put((byte)num);
    }

    // Token: 0x060000D7 RID: 215 RVA: 0x00004FCC File Offset: 0x000031CC
    public override void Deserialize(NetDataReader reader)
    {
        this._userId = reader.GetString();
        byte @byte = reader.GetByte();
        this._isServerOwner = ((@byte & 1) > 0);
        this._hasRecommendBeatmapsPermission = ((@byte & 2) > 0);
        this._hasRecommendGameplayModifiersPermission = ((@byte & 4) > 0);
        this._hasKickVotePermission = ((@byte & 8) > 0);
        this._hasInvitePermission = ((@byte & 16) > 0);
    }

    // Token: 0x040000D4 RID: 212
    protected string _userId;

    // Token: 0x040000D5 RID: 213
    protected bool _isServerOwner;

    // Token: 0x040000D6 RID: 214
    protected bool _hasRecommendBeatmapsPermission;

    // Token: 0x040000D7 RID: 215
    protected bool _hasRecommendGameplayModifiersPermission;

    // Token: 0x040000D8 RID: 216
    protected bool _hasKickVotePermission;

    // Token: 0x040000D9 RID: 217
    protected bool _hasInvitePermission;
}
