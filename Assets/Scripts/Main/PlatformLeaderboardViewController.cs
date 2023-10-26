// Decompiled with JetBrains decompiler
// Type: PlatformLeaderboardViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlatformLeaderboardViewController : LeaderboardViewController
{
  [SerializeField]
  protected LeaderboardTableView _leaderboardTableView;
  [SerializeField]
  protected IconSegmentedControl _scopeSegmentedControl;
  [SerializeField]
  protected LoadingControl _loadingControl;
  [Space]
  [SerializeField]
  protected Sprite _globalLeaderboardIcon;
  [SerializeField]
  protected Sprite _aroundPlayerLeaderboardIcon;
  [SerializeField]
  protected Sprite _friendsLeaderboardIcon;
  [SerializeField]
  protected LevelStatsView _levelStatsView;
  [Inject]
  protected readonly PlatformLeaderboardsModel _leaderboardsModel;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [DoesNotRequireDomainReloadInit]
  protected static PlatformLeaderboardsModel.ScoresScope _scoresScope;
  protected HMAsyncRequest _getScoresAsyncRequest;
  protected int[] _playerScorePos;
  protected List<LeaderboardTableView.ScoreData> _scores = new List<LeaderboardTableView.ScoreData>(10);
  protected IDifficultyBeatmap _difficultyBeatmap;
  protected bool _refreshIsNeeded;
  protected bool _hasScoresData;
  protected PlatformLeaderboardsModel.ScoresScope[] _scoreScopes;

  public PlatformLeaderboardsModel leaderboardsModel => this._leaderboardsModel;

  public override void SetData(IDifficultyBeatmap difficultyBeatmap)
  {
    if (this._difficultyBeatmap != difficultyBeatmap)
      this._refreshIsNeeded = true;
    this._difficultyBeatmap = difficultyBeatmap;
    if (this.isActivated && this._refreshIsNeeded)
      this.Refresh(true, true);
    this.RefreshLevelStats();
  }

  public override void RefreshLevelStats() => this._levelStatsView.ShowStats(this._difficultyBeatmap, this._playerDataModel.playerData);

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._playerScorePos = new int[3]{ -1, -1, -1 };
      this._scopeSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
      this.rectTransform.anchorMin = Vector2.zero;
      this.rectTransform.anchorMax = Vector2.one;
      this.rectTransform.offsetMin = Vector2.zero;
      this.rectTransform.offsetMax = Vector2.zero;
      this._scoreScopes = new PlatformLeaderboardsModel.ScoresScope[3]
      {
        PlatformLeaderboardsModel.ScoresScope.Global,
        PlatformLeaderboardsModel.ScoresScope.AroundPlayer,
        PlatformLeaderboardsModel.ScoresScope.Friends
      };
      this._scopeSegmentedControl.SetData(new IconSegmentedControl.DataItem[3]
      {
        new IconSegmentedControl.DataItem(this._globalLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_GLOBAL")),
        new IconSegmentedControl.DataItem(this._aroundPlayerLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_AROUND_YOU")),
        new IconSegmentedControl.DataItem(this._friendsLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_FRIENDS"))
      });
    }
    if (addedToHierarchy)
      this._scopeSegmentedControl.SelectCellWithNumber(this.ScoreScopeToScoreScopeIndex(PlatformLeaderboardViewController._scoresScope));
    this._leaderboardsModel.allScoresDidUploadEvent += new System.Action(this.HandlePlatformLeaderboardsModelAllScoresDidUpload);
    this._loadingControl.didPressRefreshButtonEvent += new System.Action(this.HandleDidPressRefreshButton);
    if (!(this._refreshIsNeeded | addedToHierarchy))
      return;
    this.Refresh(true, true);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._leaderboardsModel.allScoresDidUploadEvent -= new System.Action(this.HandlePlatformLeaderboardsModelAllScoresDidUpload);
    this._loadingControl.didPressRefreshButtonEvent -= new System.Action(this.HandleDidPressRefreshButton);
    if (this._getScoresAsyncRequest != null)
    {
      this._getScoresAsyncRequest.Cancel();
      this._getScoresAsyncRequest = (HMAsyncRequest) null;
      this._refreshIsNeeded = true;
    }
    if (!this._hasScoresData)
      this._refreshIsNeeded = true;
    this._loadingControl.Hide();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._scopeSegmentedControl != (UnityEngine.Object) null)
      this._scopeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
    if ((UnityEngine.Object) this._loadingControl != (UnityEngine.Object) null)
      this._loadingControl.didPressRefreshButtonEvent -= new System.Action(this.HandleDidPressRefreshButton);
    this._leaderboardsModel.allScoresDidUploadEvent -= new System.Action(this.HandlePlatformLeaderboardsModelAllScoresDidUpload);
    if (this._getScoresAsyncRequest == null)
      return;
    this._getScoresAsyncRequest.Cancel();
    this._getScoresAsyncRequest = (HMAsyncRequest) null;
  }

  public virtual int ScoreScopeToScoreScopeIndex(PlatformLeaderboardsModel.ScoresScope scoresScope)
  {
    for (int scoreScopeIndex = 0; scoreScopeIndex < this._scoreScopes.Length; ++scoreScopeIndex)
    {
      if (this._scoreScopes[scoreScopeIndex] == PlatformLeaderboardViewController._scoresScope)
        return scoreScopeIndex;
    }
    return 0;
  }

  public virtual PlatformLeaderboardsModel.ScoresScope ScopeScopeIndexToScoreScope(
    int scoreScopeIndex)
  {
    return scoreScopeIndex < this._scoreScopes.Length ? this._scoreScopes[scoreScopeIndex] : this._scoreScopes[0];
  }

  public virtual void HandleDidPressRefreshButton() => this.Refresh(true, true);

  public virtual void HandleLeaderboardsResultsReturned(
    PlatformLeaderboardsModel.GetScoresResult result,
    PlatformLeaderboardsModel.LeaderboardScore[] scores,
    int playerScoreIndex)
  {
    this._loadingControl.Hide();
    this._getScoresAsyncRequest = (HMAsyncRequest) null;
    if (result == PlatformLeaderboardsModel.GetScoresResult.Ok)
    {
      this._hasScoresData = true;
      this._scores.Clear();
      for (int index = 0; index < 10; ++index)
      {
        if (index < scores.Length)
        {
          PlatformLeaderboardsModel.LeaderboardScore score = scores[index];
          this._scores.Add(new LeaderboardTableView.ScoreData(score.score, score.playerName, score.rank, false));
        }
      }
      this._playerScorePos[(int) PlatformLeaderboardViewController._scoresScope] = playerScoreIndex;
      this._leaderboardTableView.SetScores(this._scores, this._playerScorePos[(int) PlatformLeaderboardViewController._scoresScope]);
    }
    else
      this._loadingControl.ShowText(Localization.Get("LEADERBOARDS_LOADING_FAILED"), false);
  }

  public virtual void HandleScopeSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellNumber)
  {
    PlatformLeaderboardViewController._scoresScope = this.ScopeScopeIndexToScoreScope(cellNumber);
    this.Refresh(true, true);
  }

  public virtual void HandlePlatformLeaderboardsModelAllScoresDidUpload() => this.Refresh(false, false);

  public virtual void Refresh(bool showLoadingIndicator, bool clear)
  {
    if (this._difficultyBeatmap == null)
    {
      this.StopAllCoroutines();
      this.ClearContent();
    }
    else if (this._difficultyBeatmap.level is CustomBeatmapLevel)
    {
      this.StopAllCoroutines();
      this.ClearContent();
      this._loadingControl.ShowText(Localization.Get("CUSTOM_LEVELS_LEADERBOARDS_NOT_SUPPORTED"), false);
    }
    else
    {
      if (showLoadingIndicator)
        this._loadingControl.ShowLoading();
      else
        this._loadingControl.Hide();
      this.StopAllCoroutines();
      this.StartCoroutine(this.RefreshDelayed(showLoadingIndicator, clear));
    }
  }

  public virtual IEnumerator RefreshDelayed(bool showLoadingIndicator, bool clear)
  {
    PlatformLeaderboardViewController leaderboardViewController = this;
    leaderboardViewController._refreshIsNeeded = false;
    if (clear)
    {
      leaderboardViewController.ClearContent();
    }
    if (showLoadingIndicator)
      leaderboardViewController._loadingControl.ShowLoading();
    else
      leaderboardViewController._loadingControl.Hide();
    yield return (object) new WaitForSeconds(0.4f);
    if (leaderboardViewController._getScoresAsyncRequest != null)
      leaderboardViewController._getScoresAsyncRequest.Cancel();
    switch (PlatformLeaderboardViewController._scoresScope)
    {
      case PlatformLeaderboardsModel.ScoresScope.Global:
        leaderboardViewController._getScoresAsyncRequest = leaderboardViewController._leaderboardsModel.GetScores(leaderboardViewController._difficultyBeatmap, 10, 1, new PlatformLeaderboardsModel.GetScoresCompletionHandler(leaderboardViewController.HandleLeaderboardsResultsReturned));
        break;
      case PlatformLeaderboardsModel.ScoresScope.AroundPlayer:
        leaderboardViewController._getScoresAsyncRequest = leaderboardViewController._leaderboardsModel.GetScoresAroundPlayer(leaderboardViewController._difficultyBeatmap, 10, new PlatformLeaderboardsModel.GetScoresCompletionHandler(leaderboardViewController.HandleLeaderboardsResultsReturned));
        break;
      case PlatformLeaderboardsModel.ScoresScope.Friends:
        leaderboardViewController._getScoresAsyncRequest = leaderboardViewController._leaderboardsModel.GetFriendsScores(leaderboardViewController._difficultyBeatmap, 10, 1, new PlatformLeaderboardsModel.GetScoresCompletionHandler(leaderboardViewController.HandleLeaderboardsResultsReturned));
        break;
    }
  }

  public virtual void ClearContent()
  {
    this._hasScoresData = false;
    this._scores.Clear();
    this._leaderboardTableView.SetScores(this._scores, this._playerScorePos[(int) PlatformLeaderboardViewController._scoresScope]);
  }
}
