using System;

// Token: 0x0200003C RID: 60
public interface IMenuRpcManager : IDisposable
{
    // Token: 0x1700005F RID: 95
    // (get) Token: 0x06000201 RID: 513
    float syncTime { get; }

    // Token: 0x1400003D RID: 61
    // (add) Token: 0x06000202 RID: 514
    // (remove) Token: 0x06000203 RID: 515
    event Action<string> getPlayersPermissionConfigurationEvent;

    // Token: 0x1400003E RID: 62
    // (add) Token: 0x06000204 RID: 516
    // (remove) Token: 0x06000205 RID: 517
    event Action<string, PlayersLobbyPermissionConfigurationNetSerializable> setPlayersPermissionConfigurationEvent;

    // Token: 0x1400003F RID: 63
    // (add) Token: 0x06000206 RID: 518
    // (remove) Token: 0x06000207 RID: 519
    event Action<string, BeatmapIdentifierNetSerializable> recommendBeatmapEvent;

    // Token: 0x14000040 RID: 64
    // (add) Token: 0x06000208 RID: 520
    // (remove) Token: 0x06000209 RID: 521
    event Action<string> getRecommendedBeatmapEvent;

    // Token: 0x14000041 RID: 65
    // (add) Token: 0x0600020A RID: 522
    // (remove) Token: 0x0600020B RID: 523
    event Action<string> clearRecommendedBeatmapEvent;

    // Token: 0x14000042 RID: 66
    // (add) Token: 0x0600020C RID: 524
    // (remove) Token: 0x0600020D RID: 525
    event Action<string, BeatmapIdentifierNetSerializable> setSelectedBeatmapEvent;

    // Token: 0x14000043 RID: 67
    // (add) Token: 0x0600020E RID: 526
    // (remove) Token: 0x0600020F RID: 527
    event Action<string> clearSelectedBeatmapEvent;

    // Token: 0x14000044 RID: 68
    // (add) Token: 0x06000210 RID: 528
    // (remove) Token: 0x06000211 RID: 529
    event Action<string> getSelectedBeatmapEvent;

    // Token: 0x14000045 RID: 69
    // (add) Token: 0x06000212 RID: 530
    // (remove) Token: 0x06000213 RID: 531
    event Action<string, GameplayModifiers> recommendGameplayModifiersEvent;

    // Token: 0x14000046 RID: 70
    // (add) Token: 0x06000214 RID: 532
    // (remove) Token: 0x06000215 RID: 533
    event Action<string> getRecommendedGameplayModifiersEvent;

    // Token: 0x14000047 RID: 71
    // (add) Token: 0x06000216 RID: 534
    // (remove) Token: 0x06000217 RID: 535
    event Action<string> clearRecommendedGameplayModifiersEvent;

    // Token: 0x14000048 RID: 72
    // (add) Token: 0x06000218 RID: 536
    // (remove) Token: 0x06000219 RID: 537
    event Action<string, GameplayModifiers> setSelectedGameplayModifiersEvent;

    // Token: 0x14000049 RID: 73
    // (add) Token: 0x0600021A RID: 538
    // (remove) Token: 0x0600021B RID: 539
    event Action<string> clearSelectedGameplayModifiersEvent;

    // Token: 0x1400004A RID: 74
    // (add) Token: 0x0600021C RID: 540
    // (remove) Token: 0x0600021D RID: 541
    event Action<string> getSelectedGameplayModifiersEvent;

    // Token: 0x1400004B RID: 75
    // (add) Token: 0x0600021E RID: 542
    // (remove) Token: 0x0600021F RID: 543
    event Action<string> getIsStartButtonEnabledEvent;

    // Token: 0x1400004C RID: 76
    // (add) Token: 0x06000220 RID: 544
    // (remove) Token: 0x06000221 RID: 545
    event Action<string, CannotStartGameReason> setIsStartButtonEnabledEvent;

    // Token: 0x1400004D RID: 77
    // (add) Token: 0x06000222 RID: 546
    // (remove) Token: 0x06000223 RID: 547
    event Action<string> getIsReadyEvent;

    // Token: 0x1400004E RID: 78
    // (add) Token: 0x06000224 RID: 548
    // (remove) Token: 0x06000225 RID: 549
    event Action<string, bool> setIsReadyEvent;

    // Token: 0x1400004F RID: 79
    // (add) Token: 0x06000226 RID: 550
    // (remove) Token: 0x06000227 RID: 551
    event Action<string, float> setStartGameTimeEvent;

    // Token: 0x14000050 RID: 80
    // (add) Token: 0x06000228 RID: 552
    // (remove) Token: 0x06000229 RID: 553
    event Action<string> cancelledLevelStartEvent;

    // Token: 0x14000051 RID: 81
    // (add) Token: 0x0600022A RID: 554
    // (remove) Token: 0x0600022B RID: 555
    event Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float> startedLevelEvent;

    // Token: 0x14000052 RID: 82
    // (add) Token: 0x0600022C RID: 556
    // (remove) Token: 0x0600022D RID: 557
    event Action<string> getStartedLevelEvent;

    // Token: 0x14000053 RID: 83
    // (add) Token: 0x0600022E RID: 558
    // (remove) Token: 0x0600022F RID: 559
    event Action<string> getMultiplayerGameStateEvent;

    // Token: 0x14000054 RID: 84
    // (add) Token: 0x06000230 RID: 560
    // (remove) Token: 0x06000231 RID: 561
    event Action<string, MultiplayerGameState> setMultiplayerGameStateEvent;

    // Token: 0x14000055 RID: 85
    // (add) Token: 0x06000232 RID: 562
    // (remove) Token: 0x06000233 RID: 563
    event Action<string, string> getIsEntitledToLevelEvent;

    // Token: 0x14000056 RID: 86
    // (add) Token: 0x06000234 RID: 564
    // (remove) Token: 0x06000235 RID: 565
    event Action<string, PlayersMissingEntitlementsNetSerializable> setPlayersMissingEntitlementsToLevelEvent;

    // Token: 0x14000057 RID: 87
    // (add) Token: 0x06000236 RID: 566
    // (remove) Token: 0x06000237 RID: 567
    event Action<string, string, EntitlementsStatus> setIsEntitledToLevelEvent;

    // Token: 0x14000058 RID: 88
    // (add) Token: 0x06000238 RID: 568
    // (remove) Token: 0x06000239 RID: 569
    event Action<string> levelEntitlementStatusesInvalidatedEvent;

    // Token: 0x14000059 RID: 89
    // (add) Token: 0x0600023A RID: 570
    // (remove) Token: 0x0600023B RID: 571
    event Action<string> getIsInLobbyEvent;

    // Token: 0x1400005A RID: 90
    // (add) Token: 0x0600023C RID: 572
    // (remove) Token: 0x0600023D RID: 573
    event Action<string, bool> setIsInLobbyEvent;

    // Token: 0x1400005B RID: 91
    // (add) Token: 0x0600023E RID: 574
    // (remove) Token: 0x0600023F RID: 575
    event Action<string> cancelCountdownEvent;

    // Token: 0x1400005C RID: 92
    // (add) Token: 0x06000240 RID: 576
    // (remove) Token: 0x06000241 RID: 577
    event Action<string, float> setCountdownEndTimeEvent;

    // Token: 0x1400005D RID: 93
    // (add) Token: 0x06000242 RID: 578
    // (remove) Token: 0x06000243 RID: 579
    event Action<string, SongPackMask> setOwnedSongPacksEvent;

    // Token: 0x1400005E RID: 94
    // (add) Token: 0x06000244 RID: 580
    // (remove) Token: 0x06000245 RID: 581
    event Action<string> getOwnedSongPacksEvent;

    // Token: 0x1400005F RID: 95
    // (add) Token: 0x06000246 RID: 582
    // (remove) Token: 0x06000247 RID: 583
    event Action<string, string> requestedKickPlayerEvent;

    // Token: 0x06000248 RID: 584
    void GetPlayersPermissionConfiguration();

    // Token: 0x06000249 RID: 585
    void SetPlayersPermissionConfiguration(PlayersLobbyPermissionConfigurationNetSerializable permissions);

    // Token: 0x0600024A RID: 586
    void RecommendBeatmap(BeatmapIdentifierNetSerializable identifier);

    // Token: 0x0600024B RID: 587
    void GetRecommendedBeatmap();

    // Token: 0x0600024C RID: 588
    void ClearRecommendedBeatmap();

    // Token: 0x0600024D RID: 589
    void SetSelectedBeatmap(BeatmapIdentifierNetSerializable identifier);

    // Token: 0x0600024E RID: 590
    void GetSelectedBeatmap();

    // Token: 0x0600024F RID: 591
    void RecommendGameplayModifiers(GameplayModifiers gameplayModifiers);

    // Token: 0x06000250 RID: 592
    void GetRecommendedGameplayModifiers();

    // Token: 0x06000251 RID: 593
    void ClearRecommendedGameplayModifiers();

    // Token: 0x06000252 RID: 594
    void SetSelectedGameplayModifiers(GameplayModifiers gameplayModifiers);

    // Token: 0x06000253 RID: 595
    void GetSelectedGameplayModifiers();

    // Token: 0x06000254 RID: 596
    void GetIsReady();

    // Token: 0x06000255 RID: 597
    void SetIsReady(bool isReady);

    // Token: 0x06000256 RID: 598
    void GetIsStartButtonEnabled();

    // Token: 0x06000257 RID: 599
    void SetIsStartButtonEnabled(CannotStartGameReason reason);

    // Token: 0x06000258 RID: 600
    void GetIsInLobby();

    // Token: 0x06000259 RID: 601
    void SetIsInLobby(bool isBack);

    // Token: 0x0600025A RID: 602
    void SetPlayersMissingEntitlementsToLevel(PlayersMissingEntitlementsNetSerializable playersMissingEntitlements);

    // Token: 0x0600025B RID: 603
    void GetIsEntitledToLevel(string levelId);

    // Token: 0x0600025C RID: 604
    void SetIsEntitledToLevel(string levelId, EntitlementsStatus entitlementStatus);

    // Token: 0x0600025D RID: 605
    void InvalidateLevelEntitlementStatuses();

    // Token: 0x0600025E RID: 606
    void GetMultiplayerGameState();

    // Token: 0x0600025F RID: 607
    void SetMultiplayerGameState(MultiplayerGameState lobbyState);

    // Token: 0x06000260 RID: 608
    void GetOwnedSongPacks();

    // Token: 0x06000261 RID: 609
    void SetOwnedSongPacks(SongPackMask songPackMask);

    // Token: 0x06000262 RID: 610
    void StartLevel(BeatmapIdentifierNetSerializable beatmapId, GameplayModifiers gameplayModifiers, float startTime);

    // Token: 0x06000263 RID: 611
    void GetStartedLevel();

    // Token: 0x06000264 RID: 612
    void CancelLevelStart();

    // Token: 0x06000265 RID: 613
    void SetStartGameTime(float newTime);

    // Token: 0x06000266 RID: 614
    void GetCountdownEndTime();

    // Token: 0x06000267 RID: 615
    void RequestKickPlayer(string kickedPlayerId);
}
