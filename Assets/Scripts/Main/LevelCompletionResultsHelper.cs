// Decompiled with JetBrains decompiler
// Type: LevelCompletionResultsHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public abstract class LevelCompletionResultsHelper
{
  public static LevelCompletionResults Create(
    IReadonlyBeatmapData beatmapData,
    BeatmapObjectExecutionRating[] beatmapObjectExecutionRatings,
    GameplayModifiers gameplayModifiers,
    GameplayModifiersModelSO gameplayModifiersModel,
    int multipliedScore,
    int modifiedScore,
    int maxCombo,
    float[] saberActivityValues,
    float leftSaberMovementDistance,
    float rightSaberMovementDistance,
    float[] handActivityValues,
    float leftHandMovementDistance,
    float rightHandMovementDistance,
    LevelCompletionResults.LevelEndStateType levelEndStateType,
    LevelCompletionResults.LevelEndAction levelEndAction,
    float energy,
    float songTime)
  {
    int goodCutsCount = 0;
    int badCutsCount = 0;
    int okCount = 0;
    int notGoodCount = 0;
    int missedCount = 0;
    int maxCutScore = 0;
    int totalCutScore = 0;
    int goodCutsCountForNotesWithFullScoreScoringType = 0;
    float averageCutScoreForNotesWithFullScoreScoringType = 0.0f;
    float averageCenterDistanceCutScoreForNotesWithFullScoreScoringType = 0.0f;
    foreach (BeatmapObjectExecutionRating objectExecutionRating in beatmapObjectExecutionRatings)
    {
      switch (objectExecutionRating)
      {
        case NoteExecutionRating noteExecutionRating:
          if (noteExecutionRating.rating == NoteExecutionRating.Rating.GoodCut)
          {
            ++goodCutsCount;
            if (maxCutScore < noteExecutionRating.cutScore)
              maxCutScore = noteExecutionRating.cutScore;
            if (ScoreModel.fullScoreScoringTypes.Contains(noteExecutionRating.scoringType))
            {
              averageCenterDistanceCutScoreForNotesWithFullScoreScoringType += (float) noteExecutionRating.centerDistanceCutScore;
              averageCutScoreForNotesWithFullScoreScoringType += (float) noteExecutionRating.cutScore;
              ++goodCutsCountForNotesWithFullScoreScoringType;
            }
            totalCutScore += noteExecutionRating.cutScore;
            break;
          }
          if (noteExecutionRating.rating == NoteExecutionRating.Rating.BadCut)
          {
            ++badCutsCount;
            break;
          }
          if (noteExecutionRating.rating == NoteExecutionRating.Rating.Miss)
          {
            ++missedCount;
            break;
          }
          break;
        case BombExecutionRating bombExecutionRating:
          if (bombExecutionRating.rating == BombExecutionRating.Rating.OK)
          {
            ++okCount;
            break;
          }
          ++notGoodCount;
          break;
        case ObstacleExecutionRating obstacleExecutionRating:
          if (obstacleExecutionRating.rating == ObstacleExecutionRating.Rating.OK)
          {
            ++okCount;
            break;
          }
          ++notGoodCount;
          break;
      }
    }
    if (goodCutsCountForNotesWithFullScoreScoringType > 0)
    {
      averageCenterDistanceCutScoreForNotesWithFullScoreScoringType /= (float) goodCutsCountForNotesWithFullScoreScoringType;
      averageCutScoreForNotesWithFullScoreScoringType /= (float) goodCutsCountForNotesWithFullScoreScoringType;
    }
    int multipliedScoreForBeatmap = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(beatmapData);
    List<GameplayModifierParamsSO> modifierParamsList = gameplayModifiersModel.CreateModifierParamsList(gameplayModifiers);
    int maxModifiedScore = gameplayModifiersModel.MaxModifiedScoreForMaxMultipliedScore(multipliedScoreForBeatmap, modifierParamsList, energy);
    RankModel.Rank rankForScore = RankModel.GetRankForScore(multipliedScore, modifiedScore, multipliedScoreForBeatmap, maxModifiedScore);
    return new LevelCompletionResults(gameplayModifiers, modifiedScore, multipliedScore, rankForScore, missedCount == 0 && badCutsCount == 0 && notGoodCount == 0 && maxModifiedScore > 0, leftSaberMovementDistance, rightSaberMovementDistance, leftHandMovementDistance, rightHandMovementDistance, levelEndStateType, levelEndAction, energy, goodCutsCount, badCutsCount, missedCount, notGoodCount, okCount, maxCutScore, totalCutScore, goodCutsCountForNotesWithFullScoreScoringType, averageCenterDistanceCutScoreForNotesWithFullScoreScoringType, averageCutScoreForNotesWithFullScoreScoringType, maxCombo, songTime);
  }

  public static void ProcessScore(
    PlayerData playerData,
    PlayerLevelStatsData playerLevelStats,
    LevelCompletionResults levelCompletionResults,
    IReadonlyBeatmapData transformedBeatmapData,
    IDifficultyBeatmap difficultyBeatmap,
    PlatformLeaderboardsModel platformLeaderboardsModel)
  {
    playerData.IncreaseNumberOfGameplays(playerLevelStats);
    int multipliedScoreForBeatmap = ScoreModel.ComputeMaxMultipliedScoreForBeatmap(transformedBeatmapData);
    if (levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
      return;
    playerLevelStats.UpdateScoreData(levelCompletionResults.modifiedScore, levelCompletionResults.maxCombo, levelCompletionResults.fullCombo, levelCompletionResults.rank);
    platformLeaderboardsModel.UploadScore(difficultyBeatmap, levelCompletionResults.multipliedScore, levelCompletionResults.modifiedScore, multipliedScoreForBeatmap, levelCompletionResults.fullCombo, levelCompletionResults.goodCutsCount, levelCompletionResults.badCutsCount, levelCompletionResults.missedCount, levelCompletionResults.maxCombo, levelCompletionResults.energy, levelCompletionResults.gameplayModifiers);
  }
}
