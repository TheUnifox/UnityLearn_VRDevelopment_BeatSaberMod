// Decompiled with JetBrains decompiler
// Type: LevelSelectionFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using Zenject;

public abstract class LevelSelectionFlowCoordinator : FlowCoordinator
{
  [Inject]
  protected readonly PlayerDataModel playerDataModel;
  [Inject]
  protected readonly LevelSelectionNavigationController levelSelectionNavigationController;
  [Inject]
  private readonly SearchFilterParamsViewController _searchFilterParamsViewController;
  [Inject]
  private readonly LevelSearchViewController _levelSearchViewController;
  private LevelSelectionFlowCoordinator.State _startState;

  protected bool isInRootViewController => (UnityEngine.Object) this.topViewController == (UnityEngine.Object) this.levelSelectionNavigationController;

  protected SelectLevelCategoryViewController.LevelCategory selectedLevelCategory => this.levelSelectionNavigationController.selectedLevelCategory;

  protected IBeatmapLevelPack selectedBeatmapLevelPack => this.levelSelectionNavigationController.selectedBeatmapLevelPack;

  protected IDifficultyBeatmap selectedDifficultyBeatmap => this.levelSelectionNavigationController.selectedDifficultyBeatmap;

  protected IPreviewBeatmapLevel selectedBeatmapLevel => this.levelSelectionNavigationController.selectedBeatmapLevel;

  protected virtual ViewController initialTopScreenViewController => (ViewController) null;

  protected virtual ViewController initialLeftScreenViewController => (ViewController) null;

  protected virtual ViewController initialRightScreenViewController => (ViewController) null;

  protected virtual bool showBackButtonForMainViewController => false;

  protected virtual bool hidePacksIfOneOrNone => false;

  protected virtual bool hidePracticeButton => false;

  protected virtual string actionButtonText => Localization.Get("BUTTON_PLAY");

  protected virtual string mainTitle => (string) null;

  protected virtual bool enableCustomLevels => false;

  protected virtual SongPackMask songPackMask => SongPackMask.all;

  protected virtual BeatmapDifficultyMask allowedBeatmapDifficultyMask => BeatmapDifficultyMask.All;

  protected virtual BeatmapCharacteristicSO[] notAllowedCharacteristics => new BeatmapCharacteristicSO[0];

  protected virtual void LevelSelectionFlowCoordinatorDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
  }

  protected virtual void LevelSelectionFlowCoordinatorDidDeactivate(bool removedFromHierarchy)
  {
  }

  protected virtual void LevelSelectionFlowCoordinatorTopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._searchFilterParamsViewController)
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
      this.SetBottomScreenViewController((ViewController) null, animationType);
      this.showBackButton = false;
      this.SetTitle(Localization.Get("TITLE_FILTER_PARAMETERS"));
    }
    else
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
      this.SetBottomScreenViewController((ViewController) null, animationType);
      this.showBackButton = false;
    }
  }

  protected virtual void ActionButtonWasPressed()
  {
  }

  protected virtual void PracticeButtonWasPressed()
  {
  }

  protected virtual void SelectionDidChange(IBeatmapLevelPack pack, IDifficultyBeatmap beatmap)
  {
  }

  public void Setup(LevelSelectionFlowCoordinator.State state) => this._startState = state;

  protected override sealed void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this.levelSelectionNavigationController.didChangeDifficultyBeatmapEvent += new System.Action<LevelSelectionNavigationController, IDifficultyBeatmap>(this.HandleLevelSelectionNavigationControllerDidChangeDifficultyBeatmap);
      this.levelSelectionNavigationController.didSelectLevelPackEvent += new System.Action<LevelSelectionNavigationController, IBeatmapLevelPack>(this.HandleLevelSelectionNavigationControllerDidSelectPack);
      this.levelSelectionNavigationController.didPressActionButtonEvent += new System.Action<LevelSelectionNavigationController>(this.HandleLevelSelectionNavigationControllerDidPressActionButton);
      this.levelSelectionNavigationController.didPressPracticeButtonEvent += new System.Action<LevelSelectionNavigationController, IBeatmapLevel>(this.HandleLevelSelectionNavigationControllerDidPressPracticeButton);
      this.levelSelectionNavigationController.didChangeLevelDetailContentEvent += new System.Action<LevelSelectionNavigationController, StandardLevelDetailViewController.ContentType>(this.HandleLevelSelectionNavigationControllerDidChangeLevelDetailContent);
      this._searchFilterParamsViewController.didFinishEvent += new System.Action<SearchFilterParamsViewController, LevelFilterParams>(this.HandleSearchFilterParamsViewControllerDidFinish);
      this._levelSearchViewController.didPressSearchButtonEvent += new System.Action<LevelSearchViewController, LevelFilterParams>(this.HandleLevelSearchViewControllerDidPressSearchButton);
      LevelSelectionNavigationController navigationController = this.levelSelectionNavigationController;
      SongPackMask songPackMask = this.songPackMask;
      int beatmapDifficultyMask = (int) this.allowedBeatmapDifficultyMask;
      BeatmapCharacteristicSO[] allowedCharacteristics = this.notAllowedCharacteristics;
      int num1 = this.hidePacksIfOneOrNone ? 1 : 0;
      int num2 = this.hidePracticeButton ? 1 : 0;
      string actionButtonText = this.actionButtonText;
      IBeatmapLevelPack beatmapLevelPack = this._startState?.beatmapLevelPack;
      IPreviewBeatmapLevel previewBeatmapLevel = this._startState?.previewBeatmapLevel;
      int startLevelCategory = (int)(this._startState?.levelCategory ?? 0);
      IPreviewBeatmapLevel beatmapLevelToBeSelectedAfterPresent = previewBeatmapLevel;
      int num3 = this.enableCustomLevels ? 1 : 0;
      navigationController.Setup(songPackMask, (BeatmapDifficultyMask) beatmapDifficultyMask, allowedCharacteristics, num1 != 0, num2 != 0, actionButtonText, beatmapLevelPack, (SelectLevelCategoryViewController.LevelCategory) startLevelCategory, beatmapLevelToBeSelectedAfterPresent, num3 != 0);
      this._startState = (LevelSelectionFlowCoordinator.State) null;
      this.ProvideInitialViewControllers((ViewController) this.levelSelectionNavigationController, this.initialLeftScreenViewController, this.initialRightScreenViewController, topScreenViewController: this.initialTopScreenViewController);
      this.showBackButton = this.showBackButtonForMainViewController;
      this.SetTitle(this.mainTitle);
    }
    this.LevelSelectionFlowCoordinatorDidActivate(firstActivation, addedToHierarchy);
  }

  protected override sealed void DidDeactivate(
    bool removedFromHierarchy,
    bool screenSystemDisabling)
  {
    if (removedFromHierarchy)
    {
      this.levelSelectionNavigationController.didChangeDifficultyBeatmapEvent -= new System.Action<LevelSelectionNavigationController, IDifficultyBeatmap>(this.HandleLevelSelectionNavigationControllerDidChangeDifficultyBeatmap);
      this.levelSelectionNavigationController.didSelectLevelPackEvent -= new System.Action<LevelSelectionNavigationController, IBeatmapLevelPack>(this.HandleLevelSelectionNavigationControllerDidSelectPack);
      this.levelSelectionNavigationController.didPressActionButtonEvent -= new System.Action<LevelSelectionNavigationController>(this.HandleLevelSelectionNavigationControllerDidPressActionButton);
      this.levelSelectionNavigationController.didPressPracticeButtonEvent -= new System.Action<LevelSelectionNavigationController, IBeatmapLevel>(this.HandleLevelSelectionNavigationControllerDidPressPracticeButton);
      this.levelSelectionNavigationController.didChangeLevelDetailContentEvent -= new System.Action<LevelSelectionNavigationController, StandardLevelDetailViewController.ContentType>(this.HandleLevelSelectionNavigationControllerDidChangeLevelDetailContent);
      this._searchFilterParamsViewController.didFinishEvent -= new System.Action<SearchFilterParamsViewController, LevelFilterParams>(this.HandleSearchFilterParamsViewControllerDidFinish);
      this._levelSearchViewController.didPressSearchButtonEvent -= new System.Action<LevelSearchViewController, LevelFilterParams>(this.HandleLevelSearchViewControllerDidPressSearchButton);
    }
    this.LevelSelectionFlowCoordinatorDidDeactivate(removedFromHierarchy);
  }

  protected override sealed void TopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    this.LevelSelectionFlowCoordinatorTopViewControllerWillChange(oldViewController, newViewController, animationType);
  }

  private void HandleLevelSelectionNavigationControllerDidSelectPack(
    LevelSelectionNavigationController viewController,
    IBeatmapLevelPack pack)
  {
    this.SelectionDidChange(pack, (IDifficultyBeatmap) null);
  }

  private void HandleSearchFilterParamsViewControllerDidFinish(
    SearchFilterParamsViewController viewController,
    LevelFilterParams levelFilterParams)
  {
    this.DismissViewController((ViewController) this._searchFilterParamsViewController, ViewController.AnimationDirection.Vertical);
    this._levelSearchViewController.UpdateSearchLevelFilterParams(levelFilterParams);
  }

  private void HandleLevelSearchViewControllerDidPressSearchButton(
    LevelSearchViewController viewController,
    LevelFilterParams levelFilterParams)
  {
    this._searchFilterParamsViewController.Setup(levelFilterParams);
    this.PresentViewController((ViewController) this._searchFilterParamsViewController, animationDirection: ViewController.AnimationDirection.Vertical);
  }

  private void HandleLevelSelectionNavigationControllerDidPressActionButton(
    LevelSelectionNavigationController viewController)
  {
    this.ActionButtonWasPressed();
  }

  protected virtual void HandleLevelSelectionNavigationControllerDidPressPracticeButton(
    LevelSelectionNavigationController viewController,
    IBeatmapLevel level)
  {
    this.PracticeButtonWasPressed();
  }

  protected virtual void HandleLevelSelectionNavigationControllerDidChangeDifficultyBeatmap(
    LevelSelectionNavigationController viewController,
    IDifficultyBeatmap beatmap)
  {
    this.SelectionDidChange((IBeatmapLevelPack) null, beatmap);
  }

  protected virtual void HandleLevelSelectionNavigationControllerDidChangeLevelDetailContent(
    LevelSelectionNavigationController viewController,
    StandardLevelDetailViewController.ContentType contentType)
  {
    if (contentType == StandardLevelDetailViewController.ContentType.OwnedAndReady)
      this.SelectionDidChange((IBeatmapLevelPack) null, this.selectedDifficultyBeatmap);
    else
      this.SelectionDidChange((IBeatmapLevelPack) null, (IDifficultyBeatmap) null);
  }

  protected virtual void Refresh() => this.levelSelectionNavigationController.RefreshDetail();

  protected bool IsMainViewController(ViewController viewController) => (UnityEngine.Object) viewController == (UnityEngine.Object) this.levelSelectionNavigationController;

  protected void PresentMainViewController(
    System.Action finishedCallback,
    ViewController.AnimationType animationType)
  {
    this.ReplaceTopViewController((ViewController) this.levelSelectionNavigationController, finishedCallback, animationType);
  }

  public class State
  {
    public readonly SelectLevelCategoryViewController.LevelCategory? levelCategory;
    public readonly IBeatmapLevelPack beatmapLevelPack;
    public readonly IPreviewBeatmapLevel previewBeatmapLevel;
    public readonly IDifficultyBeatmap difficultyBeatmap;

    private State(
      SelectLevelCategoryViewController.LevelCategory? levelCategory,
      IBeatmapLevelPack beatmapLevelPack,
      IPreviewBeatmapLevel previewBeatmapLevel,
      IDifficultyBeatmap difficultyBeatmap)
    {
      this.levelCategory = levelCategory;
      this.beatmapLevelPack = beatmapLevelPack;
      this.difficultyBeatmap = difficultyBeatmap;
      this.previewBeatmapLevel = previewBeatmapLevel;
    }

    public State(IBeatmapLevelPack beatmapLevelPack)
      : this(new SelectLevelCategoryViewController.LevelCategory?(), beatmapLevelPack, (IPreviewBeatmapLevel) null, (IDifficultyBeatmap) null)
    {
    }

    public State(IBeatmapLevelPack beatmapLevelPack, IPreviewBeatmapLevel previewBeatmapLevel)
      : this(new SelectLevelCategoryViewController.LevelCategory?(), beatmapLevelPack, previewBeatmapLevel, (IDifficultyBeatmap) null)
    {
    }

    public State(
      SelectLevelCategoryViewController.LevelCategory levelCategory,
      IBeatmapLevelPack beatmapLevelPack,
      IDifficultyBeatmap difficultyBeatmap)
      : this(new SelectLevelCategoryViewController.LevelCategory?(levelCategory), beatmapLevelPack, (IPreviewBeatmapLevel) difficultyBeatmap.level, difficultyBeatmap)
    {
    }
  }
}
