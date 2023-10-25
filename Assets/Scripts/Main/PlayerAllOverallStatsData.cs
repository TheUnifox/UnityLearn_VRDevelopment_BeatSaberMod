// Decompiled with JetBrains decompiler
// Type: PlayerAllOverallStatsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class PlayerAllOverallStatsData
{
  [CompilerGenerated]
  protected PlayerAllOverallStatsData.PlayerOverallStatsData m_CcampaignOverallStatsData;
  [CompilerGenerated]
  protected PlayerAllOverallStatsData.PlayerOverallStatsData m_CsoloFreePlayOverallStatsData;
  [CompilerGenerated]
  protected PlayerAllOverallStatsData.PlayerOverallStatsData m_CpartyFreePlayOverallStatsData;
  [CompilerGenerated]
  protected PlayerAllOverallStatsData.PlayerOverallStatsData m_ConlinePlayOverallStatsData;

  public PlayerAllOverallStatsData.PlayerOverallStatsData allOverallStatsData => this.campaignOverallStatsData + this.soloFreePlayOverallStatsData + this.partyFreePlayOverallStatsData + this.onlinePlayOverallStatsData;

  public PlayerAllOverallStatsData.PlayerOverallStatsData campaignOverallStatsData
  {
    get => this.m_CcampaignOverallStatsData;
    private set => this.m_CcampaignOverallStatsData = value;
  }

  public PlayerAllOverallStatsData.PlayerOverallStatsData soloFreePlayOverallStatsData
  {
    get => this.m_CsoloFreePlayOverallStatsData;
    private set => this.m_CsoloFreePlayOverallStatsData = value;
  }

  public PlayerAllOverallStatsData.PlayerOverallStatsData partyFreePlayOverallStatsData
  {
    get => this.m_CpartyFreePlayOverallStatsData;
    private set => this.m_CpartyFreePlayOverallStatsData = value;
  }

  public PlayerAllOverallStatsData.PlayerOverallStatsData onlinePlayOverallStatsData
  {
    get => this.m_ConlinePlayOverallStatsData;
    private set => this.m_ConlinePlayOverallStatsData = value;
  }

  public event System.Action<LevelCompletionResults, IDifficultyBeatmap> didUpdateSoloFreePlayOverallStatsDataEvent;

  public event System.Action<LevelCompletionResults, IDifficultyBeatmap> didUpdatePartyFreePlayOverallStatsDataEvent;

  public event System.Action<MissionCompletionResults, MissionNode> didUpdateCampaignOverallStatsDataEvent;

  public PlayerAllOverallStatsData()
  {
    this.campaignOverallStatsData = new PlayerAllOverallStatsData.PlayerOverallStatsData();
    this.soloFreePlayOverallStatsData = new PlayerAllOverallStatsData.PlayerOverallStatsData();
    this.partyFreePlayOverallStatsData = new PlayerAllOverallStatsData.PlayerOverallStatsData();
    this.onlinePlayOverallStatsData = new PlayerAllOverallStatsData.PlayerOverallStatsData();
  }

  public PlayerAllOverallStatsData(
    PlayerAllOverallStatsData.PlayerOverallStatsData campaignOverallStatsData,
    PlayerAllOverallStatsData.PlayerOverallStatsData soloFreePlayOverallStatsData,
    PlayerAllOverallStatsData.PlayerOverallStatsData partyFreePlayOverallStatsData,
    PlayerAllOverallStatsData.PlayerOverallStatsData onlinePlayOverallStatsData)
  {
    this.campaignOverallStatsData = campaignOverallStatsData;
    this.soloFreePlayOverallStatsData = soloFreePlayOverallStatsData;
    this.partyFreePlayOverallStatsData = partyFreePlayOverallStatsData;
    this.onlinePlayOverallStatsData = onlinePlayOverallStatsData;
  }

  public virtual void UpdateSoloFreePlayOverallStatsData(
    LevelCompletionResults levelCompletionResults,
    IDifficultyBeatmap difficultyBeatmap)
  {
    this.soloFreePlayOverallStatsData.UpdateWithLevelCompletionResults(levelCompletionResults);
    System.Action<LevelCompletionResults, IDifficultyBeatmap> overallStatsDataEvent = this.didUpdateSoloFreePlayOverallStatsDataEvent;
    if (overallStatsDataEvent == null)
      return;
    overallStatsDataEvent(levelCompletionResults, difficultyBeatmap);
  }

  public virtual void UpdatePartyFreePlayOverallStatsData(
    LevelCompletionResults levelCompletionResults,
    IDifficultyBeatmap difficultyBeatmap)
  {
    this.partyFreePlayOverallStatsData.UpdateWithLevelCompletionResults(levelCompletionResults);
    System.Action<LevelCompletionResults, IDifficultyBeatmap> overallStatsDataEvent = this.didUpdatePartyFreePlayOverallStatsDataEvent;
    if (overallStatsDataEvent == null)
      return;
    overallStatsDataEvent(levelCompletionResults, difficultyBeatmap);
  }

  public virtual void UpdateCampaignOverallStatsData(
    MissionCompletionResults missionCompletionResults,
    MissionNode missionNode)
  {
    this.campaignOverallStatsData.UpdateWithLevelCompletionResults(missionCompletionResults.levelCompletionResults);
    System.Action<MissionCompletionResults, MissionNode> overallStatsDataEvent = this.didUpdateCampaignOverallStatsDataEvent;
    if (overallStatsDataEvent == null)
      return;
    overallStatsDataEvent(missionCompletionResults, missionNode);
  }

  public virtual void UpdateOnlinePlayOverallStatsData(
    LevelCompletionResults levelCompletionResults,
    IDifficultyBeatmap difficultyBeatmap)
  {
    this.onlinePlayOverallStatsData.UpdateWithLevelCompletionResults(levelCompletionResults);
  }

  public class PlayerOverallStatsData
  {
    [CompilerGenerated]
    protected int m_CgoodCutsCount;
    [CompilerGenerated]
    protected int m_CbadCutsCount;
    [CompilerGenerated]
    protected int m_CmissedCutsCount;
    [CompilerGenerated]
    protected long m_CtotalScore;
    [CompilerGenerated]
    protected int m_CplayedLevelsCount;
    [CompilerGenerated]
    protected int m_CclearedLevelsCount;
    [CompilerGenerated]
    protected int m_CfailedLevelsCount;
    [CompilerGenerated]
    protected int m_CfullComboCount;
    [CompilerGenerated]
    protected float m_CtimePlayed;
    [CompilerGenerated]
    protected int m_ChandDistanceTravelled;
    [CompilerGenerated]
    protected long m_CtotalCutScore;

    public int goodCutsCount
    {
      get => this.m_CgoodCutsCount;
      private set => this.m_CgoodCutsCount = value;
    }

    public int badCutsCount
    {
      get => this.m_CbadCutsCount;
      private set => this.m_CbadCutsCount = value;
    }

    public int missedCutsCount
    {
      get => this.m_CmissedCutsCount;
      private set => this.m_CmissedCutsCount = value;
    }

    public long totalScore
    {
      get => this.m_CtotalScore;
      private set => this.m_CtotalScore = value;
    }

    public int playedLevelsCount
    {
      get => this.m_CplayedLevelsCount;
      private set => this.m_CplayedLevelsCount = value;
    }

    public int clearedLevelsCount
    {
      get => this.m_CclearedLevelsCount;
      private set => this.m_CclearedLevelsCount = value;
    }

    public int failedLevelsCount
    {
      get => this.m_CfailedLevelsCount;
      private set => this.m_CfailedLevelsCount = value;
    }

    public int fullComboCount
    {
      get => this.m_CfullComboCount;
      private set => this.m_CfullComboCount = value;
    }

    public float timePlayed
    {
      get => this.m_CtimePlayed;
      private set => this.m_CtimePlayed = value;
    }

    public int handDistanceTravelled
    {
      get => this.m_ChandDistanceTravelled;
      private set => this.m_ChandDistanceTravelled = value;
    }

    public long totalCutScore
    {
      get => this.m_CtotalCutScore;
      private set => this.m_CtotalCutScore = value;
    }

    public PlayerOverallStatsData()
    {
    }

    public PlayerOverallStatsData(
      int goodCutsCount,
      int badCutsCount,
      int missedCutsCount,
      long totalScore,
      int playedLevelsCount,
      int clearedLevelsCount,
      int failedLevelsCount,
      int fullComboCount,
      float timePlayed,
      int handDistanceTravelled,
      long totalCutScore)
    {
      this.goodCutsCount = goodCutsCount;
      this.badCutsCount = badCutsCount;
      this.missedCutsCount = missedCutsCount;
      this.totalScore = totalScore;
      this.playedLevelsCount = playedLevelsCount;
      this.clearedLevelsCount = clearedLevelsCount;
      this.failedLevelsCount = failedLevelsCount;
      this.fullComboCount = fullComboCount;
      this.timePlayed = timePlayed;
      this.handDistanceTravelled = handDistanceTravelled;
      this.totalCutScore = totalCutScore;
    }

    public static PlayerAllOverallStatsData.PlayerOverallStatsData operator +(
      PlayerAllOverallStatsData.PlayerOverallStatsData a,
      PlayerAllOverallStatsData.PlayerOverallStatsData b)
    {
      return new PlayerAllOverallStatsData.PlayerOverallStatsData(a.goodCutsCount + b.goodCutsCount, a.badCutsCount + b.badCutsCount, a.missedCutsCount + b.missedCutsCount, a.totalScore + b.totalScore, a.playedLevelsCount + b.playedLevelsCount, a.clearedLevelsCount + b.clearedLevelsCount, a.failedLevelsCount + b.failedLevelsCount, a.fullComboCount + b.fullComboCount, a.timePlayed + b.timePlayed, a.handDistanceTravelled + b.handDistanceTravelled, a.totalCutScore + b.totalCutScore);
    }

    public virtual void UpdateWithLevelCompletionResults(
      LevelCompletionResults levelCompletionResults)
    {
      this.goodCutsCount += levelCompletionResults.goodCutsCount;
      this.badCutsCount += levelCompletionResults.badCutsCount;
      this.missedCutsCount += levelCompletionResults.missedCount;
      ++this.playedLevelsCount;
      if (levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
      {
        ++this.clearedLevelsCount;
        this.totalScore += (long) levelCompletionResults.modifiedScore;
        if (levelCompletionResults.fullCombo)
          ++this.fullComboCount;
      }
      else if (levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed)
        ++this.failedLevelsCount;
      this.timePlayed += levelCompletionResults.endSongTime * levelCompletionResults.gameplayModifiers.songSpeedMul;
      this.handDistanceTravelled += (int) ((double) levelCompletionResults.leftHandMovementDistance + (double) levelCompletionResults.rightHandMovementDistance);
      this.totalCutScore += (long) levelCompletionResults.totalCutScore;
    }
  }
}
