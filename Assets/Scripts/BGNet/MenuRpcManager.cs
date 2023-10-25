using System;

// Token: 0x02000056 RID: 86
public class MenuRpcManager : IMenuRpcManager, IDisposable
{
    // Token: 0x170000A7 RID: 167
    // (get) Token: 0x06000354 RID: 852 RVA: 0x000080E9 File Offset: 0x000062E9
    public IMultiplayerSessionManager multiplayerSessionManager
    {
        get
        {
            return this._multiplayerSessionManager;
        }
    }

    // Token: 0x170000A8 RID: 168
    // (get) Token: 0x06000355 RID: 853 RVA: 0x000080F1 File Offset: 0x000062F1
    // (set) Token: 0x06000356 RID: 854 RVA: 0x00008103 File Offset: 0x00006303
    public bool enabled
    {
        get
        {
            return this._multiplayerSessionManager.LocalPlayerHasState("in_menu");
        }
        set
        {
            this._multiplayerSessionManager.SetLocalPlayerState("in_menu", value);
        }
    }

    // Token: 0x170000A9 RID: 169
    // (get) Token: 0x06000357 RID: 855 RVA: 0x00008118 File Offset: 0x00006318
    public bool enabledForAllPlayers
    {
        get
        {
            for (int i = 0; i < this._multiplayerSessionManager.connectedPlayerCount; i++)
            {
                if (!this._multiplayerSessionManager.GetConnectedPlayer(i).HasState("in_menu"))
                {
                    return false;
                }
            }
            return true;
        }
    }

    // Token: 0x170000AA RID: 170
    // (get) Token: 0x06000358 RID: 856 RVA: 0x00008156 File Offset: 0x00006356
    public float syncTime
    {
        get
        {
            return this._multiplayerSessionManager.syncTime;
        }
    }

    // Token: 0x06000359 RID: 857 RVA: 0x00008164 File Offset: 0x00006364
    public MenuRpcManager(IMultiplayerSessionManager multiplayerSessionManager)
    {
        this._multiplayerSessionManager = multiplayerSessionManager;
        this._rpcHandler = new RpcHandler<MenuRpcManager.RpcType>(this._multiplayerSessionManager, MultiplayerSessionManager.MessageType.MenuRpc);
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetPlayersPermissionConfigurationRpc>(MenuRpcManager.RpcType.GetPermissionConfiguration, new Action<string>(this.InvokeGetPlayersPermissionConfiguration));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetPlayersPermissionConfigurationRpc, PlayersLobbyPermissionConfigurationNetSerializable>(MenuRpcManager.RpcType.SetPermissionConfiguration, new Action<string, PlayersLobbyPermissionConfigurationNetSerializable>(this.InvokeSetPlayersPermissionConfiguration));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetSelectedBeatmapRpc, BeatmapIdentifierNetSerializable>(MenuRpcManager.RpcType.SetSelectedBeatmap, new Action<string, BeatmapIdentifierNetSerializable>(this.InvokeSetSelectedBeatmap));
        this._rpcHandler.RegisterCallback<MenuRpcManager.ClearSelectedBeatmapRpc>(MenuRpcManager.RpcType.ClearSelectedBeatmap, new Action<string>(this.InvokeClearSelectedBeatmap));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetSelectedBeatmapRpc>(MenuRpcManager.RpcType.GetSelectedBeatmap, new Action<string>(this.InvokeGetSelectedBeatmapRpc));
        this._rpcHandler.RegisterCallback<MenuRpcManager.RecommendBeatmapRpc, BeatmapIdentifierNetSerializable>(MenuRpcManager.RpcType.RecommendBeatmap, new Action<string, BeatmapIdentifierNetSerializable>(this.InvokeRecommendBeatmap));
        this._rpcHandler.RegisterCallback<MenuRpcManager.ClearRecommendedBeatmapRpc>(MenuRpcManager.RpcType.ClearRecommendedBeatmap, new Action<string>(this.InvokeClearRecommendedBeatmap));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetRecommendedBeatmapRpc>(MenuRpcManager.RpcType.GetRecommendedBeatmap, new Action<string>(this.InvokeGetRecommendedBeatmap));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetSelectedGameplayModifiersRpc, GameplayModifiers>(MenuRpcManager.RpcType.SetSelectedGameplayModifiers, new Action<string, GameplayModifiers>(this.InvokeSetSelectedGameplayModifiers));
        this._rpcHandler.RegisterCallback<MenuRpcManager.ClearSelectedGameplayModifiersRpc>(MenuRpcManager.RpcType.ClearSelectedGameplayModifiers, new Action<string>(this.InvokeClearSelectedGameplayModifiers));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetSelectedGameplayModifiersRpc>(MenuRpcManager.RpcType.GetSelectedGameplayModifiers, new Action<string>(this.InvokeGetSelectedGameplayModifiers));
        this._rpcHandler.RegisterCallback<MenuRpcManager.RecommendGameplayModifiersRpc, GameplayModifiers>(MenuRpcManager.RpcType.RecommendGameplayModifiers, new Action<string, GameplayModifiers>(this.InvokeRecommendGameplayModifiers));
        this._rpcHandler.RegisterCallback<MenuRpcManager.ClearRecommendedGameplayModifiersRpc>(MenuRpcManager.RpcType.ClearRecommendedGameplayModifiers, new Action<string>(this.InvokeClearRecommendedGameplayModifiers));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetRecommendedGameplayModifiersRpc>(MenuRpcManager.RpcType.GetRecommendedGameplayModifiers, new Action<string>(this.InvokeGetRecommendedGameplayModifiers));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetIsStartButtonEnabledRpc>(MenuRpcManager.RpcType.GetIsStartButtonEnabled, new Action<string>(this.InvokeGetIsStartButtonEnabled));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetIsStartButtonEnabledRpc, CannotStartGameReason>(MenuRpcManager.RpcType.SetIsStartButtonEnabled, new Action<string, CannotStartGameReason>(this.InvokeSetIsStartButtonEnabled));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetPlayersMissingEntitlementsToLevelRpc, PlayersMissingEntitlementsNetSerializable>(MenuRpcManager.RpcType.SetPlayersMissingEntitlementsToLevel, new Action<string, PlayersMissingEntitlementsNetSerializable>(this.InvokeSetPlayersMissingEntitlementsToLevelRpc));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetIsEntitledToLevelRpc, string>(MenuRpcManager.RpcType.GetIsEntitledToLevel, new Action<string, string>(this.InvokeGetIsEntitledToLevel));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetIsEntitledToLevelRpc, string, int>(MenuRpcManager.RpcType.SetIsEntitledToLevel, new Action<string, string, int>(this.InvokeSetIsEntitledToLevel));
        this._rpcHandler.RegisterCallback<MenuRpcManager.InvalidateLevelEntitlementStatusesRpc>(MenuRpcManager.RpcType.InvalidateLevelEntitlementStatuses, new Action<string>(this.InvokeLevelEntitlementStatusesInvalidated));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SelectLevelPackRpc, string>(MenuRpcManager.RpcType.SelectLevelPack, new Action<string, string>(this.InvokeOnSelectedLevelPackEvent));
        this._rpcHandler.RegisterCallback<MenuRpcManager.LevelLoadErrorRpc, string>(MenuRpcManager.RpcType.LevelLoadError, new Action<string, string>(this.InvokeLevelLoadError));
        this._rpcHandler.RegisterCallback<MenuRpcManager.LevelLoadSuccessRpc, string>(MenuRpcManager.RpcType.LevelLoadSuccess, new Action<string, string>(this.InvokeLevelLoadSuccess));
        this._rpcHandler.RegisterCallback<MenuRpcManager.StartLevelRpc, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(MenuRpcManager.RpcType.StartLevel, new Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(this.InvokeStartLevel));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetStartedLevelRpc>(MenuRpcManager.RpcType.GetStartedLevel, new Action<string>(this.InvokeGetStartedLevel));
        this._rpcHandler.RegisterCallback<MenuRpcManager.CancelLevelStartRpc>(MenuRpcManager.RpcType.CancelLevelStart, new Action<string>(this.InvokeCancelLevelStart));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetMultiplayerGameStateRpc>(MenuRpcManager.RpcType.GetMultiplayerGameState, new Action<string>(this.InvokeGetMultiplayerGameState));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetMultiplayerGameStateRpc, MultiplayerGameState>(MenuRpcManager.RpcType.SetMultiplayerGameState, new Action<string, MultiplayerGameState>(this.InvokeSetMultiplayerGameState));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetIsReadyRpc>(MenuRpcManager.RpcType.GetIsReady, new Action<string>(this.InvokeGetIsReady));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetIsReadyRpc, bool>(MenuRpcManager.RpcType.SetIsReady, new Action<string, bool>(this.InvokeSetIsReady));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetStartGameTimeRpc, float>(MenuRpcManager.RpcType.SetStartGameTime, new Action<string, float>(this.InvokeSetStartGameCountdown));
        this._rpcHandler.RegisterCallback<MenuRpcManager.CancelStartGameTimeRpc>(MenuRpcManager.RpcType.CancelStartGameTime, new Action<string>(this.InvokeCancelStartGameCountdown));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetIsInLobbyRpc>(MenuRpcManager.RpcType.GetIsInLobby, new Action<string>(this.InvokeGetIsInLobby));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetIsInLobbyRpc, bool>(MenuRpcManager.RpcType.SetIsInLobby, new Action<string, bool>(this.InvokeSetIsInLobby));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetCountdownEndTimeRpc>(MenuRpcManager.RpcType.GetCountdownEndTime, new Action<string>(this.InvokeGetCountdownEndTime));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetCountdownEndTimeRpc, float>(MenuRpcManager.RpcType.SetCountdownEndTime, new Action<string, float>(this.InvokeSetCountdownEndTime));
        this._rpcHandler.RegisterCallback<MenuRpcManager.CancelCountdownRpc>(MenuRpcManager.RpcType.CancelCountdown, new Action<string>(this.InvokeCancelCountdown));
        this._rpcHandler.RegisterCallback<MenuRpcManager.GetOwnedSongPacksRpc>(MenuRpcManager.RpcType.GetOwnedSongPacks, new Action<string>(this.InvokeGetOwnedSongPacks));
        this._rpcHandler.RegisterCallback<MenuRpcManager.SetOwnedSongPacksRpc, SongPackMask>(MenuRpcManager.RpcType.SetOwnedSongPacks, new Action<string, SongPackMask>(this.InvokeSetOwnedSongPacks));
        this._rpcHandler.RegisterCallback<MenuRpcManager.RequestKickPlayerRpc, string>(MenuRpcManager.RpcType.RequestKickPlayer, new Action<string, string>(this.InvokeKickPlayer));
        this.enabled = true;
    }

    // Token: 0x0600035A RID: 858 RVA: 0x00008576 File Offset: 0x00006776
    public void Dispose()
    {
        this.enabled = false;
        this._rpcHandler.Destroy();
    }

    // Token: 0x0600035B RID: 859 RVA: 0x0000858A File Offset: 0x0000678A
    public bool EnabledForPlayer(IConnectedPlayer player)
    {
        return player.HasState("in_menu");
    }

    // Token: 0x14000079 RID: 121
    // (add) Token: 0x0600035C RID: 860 RVA: 0x00008598 File Offset: 0x00006798
    // (remove) Token: 0x0600035D RID: 861 RVA: 0x000085D0 File Offset: 0x000067D0
    public event Action<string> getPlayersPermissionConfigurationEvent;

    // Token: 0x0600035E RID: 862 RVA: 0x00008605 File Offset: 0x00006805
    public void GetPlayersPermissionConfiguration()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetPlayersPermissionConfigurationRpc>();
    }

    // Token: 0x0600035F RID: 863 RVA: 0x00008612 File Offset: 0x00006812
    private void InvokeGetPlayersPermissionConfiguration(string userId)
    {
        Action<string> action = this.getPlayersPermissionConfigurationEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400007A RID: 122
    // (add) Token: 0x06000360 RID: 864 RVA: 0x00008628 File Offset: 0x00006828
    // (remove) Token: 0x06000361 RID: 865 RVA: 0x00008660 File Offset: 0x00006860
    public event Action<string, PlayersLobbyPermissionConfigurationNetSerializable> setPlayersPermissionConfigurationEvent;

    // Token: 0x06000362 RID: 866 RVA: 0x00008695 File Offset: 0x00006895
    public void SetPlayersPermissionConfiguration(PlayersLobbyPermissionConfigurationNetSerializable playersPermissionConfiguration)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetPlayersPermissionConfigurationRpc, PlayersLobbyPermissionConfigurationNetSerializable>(playersPermissionConfiguration);
    }

    // Token: 0x06000363 RID: 867 RVA: 0x000086A3 File Offset: 0x000068A3
    private void InvokeSetPlayersPermissionConfiguration(string userId, PlayersLobbyPermissionConfigurationNetSerializable playersPermissionConfiguration)
    {
        Action<string, PlayersLobbyPermissionConfigurationNetSerializable> action = this.setPlayersPermissionConfigurationEvent;
        if (action == null)
        {
            return;
        }
        action(userId, playersPermissionConfiguration);
    }

    // Token: 0x1400007B RID: 123
    // (add) Token: 0x06000364 RID: 868 RVA: 0x000086B8 File Offset: 0x000068B8
    // (remove) Token: 0x06000365 RID: 869 RVA: 0x000086F0 File Offset: 0x000068F0
    public event Action<string, PlayersMissingEntitlementsNetSerializable> setPlayersMissingEntitlementsToLevelEvent;

    // Token: 0x06000366 RID: 870 RVA: 0x00008725 File Offset: 0x00006925
    public void SetPlayersMissingEntitlementsToLevel(PlayersMissingEntitlementsNetSerializable playersMissingEntitlements)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetPlayersMissingEntitlementsToLevelRpc, PlayersMissingEntitlementsNetSerializable>(playersMissingEntitlements);
    }

    // Token: 0x06000367 RID: 871 RVA: 0x00008733 File Offset: 0x00006933
    private void InvokeSetPlayersMissingEntitlementsToLevelRpc(string userId, PlayersMissingEntitlementsNetSerializable playersMissingEntitlements)
    {
        Action<string, PlayersMissingEntitlementsNetSerializable> action = this.setPlayersMissingEntitlementsToLevelEvent;
        if (action == null)
        {
            return;
        }
        action(userId, playersMissingEntitlements);
    }

    // Token: 0x1400007C RID: 124
    // (add) Token: 0x06000368 RID: 872 RVA: 0x00008748 File Offset: 0x00006948
    // (remove) Token: 0x06000369 RID: 873 RVA: 0x00008780 File Offset: 0x00006980
    public event Action<string, string> getIsEntitledToLevelEvent;

    // Token: 0x0600036A RID: 874 RVA: 0x000087B5 File Offset: 0x000069B5
    public void GetIsEntitledToLevel(string levelId)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetIsEntitledToLevelRpc, string>(levelId);
    }

    // Token: 0x0600036B RID: 875 RVA: 0x000087C3 File Offset: 0x000069C3
    private void InvokeGetIsEntitledToLevel(string userId, string levelId)
    {
        Action<string, string> action = this.getIsEntitledToLevelEvent;
        if (action == null)
        {
            return;
        }
        action(userId, levelId);
    }

    // Token: 0x1400007D RID: 125
    // (add) Token: 0x0600036C RID: 876 RVA: 0x000087D8 File Offset: 0x000069D8
    // (remove) Token: 0x0600036D RID: 877 RVA: 0x00008810 File Offset: 0x00006A10
    public event Action<string, string, EntitlementsStatus> setIsEntitledToLevelEvent;

    // Token: 0x0600036E RID: 878 RVA: 0x00008845 File Offset: 0x00006A45
    public void SetIsEntitledToLevel(string levelId, EntitlementsStatus entitlementStatus)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetIsEntitledToLevelRpc, string, int>(levelId, (int)entitlementStatus);
    }

    // Token: 0x0600036F RID: 879 RVA: 0x00008854 File Offset: 0x00006A54
    private void InvokeSetIsEntitledToLevel(string userId, string levelId, int entitlementStatus)
    {
        Action<string, string, EntitlementsStatus> action = this.setIsEntitledToLevelEvent;
        if (action == null)
        {
            return;
        }
        action(userId, levelId, (EntitlementsStatus)entitlementStatus);
    }

    // Token: 0x1400007E RID: 126
    // (add) Token: 0x06000370 RID: 880 RVA: 0x0000886C File Offset: 0x00006A6C
    // (remove) Token: 0x06000371 RID: 881 RVA: 0x000088A4 File Offset: 0x00006AA4
    public event Action<string> levelEntitlementStatusesInvalidatedEvent;

    // Token: 0x06000372 RID: 882 RVA: 0x000088D9 File Offset: 0x00006AD9
    public void InvalidateLevelEntitlementStatuses()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.InvalidateLevelEntitlementStatusesRpc>();
    }

    // Token: 0x06000373 RID: 883 RVA: 0x000088E6 File Offset: 0x00006AE6
    public void InvokeLevelEntitlementStatusesInvalidated(string userId)
    {
        Action<string> action = this.levelEntitlementStatusesInvalidatedEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400007F RID: 127
    // (add) Token: 0x06000374 RID: 884 RVA: 0x000088FC File Offset: 0x00006AFC
    // (remove) Token: 0x06000375 RID: 885 RVA: 0x00008934 File Offset: 0x00006B34
    public event Action<string, string> selectedLevelPackEvent;

    // Token: 0x06000376 RID: 886 RVA: 0x00008969 File Offset: 0x00006B69
    public void SelectLevelPack(string levelPackId)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SelectLevelPackRpc, string>(levelPackId);
    }

    // Token: 0x06000377 RID: 887 RVA: 0x00008977 File Offset: 0x00006B77
    private void InvokeOnSelectedLevelPackEvent(string userId, string levelPackId)
    {
        Action<string, string> action = this.selectedLevelPackEvent;
        if (action == null)
        {
            return;
        }
        action(userId, levelPackId);
    }

    // Token: 0x14000080 RID: 128
    // (add) Token: 0x06000378 RID: 888 RVA: 0x0000898C File Offset: 0x00006B8C
    // (remove) Token: 0x06000379 RID: 889 RVA: 0x000089C4 File Offset: 0x00006BC4
    public event Action<string, BeatmapIdentifierNetSerializable> setSelectedBeatmapEvent;

    // Token: 0x0600037A RID: 890 RVA: 0x000089F9 File Offset: 0x00006BF9
    public void SetSelectedBeatmap(BeatmapIdentifierNetSerializable identifier)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetSelectedBeatmapRpc, BeatmapIdentifierNetSerializable>(identifier);
    }

    // Token: 0x0600037B RID: 891 RVA: 0x00008A07 File Offset: 0x00006C07
    private void InvokeSetSelectedBeatmap(string userId, BeatmapIdentifierNetSerializable identifier)
    {
        Action<string, BeatmapIdentifierNetSerializable> action = this.setSelectedBeatmapEvent;
        if (action == null)
        {
            return;
        }
        action(userId, identifier);
    }

    // Token: 0x14000081 RID: 129
    // (add) Token: 0x0600037C RID: 892 RVA: 0x00008A1C File Offset: 0x00006C1C
    // (remove) Token: 0x0600037D RID: 893 RVA: 0x00008A54 File Offset: 0x00006C54
    public event Action<string> clearSelectedBeatmapEvent;

    // Token: 0x0600037E RID: 894 RVA: 0x00008A89 File Offset: 0x00006C89
    public void ClearSelectedBeatmap()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.ClearSelectedBeatmapRpc>();
    }

    // Token: 0x0600037F RID: 895 RVA: 0x00008A96 File Offset: 0x00006C96
    private void InvokeClearSelectedBeatmap(string userId)
    {
        Action<string> action = this.clearSelectedBeatmapEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000082 RID: 130
    // (add) Token: 0x06000380 RID: 896 RVA: 0x00008AAC File Offset: 0x00006CAC
    // (remove) Token: 0x06000381 RID: 897 RVA: 0x00008AE4 File Offset: 0x00006CE4
    public event Action<string> getSelectedBeatmapEvent;

    // Token: 0x06000382 RID: 898 RVA: 0x00008B19 File Offset: 0x00006D19
    public void GetSelectedBeatmap()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetSelectedBeatmapRpc>();
    }

    // Token: 0x06000383 RID: 899 RVA: 0x00008B26 File Offset: 0x00006D26
    private void InvokeGetSelectedBeatmapRpc(string userId)
    {
        Action<string> action = this.getSelectedBeatmapEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000083 RID: 131
    // (add) Token: 0x06000384 RID: 900 RVA: 0x00008B3C File Offset: 0x00006D3C
    // (remove) Token: 0x06000385 RID: 901 RVA: 0x00008B74 File Offset: 0x00006D74
    public event Action<string, BeatmapIdentifierNetSerializable> recommendBeatmapEvent;

    // Token: 0x06000386 RID: 902 RVA: 0x00008BA9 File Offset: 0x00006DA9
    public void RecommendBeatmap(BeatmapIdentifierNetSerializable identifier)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.RecommendBeatmapRpc, BeatmapIdentifierNetSerializable>(identifier);
    }

    // Token: 0x06000387 RID: 903 RVA: 0x00008BB7 File Offset: 0x00006DB7
    private void InvokeRecommendBeatmap(string userId, BeatmapIdentifierNetSerializable identifier)
    {
        Action<string, BeatmapIdentifierNetSerializable> action = this.recommendBeatmapEvent;
        if (action == null)
        {
            return;
        }
        action(userId, identifier);
    }

    // Token: 0x14000084 RID: 132
    // (add) Token: 0x06000388 RID: 904 RVA: 0x00008BCC File Offset: 0x00006DCC
    // (remove) Token: 0x06000389 RID: 905 RVA: 0x00008C04 File Offset: 0x00006E04
    public event Action<string> clearRecommendedBeatmapEvent;

    // Token: 0x0600038A RID: 906 RVA: 0x00008C39 File Offset: 0x00006E39
    public void ClearRecommendedBeatmap()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.ClearRecommendedBeatmapRpc>();
    }

    // Token: 0x0600038B RID: 907 RVA: 0x00008C46 File Offset: 0x00006E46
    private void InvokeClearRecommendedBeatmap(string userId)
    {
        Action<string> action = this.clearRecommendedBeatmapEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000085 RID: 133
    // (add) Token: 0x0600038C RID: 908 RVA: 0x00008C5C File Offset: 0x00006E5C
    // (remove) Token: 0x0600038D RID: 909 RVA: 0x00008C94 File Offset: 0x00006E94
    public event Action<string> getRecommendedBeatmapEvent;

    // Token: 0x0600038E RID: 910 RVA: 0x00008CC9 File Offset: 0x00006EC9
    public void GetRecommendedBeatmap()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetRecommendedBeatmapRpc>();
    }

    // Token: 0x0600038F RID: 911 RVA: 0x00008CD6 File Offset: 0x00006ED6
    private void InvokeGetRecommendedBeatmap(string userId)
    {
        Action<string> action = this.getRecommendedBeatmapEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000086 RID: 134
    // (add) Token: 0x06000390 RID: 912 RVA: 0x00008CEC File Offset: 0x00006EEC
    // (remove) Token: 0x06000391 RID: 913 RVA: 0x00008D24 File Offset: 0x00006F24
    public event Action<string, GameplayModifiers> setSelectedGameplayModifiersEvent;

    // Token: 0x06000392 RID: 914 RVA: 0x00008D59 File Offset: 0x00006F59
    public void SetSelectedGameplayModifiers(GameplayModifiers gameplayModifiers)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetSelectedGameplayModifiersRpc, GameplayModifiers>(gameplayModifiers);
    }

    // Token: 0x06000393 RID: 915 RVA: 0x00008D67 File Offset: 0x00006F67
    private void InvokeSetSelectedGameplayModifiers(string userId, GameplayModifiers gameplayModifiers)
    {
        Action<string, GameplayModifiers> action = this.setSelectedGameplayModifiersEvent;
        if (action == null)
        {
            return;
        }
        action(userId, gameplayModifiers);
    }

    // Token: 0x14000087 RID: 135
    // (add) Token: 0x06000394 RID: 916 RVA: 0x00008D7C File Offset: 0x00006F7C
    // (remove) Token: 0x06000395 RID: 917 RVA: 0x00008DB4 File Offset: 0x00006FB4
    public event Action<string> clearSelectedGameplayModifiersEvent;

    // Token: 0x06000396 RID: 918 RVA: 0x00008DE9 File Offset: 0x00006FE9
    public void ClearSelectedGameplayModifiers()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.ClearSelectedGameplayModifiersRpc>();
    }

    // Token: 0x06000397 RID: 919 RVA: 0x00008DF6 File Offset: 0x00006FF6
    private void InvokeClearSelectedGameplayModifiers(string userId)
    {
        Action<string> action = this.clearSelectedGameplayModifiersEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000088 RID: 136
    // (add) Token: 0x06000398 RID: 920 RVA: 0x00008E0C File Offset: 0x0000700C
    // (remove) Token: 0x06000399 RID: 921 RVA: 0x00008E44 File Offset: 0x00007044
    public event Action<string> getSelectedGameplayModifiersEvent;

    // Token: 0x0600039A RID: 922 RVA: 0x00008E79 File Offset: 0x00007079
    public void GetSelectedGameplayModifiers()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetSelectedGameplayModifiersRpc>();
    }

    // Token: 0x0600039B RID: 923 RVA: 0x00008E86 File Offset: 0x00007086
    private void InvokeGetSelectedGameplayModifiers(string userId)
    {
        Action<string> action = this.getSelectedGameplayModifiersEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000089 RID: 137
    // (add) Token: 0x0600039C RID: 924 RVA: 0x00008E9C File Offset: 0x0000709C
    // (remove) Token: 0x0600039D RID: 925 RVA: 0x00008ED4 File Offset: 0x000070D4
    public event Action<string, GameplayModifiers> recommendGameplayModifiersEvent;

    // Token: 0x0600039E RID: 926 RVA: 0x00008F09 File Offset: 0x00007109
    public void RecommendGameplayModifiers(GameplayModifiers gameplayModifiers)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.RecommendGameplayModifiersRpc, GameplayModifiers>(gameplayModifiers);
    }

    // Token: 0x0600039F RID: 927 RVA: 0x00008F17 File Offset: 0x00007117
    private void InvokeRecommendGameplayModifiers(string userId, GameplayModifiers gameplayModifiers)
    {
        Action<string, GameplayModifiers> action = this.recommendGameplayModifiersEvent;
        if (action == null)
        {
            return;
        }
        action(userId, gameplayModifiers);
    }

    // Token: 0x1400008A RID: 138
    // (add) Token: 0x060003A0 RID: 928 RVA: 0x00008F2C File Offset: 0x0000712C
    // (remove) Token: 0x060003A1 RID: 929 RVA: 0x00008F64 File Offset: 0x00007164
    public event Action<string> clearRecommendedGameplayModifiersEvent;

    // Token: 0x060003A2 RID: 930 RVA: 0x00008F99 File Offset: 0x00007199
    public void ClearRecommendedGameplayModifiers()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.ClearRecommendedGameplayModifiersRpc>();
    }

    // Token: 0x060003A3 RID: 931 RVA: 0x00008FA6 File Offset: 0x000071A6
    private void InvokeClearRecommendedGameplayModifiers(string userId)
    {
        Action<string> action = this.clearRecommendedGameplayModifiersEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400008B RID: 139
    // (add) Token: 0x060003A4 RID: 932 RVA: 0x00008FBC File Offset: 0x000071BC
    // (remove) Token: 0x060003A5 RID: 933 RVA: 0x00008FF4 File Offset: 0x000071F4
    public event Action<string> getRecommendedGameplayModifiersEvent;

    // Token: 0x060003A6 RID: 934 RVA: 0x00009029 File Offset: 0x00007229
    public void GetRecommendedGameplayModifiers()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetRecommendedGameplayModifiersRpc>();
    }

    // Token: 0x060003A7 RID: 935 RVA: 0x00009036 File Offset: 0x00007236
    private void InvokeGetRecommendedGameplayModifiers(string userId)
    {
        Action<string> action = this.getRecommendedGameplayModifiersEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400008C RID: 140
    // (add) Token: 0x060003A8 RID: 936 RVA: 0x0000904C File Offset: 0x0000724C
    // (remove) Token: 0x060003A9 RID: 937 RVA: 0x00009084 File Offset: 0x00007284
    public event Action<string> getIsStartButtonEnabledEvent;

    // Token: 0x060003AA RID: 938 RVA: 0x000090B9 File Offset: 0x000072B9
    public void GetIsStartButtonEnabled()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetIsStartButtonEnabledRpc>();
    }

    // Token: 0x060003AB RID: 939 RVA: 0x000090C6 File Offset: 0x000072C6
    private void InvokeGetIsStartButtonEnabled(string userId)
    {
        Action<string> action = this.getIsStartButtonEnabledEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400008D RID: 141
    // (add) Token: 0x060003AC RID: 940 RVA: 0x000090DC File Offset: 0x000072DC
    // (remove) Token: 0x060003AD RID: 941 RVA: 0x00009114 File Offset: 0x00007314
    public event Action<string, CannotStartGameReason> setIsStartButtonEnabledEvent;

    // Token: 0x060003AE RID: 942 RVA: 0x00009149 File Offset: 0x00007349
    public void SetIsStartButtonEnabled(CannotStartGameReason reason)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetIsStartButtonEnabledRpc, CannotStartGameReason>(reason);
    }

    // Token: 0x060003AF RID: 943 RVA: 0x00009157 File Offset: 0x00007357
    private void InvokeSetIsStartButtonEnabled(string userId, CannotStartGameReason reason)
    {
        Action<string, CannotStartGameReason> action = this.setIsStartButtonEnabledEvent;
        if (action == null)
        {
            return;
        }
        action(userId, reason);
    }

    // Token: 0x1400008E RID: 142
    // (add) Token: 0x060003B0 RID: 944 RVA: 0x0000916C File Offset: 0x0000736C
    // (remove) Token: 0x060003B1 RID: 945 RVA: 0x000091A4 File Offset: 0x000073A4
    public event Action<string, string> levelLoadErrorEvent;

    // Token: 0x060003B2 RID: 946 RVA: 0x000091D9 File Offset: 0x000073D9
    public void LevelLoadError(string levelId)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.LevelLoadErrorRpc, string>(levelId);
    }

    // Token: 0x060003B3 RID: 947 RVA: 0x000091E7 File Offset: 0x000073E7
    private void InvokeLevelLoadError(string userId, string levelId)
    {
        Action<string, string> action = this.levelLoadErrorEvent;
        if (action == null)
        {
            return;
        }
        action(userId, levelId);
    }

    // Token: 0x1400008F RID: 143
    // (add) Token: 0x060003B4 RID: 948 RVA: 0x000091FC File Offset: 0x000073FC
    // (remove) Token: 0x060003B5 RID: 949 RVA: 0x00009234 File Offset: 0x00007434
    public event Action<string, string> levelLoadSuccessEvent;

    // Token: 0x060003B6 RID: 950 RVA: 0x00009269 File Offset: 0x00007469
    public void LevelLoadSuccess(string levelId)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.LevelLoadSuccessRpc, string>(levelId);
    }

    // Token: 0x060003B7 RID: 951 RVA: 0x00009277 File Offset: 0x00007477
    private void InvokeLevelLoadSuccess(string userId, string levelId)
    {
        Action<string, string> action = this.levelLoadSuccessEvent;
        if (action == null)
        {
            return;
        }
        action(userId, levelId);
    }

    // Token: 0x14000090 RID: 144
    // (add) Token: 0x060003B8 RID: 952 RVA: 0x0000928C File Offset: 0x0000748C
    // (remove) Token: 0x060003B9 RID: 953 RVA: 0x000092C4 File Offset: 0x000074C4
    public event Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float> startedLevelEvent;

    // Token: 0x060003BA RID: 954 RVA: 0x000092F9 File Offset: 0x000074F9
    public void StartLevel(BeatmapIdentifierNetSerializable beatmapId, GameplayModifiers gameplayModifiers, float startTime)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.StartLevelRpc, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(beatmapId, gameplayModifiers, startTime);
    }

    // Token: 0x060003BB RID: 955 RVA: 0x00009309 File Offset: 0x00007509
    private void InvokeStartLevel(string userId, BeatmapIdentifierNetSerializable beatmapId, GameplayModifiers gameplayModifiers, float startTime)
    {
        Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float> action = this.startedLevelEvent;
        if (action == null)
        {
            return;
        }
        action(userId, beatmapId, gameplayModifiers, startTime);
    }

    // Token: 0x14000091 RID: 145
    // (add) Token: 0x060003BC RID: 956 RVA: 0x00009320 File Offset: 0x00007520
    // (remove) Token: 0x060003BD RID: 957 RVA: 0x00009358 File Offset: 0x00007558
    public event Action<string> getStartedLevelEvent;

    // Token: 0x060003BE RID: 958 RVA: 0x0000938D File Offset: 0x0000758D
    public void GetStartedLevel()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetStartedLevelRpc>();
    }

    // Token: 0x060003BF RID: 959 RVA: 0x0000939A File Offset: 0x0000759A
    private void InvokeGetStartedLevel(string userId)
    {
        Action<string> action = this.getStartedLevelEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000092 RID: 146
    // (add) Token: 0x060003C0 RID: 960 RVA: 0x000093B0 File Offset: 0x000075B0
    // (remove) Token: 0x060003C1 RID: 961 RVA: 0x000093E8 File Offset: 0x000075E8
    public event Action<string> getMultiplayerGameStateEvent;

    // Token: 0x060003C2 RID: 962 RVA: 0x0000941D File Offset: 0x0000761D
    public void GetMultiplayerGameState()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetMultiplayerGameStateRpc>();
    }

    // Token: 0x060003C3 RID: 963 RVA: 0x0000942A File Offset: 0x0000762A
    private void InvokeGetMultiplayerGameState(string userId)
    {
        Action<string> action = this.getMultiplayerGameStateEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000093 RID: 147
    // (add) Token: 0x060003C4 RID: 964 RVA: 0x00009440 File Offset: 0x00007640
    // (remove) Token: 0x060003C5 RID: 965 RVA: 0x00009478 File Offset: 0x00007678
    public event Action<string, MultiplayerGameState> setMultiplayerGameStateEvent;

    // Token: 0x060003C6 RID: 966 RVA: 0x000094AD File Offset: 0x000076AD
    public void SetMultiplayerGameState(MultiplayerGameState lobbyState)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetMultiplayerGameStateRpc, MultiplayerGameState>(lobbyState);
    }

    // Token: 0x060003C7 RID: 967 RVA: 0x000094BB File Offset: 0x000076BB
    private void InvokeSetMultiplayerGameState(string userId, MultiplayerGameState lobbyState)
    {
        Action<string, MultiplayerGameState> action = this.setMultiplayerGameStateEvent;
        if (action == null)
        {
            return;
        }
        action(userId, lobbyState);
    }

    // Token: 0x14000094 RID: 148
    // (add) Token: 0x060003C8 RID: 968 RVA: 0x000094D0 File Offset: 0x000076D0
    // (remove) Token: 0x060003C9 RID: 969 RVA: 0x00009508 File Offset: 0x00007708
    public event Action<string> cancelCountdownEvent;

    // Token: 0x060003CA RID: 970 RVA: 0x0000953D File Offset: 0x0000773D
    public void CancelCountdown()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.CancelCountdownRpc>();
    }

    // Token: 0x060003CB RID: 971 RVA: 0x0000954A File Offset: 0x0000774A
    private void InvokeCancelCountdown(string userId)
    {
        Action<string> action = this.cancelCountdownEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000095 RID: 149
    // (add) Token: 0x060003CC RID: 972 RVA: 0x00009560 File Offset: 0x00007760
    // (remove) Token: 0x060003CD RID: 973 RVA: 0x00009598 File Offset: 0x00007798
    public event Action<string, float> setCountdownEndTimeEvent;

    // Token: 0x060003CE RID: 974 RVA: 0x000095CD File Offset: 0x000077CD
    public void SetCountdownEndTime(float newTime)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetCountdownEndTimeRpc, float>(newTime);
    }

    // Token: 0x060003CF RID: 975 RVA: 0x000095DB File Offset: 0x000077DB
    private void InvokeSetCountdownEndTime(string userId, float newTime)
    {
        Action<string, float> action = this.setCountdownEndTimeEvent;
        if (action == null)
        {
            return;
        }
        action(userId, newTime);
    }

    // Token: 0x14000096 RID: 150
    // (add) Token: 0x060003D0 RID: 976 RVA: 0x000095F0 File Offset: 0x000077F0
    // (remove) Token: 0x060003D1 RID: 977 RVA: 0x00009628 File Offset: 0x00007828
    public event Action<string> getCountdownEndTimeEvent;

    // Token: 0x060003D2 RID: 978 RVA: 0x0000965D File Offset: 0x0000785D
    public void GetCountdownEndTime()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetCountdownEndTimeRpc>();
    }

    // Token: 0x060003D3 RID: 979 RVA: 0x0000966A File Offset: 0x0000786A
    private void InvokeGetCountdownEndTime(string userId)
    {
        Action<string> action = this.getCountdownEndTimeEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000097 RID: 151
    // (add) Token: 0x060003D4 RID: 980 RVA: 0x00009680 File Offset: 0x00007880
    // (remove) Token: 0x060003D5 RID: 981 RVA: 0x000096B8 File Offset: 0x000078B8
    public event Action<string> cancelledLevelStartEvent;

    // Token: 0x060003D6 RID: 982 RVA: 0x000096ED File Offset: 0x000078ED
    public void CancelLevelStart()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.CancelLevelStartRpc>();
    }

    // Token: 0x060003D7 RID: 983 RVA: 0x000096FA File Offset: 0x000078FA
    private void InvokeCancelLevelStart(string userId)
    {
        Action<string> action = this.cancelledLevelStartEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000098 RID: 152
    // (add) Token: 0x060003D8 RID: 984 RVA: 0x00009710 File Offset: 0x00007910
    // (remove) Token: 0x060003D9 RID: 985 RVA: 0x00009748 File Offset: 0x00007948
    public event Action<string> getIsReadyEvent;

    // Token: 0x060003DA RID: 986 RVA: 0x0000977D File Offset: 0x0000797D
    public void GetIsReady()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetIsReadyRpc>();
    }

    // Token: 0x060003DB RID: 987 RVA: 0x0000978A File Offset: 0x0000798A
    private void InvokeGetIsReady(string userId)
    {
        Action<string> action = this.getIsReadyEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000099 RID: 153
    // (add) Token: 0x060003DC RID: 988 RVA: 0x000097A0 File Offset: 0x000079A0
    // (remove) Token: 0x060003DD RID: 989 RVA: 0x000097D8 File Offset: 0x000079D8
    public event Action<string, bool> setIsReadyEvent;

    // Token: 0x060003DE RID: 990 RVA: 0x0000980D File Offset: 0x00007A0D
    public void SetIsReady(bool isReady)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetIsReadyRpc, bool>(isReady);
    }

    // Token: 0x060003DF RID: 991 RVA: 0x0000981B File Offset: 0x00007A1B
    private void InvokeSetIsReady(string userId, bool isReady)
    {
        Action<string, bool> action = this.setIsReadyEvent;
        if (action == null)
        {
            return;
        }
        action(userId, isReady);
    }

    // Token: 0x1400009A RID: 154
    // (add) Token: 0x060003E0 RID: 992 RVA: 0x00009830 File Offset: 0x00007A30
    // (remove) Token: 0x060003E1 RID: 993 RVA: 0x00009868 File Offset: 0x00007A68
    public event Action<string, float> setStartGameTimeEvent;

    // Token: 0x060003E2 RID: 994 RVA: 0x0000989D File Offset: 0x00007A9D
    public void SetStartGameTime(float newTime)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetStartGameTimeRpc, float>(newTime);
    }

    // Token: 0x060003E3 RID: 995 RVA: 0x000098AB File Offset: 0x00007AAB
    private void InvokeSetStartGameCountdown(string userId, float newTime)
    {
        Action<string, float> action = this.setStartGameTimeEvent;
        if (action == null)
        {
            return;
        }
        action(userId, newTime);
    }

    // Token: 0x1400009B RID: 155
    // (add) Token: 0x060003E4 RID: 996 RVA: 0x000098C0 File Offset: 0x00007AC0
    // (remove) Token: 0x060003E5 RID: 997 RVA: 0x000098F8 File Offset: 0x00007AF8
    public event Action<string> cancelStartGameTimeEvent;

    // Token: 0x060003E6 RID: 998 RVA: 0x0000992D File Offset: 0x00007B2D
    public void CancelStartGameTime()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetStartGameTimeRpc>();
    }

    // Token: 0x060003E7 RID: 999 RVA: 0x0000993A File Offset: 0x00007B3A
    private void InvokeCancelStartGameCountdown(string userId)
    {
        Action<string> action = this.cancelStartGameTimeEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400009C RID: 156
    // (add) Token: 0x060003E8 RID: 1000 RVA: 0x00009950 File Offset: 0x00007B50
    // (remove) Token: 0x060003E9 RID: 1001 RVA: 0x00009988 File Offset: 0x00007B88
    public event Action<string> getIsInLobbyEvent;

    // Token: 0x060003EA RID: 1002 RVA: 0x000099BD File Offset: 0x00007BBD
    public void GetIsInLobby()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetIsInLobbyRpc>();
    }

    // Token: 0x060003EB RID: 1003 RVA: 0x000099CA File Offset: 0x00007BCA
    private void InvokeGetIsInLobby(string userId)
    {
        Action<string> action = this.getIsInLobbyEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400009D RID: 157
    // (add) Token: 0x060003EC RID: 1004 RVA: 0x000099E0 File Offset: 0x00007BE0
    // (remove) Token: 0x060003ED RID: 1005 RVA: 0x00009A18 File Offset: 0x00007C18
    public event Action<string, bool> setIsInLobbyEvent;

    // Token: 0x060003EE RID: 1006 RVA: 0x00009A4D File Offset: 0x00007C4D
    public void SetIsInLobby(bool isBack)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetIsInLobbyRpc, bool>(isBack);
    }

    // Token: 0x060003EF RID: 1007 RVA: 0x00009A5B File Offset: 0x00007C5B
    private void InvokeSetIsInLobby(string userId, bool isBack)
    {
        Action<string, bool> action = this.setIsInLobbyEvent;
        if (action == null)
        {
            return;
        }
        action(userId, isBack);
    }

    // Token: 0x1400009E RID: 158
    // (add) Token: 0x060003F0 RID: 1008 RVA: 0x00009A70 File Offset: 0x00007C70
    // (remove) Token: 0x060003F1 RID: 1009 RVA: 0x00009AA8 File Offset: 0x00007CA8
    public event Action<string> getOwnedSongPacksEvent;

    // Token: 0x060003F2 RID: 1010 RVA: 0x00009ADD File Offset: 0x00007CDD
    public void GetOwnedSongPacks()
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.GetOwnedSongPacksRpc>();
    }

    // Token: 0x060003F3 RID: 1011 RVA: 0x00009AEA File Offset: 0x00007CEA
    private void InvokeGetOwnedSongPacks(string userId)
    {
        Action<string> action = this.getOwnedSongPacksEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x1400009F RID: 159
    // (add) Token: 0x060003F4 RID: 1012 RVA: 0x00009B00 File Offset: 0x00007D00
    // (remove) Token: 0x060003F5 RID: 1013 RVA: 0x00009B38 File Offset: 0x00007D38
    public event Action<string, SongPackMask> setOwnedSongPacksEvent;

    // Token: 0x060003F6 RID: 1014 RVA: 0x00009B6D File Offset: 0x00007D6D
    public void SetOwnedSongPacks(SongPackMask songPackMask)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.SetOwnedSongPacksRpc, SongPackMask>(songPackMask);
    }

    // Token: 0x060003F7 RID: 1015 RVA: 0x00009B7B File Offset: 0x00007D7B
    private void InvokeSetOwnedSongPacks(string userId, SongPackMask songPackMask)
    {
        Action<string, SongPackMask> action = this.setOwnedSongPacksEvent;
        if (action == null)
        {
            return;
        }
        action(userId, songPackMask);
    }

    // Token: 0x140000A0 RID: 160
    // (add) Token: 0x060003F8 RID: 1016 RVA: 0x00009B90 File Offset: 0x00007D90
    // (remove) Token: 0x060003F9 RID: 1017 RVA: 0x00009BC8 File Offset: 0x00007DC8
    public event Action<string, string> requestedKickPlayerEvent;

    // Token: 0x060003FA RID: 1018 RVA: 0x00009BFD File Offset: 0x00007DFD
    public void RequestKickPlayer(string kickedPlayerId)
    {
        this._rpcHandler.EnqueueRpc<MenuRpcManager.RequestKickPlayerRpc, string>(kickedPlayerId);
    }

    // Token: 0x060003FB RID: 1019 RVA: 0x00009C0B File Offset: 0x00007E0B
    private void InvokeKickPlayer(string userId, string kickedPlayerId)
    {
        Action<string, string> action = this.requestedKickPlayerEvent;
        if (action == null)
        {
            return;
        }
        action(userId, kickedPlayerId);
    }

    // Token: 0x0400012C RID: 300
    private const string kMenuState = "in_menu";

    // Token: 0x0400012D RID: 301
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;

    // Token: 0x0400012E RID: 302
    private readonly RpcHandler<MenuRpcManager.RpcType> _rpcHandler;

    // Token: 0x02000103 RID: 259
    private enum RpcType : byte
    {
        // Token: 0x040003B1 RID: 945
        SetPlayersMissingEntitlementsToLevel,
        // Token: 0x040003B2 RID: 946
        GetIsEntitledToLevel,
        // Token: 0x040003B3 RID: 947
        SetIsEntitledToLevel,
        // Token: 0x040003B4 RID: 948
        InvalidateLevelEntitlementStatuses,
        // Token: 0x040003B5 RID: 949
        SelectLevelPack,
        // Token: 0x040003B6 RID: 950
        SetSelectedBeatmap,
        // Token: 0x040003B7 RID: 951
        GetSelectedBeatmap,
        // Token: 0x040003B8 RID: 952
        RecommendBeatmap,
        // Token: 0x040003B9 RID: 953
        ClearRecommendedBeatmap,
        // Token: 0x040003BA RID: 954
        GetRecommendedBeatmap,
        // Token: 0x040003BB RID: 955
        SetSelectedGameplayModifiers,
        // Token: 0x040003BC RID: 956
        GetSelectedGameplayModifiers,
        // Token: 0x040003BD RID: 957
        RecommendGameplayModifiers,
        // Token: 0x040003BE RID: 958
        ClearRecommendedGameplayModifiers,
        // Token: 0x040003BF RID: 959
        GetRecommendedGameplayModifiers,
        // Token: 0x040003C0 RID: 960
        LevelLoadError,
        // Token: 0x040003C1 RID: 961
        LevelLoadSuccess,
        // Token: 0x040003C2 RID: 962
        StartLevel,
        // Token: 0x040003C3 RID: 963
        GetStartedLevel,
        // Token: 0x040003C4 RID: 964
        CancelLevelStart,
        // Token: 0x040003C5 RID: 965
        GetMultiplayerGameState,
        // Token: 0x040003C6 RID: 966
        SetMultiplayerGameState,
        // Token: 0x040003C7 RID: 967
        GetIsReady,
        // Token: 0x040003C8 RID: 968
        SetIsReady,
        // Token: 0x040003C9 RID: 969
        SetStartGameTime,
        // Token: 0x040003CA RID: 970
        CancelStartGameTime,
        // Token: 0x040003CB RID: 971
        GetIsInLobby,
        // Token: 0x040003CC RID: 972
        SetIsInLobby,
        // Token: 0x040003CD RID: 973
        GetCountdownEndTime,
        // Token: 0x040003CE RID: 974
        SetCountdownEndTime,
        // Token: 0x040003CF RID: 975
        CancelCountdown,
        // Token: 0x040003D0 RID: 976
        GetOwnedSongPacks,
        // Token: 0x040003D1 RID: 977
        SetOwnedSongPacks,
        // Token: 0x040003D2 RID: 978
        RequestKickPlayer,
        // Token: 0x040003D3 RID: 979
        GetPermissionConfiguration,
        // Token: 0x040003D4 RID: 980
        SetPermissionConfiguration,
        // Token: 0x040003D5 RID: 981
        GetIsStartButtonEnabled,
        // Token: 0x040003D6 RID: 982
        SetIsStartButtonEnabled,
        // Token: 0x040003D7 RID: 983
        ClearSelectedBeatmap,
        // Token: 0x040003D8 RID: 984
        ClearSelectedGameplayModifiers
    }

    // Token: 0x02000104 RID: 260
    private class GetPlayersPermissionConfigurationRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000105 RID: 261
    private class SetPlayersPermissionConfigurationRpc : RemoteProcedureCall<PlayersLobbyPermissionConfigurationNetSerializable>
    {
    }

    // Token: 0x02000106 RID: 262
    private class SetPlayersMissingEntitlementsToLevelRpc : RemoteProcedureCall<PlayersMissingEntitlementsNetSerializable>
    {
    }

    // Token: 0x02000107 RID: 263
    private class GetIsEntitledToLevelRpc : RemoteProcedureCall<string>
    {
    }

    // Token: 0x02000108 RID: 264
    private class SetIsEntitledToLevelRpc : RemoteProcedureCall<string, int>
    {
    }

    // Token: 0x02000109 RID: 265
    private class InvalidateLevelEntitlementStatusesRpc : RemoteProcedureCall
    {
    }

    // Token: 0x0200010A RID: 266
    private class SelectLevelPackRpc : RemoteProcedureCall<string>
    {
    }

    // Token: 0x0200010B RID: 267
    private class SetSelectedBeatmapRpc : RemoteProcedureCall<BeatmapIdentifierNetSerializable>
    {
    }

    // Token: 0x0200010C RID: 268
    private class ClearSelectedBeatmapRpc : RemoteProcedureCall
    {
    }

    // Token: 0x0200010D RID: 269
    private class GetSelectedBeatmapRpc : RemoteProcedureCall
    {
    }

    // Token: 0x0200010E RID: 270
    private class RecommendBeatmapRpc : RemoteProcedureCall<BeatmapIdentifierNetSerializable>
    {
    }

    // Token: 0x0200010F RID: 271
    private class ClearRecommendedBeatmapRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000110 RID: 272
    private class GetRecommendedBeatmapRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000111 RID: 273
    private class SetSelectedGameplayModifiersRpc : RemoteProcedureCall<GameplayModifiers>
    {
    }

    // Token: 0x02000112 RID: 274
    private class ClearSelectedGameplayModifiersRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000113 RID: 275
    private class GetSelectedGameplayModifiersRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000114 RID: 276
    private class RecommendGameplayModifiersRpc : RemoteProcedureCall<GameplayModifiers>
    {
    }

    // Token: 0x02000115 RID: 277
    private class ClearRecommendedGameplayModifiersRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000116 RID: 278
    private class GetRecommendedGameplayModifiersRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000117 RID: 279
    private class GetIsStartButtonEnabledRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000118 RID: 280
    private class SetIsStartButtonEnabledRpc : RemoteProcedureCall<CannotStartGameReason>
    {
    }

    // Token: 0x02000119 RID: 281
    private class LevelLoadErrorRpc : RemoteProcedureCall<string>
    {
    }

    // Token: 0x0200011A RID: 282
    private class LevelLoadSuccessRpc : RemoteProcedureCall<string>
    {
    }

    // Token: 0x0200011B RID: 283
    private class StartLevelRpc : RemoteProcedureCall<BeatmapIdentifierNetSerializable, GameplayModifiers, float>
    {
    }

    // Token: 0x0200011C RID: 284
    private class GetStartedLevelRpc : RemoteProcedureCall
    {
    }

    // Token: 0x0200011D RID: 285
    private class GetMultiplayerGameStateRpc : RemoteProcedureCall
    {
    }

    // Token: 0x0200011E RID: 286
    private class SetMultiplayerGameStateRpc : RemoteProcedureCall<MultiplayerGameState>
    {
    }

    // Token: 0x0200011F RID: 287
    private class CancelCountdownRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000120 RID: 288
    private class SetCountdownEndTimeRpc : RemoteProcedureCall<float>
    {
    }

    // Token: 0x02000121 RID: 289
    private class GetCountdownEndTimeRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000122 RID: 290
    private class CancelLevelStartRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000123 RID: 291
    private class GetIsReadyRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000124 RID: 292
    private class SetIsReadyRpc : RemoteProcedureCall<bool>
    {
    }

    // Token: 0x02000125 RID: 293
    private class SetStartGameTimeRpc : RemoteProcedureCall<float>
    {
    }

    // Token: 0x02000126 RID: 294
    private class CancelStartGameTimeRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000127 RID: 295
    private class GetIsInLobbyRpc : RemoteProcedureCall
    {
    }

    // Token: 0x02000128 RID: 296
    private class SetIsInLobbyRpc : RemoteProcedureCall<bool>
    {
    }

    // Token: 0x02000129 RID: 297
    private class GetOwnedSongPacksRpc : RemoteProcedureCall
    {
    }

    // Token: 0x0200012A RID: 298
    private class SetOwnedSongPacksRpc : RemoteProcedureCall<SongPackMask>
    {
    }

    // Token: 0x0200012B RID: 299
    private class RequestKickPlayerRpc : RemoteProcedureCall<string>
    {
    }
}
