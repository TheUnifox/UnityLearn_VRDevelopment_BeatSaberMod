// Decompiled with JetBrains decompiler
// Type: SoloFreePlayFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using Zenject;

public class SoloFreePlayFlowCoordinator : SinglePlayerLevelSelectionFlowCoordinator
{
  [SerializeField]
  protected MenuLightsPresetSO _defaultLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _resultsClearedLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _resultsFailedLightsPreset;
  [Inject]
  protected readonly MenuLightsManager _menuLightsManager;
  [Inject]
  protected readonly ResultsViewController _resultsViewController;
  [Inject]
  protected readonly PlatformLeaderboardViewController _platformLeaderboardViewController;
  [Inject]
  protected readonly PlatformLeaderboardsModel _platformLeaderboardsModel;

  protected override string gameMode => "Solo";

  protected override LeaderboardViewController leaderboardViewController => (LeaderboardViewController) this._platformLeaderboardViewController;

  protected override bool showBackButtonForMainViewController => true;

  protected override string mainTitle => Localization.Get("TITLE_SOLO");

  protected override void SinglePlayerLevelSelectionFlowCoordinatorDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
    if (addedToHierarchy)
    {
      this.SetupGameplaySetupViewController(true, true, true);
      this._resultsViewController.continueButtonPressedEvent += new System.Action<ResultsViewController>(this.HandleResultsViewControllerContinueButtonPressed);
      this._resultsViewController.restartButtonPressedEvent += new System.Action<ResultsViewController>(this.HandleResultsViewControllerRestartButtonPressed);
    }
    this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, true);
    if (!this.IsMainViewController(this.topViewController) || this.selectedDifficultyBeatmap == null)
      return;
    this.SetRightScreenViewController((ViewController) this.leaderboardViewController, ViewController.AnimationType.None);
  }

  protected override void SinglePlayerLevelSelectionFlowCoordinatorDidDeactivate(
    bool removedFromHierarchy)
  {
    if (!removedFromHierarchy || !((UnityEngine.Object) this._resultsViewController != (UnityEngine.Object) null))
      return;
    this._resultsViewController.continueButtonPressedEvent -= new System.Action<ResultsViewController>(this.HandleResultsViewControllerContinueButtonPressed);
    this._resultsViewController.restartButtonPressedEvent -= new System.Action<ResultsViewController>(this.HandleResultsViewControllerRestartButtonPressed);
  }

  protected override void ProcessLevelCompletionResultsAfterLevelDidFinish(
    LevelCompletionResults levelCompletionResults,
    IReadonlyBeatmapData transformedBeatmapData,
    IDifficultyBeatmap difficultyBeatmap,
    GameplayModifiers gameplayModifiers,
    bool practice)
  {
    if (!practice)
      this.playerDataModel.playerData.playerAllOverallStatsData.UpdateSoloFreePlayOverallStatsData(levelCompletionResults, difficultyBeatmap);
    if (this.HandleBasicLevelCompletionResults(levelCompletionResults, practice))
    {
      if (practice)
        return;
      this.playerDataModel.Save();
    }
    else
    {
      bool newHighScore = false;
      if (!practice)
      {
        PlayerLevelStatsData playerLevelStatsData = this.playerDataModel.playerData.GetPlayerLevelStatsData(difficultyBeatmap);
        newHighScore = this.IsNewHighScore(playerLevelStatsData, levelCompletionResults);
        LevelCompletionResultsHelper.ProcessScore(this.playerDataModel.playerData, playerLevelStatsData, levelCompletionResults, transformedBeatmapData, difficultyBeatmap, this._platformLeaderboardsModel);
      }
      this._menuLightsManager.SetColorPreset(levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared ? this._resultsClearedLightsPreset : this._resultsFailedLightsPreset, true);
      this._resultsViewController.Init(levelCompletionResults, transformedBeatmapData, difficultyBeatmap, practice, newHighScore);
      this.PresentViewController((ViewController) this._resultsViewController, immediately: true);
      this.playerDataModel.Save();
    }
  }

  public virtual bool IsNewHighScore(
    PlayerLevelStatsData playerLevelStats,
    LevelCompletionResults levelCompletionResults)
  {
    return playerLevelStats.highScore < levelCompletionResults.modifiedScore;
  }

  protected override void Refresh()
  {
    base.Refresh();
    this.leaderboardViewController.RefreshLevelStats();
  }

  public virtual void HandleResultsViewControllerContinueButtonPressed(
    ResultsViewController viewController)
  {
    this.DismissViewController((ViewController) viewController);
    this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, true);
  }

  public virtual void HandleResultsViewControllerRestartButtonPressed(
    ResultsViewController resultsViewController)
  {
    this.StartLevel((System.Action) (() =>
    {
      this.DismissViewController((ViewController) resultsViewController, immediately: true);
      this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, false);
    }), resultsViewController.practice);
  }

  public virtual void __SetupFromDestination(RunLevelMenuDestination runLevelMenuDestination)
  {
    if (runLevelMenuDestination.practice)
    {
      this._practiceViewController.Init(this.selectedDifficultyBeatmap.level, this.selectedDifficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, this.selectedDifficultyBeatmap.difficulty);
      this._practiceViewController.practiceSettings.startSongTime = runLevelMenuDestination.startSongTime;
      this._practiceViewController.practiceSettings.songSpeedMul = runLevelMenuDestination.songSpeedMultiplier;
    }
    this.playerDataModel.playerData.overrideEnvironmentSettings.overrideEnvironments = runLevelMenuDestination.overrideEnvironments;
    if (!runLevelMenuDestination.overrideEnvironments)
      return;
    EnvironmentInfoSO bySerializedName = this.playerDataModel.playerDataFileManager.GetEnvironmentInfoBySerializedName(runLevelMenuDestination.environmentName);
    if (!((UnityEngine.Object) bySerializedName != (UnityEngine.Object) null))
      return;
    this.playerDataModel.playerData.overrideEnvironmentSettings.SetEnvironmentInfoForType(bySerializedName.environmentType, bySerializedName);
  }
}
