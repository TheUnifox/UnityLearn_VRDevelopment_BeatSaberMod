// Decompiled with JetBrains decompiler
// Type: LevelSelectionNavigationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using UnityEngine;
using Zenject;

public class LevelSelectionNavigationController : NavigationController
{
  [Inject]
  protected readonly LevelFilteringNavigationController _levelFilteringNavigationController;
  [Inject]
  protected readonly LevelCollectionNavigationController _levelCollectionNavigationController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  protected bool _hidePacksIfOneOrNone;
  protected bool _hidePracticeButton;
  protected string _actionButtonText;
  protected BeatmapDifficultyMask _allowedBeatmapDifficultyMask;
  protected BeatmapCharacteristicSO[] _notAllowedCharacteristics;

  public event System.Action<LevelSelectionNavigationController, StandardLevelDetailViewController.ContentType> didChangeLevelDetailContentEvent;

  public event System.Action<LevelSelectionNavigationController, IBeatmapLevelPack> didSelectLevelPackEvent;

  public event System.Action<LevelSelectionNavigationController> didPressActionButtonEvent;

  public event System.Action<LevelSelectionNavigationController, IBeatmapLevel> didPressPracticeButtonEvent;

  public event System.Action<LevelSelectionNavigationController, IDifficultyBeatmap> didChangeDifficultyBeatmapEvent;

  public IDifficultyBeatmap selectedDifficultyBeatmap => this._levelCollectionNavigationController.selectedDifficultyBeatmap;

  public IPreviewBeatmapLevel selectedBeatmapLevel => this._levelCollectionNavigationController.selectedBeatmapLevel;

  public SelectLevelCategoryViewController.LevelCategory selectedLevelCategory => this._levelFilteringNavigationController.selectedLevelCategory;

  public IBeatmapLevelPack selectedBeatmapLevelPack => this._levelFilteringNavigationController.selectedBeatmapLevelPack;

  public virtual void Setup(
    SongPackMask songPackMask,
    BeatmapDifficultyMask allowedBeatmapDifficultyMask,
    BeatmapCharacteristicSO[] notAllowedCharacteristics,
    bool hidePacksIfOneOrNone,
    bool hidePracticeButton,
    string actionButtonText,
    IBeatmapLevelPack levelPackToBeSelectedAfterPresent,
    SelectLevelCategoryViewController.LevelCategory startLevelCategory,
    IPreviewBeatmapLevel beatmapLevelToBeSelectedAfterPresent,
    bool enableCustomLevels)
  {
    this._allowedBeatmapDifficultyMask = allowedBeatmapDifficultyMask;
    this._notAllowedCharacteristics = notAllowedCharacteristics;
    this._hidePacksIfOneOrNone = hidePacksIfOneOrNone;
    this._hidePracticeButton = hidePracticeButton;
    this._actionButtonText = actionButtonText;
    this._levelFilteringNavigationController.Setup(songPackMask, levelPackToBeSelectedAfterPresent, startLevelCategory, this._hidePacksIfOneOrNone, enableCustomLevels);
    this._levelCollectionNavigationController.SelectLevel(beatmapLevelToBeSelectedAfterPresent);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    if (!addedToHierarchy)
      return;
    this._levelFilteringNavigationController.didOpenBeatmapLevelCollectionsEvent += new System.Action(this.HandleLevelFilteringNavigationControllerDidOpenBeatmapLevelCollections);
    this._levelFilteringNavigationController.didCloseBeatmapLevelCollectionsEvent += new System.Action(this.HandleLevelFilteringNavigationControllerDidCloseBeatmapLevelCollections);
    this._levelFilteringNavigationController.didSelectAnnotatedBeatmapLevelCollectionEvent += new Action<LevelFilteringNavigationController, IAnnotatedBeatmapLevelCollection, GameObject, BeatmapCharacteristicSO>(this.LevelFilteringNavigationControllerDidSelectAnnotatedBeatmapLevelCollection);
    this._levelFilteringNavigationController.didStartLoadingEvent += new System.Action<LevelFilteringNavigationController>(this.LevelFilteringNavigationControllerDidStartLoading);
    this._levelFilteringNavigationController.didPressAllSongsEvent += new System.Action<LevelFilteringNavigationController>(this.LevelFilteringNavigationControllerDidPressAllSongs);
    this._levelCollectionNavigationController.didChangeDifficultyBeatmapEvent += new System.Action<LevelCollectionNavigationController, IDifficultyBeatmap>(this.HandleLevelCollectionNavigationControllerDidChangeDifficultyBeatmap);
    this._levelCollectionNavigationController.didSelectLevelPackEvent += new System.Action<LevelCollectionNavigationController, IBeatmapLevelPack>(this.HandleLevelCollectionNavigationControllerDidSelectPack);
    this._levelCollectionNavigationController.didPressActionButtonEvent += new System.Action<LevelCollectionNavigationController>(this.HandleLevelCollectionNavigationControllerDidPressActionButton);
    this._levelCollectionNavigationController.didPressPracticeButtonEvent += new System.Action<LevelCollectionNavigationController, IBeatmapLevel>(this.HandleLevelCollectionNavigationControllerDidPressPracticeButton);
    this._levelCollectionNavigationController.didChangeLevelDetailContentEvent += new System.Action<LevelCollectionNavigationController, StandardLevelDetailViewController.ContentType>(this.HandleLevelCollectionNavigationControllerDidChangeLevelDetailContent);
    this._levelCollectionNavigationController.didPressOpenPackButtonEvent += new System.Action<LevelCollectionNavigationController, IBeatmapLevelPack>(this.HandleLevelCollectionNavigationControllerDidPressOpenPackButton);
    this.SetChildViewControllers((ViewController) this._levelFilteringNavigationController, (ViewController) this._levelCollectionNavigationController);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
    if (!removedFromHierarchy)
      return;
    this._levelFilteringNavigationController.didOpenBeatmapLevelCollectionsEvent -= new System.Action(this.HandleLevelFilteringNavigationControllerDidOpenBeatmapLevelCollections);
    this._levelFilteringNavigationController.didCloseBeatmapLevelCollectionsEvent -= new System.Action(this.HandleLevelFilteringNavigationControllerDidCloseBeatmapLevelCollections);
    this._levelFilteringNavigationController.didSelectAnnotatedBeatmapLevelCollectionEvent -= new Action<LevelFilteringNavigationController, IAnnotatedBeatmapLevelCollection, GameObject, BeatmapCharacteristicSO>(this.LevelFilteringNavigationControllerDidSelectAnnotatedBeatmapLevelCollection);
    this._levelFilteringNavigationController.didStartLoadingEvent -= new System.Action<LevelFilteringNavigationController>(this.LevelFilteringNavigationControllerDidStartLoading);
    this._levelFilteringNavigationController.didPressAllSongsEvent -= new System.Action<LevelFilteringNavigationController>(this.LevelFilteringNavigationControllerDidPressAllSongs);
    this._levelCollectionNavigationController.didChangeDifficultyBeatmapEvent -= new System.Action<LevelCollectionNavigationController, IDifficultyBeatmap>(this.HandleLevelCollectionNavigationControllerDidChangeDifficultyBeatmap);
    this._levelCollectionNavigationController.didSelectLevelPackEvent -= new System.Action<LevelCollectionNavigationController, IBeatmapLevelPack>(this.HandleLevelCollectionNavigationControllerDidSelectPack);
    this._levelCollectionNavigationController.didPressActionButtonEvent -= new System.Action<LevelCollectionNavigationController>(this.HandleLevelCollectionNavigationControllerDidPressActionButton);
    this._levelCollectionNavigationController.didPressPracticeButtonEvent -= new System.Action<LevelCollectionNavigationController, IBeatmapLevel>(this.HandleLevelCollectionNavigationControllerDidPressPracticeButton);
    this._levelCollectionNavigationController.didChangeLevelDetailContentEvent -= new System.Action<LevelCollectionNavigationController, StandardLevelDetailViewController.ContentType>(this.HandleLevelCollectionNavigationControllerDidChangeLevelDetailContent);
    this._levelCollectionNavigationController.didPressOpenPackButtonEvent -= new System.Action<LevelCollectionNavigationController, IBeatmapLevelPack>(this.HandleLevelCollectionNavigationControllerDidPressOpenPackButton);
    this.ClearChildViewControllers();
  }

  public virtual void HandleLevelCollectionNavigationControllerDidChangeLevelDetailContent(
    LevelCollectionNavigationController viewController,
    StandardLevelDetailViewController.ContentType contentType)
  {
    System.Action<LevelSelectionNavigationController, StandardLevelDetailViewController.ContentType> detailContentEvent = this.didChangeLevelDetailContentEvent;
    if (detailContentEvent == null)
      return;
    detailContentEvent(this, contentType);
  }

  public virtual void HandleLevelCollectionNavigationControllerDidPressPracticeButton(
    LevelCollectionNavigationController arg1,
    IBeatmapLevel beatmapLevel)
  {
    System.Action<LevelSelectionNavigationController, IBeatmapLevel> practiceButtonEvent = this.didPressPracticeButtonEvent;
    if (practiceButtonEvent == null)
      return;
    practiceButtonEvent(this, beatmapLevel);
  }

  public virtual void HandleLevelCollectionNavigationControllerDidPressActionButton(
    LevelCollectionNavigationController viewController)
  {
    System.Action<LevelSelectionNavigationController> actionButtonEvent = this.didPressActionButtonEvent;
    if (actionButtonEvent == null)
      return;
    actionButtonEvent(this);
  }

  public virtual void HandleLevelCollectionNavigationControllerDidSelectPack(
    LevelCollectionNavigationController viewController,
    IBeatmapLevelPack beatmapLevelPack)
  {
    System.Action<LevelSelectionNavigationController, IBeatmapLevelPack> selectLevelPackEvent = this.didSelectLevelPackEvent;
    if (selectLevelPackEvent == null)
      return;
    selectLevelPackEvent(this, beatmapLevelPack);
  }

  public virtual void HandleLevelCollectionNavigationControllerDidChangeDifficultyBeatmap(
    LevelCollectionNavigationController viewController,
    IDifficultyBeatmap difficultyBeatmap)
  {
    System.Action<LevelSelectionNavigationController, IDifficultyBeatmap> difficultyBeatmapEvent = this.didChangeDifficultyBeatmapEvent;
    if (difficultyBeatmapEvent == null)
      return;
    difficultyBeatmapEvent(this, difficultyBeatmap);
  }

  public virtual void HandleLevelCollectionNavigationControllerDidPressOpenPackButton(
    LevelCollectionNavigationController viewController,
    IBeatmapLevelPack beatmapLevelPack)
  {
    this._levelFilteringNavigationController.SelectAnnotatedBeatmapLevelCollection(beatmapLevelPack);
  }

  public virtual void RefreshDetail() => this._levelCollectionNavigationController.RefreshDetail();

  public virtual void ClearSelected() => this._levelCollectionNavigationController.ClearSelected();

  public virtual void HandleLevelFilteringNavigationControllerDidOpenBeatmapLevelCollections() => this._levelCollectionNavigationController.AnimateCanvasGroupAlpha(LevelCollectionNavigationController.AlphaAnimationType.Out);

  public virtual void HandleLevelFilteringNavigationControllerDidCloseBeatmapLevelCollections() => this._levelCollectionNavigationController.AnimateCanvasGroupAlpha(LevelCollectionNavigationController.AlphaAnimationType.In);

  public virtual void LevelFilteringNavigationControllerDidSelectAnnotatedBeatmapLevelCollection(
    LevelFilteringNavigationController controller,
    IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection,
    GameObject noDataInfoPrefab,
    BeatmapCharacteristicSO preferredBeatmapCharacteristic)
  {
    if ((UnityEngine.Object) preferredBeatmapCharacteristic != (UnityEngine.Object) null)
      this._playerDataModel.playerData.SetLastSelectedBeatmapCharacteristic(preferredBeatmapCharacteristic);
    this._levelCollectionNavigationController.SetData(annotatedBeatmapLevelCollection, true, !this._hidePracticeButton, this._actionButtonText, noDataInfoPrefab, this._allowedBeatmapDifficultyMask, this._notAllowedCharacteristics);
  }

  public virtual void LevelFilteringNavigationControllerDidStartLoading(
    LevelFilteringNavigationController controller)
  {
    this._levelCollectionNavigationController.ShowLoading();
  }

  public virtual void LevelFilteringNavigationControllerDidPressAllSongs(
    LevelFilteringNavigationController controller)
  {
    this.ClearSelected();
  }
}
