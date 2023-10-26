// Decompiled with JetBrains decompiler
// Type: LevelSearchViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class LevelSearchViewController : ViewController
{
  [SerializeField]
  protected Button _searchButton;
  [SerializeField]
  protected Button _clearFiltersButton;
  [SerializeField]
  protected TextMeshProUGUI _filterParamsText;
  [SerializeField]
  protected GameObject _filterPlaceholder;
  [SerializeField]
  protected InputFieldView _searchTextInputFieldView;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly SongPackMasksModel _songPackMasksModel;
  [Inject]
  protected readonly AdditionalContentModel _additionalContentModel;
  protected LevelSearchViewController.BeatmapLevelPackCollection _beatmapLevelPackCollection;
  protected BeatmapCharacteristicSO _preferredBeatmapCharacteristic;
  protected LevelFilterParams _currentFilterParams;
  protected bool _onlyFavorites;
  protected CancellationTokenSource _cancellationTokenSource;
  protected IBeatmapLevelPack[] _beatmapLevelPacks;

  public event System.Action<LevelSearchViewController, LevelFilterParams> didPressSearchButtonEvent;

  public event System.Action<IAnnotatedBeatmapLevelCollection, BeatmapCharacteristicSO> didFilterBeatmapLevelCollectionEvent;

  public event System.Action<LevelSearchViewController> didStartLoadingEvent;

  public virtual void Setup(IBeatmapLevelPack[] beatmapLevelPacks)
  {
    this._onlyFavorites = false;
    this._beatmapLevelPacks = beatmapLevelPacks;
    this.ResetCurrentFilterParams();
  }

  public virtual void ResetFilterParams(bool onlyFavorites)
  {
    this._onlyFavorites = onlyFavorites;
    this.UpdateCurrentFilterParams();
    this.UpdateBeatmapLevelPackCollectionAsync();
  }

  public virtual void UpdateSearchLevelFilterParams(LevelFilterParams levelFilterParams)
  {
    this._currentFilterParams = levelFilterParams;
    this.Refresh();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._searchButton, (System.Action) (() =>
      {
        System.Action<LevelSearchViewController, LevelFilterParams> searchButtonEvent = this.didPressSearchButtonEvent;
        if (searchButtonEvent == null)
          return;
        searchButtonEvent(this, this._currentFilterParams);
      }));
      this.buttonBinder.AddBinding(this._clearFiltersButton, (System.Action) (() =>
      {
        this.ResetCurrentFilterParams();
        this.Refresh();
      }));
    }
    if (!addedToHierarchy)
      return;
    this._searchTextInputFieldView.onValueChanged.AddListener(new UnityAction<InputFieldView>(this.SearchTextInputFieldViewOnValueChanged));
    System.Action<IAnnotatedBeatmapLevelCollection, BeatmapCharacteristicSO> levelCollectionEvent = this.didFilterBeatmapLevelCollectionEvent;
    if (levelCollectionEvent != null)
      levelCollectionEvent((IAnnotatedBeatmapLevelCollection) this._beatmapLevelPackCollection, this._preferredBeatmapCharacteristic);
    this.ResetCurrentFilterParams();
    this.Refresh();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this.ResetCurrentFilterParams();
    this._searchTextInputFieldView.onValueChanged.RemoveListener(new UnityAction<InputFieldView>(this.SearchTextInputFieldViewOnValueChanged));
  }

  public virtual void ResetCurrentFilterParams()
  {
    if (this._onlyFavorites)
    {
      this._currentFilterParams = LevelFilterParams.ByBeatmapLevelIds(this._playerDataModel.playerData.favoritesLevelIds);
    }
    else
    {
      this._currentFilterParams = LevelFilterParams.NoFilter();
      this._currentFilterParams.searchText = string.Empty;
    }
    this._searchTextInputFieldView.SetText(string.Empty);
    this._filterParamsText.text = string.Empty;
    this._filterPlaceholder.SetActive(true);
  }

  public virtual async void UpdateBeatmapLevelPackCollectionAsync()
  {
    LevelSearchViewController searchViewController = this;
    searchViewController._clearFiltersButton.interactable = !searchViewController._currentFilterParams.IsWithoutFilter(true);
    searchViewController._cancellationTokenSource?.Cancel();
    searchViewController._cancellationTokenSource?.Dispose();
    searchViewController._cancellationTokenSource = (CancellationTokenSource) null;
    searchViewController._cancellationTokenSource = new CancellationTokenSource();
    CancellationToken token = searchViewController._cancellationTokenSource.Token;
    try
    {
      System.Action<LevelSearchViewController> startLoadingEvent = searchViewController.didStartLoadingEvent;
      if (startLoadingEvent != null)
        startLoadingEvent(searchViewController);
      // ISSUE: explicit non-virtual call
      searchViewController._filterParamsText.text = searchViewController.LocalizedLevelFilterParamsDescription(searchViewController._currentFilterParams);
      searchViewController._filterPlaceholder.SetActive(string.IsNullOrEmpty(searchViewController._filterParamsText.text));
      IBeatmapLevelCollection beatmapLevelCollection = await BeatmapLevelFilterModel.FilerBeatmapLevelPackCollectionAsync(searchViewController._beatmapLevelPacks, searchViewController._currentFilterParams, searchViewController._playerDataModel, searchViewController._additionalContentModel, token);
      searchViewController._beatmapLevelPackCollection = new LevelSearchViewController.BeatmapLevelPackCollection(beatmapLevelCollection);
      searchViewController._preferredBeatmapCharacteristic = (bool) (UnityEngine.Object) searchViewController._currentFilterParams.filteredCharacteristic ? searchViewController._currentFilterParams.filteredCharacteristic : (BeatmapCharacteristicSO) null;
      System.Action<IAnnotatedBeatmapLevelCollection, BeatmapCharacteristicSO> levelCollectionEvent = searchViewController.didFilterBeatmapLevelCollectionEvent;
      if (levelCollectionEvent == null)
        return;
      levelCollectionEvent((IAnnotatedBeatmapLevelCollection) searchViewController._beatmapLevelPackCollection, searchViewController._preferredBeatmapCharacteristic);
    }
    catch (OperationCanceledException)
    {
    }
    finally
    {
      searchViewController._cancellationTokenSource?.Dispose();
      searchViewController._cancellationTokenSource = (CancellationTokenSource) null;
    }
  }

  public virtual void SearchTextInputFieldViewOnValueChanged(InputFieldView inputFieldView)
  {
    this._currentFilterParams.searchText = inputFieldView.text;
    this.Refresh();
  }

  public virtual void UpdateCurrentFilterParams()
  {
    this._currentFilterParams.filterByLevelIds = this._onlyFavorites;
    this._currentFilterParams.beatmapLevelIds = this._onlyFavorites ? this._playerDataModel.playerData.favoritesLevelIds : (HashSet<string>) null;
  }

  public virtual void RefreshAfterIncreaseNumberOfGameplay()
  {
    if (!this._currentFilterParams.filterByNotPlayedYet)
      return;
    this.Refresh();
  }

  public virtual void Refresh() => this.UpdateBeatmapLevelPackCollectionAsync();

  public virtual string LocalizedLevelFilterParamsDescription(LevelFilterParams levelFilterParams)
  {
    List<string> stringList = new List<string>();
    if (levelFilterParams.filterByOwned)
      stringList.Add(Localization.Get("FILTER_BY_OWNED_SONGS"));
    if (levelFilterParams.filterByNotOwned)
      stringList.Add(Localization.Get("FILTER_BY_NOT_OWNED_SONGS"));
    if (levelFilterParams.filterByCharacteristic)
      stringList.Add(Localization.Get(levelFilterParams.filteredCharacteristic.descriptionLocalizationKey) ?? "");
    if (levelFilterParams.filterByDifficulty)
      stringList.Add(Localization.Get(levelFilterParams.filteredDifficulty.LocalizedKey()) ?? "");
    if (levelFilterParams.filterBySongPacks)
      stringList.Add(this._songPackMasksModel.GetSongPackMaskText(levelFilterParams.filteredSongPacks) ?? "");
    if (levelFilterParams.filterByNotPlayedYet)
      stringList.Add(Localization.Get("FILTER_BY_NOT_PLAYED_YET"));
    if (levelFilterParams.filterByMinBpm)
      stringList.Add(string.Format("{0}:{1}", (object) Localization.Get("MIN_BPM"), (object) levelFilterParams.filteredMinBpm));
    if (levelFilterParams.filterByMaxBpm)
      stringList.Add(string.Format("{0}:{1}", (object) Localization.Get("MAX_BPM"), (object) levelFilterParams.filteredMaxBpm));
    return stringList.Count != 0 ? string.Join(", ", stringList.ToArray()) : "";
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__27_0()
  {
    System.Action<LevelSearchViewController, LevelFilterParams> searchButtonEvent = this.didPressSearchButtonEvent;
    if (searchButtonEvent == null)
      return;
    searchButtonEvent(this, this._currentFilterParams);
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__27_1()
  {
    this.ResetCurrentFilterParams();
    this.Refresh();
  }

  public class BeatmapLevelPackCollection : IAnnotatedBeatmapLevelCollection
  {
    [CompilerGenerated]
    protected IBeatmapLevelCollection m_CbeatmapLevelCollection;

    public string collectionName => (string) null;

    public Sprite coverImage => (Sprite) null;

    public Sprite smallCoverImage => (Sprite) null;

    public IBeatmapLevelCollection beatmapLevelCollection
    {
      get => this.m_CbeatmapLevelCollection;
      private set => this.m_CbeatmapLevelCollection = value;
    }

    public BeatmapLevelPackCollection(IBeatmapLevelCollection beatmapLevelCollection) => this.beatmapLevelCollection = beatmapLevelCollection;
  }
}
