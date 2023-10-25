// Decompiled with JetBrains decompiler
// Type: LocalLeaderboardViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LocalLeaderboardViewController : LeaderboardViewController
{
  [SerializeField]
  protected int _maxNumberOfCells = 7;
  [SerializeField]
  protected LocalLeaderboardsModel _localLeaderboardsModel;
  [Space]
  [SerializeField]
  protected LocalLeaderboardTableView _leaderboardTableView;
  [SerializeField]
  protected GameObject _clearLeaderboardsWrapper;
  [SerializeField]
  protected NoTransitionsButton _clearLeaderboardsButton;
  [SerializeField]
  protected IconSegmentedControl _scopeSegmentedControl;
  [Space]
  [SerializeField]
  protected Sprite _allTimeLeaderboardIcon;
  [SerializeField]
  protected Sprite _todayLeaderboardIcon;
  [SerializeField]
  protected Sprite _clearLeaderboardIcon;
  [Inject]
  protected PlayerDataModel _playerDataModel;
  [DoesNotRequireDomainReloadInit]
  protected static LocalLeaderboardsModel.LeaderboardType _leaderboardType;
  protected IDifficultyBeatmap _difficultyBeatmap;
  protected bool _refreshIsNeeded;
  protected bool _enableClear;

  public LocalLeaderboardsModel leaderboardsModel => this._localLeaderboardsModel;

  public virtual void Setup(bool enableClear) => this._enableClear = enableClear;

  public override void SetData(IDifficultyBeatmap difficultyBeatmap)
  {
    this._refreshIsNeeded = this._difficultyBeatmap != difficultyBeatmap;
    this._difficultyBeatmap = difficultyBeatmap;
    if (!this.isActivated || !this._refreshIsNeeded)
      return;
    this.Refresh();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this.buttonBinder.AddBinding((Button) this._clearLeaderboardsButton, (System.Action) (() =>
      {
        this.ClearLeaderboards();
        this._scopeSegmentedControl.SelectCellWithNumber(0);
        this.HandleScopeSegmentedControlDidSelectCell((SegmentedControl) this._scopeSegmentedControl, 0);
      }));
    if (addedToHierarchy)
    {
      this.RefreshScopeSegmentedControl();
      this._localLeaderboardsModel.newScoreWasAddedToLeaderboardEvent += new System.Action<string, LocalLeaderboardsModel.LeaderboardType>(this.HandleNewScoreWasAddedToLeaderboard);
      this._scopeSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
      this._scopeSegmentedControl.SelectCellWithNumber((int) LocalLeaderboardViewController._leaderboardType);
      this._refreshIsNeeded = true;
      this.HandleScopeSegmentedControlDidSelectCell((SegmentedControl) this._scopeSegmentedControl, (int) LocalLeaderboardViewController._leaderboardType);
    }
    if (!this._refreshIsNeeded)
      return;
    this.Refresh();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._localLeaderboardsModel.newScoreWasAddedToLeaderboardEvent -= new System.Action<string, LocalLeaderboardsModel.LeaderboardType>(this.HandleNewScoreWasAddedToLeaderboard);
    this._scopeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    this._localLeaderboardsModel.newScoreWasAddedToLeaderboardEvent -= new System.Action<string, LocalLeaderboardsModel.LeaderboardType>(this.HandleNewScoreWasAddedToLeaderboard);
  }

  public virtual void RefreshScopeSegmentedControl()
  {
    List<IconSegmentedControl.DataItem> dataItemList = new List<IconSegmentedControl.DataItem>(3);
    dataItemList.Add(new IconSegmentedControl.DataItem(this._allTimeLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_ALL_TIME")));
    dataItemList.Add(new IconSegmentedControl.DataItem(this._todayLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_TODAY")));
    if (this._enableClear)
      dataItemList.Add(new IconSegmentedControl.DataItem(this._clearLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_CLEAR_ALL")));
    this._scopeSegmentedControl.SetData(dataItemList.ToArray());
  }

  public virtual void HandleScopeSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellNumber)
  {
    switch (cellNumber)
    {
      case 0:
        this._leaderboardTableView.gameObject.SetActive(true);
        this._clearLeaderboardsWrapper.SetActive(false);
        LocalLeaderboardViewController._leaderboardType = LocalLeaderboardsModel.LeaderboardType.AllTime;
        this.Refresh();
        break;
      case 1:
        this._leaderboardTableView.gameObject.SetActive(true);
        this._clearLeaderboardsWrapper.SetActive(false);
        LocalLeaderboardViewController._leaderboardType = LocalLeaderboardsModel.LeaderboardType.Daily;
        this.Refresh();
        break;
      case 2:
        this._leaderboardTableView.gameObject.SetActive(false);
        this._clearLeaderboardsWrapper.SetActive(true);
        break;
    }
  }

  public virtual void ClearLeaderboards()
  {
    this._localLeaderboardsModel.ClearAllLeaderboards(true);
    this._localLeaderboardsModel.ClearLastScorePosition();
    this._playerDataModel.playerData.DeleteAllGuestPlayers();
    this._playerDataModel.Save();
  }

  public virtual void SetContent(
    string leaderboardID,
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    this._leaderboardTableView.SetScores(this._localLeaderboardsModel.GetScores(leaderboardID, leaderboardType), this._localLeaderboardsModel.GetLastScorePosition(leaderboardID, leaderboardType), this._maxNumberOfCells);
  }

  public virtual void HandleNewScoreWasAddedToLeaderboard(
    string leaderboardID,
    LocalLeaderboardsModel.LeaderboardType leaderboardType)
  {
    if (LocalLeaderboardViewController._leaderboardType != leaderboardType || !(leaderboardID == LocalLeaderboardsIdModel.GetLocalLeaderboardID(this._difficultyBeatmap)))
      return;
    if (this.isActivated)
      this.Refresh();
    else
      this._refreshIsNeeded = true;
  }

  public virtual void Refresh()
  {
    this._refreshIsNeeded = false;
    this.SetContent(LocalLeaderboardsIdModel.GetLocalLeaderboardID(this._difficultyBeatmap), LocalLeaderboardViewController._leaderboardType);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__18_0()
  {
    this.ClearLeaderboards();
    this._scopeSegmentedControl.SelectCellWithNumber(0);
    this.HandleScopeSegmentedControlDidSelectCell((SegmentedControl) this._scopeSegmentedControl, 0);
  }
}
