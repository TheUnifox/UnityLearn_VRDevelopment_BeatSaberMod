// Decompiled with JetBrains decompiler
// Type: PlatformLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlatformLeaderboardsModel : MonoBehaviour
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [Inject]
  protected readonly LeaderboardScoreUploader _leaderboardScoreUploader;
  [Inject]
  protected readonly IPlatformUserModel _platformUserModel;
  [Inject]
  protected readonly PlatformLeaderboardsHandler _platformLeaderboardsHandler;
  protected PlatformLeaderboardsModel.State _state;
  protected string _playerId;

  public event System.Action allScoresDidUploadEvent;

  private bool initialized
  {
    get
    {
      if (this._state == PlatformLeaderboardsModel.State.NotInitialized)
        this.Initialize();
      return this._state == PlatformLeaderboardsModel.State.Initialized;
    }
  }

  public virtual async void Initialize()
  {
    PlatformLeaderboardsModel leaderboardsModel = this;
    if (leaderboardsModel._state != PlatformLeaderboardsModel.State.NotInitialized)
      return;
    leaderboardsModel._state = PlatformLeaderboardsModel.State.Initializing;
    UserInfo userInfo = await leaderboardsModel._platformUserModel.GetUserInfo();
    if (userInfo != null)
    {
      leaderboardsModel._playerId = userInfo.platformUserId;
      leaderboardsModel._leaderboardScoreUploader.Init(new LeaderboardScoreUploader.UploadScoreCallback(leaderboardsModel.UploadScore), leaderboardsModel._playerId);
      leaderboardsModel._leaderboardScoreUploader.allScoresDidUploadEvent += new System.Action(leaderboardsModel.HandleAllScoresDidUpload);
      leaderboardsModel._state = PlatformLeaderboardsModel.State.Initialized;
    }
    else
      leaderboardsModel._state = PlatformLeaderboardsModel.State.NotInitialized;
  }

  public virtual HMAsyncRequest UploadScore(
    LeaderboardScoreUploader.ScoreData scoreData,
    PlatformLeaderboardsModel.UploadScoreCompletionHandler completionHandler)
  {
    if (this.initialized)
      return this._platformLeaderboardsHandler.UploadScore(scoreData, completionHandler);
    if (completionHandler != null)
      completionHandler(PlatformLeaderboardsModel.UploadScoreResult.Failed);
    return (HMAsyncRequest) null;
  }

  public virtual HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.ScoresScope scope,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    if (this.initialized)
      return this._platformLeaderboardsHandler.GetScores(beatmap, count, fromRank, scope, this._playerId, completionHandler);
    if (completionHandler != null)
      completionHandler(PlatformLeaderboardsModel.GetScoresResult.Failed, (PlatformLeaderboardsModel.LeaderboardScore[]) null, -1);
    return (HMAsyncRequest) null;
  }

  public virtual void HandleAllScoresDidUpload()
  {
    System.Action scoresDidUploadEvent = this.allScoresDidUploadEvent;
    if (scoresDidUploadEvent == null)
      return;
    scoresDidUploadEvent();
  }

  public virtual HMAsyncRequest GetScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    return this.GetScores(beatmap, count, fromRank, PlatformLeaderboardsModel.ScoresScope.Global, completionHandler);
  }

  public virtual HMAsyncRequest GetScoresAroundPlayer(
    IDifficultyBeatmap beatmap,
    int count,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    return this.GetScores(beatmap, count, 0, PlatformLeaderboardsModel.ScoresScope.AroundPlayer, completionHandler);
  }

  public virtual HMAsyncRequest GetFriendsScores(
    IDifficultyBeatmap beatmap,
    int count,
    int fromRank,
    PlatformLeaderboardsModel.GetScoresCompletionHandler completionHandler)
  {
    return this.GetScores(beatmap, count, fromRank, PlatformLeaderboardsModel.ScoresScope.Friends, completionHandler);
  }

  public virtual void UploadScore(
    IDifficultyBeatmap beatmap,
    int multipliedScore,
    int modifiedScore,
    int maxPossibleMultipliedScore,
    bool fullCombo,
    int goodCutsCount,
    int badCutsCount,
    int missedCount,
    int maxCombo,
    float energy,
    GameplayModifiers gameplayModifiers)
  {
    List<GameplayModifierParamsSO> modifierParamsList = this._gameplayModifiersModel.CreateModifierParamsList(gameplayModifiers);
    int num = this._gameplayModifiersModel.MaxModifiedScoreForMaxMultipliedScore(maxPossibleMultipliedScore, modifierParamsList, energy);
    if (multipliedScore > maxPossibleMultipliedScore || modifiedScore > num || !this.initialized)
      return;
    this._leaderboardScoreUploader.AddScore(new LeaderboardScoreUploader.ScoreData(this._playerId, beatmap, multipliedScore, modifiedScore, fullCombo, goodCutsCount, badCutsCount, missedCount, maxCombo, gameplayModifiers));
  }

  public enum State
  {
    NotInitialized,
    Initializing,
    Initialized,
  }

  public enum GetScoresResult
  {
    Ok,
    Failed,
  }

  public enum UploadScoreResult
  {
    Ok,
    Failed,
  }

  public enum ScoresScope
  {
    Global,
    AroundPlayer,
    Friends,
  }

  public delegate void GetScoresCompletionHandler(
    PlatformLeaderboardsModel.GetScoresResult result,
    PlatformLeaderboardsModel.LeaderboardScore[] scores,
    int referencePlayerScoreIndex);

  public delegate void UploadScoreCompletionHandler(
    PlatformLeaderboardsModel.UploadScoreResult result);

  public class LeaderboardScore
  {
    public readonly int score;
    public readonly int rank;
    public readonly string playerName;
    public readonly string playerId;

    public LeaderboardScore(
      int score,
      int rank,
      string playerName,
      string playerId,
      List<GameplayModifierParamsSO> gameplayModifiers)
    {
      this.score = score;
      this.rank = rank;
      this.playerName = playerName;
      this.playerId = playerId;
    }
  }
}
