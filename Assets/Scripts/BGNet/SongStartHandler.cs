using System;
using System.Collections.Generic;
using System.Diagnostics;
using BGNet.Logging;

// Token: 0x0200007F RID: 127
public class SongStartHandler : IDisposable
{
    // Token: 0x170000D6 RID: 214
    // (get) Token: 0x06000542 RID: 1346 RVA: 0x0000E6D6 File Offset: 0x0000C8D6
    public float songStartSyncTime
    {
        get
        {
            return this._startTime;
        }
    }

    // Token: 0x140000AD RID: 173
    // (add) Token: 0x06000543 RID: 1347 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
    // (remove) Token: 0x06000544 RID: 1348 RVA: 0x0000E718 File Offset: 0x0000C918
    public event Action<float> setSongStartSyncTimeEvent;

    // Token: 0x06000545 RID: 1349 RVA: 0x0000E74D File Offset: 0x0000C94D
    public SongStartHandler(IMultiplayerSessionManager multiplayerSessionManager, IGameplayRpcManager gameplayRpcManager, PlayersSpecificSettingsAtGameStartModel playersAtGameStartModel)
    {
        this._gameplayRpcManager = gameplayRpcManager;
        this._multiplayerSessionManager = multiplayerSessionManager;
        this._playersAtGameStartModel = playersAtGameStartModel;
    }

    // Token: 0x06000546 RID: 1350 RVA: 0x0000E778 File Offset: 0x0000C978
    public void GetLevelStartTimeOffset()
    {
        if (this._multiplayerSessionManager.isConnectionOwner)
        {
            this._gameplayRpcManager.setGameplaySongReadyEvent += this.HandleSetGameplaySongReady;
            this._gameplayRpcManager.GetGameplaySongReady();
        }
        else
        {
            this._gameplayRpcManager.getGameplaySongReadyEvent += this.HandleGetGameplaySongReady;
            this._gameplayRpcManager.setSongStartTimeEvent += this.HandleSetSongStartTime;
            this._gameplayRpcManager.SetGameplaySongReady();
        }
        if (this._multiplayerSessionManager.connectedPlayerCount == 0 && !this._started && this._multiplayerSessionManager.isConnectionOwner)
        {
            this._started = true;
            this._startTime = this._multiplayerSessionManager.syncTime + 0.25f;
            Action<float> action = this.setSongStartSyncTimeEvent;
            if (action == null)
            {
                return;
            }
            action(this.songStartSyncTime);
        }
    }

    // Token: 0x06000547 RID: 1351 RVA: 0x0000E848 File Offset: 0x0000CA48
    public void Dispose()
    {
        this._gameplayRpcManager.setGameplaySongReadyEvent -= this.HandleSetGameplaySongReady;
        this._gameplayRpcManager.getGameplaySongReadyEvent -= this.HandleGetGameplaySongReady;
        this._gameplayRpcManager.setSongStartTimeEvent -= this.HandleSetSongStartTime;
    }

    // Token: 0x06000548 RID: 1352 RVA: 0x0000E89C File Offset: 0x0000CA9C
    public void ForceStart(string sessionGameId)
    {
        if (!this._started)
        {
            if (this._multiplayerSessionManager.isConnectionOwner)
            {
                this._started = true;
                foreach (IConnectedPlayer connectedPlayer in this._multiplayerSessionManager.connectedPlayers)
                {
                    if (connectedPlayer.WasActiveAtLevelStart() && !this._readyPlayers.Contains(connectedPlayer.userId))
                    {
                        this._gameplayRpcManager.SetPlayerDidConnectLate(connectedPlayer.userId, this._playersAtGameStartModel.playersAtGameStartNetSerializable, sessionGameId);
                    }
                }
                this.StartSong();
                this._gameplayRpcManager.SetSongStartTime(this._startTime);
                return;
            }
            BGNet.Logging.Debug.LogError("Should not try to force start while not being host");
        }
    }

    // Token: 0x06000549 RID: 1353 RVA: 0x0000E964 File Offset: 0x0000CB64
    private void StartSong()
    {
        float num = 0f;
        for (int i = 0; i < this._multiplayerSessionManager.connectedPlayerCount; i++)
        {
            IConnectedPlayer connectedPlayer = this._multiplayerSessionManager.GetConnectedPlayer(i);
            num = Math.Max(num, connectedPlayer.currentLatency);
        }
        this._started = true;
        this._startTime = this._multiplayerSessionManager.syncTime + 0.25f + num * 2f;
        Action<float> action = this.setSongStartSyncTimeEvent;
        if (action == null)
        {
            return;
        }
        action(this.songStartSyncTime);
    }

    // Token: 0x0600054A RID: 1354 RVA: 0x0000E9E4 File Offset: 0x0000CBE4
    private void HandleSetGameplaySongReady(string user)
    {
        if (!this._started)
        {
            this._readyPlayers.Add(user);
            for (int i = 0; i < this._multiplayerSessionManager.connectedPlayerCount; i++)
            {
                IConnectedPlayer connectedPlayer = this._multiplayerSessionManager.GetConnectedPlayer(i);
                if (!this._readyPlayers.Contains(connectedPlayer.userId))
                {
                    return;
                }
            }
            this.StartSong();
        }
        this._gameplayRpcManager.SetSongStartTime(this._startTime);
    }

    // Token: 0x0600054B RID: 1355 RVA: 0x0000EA54 File Offset: 0x0000CC54
    private void HandleGetGameplaySongReady(string user)
    {
        this._gameplayRpcManager.SetGameplaySongReady();
    }

    // Token: 0x0600054C RID: 1356 RVA: 0x0000EA61 File Offset: 0x0000CC61
    private void HandleSetSongStartTime(string user, float time)
    {
        this._started = true;
        this._startTime = time;
        Action<float> action = this.setSongStartSyncTimeEvent;
        if (action == null)
        {
            return;
        }
        action(this.songStartSyncTime);
    }

    // Token: 0x0600054D RID: 1357 RVA: 0x0000EA87 File Offset: 0x0000CC87
    [Conditional("VERBOSE_LOGGING")]
    private void Log(string message)
    {
        BGNet.Logging.Debug.Log(message);
    }

    // Token: 0x04000212 RID: 530
    private const float kFixedStartDelay = 0.25f;

    // Token: 0x04000213 RID: 531
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;

    // Token: 0x04000214 RID: 532
    private readonly IGameplayRpcManager _gameplayRpcManager;

    // Token: 0x04000215 RID: 533
    private readonly PlayersSpecificSettingsAtGameStartModel _playersAtGameStartModel;

    // Token: 0x04000216 RID: 534
    private readonly HashSet<string> _readyPlayers = new HashSet<string>();

    // Token: 0x04000217 RID: 535
    private bool _started;

    // Token: 0x04000218 RID: 536
    private float _startTime;
}
