// Decompiled with JetBrains decompiler
// Type: LevelCollectionNavigationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class LevelCollectionNavigationController : NavigationController
{
  [SerializeField]
  protected LoadingControl _loadingControl;
  [Inject]
  protected readonly LevelCollectionViewController _levelCollectionViewController;
  [Inject]
  protected readonly LevelPackDetailViewController _levelPackDetailViewController;
  [Inject]
  protected readonly StandardLevelDetailViewController _levelDetailViewController;
  [Inject]
  protected readonly AppStaticSettingsSO _appStaticSettings;
  [Inject]
  protected readonly TimeTweeningManager _timeTweeningManager;
  protected bool _showPracticeButtonInDetailView;
  protected string _actionButtonTextInDetailView;
  protected IBeatmapLevelPack _levelPack;
  protected BeatmapDifficultyMask _allowedBeatmapDifficultyMask;
  protected IPreviewBeatmapLevel _beatmapLevelToBeSelectedAfterPresent;
  protected bool _loading;
  protected bool _hideDetailViewController;
  protected BeatmapCharacteristicSO[] _notAllowedCharacteristics;
  protected FloatTween _floatTween;

  public event System.Action<LevelCollectionNavigationController, StandardLevelDetailViewController.ContentType> didChangeLevelDetailContentEvent;

  public event System.Action<LevelCollectionNavigationController, IBeatmapLevelPack> didSelectLevelPackEvent;

  public event System.Action<LevelCollectionNavigationController> didPressActionButtonEvent;

  public event System.Action<LevelCollectionNavigationController, IBeatmapLevelPack> didPressOpenPackButtonEvent;

  public event System.Action<LevelCollectionNavigationController, IBeatmapLevel> didPressPracticeButtonEvent;

  public event System.Action<LevelCollectionNavigationController, IDifficultyBeatmap> didChangeDifficultyBeatmapEvent;

  public IDifficultyBeatmap selectedDifficultyBeatmap => this._levelDetailViewController.selectedDifficultyBeatmap;

  public IPreviewBeatmapLevel selectedBeatmapLevel => this._levelDetailViewController.beatmapLevel;

  public virtual void SetData(
    IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection,
    bool showPackHeader,
    bool showPracticeButton,
    string actionButtonText,
    GameObject noDataInfoPrefab,
    BeatmapDifficultyMask allowedBeatmapDifficultyMask,
    BeatmapCharacteristicSO[] notAllowedCharacteristics)
  {
    this._allowedBeatmapDifficultyMask = allowedBeatmapDifficultyMask;
    this._notAllowedCharacteristics = notAllowedCharacteristics;
    if (annotatedBeatmapLevelCollection is IBeatmapLevelPack)
      this.SetDataForPack((IBeatmapLevelPack) annotatedBeatmapLevelCollection, showPackHeader, showPracticeButton, actionButtonText);
    else
      this.SetDataForLevelCollection(annotatedBeatmapLevelCollection?.beatmapLevelCollection, showPracticeButton, actionButtonText, noDataInfoPrefab);
  }

  public virtual void SelectLevel(IPreviewBeatmapLevel beatmapLevel)
  {
    this._hideDetailViewController = false;
    if (this.isActivated)
    {
      this._levelCollectionViewController.SelectLevel(beatmapLevel);
      this._beatmapLevelToBeSelectedAfterPresent = (IPreviewBeatmapLevel) null;
    }
    else
      this._beatmapLevelToBeSelectedAfterPresent = beatmapLevel;
  }

  public virtual void AnimateCanvasGroupAlpha(
    LevelCollectionNavigationController.AlphaAnimationType animationType)
  {
    if (this._floatTween != null)
    {
      FloatTween.Pool.Despawn(this._floatTween);
      this._floatTween = (FloatTween) null;
    }
    float alpha = this.canvasGroup.alpha;
    float p2 = 0.0f;
    switch (animationType)
    {
      case LevelCollectionNavigationController.AlphaAnimationType.In:
        p2 = 1f;
        break;
      case LevelCollectionNavigationController.AlphaAnimationType.Out:
        p2 = 0.2f;
        break;
    }
    this._floatTween = FloatTween.Pool.Spawn(alpha, p2, (System.Action<float>) (f => this.canvasGroup.alpha = f), 0.25f, EaseType.OutQuint, 0.0f);
    this._floatTween.onCompleted = (System.Action) (() =>
    {
      FloatTween.Pool.Despawn(this._floatTween);
      this._floatTween = (FloatTween) null;
    });
    this._timeTweeningManager.RestartTween((Tween) this._floatTween, (object) this);
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this._levelCollectionViewController.didSelectLevelEvent += new System.Action<LevelCollectionViewController, IPreviewBeatmapLevel>(this.HandleLevelCollectionViewControllerDidSelectLevel);
      this._levelCollectionViewController.didSelectHeaderEvent += new System.Action<LevelCollectionViewController>(this.HandleLevelCollectionViewControllerDidSelectPack);
      this._levelDetailViewController.didPressActionButtonEvent += new System.Action<StandardLevelDetailViewController>(this.HandleLevelDetailViewControllerDidPressActionButton);
      this._levelDetailViewController.didPressPracticeButtonEvent += new System.Action<StandardLevelDetailViewController, IBeatmapLevel>(this.HandleLevelDetailViewControllerDidPressPracticeButton);
      this._levelDetailViewController.didChangeDifficultyBeatmapEvent += new System.Action<StandardLevelDetailViewController, IDifficultyBeatmap>(this.HandleLevelDetailViewControllerDidChangeDifficultyBeatmap);
      this._levelDetailViewController.didChangeContentEvent += new System.Action<StandardLevelDetailViewController, StandardLevelDetailViewController.ContentType>(this.HandleLevelDetailViewControllerDidPresentContent);
      this._levelDetailViewController.didPressOpenLevelPackButtonEvent += new System.Action<StandardLevelDetailViewController, IBeatmapLevelPack>(this.HandleLevelDetailViewControllerDidPressOpenLevelPackButton);
      this._levelDetailViewController.levelFavoriteStatusDidChangeEvent += new System.Action<StandardLevelDetailViewController, bool>(this.HandleLevelDetailViewControllerLevelFavoriteStatusDidChange);
      if (this._beatmapLevelToBeSelectedAfterPresent != null)
      {
        this._levelCollectionViewController.SelectLevel(this._beatmapLevelToBeSelectedAfterPresent);
        this.SetChildViewControllers((ViewController) this._levelCollectionViewController);
        this._beatmapLevelToBeSelectedAfterPresent = (IPreviewBeatmapLevel) null;
      }
      else
        this.SetChildViewControllers((ViewController) this._levelCollectionViewController, (ViewController) this._levelPackDetailViewController);
    }
    else if (this._loading)
    {
      this.ClearChildViewControllers();
    }
    else
    {
      if (!this._hideDetailViewController)
        return;
      this.SetChildViewControllers((ViewController) this._levelCollectionViewController);
      this._hideDetailViewController = false;
    }
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._levelCollectionViewController.didSelectLevelEvent -= new System.Action<LevelCollectionViewController, IPreviewBeatmapLevel>(this.HandleLevelCollectionViewControllerDidSelectLevel);
    this._levelCollectionViewController.didSelectHeaderEvent -= new System.Action<LevelCollectionViewController>(this.HandleLevelCollectionViewControllerDidSelectPack);
    this._levelDetailViewController.didPressActionButtonEvent -= new System.Action<StandardLevelDetailViewController>(this.HandleLevelDetailViewControllerDidPressActionButton);
    this._levelDetailViewController.didPressPracticeButtonEvent -= new System.Action<StandardLevelDetailViewController, IBeatmapLevel>(this.HandleLevelDetailViewControllerDidPressPracticeButton);
    this._levelDetailViewController.didChangeDifficultyBeatmapEvent -= new System.Action<StandardLevelDetailViewController, IDifficultyBeatmap>(this.HandleLevelDetailViewControllerDidChangeDifficultyBeatmap);
    this._levelDetailViewController.didChangeContentEvent -= new System.Action<StandardLevelDetailViewController, StandardLevelDetailViewController.ContentType>(this.HandleLevelDetailViewControllerDidPresentContent);
    this._levelDetailViewController.didPressOpenLevelPackButtonEvent -= new System.Action<StandardLevelDetailViewController, IBeatmapLevelPack>(this.HandleLevelDetailViewControllerDidPressOpenLevelPackButton);
    this._levelDetailViewController.levelFavoriteStatusDidChangeEvent -= new System.Action<StandardLevelDetailViewController, bool>(this.HandleLevelDetailViewControllerLevelFavoriteStatusDidChange);
    this.ClearChildViewControllers();
  }

  public virtual void SetDataForPack(
    IBeatmapLevelPack levelPack,
    bool showPackHeader,
    bool showPracticeButton,
    string actionButtonText)
  {
    this._levelPack = levelPack;
    this._showPracticeButtonInDetailView = showPracticeButton;
    this._actionButtonTextInDetailView = actionButtonText;
    this._levelCollectionViewController.SetData(levelPack.beatmapLevelCollection, showPackHeader ? levelPack.packName : (string) null, showPackHeader ? levelPack.coverImage : (Sprite) null, false, (GameObject) null);
    this._levelPackDetailViewController.SetData(levelPack);
    this.PresentViewControllersForPack();
  }

  public virtual void SetDataForLevelCollection(
    IBeatmapLevelCollection beatmapLevelCollection,
    bool showPracticeButton,
    string actionButtonText,
    GameObject noDataInfoPrefab)
  {
    this._levelPack = (IBeatmapLevelPack) null;
    this._showPracticeButtonInDetailView = showPracticeButton;
    this._actionButtonTextInDetailView = actionButtonText;
    this._levelCollectionViewController.SetData(beatmapLevelCollection, (string) null, (Sprite) null, true, noDataInfoPrefab);
    this.PresentViewControllersForLevelCollection();
  }

  public virtual void RefreshDetail() => this._levelDetailViewController.RefreshContentLevelDetailView();

  public virtual void ClearSelected() => this._levelDetailViewController.ClearSelected();

  public virtual void ShowLoading()
  {
    this._loading = true;
    this._loadingControl.ShowLoading();
    if (!this.isActiveAndEnabled)
      return;
    if (this._levelPackDetailViewController.isInViewControllerHierarchy || this._levelDetailViewController.isInViewControllerHierarchy)
      this.PopViewController((System.Action) null, true);
    if (!this._levelCollectionViewController.isInViewControllerHierarchy)
      return;
    this.PopViewController((System.Action) null, true);
  }

  public virtual void PresentViewControllersForPack()
  {
    this.HideLoading();
    this._hideDetailViewController = false;
    if (!this.isActiveAndEnabled)
      return;
    if (!this._levelCollectionViewController.isInViewControllerHierarchy)
      this.PresentDetailViewController((ViewController) this._levelCollectionViewController, true);
    if (this._levelPackDetailViewController.isInViewControllerHierarchy)
      return;
    this.PresentDetailViewController((ViewController) this._levelPackDetailViewController, true);
  }

  public virtual void PresentViewControllersForLevelCollection()
  {
    this.HideLoading();
    if (!this.isActiveAndEnabled)
    {
      this._hideDetailViewController = true;
    }
    else
    {
      if (!this._levelCollectionViewController.isInViewControllerHierarchy)
        this.PresentDetailViewController((ViewController) this._levelCollectionViewController, true);
      this.HideDetailViewController();
    }
  }

  public virtual void HideLoading()
  {
    this._loading = false;
    this._loadingControl.Hide();
  }

  public virtual void HideDetailViewController()
  {
    if (!this._levelPackDetailViewController.isInViewControllerHierarchy && !this._levelDetailViewController.isInViewControllerHierarchy)
      return;
    this.PopViewController((System.Action) null, true);
  }

  public virtual void HandleLevelCollectionViewControllerDidSelectLevel(
    LevelCollectionViewController viewController,
    IPreviewBeatmapLevel level)
  {
    if (this._levelPack == null)
      this._levelDetailViewController.SetData(level, !this._showPracticeButtonInDetailView, !this._appStaticSettings.enable360DegreeLevels, this._actionButtonTextInDetailView, this._allowedBeatmapDifficultyMask, this._notAllowedCharacteristics);
    else
      this._levelDetailViewController.SetData(this._levelPack, level, !this._showPracticeButtonInDetailView, !this._appStaticSettings.enable360DegreeLevels, true, this._actionButtonTextInDetailView, this._allowedBeatmapDifficultyMask, this._notAllowedCharacteristics);
    this.PresentDetailViewController((ViewController) this._levelDetailViewController, false);
  }

  public virtual void HandleLevelCollectionViewControllerDidSelectPack(
    LevelCollectionViewController viewController)
  {
    if (this._levelPack == null)
      return;
    this._levelPackDetailViewController.SetData(this._levelPack);
    this.PresentDetailViewController((ViewController) this._levelPackDetailViewController, false);
    System.Action<LevelCollectionNavigationController, IBeatmapLevelPack> selectLevelPackEvent = this.didSelectLevelPackEvent;
    if (selectLevelPackEvent == null)
      return;
    selectLevelPackEvent(this, this._levelPack);
  }

  public virtual void PresentDetailViewController(ViewController viewController, bool immediately)
  {
    if ((UnityEngine.Object) viewController == (UnityEngine.Object) this._levelPackDetailViewController && this._levelDetailViewController.isInViewControllerHierarchy || (UnityEngine.Object) viewController == (UnityEngine.Object) this._levelDetailViewController && this._levelPackDetailViewController.isInViewControllerHierarchy)
    {
      this.PopViewController((System.Action) (() => this.PushViewController(viewController, (System.Action) null, true)), true);
    }
    else
    {
      if (viewController.isInViewControllerHierarchy)
        return;
      this.PushViewController(viewController, (System.Action) null, immediately);
    }
  }

  public virtual void HandleLevelDetailViewControllerDidPressActionButton(
    StandardLevelDetailViewController viewController)
  {
    System.Action<LevelCollectionNavigationController> actionButtonEvent = this.didPressActionButtonEvent;
    if (actionButtonEvent == null)
      return;
    actionButtonEvent(this);
  }

  public virtual void HandleLevelDetailViewControllerDidPressPracticeButton(
    StandardLevelDetailViewController viewController,
    IBeatmapLevel level)
  {
    System.Action<LevelCollectionNavigationController, IBeatmapLevel> practiceButtonEvent = this.didPressPracticeButtonEvent;
    if (practiceButtonEvent == null)
      return;
    practiceButtonEvent(this, level);
  }

  public virtual void HandleLevelDetailViewControllerDidChangeDifficultyBeatmap(
    StandardLevelDetailViewController viewController,
    IDifficultyBeatmap beatmap)
  {
    System.Action<LevelCollectionNavigationController, IDifficultyBeatmap> difficultyBeatmapEvent = this.didChangeDifficultyBeatmapEvent;
    if (difficultyBeatmapEvent == null)
      return;
    difficultyBeatmapEvent(this, beatmap);
  }

  public virtual void HandleLevelDetailViewControllerDidPresentContent(
    StandardLevelDetailViewController viewController,
    StandardLevelDetailViewController.ContentType contentType)
  {
    System.Action<LevelCollectionNavigationController, StandardLevelDetailViewController.ContentType> detailContentEvent = this.didChangeLevelDetailContentEvent;
    if (detailContentEvent == null)
      return;
    detailContentEvent(this, contentType);
  }

  public virtual void HandleLevelDetailViewControllerDidPressOpenLevelPackButton(
    StandardLevelDetailViewController viewController,
    IBeatmapLevelPack levelPack)
  {
    System.Action<LevelCollectionNavigationController, IBeatmapLevelPack> openPackButtonEvent = this.didPressOpenPackButtonEvent;
    if (openPackButtonEvent == null)
      return;
    openPackButtonEvent(this, levelPack);
  }

  public virtual void HandleLevelDetailViewControllerLevelFavoriteStatusDidChange(
    StandardLevelDetailViewController viewController,
    bool favoriteStatus)
  {
    this._levelCollectionViewController.RefreshFavorites();
  }

  [CompilerGenerated]
  public virtual void m_CAnimateCanvasGroupAlpham_Eb__40_0(float f) => this.canvasGroup.alpha = f;

  [CompilerGenerated]
  public virtual void m_CAnimateCanvasGroupAlpham_Eb__40_1()
  {
    FloatTween.Pool.Despawn(this._floatTween);
    this._floatTween = (FloatTween) null;
  }

  public enum AlphaAnimationType
  {
    In,
    Out,
  }
}
