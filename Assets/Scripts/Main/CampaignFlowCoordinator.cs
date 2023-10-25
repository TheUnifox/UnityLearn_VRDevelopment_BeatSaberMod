// Decompiled with JetBrains decompiler
// Type: CampaignFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class CampaignFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected MenuLightsPresetSO _defaultLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _resultsClearedLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _resultsFailedLightsPreset;
  [SerializeField]
  protected MenuLightsPresetSO _newObjectiveLightsPreset;
  [Inject]
  protected readonly MenuTransitionsHelper _menuTransitionsHelper;
  [Inject]
  protected readonly MenuLightsManager _menuLightsManager;
  [Inject]
  protected readonly MissionSelectionNavigationController _missionSelectionNavigationController;
  [Inject]
  protected readonly MissionResultsViewController _missionResultsViewController;
  [Inject]
  protected readonly GameplaySetupViewController _gameplaySetupViewController;
  [Inject]
  protected readonly MissionHelpViewController _missionHelpViewController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly CampaignProgressModel _campaignProgressModel;
  protected bool _showCredits;

  public event System.Action<CampaignFlowCoordinator> didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this.SetTitle(Localization.Get("TITLE_CAMPAIGN"));
    if (addedToHierarchy)
    {
      this._missionSelectionNavigationController.didPressPlayButtonEvent += new System.Action<MissionSelectionNavigationController>(this.HandleMissionSelectionNavigationControllerDidPressPlayButton);
      this._missionResultsViewController.continueButtonPressedEvent += new System.Action<MissionResultsViewController>(this.HandleMissionResultsViewControllerContinueButtonPressed);
      this._missionResultsViewController.retryButtonPressedEvent += new System.Action<MissionResultsViewController>(this.HandleMissionResultsViewControllerRetryButtonPressed);
      this._missionHelpViewController.didFinishEvent += new System.Action<MissionHelpViewController>(this.HandleMissionHelpViewControllerDidFinish);
      this.showBackButton = true;
      this._gameplaySetupViewController.Setup(false, false, true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
      this.ProvideInitialViewControllers((ViewController) this._missionSelectionNavigationController, (ViewController) this._gameplaySetupViewController);
    }
    this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, true);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._missionSelectionNavigationController.didPressPlayButtonEvent -= new System.Action<MissionSelectionNavigationController>(this.HandleMissionSelectionNavigationControllerDidPressPlayButton);
    this._missionResultsViewController.continueButtonPressedEvent -= new System.Action<MissionResultsViewController>(this.HandleMissionResultsViewControllerContinueButtonPressed);
    this._missionResultsViewController.retryButtonPressedEvent -= new System.Action<MissionResultsViewController>(this.HandleMissionResultsViewControllerRetryButtonPressed);
    this._missionHelpViewController.didFinishEvent -= new System.Action<MissionHelpViewController>(this.HandleMissionHelpViewControllerDidFinish);
  }

  protected override void TopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._missionSelectionNavigationController)
    {
      this.SetLeftScreenViewController((ViewController) this._gameplaySetupViewController, animationType);
      this.SetTitle(Localization.Get("TITLE_CAMPAIGN"), animationType);
      this.showBackButton = true;
    }
    else
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetTitle((string) null, animationType);
      this.showBackButton = false;
    }
  }

  public virtual void HandleMissionSelectionNavigationControllerDidPressPlayButton(
    MissionSelectionNavigationController viewController)
  {
    MissionHelpSO missionHelp = viewController.selectedMissionNode.missionData.missionHelp;
    if ((UnityEngine.Object) missionHelp != (UnityEngine.Object) null && !this._playerDataModel.playerData.WasMissionHelpShowed(missionHelp))
    {
      this._playerDataModel.playerData.MissionHelpWasShowed(missionHelp);
      this._menuLightsManager.SetColorPreset(this._newObjectiveLightsPreset, true);
      this._missionHelpViewController.Setup(missionHelp);
      this.PresentViewController((ViewController) this._missionHelpViewController);
    }
    else
      this.StartLevel((System.Action) null);
  }

  public virtual void HandleMissionHelpViewControllerDidFinish(
    MissionHelpViewController viewController)
  {
    this.StartLevel((System.Action) (() =>
    {
      this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, false);
      this.DismissViewController((ViewController) viewController, immediately: true);
    }));
  }

  public virtual void HandleMissionResultsViewControllerContinueButtonPressed(
    MissionResultsViewController viewController)
  {
    this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, true);
    this.DismissViewController((ViewController) viewController, finishedCallback: (System.Action) (() => this._missionSelectionNavigationController.PresentMissionClearedIfNeeded((System.Action<bool>) (presented =>
    {
      if (!presented || !this._showCredits)
        return;
      this._showCredits = false;
      this._menuTransitionsHelper.ShowCredits();
    }))));
  }

  public virtual void HandleMissionResultsViewControllerRetryButtonPressed(
    MissionResultsViewController viewController)
  {
    this.StartLevel((System.Action) (() =>
    {
      this._menuLightsManager.SetColorPreset(this._defaultLightsPreset, false);
      this.DismissViewController((ViewController) viewController, immediately: true);
    }));
  }

  public virtual void StartLevel(System.Action beforeSceneSwitchCallback)
  {
    string missionId = this._missionSelectionNavigationController.selectedMissionNode.missionId;
    MissionDataSO missionData = this._missionSelectionNavigationController.selectedMissionNode.missionData;
    IDifficultyBeatmap difficultyBeatmap = missionData.level.beatmapLevelData.GetDifficultyBeatmap(missionData.beatmapCharacteristic, missionData.beatmapDifficulty);
    GameplayModifiers gameplayModifiers = missionData.gameplayModifiers;
    MissionObjective[] missionObjectives = missionData.missionObjectives;
    PlayerSpecificSettings playerSettings = this._gameplaySetupViewController.playerSettings;
    ColorScheme overrideColorScheme = this._playerDataModel.playerData.colorSchemesSettings.GetOverrideColorScheme();
    this._menuTransitionsHelper.StartMissionLevel(missionId, difficultyBeatmap, (IPreviewBeatmapLevel) missionData.level, overrideColorScheme, gameplayModifiers, missionObjectives, playerSettings, beforeSceneSwitchCallback, new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelSceneDidFinish), new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelSceneRestarted));
  }

  public virtual void HandleMissionLevelSceneDidFinish(
    MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData,
    MissionCompletionResults missionCompletionResults)
  {
    MissionNode selectedMissionNode = this._missionSelectionNavigationController.selectedMissionNode;
    this.UpdatePlayerStatistics(missionCompletionResults, selectedMissionNode);
    if (missionCompletionResults.levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Failed && missionCompletionResults.levelCompletionResults.levelEndStateType != LevelCompletionResults.LevelEndStateType.Cleared)
      return;
    this._menuLightsManager.SetColorPreset(missionCompletionResults.IsMissionComplete ? this._resultsClearedLightsPreset : this._resultsFailedLightsPreset, true);
    this._missionResultsViewController.Init(selectedMissionNode, missionCompletionResults);
    this.PresentViewController((ViewController) this._missionResultsViewController, immediately: true);
  }

  public virtual void HandleMissionLevelSceneRestarted(
    MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData,
    MissionCompletionResults missionCompletionResults)
  {
    MissionNode selectedMissionNode = this._missionSelectionNavigationController.selectedMissionNode;
    this.UpdatePlayerStatistics(missionCompletionResults, selectedMissionNode);
  }

  public virtual void UpdatePlayerStatistics(
    MissionCompletionResults missionCompletionResults,
    MissionNode missionNode)
  {
    if (missionCompletionResults.IsMissionComplete)
    {
      this._showCredits = this._campaignProgressModel.WillFinishGameAfterThisMission(missionNode.missionId);
      this._campaignProgressModel.SetMissionCleared(missionNode.missionId);
    }
    this._playerDataModel.playerData.playerAllOverallStatsData.UpdateCampaignOverallStatsData(missionCompletionResults, missionNode);
    this._playerDataModel.Save();
  }

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    if (topViewController.isInTransition || !((UnityEngine.Object) topViewController == (UnityEngine.Object) this._missionSelectionNavigationController))
      return;
    System.Action<CampaignFlowCoordinator> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }

  [CompilerGenerated]
  public virtual void m_CHandleMissionResultsViewControllerContinueButtonPressedm_Eb__21_0() => this._missionSelectionNavigationController.PresentMissionClearedIfNeeded((System.Action<bool>) (presented =>
  {
    if (!presented || !this._showCredits)
      return;
    this._showCredits = false;
    this._menuTransitionsHelper.ShowCredits();
  }));

  [CompilerGenerated]
  public virtual void m_CHandleMissionResultsViewControllerContinueButtonPressedm_Eb__21_1(
    bool presented)
  {
    if (!presented || !this._showCredits)
      return;
    this._showCredits = false;
    this._menuTransitionsHelper.ShowCredits();
  }
}
