// Decompiled with JetBrains decompiler
// Type: PartyFreePlayFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using Zenject;

public class PartyFreePlayFlowCoordinator : SinglePlayerLevelSelectionFlowCoordinator
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
  protected readonly LocalLeaderboardViewController _localLeaderboardViewController;
  [Inject]
  protected readonly EnterPlayerGuestNameViewController _enterNameViewController;

  protected override string gameMode => "Party";

  protected override LeaderboardViewController leaderboardViewController => (LeaderboardViewController) this._localLeaderboardViewController;

  protected override bool showBackButtonForMainViewController => true;

  protected override string mainTitle => Localization.Get("TITLE_PARTY");

  protected override void SinglePlayerLevelSelectionFlowCoordinatorDidActivate(
    bool firstActivation,
    bool addedToHierarchy)
  {
    if (addedToHierarchy)
    {
      this._localLeaderboardViewController.Setup(true);
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
    if (!removedFromHierarchy)
      return;
    this._localLeaderboardViewController.leaderboardsModel.ClearLastScorePosition();
    if (!((UnityEngine.Object) this._resultsViewController != (UnityEngine.Object) null))
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
      this.playerDataModel.playerData.playerAllOverallStatsData.UpdatePartyFreePlayOverallStatsData(levelCompletionResults, difficultyBeatmap);
    if (this.HandleBasicLevelCompletionResults(levelCompletionResults, practice))
    {
      if (practice)
        return;
      this.playerDataModel.Save();
    }
    else
    {
      string leaderboardId = LocalLeaderboardsIdModel.GetLocalLeaderboardID(difficultyBeatmap);
      this.playerDataModel.playerData.IncreaseNumberOfGameplays(this.playerDataModel.playerData.GetPlayerLevelStatsData(difficultyBeatmap));
      if (this.WillScoreGoToLeaderboard(levelCompletionResults, leaderboardId, practice))
      {
        this._enterNameViewController.Init((EnterPlayerGuestNameViewController.FinishDelegate) ((viewController, playerName) =>
        {
          bool newHighScore = this.IsNewHighScore(levelCompletionResults, leaderboardId);
          this._menuLightsManager.SetColorPreset(levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared ? this._resultsClearedLightsPreset : this._resultsFailedLightsPreset, true);
          this.ProcessScore(levelCompletionResults, leaderboardId, playerName);
          this._resultsViewController.Init(levelCompletionResults, transformedBeatmapData, difficultyBeatmap, practice, newHighScore);
          this.ReplaceTopViewController((ViewController) this._resultsViewController);
        }));
        this.PresentViewController((ViewController) this._enterNameViewController, immediately: true);
      }
      else
      {
        this._menuLightsManager.SetColorPreset(levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared ? this._resultsClearedLightsPreset : this._resultsFailedLightsPreset, true);
        this._resultsViewController.Init(levelCompletionResults, transformedBeatmapData, difficultyBeatmap, practice, false);
        this.PresentViewController((ViewController) this._resultsViewController, immediately: true);
      }
      this.playerDataModel.Save();
    }
  }

  public virtual bool WillScoreGoToLeaderboard(
    LevelCompletionResults levelCompletionResults,
    string leaderboardId,
    bool practice)
  {
    return !practice && levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared && this._localLeaderboardViewController.leaderboardsModel.WillScoreGoIntoLeaderboard(leaderboardId, levelCompletionResults.modifiedScore);
  }

  public virtual bool IsNewHighScore(
    LevelCompletionResults levelCompletionResults,
    string leaderboardId)
  {
    return this._localLeaderboardViewController.leaderboardsModel.GetHighScore(leaderboardId, LocalLeaderboardsModel.LeaderboardType.AllTime) < levelCompletionResults.modifiedScore;
  }

  public virtual void ProcessScore(
    LevelCompletionResults levelCompletionResults,
    string leaderboardId,
    string playerName)
  {
    if (string.IsNullOrEmpty(playerName))
      return;
    this._localLeaderboardViewController.leaderboardsModel.AddScore(leaderboardId, playerName, levelCompletionResults.modifiedScore, levelCompletionResults.fullCombo);
    this._localLeaderboardViewController.leaderboardsModel.Save();
  }

  public virtual void HandleResultsViewControllerContinueButtonPressed(
    ResultsViewController resultsViewController)
  {
    this.DismissViewController((ViewController) resultsViewController);
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
}
