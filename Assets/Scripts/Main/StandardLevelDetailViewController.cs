// Decompiled with JetBrains decompiler
// Type: StandardLevelDetailViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StandardLevelDetailViewController : ViewController
{
  [SerializeField]
  protected StandardLevelDetailView _standardLevelDetailView;
  [SerializeField]
  protected StandardLevelBuyView _standardLevelBuyView;
  [SerializeField]
  protected StandardLevelBuyInfoView _standardLevelBuyInfoView;
  [SerializeField]
  protected LoadingControl _loadingControl;
  [SerializeField]
  protected GameObject _noAllowedBeatmapInfoContainer;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  [Inject]
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;
  [Inject]
  protected readonly DlcPromoPanelModel _dlcPromoPanelModel;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  [LocalizationKey]
  protected const string kLoadingDataErrorNoInternetLocalizationKey = "ERROR_LOADING_DATA_NO_INTERNET_MESSAGE";
  [LocalizationKey]
  protected const string kLoadingDataErrorLocalizationKey = "ERROR_LOADING_DATA";
  protected readonly EventBinder _ownedObjectsEventBinder = new EventBinder();
  protected readonly EventBinder _eventBinder = new EventBinder();
  protected CancellationTokenSource _cancellationTokenSource;
  protected IPreviewBeatmapLevel _previewBeatmapLevel;
  protected IBeatmapLevel _beatmapLevel;
  protected IBeatmapLevelPack _pack;
  protected bool _canBuyPack;
  protected BeatmapDifficultyMask _allowedBeatmapDifficultyMask;
  protected HashSet<BeatmapCharacteristicSO> _notAllowedCharacteristics;
  protected bool _contentIsOwnedAndReady;

  public event System.Action<StandardLevelDetailViewController> didPressActionButtonEvent;

  public event System.Action<StandardLevelDetailViewController, IBeatmapLevelPack> didPressOpenLevelPackButtonEvent;

  public event System.Action<StandardLevelDetailViewController, bool> levelFavoriteStatusDidChangeEvent;

  public event System.Action<StandardLevelDetailViewController, IBeatmapLevel> didPressPracticeButtonEvent;

  public event System.Action<StandardLevelDetailViewController, IDifficultyBeatmap> didChangeDifficultyBeatmapEvent;

  public event System.Action<StandardLevelDetailViewController, StandardLevelDetailViewController.ContentType> didChangeContentEvent;

  public IDifficultyBeatmap selectedDifficultyBeatmap => this._standardLevelDetailView.selectedDifficultyBeatmap;

  public IPreviewBeatmapLevel beatmapLevel => (IPreviewBeatmapLevel) this._beatmapLevel;

  public virtual void SetData(
    IPreviewBeatmapLevel previewBeatmapLevel,
    bool hidePracticeButton,
    bool hide360DegreeBeatmapCharacteristic,
    string playButtonText,
    BeatmapDifficultyMask allowedBeatmapDifficultyMask,
    BeatmapCharacteristicSO[] notAllowedCharacteristics)
  {
    this.SetData(this._beatmapLevelsModel.GetLevelPackForLevelId(previewBeatmapLevel.levelID), previewBeatmapLevel, hidePracticeButton, hide360DegreeBeatmapCharacteristic, false, playButtonText, allowedBeatmapDifficultyMask, notAllowedCharacteristics);
  }

  public virtual void SetData(
    IBeatmapLevelPack pack,
    IPreviewBeatmapLevel previewBeatmapLevel,
    bool hidePracticeButton,
    bool hide360DegreeBeatmapCharacteristic,
    bool canBuyPack,
    string playButtonText,
    BeatmapDifficultyMask allowedBeatmapDifficultyMask,
    BeatmapCharacteristicSO[] notAllowedCharacteristics)
  {
    this._canBuyPack = canBuyPack;
    this._beatmapLevel = (IBeatmapLevel) null;
    this._pack = pack;
    this._previewBeatmapLevel = previewBeatmapLevel;
    this._standardLevelBuyView.SetContent(previewBeatmapLevel);
    this._standardLevelDetailView.hidePracticeButton = hidePracticeButton;
    this._standardLevelDetailView.actionButtonText = playButtonText;
    this._allowedBeatmapDifficultyMask = allowedBeatmapDifficultyMask;
    this._notAllowedCharacteristics = new HashSet<BeatmapCharacteristicSO>((IEnumerable<BeatmapCharacteristicSO>) notAllowedCharacteristics);
    this._contentIsOwnedAndReady = false;
    this._cancellationTokenSource?.Cancel();
    this.RefreshAvailabilityIfNeeded();
    this._analyticsModel.LogImpression("Level Detail", new Dictionary<string, string>()
    {
      ["item_id"] = this._previewBeatmapLevel.levelID
    });
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._standardLevelBuyView.buyButton, new System.Action(this.OpenLevelProductStoreOrShowBuyInfo));
      this.buttonBinder.AddBinding(this._standardLevelBuyInfoView.buyLevelButton, new System.Action(this.BuyLevelButtonWasPressed));
      this.buttonBinder.AddBinding(this._standardLevelBuyInfoView.buyPackButton, new System.Action(this.BuyPackButtonWasPressed));
      this.buttonBinder.AddBinding(this._standardLevelBuyInfoView.openPackButton, (System.Action) (() =>
      {
        System.Action<StandardLevelDetailViewController, IBeatmapLevelPack> levelPackButtonEvent = this.didPressOpenLevelPackButtonEvent;
        if (levelPackButtonEvent == null)
          return;
        levelPackButtonEvent(this, this._pack);
      }));
      this.buttonBinder.AddBinding(this._standardLevelDetailView.actionButton, (System.Action) (() =>
      {
        System.Action<StandardLevelDetailViewController> actionButtonEvent = this.didPressActionButtonEvent;
        if (actionButtonEvent == null)
          return;
        actionButtonEvent(this);
      }));
      this.buttonBinder.AddBinding(this._standardLevelDetailView.practiceButton, (System.Action) (() =>
      {
        System.Action<StandardLevelDetailViewController, IBeatmapLevel> practiceButtonEvent = this.didPressPracticeButtonEvent;
        if (practiceButtonEvent == null)
          return;
        practiceButtonEvent(this, this._beatmapLevel);
      }));
      this._ownedObjectsEventBinder.Bind((System.Action) (() =>
      {
        this._loadingControl.didPressRefreshButtonEvent += new System.Action(this.RefreshAvailabilityIfNeeded);
        this._standardLevelDetailView.didChangeDifficultyBeatmapEvent += new System.Action<StandardLevelDetailView, IDifficultyBeatmap>(this.HandleDidChangeDifficultyBeatmap);
        this._standardLevelDetailView.didFavoriteToggleChangeEvent += new System.Action<StandardLevelDetailView, Toggle>(this.HandleDidFavoriteToggleChange);
      }), (System.Action) (() =>
      {
        if ((UnityEngine.Object) this._loadingControl != (UnityEngine.Object) null)
          this._loadingControl.didPressRefreshButtonEvent -= new System.Action(this.RefreshAvailabilityIfNeeded);
        if (!((UnityEngine.Object) this._standardLevelDetailView != (UnityEngine.Object) null))
          return;
        this._standardLevelDetailView.didChangeDifficultyBeatmapEvent -= new System.Action<StandardLevelDetailView, IDifficultyBeatmap>(this.HandleDidChangeDifficultyBeatmap);
        this._standardLevelDetailView.didFavoriteToggleChangeEvent -= new System.Action<StandardLevelDetailView, Toggle>(this.HandleDidFavoriteToggleChange);
      }));
    }
    this._eventBinder.Bind((System.Action) (() =>
    {
      this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.RefreshAvailabilityIfNeeded);
      this._beatmapLevelsModel.levelDownloadingUpdateEvent += new System.Action<BeatmapLevelsModel.LevelDownloadingUpdate>(this.HandleLevelLoadingUpdate);
    }), (System.Action) (() =>
    {
      if ((UnityEngine.Object) this._additionalContentModel != (UnityEngine.Object) null)
        this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.RefreshAvailabilityIfNeeded);
      if (!((UnityEngine.Object) this._beatmapLevelsModel != (UnityEngine.Object) null))
        return;
      this._beatmapLevelsModel.levelDownloadingUpdateEvent -= new System.Action<BeatmapLevelsModel.LevelDownloadingUpdate>(this.HandleLevelLoadingUpdate);
    }));
    this.RefreshAvailabilityIfNeeded();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._cancellationTokenSource?.Cancel();
    this._eventBinder.ClearAllBindings();
    System.Action<StandardLevelDetailViewController, StandardLevelDetailViewController.ContentType> changeContentEvent = this.didChangeContentEvent;
    if (changeContentEvent == null)
      return;
    changeContentEvent((StandardLevelDetailViewController) null, StandardLevelDetailViewController.ContentType.Inactive);
  }

  protected override void OnDestroy()
  {
    this._eventBinder.ClearAllBindings();
    this._ownedObjectsEventBinder.ClearAllBindings();
    base.OnDestroy();
  }

  public virtual void RefreshContentLevelDetailView() => this._standardLevelDetailView.RefreshContent();

  public virtual void ClearSelected() => this._standardLevelDetailView.ClearContent();

  public virtual void HandleDidChangeDifficultyBeatmap(
    StandardLevelDetailView view,
    IDifficultyBeatmap beatmap)
  {
    this._playerDataModel.playerData.SetLastSelectedBeatmapDifficulty(beatmap.difficulty);
    this._playerDataModel.playerData.SetLastSelectedBeatmapCharacteristic(beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
    System.Action<StandardLevelDetailViewController, IDifficultyBeatmap> difficultyBeatmapEvent = this.didChangeDifficultyBeatmapEvent;
    if (difficultyBeatmapEvent == null)
      return;
    difficultyBeatmapEvent(this, beatmap);
  }

  public virtual void HandleDidFavoriteToggleChange(StandardLevelDetailView view, Toggle toggle)
  {
    if (toggle.isOn)
      this._playerDataModel.playerData.AddLevelToFavorites(this._previewBeatmapLevel);
    else
      this._playerDataModel.playerData.RemoveLevelFromFavorites(this._previewBeatmapLevel);
    System.Action<StandardLevelDetailViewController, bool> statusDidChangeEvent = this.levelFavoriteStatusDidChangeEvent;
    if (statusDidChangeEvent == null)
      return;
    statusDidChangeEvent(this, toggle.isOn);
  }

  public virtual void HandleLevelLoadingUpdate(
    BeatmapLevelsModel.LevelDownloadingUpdate levelLoadingUpdate)
  {
    if (levelLoadingUpdate.levelID != this._previewBeatmapLevel.levelID)
      return;
    if (levelLoadingUpdate.downloadingState == BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState.Completed)
    {
      this.RefreshAvailabilityIfNeeded();
    }
    else
    {
      int num = levelLoadingUpdate.downloadingState == BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState.PreparingToDownload ? 1 : 0;
      this.ShowContent(StandardLevelDetailViewController.ContentType.OwnedAndDownloading, downloadingProgress: (double) levelLoadingUpdate.bytesTotal > 0.0 ? (float) levelLoadingUpdate.bytesTransferred / (float) levelLoadingUpdate.bytesTotal : 0.0f, downloadingText: num != 0 ? Localization.Get("PREPARING_TO_DOWNLOAD") : Localization.Get("DOWNLOADING"));
    }
  }

  public virtual void BuyLevelButtonWasPressed()
  {
    this._dlcPromoPanelModel.BuyLevelButtonWasPressed(this._previewBeatmapLevel, "Buy Level or Pack", "Buy - " + this._previewBeatmapLevel.songName);
    this.OpenLevelProductStore();
  }

  public virtual void BuyPackButtonWasPressed()
  {
    this._dlcPromoPanelModel.BuyPackButtonWasPressed(this._pack, "Buy Level or Pack", "Buy Info - " + this._previewBeatmapLevel.songName);
    if (this._pack == null)
      return;
    this.ShowLoadingAndDoSomething((Func<CancellationToken, Task>) (async token =>
    {
      int num = (int) await this._additionalContentModel.OpenLevelPackProductStoreAsync(this._pack.packID, token);
      this.RefreshAvailabilityIfNeeded();
    }));
  }

  public virtual async Task LoadBeatmapLevelAsync(CancellationToken cancellationToken)
  {
    BeatmapLevelsModel.GetBeatmapLevelResult beatmapLevelAsync = await this._beatmapLevelsModel.GetBeatmapLevelAsync(this._previewBeatmapLevel.levelID, cancellationToken);
    if (beatmapLevelAsync.isError || beatmapLevelAsync.beatmapLevel == null)
    {
      this.ShowContent(StandardLevelDetailViewController.ContentType.Error, await InternetConnectionChecker.IsConnectedToInternetAsync(cancellationToken) ? Localization.Get("ERROR_LOADING_DATA") : Localization.Get("ERROR_LOADING_DATA_NO_INTERNET_MESSAGE"));
    }
    else
    {
      this._beatmapLevel = beatmapLevelAsync.beatmapLevel;
      if (this._allowedBeatmapDifficultyMask != BeatmapDifficultyMask.All || this._notAllowedCharacteristics.Count > 0)
      {
        FilteredBeatmapLevel filteredBeatmapLevel = new FilteredBeatmapLevel(this._beatmapLevel, this._allowedBeatmapDifficultyMask, this._notAllowedCharacteristics);
        if (filteredBeatmapLevel.isEmpty)
        {
          this._beatmapLevel = (IBeatmapLevel) null;
          this.ShowContent(StandardLevelDetailViewController.ContentType.NoAllowedDifficultyBeatmap);
          return;
        }
        this._beatmapLevel = (IBeatmapLevel) filteredBeatmapLevel;
      }
      this._standardLevelDetailView.SetContent(this._beatmapLevel, this._playerDataModel.playerData.lastSelectedBeatmapDifficulty, this._playerDataModel.playerData.lastSelectedBeatmapCharacteristic, this._playerDataModel.playerData);
      this._contentIsOwnedAndReady = true;
      this.ShowContent(StandardLevelDetailViewController.ContentType.OwnedAndReady);
    }
  }

  public virtual void OpenLevelProductStoreOrShowBuyInfo()
  {
    this._dlcPromoPanelModel.BuyLevelButtonWasPressed(this._previewBeatmapLevel, "Level", "Buy - " + this._previewBeatmapLevel.songName);
    this.ShowLoadingAndDoSomething((Func<CancellationToken, Task>) (async token =>
    {
      switch (await this._additionalContentModel.IsPackBetterBuyThanLevelAsync(this._pack.packID, this._cancellationTokenSource.Token))
      {
        case AdditionalContentModel.IsPackBetterBuyThanLevelResult.PackIsBetter:
          this._standardLevelBuyInfoView.RefreshView(Localization.Get("BUY_VIEW_INFO_TEXT"), this._canBuyPack);
          this.ShowContent(StandardLevelDetailViewController.ContentType.BuyInfo);
          break;
        case AdditionalContentModel.IsPackBetterBuyThanLevelResult.LevelIsBetter:
          this.OpenLevelProductStore();
          break;
        case AdditionalContentModel.IsPackBetterBuyThanLevelResult.Failed:
          this.ShowContent(StandardLevelDetailViewController.ContentType.Error, Localization.Get("ERROR_LOADING_DATA"));
          break;
      }
    }));
  }

  public virtual void OpenLevelProductStore() => this.ShowLoadingAndDoSomething((Func<CancellationToken, Task>) (async token =>
  {
    int num = (int) await this._additionalContentModel.OpenLevelProductStoreAsync(this._previewBeatmapLevel.levelID, token);
    this.RefreshAvailabilityIfNeeded();
  }));

  public virtual void RefreshAvailabilityIfNeeded()
  {
    if (this._contentIsOwnedAndReady || !this.isActiveAndEnabled)
      return;
    this.ShowLoadingAndDoSomething((Func<CancellationToken, Task>) (async token =>
    {
      AdditionalContentModel.EntitlementStatus entitlementStatusAsync = await this._additionalContentModel.GetLevelEntitlementStatusAsync(this._previewBeatmapLevel.levelID, token);
      token.ThrowIfCancellationRequested();
      switch (entitlementStatusAsync)
      {
        case AdditionalContentModel.EntitlementStatus.Owned:
          await this.LoadBeatmapLevelAsync(token);
          break;
        case AdditionalContentModel.EntitlementStatus.NotOwned:
          this.ShowContent(StandardLevelDetailViewController.ContentType.Buy);
          break;
        default:
          this.ShowContent(StandardLevelDetailViewController.ContentType.Error, Localization.Get("ERROR_LOADING_DATA"));
          break;
      }
    }));
  }

  public virtual void ShowContent(
    StandardLevelDetailViewController.ContentType contentType,
    string errorText = "",
    float downloadingProgress = 0.0f,
    string downloadingText = "")
  {
    this._standardLevelDetailView.gameObject.SetActive(contentType == StandardLevelDetailViewController.ContentType.OwnedAndReady);
    this._standardLevelBuyView.gameObject.SetActive(contentType == StandardLevelDetailViewController.ContentType.Buy);
    this._standardLevelBuyInfoView.gameObject.SetActive(contentType == StandardLevelDetailViewController.ContentType.BuyInfo);
    this._noAllowedBeatmapInfoContainer.SetActive(contentType == StandardLevelDetailViewController.ContentType.NoAllowedDifficultyBeatmap);
    switch (contentType)
    {
      case StandardLevelDetailViewController.ContentType.Loading:
        this._loadingControl.ShowLoading();
        break;
      case StandardLevelDetailViewController.ContentType.OwnedAndReady:
        this._loadingControl.Hide();
        break;
      case StandardLevelDetailViewController.ContentType.OwnedAndDownloading:
        this._loadingControl.ShowDownloadingProgress(downloadingText, downloadingProgress);
        break;
      case StandardLevelDetailViewController.ContentType.Error:
        this._loadingControl.ShowText(errorText, true);
        break;
      default:
        this._loadingControl.Hide();
        break;
    }
    switch (contentType)
    {
      case StandardLevelDetailViewController.ContentType.Buy:
        this._dlcPromoPanelModel.BuyLevelButtonWasShown(this._previewBeatmapLevel, "Level", "Buy Level - " + this._previewBeatmapLevel.songName);
        break;
      case StandardLevelDetailViewController.ContentType.BuyInfo:
        this._dlcPromoPanelModel.BuyLevelButtonWasShown(this._previewBeatmapLevel, "Buy Level or Pack", "Buy Info - " + this._previewBeatmapLevel.songName);
        this._dlcPromoPanelModel.BuyPackButtonWasShown(this._pack, "Buy Level or Pack", "Buy Level Info - " + this._previewBeatmapLevel.songName);
        break;
    }
    System.Action<StandardLevelDetailViewController, StandardLevelDetailViewController.ContentType> changeContentEvent = this.didChangeContentEvent;
    if (changeContentEvent == null)
      return;
    changeContentEvent(this, contentType);
  }

  public virtual async void ShowLoadingAndDoSomething(Func<CancellationToken, Task> action)
  {
    try
    {
      this.ShowContent(StandardLevelDetailViewController.ContentType.Loading);
      this._cancellationTokenSource?.Cancel();
      this._cancellationTokenSource = new CancellationTokenSource();
      await action(this._cancellationTokenSource.Token);
    }
    catch (OperationCanceledException) { }
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_0()
  {
    System.Action<StandardLevelDetailViewController, IBeatmapLevelPack> levelPackButtonEvent = this.didPressOpenLevelPackButtonEvent;
    if (levelPackButtonEvent == null)
      return;
    levelPackButtonEvent(this, this._pack);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_1()
  {
    System.Action<StandardLevelDetailViewController> actionButtonEvent = this.didPressActionButtonEvent;
    if (actionButtonEvent == null)
      return;
    actionButtonEvent(this);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_2()
  {
    System.Action<StandardLevelDetailViewController, IBeatmapLevel> practiceButtonEvent = this.didPressPracticeButtonEvent;
    if (practiceButtonEvent == null)
      return;
    practiceButtonEvent(this, this._beatmapLevel);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_3()
  {
    this._loadingControl.didPressRefreshButtonEvent += new System.Action(this.RefreshAvailabilityIfNeeded);
    this._standardLevelDetailView.didChangeDifficultyBeatmapEvent += new System.Action<StandardLevelDetailView, IDifficultyBeatmap>(this.HandleDidChangeDifficultyBeatmap);
    this._standardLevelDetailView.didFavoriteToggleChangeEvent += new System.Action<StandardLevelDetailView, Toggle>(this.HandleDidFavoriteToggleChange);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_4()
  {
    if ((UnityEngine.Object) this._loadingControl != (UnityEngine.Object) null)
      this._loadingControl.didPressRefreshButtonEvent -= new System.Action(this.RefreshAvailabilityIfNeeded);
    if (!((UnityEngine.Object) this._standardLevelDetailView != (UnityEngine.Object) null))
      return;
    this._standardLevelDetailView.didChangeDifficultyBeatmapEvent -= new System.Action<StandardLevelDetailView, IDifficultyBeatmap>(this.HandleDidChangeDifficultyBeatmap);
    this._standardLevelDetailView.didFavoriteToggleChangeEvent -= new System.Action<StandardLevelDetailView, Toggle>(this.HandleDidFavoriteToggleChange);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_5()
  {
    this._additionalContentModel.didInvalidateDataEvent += new System.Action(this.RefreshAvailabilityIfNeeded);
    this._beatmapLevelsModel.levelDownloadingUpdateEvent += new System.Action<BeatmapLevelsModel.LevelDownloadingUpdate>(this.HandleLevelLoadingUpdate);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__47_6()
  {
    if ((UnityEngine.Object) this._additionalContentModel != (UnityEngine.Object) null)
      this._additionalContentModel.didInvalidateDataEvent -= new System.Action(this.RefreshAvailabilityIfNeeded);
    if (!((UnityEngine.Object) this._beatmapLevelsModel != (UnityEngine.Object) null))
      return;
    this._beatmapLevelsModel.levelDownloadingUpdateEvent -= new System.Action<BeatmapLevelsModel.LevelDownloadingUpdate>(this.HandleLevelLoadingUpdate);
  }

  [CompilerGenerated]
  public virtual async Task m_CBuyPackButtonWasPressedm_Eb__56_0(CancellationToken token)
  {
    int num = (int) await this._additionalContentModel.OpenLevelPackProductStoreAsync(this._pack.packID, token);
    this.RefreshAvailabilityIfNeeded();
  }

  [CompilerGenerated]
  public virtual async Task m_COpenLevelProductStoreOrShowBuyInfom_Eb__58_0(
    CancellationToken token)
  {
    switch (await this._additionalContentModel.IsPackBetterBuyThanLevelAsync(this._pack.packID, this._cancellationTokenSource.Token))
    {
      case AdditionalContentModel.IsPackBetterBuyThanLevelResult.PackIsBetter:
        this._standardLevelBuyInfoView.RefreshView(Localization.Get("BUY_VIEW_INFO_TEXT"), this._canBuyPack);
        this.ShowContent(StandardLevelDetailViewController.ContentType.BuyInfo);
        break;
      case AdditionalContentModel.IsPackBetterBuyThanLevelResult.LevelIsBetter:
        this.OpenLevelProductStore();
        break;
      case AdditionalContentModel.IsPackBetterBuyThanLevelResult.Failed:
        this.ShowContent(StandardLevelDetailViewController.ContentType.Error, Localization.Get("ERROR_LOADING_DATA"));
        break;
    }
  }

  [CompilerGenerated]
  public virtual async Task m_COpenLevelProductStorem_Eb__59_0(CancellationToken token)
  {
    int num = (int) await this._additionalContentModel.OpenLevelProductStoreAsync(this._previewBeatmapLevel.levelID, token);
    this.RefreshAvailabilityIfNeeded();
  }

  [CompilerGenerated]
  public virtual async Task m_CRefreshAvailabilityIfNeededm_Eb__60_0(CancellationToken token)
  {
    AdditionalContentModel.EntitlementStatus entitlementStatusAsync = await this._additionalContentModel.GetLevelEntitlementStatusAsync(this._previewBeatmapLevel.levelID, token);
    token.ThrowIfCancellationRequested();
    switch (entitlementStatusAsync)
    {
      case AdditionalContentModel.EntitlementStatus.Owned:
        await this.LoadBeatmapLevelAsync(token);
        break;
      case AdditionalContentModel.EntitlementStatus.NotOwned:
        this.ShowContent(StandardLevelDetailViewController.ContentType.Buy);
        break;
      default:
        this.ShowContent(StandardLevelDetailViewController.ContentType.Error, Localization.Get("ERROR_LOADING_DATA"));
        break;
    }
  }

  public enum ContentType
  {
    Loading,
    OwnedAndReady,
    NoAllowedDifficultyBeatmap,
    OwnedAndDownloading,
    Buy,
    BuyInfo,
    Error,
    Inactive,
  }
}
