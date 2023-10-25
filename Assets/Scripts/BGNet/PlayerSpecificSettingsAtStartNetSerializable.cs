using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine.Scripting;

// Token: 0x0200006D RID: 109
[Preserve]
public class PlayerSpecificSettingsAtStartNetSerializable : INetSerializable
{
    // Token: 0x170000C4 RID: 196
    // (get) Token: 0x060004C0 RID: 1216 RVA: 0x0000CB8C File Offset: 0x0000AD8C
    public List<IConnectedPlayer> activePlayersAtGameStart
    {
        get
        {
            if (this._activePlayersAtGameStart == null && this.activePlayerSpecificSettingsAtGameStart != null)
            {
                List<IConnectedPlayer> list = new List<IConnectedPlayer>();
                for (int i = 0; i < this.activePlayerSpecificSettingsAtGameStart.Count; i++)
                {
                    PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable = this.activePlayerSpecificSettingsAtGameStart[i];
                    list.Add(new DisconnectedPlayer(playerSpecificSettingsNetSerializable.userId, playerSpecificSettingsNetSerializable.userName, i));
                }
                this._activePlayersAtGameStart = list;
            }
            return this._activePlayersAtGameStart;
        }
    }

    // Token: 0x170000C5 RID: 197
    // (get) Token: 0x060004C1 RID: 1217 RVA: 0x0000CBF7 File Offset: 0x0000ADF7
    // (set) Token: 0x060004C2 RID: 1218 RVA: 0x0000CBFF File Offset: 0x0000ADFF
    public IReadOnlyList<PlayerSpecificSettingsNetSerializable> activePlayerSpecificSettingsAtGameStart { get; private set; }

    // Token: 0x060004C3 RID: 1219 RVA: 0x000024B7 File Offset: 0x000006B7
    public PlayerSpecificSettingsAtStartNetSerializable()
    {
    }

    // Token: 0x060004C4 RID: 1220 RVA: 0x0000CC08 File Offset: 0x0000AE08
    public PlayerSpecificSettingsAtStartNetSerializable(List<PlayerSpecificSettingsNetSerializable> activePlayerSpecificSettingsAtGameStart)
    {
        this.activePlayerSpecificSettingsAtGameStart = activePlayerSpecificSettingsAtGameStart;
    }

    // Token: 0x060004C5 RID: 1221 RVA: 0x0000CC18 File Offset: 0x0000AE18
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this.activePlayerSpecificSettingsAtGameStart.Count);
        foreach (PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable in this.activePlayerSpecificSettingsAtGameStart)
        {
            playerSpecificSettingsNetSerializable.Serialize(writer);
        }
    }

    // Token: 0x060004C6 RID: 1222 RVA: 0x0000CC74 File Offset: 0x0000AE74
    public void Deserialize(NetDataReader reader)
    {
        int @int = reader.GetInt();
        List<PlayerSpecificSettingsNetSerializable> list = new List<PlayerSpecificSettingsNetSerializable>(@int);
        for (int i = 0; i < @int; i++)
        {
            PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable = new PlayerSpecificSettingsNetSerializable();
            playerSpecificSettingsNetSerializable.Deserialize(reader);
            list.Add(playerSpecificSettingsNetSerializable);
        }
        this.activePlayerSpecificSettingsAtGameStart = list;
    }

    // Token: 0x040001D1 RID: 465
    private List<IConnectedPlayer> _activePlayersAtGameStart;
}
