// Decompiled with JetBrains decompiler
// Type: AchievementsEvaluationHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class AchievementsEvaluationHandler : MonoBehaviour
{
  [SerializeField]
  protected AchievementsModelSO _achievementsModel;
  [Space]
  [SerializeField]
  protected AchievementSO _clearedLevel100Achievement;
  [SerializeField]
  protected AchievementSO _totalScore100MillionAchievement;
  [SerializeField]
  protected AchievementSO _24HoursPlayedAchievement;
  [SerializeField]
  protected AchievementSO _kilometersTravelled100Achievement;
  [Space]
  [SerializeField]
  protected AchievementSO _15ExpertLevelsRankSAchievement;
  [SerializeField]
  protected AchievementSO _15ExpertLevelsFullComboAchievement;
  [SerializeField]
  protected AchievementSO _15HardLevelsRankSAchievement;
  [SerializeField]
  protected AchievementSO _15HardLevelsFullComboAchievement;
  [Space]
  [SerializeField]
  protected AchievementSO _expertLevelClearedWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _fullComboExpertWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _goodCuts10000Achievement;
  [Space]
  [SerializeField]
  protected AchievementSO _resultMinRankANormalWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _resultMinRankSHardWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _resultMinRankSSExpertWithoutModifiersAchievement;
  [Space]
  [SerializeField]
  protected AchievementSO _combo50NormalWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _combo100HardWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _combo500ExpertWithoutModifiersAchievement;
  [Space]
  [SerializeField]
  protected AchievementSO _clearedLevelWithoutModifiersAchievement;
  [SerializeField]
  protected AchievementSO _clearedLevelWithSongSpeedFasterModifierAchievement;
  [SerializeField]
  protected AchievementSO _clearedLevelWithInstaFailModifierAchievement;
  [SerializeField]
  protected AchievementSO _clearedLevelWithDisappearingArrowsModifierAchievement;
  [SerializeField]
  protected AchievementSO _clearedLevelWithBatteryEnergyModifierAchievement;
  [Space]
  [SerializeField]
  protected AchievementSO _cleared30MissionsAchievement;
  [SerializeField]
  protected AchievementSO _finalMissionClearedAchievement;
  [SerializeField]
  protected AchievementSO _allMissionClearedAchievement;
  [Inject]
  protected PlayerDataModel _playerDataModel;
  [Inject]
  protected MissionNodesManager _missionNodesManager;

  public virtual void Start()
  {
    this._achievementsModel.Initialize();
    this._playerDataModel.playerData.playerAllOverallStatsData.didUpdatePartyFreePlayOverallStatsDataEvent += new System.Action<LevelCompletionResults, IDifficultyBeatmap>(this.HandlePartyFreePlayOverallStatsDataDidUpdate);
    this._playerDataModel.playerData.playerAllOverallStatsData.didUpdateSoloFreePlayOverallStatsDataEvent += new System.Action<LevelCompletionResults, IDifficultyBeatmap>(this.HandleSoloFreePlayOverallStatsDataDidUpdate);
    this._playerDataModel.playerData.playerAllOverallStatsData.didUpdateCampaignOverallStatsDataEvent += new System.Action<MissionCompletionResults, MissionNode>(this.HandleCampaignOverallStatsDataDidUpdate);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._playerDataModel != (UnityEngine.Object) null) || this._playerDataModel.playerData == null)
      return;
    this._playerDataModel.playerData.playerAllOverallStatsData.didUpdatePartyFreePlayOverallStatsDataEvent -= new System.Action<LevelCompletionResults, IDifficultyBeatmap>(this.HandlePartyFreePlayOverallStatsDataDidUpdate);
    this._playerDataModel.playerData.playerAllOverallStatsData.didUpdateSoloFreePlayOverallStatsDataEvent -= new System.Action<LevelCompletionResults, IDifficultyBeatmap>(this.HandleSoloFreePlayOverallStatsDataDidUpdate);
    this._playerDataModel.playerData.playerAllOverallStatsData.didUpdateCampaignOverallStatsDataEvent -= new System.Action<MissionCompletionResults, MissionNode>(this.HandleCampaignOverallStatsDataDidUpdate);
  }

  public virtual void HandleSoloFreePlayOverallStatsDataDidUpdate(
    LevelCompletionResults levelCompletionResults,
    IDifficultyBeatmap difficultyBeatmap)
  {
    this.ProcessLevelFinishData(difficultyBeatmap, levelCompletionResults);
    this.ProcessSoloFreePlayLevelFinishData(difficultyBeatmap, levelCompletionResults);
  }

  public virtual void HandlePartyFreePlayOverallStatsDataDidUpdate(
    LevelCompletionResults levelCompletionResults,
    IDifficultyBeatmap difficultyBeatmap)
  {
    this.ProcessLevelFinishData(difficultyBeatmap, levelCompletionResults);
  }

  public virtual void HandleCampaignOverallStatsDataDidUpdate(
    MissionCompletionResults missionCompletionResults,
    MissionNode missionNode)
  {
    this.ProcessMissionFinishData(missionNode, missionCompletionResults);
    this.ProcessLevelFinishData(missionNode.missionData.level.beatmapLevelData.GetDifficultyBeatmap(missionNode.missionData.beatmapCharacteristic, missionNode.missionData.beatmapDifficulty), missionCompletionResults.levelCompletionResults);
  }

  public virtual void ProcessMissionFinishData(
    MissionNode missionNode,
    MissionCompletionResults missionCompletionResults)
  {
    if (!missionCompletionResults.IsMissionComplete)
      return;
    int num = 0;
    string missionId = this._missionNodesManager.finalMissionNode.missionId;
    if (missionNode.missionId == missionId)
      this._achievementsModel.UnlockAchievement(this._finalMissionClearedAchievement);
    foreach (PlayerMissionStatsData missionStatsData in this._playerDataModel.playerData.missionsStatsData)
    {
      if (missionStatsData.cleared)
        ++num;
    }
    if (num >= 30)
      this._achievementsModel.UnlockAchievement(this._cleared30MissionsAchievement);
    if (num < this._missionNodesManager.allMissionNodes.Length)
      return;
    this._achievementsModel.UnlockAchievement(this._allMissionClearedAchievement);
  }

  public virtual void ProcessSoloFreePlayLevelFinishData(
    IDifficultyBeatmap difficultyBeatmap,
    LevelCompletionResults levelCompletionResults)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    foreach (PlayerLevelStatsData playerLevelStatsData in this._playerDataModel.playerData.levelsStatsData)
    {
      if (playerLevelStatsData.validScore)
      {
        if (playerLevelStatsData.fullCombo)
        {
          if (playerLevelStatsData.difficulty == BeatmapDifficulty.Expert)
            ++num1;
          if (playerLevelStatsData.difficulty == BeatmapDifficulty.Hard)
            ++num2;
        }
        if (playerLevelStatsData.maxRank >= RankModel.Rank.S)
        {
          if (playerLevelStatsData.difficulty == BeatmapDifficulty.Expert)
            ++num3;
          if (playerLevelStatsData.difficulty == BeatmapDifficulty.Hard)
            ++num4;
        }
      }
    }
    if (levelCompletionResults.rank >= RankModel.Rank.S)
    {
      if (num4 >= 15 && difficultyBeatmap.difficulty == BeatmapDifficulty.Hard)
        this._achievementsModel.UnlockAchievement(this._15HardLevelsRankSAchievement);
      if (num3 >= 15 && difficultyBeatmap.difficulty == BeatmapDifficulty.Expert)
        this._achievementsModel.UnlockAchievement(this._15ExpertLevelsRankSAchievement);
    }
    if (!levelCompletionResults.fullCombo)
      return;
    if (num2 >= 15 && difficultyBeatmap.difficulty == BeatmapDifficulty.Hard)
      this._achievementsModel.UnlockAchievement(this._15HardLevelsFullComboAchievement);
    if (num1 < 15 || difficultyBeatmap.difficulty != BeatmapDifficulty.Expert)
      return;
    this._achievementsModel.UnlockAchievement(this._15ExpertLevelsFullComboAchievement);
  }

  public virtual void ProcessLevelFinishData(
    IDifficultyBeatmap difficultyBeatmap,
    LevelCompletionResults levelCompletionResults)
  {
    PlayerAllOverallStatsData.PlayerOverallStatsData overallStatsData = this._playerDataModel.playerData.playerAllOverallStatsData.allOverallStatsData;
    bool flag = levelCompletionResults.gameplayModifiers.IsWithoutModifiers();
    if ((double) overallStatsData.timePlayed >= 86400.0)
      this._achievementsModel.UnlockAchievement(this._24HoursPlayedAchievement);
    if (levelCompletionResults.modifiedScore > 0 && overallStatsData.totalScore >= 100000000L)
      this._achievementsModel.UnlockAchievement(this._totalScore100MillionAchievement);
    if (((double) levelCompletionResults.leftHandMovementDistance > 0.0 || (double) levelCompletionResults.rightHandMovementDistance > 0.0) && overallStatsData.handDistanceTravelled >= 100000)
      this._achievementsModel.UnlockAchievement(this._kilometersTravelled100Achievement);
    if (levelCompletionResults.goodCutsCount > 0 && overallStatsData.goodCutsCount >= 10000)
      this._achievementsModel.UnlockAchievement(this._goodCuts10000Achievement);
    if (levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
      return;
    if (flag)
      this._achievementsModel.UnlockAchievement(this._clearedLevelWithoutModifiersAchievement);
    if (overallStatsData.clearedLevelsCount >= 100)
      this._achievementsModel.UnlockAchievement(this._clearedLevel100Achievement);
    if (difficultyBeatmap.difficulty == BeatmapDifficulty.Expert & flag)
      this._achievementsModel.UnlockAchievement(this._expertLevelClearedWithoutModifiersAchievement);
    if (levelCompletionResults.gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Faster)
      this._achievementsModel.UnlockAchievement(this._clearedLevelWithSongSpeedFasterModifierAchievement);
    if (levelCompletionResults.gameplayModifiers.instaFail)
      this._achievementsModel.UnlockAchievement(this._clearedLevelWithInstaFailModifierAchievement);
    if (levelCompletionResults.gameplayModifiers.disappearingArrows)
      this._achievementsModel.UnlockAchievement(this._clearedLevelWithDisappearingArrowsModifierAchievement);
    if (levelCompletionResults.gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery)
      this._achievementsModel.UnlockAchievement(this._clearedLevelWithBatteryEnergyModifierAchievement);
    if (levelCompletionResults.rank >= RankModel.Rank.A & flag && difficultyBeatmap.difficulty == BeatmapDifficulty.Normal)
      this._achievementsModel.UnlockAchievement(this._resultMinRankANormalWithoutModifiersAchievement);
    if (levelCompletionResults.rank >= RankModel.Rank.S & flag && difficultyBeatmap.difficulty == BeatmapDifficulty.Hard)
      this._achievementsModel.UnlockAchievement(this._resultMinRankSHardWithoutModifiersAchievement);
    if (levelCompletionResults.rank >= RankModel.Rank.SS && flag && difficultyBeatmap.difficulty == BeatmapDifficulty.Expert)
      this._achievementsModel.UnlockAchievement(this._resultMinRankSSExpertWithoutModifiersAchievement);
    if (levelCompletionResults.fullCombo && difficultyBeatmap.difficulty == BeatmapDifficulty.Expert & flag)
      this._achievementsModel.UnlockAchievement(this._fullComboExpertWithoutModifiersAchievement);
    if (levelCompletionResults.maxCombo >= 50 & flag && difficultyBeatmap.difficulty == BeatmapDifficulty.Normal)
      this._achievementsModel.UnlockAchievement(this._combo50NormalWithoutModifiersAchievement);
    if (levelCompletionResults.maxCombo >= 100 & flag && difficultyBeatmap.difficulty == BeatmapDifficulty.Hard)
      this._achievementsModel.UnlockAchievement(this._combo100HardWithoutModifiersAchievement);
    if (!(levelCompletionResults.maxCombo >= 500 & flag) || difficultyBeatmap.difficulty != BeatmapDifficulty.Expert)
      return;
    this._achievementsModel.UnlockAchievement(this._combo500ExpertWithoutModifiersAchievement);
  }
}
