// Decompiled with JetBrains decompiler
// Type: PlatformLeaderboardsHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class PlatformLeaderboardsHandler
{
  public abstract HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.ScoresScope scope,
    string referencePlayerId,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler);

  public abstract HMAsyncRequest UploadScore(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler);
}
