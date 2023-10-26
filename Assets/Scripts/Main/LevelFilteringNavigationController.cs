// Decompiled with JetBrains decompiler
// Type: LevelFilteringNavigationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Zenject;

public class LevelFilteringNavigationController : NavigationController
{
  [SerializeField]
  protected GameObject _emptyFavoritesListInfoPrefab;
  [SerializeField]
  protected GameObject _emptyCustomSongListInfoPrefab;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly SelectLevelCategoryViewController _selectLevelCategoryViewController;
  [Inject]
  protected readonly AnnotatedBeatmapLevelCollectionsViewController _annotatedBeatmapLevelCollectionsViewController;
  [Inject]
  protected readonly LevelSearchViewController _levelSearchViewController;
  [Inject]
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;
  protected CancellationTokenSource _cancellationTokenSource;
  protected GameObject _currentNoDataInfoPrefab;
  protected string _levelPackIdToBeSelectedAfterPresent;
  protected bool _hidePacksIfOneOrNone;
  protected bool _enableCustomLevels;
  protected SongPackMask _songPackMask;
  protected SelectLevelCategoryViewController.LevelCategory[] _enabledLevelCategories;
  protected IBeatmapLevelPack[] _ostBeatmapLevelPacks;
  protected IBeatmapLevelPack[] _musicPacksBeatmapLevelPacks;
  protected IBeatmapLevelPack[] _customLevelPacks;
  protected IBeatmapLevelPack[] _allOfficialBeatmapLevelPacks;
  protected IBeatmapLevelPack[] _allBeatmapLevelPacks;

  public event Action<LevelFilteringNavigationController, IAnnotatedBeatmapLevelCollection, GameObject, BeatmapCharacteristicSO> didSelectAnnotatedBeatmapLevelCollectionEvent;

  public event System.Action<LevelFilteringNavigationController> didStartLoadingEvent;

  public event System.Action<LevelFilteringNavigationController> didPressAllSongsEvent;

  public event System.Action didOpenBeatmapLevelCollectionsEvent;

  public event System.Action didCloseBeatmapLevelCollectionsEvent;

  public IBeatmapLevelPack selectedBeatmapLevelPack => this._annotatedBeatmapLevelCollectionsViewController.selectedAnnotatedBeatmapLevelCollection as IBeatmapLevelPack;

  public SelectLevelCategoryViewController.LevelCategory selectedLevelCategory => this._selectLevelCategoryViewController.selectedLevelCategory;

  public virtual void Setup(
    SongPackMask songPackMask,
    IBeatmapLevelPack levelPackToBeSelectedAfterPresent,
    SelectLevelCategoryViewController.LevelCategory startLevelCategory,
    bool hidePacksIfOneOrNone,
    bool enableCustomLevels)
  {
    this._hidePacksIfOneOrNone = hidePacksIfOneOrNone;
    this._enableCustomLevels = enableCustomLevels;
    this._songPackMask = songPackMask;
    this._levelPackIdToBeSelectedAfterPresent = levelPackToBeSelectedAfterPresent?.packID;
    this.SetupBeatmapLevelPacks();
    if (this._levelPackIdToBeSelectedAfterPresent != null)
    {
      if (startLevelCategory == SelectLevelCategoryViewController.LevelCategory.None)
        startLevelCategory = this._enabledLevelCategories[0];
      this._selectLevelCategoryViewController.Setup(startLevelCategory, this._enabledLevelCategories);
    }
    else
      this._selectLevelCategoryViewController.Setup(this._enabledLevelCategories[0], this._enabledLevelCategories);
  }

  public virtual void SetupBeatmapLevelPacks()
  {
    this._allBeatmapLevelPacks = (IBeatmapLevelPack[]) null;
    this._customLevelPacks = (IBeatmapLevelPack[]) null;
    List<SelectLevelCategoryViewController.LevelCategory> levelCategoryList = new List<SelectLevelCategoryViewController.LevelCategory>()
    {
      SelectLevelCategoryViewController.LevelCategory.MusicPacks
    };
    // ISSUE: explicit non-virtual call
    this._ostBeatmapLevelPacks = ((IEnumerable<IBeatmapLevelPack>) this._beatmapLevelsModel.ostAndExtrasPackCollection.beatmapLevelPacks).Where<IBeatmapLevelPack>((Func<IBeatmapLevelPack, bool>) (pack => this._songPackMask.Contains((SongPackMask) pack.packID))).ToArray<IBeatmapLevelPack>();
    // ISSUE: explicit non-virtual call
    this._musicPacksBeatmapLevelPacks = ((IEnumerable<IBeatmapLevelPack>) this._beatmapLevelsModel.dlcBeatmapLevelPackCollection.beatmapLevelPacks).Reverse<IBeatmapLevelPack>().Where<IBeatmapLevelPack>((Func<IBeatmapLevelPack, bool>) (pack => this._songPackMask.Contains((SongPackMask)pack.packID))).ToArray<IBeatmapLevelPack>();
    this._allOfficialBeatmapLevelPacks = ((IEnumerable<IBeatmapLevelPack>) this._ostBeatmapLevelPacks).Concat<IBeatmapLevelPack>((IEnumerable<IBeatmapLevelPack>) this._musicPacksBeatmapLevelPacks).ToArray<IBeatmapLevelPack>();
    if (this._enableCustomLevels)
    {
      levelCategoryList.Add(SelectLevelCategoryViewController.LevelCategory.CustomSongs);
    }
    else
    {
      this._allBeatmapLevelPacks = ((IEnumerable<IBeatmapLevelPack>) this._ostBeatmapLevelPacks).Concat<IBeatmapLevelPack>((IEnumerable<IBeatmapLevelPack>) this._musicPacksBeatmapLevelPacks).ToArray<IBeatmapLevelPack>();
      this._levelSearchViewController.Setup(this._allBeatmapLevelPacks);
    }
    levelCategoryList.Add(SelectLevelCategoryViewController.LevelCategory.Favorites);
    levelCategoryList.Add(SelectLevelCategoryViewController.LevelCategory.All);
    this._enabledLevelCategories = levelCategoryList.ToArray();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    if (!addedToHierarchy)
      return;
    this._annotatedBeatmapLevelCollectionsViewController.didOpenBeatmapLevelCollectionsEvent += new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsViewControllerDidOpenBeatmapLevelCollections);
    this._annotatedBeatmapLevelCollectionsViewController.didCloseBeatmapLevelCollectionsEvent += new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsViewControllerDidCloseBeatmapLevelCollections);
    this._annotatedBeatmapLevelCollectionsViewController.didSelectAnnotatedBeatmapLevelCollectionEvent += new System.Action<IAnnotatedBeatmapLevelCollection>(this.HandleAnnotatedBeatmapLevelCollectionsViewControllerDidSelectAnnotatedBeatmapLevelCollection);
    this._playerDataModel.playerData.favoriteLevelsSetDidChangeEvent += new System.Action(this.HandlePlayerDataFavoriteLevelsSetDidChange);
    this._playerDataModel.playerData.didIncreaseNumberOfGameplaysEvent += new System.Action(this.HandleIncreaseNumberOfGameplays);
    this._selectLevelCategoryViewController.didSelectLevelCategoryEvent += new System.Action<SelectLevelCategoryViewController, SelectLevelCategoryViewController.LevelCategory>(this.SelectLevelCategoryViewControllerDidSelectLevelCategory);
    this._levelSearchViewController.didFilterBeatmapLevelCollectionEvent += new System.Action<IAnnotatedBeatmapLevelCollection, BeatmapCharacteristicSO>(this.LevelSearchViewControllerDidFilterBeatmapLevelCollection);
    this._levelSearchViewController.didStartLoadingEvent += new System.Action<LevelSearchViewController>(this.LevelSearchViewControllerDidStartLoading);
    this.SetChildViewControllers((ViewController) this._selectLevelCategoryViewController, (ViewController) this._annotatedBeatmapLevelCollectionsViewController);
    this.UpdateSecondChildControllerContent(this._selectLevelCategoryViewController.selectedLevelCategory);
  }

  public virtual void LevelSearchViewControllerDidStartLoading(LevelSearchViewController obj)
  {
    System.Action<LevelFilteringNavigationController> startLoadingEvent = this.didStartLoadingEvent;
    if (startLoadingEvent == null)
      return;
    startLoadingEvent(this);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
    if (!removedFromHierarchy)
      return;
    this._annotatedBeatmapLevelCollectionsViewController.didOpenBeatmapLevelCollectionsEvent -= new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsViewControllerDidOpenBeatmapLevelCollections);
    this._annotatedBeatmapLevelCollectionsViewController.didCloseBeatmapLevelCollectionsEvent -= new System.Action(this.HandleAnnotatedBeatmapLevelCollectionsViewControllerDidCloseBeatmapLevelCollections);
    this._annotatedBeatmapLevelCollectionsViewController.didSelectAnnotatedBeatmapLevelCollectionEvent -= new System.Action<IAnnotatedBeatmapLevelCollection>(this.HandleAnnotatedBeatmapLevelCollectionsViewControllerDidSelectAnnotatedBeatmapLevelCollection);
    this._playerDataModel.playerData.favoriteLevelsSetDidChangeEvent -= new System.Action(this.HandlePlayerDataFavoriteLevelsSetDidChange);
    this._playerDataModel.playerData.didIncreaseNumberOfGameplaysEvent -= new System.Action(this.HandlePlayerDataFavoriteLevelsSetDidChange);
    this._selectLevelCategoryViewController.didSelectLevelCategoryEvent -= new System.Action<SelectLevelCategoryViewController, SelectLevelCategoryViewController.LevelCategory>(this.SelectLevelCategoryViewControllerDidSelectLevelCategory);
    this._levelSearchViewController.didFilterBeatmapLevelCollectionEvent -= new System.Action<IAnnotatedBeatmapLevelCollection, BeatmapCharacteristicSO>(this.LevelSearchViewControllerDidFilterBeatmapLevelCollection);
    this._levelSearchViewController.didStartLoadingEvent -= new System.Action<LevelSearchViewController>(this.LevelSearchViewControllerDidStartLoading);
    this._cancellationTokenSource?.Cancel();
    this._cancellationTokenSource?.Dispose();
    this._cancellationTokenSource = (CancellationTokenSource) null;
    this.ClearChildViewControllers();
  }

  public virtual void LevelSearchViewControllerDidFilterBeatmapLevelCollection(
    IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection,
    BeatmapCharacteristicSO preferredBeatmapCharacteristic)
  {
    Action<LevelFilteringNavigationController, IAnnotatedBeatmapLevelCollection, GameObject, BeatmapCharacteristicSO> levelCollectionEvent = this.didSelectAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent == null)
      return;
    levelCollectionEvent(this, annotatedBeatmapLevelCollection, this._currentNoDataInfoPrefab, preferredBeatmapCharacteristic);
  }

  public virtual void SelectLevelCategoryViewControllerDidSelectLevelCategory(
    SelectLevelCategoryViewController viewController,
    SelectLevelCategoryViewController.LevelCategory levelCategory)
  {
    this.UpdateSecondChildControllerContent(levelCategory);
  }

  public virtual void UpdateSecondChildControllerContent(
    SelectLevelCategoryViewController.LevelCategory levelCategory)
  {
    this._currentNoDataInfoPrefab = (GameObject) null;
    switch (levelCategory)
    {
      case SelectLevelCategoryViewController.LevelCategory.MusicPacks:
        this.ShowPacksInSecondChildController((IReadOnlyList<IBeatmapLevelPack>) this._allOfficialBeatmapLevelPacks);
        return;
      case SelectLevelCategoryViewController.LevelCategory.CustomSongs:
        this._currentNoDataInfoPrefab = this._emptyCustomSongListInfoPrefab;
        if (this._customLevelPacks != null)
        {
          this.ShowPacksInSecondChildController((IReadOnlyList<IBeatmapLevelPack>) this._customLevelPacks);
          return;
        }
        break;
      case SelectLevelCategoryViewController.LevelCategory.Favorites:
        this._currentNoDataInfoPrefab = this._emptyFavoritesListInfoPrefab;
        if (this._allBeatmapLevelPacks != null)
        {
          this._levelSearchViewController.ResetFilterParams(true);
          this.ReplaceSecondViewController((ViewController) this._levelSearchViewController);
          return;
        }
        break;
      case SelectLevelCategoryViewController.LevelCategory.All:
        if (this._allBeatmapLevelPacks != null)
        {
          System.Action<LevelFilteringNavigationController> pressAllSongsEvent = this.didPressAllSongsEvent;
          if (pressAllSongsEvent != null)
            pressAllSongsEvent(this);
          this._levelSearchViewController.ResetFilterParams(false);
          this.ReplaceSecondViewController((ViewController) this._levelSearchViewController);
          return;
        }
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (levelCategory), (object) levelCategory, (string) null);
    }
    this._annotatedBeatmapLevelCollectionsViewController.ShowLoading();
    System.Action<LevelFilteringNavigationController> startLoadingEvent = this.didStartLoadingEvent;
    if (startLoadingEvent != null)
      startLoadingEvent(this);
    this.UpdateCustomSongs();
  }

  public virtual void ShowPacksInSecondChildController(
    IReadOnlyList<IBeatmapLevelPack> beatmapLevelPacks)
  {
    int selectedItemIndex = 0;
    for (int index = 0; index < beatmapLevelPacks.Count; ++index)
    {
      if (!(beatmapLevelPacks[index].packID != this._levelPackIdToBeSelectedAfterPresent))
      {
        selectedItemIndex = index;
        break;
      }
    }
    this._annotatedBeatmapLevelCollectionsViewController.SetData((IReadOnlyList<IAnnotatedBeatmapLevelCollection>) beatmapLevelPacks, selectedItemIndex, this._hidePacksIfOneOrNone);
    this.ReplaceSecondViewController((ViewController) this._annotatedBeatmapLevelCollectionsViewController);
    Action<LevelFilteringNavigationController, IAnnotatedBeatmapLevelCollection, GameObject, BeatmapCharacteristicSO> levelCollectionEvent = this.didSelectAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent != null)
      levelCollectionEvent(this, this._annotatedBeatmapLevelCollectionsViewController.selectedAnnotatedBeatmapLevelCollection, this._currentNoDataInfoPrefab, (BeatmapCharacteristicSO) null);
    this._levelPackIdToBeSelectedAfterPresent = (string) null;
  }

  public virtual void ReplaceSecondViewController(ViewController viewController)
  {
    if (viewController.isInViewControllerHierarchy)
      return;
    if (this.viewControllers.Count > 1)
      this.PopViewControllers(this.viewControllers.Count - 1, (System.Action) null, true);
    this.PushViewController(viewController, (System.Action) null, true);
  }

  public virtual void HandlePlayerDataFavoriteLevelsSetDidChange()
  {
    if (this._selectLevelCategoryViewController.selectedLevelCategory != SelectLevelCategoryViewController.LevelCategory.Favorites)
      return;
    this.UpdateSecondChildControllerContent(this._selectLevelCategoryViewController.selectedLevelCategory);
  }

  public virtual void HandleIncreaseNumberOfGameplays()
  {
    if (!this._levelSearchViewController.isActiveAndEnabled)
      return;
    this._levelSearchViewController.RefreshAfterIncreaseNumberOfGameplay();
  }

  public virtual void HandleAnnotatedBeatmapLevelCollectionsViewControllerDidOpenBeatmapLevelCollections()
  {
    System.Action collectionsEvent = this.didOpenBeatmapLevelCollectionsEvent;
    if (collectionsEvent == null)
      return;
    collectionsEvent();
  }

  public virtual void HandleAnnotatedBeatmapLevelCollectionsViewControllerDidCloseBeatmapLevelCollections()
  {
    System.Action collectionsEvent = this.didCloseBeatmapLevelCollectionsEvent;
    if (collectionsEvent == null)
      return;
    collectionsEvent();
  }

  public virtual void HandleAnnotatedBeatmapLevelCollectionsViewControllerDidSelectAnnotatedBeatmapLevelCollection(
    IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection)
  {
    Action<LevelFilteringNavigationController, IAnnotatedBeatmapLevelCollection, GameObject, BeatmapCharacteristicSO> levelCollectionEvent = this.didSelectAnnotatedBeatmapLevelCollectionEvent;
    if (levelCollectionEvent == null)
      return;
    levelCollectionEvent(this, annotatedBeatmapLevelCollection, this._currentNoDataInfoPrefab, (BeatmapCharacteristicSO) null);
  }

  public virtual void SelectAnnotatedBeatmapLevelCollection(IBeatmapLevelPack levelPack)
  {
    if (((IEnumerable<IBeatmapLevelPack>) this._allBeatmapLevelPacks).Select<IBeatmapLevelPack, string>((Func<IBeatmapLevelPack, string>) (pack => pack.packID)).Any<string>((Func<string, bool>) (packID => packID == levelPack.packID)))
      this._selectLevelCategoryViewController.Setup(SelectLevelCategoryViewController.LevelCategory.MusicPacks, this._enabledLevelCategories);
    this._levelPackIdToBeSelectedAfterPresent = levelPack.packID;
    this.UpdateSecondChildControllerContent(this._selectLevelCategoryViewController.selectedLevelCategory);
  }

  public virtual async void UpdateCustomSongs()
  {
    if (this._cancellationTokenSource != null)
      return;
    this._cancellationTokenSource = new CancellationTokenSource();
    CancellationToken cancellationToken = this._cancellationTokenSource.Token;
    try
    {
      IBeatmapLevelPackCollection levelPackCollection = await this._beatmapLevelsModel.ReloadCustomLevelPackCollectionAsync(cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      this._customLevelPacks = levelPackCollection?.beatmapLevelPacks ?? new IBeatmapLevelPack[0];
      this._allBeatmapLevelPacks = ((IEnumerable<IBeatmapLevelPack>) this._ostBeatmapLevelPacks).Concat<IBeatmapLevelPack>((IEnumerable<IBeatmapLevelPack>) this._musicPacksBeatmapLevelPacks).Concat<IBeatmapLevelPack>((IEnumerable<IBeatmapLevelPack>) this._customLevelPacks).ToArray<IBeatmapLevelPack>();
      this._levelSearchViewController.Setup(this._allBeatmapLevelPacks);
      this.UpdateSecondChildControllerContent(this._selectLevelCategoryViewController.selectedLevelCategory);
    }
    catch (OperationCanceledException)
    {
    }
    finally
    {
      this._cancellationTokenSource?.Dispose();
      this._cancellationTokenSource = (CancellationTokenSource) null;
    }
  }

  [CompilerGenerated]
  public virtual bool m_CSetupBeatmapLevelPacksm_Eb__39_0(IBeatmapLevelPack pack) => this._songPackMask.Contains((SongPackMask)pack.packID);

  [CompilerGenerated]
  public virtual bool m_CSetupBeatmapLevelPacksm_Eb__39_1(IBeatmapLevelPack pack) => this._songPackMask.Contains((SongPackMask)pack.packID);
}
