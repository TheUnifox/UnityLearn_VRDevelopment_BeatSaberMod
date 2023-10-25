// Decompiled with JetBrains decompiler
// Type: PlayerDataFileManagerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataFileManagerSO : PersistentScriptableObject
{
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  [SerializeField]
  protected ColorSchemesListSO _defaultColorSchemes;
  [SerializeField]
  protected EnvironmentsListSO _allEnvironmentInfos;
  [SerializeField]
  protected EnvironmentTypeSO _normalEnvironmentType;
  [SerializeField]
  protected EnvironmentTypeSO _a360DegreesEnvironmentType;
  [SerializeField]
  protected BeatmapCharacteristicSO _defaultLastSelectedBeatmapCharacteristic;
  [SerializeField]
  protected string _buildInSongPackSerializedName = "BUILT_IN_LEVEL_PACKS";
  [SerializeField]
  protected string _allSongPackSerializedName = "ALL_LEVEL_PACKS";
  protected const string kPlayerDataFileName = "PlayerData.dat";
  protected const string kTempFileName = "PlayerData.dat.tmp";
  protected const string kBackupFileName = "PlayerData.dat.bak";
  [DoesNotRequireDomainReloadInit]
  protected static readonly Version _lastVersionWithoutSavedCustomColorSchemeBoostColors = new Version("2.0.21");
  [DoesNotRequireDomainReloadInit]
  protected static readonly Version _eulaUpdateVersion = new Version("2.0.18");
  [DoesNotRequireDomainReloadInit]
  protected static readonly Version _lastVersionWithoutArcsOptions = new Version("2.0.22");

  public virtual void Save(PlayerData playerData)
  {
    PlayerSaveData playerSaveData = new PlayerSaveData()
    {
      localPlayers = new List<PlayerSaveData.LocalPlayer>(1)
    };
    PlayerSaveData.LocalPlayer localPlayer = new PlayerSaveData.LocalPlayer();
    playerSaveData.localPlayers.Add(localPlayer);
    localPlayer.playerName = playerData.playerName;
    localPlayer.playerId = playerData.playerId;
    localPlayer.shouldShowTutorialPrompt = playerData.shouldShowTutorialPrompt;
    localPlayer.shouldShow360Warning = playerData.shouldShow360Warning;
    localPlayer.agreedToEula = playerData.agreedToEula;
    localPlayer.didSelectLanguage = playerData.didSelectLanguage;
    localPlayer.didSelectRegionVersion = playerData.didSelectRegionVersion;
    localPlayer.agreedToMultiplayerDisclaimer = playerData.agreedToMultiplayerDisclaimer;
    localPlayer.avatarCreated = playerData.avatarCreated;
    localPlayer.lastSelectedBeatmapDifficulty = playerData.lastSelectedBeatmapDifficulty;
    localPlayer.lastSelectedBeatmapCharacteristicName = playerData.lastSelectedBeatmapCharacteristic.serializedName;
    localPlayer.currentDlcPromoDisplayCount = playerData.currentDlcPromoDisplayCount;
    localPlayer.currentDlcPromoId = playerData.currentDlcPromoId;
    localPlayer.gameplayModifiers = new PlayerSaveData.GameplayModifiers()
    {
      energyType = (PlayerSaveData.GameplayModifiers.EnergyType) playerData.gameplayModifiers.energyType,
      noFailOn0Energy = playerData.gameplayModifiers.noFailOn0Energy,
      instaFail = playerData.gameplayModifiers.instaFail,
      failOnSaberClash = playerData.gameplayModifiers.failOnSaberClash,
      enabledObstacleType = (PlayerSaveData.GameplayModifiers.EnabledObstacleType) playerData.gameplayModifiers.enabledObstacleType,
      fastNotes = playerData.gameplayModifiers.fastNotes,
      strictAngles = playerData.gameplayModifiers.strictAngles,
      disappearingArrows = playerData.gameplayModifiers.disappearingArrows,
      ghostNotes = playerData.gameplayModifiers.ghostNotes,
      noBombs = playerData.gameplayModifiers.noBombs,
      songSpeed = (PlayerSaveData.GameplayModifiers.SongSpeed) playerData.gameplayModifiers.songSpeed,
      noArrows = playerData.gameplayModifiers.noArrows,
      proMode = playerData.gameplayModifiers.proMode,
      zenMode = playerData.gameplayModifiers.zenMode,
      smallCubes = playerData.gameplayModifiers.smallCubes
    };
    localPlayer.playerSpecificSettings = new PlayerSaveData.PlayerSpecificSettings()
    {
      leftHanded = playerData.playerSpecificSettings.leftHanded,
      playerHeight = playerData.playerSpecificSettings.playerHeight,
      automaticPlayerHeight = playerData.playerSpecificSettings.automaticPlayerHeight,
      sfxVolume = playerData.playerSpecificSettings.sfxVolume,
      reduceDebris = playerData.playerSpecificSettings.reduceDebris,
      advancedHud = playerData.playerSpecificSettings.advancedHud,
      noTextsAndHuds = playerData.playerSpecificSettings.noTextsAndHuds,
      saberTrailIntensity = playerData.playerSpecificSettings.saberTrailIntensity,
      _noteJumpDurationTypeSettingsSaveData = playerData.playerSpecificSettings.noteJumpDurationTypeSettings.GetSaveData(),
      noteJumpFixedDuration = playerData.playerSpecificSettings.noteJumpFixedDuration,
      autoRestart = playerData.playerSpecificSettings.autoRestart,
      noFailEffects = playerData.playerSpecificSettings.noFailEffects,
      noteJumpBeatOffset = playerData.playerSpecificSettings.noteJumpStartBeatOffset,
      hideNoteSpawnEffect = playerData.playerSpecificSettings.hideNoteSpawnEffect,
      adaptiveSfx = playerData.playerSpecificSettings.adaptiveSfx,
      arcsHapticFeedback = playerData.playerSpecificSettings.arcsHapticFeedback,
      arcsVisibleSaveData = playerData.playerSpecificSettings.arcsVisible.GetSaveData(),
      environmentEffectsFilterDefaultPreset = playerData.playerSpecificSettings.environmentEffectsFilterDefaultPreset.GetSaveData(),
      environmentEffectsFilterExpertPlusPreset = playerData.playerSpecificSettings.environmentEffectsFilterExpertPlusPreset.GetSaveData()
    };
    localPlayer.practiceSettings = new PlayerSaveData.PracticeSettings()
    {
      songSpeedMul = playerData.practiceSettings.songSpeedMul,
      startSongTime = playerData.practiceSettings.startSongTime
    };
    localPlayer.playerAllOverallStatsData = playerData.playerAllOverallStatsData.ToPlayerAllOverallStatsData();
    localPlayer.levelsStatsData = new List<PlayerSaveData.PlayerLevelStatsData>(playerData.levelsStatsData.Count);
    foreach (PlayerLevelStatsData playerLevelStatsData1 in playerData.levelsStatsData)
    {
      PlayerSaveData.PlayerLevelStatsData playerLevelStatsData2 = new PlayerSaveData.PlayerLevelStatsData()
      {
        levelId = playerLevelStatsData1.levelID,
        difficulty = playerLevelStatsData1.difficulty,
        beatmapCharacteristicName = playerLevelStatsData1.beatmapCharacteristic.serializedName,
        highScore = playerLevelStatsData1.highScore,
        maxCombo = playerLevelStatsData1.maxCombo,
        fullCombo = playerLevelStatsData1.fullCombo,
        maxRank = playerLevelStatsData1.maxRank,
        playCount = playerLevelStatsData1.playCount,
        validScore = playerLevelStatsData1.validScore
      };
      localPlayer.levelsStatsData.Add(playerLevelStatsData2);
    }
    localPlayer.missionsStatsData = new List<PlayerSaveData.PlayerMissionStatsData>(playerData.missionsStatsData.Count);
    foreach (PlayerMissionStatsData missionStatsData1 in playerData.missionsStatsData)
    {
      PlayerSaveData.PlayerMissionStatsData missionStatsData2 = new PlayerSaveData.PlayerMissionStatsData()
      {
        missionId = missionStatsData1.missionId,
        cleared = missionStatsData1.cleared
      };
      localPlayer.missionsStatsData.Add(missionStatsData2);
    }
    localPlayer.showedMissionHelpIds = playerData.showedMissionHelpIds;
    playerSaveData.guestPlayers = new List<PlayerSaveData.GuestPlayer>(playerData.guestPlayerNames.Count);
    foreach (string guestPlayerName in playerData.guestPlayerNames)
      playerSaveData.guestPlayers.Add(new PlayerSaveData.GuestPlayer()
      {
        playerName = guestPlayerName
      });
    List<PlayerSaveData.ColorScheme> colorSchemes = new List<PlayerSaveData.ColorScheme>(playerData.colorSchemesSettings.GetNumberOfColorSchemes());
    for (int idx = 0; idx < playerData.colorSchemesSettings.GetNumberOfColorSchemes(); ++idx)
    {
      ColorScheme colorSchemeForIdx = playerData.colorSchemesSettings.GetColorSchemeForIdx(idx);
      if (colorSchemeForIdx.isEditable)
      {
        PlayerSaveData.ColorScheme colorScheme = new PlayerSaveData.ColorScheme(colorSchemeForIdx.colorSchemeId, colorSchemeForIdx.saberAColor, colorSchemeForIdx.saberBColor, colorSchemeForIdx.environmentColor0, colorSchemeForIdx.environmentColor1, colorSchemeForIdx.obstaclesColor, colorSchemeForIdx.environmentColor0Boost, colorSchemeForIdx.environmentColor1Boost);
        colorSchemes.Add(colorScheme);
      }
    }
    localPlayer.colorSchemesSettings = new PlayerSaveData.ColorSchemesSettings(playerData.colorSchemesSettings.overrideDefaultColors, playerData.colorSchemesSettings.selectedColorSchemeId, colorSchemes);
    localPlayer.overrideEnvironmentSettings = new PlayerSaveData.OverrideEnvironmentSettings()
    {
      overrideEnvironments = playerData.overrideEnvironmentSettings.overrideEnvironments
    };
    EnvironmentInfoSO environmentInfoForType1 = playerData.overrideEnvironmentSettings.GetOverrideEnvironmentInfoForType(this._a360DegreesEnvironmentType);
    localPlayer.overrideEnvironmentSettings.override360EnvironmentName = (UnityEngine.Object) environmentInfoForType1 != (UnityEngine.Object) null ? environmentInfoForType1.serializedName : "";
    EnvironmentInfoSO environmentInfoForType2 = playerData.overrideEnvironmentSettings.GetOverrideEnvironmentInfoForType(this._normalEnvironmentType);
    localPlayer.overrideEnvironmentSettings.overrideNormalEnvironmentName = (UnityEngine.Object) environmentInfoForType2 != (UnityEngine.Object) null ? environmentInfoForType2.serializedName : "";
    localPlayer.favoritesLevelIds = playerData.favoritesLevelIds != null ? new List<string>((IEnumerable<string>) playerData.favoritesLevelIds) : new List<string>();
    localPlayer.multiplayerModeSettings = new PlayerSaveData.MultiplayerModeSettings()
    {
      createServerNumberOfPlayers = playerData.multiplayerModeSettings.createServerPlayersCount,
      quickPlayDifficulty = playerData.multiplayerModeSettings.quickPlayBeatmapDifficulty.FromMask().SerializedName(),
      quickPlaySongPackMaskSerializedName = playerData.multiplayerModeSettings.quickPlaySongPackMaskSerializedName
    };
    localPlayer.playerAgreements = new PlayerSaveData.PlayerAgreementsData()
    {
      eulaVersion = playerData.playerAgreements.eulaVersion,
      healthAndSafetyVersion = playerData.playerAgreements.healthAndSafetyVersion,
      privacyPolicyVersion = playerData.playerAgreements.privacyPolicyVersion
    };
    localPlayer.agreedToEula = playerData.agreedToEula || playerData.playerAgreements.AgreedToEula();
    localPlayer.agreedToMultiplayerDisclaimer = playerData.agreedToMultiplayerDisclaimer;
    string filePath = Application.persistentDataPath + "/PlayerData.dat";
    string tempFilePath = Application.persistentDataPath + "/PlayerData.dat.tmp";
    string backupFilePath = Application.persistentDataPath + "/PlayerData.dat.bak";
    FileHelpers.SaveToJSONFile((object) playerSaveData, filePath, tempFilePath, backupFilePath);
  }

  public virtual PlayerData Load()
  {
    string filePath1 = Application.persistentDataPath + "/PlayerData.dat";
    string filePath2 = Application.persistentDataPath + "/PlayerData.dat.bak";
    return (this.LoadFromJSONString(FileHelpers.LoadJSONFile(filePath1)) ?? this.LoadFromJSONString(FileHelpers.LoadJSONFile(filePath2))) ?? this.CreateDefaultPlayerData();
  }

  public virtual PlayerData LoadFromJSONString(string jsonString)
  {
    try
    {
      VersionSaveData versionSaveData = JsonUtility.FromJson<VersionSaveData>(jsonString);
      return versionSaveData != null && (versionSaveData.version == "" || versionSaveData.version == "1.0.1") ? this.LoadFromVersionV1_0_1(JsonUtility.FromJson<PlayerSaveDataV1_0_1>(jsonString)) : this.LoadFromCurrentVersion(JsonUtility.FromJson<PlayerSaveData>(jsonString));
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    return (PlayerData) null;
  }

  public virtual PlayerData LoadFromCurrentVersion(PlayerSaveData playerSaveData)
  {
    if (playerSaveData == null || playerSaveData.localPlayers == null || playerSaveData.localPlayers.Count == 0)
      return (PlayerData) null;
    PlayerSaveData.LocalPlayer localPlayer = playerSaveData.localPlayers[0];
    GameplayModifiers gameplayModifiers1 = new GameplayModifiers((GameplayModifiers.EnergyType) localPlayer.gameplayModifiers.energyType, localPlayer.gameplayModifiers.noFailOn0Energy, localPlayer.gameplayModifiers.instaFail, localPlayer.gameplayModifiers.failOnSaberClash, (GameplayModifiers.EnabledObstacleType) localPlayer.gameplayModifiers.enabledObstacleType, localPlayer.gameplayModifiers.noBombs, localPlayer.gameplayModifiers.fastNotes, localPlayer.gameplayModifiers.strictAngles, localPlayer.gameplayModifiers.disappearingArrows, (GameplayModifiers.SongSpeed) localPlayer.gameplayModifiers.songSpeed, localPlayer.gameplayModifiers.noArrows, localPlayer.gameplayModifiers.ghostNotes, localPlayer.gameplayModifiers.proMode, localPlayer.gameplayModifiers.zenMode, localPlayer.gameplayModifiers.smallCubes);
    bool flag1 = new Version(playerSaveData.version).CompareTo(PlayerDataFileManagerSO._lastVersionWithoutArcsOptions) <= 0;
    PlayerSpecificSettings specificSettings = new PlayerSpecificSettings(localPlayer.playerSpecificSettings.leftHanded, localPlayer.playerSpecificSettings.playerHeight, localPlayer.playerSpecificSettings.automaticPlayerHeight, localPlayer.playerSpecificSettings.sfxVolume, localPlayer.playerSpecificSettings.reduceDebris, localPlayer.playerSpecificSettings.noTextsAndHuds, localPlayer.playerSpecificSettings.noFailEffects, localPlayer.playerSpecificSettings.advancedHud, localPlayer.playerSpecificSettings.autoRestart, localPlayer.playerSpecificSettings.saberTrailIntensity, localPlayer.playerSpecificSettings._noteJumpDurationTypeSettingsSaveData.GetRuntimeData(), localPlayer.playerSpecificSettings.noteJumpFixedDuration, localPlayer.playerSpecificSettings.noteJumpBeatOffset, localPlayer.playerSpecificSettings.hideNoteSpawnEffect, localPlayer.playerSpecificSettings.adaptiveSfx, flag1 || localPlayer.playerSpecificSettings.arcsHapticFeedback, flag1 ? ArcVisibilityType.Standard : localPlayer.playerSpecificSettings.arcsVisibleSaveData.GetRuntimeData(), localPlayer.playerSpecificSettings.environmentEffectsFilterDefaultPreset.GetRuntimeData(), localPlayer.playerSpecificSettings.environmentEffectsFilterExpertPlusPreset.GetRuntimeData());
    PlayerAllOverallStatsData overallStatsData = localPlayer.playerAllOverallStatsData.ToPlayerAllOverallStatsData();
    List<PlayerLevelStatsData> playerLevelStatsDataList = new List<PlayerLevelStatsData>(localPlayer.levelsStatsData.Count);
    foreach (PlayerSaveData.PlayerLevelStatsData playerLevelStatsData1 in localPlayer.levelsStatsData)
    {
      BeatmapCharacteristicSO bySerializedName = this._beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(playerLevelStatsData1.beatmapCharacteristicName);
      if ((UnityEngine.Object) bySerializedName != (UnityEngine.Object) null)
      {
        PlayerLevelStatsData playerLevelStatsData2 = new PlayerLevelStatsData(playerLevelStatsData1.levelId, playerLevelStatsData1.difficulty, bySerializedName, playerLevelStatsData1.highScore, playerLevelStatsData1.maxCombo, playerLevelStatsData1.fullCombo, playerLevelStatsData1.maxRank, playerLevelStatsData1.validScore, playerLevelStatsData1.playCount);
        playerLevelStatsDataList.Add(playerLevelStatsData2);
      }
    }
    List<PlayerMissionStatsData> missionStatsDataList = new List<PlayerMissionStatsData>(localPlayer.missionsStatsData.Count);
    foreach (PlayerSaveData.PlayerMissionStatsData missionStatsData1 in localPlayer.missionsStatsData)
    {
      PlayerMissionStatsData missionStatsData2 = new PlayerMissionStatsData(missionStatsData1.missionId, missionStatsData1.cleared);
      missionStatsDataList.Add(missionStatsData2);
    }
    List<string> showedMissionHelpIds1 = localPlayer.showedMissionHelpIds;
    PracticeSettings practiceSettings1 = new PracticeSettings(localPlayer.practiceSettings.startSongTime, localPlayer.practiceSettings.songSpeedMul);
    List<string> stringList = new List<string>(playerSaveData.guestPlayers.Count);
    foreach (PlayerSaveData.GuestPlayer guestPlayer in playerSaveData.guestPlayers)
      stringList.Add(guestPlayer.playerName);
    ColorSchemesSettings colorSchemesSettings1 = new ColorSchemesSettings(this._defaultColorSchemes.colorSchemes);
    bool flag2 = new Version(playerSaveData.version).CompareTo(PlayerDataFileManagerSO._lastVersionWithoutSavedCustomColorSchemeBoostColors) <= 0;
    if (localPlayer.colorSchemesSettings != null && localPlayer.colorSchemesSettings.colorSchemes != null)
    {
      for (int index = 0; index < localPlayer.colorSchemesSettings.colorSchemes.Count; ++index)
      {
        PlayerSaveData.ColorScheme colorScheme1 = localPlayer.colorSchemesSettings.colorSchemes[index];
        ColorScheme colorScheme2 = (ColorScheme) null;
        if (colorScheme1.colorSchemeId != null)
          colorScheme2 = colorSchemesSettings1.GetColorSchemeForId(colorScheme1.colorSchemeId);
        if (colorScheme2 != null && colorScheme2.isEditable)
          colorSchemesSettings1.SetColorSchemeForId(new ColorScheme(colorScheme2.colorSchemeId, colorScheme2.colorSchemeNameLocalizationKey, colorScheme2.useNonLocalizedName, colorScheme2.nonLocalizedName, colorScheme2.isEditable, colorScheme1.saberAColor, colorScheme1.saberBColor, colorScheme1.environmentColor0, colorScheme1.environmentColor1, true, flag2 ? colorScheme1.environmentColor0 : colorScheme1.environmentColor0Boost, flag2 ? colorScheme1.environmentColor1 : colorScheme1.environmentColor1Boost, colorScheme1.obstaclesColor));
      }
      colorSchemesSettings1.overrideDefaultColors = localPlayer.colorSchemesSettings.overrideDefaultColors;
      if (!string.IsNullOrEmpty(localPlayer.colorSchemesSettings.selectedColorSchemeId))
        colorSchemesSettings1.selectedColorSchemeId = localPlayer.colorSchemesSettings.selectedColorSchemeId;
    }
    OverrideEnvironmentSettings environmentSettings = this.CreateDefaultOverrideEnvironmentSettings();
    BeatmapCharacteristicSO characteristicSo = this._beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(localPlayer.lastSelectedBeatmapCharacteristicName);
    if ((UnityEngine.Object) characteristicSo == (UnityEngine.Object) null)
      characteristicSo = this._defaultLastSelectedBeatmapCharacteristic;
    BeatmapDifficulty difficulty;
    if (!localPlayer.multiplayerModeSettings.quickPlayDifficulty.BeatmapDifficultyFromSerializedName(out difficulty))
      difficulty = BeatmapDifficulty.Easy;
    string str = localPlayer.multiplayerModeSettings.quickPlaySongPackMaskSerializedName;
    if (string.IsNullOrEmpty(str))
      str = this.LoadCorrectedSongPackMask(localPlayer.multiplayerModeSettings.quickPlaySongPackMask);
    MultiplayerModeSettings multiplayerModeSettings1 = new MultiplayerModeSettings()
    {
      createServerPlayersCount = localPlayer.multiplayerModeSettings.createServerNumberOfPlayers,
      quickPlayBeatmapDifficulty = difficulty.ToMask(),
      quickPlaySongPackMaskSerializedName = str,
      quickPlayEnableLevelSelection = localPlayer.multiplayerModeSettings.quickPlayEnableLevelSelection
    };
    PlayerAgreements playerAgreements1 = new PlayerAgreements(localPlayer.playerAgreements.eulaVersion, localPlayer.playerAgreements.privacyPolicyVersion, localPlayer.playerAgreements.healthAndSafetyVersion);
    if (localPlayer.agreedToEula && playerAgreements1.eulaVersion == 0)
      playerAgreements1.eulaVersion = 1;
    if (localPlayer.agreedToEula && playerAgreements1.privacyPolicyVersion == 0)
      playerAgreements1.privacyPolicyVersion = 1;
    string playerId = localPlayer.playerId;
    string playerName = localPlayer.playerName;
    int num1 = localPlayer.shouldShowTutorialPrompt ? 1 : 0;
    int num2 = localPlayer.shouldShow360Warning ? 1 : 0;
    int num3 = localPlayer.agreedToEula ? 1 : 0;
    int num4 = localPlayer.didSelectLanguage ? 1 : 0;
    int num5 = localPlayer.agreedToMultiplayerDisclaimer ? 1 : 0;
    int selectRegionVersion = localPlayer.didSelectRegionVersion;
    PlayerAgreements playerAgreements2 = playerAgreements1;
    int num6 = localPlayer.avatarCreated ? 1 : 0;
    int didSelectRegionVersion = selectRegionVersion;
    PlayerAgreements playerAgreements3 = playerAgreements2;
    int beatmapDifficulty = (int) localPlayer.lastSelectedBeatmapDifficulty;
    BeatmapCharacteristicSO lastSelectedBeatmapCharacteristic = characteristicSo;
    GameplayModifiers gameplayModifiers2 = gameplayModifiers1;
    PlayerSpecificSettings playerSpecificSettings = specificSettings;
    PracticeSettings practiceSettings2 = practiceSettings1;
    PlayerAllOverallStatsData playerAllOverallStatsData = overallStatsData;
    List<PlayerLevelStatsData> levelsStatsData = playerLevelStatsDataList;
    List<PlayerMissionStatsData> missionsStatsData = missionStatsDataList;
    List<string> showedMissionHelpIds2 = showedMissionHelpIds1;
    List<string> guestPlayerNames = stringList;
    ColorSchemesSettings colorSchemesSettings2 = colorSchemesSettings1;
    OverrideEnvironmentSettings overrideEnvironmentSettings = environmentSettings;
    List<string> favoritesLevelIds = localPlayer.favoritesLevelIds;
    MultiplayerModeSettings multiplayerModeSettings2 = multiplayerModeSettings1;
    int promoDisplayCount = localPlayer.currentDlcPromoDisplayCount;
    string currentDlcPromoId = localPlayer.currentDlcPromoId;
    return new PlayerData(playerId, playerName, num1 != 0, num2 != 0, num3 != 0, num4 != 0, num5 != 0, num6 != 0, didSelectRegionVersion, playerAgreements3, (BeatmapDifficulty) beatmapDifficulty, lastSelectedBeatmapCharacteristic, gameplayModifiers2, playerSpecificSettings, practiceSettings2, playerAllOverallStatsData, levelsStatsData, missionsStatsData, showedMissionHelpIds2, guestPlayerNames, colorSchemesSettings2, overrideEnvironmentSettings, favoritesLevelIds, multiplayerModeSettings2, promoDisplayCount, currentDlcPromoId);
  }

  public virtual PlayerData LoadFromVersionV1_0_1(PlayerSaveDataV1_0_1 playerDataModelSaveData)
  {
    if (playerDataModelSaveData.localPlayers == null || playerDataModelSaveData.localPlayers.Count == 0)
      return (PlayerData) null;
    PlayerSaveDataV1_0_1.LocalPlayer localPlayer = playerDataModelSaveData.localPlayers[0];
    GameplayModifiers noModifiers = GameplayModifiers.noModifiers;
    GameplayModifiers.EnergyType? energyType = new GameplayModifiers.EnergyType?((GameplayModifiers.EnergyType) localPlayer.gameplayModifiers.energyType);
    bool? noFailOn0Energy = new bool?(false);
    bool? instaFail = new bool?(localPlayer.gameplayModifiers.instaFail);
    bool? failOnSaberClash = new bool?(localPlayer.gameplayModifiers.failOnSaberClash);
    GameplayModifiers.EnabledObstacleType? enabledObstacleType = new GameplayModifiers.EnabledObstacleType?((GameplayModifiers.EnabledObstacleType) localPlayer.gameplayModifiers.enabledObstacleType);
    bool? nullable1 = new bool?(localPlayer.gameplayModifiers.fastNotes);
    bool? nullable2 = new bool?(localPlayer.gameplayModifiers.strictAngles);
    bool? nullable3 = new bool?(localPlayer.gameplayModifiers.disappearingArrows);
    bool? noBombs = new bool?(localPlayer.gameplayModifiers.noBombs);
    bool? fastNotes = nullable1;
    bool? strictAngles = nullable2;
    bool? disappearingArrows = nullable3;
    GameplayModifiers.SongSpeed? songSpeed = new GameplayModifiers.SongSpeed?((GameplayModifiers.SongSpeed) localPlayer.gameplayModifiers.songSpeed);
    bool? noArrows = new bool?();
    bool? ghostNotes = new bool?();
    bool? proMode = new bool?();
    bool? zenMode = new bool?();
    bool? smallCubes = new bool?();
    GameplayModifiers gameplayModifiers = noModifiers.CopyWith(energyType, noFailOn0Energy, instaFail, failOnSaberClash, enabledObstacleType, noBombs, fastNotes, strictAngles, disappearingArrows, songSpeed, noArrows, ghostNotes, proMode, zenMode, smallCubes);
    PlayerSpecificSettings playerSpecificSettings = new PlayerSpecificSettings().CopyWith(new bool?(localPlayer.playerSpecificSettings.leftHanded), new float?(localPlayer.playerSpecificSettings.playerHeight), reduceDebris: new bool?(localPlayer.playerSpecificSettings.reduceDebris), noTextsAndHuds: new bool?(localPlayer.playerSpecificSettings.noTextsAndHuds), advancedHud: new bool?(localPlayer.playerSpecificSettings.advancedHud));
    PlayerAllOverallStatsData overallStatsData = localPlayer.playerAllOverallStatsData.ToPlayerAllOverallStatsData();
    List<PlayerLevelStatsData> levelsStatsData = new List<PlayerLevelStatsData>(localPlayer.levelsStatsData.Count);
    foreach (PlayerSaveDataV1_0_1.PlayerLevelStatsData playerLevelStatsData1 in localPlayer.levelsStatsData)
    {
      BeatmapCharacteristicSO characteristicFromV101LevelId = PlayerDataFileManagerSO.GetBeatmapCharacteristicFromV_1_0_1LevelId(this._beatmapCharacteristicCollection, playerLevelStatsData1.levelId);
      if ((UnityEngine.Object) characteristicFromV101LevelId != (UnityEngine.Object) null)
      {
        string idFromV101LevelId = PlayerDataFileManagerSO.GetLevelIdFromV_1_0_1LevelId(playerLevelStatsData1.levelId, characteristicFromV101LevelId);
        if (idFromV101LevelId != null)
        {
          PlayerLevelStatsData playerLevelStatsData2 = new PlayerLevelStatsData(idFromV101LevelId, playerLevelStatsData1.difficulty, characteristicFromV101LevelId, playerLevelStatsData1.highScore, playerLevelStatsData1.maxCombo, playerLevelStatsData1.fullCombo, playerLevelStatsData1.maxRank, playerLevelStatsData1.validScore, playerLevelStatsData1.playCount);
          levelsStatsData.Add(playerLevelStatsData2);
        }
      }
    }
    List<PlayerMissionStatsData> missionsStatsData = new List<PlayerMissionStatsData>(localPlayer.missionsStatsData.Count);
    foreach (PlayerSaveDataV1_0_1.PlayerMissionStatsData missionStatsData1 in localPlayer.missionsStatsData)
    {
      PlayerMissionStatsData missionStatsData2 = new PlayerMissionStatsData(missionStatsData1.missionId, missionStatsData1.cleared);
      missionsStatsData.Add(missionStatsData2);
    }
    List<string> showedMissionHelpIds = localPlayer.showedMissionHelpIds;
    PracticeSettings practiceSettings = new PracticeSettings();
    List<string> guestPlayerNames = new List<string>();
    ColorSchemesSettings colorSchemesSettings = new ColorSchemesSettings(this._defaultColorSchemes.colorSchemes);
    OverrideEnvironmentSettings environmentSettings = this.CreateDefaultOverrideEnvironmentSettings();
    return new PlayerData(localPlayer.playerId, localPlayer.playerName, localPlayer.shouldShowTutorialPrompt, true, false, false, false, false, 0, new PlayerAgreements(), playerDataModelSaveData.lastSelectedBeatmapDifficulty, this._defaultLastSelectedBeatmapCharacteristic, gameplayModifiers, playerSpecificSettings, practiceSettings, overallStatsData, levelsStatsData, missionsStatsData, showedMissionHelpIds, guestPlayerNames, colorSchemesSettings, environmentSettings, new List<string>(), new MultiplayerModeSettings(), 0, (string) null);
  }

  public virtual PlayerData CreateDefaultPlayerData() => new PlayerData("", "<NO NAME!>", this._defaultLastSelectedBeatmapCharacteristic, new ColorSchemesSettings(this._defaultColorSchemes.colorSchemes), this.CreateDefaultOverrideEnvironmentSettings());

  public static string GetLevelIdFromV_1_0_1LevelId(
    string oldLevelId,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    string compoundIdPartName = beatmapCharacteristic.compoundIdPartName;
    return oldLevelId.EndsWith(compoundIdPartName) ? oldLevelId.Substring(0, oldLevelId.Length - compoundIdPartName.Length) : (string) null;
  }

  public static BeatmapCharacteristicSO GetBeatmapCharacteristicFromV_1_0_1LevelId(
    BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection,
    string levelId)
  {
    BeatmapCharacteristicSO characteristicFromV101LevelId = (BeatmapCharacteristicSO) null;
    foreach (BeatmapCharacteristicSO beatmapCharacteristic in beatmapCharacteristicCollection.beatmapCharacteristics)
    {
      if (levelId.Contains(beatmapCharacteristic.compoundIdPartName) && ((UnityEngine.Object) characteristicFromV101LevelId == (UnityEngine.Object) null || characteristicFromV101LevelId.compoundIdPartName.Length < beatmapCharacteristic.compoundIdPartName.Length))
        characteristicFromV101LevelId = beatmapCharacteristic;
    }
    return characteristicFromV101LevelId;
  }

  public virtual OverrideEnvironmentSettings CreateDefaultOverrideEnvironmentSettings()
  {
    OverrideEnvironmentSettings environmentSettings = new OverrideEnvironmentSettings();
    environmentSettings.overrideEnvironments = false;
    environmentSettings.SetEnvironmentInfoForType(this._normalEnvironmentType, this._allEnvironmentInfos.GetFirstEnvironmentInfoWithType(this._normalEnvironmentType));
    environmentSettings.SetEnvironmentInfoForType(this._a360DegreesEnvironmentType, this._allEnvironmentInfos.GetFirstEnvironmentInfoWithType(this._a360DegreesEnvironmentType));
    return environmentSettings;
  }

  public virtual EnvironmentInfoSO GetEnvironmentInfoBySerializedName(string environmentName) => this._allEnvironmentInfos.GetEnvironmentInfoBySerializedName(environmentName);

  public virtual string LoadCorrectedSongPackMask(byte[] songMaskPackBytes) => songMaskPackBytes == null || songMaskPackBytes.Length == 0 || SongPackMask.FromBytes(songMaskPackBytes) != SongPackMask.all ? this._buildInSongPackSerializedName : this._allSongPackSerializedName;
}
