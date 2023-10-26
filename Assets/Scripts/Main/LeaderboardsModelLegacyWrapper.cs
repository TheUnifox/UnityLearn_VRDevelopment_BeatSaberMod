// Decompiled with JetBrains decompiler
// Type: LeaderboardsModelLegacyWrapper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using OnlineServices;
using System;
using System.Collections.Generic;
using System.Threading;
using Zenject;

public class LeaderboardsModelLegacyWrapper : PlatformLeaderboardsHandler
{
  [Inject]
  protected ILeaderboardsModel _leaderboardsModel;

  public override HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.ScoresScope scope,
    string referencePlayerId,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    LeaderboardsModelLegacyWrapper.HMAsyncRequestWithCancellationToken asyncRequest = new LeaderboardsModelLegacyWrapper.HMAsyncRequestWithCancellationToken();
    OnlineServices.ScoresScope scope1 = OnlineServices.ScoresScope.Global;
    switch (scope)
    {
      case PlatformLeaderboardsModel.ScoresScope.AroundPlayer:
        scope1 = OnlineServices.ScoresScope.Global;
        break;
      case PlatformLeaderboardsModel.ScoresScope.Friends:
        scope1 = OnlineServices.ScoresScope.Friends;
        break;
    }
    this.GetLeaderboardEntriesAsync(new GetLeaderboardFilterData(beatmap, count, fromRank, scope1, (GameplayModifiers) null), asyncRequest, completionHandler);
    return (HMAsyncRequest) asyncRequest;
  }

  public override HMAsyncRequest UploadScore(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler)
  {
    LeaderboardsModelLegacyWrapper.HMAsyncRequestWithCancellationToken asyncRequest = new LeaderboardsModelLegacyWrapper.HMAsyncRequestWithCancellationToken();
    this.SendLevelScoreResutlAsync(new LevelScoreResultsData(scoreData.beatmap, scoreData.multipliedScore, scoreData.modifiedScore, scoreData.fullCombo, scoreData.goodCutsCount, scoreData.badCutsCount, scoreData.missedCount, scoreData.maxCombo, scoreData.gameplayModifiers), asyncRequest, completionHandler);
    return (HMAsyncRequest) asyncRequest;
  }

  public virtual async void GetLeaderboardEntriesAsync(
    GetLeaderboardFilterData leaderboardFilterData,
    LeaderboardsModelLegacyWrapper.HMAsyncRequestWithCancellationToken asyncRequest,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    GetLeaderboardEntriesResult leaderboardEntriesAsync;
    try
    {
      leaderboardEntriesAsync = await this._leaderboardsModel.GetLeaderboardEntriesAsync(leaderboardFilterData, asyncRequest.cancellationTokenSource.Token);
    }
    catch (OperationCanceledException)
    {
      return;
    }
    if (asyncRequest.cancelled)
      return;
    if (leaderboardEntriesAsync.isError)
    {
      PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler1 = completionHandler;
      if (completionHandler1 == null)
        return;
      completionHandler1(PlatformLeaderboardsModel.GetScoresResult.Failed, (PlatformLeaderboardsModel.LeaderboardScore[]) null, -1);
    }
    else
    {
      PlatformLeaderboardsModel.LeaderboardScore[] scores;
      if (leaderboardEntriesAsync.leaderboardEntries != null && leaderboardEntriesAsync.leaderboardEntries.Length != 0)
      {
        scores = new PlatformLeaderboardsModel.LeaderboardScore[leaderboardEntriesAsync.leaderboardEntries.Length];
        for (int index = 0; index < leaderboardEntriesAsync.leaderboardEntries.Length; ++index)
        {
          LeaderboardEntryData leaderboardEntry = leaderboardEntriesAsync.leaderboardEntries[index];
          List<GameplayModifierParamsSO> gameplayModifiers = new List<GameplayModifierParamsSO>();
          scores[index] = new PlatformLeaderboardsModel.LeaderboardScore(leaderboardEntry.score, leaderboardEntry.rank, leaderboardEntry.displayName, leaderboardEntry.playerId, gameplayModifiers);
        }
      }
      else
        scores = new PlatformLeaderboardsModel.LeaderboardScore[0];
      PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler2 = completionHandler;
      if (completionHandler2 == null)
        return;
      completionHandler2(PlatformLeaderboardsModel.GetScoresResult.Ok, scores, leaderboardEntriesAsync.referencePlayerScoreIndex);
    }
  }

  public virtual async void SendLevelScoreResutlAsync(
    LevelScoreResultsData levelScoreResultsData,
    LeaderboardsModelLegacyWrapper.HMAsyncRequestWithCancellationToken asyncRequest,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler)
  {
    SendLeaderboardEntryResult leaderboardEntryResult;
    try
    {
      leaderboardEntryResult = await this._leaderboardsModel.SendLevelScoreResultAsync(levelScoreResultsData, asyncRequest.cancellationTokenSource.Token);
    }
    catch (OperationCanceledException)
    {
      PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler1 = completionHandler;
      if (completionHandler1 == null)
        return;
      completionHandler1(PlatformLeaderboardsModel.UploadScoreResult.Failed);
      return;
    }
    if (asyncRequest.cancelled || leaderboardEntryResult == SendLeaderboardEntryResult.Failed)
    {
      PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler2 = completionHandler;
      if (completionHandler2 == null)
        return;
      completionHandler2(PlatformLeaderboardsModel.UploadScoreResult.Failed);
    }
    else
    {
      PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler3 = completionHandler;
      if (completionHandler3 == null)
        return;
      completionHandler3(PlatformLeaderboardsModel.UploadScoreResult.Ok);
    }
  }

  public class HMAsyncRequestWithCancellationToken : HMAsyncRequest
  {
    protected CancellationTokenSource _cancellationTokenSource;

    public CancellationTokenSource cancellationTokenSource => this._cancellationTokenSource;

    public HMAsyncRequestWithCancellationToken() => this._cancellationTokenSource = new CancellationTokenSource();

    public override void Cancel()
    {
      base.Cancel();
      this._cancellationTokenSource?.Cancel();
    }
  }
}
