// Decompiled with JetBrains decompiler
// Type: OculusPlatformLeaderboardsHandler
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections.Generic;
using UnityEngine;

public class OculusPlatformLeaderboardsHandler : PlatformLeaderboardsHandler
{
  protected HashSet<ulong> _oculusRequestIds;
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  protected LeaderboardIdsModelSO _leaderboardIdsModel;

  public OculusPlatformLeaderboardsHandler(LeaderboardIdsModelSO leaderboardIdsModel)
  {
    this._oculusRequestIds = new HashSet<ulong>();
    this._leaderboardIdsModel = leaderboardIdsModel;
    Message<LeaderboardEntryList>.Callback callback = (Message<LeaderboardEntryList>.Callback) (message => { });
  }

  public virtual void AddOculusRequest(Request oculusRequest, HMAsyncRequest asyncRequest)
  {
    this._oculusRequestIds.Add(oculusRequest.RequestID);
    if (asyncRequest == null)
      return;
    asyncRequest.CancelHandler = (HMAsyncRequest.CancelHander) (request => this._oculusRequestIds.Remove(oculusRequest.RequestID));
  }

  public virtual bool CheckMessageForValidRequest(Message message)
  {
    if (!this._oculusRequestIds.Contains(message.RequestID))
      return false;
    this._oculusRequestIds.Remove(message.RequestID);
    return true;
  }

  public override HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.ScoresScope scope,
    string referencePlayerId,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    HMAsyncRequest asyncRequest = new HMAsyncRequest();
    string platformLeaderboardId;
    if (!this._leaderboardIdsModel.TryGetPlatformLeaderboardId(beatmap, out platformLeaderboardId))
    {
      Debug.LogError((object) ("Leaderboard id for " + beatmap.SerializedName() + " not found"));
      if (completionHandler != null)
        completionHandler(PlatformLeaderboardsModel.GetScoresResult.Failed, (PlatformLeaderboardsModel.LeaderboardScore[]) null, -1);
      return asyncRequest;
    }
    Message<LeaderboardEntryList>.Callback callback = (Message<LeaderboardEntryList>.Callback) (message =>
    {
      if (!this.CheckMessageForValidRequest((Message) message))
        return;
      if (message.IsError)
      {
        if (message.GetError().Code == 12074)
        {
          if (completionHandler == null)
            return;
          completionHandler(PlatformLeaderboardsModel.GetScoresResult.Ok, new PlatformLeaderboardsModel.LeaderboardScore[0], -1);
        }
        else
        {
          if (completionHandler == null)
            return;
          completionHandler(PlatformLeaderboardsModel.GetScoresResult.Failed, (PlatformLeaderboardsModel.LeaderboardScore[]) null, -1);
        }
      }
      else
      {
        PlatformLeaderboardsModel.LeaderboardScore[] scores = new PlatformLeaderboardsModel.LeaderboardScore[message.Data.Count];
        int referencePlayerScoreIndex = -1;
        for (int index = 0; index < message.Data.Count; ++index)
        {
          Oculus.Platform.Models.LeaderboardEntry leaderboardEntry = message.Data[index];
          scores[index] = new PlatformLeaderboardsModel.LeaderboardScore((int) leaderboardEntry.Score, leaderboardEntry.Rank, leaderboardEntry.User.OculusID.ToString(), leaderboardEntry.User.ID.ToString(), new List<GameplayModifierParamsSO>());
          if (scores[index].playerId == referencePlayerId)
            referencePlayerScoreIndex = index;
        }
        if (completionHandler == null)
          return;
        completionHandler(PlatformLeaderboardsModel.GetScoresResult.Ok, scores, referencePlayerScoreIndex);
      }
    });
    Request oculusRequest;
    switch (scope)
    {
      case PlatformLeaderboardsModel.ScoresScope.AroundPlayer:
        oculusRequest = (Request) Leaderboards.GetEntries(platformLeaderboardId, count, LeaderboardFilterType.None, LeaderboardStartAt.CenteredOnViewer).OnComplete(callback);
        break;
      case PlatformLeaderboardsModel.ScoresScope.Friends:
        oculusRequest = (Request) Leaderboards.GetEntries(platformLeaderboardId, count, LeaderboardFilterType.Friends, LeaderboardStartAt.CenteredOnViewerOrTop).OnComplete(callback);
        break;
      default:
        oculusRequest = (Request) Leaderboards.GetEntriesAfterRank(platformLeaderboardId, count, (ulong) (fromRank - 1)).OnComplete(callback);
        break;
    }
    this.AddOculusRequest(oculusRequest, asyncRequest);
    return asyncRequest;
  }

  public override HMAsyncRequest UploadScore(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler)
  {
    HMAsyncRequest asyncRequest = new HMAsyncRequest();
    string platformLeaderboardId;
    if (!this._leaderboardIdsModel.TryGetPlatformLeaderboardId(scoreData.beatmap, out platformLeaderboardId))
    {
      Debug.LogError((object) ("Leaderboard id for " + scoreData.beatmap.SerializedName() + " not found"));
      if (completionHandler != null)
        completionHandler(PlatformLeaderboardsModel.UploadScoreResult.Failed);
      return asyncRequest;
    }
    this.AddOculusRequest((Request) Leaderboards.WriteEntry(platformLeaderboardId, (long) scoreData.modifiedScore).OnComplete((Message<bool>.Callback) (messsage =>
    {
      if (!this.CheckMessageForValidRequest((Message) messsage) || completionHandler == null)
        return;
      completionHandler(messsage.IsError || !messsage.Data ? PlatformLeaderboardsModel.UploadScoreResult.Failed : PlatformLeaderboardsModel.UploadScoreResult.Ok);
    })), asyncRequest);
    return asyncRequest;
  }
}
