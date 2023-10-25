// Decompiled with JetBrains decompiler
// Type: TestPlatformLeaderboardsHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class TestPlatformLeaderboardsHandler : PlatformLeaderboardsHandler
{
  public override HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.ScoresScope scope,
    string referencePlayerId,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    return new HMAsyncRequest();
  }

  public override HMAsyncRequest UploadScore(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler)
  {
    return new HMAsyncRequest();
  }
}
