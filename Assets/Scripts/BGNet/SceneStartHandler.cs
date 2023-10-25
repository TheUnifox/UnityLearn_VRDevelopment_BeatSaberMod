using System;
using System.Collections.Generic;
using BGNet.Logging;

// Token: 0x0200007B RID: 123
public class SceneStartHandler : IDisposable
{
    // Token: 0x140000AB RID: 171
    // (add) Token: 0x0600052C RID: 1324 RVA: 0x0000DE90 File Offset: 0x0000C090
    // (remove) Token: 0x0600052D RID: 1325 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
    public event Action<string> sceneSetupDidFinishEvent;

    // Token: 0x140000AC RID: 172
    // (add) Token: 0x0600052E RID: 1326 RVA: 0x0000DF00 File Offset: 0x0000C100
    // (remove) Token: 0x0600052F RID: 1327 RVA: 0x0000DF38 File Offset: 0x0000C138
    public event Action<string> sceneSetupDidReceiveTooLateEvent;

    // Token: 0x06000530 RID: 1328 RVA: 0x0000DF6D File Offset: 0x0000C16D
    public SceneStartHandler(IMultiplayerSessionManager multiplayerSessionManager, IGameplayRpcManager gameplayRpcManager, PlayersSpecificSettingsAtGameStartModel playersAtGameStartModel)
    {
        this._gameplayRpcManager = gameplayRpcManager;
        this._multiplayerSessionManager = multiplayerSessionManager;
        this._playersAtGameStartModel = playersAtGameStartModel;
    }

    // Token: 0x06000531 RID: 1329 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
    public void Dispose()
    {
        if (this._multiplayerSessionManager.isConnectionOwner)
        {
            this._gameplayRpcManager.setGameplaySceneReadyEvent -= this.HandleSetGameplaySceneReady;
            return;
        }
        this._gameplayRpcManager.getGameplaySceneReadyEvent -= this.HandleGetGameplaySceneReady;
        this._gameplayRpcManager.setGameplaySceneSyncFinishedEvent -= this.HandleSetGameplaySceneSyncFinished;
        this._gameplayRpcManager.setPlayerDidConnectLateEvent -= this.HandleSetPlayerDidConnectLate;
    }

    // Token: 0x06000532 RID: 1330 RVA: 0x0000E018 File Offset: 0x0000C218
    public void GetSceneLoadStatus()
    {
        if (this._multiplayerSessionManager.isConnectionOwner && this._multiplayerSessionManager.connectedPlayerCount == 0 && !this._started)
        {
            this.AddPlayerSpecificSettingsToDictionary(this._playersAtGameStartModel.localPlayerSpecificSettings);
            this._gameplayRpcManager.setGameplaySceneReadyEvent += this.HandleSetGameplaySceneReady;
            this._started = true;
            this._sessionGameId = Guid.NewGuid().ToString();
            this._playersAtGameStartModel.SaveFromNetSerializable(this.CreatePlayersSpecificSettingsAtGameStartData());
            Action<string> action = this.sceneSetupDidFinishEvent;
            if (action == null)
            {
                return;
            }
            action(this._sessionGameId);
            return;
        }
        else
        {
            if (this._multiplayerSessionManager.isConnectionOwner)
            {
                this._gameplayRpcManager.setGameplaySceneReadyEvent += this.HandleSetGameplaySceneReady;
                if (this._multiplayerSessionManager.localPlayer.IsActiveOrFinished())
                {
                    this.AddPlayerSpecificSettingsToDictionary(this._playersAtGameStartModel.localPlayerSpecificSettings);
                }
                this._sessionGameId = Guid.NewGuid().ToString();
                this._gameplayRpcManager.GetGameplaySceneReady();
                return;
            }
            this._gameplayRpcManager.getGameplaySceneReadyEvent += this.HandleGetGameplaySceneReady;
            this._gameplayRpcManager.setGameplaySceneSyncFinishedEvent += this.HandleSetGameplaySceneSyncFinished;
            this._gameplayRpcManager.setPlayerDidConnectLateEvent += this.HandleSetPlayerDidConnectLate;
            this._gameplayRpcManager.SetGameplaySceneReady(this._playersAtGameStartModel.localPlayerSpecificSettings);
            return;
        }
    }

    // Token: 0x06000533 RID: 1331 RVA: 0x0000E184 File Offset: 0x0000C384
    public void ForceStart()
    {
        if (!this._started)
        {
            if (this._multiplayerSessionManager.isConnectionOwner)
            {
                this._started = true;
                this._playersAtGameStartModel.SaveFromNetSerializable(this.CreatePlayersSpecificSettingsAtGameStartData());
                foreach (IConnectedPlayer connectedPlayer in this._multiplayerSessionManager.connectedPlayers)
                {
                    if (connectedPlayer.WasActiveAtLevelStart() && !this._readyPlayers.Contains(connectedPlayer.userId))
                    {
                        this._gameplayRpcManager.SetPlayerDidConnectLate(connectedPlayer.userId, this._playersAtGameStartModel.playersAtGameStartNetSerializable, this._sessionGameId);
                    }
                }
                this._gameplayRpcManager.SetGameplaySceneSyncFinished(this._playersAtGameStartModel.playersAtGameStartNetSerializable, this._sessionGameId);
                Action<string> action = this.sceneSetupDidFinishEvent;
                if (action == null)
                {
                    return;
                }
                action(this._sessionGameId);
                return;
            }
            else
            {
                Debug.LogError("Should not try to force start while not being host");
            }
        }
    }

    // Token: 0x06000534 RID: 1332 RVA: 0x0000E27C File Offset: 0x0000C47C
    private void HandleSetGameplaySceneReady(string userId, PlayerSpecificSettingsNetSerializable playerSpecificSettings)
    {
        if (!this._started)
        {
            this._readyPlayers.Add(userId);
            this._playersSpecificSettings[userId] = playerSpecificSettings;
            for (int i = 0; i < this._multiplayerSessionManager.connectedPlayerCount; i++)
            {
                IConnectedPlayer connectedPlayer = this._multiplayerSessionManager.GetConnectedPlayer(i);
                if (!this._readyPlayers.Contains(connectedPlayer.userId))
                {
                    return;
                }
            }
            this._playersAtGameStartModel.SaveFromNetSerializable(this.CreatePlayersSpecificSettingsAtGameStartData());
            this._started = true;
            Action<string> action = this.sceneSetupDidFinishEvent;
            if (action != null)
            {
                action(this._sessionGameId);
            }
            this._gameplayRpcManager.SetGameplaySceneSyncFinished(this._playersAtGameStartModel.playersAtGameStartNetSerializable, this._sessionGameId);
            return;
        }
        this._gameplayRpcManager.SetPlayerDidConnectLate(userId, this._playersAtGameStartModel.playersAtGameStartNetSerializable, this._sessionGameId);
    }

    // Token: 0x06000535 RID: 1333 RVA: 0x0000E34E File Offset: 0x0000C54E
    private void HandleGetGameplaySceneReady(string userId)
    {
        this._gameplayRpcManager.SetGameplaySceneReady(this._playersAtGameStartModel.localPlayerSpecificSettings);
    }

    // Token: 0x06000536 RID: 1334 RVA: 0x0000E366 File Offset: 0x0000C566
    private void HandleSetGameplaySceneSyncFinished(string userId, PlayerSpecificSettingsAtStartNetSerializable playersAtGameStart, string sessionId)
    {
        if (!this._started)
        {
            this._playersAtGameStartModel.SaveFromNetSerializable(playersAtGameStart);
            this._started = true;
            this._sessionGameId = sessionId;
            Action<string> action = this.sceneSetupDidFinishEvent;
            if (action == null)
            {
                return;
            }
            action(this._sessionGameId);
        }
    }

    // Token: 0x06000537 RID: 1335 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
    private void HandleSetPlayerDidConnectLate(string userId, string failedUserId, PlayerSpecificSettingsAtStartNetSerializable playersAtGameStart, string sessionId)
    {
        if (this._multiplayerSessionManager.localPlayer.userId == failedUserId && !this._started)
        {
            this._playersAtGameStartModel.SaveFromNetSerializable(playersAtGameStart);
            this._multiplayerSessionManager.SetLocalPlayerState("was_active_at_level_start", false);
            this._multiplayerSessionManager.SetLocalPlayerState("is_active", false);
            this._started = true;
            this._sessionGameId = sessionId;
            Action<string> action = this.sceneSetupDidReceiveTooLateEvent;
            if (action == null)
            {
                return;
            }
            action(this._sessionGameId);
        }
    }

    // Token: 0x06000538 RID: 1336 RVA: 0x0000E420 File Offset: 0x0000C620
    private void AddPlayerSpecificSettingsToDictionary(PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable)
    {
        if (playerSpecificSettingsNetSerializable != null)
        {
            this._playersSpecificSettings[playerSpecificSettingsNetSerializable.userId] = playerSpecificSettingsNetSerializable;
        }
    }

    // Token: 0x06000539 RID: 1337 RVA: 0x0000E438 File Offset: 0x0000C638
    private PlayerSpecificSettingsAtStartNetSerializable CreatePlayersSpecificSettingsAtGameStartData()
    {
        List<PlayerSpecificSettingsNetSerializable> list = new List<PlayerSpecificSettingsNetSerializable>();
        foreach (PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable in this._playersSpecificSettings.Values)
        {
            string userId = playerSpecificSettingsNetSerializable.userId;
            if (this._multiplayerSessionManager.GetPlayerByUserId(userId).IsActiveOrFinished())
            {
                list.Add(playerSpecificSettingsNetSerializable);
            }
        }
        return new PlayerSpecificSettingsAtStartNetSerializable(list);
    }

    // Token: 0x040001F7 RID: 503
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;

    // Token: 0x040001F8 RID: 504
    private readonly IGameplayRpcManager _gameplayRpcManager;

    // Token: 0x040001F9 RID: 505
    private readonly PlayersSpecificSettingsAtGameStartModel _playersAtGameStartModel;

    // Token: 0x040001FA RID: 506
    private readonly HashSet<string> _readyPlayers = new HashSet<string>();

    // Token: 0x040001FB RID: 507
    private readonly Dictionary<string, PlayerSpecificSettingsNetSerializable> _playersSpecificSettings = new Dictionary<string, PlayerSpecificSettingsNetSerializable>();

    // Token: 0x040001FC RID: 508
    private bool _started;

    // Token: 0x040001FD RID: 509
    private string _sessionGameId;
}
