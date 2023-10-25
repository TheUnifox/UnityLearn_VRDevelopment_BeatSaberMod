// Decompiled with JetBrains decompiler
// Type: SinglePlayerLevelSelectionFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using Zenject;

public abstract class SinglePlayerLevelSelectionFlowCoordinator : LevelSelectionFlowCoordinator
{
  [SerializeField]
  private BeatmapCharacteristicSO _degree360BeatmapCharacteristic;
  [Inject]
  protected readonly PracticeViewController _practiceViewController;
  [Inject]
  private readonly GameplaySetupViewController _gameplaySetupViewController;
  [Inject]
  private readonly MenuTransitionsHelper _menuTransitionsHelper;
  [Inject]
  private readonly IVRPlatformHelper _vrPlatformHelper;
  [Inject]
  private readonly AppStaticSettingsSO _appStaticSettings;
  [Inject]
  private readonly SimpleDialogPromptViewController _simpleDialogPromptViewController;

  protected abstract string gameMode { get; }

  protected bool isInPracticeView => (UnityEngine.Object) this.topViewController == (UnityEngine.Object) this._practiceViewController;

  protected PlayerSpecificSettings playerSettings => this._gameplaySetupViewController.playerSettings;

  protected override bool enableCustomLevels => this._appStaticSettings.enableCustomLevels;

  private GameplayModifiers gameplayModifiers => this._gameplaySetupViewController.gameplayModifiers;

  protected virtual bool hideGameplaySetup => false;

  protected virtual LeaderboardViewController leaderboardViewController => (LeaderboardViewController) null;

  protected override ViewController initialTopScreenViewController => (ViewController) null;

  protected override ViewController initialLeftScreenViewController => !this.hideGameplaySetup ? (ViewController) this._gameplaySetupViewController : (ViewController) null;

  protected virtual void SinglePlayerLevelSelectionFlowCoordinatorDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
  }

  protected virtual void SinglePlayerLevelSelectionFlowCoordinatorDidDeactivate(
    bool removedFromHierarchy)
  {
  }

  protected virtual void ProcessLevelCompletionResultsAfterLevelDidFinish(
    LevelCompletionResults levelCompletionResults,
    IReadonlyBeatmapData transformedBeatmapData,
    IDifficultyBeatmap difficultyBeatmap,
    GameplayModifiers gameplayModifiers,
    bool practice)
  {
  }

  public event System.Action<SinglePlayerLevelSelectionFlowCoordinator> didFinishEvent;

  protected override sealed void LevelSelectionFlowCoordinatorDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
    if (addedToHierarchy)
      this._practiceViewController.didPressPlayButtonEvent += new System.Action(this.HandlePracticeViewControllerDidPressPlayButton);
    this.SinglePlayerLevelSelectionFlowCoordinatorDidActivate(firstActivation, addedToHierarchy);
  }

  protected override sealed void LevelSelectionFlowCoordinatorDidDeactivate(
    bool removedFromHierarchy)
  {
    if (removedFromHierarchy)
      this._practiceViewController.didPressPlayButtonEvent -= new System.Action(this.HandlePracticeViewControllerDidPressPlayButton);
    this.SinglePlayerLevelSelectionFlowCoordinatorDidDeactivate(removedFromHierarchy);
  }

  protected override void LevelSelectionFlowCoordinatorTopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    if (this.IsMainViewController(newViewController))
    {
      this.showBackButton = this.showBackButtonForMainViewController;
      this.SetLeftScreenViewController(this.hideGameplaySetup ? (ViewController) null : (ViewController) this._gameplaySetupViewController, animationType);
      if (this.selectedDifficultyBeatmap != null)
        this.SetRightScreenViewController((ViewController) this.leaderboardViewController, animationType);
      this.SetTitle(this.mainTitle, animationType);
    }
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._practiceViewController)
    {
      this.SetLeftScreenViewController(this.hideGameplaySetup ? (ViewController) null : (ViewController) this._gameplaySetupViewController, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
      this.SetBottomScreenViewController((ViewController) null, animationType);
      this.SetTitle(Localization.Get("TITLE_PRACTICE_MODE"), animationType);
      this.showBackButton = true;
    }
    else
    {
      this.SetTitle((string) null, animationType);
      base.LevelSelectionFlowCoordinatorTopViewControllerWillChange(oldViewController, newViewController, animationType);
    }
  }

  protected override void ActionButtonWasPressed() => this.StartLevelOrShow360Prompt((System.Action) null, false);

  protected override void PracticeButtonWasPressed()
  {
    this._practiceViewController.Init(this.selectedDifficultyBeatmap.level, this.selectedDifficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, this.selectedDifficultyBeatmap.difficulty);
    this.PresentViewController((ViewController) this._practiceViewController);
  }

  protected override void SelectionDidChange(IBeatmapLevelPack pack, IDifficultyBeatmap beatmap)
  {
    if (beatmap == null)
    {
      this.SetRightScreenViewController((ViewController) null, ViewController.AnimationType.Out);
    }
    else
    {
      this.leaderboardViewController.SetData(beatmap);
      this.SetRightScreenViewController((ViewController) this.leaderboardViewController, ViewController.AnimationType.In);
    }
  }

  private void StartLevelOrShow360Prompt(System.Action beforeSceneSwitchCallback, bool practice)
  {
    if (this._appStaticSettings.enable360DegreeLevels && !this._vrPlatformHelper.isAlwaysWireless && this.playerDataModel.playerData.shouldShow360Warning && (UnityEngine.Object) this.selectedDifficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic == (UnityEngine.Object) this._degree360BeatmapCharacteristic)
    {
      this.playerDataModel.playerData.Mark360WarningAsShown();
      this.playerDataModel.Save();
      this._simpleDialogPromptViewController.Init(Localization.Get("PROMPT_INFORMATION"), Localization.Get("PROMPT_HAVENT_PLAYED_360_YET"), Localization.Get("BUTTON_OK"), (System.Action<int>) (buttonNumber => this.StartLevel((System.Action) (() =>
      {
        this.DismissViewController((ViewController) this._simpleDialogPromptViewController, immediately: true);
        System.Action action = beforeSceneSwitchCallback;
        if (action == null)
          return;
        action();
      }), practice)));
      this.PresentViewController((ViewController) this._simpleDialogPromptViewController);
    }
    else
      this.StartLevel(beforeSceneSwitchCallback, practice);
  }

  public void StartLevel(System.Action beforeSceneSwitchCallback, bool practice) => this._menuTransitionsHelper.StartStandardLevel(this.gameMode, this.selectedDifficultyBeatmap, this.selectedBeatmapLevel, this._gameplaySetupViewController.environmentOverrideSettings, this._gameplaySetupViewController.colorSchemesSettings.GetOverrideColorScheme(), this.gameplayModifiers, this.playerSettings, practice ? this._practiceViewController.practiceSettings : (PracticeSettings) null, Localization.Get("BUTTON_MENU"), false, false, beforeSceneSwitchCallback, new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleStandardLevelDidFinish), (System.Action<LevelScenesTransitionSetupDataSO, LevelCompletionResults>) null);

  private void HandlePracticeViewControllerDidPressPlayButton() => this.StartLevelOrShow360Prompt((System.Action) null, true);

  private void HandleStandardLevelDidFinish(
    StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData,
    LevelCompletionResults levelCompletionResults)
  {
    bool practice = standardLevelScenesTransitionSetupData.practiceSettings != null;
    IDifficultyBeatmap difficultyBeatmap = standardLevelScenesTransitionSetupData.difficultyBeatmap;
    GameplayModifiers gameplayModifiers = standardLevelScenesTransitionSetupData.gameplayModifiers;
    IReadonlyBeatmapData transformedBeatmapData = standardLevelScenesTransitionSetupData.transformedBeatmapData;
    this.ProcessLevelCompletionResultsAfterLevelDidFinish(levelCompletionResults, transformedBeatmapData, difficultyBeatmap, gameplayModifiers, practice);
    this.Refresh();
  }

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    if (this.isInPracticeView)
      this.DismissViewController((ViewController) this._practiceViewController);
    else if (this.isInRootViewController)
    {
      System.Action<SinglePlayerLevelSelectionFlowCoordinator> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(this);
    }
    else
      base.BackButtonWasPressed(topViewController);
  }

  protected void SetupGameplaySetupViewController(
    bool showModifiers,
    bool showEnvironmentOverrideSettings,
    bool showColorSchemesSettings)
  {
    this._gameplaySetupViewController.Setup(showModifiers, showEnvironmentOverrideSettings, showColorSchemesSettings, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
  }

  protected bool HandleBasicLevelCompletionResults(
    LevelCompletionResults levelCompletionResults,
    bool practice)
  {
    return levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Failed && levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared;
  }

  protected void DismissPracticeViewController(System.Action finishedCallback, bool immediately)
  {
    if (!((UnityEngine.Object) this.topViewController == (UnityEngine.Object) this._practiceViewController))
      return;
    this.DismissViewController((ViewController) this._practiceViewController, finishedCallback: finishedCallback, immediately: immediately);
  }
}
