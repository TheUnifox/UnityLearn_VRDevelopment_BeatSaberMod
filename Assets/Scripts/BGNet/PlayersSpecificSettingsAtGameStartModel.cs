using System;
using System.Collections.Generic;
using System.Linq;
using BGNet.Logging;

// Token: 0x02000070 RID: 112
public class PlayersSpecificSettingsAtGameStartModel
{
    // Token: 0x170000C6 RID: 198
    // (get) Token: 0x060004DC RID: 1244 RVA: 0x0000CF8F File Offset: 0x0000B18F
    public List<IConnectedPlayer> playersAtGameStart
    {
        get
        {
            return this.playersAtGameStartNetSerializable.activePlayersAtGameStart;
        }
    }

    // Token: 0x170000C7 RID: 199
    // (get) Token: 0x060004DD RID: 1245 RVA: 0x0000CF9C File Offset: 0x0000B19C
    // (set) Token: 0x060004DE RID: 1246 RVA: 0x0000CFA4 File Offset: 0x0000B1A4
    public PlayerSpecificSettingsNetSerializable localPlayerSpecificSettings { get; private set; }

    // Token: 0x170000C8 RID: 200
    // (get) Token: 0x060004DF RID: 1247 RVA: 0x0000CFAD File Offset: 0x0000B1AD
    // (set) Token: 0x060004E0 RID: 1248 RVA: 0x0000CFB5 File Offset: 0x0000B1B5
    public PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable { get; private set; }

    // Token: 0x060004E1 RID: 1249 RVA: 0x0000CFBE File Offset: 0x0000B1BE
    public PlayersSpecificSettingsAtGameStartModel(IMultiplayerSessionManager multiplayerSessionManager, PlayerSpecificSettingsNetSerializable localPlayerSpecificSettings)
    {
        this._multiplayerSessionManager = multiplayerSessionManager;
        this.localPlayerSpecificSettings = localPlayerSpecificSettings;
        this.playersAtGameStartNetSerializable = new PlayerSpecificSettingsAtStartNetSerializable();
    }

    // Token: 0x060004E2 RID: 1250 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
    public PlayerSpecificSettingsNetSerializable GetPlayerSpecificSettingsForUserId(string userId)
    {
        foreach (PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable in this.playersAtGameStartNetSerializable.activePlayerSpecificSettingsAtGameStart)
        {
            if (playerSpecificSettingsNetSerializable.userId == userId)
            {
                return playerSpecificSettingsNetSerializable;
            }
        }
        Debug.LogError("Unable to find player specific settings requested for userId \"" + userId + "\"");
        return null;
    }

    // Token: 0x060004E3 RID: 1251 RVA: 0x0000D058 File Offset: 0x0000B258
    public void SaveFromNetSerializable(PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable)
    {
        for (int i = 0; i < playersAtGameStartNetSerializable.activePlayersAtGameStart.Count; i++)
        {
            IConnectedPlayer player = playersAtGameStartNetSerializable.activePlayersAtGameStart[i];
            if (player.userId == this._multiplayerSessionManager.localPlayer.userId)
            {
                playersAtGameStartNetSerializable.activePlayersAtGameStart[i] = this._multiplayerSessionManager.localPlayer;
            }
            else
            {
                IConnectedPlayer connectedPlayer = this._multiplayerSessionManager.connectedPlayers.FirstOrDefault((IConnectedPlayer p) => p.userId == player.userId);
                if (connectedPlayer != null)
                {
                    playersAtGameStartNetSerializable.activePlayersAtGameStart[i] = connectedPlayer;
                }
            }
        }
        this.playersAtGameStartNetSerializable = playersAtGameStartNetSerializable;
    }

    // Token: 0x040001DE RID: 478
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;
}
