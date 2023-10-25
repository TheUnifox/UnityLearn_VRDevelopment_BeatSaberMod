// Decompiled with JetBrains decompiler
// Type: MultiplayerLevelSelectionFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using Zenject;

public class MultiplayerLevelSelectionFlowCoordinator : LevelSelectionFlowCoordinator
{
  [SerializeField]
  protected BeatmapCharacteristicSO[] _notAllowedCharacteristics;
  [Inject]
  protected readonly ILobbyGameStateController _lobbyGameStateController;
  protected string _actionButtonText;
  protected string _titleText;
  protected SongPackMask _songPackMask;
  protected BeatmapDifficultyMask _allowedBeatmapDifficultyMask;
  protected LevelSelectionFlowCoordinator.State _state;
  protected bool _isBeingFinished;

  public event System.Action<LevelSelectionFlowCoordinator.State> didSelectLevelEvent;

  public event System.Action didFinishedEvent;

  protected override bool hidePracticeButton => true;

  protected override bool hidePacksIfOneOrNone => false;

  protected override string actionButtonText => this._actionButtonText;

  protected override bool showBackButtonForMainViewController => true;

  protected override SongPackMask songPackMask => this._songPackMask;

  protected override bool enableCustomLevels => false;

  protected override BeatmapDifficultyMask allowedBeatmapDifficultyMask => this._allowedBeatmapDifficultyMask;

  protected override BeatmapCharacteristicSO[] notAllowedCharacteristics => this._notAllowedCharacteristics;

  protected override string mainTitle => this._titleText;

  protected override void ActionButtonWasPressed()
  {
    System.Action<LevelSelectionFlowCoordinator.State> selectLevelEvent = this.didSelectLevelEvent;
    if (selectLevelEvent == null)
      return;
    selectLevelEvent(new LevelSelectionFlowCoordinator.State(this.selectedLevelCategory, this.selectedBeatmapLevelPack, this.selectedDifficultyBeatmap));
  }

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    if (this.isInRootViewController)
    {
      System.Action didFinishedEvent = this.didFinishedEvent;
      if (didFinishedEvent == null)
        return;
      didFinishedEvent();
    }
    else
      base.BackButtonWasPressed(topViewController);
  }

  protected override void LevelSelectionFlowCoordinatorTopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this.levelSelectionNavigationController)
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
      this.SetBottomScreenViewController((ViewController) null, animationType);
      this.SetTitle(this._titleText, animationType);
      this.showBackButton = true;
    }
    else
      base.LevelSelectionFlowCoordinatorTopViewControllerWillChange(oldViewController, newViewController, animationType);
  }

  protected override void TransitionDidStart()
  {
    base.TransitionDidStart();
    if (!this.IsFlowCoordinatorInHierarchy((FlowCoordinator) this))
      return;
    this._lobbyGameStateController.gameStartedEvent -= new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerGameStarted);
  }

  protected override void TransitionDidFinish()
  {
    base.TransitionDidFinish();
    if (this._isBeingFinished || !this.IsFlowCoordinatorInHierarchy((FlowCoordinator) this))
      return;
    if (this._lobbyGameStateController.state != MultiplayerLobbyState.LobbySetup && this._lobbyGameStateController.state != MultiplayerLobbyState.LobbyCountdown)
      this.DismissViewControllersAndFinish();
    else
      this._lobbyGameStateController.gameStartedEvent += new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerGameStarted);
  }

  public virtual void Setup(
    LevelSelectionFlowCoordinator.State state,
    SongPackMask songPackMask,
    BeatmapDifficultyMask allowedBeatmapDifficultyMask,
    string actionText,
    string titleText)
  {
    this._state = state;
    if (this._state != null)
      this.Setup(state);
    this._songPackMask = songPackMask;
    this._allowedBeatmapDifficultyMask = allowedBeatmapDifficultyMask;
    this._actionButtonText = actionText;
    this._titleText = titleText;
  }

  public virtual void HandleLobbyGameStateControllerGameStarted(
    ILevelGameplaySetupData levelGameplaySetupData)
  {
    this.DismissViewControllersAndFinish();
  }

  public virtual void DismissViewControllersAndFinish()
  {
    this._isBeingFinished = true;
    while (!this.isInRootViewController)
      this.DismissViewController(this.topViewController, immediately: true);
    this._isBeingFinished = false;
    System.Action didFinishedEvent = this.didFinishedEvent;
    if (didFinishedEvent == null)
      return;
    didFinishedEvent();
  }
}
