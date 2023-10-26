// Decompiled with JetBrains decompiler
// Type: CentralLeaderboardViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using OnlineServices;
using Polyglot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CentralLeaderboardViewController : LeaderboardViewController
{
  [SerializeField]
  protected LeaderboardTableView _leaderboardTableView;
  [SerializeField]
  protected IconSegmentedControl _scopeSegmentedControl;
  [SerializeField]
  protected LoadingControl _loadingControl;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [Space]
  [SerializeField]
  protected TextSegmentedControl _leaderboardTypeSegmentedControl;
  [SerializeField]
  protected Sprite _globalLeaderboardIcon;
  [SerializeField]
  protected Sprite _friendsLeaderboardIcon;
  [SerializeField]
  protected Button _enableOnlineServicesButton;
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [SerializeField]
  protected GameObject _optInOnlineServicesView;
  [SerializeField]
  protected GameObject _leaderboardView;
  [Inject]
  protected readonly GameplaySetupViewController _gameplaySetupViewController;
  protected readonly ServerManager _serverManager;
  protected readonly List<LeaderboardTableView.ScoreData> _scores = new List<LeaderboardTableView.ScoreData>(10);
  protected IDifficultyBeatmap _difficultyBeatmap;
  protected CancellationTokenSource _cancellationTokenSource;
  protected CentralLeaderboardViewController.LeaderboardPanel[] _leaderboardPanels;
  protected CentralLeaderboardViewController.ScoreScopeInfo[] _scoreScopeInfos;
  protected GameplayModifiers _gameplayModifiers;

  private GameplayModifiers gameplayModifiers => this._gameplaySetupViewController.gameplayModifiers;

  private bool hasScoresData => this._scores != null && this._scores.Count != 0;

  public override void SetData(IDifficultyBeatmap difficultyBeatmap)
  {
    if (this._difficultyBeatmap == difficultyBeatmap)
      return;
    this._difficultyBeatmap = difficultyBeatmap;
    this.ClearContent();
    if (!this.isActivated)
      return;
    this._gameplayModifiers = this.gameplayModifiers;
    this.Refresh(true, true);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._enableOnlineServicesButton, (System.Action) (() =>
      {
        this._mainSettingsModel.onlineServicesEnabled.value = true;
        this._mainSettingsModel.Save();
        this.Refresh(true, true);
      }));
      this._leaderboardPanels = new CentralLeaderboardViewController.LeaderboardPanel[2]
      {
        new CentralLeaderboardViewController.LeaderboardPanel(Localization.Get("LEADERBOARDS_CURRENT_MODIFIERS_ONLY_TITLE"), "", false),
        new CentralLeaderboardViewController.LeaderboardPanel(Localization.Get("LEADERBOARDS_MIXED_LEADERBOARDS_TITLE"), "", true)
      };
      this._gameplayModifiers = (GameplayModifiers) null;
      this._leaderboardTypeSegmentedControl.SetTexts((IReadOnlyList<string>) ((IEnumerable<CentralLeaderboardViewController.LeaderboardPanel>) this._leaderboardPanels).Select<CentralLeaderboardViewController.LeaderboardPanel, string>((Func<CentralLeaderboardViewController.LeaderboardPanel, string>) (x => x.title)).ToArray<string>());
      this._scoreScopeInfos = new CentralLeaderboardViewController.ScoreScopeInfo[2]
      {
        new CentralLeaderboardViewController.ScoreScopeInfo(OnlineServices.ScoresScope.Global, this._globalLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_GLOBAL")),
        new CentralLeaderboardViewController.ScoreScopeInfo(OnlineServices.ScoresScope.Friends, this._friendsLeaderboardIcon, Localization.Get("BUTTON_HIGHSCORES_FRIENDS"))
      };
      this._scopeSegmentedControl.SetData(((IEnumerable<CentralLeaderboardViewController.ScoreScopeInfo>) this._scoreScopeInfos).Select<CentralLeaderboardViewController.ScoreScopeInfo, IconSegmentedControl.DataItem>((Func<CentralLeaderboardViewController.ScoreScopeInfo, IconSegmentedControl.DataItem>) (x => new IconSegmentedControl.DataItem(x.icon, x.localizedTitle))).ToArray<IconSegmentedControl.DataItem>());
    }
    this._serverManager.scoreForLeaderboardDidUploadEvent += new System.Action<string>(this.HandleScoreForLeaderboardDidUpload);
    this._serverManager.platformServicesAvailabilityInfoChangedEvent += new System.Action<PlatformServicesAvailabilityInfo>(this.HandlelatformServicesAvailabilityInfoChanged);
    this._scopeSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
    this._loadingControl.didPressRefreshButtonEvent += new System.Action(this.HandleDidPressRefreshButton);
    this._leaderboardTypeSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HanldeLeaderboardTypeSegmentedControlDidSelectCell);
    this._gameplaySetupViewController.didChangeGameplayModifiersEvent += new System.Action(this.HandleGameplaySetupViewControllerDidChangeGameplayModifiers);
    this._gameplayModifiers = this.gameplayModifiers;
    if (!(!this.hasScoresData & addedToHierarchy))
      return;
    this.Refresh(true, true);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._serverManager.scoreForLeaderboardDidUploadEvent -= new System.Action<string>(this.HandleScoreForLeaderboardDidUpload);
    this._serverManager.platformServicesAvailabilityInfoChangedEvent -= new System.Action<PlatformServicesAvailabilityInfo>(this.HandlelatformServicesAvailabilityInfoChanged);
    this._scopeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
    this._loadingControl.didPressRefreshButtonEvent -= new System.Action(this.HandleDidPressRefreshButton);
    this._leaderboardTypeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HanldeLeaderboardTypeSegmentedControlDidSelectCell);
    this._gameplaySetupViewController.didChangeGameplayModifiersEvent -= new System.Action(this.HandleGameplaySetupViewControllerDidChangeGameplayModifiers);
    this._loadingControl.Hide();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    this._cancellationTokenSource?.Cancel();
    this._cancellationTokenSource = (CancellationTokenSource) null;
    if ((UnityEngine.Object) this._scopeSegmentedControl != (UnityEngine.Object) null)
      this._scopeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleScopeSegmentedControlDidSelectCell);
    if ((UnityEngine.Object) this._loadingControl != (UnityEngine.Object) null)
      this._loadingControl.didPressRefreshButtonEvent -= new System.Action(this.HandleDidPressRefreshButton);
    this._serverManager.scoreForLeaderboardDidUploadEvent -= new System.Action<string>(this.HandleScoreForLeaderboardDidUpload);
    if (!((UnityEngine.Object) this._leaderboardTypeSegmentedControl != (UnityEngine.Object) null))
      return;
    this._leaderboardTypeSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HanldeLeaderboardTypeSegmentedControlDidSelectCell);
  }

  public virtual void HandleDidPressRefreshButton() => this.Refresh(true, true);

  public virtual void HandleGameplaySetupViewControllerDidChangeGameplayModifiers()
  {
    if (this._gameplayModifiers == null)
      return;
    this._gameplayModifiers = this.gameplayModifiers;
    this.Refresh(true, true);
  }

  public virtual void HandlelatformServicesAvailabilityInfoChanged(
    PlatformServicesAvailabilityInfo availabilityInfo)
  {
    if (availabilityInfo.availability != PlatformServicesAvailabilityInfo.OnlineServicesAvailability.Available)
      return;
    this.Refresh(true, true);
  }

  public virtual void HanldeLeaderboardTypeSegmentedControlDidSelectCell(
    SegmentedControl control,
    int index)
  {
    this._gameplayModifiers = this._leaderboardPanels[index].mixed ? (GameplayModifiers) null : this.gameplayModifiers;
    this.Refresh(true, true);
  }

  public virtual void HandleScopeSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellNumber)
  {
    this.Refresh(true, true);
  }

  public virtual void HandleScoreForLeaderboardDidUpload(string leaderboardId)
  {
    if (!(this._serverManager.GetLeaderboardId(this._difficultyBeatmap) == leaderboardId))
      return;
    this.Refresh(false, false);
  }

  public virtual void Refresh(bool showLoadingIndicator, bool clear)
  {
    if (!(bool) (ObservableVariableSO<bool>) this._mainSettingsModel.onlineServicesEnabled)
    {
      this._optInOnlineServicesView.SetActive(true);
      this._leaderboardView.SetActive(false);
      this._loadingControl.ShowText(Localization.Get("ONLINE_SERVICES_OPTIN_INFO"), false);
    }
    else
    {
      this._optInOnlineServicesView.SetActive(false);
      this._leaderboardView.SetActive(true);
      if (this._difficultyBeatmap.level is CustomBeatmapLevel)
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
  }

  public virtual IEnumerator RefreshDelayed(bool showLoadingIndicator, bool clear)
  {
    if (clear)
      this.ClearContent();
    if (showLoadingIndicator)
      this._loadingControl.ShowLoading();
    else
      this._loadingControl.Hide();
    yield return (object) new WaitForSeconds(0.4f);
    this.LoadScoresAsync();
  }

  public virtual async void LoadScoresAsync()
  {
    IDifficultyBeatmap loadingFordifficultyBeatmap = this._difficultyBeatmap;
    GetLeaderboardFilterData leaderboardFilterData = new GetLeaderboardFilterData(loadingFordifficultyBeatmap, 10, 1, this._scoreScopeInfos[this._scopeSegmentedControl.selectedCellNumber].scoreScope, this._gameplayModifiers);
    this._cancellationTokenSource?.Cancel();
    this._cancellationTokenSource = new CancellationTokenSource();
    LeaderboardEntriesResult leaderboardEntriesAsync;
    try
    {
      CancellationToken cancellationToken = this._cancellationTokenSource.Token;
      leaderboardEntriesAsync = await this._serverManager.GetLeaderboardEntriesAsync(leaderboardFilterData, cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      this._loadingControl.Hide();
      if (this._difficultyBeatmap != loadingFordifficultyBeatmap)
        return;
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException)
    {
      return;
    }
    if (leaderboardEntriesAsync.isError)
    {
      this._loadingControl.ShowText(leaderboardEntriesAsync.localizedErrorMessage, true);
    }
    else
    {
      this._scores.Clear();
      for (int index = 0; index < leaderboardEntriesAsync.leaderboardEntries.Length; ++index)
      {
        LeaderboardEntryData leaderboardEntry = leaderboardEntriesAsync.leaderboardEntries[index];
        string message = "";
        if (!leaderboardEntry.gameplayModifiers.IsWithoutModifiers())
          message = string.Join(", ", this._gameplayModifiersModel.CreateModifierParamsList(leaderboardEntry.gameplayModifiers).Select<GameplayModifierParamsSO, string>((Func<GameplayModifierParamsSO, string>) (m => Localization.Get(m.modifierNameLocalizationKey))));
        Debug.Log((object) message);
        this._scores.Add(new LeaderboardTableView.ScoreData(leaderboardEntry.score, leaderboardEntry.displayName, leaderboardEntry.rank, false));
      }
      this._leaderboardTableView.SetScores(this._scores, leaderboardEntriesAsync.referencePlayerScoreIndex);
    }
  }

  public virtual void ClearContent()
  {
    this._scores.Clear();
    this._leaderboardTableView.SetScores(this._scores, -1);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__26_0()
  {
    this._mainSettingsModel.onlineServicesEnabled.value = true;
    this._mainSettingsModel.Save();
    this.Refresh(true, true);
  }

  public class LeaderboardPanel
  {
    public readonly string title;
    public readonly string hint;
    public readonly bool mixed;

    public LeaderboardPanel(string title, string hint, bool mixed)
    {
      this.title = title;
      this.hint = hint;
      this.mixed = mixed;
    }
  }

  public class ScoreScopeInfo
  {
    public OnlineServices.ScoresScope scoreScope;
    public string localizedTitle;
    public Sprite icon;
    public int playerScorePos = -1;

    public ScoreScopeInfo(OnlineServices.ScoresScope scoreScope, Sprite icon, string localizedTitle)
    {
      this.scoreScope = scoreScope;
      this.icon = icon;
      this.localizedTitle = localizedTitle;
    }
  }
}
