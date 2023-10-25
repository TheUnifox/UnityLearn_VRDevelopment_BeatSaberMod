// Decompiled with JetBrains decompiler
// Type: PlayerSaveData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaveData : VersionSaveData
{
  public const string kCurrentVersion = "2.0.23";
  public List<PlayerSaveData.LocalPlayer> localPlayers;
  public List<PlayerSaveData.GuestPlayer> guestPlayers;

  public PlayerSaveData() => this.version = "2.0.23";

  [Serializable]
  public class GameplayModifiers
  {
    public PlayerSaveData.GameplayModifiers.EnergyType energyType;
    public bool instaFail;
    public bool failOnSaberClash;
    public PlayerSaveData.GameplayModifiers.EnabledObstacleType enabledObstacleType;
    public bool fastNotes;
    public bool strictAngles;
    public bool disappearingArrows;
    public bool ghostNotes;
    public bool noBombs;
    public PlayerSaveData.GameplayModifiers.SongSpeed songSpeed;
    public bool noArrows;
    public bool noFailOn0Energy;
    public bool proMode;
    public bool zenMode;
    public bool smallCubes;

    public enum EnabledObstacleType
    {
      All,
      FullHeightOnly,
      None,
    }

    public enum EnergyType
    {
      Bar,
      Battery,
    }

    public enum SongSpeed
    {
      Normal,
      Faster,
      Slower,
    }
  }

  [Serializable]
  public class PlayerSpecificSettings
  {
    public bool staticLights;
    public bool leftHanded;
    public float playerHeight = 1.7f;
    public bool automaticPlayerHeight;
    public float sfxVolume = 0.7f;
    public bool reduceDebris;
    public bool noTextsAndHuds;
    public bool advancedHud;
    public float saberTrailIntensity = 0.5f;
    public PlayerSaveData.PlayerSpecificSettings.NoteJumpDurationTypeSettingsSaveData _noteJumpDurationTypeSettingsSaveData;
    public float noteJumpFixedDuration = 0.5f;
    public bool autoRestart;
    public bool noFailEffects;
    public float noteJumpBeatOffset;
    public bool hideNoteSpawnEffect;
    public bool adaptiveSfx;
    public bool arcsHapticFeedback;
    public PlayerSaveData.PlayerSpecificSettings.ArcVisibilityTypeSaveData arcsVisibleSaveData;
    public PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData environmentEffectsFilterDefaultPreset = PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData.StrobeFilter;
    public PlayerSaveData.PlayerSpecificSettings.EnvironmentEffectsFilterPresetSaveData environmentEffectsFilterExpertPlusPreset;

    public enum EnvironmentEffectsFilterPresetSaveData
    {
      AllEffects = 0,
      StrobeFilter = 1,
      NoEffects = 10, // 0x0000000A
    }

    public enum ArcVisibilityTypeSaveData
    {
      None,
      Low,
      Standard,
      High,
    }

    public enum NoteJumpDurationTypeSettingsSaveData
    {
      Dynamic,
      Static,
    }
  }

  [Serializable]
  public class PlayerAllOverallStatsData
  {
    public PlayerSaveData.PlayerOverallStatsData campaignOverallStatsData;
    public PlayerSaveData.PlayerOverallStatsData soloFreePlayOverallStatsData;
    public PlayerSaveData.PlayerOverallStatsData partyFreePlayOverallStatsData;
    public PlayerSaveData.PlayerOverallStatsData onlinePlayOverallStatsData;

    public PlayerAllOverallStatsData()
    {
      this.campaignOverallStatsData = new PlayerSaveData.PlayerOverallStatsData();
      this.soloFreePlayOverallStatsData = new PlayerSaveData.PlayerOverallStatsData();
      this.partyFreePlayOverallStatsData = new PlayerSaveData.PlayerOverallStatsData();
      this.onlinePlayOverallStatsData = new PlayerSaveData.PlayerOverallStatsData();
    }

    public PlayerAllOverallStatsData(
      PlayerSaveData.PlayerOverallStatsData campaignOverallStatsData,
      PlayerSaveData.PlayerOverallStatsData soloFreePlayOverallStatsData,
      PlayerSaveData.PlayerOverallStatsData partyFreePlayOverallStatsData,
      PlayerSaveData.PlayerOverallStatsData onlinePlayOverallStatsData)
    {
      this.campaignOverallStatsData = campaignOverallStatsData;
      this.soloFreePlayOverallStatsData = soloFreePlayOverallStatsData;
      this.partyFreePlayOverallStatsData = partyFreePlayOverallStatsData;
      this.onlinePlayOverallStatsData = onlinePlayOverallStatsData;
    }
  }

  [Serializable]
  public class PlayerOverallStatsData
  {
    public int goodCutsCount;
    public int badCutsCount;
    public int missedCutsCount;
    public long totalScore;
    public int playedLevelsCount;
    public int cleardLevelsCount;
    public int failedLevelsCount;
    public int fullComboCount;
    public float timePlayed;
    public int handDistanceTravelled;
    public long cummulativeCutScoreWithoutMultiplier;

    public PlayerOverallStatsData()
    {
    }

    public PlayerOverallStatsData(
      int goodCutsCount,
      int badCutsCount,
      int missedCutsCount,
      long totalScore,
      int playedLevelsCount,
      int cleardLevelsCount,
      int failedLevelsCount,
      int fullComboCount,
      float timePlayed,
      int handDistanceTravelled,
      long cummulativeCutScoreWithoutMultiplier)
    {
      if (totalScore < 0L)
        totalScore = (long) int.MaxValue + totalScore - (long) int.MinValue;
      if (cummulativeCutScoreWithoutMultiplier < 0L)
        cummulativeCutScoreWithoutMultiplier = (long) int.MaxValue + cummulativeCutScoreWithoutMultiplier - (long) int.MinValue;
      this.goodCutsCount = goodCutsCount;
      this.badCutsCount = badCutsCount;
      this.missedCutsCount = missedCutsCount;
      this.totalScore = totalScore;
      this.playedLevelsCount = playedLevelsCount;
      this.cleardLevelsCount = cleardLevelsCount;
      this.failedLevelsCount = failedLevelsCount;
      this.fullComboCount = fullComboCount;
      this.timePlayed = timePlayed;
      this.handDistanceTravelled = handDistanceTravelled;
      this.cummulativeCutScoreWithoutMultiplier = cummulativeCutScoreWithoutMultiplier;
    }
  }

  [Serializable]
  public class PlayerLevelStatsData
  {
    public string levelId;
    public BeatmapDifficulty difficulty;
    public string beatmapCharacteristicName;
    public int highScore;
    public int maxCombo;
    public bool fullCombo;
    public RankModel.Rank maxRank;
    public bool validScore;
    public int playCount;
  }

  [Serializable]
  public class PlayerMissionStatsData
  {
    public string missionId;
    public bool cleared;
  }

  [Serializable]
  public class PracticeSettings
  {
    public float startSongTime;
    public float songSpeedMul;
  }

  [Serializable]
  public class ColorScheme
  {
    public string colorSchemeId;
    public Color saberAColor;
    public Color saberBColor;
    public Color environmentColor0;
    public Color environmentColor1;
    public Color obstaclesColor;
    public Color environmentColor0Boost;
    public Color environmentColor1Boost;

    public ColorScheme(
      string colorSchemeId,
      Color saberAColor,
      Color saberBColor,
      Color environmentColor0,
      Color environmentColor1,
      Color obstaclesColor,
      Color environmentColor0Boost,
      Color environmentColor1Boost)
    {
      this.colorSchemeId = colorSchemeId;
      this.saberAColor = saberAColor;
      this.saberBColor = saberBColor;
      this.environmentColor0 = environmentColor0;
      this.environmentColor1 = environmentColor1;
      this.obstaclesColor = obstaclesColor;
      this.environmentColor0Boost = environmentColor0Boost;
      this.environmentColor1Boost = environmentColor1Boost;
    }
  }

  [Serializable]
  public class ColorSchemesSettings
  {
    public bool overrideDefaultColors;
    public string selectedColorSchemeId;
    public List<PlayerSaveData.ColorScheme> colorSchemes;

    public ColorSchemesSettings(
      bool overrideDefaultColors,
      string selectedColorSchemeId,
      List<PlayerSaveData.ColorScheme> colorSchemes)
    {
      this.overrideDefaultColors = overrideDefaultColors;
      this.selectedColorSchemeId = selectedColorSchemeId;
      this.colorSchemes = colorSchemes;
    }
  }

  [Serializable]
  public class OverrideEnvironmentSettings
  {
    public bool overrideEnvironments;
    public string overrideNormalEnvironmentName;
    public string override360EnvironmentName;
  }

  [Serializable]
  public class GuestPlayer
  {
    public string playerName;
  }

  [Serializable]
  public class MultiplayerModeSettings
  {
    public int createServerNumberOfPlayers;
    public string quickPlayDifficulty;
    public byte[] quickPlaySongPackMask;
    public string quickPlaySongPackMaskSerializedName;
    public bool quickPlayEnableLevelSelection;

    public MultiplayerModeSettings()
    {
      this.createServerNumberOfPlayers = 5;
      this.quickPlayDifficulty = BeatmapDifficulty.Easy.SerializedName();
      this.quickPlaySongPackMask = (byte[]) null;
      this.quickPlaySongPackMaskSerializedName = "";
      this.quickPlayEnableLevelSelection = true;
    }
  }

  [Serializable]
  public class PlayerAgreementsData
  {
    public int eulaVersion;
    public int privacyPolicyVersion;
    public int healthAndSafetyVersion;
  }

  [Serializable]
  public class LocalPlayer
  {
    public string playerId;
    public string playerName;
    public bool shouldShowTutorialPrompt = true;
    public bool shouldShow360Warning = true;
    public bool agreedToEula;
    public bool didSelectLanguage;
    public bool agreedToMultiplayerDisclaimer;
    public bool avatarCreated;
    public int didSelectRegionVersion;
    public PlayerSaveData.PlayerAgreementsData playerAgreements;
    public BeatmapDifficulty lastSelectedBeatmapDifficulty = BeatmapDifficulty.Normal;
    public string lastSelectedBeatmapCharacteristicName;
    public PlayerSaveData.GameplayModifiers gameplayModifiers;
    public PlayerSaveData.PlayerSpecificSettings playerSpecificSettings;
    public PlayerSaveData.PracticeSettings practiceSettings;
    public PlayerSaveData.PlayerAllOverallStatsData playerAllOverallStatsData;
    public List<PlayerSaveData.PlayerLevelStatsData> levelsStatsData;
    public List<PlayerSaveData.PlayerMissionStatsData> missionsStatsData;
    public List<string> showedMissionHelpIds;
    public PlayerSaveData.ColorSchemesSettings colorSchemesSettings;
    public PlayerSaveData.OverrideEnvironmentSettings overrideEnvironmentSettings;
    public List<string> favoritesLevelIds;
    public PlayerSaveData.MultiplayerModeSettings multiplayerModeSettings;
    public int currentDlcPromoDisplayCount;
    public string currentDlcPromoId;
  }
}
