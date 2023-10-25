// Decompiled with JetBrains decompiler
// Type: PlayerSaveDataV1_0_1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveDataV1_0_1
{
  protected const BeatmapDifficulty kDefaulLastSelectedBeatmapDifficulty = BeatmapDifficulty.Normal;
  public const string kCurrentVersion = "";
  public string version;
  public List<PlayerSaveDataV1_0_1.LocalPlayer> localPlayers;
  public List<PlayerSaveDataV1_0_1.GuestPlayer> guestPlayers;
  public BeatmapDifficulty lastSelectedBeatmapDifficulty = BeatmapDifficulty.Normal;

  [Serializable]
  public class GameplayModifiers
  {
    public PlayerSaveDataV1_0_1.GameplayModifiers.EnergyType energyType;
    public bool noFail;
    public bool instaFail;
    public bool failOnSaberClash;
    public PlayerSaveDataV1_0_1.GameplayModifiers.EnabledObstacleType enabledObstacleType;
    public bool fastNotes;
    public bool strictAngles;
    public bool disappearingArrows;
    public bool noBombs;
    public PlayerSaveDataV1_0_1.GameplayModifiers.SongSpeed songSpeed;

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
    public bool swapColors;
    public float playerHeight;
    public bool disableSFX;
    public bool reduceDebris;
    public bool advancedHud;
    public bool noTextsAndHuds;
  }

  [Serializable]
  public class PlayerAllOverallStatsData
  {
    public PlayerSaveDataV1_0_1.PlayerOverallStatsData campaignOverallStatsData;
    public PlayerSaveDataV1_0_1.PlayerOverallStatsData soloFreePlayOverallStatsData;
    public PlayerSaveDataV1_0_1.PlayerOverallStatsData partyFreePlayOverallStatsData;

    public PlayerAllOverallStatsData()
    {
      this.campaignOverallStatsData = new PlayerSaveDataV1_0_1.PlayerOverallStatsData();
      this.soloFreePlayOverallStatsData = new PlayerSaveDataV1_0_1.PlayerOverallStatsData();
      this.partyFreePlayOverallStatsData = new PlayerSaveDataV1_0_1.PlayerOverallStatsData();
    }

    public PlayerAllOverallStatsData(
      PlayerSaveDataV1_0_1.PlayerOverallStatsData campaignOverallStatsData,
      PlayerSaveDataV1_0_1.PlayerOverallStatsData soloFreePlayOverallStatsData,
      PlayerSaveDataV1_0_1.PlayerOverallStatsData partyFreePlayOverallStatsData)
    {
      this.campaignOverallStatsData = campaignOverallStatsData;
      this.soloFreePlayOverallStatsData = soloFreePlayOverallStatsData;
      this.partyFreePlayOverallStatsData = partyFreePlayOverallStatsData;
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
  public class AchievementsData
  {
    public string[] unlockedAchievements;
    public string[] unlockedAchievementsToUpload;
  }

  [Serializable]
  public class LocalPlayer
  {
    public string playerId;
    public string playerName;
    public bool shouldShowTutorialPrompt = true;
    public PlayerSaveDataV1_0_1.GameplayModifiers gameplayModifiers;
    public PlayerSaveDataV1_0_1.PlayerSpecificSettings playerSpecificSettings;
    public PlayerSaveDataV1_0_1.PlayerAllOverallStatsData playerAllOverallStatsData;
    public List<PlayerSaveDataV1_0_1.PlayerLevelStatsData> levelsStatsData;
    public List<PlayerSaveDataV1_0_1.PlayerMissionStatsData> missionsStatsData;
    public List<string> showedMissionHelpIds;
    public PlayerSaveDataV1_0_1.AchievementsData achievementsData;
  }

  [Serializable]
  public class GuestPlayer
  {
    public string playerName;
    public PlayerSaveDataV1_0_1.PlayerSpecificSettings playerSpecificSettings;
  }
}
