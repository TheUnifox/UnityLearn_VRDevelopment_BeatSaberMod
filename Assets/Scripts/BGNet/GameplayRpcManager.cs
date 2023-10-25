using System;

// Token: 0x02000022 RID: 34
public class GameplayRpcManager : IDisposable, IGameplayRpcManager
{
    // Token: 0x1700003B RID: 59
    // (get) Token: 0x06000126 RID: 294 RVA: 0x00005D29 File Offset: 0x00003F29
    // (set) Token: 0x06000127 RID: 295 RVA: 0x00005D3B File Offset: 0x00003F3B
    public bool enabled
    {
        get
        {
            return this._multiplayerSessionManager.LocalPlayerHasState("in_gameplay");
        }
        set
        {
            this._multiplayerSessionManager.SetLocalPlayerState("in_gameplay", value);
        }
    }

    // Token: 0x06000128 RID: 296 RVA: 0x00005D50 File Offset: 0x00003F50
    public GameplayRpcManager(IMultiplayerSessionManager multiplayerSessionManager)
    {
        this._multiplayerSessionManager = multiplayerSessionManager;
        this._rpcHandler = new RpcHandler<GameplayRpcManager.RpcType>(this._multiplayerSessionManager, MultiplayerSessionManager.MessageType.GameplayRpc);
        this._rpcHandler.RegisterCallback<GameplayRpcManager.SetGameplaySceneSyncFinishedRpc, PlayerSpecificSettingsAtStartNetSerializable, string>(GameplayRpcManager.RpcType.SetGameplaySceneSyncFinish, new Action<string, PlayerSpecificSettingsAtStartNetSerializable, string>(this.InvokeSetGameplaySceneSyncFinishCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.SetGameplaySceneReadyRpc, PlayerSpecificSettingsNetSerializable>(GameplayRpcManager.RpcType.SetGameplaySceneReady, new Action<string, PlayerSpecificSettingsNetSerializable>(this.InvokeSetGameplaySceneReadyCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.GetGameplaySceneReadyRpc>(GameplayRpcManager.RpcType.GetGameplaySceneReady, new Action<string>(this.InvokeGetGameplaySceneReadyCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.SetPlayerDidConnectLateRpc, string, PlayerSpecificSettingsAtStartNetSerializable, string>(GameplayRpcManager.RpcType.SetActivePlayerFailedToConnect, new Action<string, string, PlayerSpecificSettingsAtStartNetSerializable, string>(this.InvokeSetPlayerDidConnectLate));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.SetSongStartTimeRpc, float>(GameplayRpcManager.RpcType.SetSongStartTime, new Action<string, float>(this.InvokeSetSongStartTimeCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.SetGameplaySongReadyRpc>(GameplayRpcManager.RpcType.SetGameplaySongReady, new Action<string>(this.InvokeSetGameplaySongReadyCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.GetGameplaySongReadyRpc>(GameplayRpcManager.RpcType.GetGameplaySongReady, new Action<string>(this.InvokeGetGameplayLevelReadyCallback));
        this._rpcHandler.RegisterCallbackWithTime<GameplayRpcManager.NoteSpawnedRpc, float, NoteSpawnInfoNetSerializable>(GameplayRpcManager.RpcType.NoteSpawned, new Action<string, float, float, NoteSpawnInfoNetSerializable>(this.InvokeNoteWasSpawnedCallback));
        this._rpcHandler.RegisterCallbackWithTime<GameplayRpcManager.ObstacleSpawnedRpc, float, ObstacleSpawnInfoNetSerializable>(GameplayRpcManager.RpcType.ObstacleSpawned, new Action<string, float, float, ObstacleSpawnInfoNetSerializable>(this.InvokeObstacleWasSpawnedCallback));
        this._rpcHandler.RegisterCallbackWithTime<GameplayRpcManager.SliderSpawnedRpc, float, SliderSpawnInfoNetSerializable>(GameplayRpcManager.RpcType.SliderSpawned, new Action<string, float, float, SliderSpawnInfoNetSerializable>(this.InvokeSliderWasSpawnedCallback));
        this._rpcHandler.RegisterCallbackWithTime<GameplayRpcManager.NoteCutRpc, float, NoteCutInfoNetSerializable>(GameplayRpcManager.RpcType.NoteCut, new Action<string, float, float, NoteCutInfoNetSerializable>(this.InvokeNoteWasCutCallback));
        this._rpcHandler.RegisterCallbackWithTime<GameplayRpcManager.NoteMissedRpc, float, NoteMissInfoNetSerializable>(GameplayRpcManager.RpcType.NoteMissed, new Action<string, float, float, NoteMissInfoNetSerializable>(this.InvokeNoteWasMissedCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.LevelFinishedRpc, MultiplayerLevelCompletionResults>(GameplayRpcManager.RpcType.LevelFinished, new Action<string, MultiplayerLevelCompletionResults>(this.InvokeLevelFinishedCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.ReturnToMenuRpc>(GameplayRpcManager.RpcType.ReturnToMenu, new Action<string>(this.InvokeReturnToMenuCallback));
        this._rpcHandler.RegisterCallback<GameplayRpcManager.RequestReturnToMenuRpc>(GameplayRpcManager.RpcType.RequestReturnToMenu, new Action<string>(this.InvokeRequestReturnToMenuCallback));
        this.enabled = true;
    }

    // Token: 0x06000129 RID: 297 RVA: 0x00005EF1 File Offset: 0x000040F1
    public void Dispose()
    {
        this.enabled = false;
        this._rpcHandler.Destroy();
    }

    // Token: 0x14000013 RID: 19
    // (add) Token: 0x0600012A RID: 298 RVA: 0x00005F08 File Offset: 0x00004108
    // (remove) Token: 0x0600012B RID: 299 RVA: 0x00005F40 File Offset: 0x00004140
    public event Action<string, PlayerSpecificSettingsAtStartNetSerializable, string> setGameplaySceneSyncFinishedEvent;

    // Token: 0x0600012C RID: 300 RVA: 0x00005F75 File Offset: 0x00004175
    public void SetGameplaySceneSyncFinished(PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable, string sessionGameId)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.SetGameplaySceneSyncFinishedRpc, PlayerSpecificSettingsAtStartNetSerializable, string>(playersAtGameStartNetSerializable, sessionGameId);
    }

    // Token: 0x0600012D RID: 301 RVA: 0x00005F84 File Offset: 0x00004184
    private void InvokeSetGameplaySceneSyncFinishCallback(string userId, PlayerSpecificSettingsAtStartNetSerializable playersAtGameStart, string sessionGameId)
    {
        Action<string, PlayerSpecificSettingsAtStartNetSerializable, string> action = this.setGameplaySceneSyncFinishedEvent;
        if (action == null)
        {
            return;
        }
        action(userId, playersAtGameStart, sessionGameId);
    }

    // Token: 0x14000014 RID: 20
    // (add) Token: 0x0600012E RID: 302 RVA: 0x00005F9C File Offset: 0x0000419C
    // (remove) Token: 0x0600012F RID: 303 RVA: 0x00005FD4 File Offset: 0x000041D4
    public event Action<string, PlayerSpecificSettingsNetSerializable> setGameplaySceneReadyEvent;

    // Token: 0x06000130 RID: 304 RVA: 0x00006009 File Offset: 0x00004209
    public void SetGameplaySceneReady(PlayerSpecificSettingsNetSerializable playerSpecificSettings)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.SetGameplaySceneReadyRpc, PlayerSpecificSettingsNetSerializable>(playerSpecificSettings);
    }

    // Token: 0x06000131 RID: 305 RVA: 0x00006017 File Offset: 0x00004217
    private void InvokeSetGameplaySceneReadyCallback(string userId, PlayerSpecificSettingsNetSerializable playerSpecificSettingsNetSerializable)
    {
        Action<string, PlayerSpecificSettingsNetSerializable> action = this.setGameplaySceneReadyEvent;
        if (action == null)
        {
            return;
        }
        action(userId, playerSpecificSettingsNetSerializable);
    }

    // Token: 0x14000015 RID: 21
    // (add) Token: 0x06000132 RID: 306 RVA: 0x0000602C File Offset: 0x0000422C
    // (remove) Token: 0x06000133 RID: 307 RVA: 0x00006064 File Offset: 0x00004264
    public event Action<string> getGameplaySceneReadyEvent;

    // Token: 0x06000134 RID: 308 RVA: 0x00006099 File Offset: 0x00004299
    public void GetGameplaySceneReady()
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.GetGameplaySceneReadyRpc>();
    }

    // Token: 0x06000135 RID: 309 RVA: 0x000060A6 File Offset: 0x000042A6
    private void InvokeGetGameplaySceneReadyCallback(string userId)
    {
        Action<string> action = this.getGameplaySceneReadyEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000016 RID: 22
    // (add) Token: 0x06000136 RID: 310 RVA: 0x000060BC File Offset: 0x000042BC
    // (remove) Token: 0x06000137 RID: 311 RVA: 0x000060F4 File Offset: 0x000042F4
    public event Action<string, string, PlayerSpecificSettingsAtStartNetSerializable, string> setPlayerDidConnectLateEvent;

    // Token: 0x06000138 RID: 312 RVA: 0x00006129 File Offset: 0x00004329
    public void SetPlayerDidConnectLate(string usedId, PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable, string sessionGameId)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.SetPlayerDidConnectLateRpc, string, PlayerSpecificSettingsAtStartNetSerializable, string>(usedId, playersAtGameStartNetSerializable, sessionGameId);
    }

    // Token: 0x06000139 RID: 313 RVA: 0x00006139 File Offset: 0x00004339
    private void InvokeSetPlayerDidConnectLate(string userId, string failedUserId, PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable, string sessionGameId)
    {
        Action<string, string, PlayerSpecificSettingsAtStartNetSerializable, string> action = this.setPlayerDidConnectLateEvent;
        if (action == null)
        {
            return;
        }
        action(userId, failedUserId, playersAtGameStartNetSerializable, sessionGameId);
    }

    // Token: 0x14000017 RID: 23
    // (add) Token: 0x0600013A RID: 314 RVA: 0x00006150 File Offset: 0x00004350
    // (remove) Token: 0x0600013B RID: 315 RVA: 0x00006188 File Offset: 0x00004388
    public event Action<string> setGameplaySongReadyEvent;

    // Token: 0x0600013C RID: 316 RVA: 0x000061BD File Offset: 0x000043BD
    public void SetGameplaySongReady()
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.SetGameplaySongReadyRpc>();
    }

    // Token: 0x0600013D RID: 317 RVA: 0x000061CA File Offset: 0x000043CA
    private void InvokeSetGameplaySongReadyCallback(string userId)
    {
        Action<string> action = this.setGameplaySongReadyEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000018 RID: 24
    // (add) Token: 0x0600013E RID: 318 RVA: 0x000061E0 File Offset: 0x000043E0
    // (remove) Token: 0x0600013F RID: 319 RVA: 0x00006218 File Offset: 0x00004418
    public event Action<string> getGameplaySongReadyEvent;

    // Token: 0x06000140 RID: 320 RVA: 0x0000624D File Offset: 0x0000444D
    public void GetGameplaySongReady()
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.GetGameplaySongReadyRpc>();
    }

    // Token: 0x06000141 RID: 321 RVA: 0x0000625A File Offset: 0x0000445A
    private void InvokeGetGameplayLevelReadyCallback(string userId)
    {
        Action<string> action = this.getGameplaySongReadyEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000019 RID: 25
    // (add) Token: 0x06000142 RID: 322 RVA: 0x00006270 File Offset: 0x00004470
    // (remove) Token: 0x06000143 RID: 323 RVA: 0x000062A8 File Offset: 0x000044A8
    public event Action<string, float> setSongStartTimeEvent;

    // Token: 0x06000144 RID: 324 RVA: 0x000062DD File Offset: 0x000044DD
    public void SetSongStartTime(float startTime)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.SetSongStartTimeRpc, float>(startTime);
    }

    // Token: 0x06000145 RID: 325 RVA: 0x000062EB File Offset: 0x000044EB
    private void InvokeSetSongStartTimeCallback(string userId, float startTime)
    {
        Action<string, float> action = this.setSongStartTimeEvent;
        if (action == null)
        {
            return;
        }
        action(userId, startTime);
    }

    // Token: 0x1400001A RID: 26
    // (add) Token: 0x06000146 RID: 326 RVA: 0x00006300 File Offset: 0x00004500
    // (remove) Token: 0x06000147 RID: 327 RVA: 0x00006338 File Offset: 0x00004538
    public event Action<string, float, float, NoteSpawnInfoNetSerializable> noteWasSpawnedEvent;

    // Token: 0x06000148 RID: 328 RVA: 0x0000636D File Offset: 0x0000456D
    public void NoteSpawned(float songTime, NoteSpawnInfoNetSerializable noteSpawnInfoNetSerializable)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.NoteSpawnedRpc, float, NoteSpawnInfoNetSerializable>(songTime, noteSpawnInfoNetSerializable);
    }

    // Token: 0x06000149 RID: 329 RVA: 0x0000637C File Offset: 0x0000457C
    private void InvokeNoteWasSpawnedCallback(string userId, float syncTime, float songTime, NoteSpawnInfoNetSerializable noteSpawnInfo)
    {
        Action<string, float, float, NoteSpawnInfoNetSerializable> action = this.noteWasSpawnedEvent;
        if (action == null)
        {
            return;
        }
        action(userId, syncTime, songTime, noteSpawnInfo);
    }

    // Token: 0x1400001B RID: 27
    // (add) Token: 0x0600014A RID: 330 RVA: 0x00006394 File Offset: 0x00004594
    // (remove) Token: 0x0600014B RID: 331 RVA: 0x000063CC File Offset: 0x000045CC
    public event Action<string, float, float, ObstacleSpawnInfoNetSerializable> obstacleWasSpawnedEvent;

    // Token: 0x0600014C RID: 332 RVA: 0x00006401 File Offset: 0x00004601
    public void ObstacleSpawned(float songTime, ObstacleSpawnInfoNetSerializable obstacleSpawnInfoNetSerializable)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.ObstacleSpawnedRpc, float, ObstacleSpawnInfoNetSerializable>(songTime, obstacleSpawnInfoNetSerializable);
    }

    // Token: 0x0600014D RID: 333 RVA: 0x00006410 File Offset: 0x00004610
    private void InvokeObstacleWasSpawnedCallback(string userId, float syncTime, float songTime, ObstacleSpawnInfoNetSerializable obstacleSpawnInfo)
    {
        Action<string, float, float, ObstacleSpawnInfoNetSerializable> action = this.obstacleWasSpawnedEvent;
        if (action == null)
        {
            return;
        }
        action(userId, syncTime, songTime, obstacleSpawnInfo);
    }

    // Token: 0x1400001C RID: 28
    // (add) Token: 0x0600014E RID: 334 RVA: 0x00006428 File Offset: 0x00004628
    // (remove) Token: 0x0600014F RID: 335 RVA: 0x00006460 File Offset: 0x00004660
    public event Action<string, float, float, SliderSpawnInfoNetSerializable> sliderWasSpawnedEvent;

    // Token: 0x06000150 RID: 336 RVA: 0x00006495 File Offset: 0x00004695
    public void SliderSpawned(float songTime, SliderSpawnInfoNetSerializable sliderSpawnInfoNetSerializable)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.SliderSpawnedRpc, float, SliderSpawnInfoNetSerializable>(songTime, sliderSpawnInfoNetSerializable);
    }

    // Token: 0x06000151 RID: 337 RVA: 0x000064A4 File Offset: 0x000046A4
    private void InvokeSliderWasSpawnedCallback(string userId, float syncTime, float songTime, SliderSpawnInfoNetSerializable sliderSpawnInfo)
    {
        Action<string, float, float, SliderSpawnInfoNetSerializable> action = this.sliderWasSpawnedEvent;
        if (action == null)
        {
            return;
        }
        action(userId, syncTime, songTime, sliderSpawnInfo);
    }

    // Token: 0x1400001D RID: 29
    // (add) Token: 0x06000152 RID: 338 RVA: 0x000064BC File Offset: 0x000046BC
    // (remove) Token: 0x06000153 RID: 339 RVA: 0x000064F4 File Offset: 0x000046F4
    public event Action<string, float, float, NoteCutInfoNetSerializable> noteWasCutEvent;

    // Token: 0x06000154 RID: 340 RVA: 0x00006529 File Offset: 0x00004729
    public void NoteCut(float songTime, NoteCutInfoNetSerializable noteCutInfoNetSerializable)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.NoteCutRpc, float, NoteCutInfoNetSerializable>(songTime, noteCutInfoNetSerializable);
    }

    // Token: 0x06000155 RID: 341 RVA: 0x00006538 File Offset: 0x00004738
    private void InvokeNoteWasCutCallback(string userId, float syncTime, float songTime, NoteCutInfoNetSerializable noteCutInfo)
    {
        Action<string, float, float, NoteCutInfoNetSerializable> action = this.noteWasCutEvent;
        if (action == null)
        {
            return;
        }
        action(userId, syncTime, songTime, noteCutInfo);
    }

    // Token: 0x1400001E RID: 30
    // (add) Token: 0x06000156 RID: 342 RVA: 0x00006550 File Offset: 0x00004750
    // (remove) Token: 0x06000157 RID: 343 RVA: 0x00006588 File Offset: 0x00004788
    public event Action<string, float, float, NoteMissInfoNetSerializable> noteWasMissedEvent;

    // Token: 0x06000158 RID: 344 RVA: 0x000065BD File Offset: 0x000047BD
    public void NoteMissed(float songTime, NoteMissInfoNetSerializable noteMissInfoNetSerializable)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.NoteMissedRpc, float, NoteMissInfoNetSerializable>(songTime, noteMissInfoNetSerializable);
    }

    // Token: 0x06000159 RID: 345 RVA: 0x000065CC File Offset: 0x000047CC
    private void InvokeNoteWasMissedCallback(string userId, float syncTime, float songTime, NoteMissInfoNetSerializable noteMissInfo)
    {
        Action<string, float, float, NoteMissInfoNetSerializable> action = this.noteWasMissedEvent;
        if (action == null)
        {
            return;
        }
        action(userId, syncTime, songTime, noteMissInfo);
    }

    // Token: 0x1400001F RID: 31
    // (add) Token: 0x0600015A RID: 346 RVA: 0x000065E4 File Offset: 0x000047E4
    // (remove) Token: 0x0600015B RID: 347 RVA: 0x0000661C File Offset: 0x0000481C
    public event Action<string, MultiplayerLevelCompletionResults> levelFinishedEvent;

    // Token: 0x0600015C RID: 348 RVA: 0x00006651 File Offset: 0x00004851
    public void LevelFinished(MultiplayerLevelCompletionResults results)
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.LevelFinishedRpc, MultiplayerLevelCompletionResults>(results);
    }

    // Token: 0x0600015D RID: 349 RVA: 0x0000665F File Offset: 0x0000485F
    private void InvokeLevelFinishedCallback(string userId, MultiplayerLevelCompletionResults results)
    {
        Action<string, MultiplayerLevelCompletionResults> action = this.levelFinishedEvent;
        if (action == null)
        {
            return;
        }
        action(userId, results);
    }

    // Token: 0x14000020 RID: 32
    // (add) Token: 0x0600015E RID: 350 RVA: 0x00006674 File Offset: 0x00004874
    // (remove) Token: 0x0600015F RID: 351 RVA: 0x000066AC File Offset: 0x000048AC
    public event Action<string> returnToMenuEvent;

    // Token: 0x06000160 RID: 352 RVA: 0x000066E1 File Offset: 0x000048E1
    public void ReturnToMenu()
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.ReturnToMenuRpc>();
    }

    // Token: 0x06000161 RID: 353 RVA: 0x000066EE File Offset: 0x000048EE
    private void InvokeReturnToMenuCallback(string userId)
    {
        Action<string> action = this.returnToMenuEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x14000021 RID: 33
    // (add) Token: 0x06000162 RID: 354 RVA: 0x00006704 File Offset: 0x00004904
    // (remove) Token: 0x06000163 RID: 355 RVA: 0x0000673C File Offset: 0x0000493C
    public event Action<string> requestReturnToMenuEvent;

    // Token: 0x06000164 RID: 356 RVA: 0x00006771 File Offset: 0x00004971
    public void RequestReturnToMenu()
    {
        this._rpcHandler.EnqueueRpc<GameplayRpcManager.RequestReturnToMenuRpc>();
    }

    // Token: 0x06000165 RID: 357 RVA: 0x0000677E File Offset: 0x0000497E
    private void InvokeRequestReturnToMenuCallback(string userId)
    {
        Action<string> action = this.requestReturnToMenuEvent;
        if (action == null)
        {
            return;
        }
        action(userId);
    }

    // Token: 0x040000C8 RID: 200
    private const string kGameplayState = "in_gameplay";

    // Token: 0x040000C9 RID: 201
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;

    // Token: 0x040000CA RID: 202
    private readonly RpcHandler<GameplayRpcManager.RpcType> _rpcHandler;

    // Token: 0x020000E7 RID: 231
    public enum RpcType : byte
    {
        // Token: 0x0400036F RID: 879
        SetGameplaySceneSyncFinish,
        // Token: 0x04000370 RID: 880
        SetGameplaySceneReady,
        // Token: 0x04000371 RID: 881
        GetGameplaySceneReady,
        // Token: 0x04000372 RID: 882
        SetActivePlayerFailedToConnect,
        // Token: 0x04000373 RID: 883
        SetGameplaySongReady,
        // Token: 0x04000374 RID: 884
        GetGameplaySongReady,
        // Token: 0x04000375 RID: 885
        SetSongStartTime,
        // Token: 0x04000376 RID: 886
        NoteCut,
        // Token: 0x04000377 RID: 887
        NoteMissed,
        // Token: 0x04000378 RID: 888
        LevelFinished,
        // Token: 0x04000379 RID: 889
        ReturnToMenu,
        // Token: 0x0400037A RID: 890
        RequestReturnToMenu,
        // Token: 0x0400037B RID: 891
        NoteSpawned,
        // Token: 0x0400037C RID: 892
        ObstacleSpawned,
        // Token: 0x0400037D RID: 893
        SliderSpawned
    }

    // Token: 0x020000E8 RID: 232
    private class SetGameplaySceneSyncFinishedRpc : RemoteProcedureCall<PlayerSpecificSettingsAtStartNetSerializable, string>
    {
    }

    // Token: 0x020000E9 RID: 233
    private class SetGameplaySceneReadyRpc : RemoteProcedureCall<PlayerSpecificSettingsNetSerializable>
    {
    }

    // Token: 0x020000EA RID: 234
    private class GetGameplaySceneReadyRpc : RemoteProcedureCall
    {
    }

    // Token: 0x020000EB RID: 235
    private class SetPlayerDidConnectLateRpc : RemoteProcedureCall<string, PlayerSpecificSettingsAtStartNetSerializable, string>
    {
    }

    // Token: 0x020000EC RID: 236
    private class SetGameplaySongReadyRpc : RemoteProcedureCall
    {
    }

    // Token: 0x020000ED RID: 237
    private class GetGameplaySongReadyRpc : RemoteProcedureCall
    {
    }

    // Token: 0x020000EE RID: 238
    private class SetSongStartTimeRpc : RemoteProcedureCall<float>
    {
    }

    // Token: 0x020000EF RID: 239
    private class NoteSpawnedRpc : RemoteProcedureCall<float, NoteSpawnInfoNetSerializable>
    {
    }

    // Token: 0x020000F0 RID: 240
    private class ObstacleSpawnedRpc : RemoteProcedureCall<float, ObstacleSpawnInfoNetSerializable>
    {
    }

    // Token: 0x020000F1 RID: 241
    private class SliderSpawnedRpc : RemoteProcedureCall<float, SliderSpawnInfoNetSerializable>
    {
    }

    // Token: 0x020000F2 RID: 242
    private class NoteCutRpc : RemoteProcedureCall<float, NoteCutInfoNetSerializable>
    {
    }

    // Token: 0x020000F3 RID: 243
    private class NoteMissedRpc : RemoteProcedureCall<float, NoteMissInfoNetSerializable>
    {
    }

    // Token: 0x020000F4 RID: 244
    private class LevelFinishedRpc : RemoteProcedureCall<MultiplayerLevelCompletionResults>
    {
    }

    // Token: 0x020000F5 RID: 245
    private class ReturnToMenuRpc : RemoteProcedureCall
    {
    }

    // Token: 0x020000F6 RID: 246
    private class RequestReturnToMenuRpc : RemoteProcedureCall
    {
    }
}
