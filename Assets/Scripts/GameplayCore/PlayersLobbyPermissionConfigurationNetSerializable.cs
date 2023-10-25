using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using UnityEngine.Scripting;

// Token: 0x0200002B RID: 43
public class PlayersLobbyPermissionConfigurationNetSerializable : PoolableSerializable
{
    // Token: 0x17000030 RID: 48
    // (get) Token: 0x060000D8 RID: 216 RVA: 0x00005029 File Offset: 0x00003229
    public List<PlayerLobbyPermissionConfigurationNetSerializable> playersPermission
    {
        get
        {
            return this._playersPermission;
        }
    }

    // Token: 0x060000D9 RID: 217 RVA: 0x00005031 File Offset: 0x00003231
    public static PlayersLobbyPermissionConfigurationNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<PlayersLobbyPermissionConfigurationNetSerializable>();
    }

    // Token: 0x060000DA RID: 218 RVA: 0x00005038 File Offset: 0x00003238
    public virtual PlayersLobbyPermissionConfigurationNetSerializable Init(IEnumerable<PlayerLobbyPermissionConfigurationNetSerializable> playersPermission)
    {
        this._playersPermission.AddRange(playersPermission);
        return this;
    }

    // Token: 0x060000DB RID: 219 RVA: 0x00005047 File Offset: 0x00003247
    [Preserve]
    public PlayersLobbyPermissionConfigurationNetSerializable()
    {
    }

    // Token: 0x060000DC RID: 220 RVA: 0x0000505C File Offset: 0x0000325C
    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(this._playersPermission.Count);
        foreach (PlayerLobbyPermissionConfigurationNetSerializable playerLobbyPermissionConfigurationNetSerializable in this._playersPermission)
        {
            playerLobbyPermissionConfigurationNetSerializable.Serialize(writer);
        }
    }

    // Token: 0x060000DD RID: 221 RVA: 0x000050C0 File Offset: 0x000032C0
    public override void Deserialize(NetDataReader reader)
    {
        int @int = reader.GetInt();
        this._playersPermission.Clear();
        for (int i = 0; i < @int; i++)
        {
            PlayerLobbyPermissionConfigurationNetSerializable playerLobbyPermissionConfigurationNetSerializable = PoolableSerializable.Obtain<PlayerLobbyPermissionConfigurationNetSerializable>();
            playerLobbyPermissionConfigurationNetSerializable.Deserialize(reader);
            this._playersPermission.Add(playerLobbyPermissionConfigurationNetSerializable);
        }
    }

    // Token: 0x060000DE RID: 222 RVA: 0x00005104 File Offset: 0x00003304
    public override void Release()
    {
        for (int i = 0; i < this._playersPermission.Count; i++)
        {
            this._playersPermission[i].Release();
        }
        this._playersPermission.Clear();
        base.Release();
    }

    // Token: 0x060000DF RID: 223 RVA: 0x0000514C File Offset: 0x0000334C
    public override void Retain()
    {
        base.Retain();
        for (int i = 0; i < this._playersPermission.Count; i++)
        {
            this._playersPermission[i].Retain();
        }
    }

    // Token: 0x040000DA RID: 218
    protected readonly List<PlayerLobbyPermissionConfigurationNetSerializable> _playersPermission = new List<PlayerLobbyPermissionConfigurationNetSerializable>();
}
