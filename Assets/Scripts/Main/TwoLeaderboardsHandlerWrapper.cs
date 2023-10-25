// Decompiled with JetBrains decompiler
// Type: TwoLeaderboardsHandlerWrapper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class TwoLeaderboardsHandlerWrapper : PlatformLeaderboardsHandler
{
  protected PlatformLeaderboardsHandler _mainHandler;
  protected PlatformLeaderboardsHandler _shadowHandler;
  protected HMAsyncRequest _getScoresShadowAsyncRequest;
  protected HMAsyncRequest _uploadScoreShadowAsyncRequest;

  public TwoLeaderboardsHandlerWrapper(
    PlatformLeaderboardsHandler mainHandler,
    PlatformLeaderboardsHandler shadowHandler)
  {
    this._mainHandler = mainHandler;
    this._shadowHandler = shadowHandler;
  }

  public override HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.ScoresScope scope,
    string referencePlayerId,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    if (this._getScoresShadowAsyncRequest != null)
    {
      this._getScoresShadowAsyncRequest.Cancel();
      this._getScoresShadowAsyncRequest = (HMAsyncRequest) null;
    }
    this._getScoresShadowAsyncRequest = this._shadowHandler.GetScores(beatmap, count, fromRank, scope, referencePlayerId, (PlatformLeaderboardsModel.GetScoresCompletionHandler) ((result, scores, referencePlayerScoreIndex) => this._getScoresShadowAsyncRequest = (HMAsyncRequest) null));
    return this._mainHandler.GetScores(beatmap, count, fromRank, scope, referencePlayerId, completionHandler);
  }

  public override HMAsyncRequest UploadScore(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler)
  {
    if (this._uploadScoreShadowAsyncRequest != null)
    {
      this._uploadScoreShadowAsyncRequest.Cancel();
      this._uploadScoreShadowAsyncRequest = (HMAsyncRequest) null;
    }
    this._shadowHandler.UploadScore(scoreData, (PlatformLeaderboardsModel.UploadScoreCompletionHandler) (result => this._uploadScoreShadowAsyncRequest = (HMAsyncRequest) null));
    return this._mainHandler.UploadScore(scoreData, completionHandler);
  }

  [CompilerGenerated]
  public virtual void m_CGetScoresm_Eb__5_0(
    PlatformLeaderboardsModel.GetScoresResult result,
    PlatformLeaderboardsModel.LeaderboardScore[] scores,
    int referencePlayerScoreIndex)
  {
    this._getScoresShadowAsyncRequest = (HMAsyncRequest) null;
  }

  [CompilerGenerated]
  public virtual void m_CUploadScorem_Eb__6_0(
    PlatformLeaderboardsModel.UploadScoreResult result)
  {
    this._uploadScoreShadowAsyncRequest = (HMAsyncRequest) null;
  }
}
