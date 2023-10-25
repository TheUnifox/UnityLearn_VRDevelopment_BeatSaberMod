using System;

// Token: 0x0200003A RID: 58
public interface IGameplayRpcManager : IDisposable
{
    // Token: 0x1700005E RID: 94
    // (get) Token: 0x060001D2 RID: 466
    // (set) Token: 0x060001D3 RID: 467
    bool enabled { get; set; }

    // Token: 0x1400002E RID: 46
    // (add) Token: 0x060001D4 RID: 468
    // (remove) Token: 0x060001D5 RID: 469
    event Action<string, PlayerSpecificSettingsAtStartNetSerializable, string> setGameplaySceneSyncFinishedEvent;

    // Token: 0x1400002F RID: 47
    // (add) Token: 0x060001D6 RID: 470
    // (remove) Token: 0x060001D7 RID: 471
    event Action<string, PlayerSpecificSettingsNetSerializable> setGameplaySceneReadyEvent;

    // Token: 0x14000030 RID: 48
    // (add) Token: 0x060001D8 RID: 472
    // (remove) Token: 0x060001D9 RID: 473
    event Action<string> getGameplaySceneReadyEvent;

    // Token: 0x14000031 RID: 49
    // (add) Token: 0x060001DA RID: 474
    // (remove) Token: 0x060001DB RID: 475
    event Action<string, string, PlayerSpecificSettingsAtStartNetSerializable, string> setPlayerDidConnectLateEvent;

    // Token: 0x14000032 RID: 50
    // (add) Token: 0x060001DC RID: 476
    // (remove) Token: 0x060001DD RID: 477
    event Action<string> setGameplaySongReadyEvent;

    // Token: 0x14000033 RID: 51
    // (add) Token: 0x060001DE RID: 478
    // (remove) Token: 0x060001DF RID: 479
    event Action<string> getGameplaySongReadyEvent;

    // Token: 0x14000034 RID: 52
    // (add) Token: 0x060001E0 RID: 480
    // (remove) Token: 0x060001E1 RID: 481
    event Action<string, float> setSongStartTimeEvent;

    // Token: 0x14000035 RID: 53
    // (add) Token: 0x060001E2 RID: 482
    // (remove) Token: 0x060001E3 RID: 483
    event Action<string> requestReturnToMenuEvent;

    // Token: 0x14000036 RID: 54
    // (add) Token: 0x060001E4 RID: 484
    // (remove) Token: 0x060001E5 RID: 485
    event Action<string> returnToMenuEvent;

    // Token: 0x14000037 RID: 55
    // (add) Token: 0x060001E6 RID: 486
    // (remove) Token: 0x060001E7 RID: 487
    event Action<string, MultiplayerLevelCompletionResults> levelFinishedEvent;

    // Token: 0x14000038 RID: 56
    // (add) Token: 0x060001E8 RID: 488
    // (remove) Token: 0x060001E9 RID: 489
    event Action<string, float, float, NoteSpawnInfoNetSerializable> noteWasSpawnedEvent;

    // Token: 0x14000039 RID: 57
    // (add) Token: 0x060001EA RID: 490
    // (remove) Token: 0x060001EB RID: 491
    event Action<string, float, float, ObstacleSpawnInfoNetSerializable> obstacleWasSpawnedEvent;

    // Token: 0x1400003A RID: 58
    // (add) Token: 0x060001EC RID: 492
    // (remove) Token: 0x060001ED RID: 493
    event Action<string, float, float, SliderSpawnInfoNetSerializable> sliderWasSpawnedEvent;

    // Token: 0x1400003B RID: 59
    // (add) Token: 0x060001EE RID: 494
    // (remove) Token: 0x060001EF RID: 495
    event Action<string, float, float, NoteCutInfoNetSerializable> noteWasCutEvent;

    // Token: 0x1400003C RID: 60
    // (add) Token: 0x060001F0 RID: 496
    // (remove) Token: 0x060001F1 RID: 497
    event Action<string, float, float, NoteMissInfoNetSerializable> noteWasMissedEvent;

    // Token: 0x060001F2 RID: 498
    void NoteSpawned(float songTime, NoteSpawnInfoNetSerializable noteSpawnInfoNetSerializable);

    // Token: 0x060001F3 RID: 499
    void ObstacleSpawned(float songTime, ObstacleSpawnInfoNetSerializable obstacleSpawnInfoNetSerializable);

    // Token: 0x060001F4 RID: 500
    void SliderSpawned(float songTime, SliderSpawnInfoNetSerializable sliderSpawnInfoNetSerializable);

    // Token: 0x060001F5 RID: 501
    void NoteMissed(float songTime, NoteMissInfoNetSerializable noteMissInfoNetSerializable);

    // Token: 0x060001F6 RID: 502
    void NoteCut(float songTime, NoteCutInfoNetSerializable noteCutInfoNetSerializable);

    // Token: 0x060001F7 RID: 503
    void SetGameplaySceneSyncFinished(PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable, string sessionGameId);

    // Token: 0x060001F8 RID: 504
    void SetGameplaySceneReady(PlayerSpecificSettingsNetSerializable playerSpecificSettings);

    // Token: 0x060001F9 RID: 505
    void GetGameplaySceneReady();

    // Token: 0x060001FA RID: 506
    void SetPlayerDidConnectLate(string userId, PlayerSpecificSettingsAtStartNetSerializable playersAtGameStartNetSerializable, string sessionGameId);

    // Token: 0x060001FB RID: 507
    void SetSongStartTime(float startTime);

    // Token: 0x060001FC RID: 508
    void SetGameplaySongReady();

    // Token: 0x060001FD RID: 509
    void GetGameplaySongReady();

    // Token: 0x060001FE RID: 510
    void RequestReturnToMenu();

    // Token: 0x060001FF RID: 511
    void ReturnToMenu();

    // Token: 0x06000200 RID: 512
    void LevelFinished(MultiplayerLevelCompletionResults results);
}
