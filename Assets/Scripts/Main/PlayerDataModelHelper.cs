// Decompiled with JetBrains decompiler
// Type: PlayerDataModelHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class PlayerDataModelHelper
{
  public static PlayerAllOverallStatsData ToPlayerAllOverallStatsData(
    this PlayerSaveData.PlayerAllOverallStatsData playerAllOverallStatsData)
  {
    return playerAllOverallStatsData == null ? new PlayerAllOverallStatsData() : new PlayerAllOverallStatsData(playerAllOverallStatsData.campaignOverallStatsData.ToPlayerOverallStats(), playerAllOverallStatsData.soloFreePlayOverallStatsData.ToPlayerOverallStats(), playerAllOverallStatsData.partyFreePlayOverallStatsData.ToPlayerOverallStats(), playerAllOverallStatsData.onlinePlayOverallStatsData.ToPlayerOverallStats());
  }

  public static PlayerAllOverallStatsData ToPlayerAllOverallStatsData(
    this PlayerSaveDataV1_0_1.PlayerAllOverallStatsData playerAllOverallStatsData)
  {
    return playerAllOverallStatsData == null ? new PlayerAllOverallStatsData() : new PlayerAllOverallStatsData(playerAllOverallStatsData.campaignOverallStatsData.ToPlayerOverallStats(), playerAllOverallStatsData.soloFreePlayOverallStatsData.ToPlayerOverallStats(), playerAllOverallStatsData.partyFreePlayOverallStatsData.ToPlayerOverallStats(), new PlayerAllOverallStatsData.PlayerOverallStatsData());
  }

  public static PlayerAllOverallStatsData.PlayerOverallStatsData ToPlayerOverallStats(
    this PlayerSaveData.PlayerOverallStatsData playerAllOverallStatsData)
  {
    return playerAllOverallStatsData == null ? new PlayerAllOverallStatsData.PlayerOverallStatsData() : new PlayerAllOverallStatsData.PlayerOverallStatsData(playerAllOverallStatsData.goodCutsCount, playerAllOverallStatsData.badCutsCount, playerAllOverallStatsData.missedCutsCount, playerAllOverallStatsData.totalScore, playerAllOverallStatsData.playedLevelsCount, playerAllOverallStatsData.cleardLevelsCount, playerAllOverallStatsData.failedLevelsCount, playerAllOverallStatsData.fullComboCount, playerAllOverallStatsData.timePlayed, playerAllOverallStatsData.handDistanceTravelled, playerAllOverallStatsData.cummulativeCutScoreWithoutMultiplier);
  }

  public static PlayerAllOverallStatsData.PlayerOverallStatsData ToPlayerOverallStats(
    this PlayerSaveDataV1_0_1.PlayerOverallStatsData playerAllOverallStatsData)
  {
    return playerAllOverallStatsData == null ? new PlayerAllOverallStatsData.PlayerOverallStatsData() : new PlayerAllOverallStatsData.PlayerOverallStatsData(playerAllOverallStatsData.goodCutsCount, playerAllOverallStatsData.badCutsCount, playerAllOverallStatsData.missedCutsCount, playerAllOverallStatsData.totalScore, playerAllOverallStatsData.playedLevelsCount, playerAllOverallStatsData.cleardLevelsCount, playerAllOverallStatsData.failedLevelsCount, playerAllOverallStatsData.fullComboCount, playerAllOverallStatsData.timePlayed, playerAllOverallStatsData.handDistanceTravelled, playerAllOverallStatsData.cummulativeCutScoreWithoutMultiplier);
  }

  public static PlayerSaveData.PlayerAllOverallStatsData ToPlayerAllOverallStatsData(
    this PlayerAllOverallStatsData playerAllOverallStatsData)
  {
    return playerAllOverallStatsData == null ? new PlayerSaveData.PlayerAllOverallStatsData() : new PlayerSaveData.PlayerAllOverallStatsData(playerAllOverallStatsData.campaignOverallStatsData.ToPlayerOverallStatsData(), playerAllOverallStatsData.soloFreePlayOverallStatsData.ToPlayerOverallStatsData(), playerAllOverallStatsData.partyFreePlayOverallStatsData.ToPlayerOverallStatsData(), playerAllOverallStatsData.onlinePlayOverallStatsData.ToPlayerOverallStatsData());
  }

  public static PlayerSaveData.PlayerOverallStatsData ToPlayerOverallStatsData(
    this PlayerAllOverallStatsData.PlayerOverallStatsData playerOverallStatsData)
  {
    return playerOverallStatsData == null ? new PlayerSaveData.PlayerOverallStatsData() : new PlayerSaveData.PlayerOverallStatsData(playerOverallStatsData.goodCutsCount, playerOverallStatsData.badCutsCount, playerOverallStatsData.missedCutsCount, playerOverallStatsData.totalScore, playerOverallStatsData.playedLevelsCount, playerOverallStatsData.clearedLevelsCount, playerOverallStatsData.failedLevelsCount, playerOverallStatsData.fullComboCount, playerOverallStatsData.timePlayed, playerOverallStatsData.handDistanceTravelled, playerOverallStatsData.totalCutScore);
  }
}
