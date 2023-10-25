// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerGameplayManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalActivePlayerGameplayManager : MonoBehaviour
{
  [Inject]
  protected readonly GameSongController _gameSongController;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly IMultiplayerLevelEndActionsListener _multiplayerLevelEndActions;
  [Inject]
  protected readonly PrepareLevelCompletionResults _prepareLevelCompletionResults;
  [Inject]
  protected readonly GameEnergyCounter _gameEnergyCounter;
  [Inject]
  protected readonly MultiplayerLocalActivePlayerInGameMenuController _inGameMenuController;
  [Inject]
  protected readonly IVRPlatformHelper _platformHelper;
  [Inject]
  protected readonly MultiplayerLocalPlayerDisconnectHelper _disconnectHelper;
  [Inject]
  protected readonly MultiplayerLocalActivePlayerGameplayManager.InitData _initData;
  [Inject]
  protected readonly SaberManager _saberManager;
  protected bool _levelFinishedOrFailed;

  public virtual void Start()
  {
    this._gameSongController.songDidFinishEvent += new System.Action(this.HandleSongDidFinish);
    this._gameEnergyCounter.gameEnergyDidReach0Event += new System.Action(this.HandleGameEnergyDidReach0);
    this._inGameMenuController.didGiveUpEvent += new System.Action(this.HandleInGameMenuControllerDidGiveUp);
    this._inGameMenuController.requestsDisconnectEvent += new System.Action(this.HandleInGameMenuControllerRequestsDisconnect);
    this._platformHelper.hmdUnmountedEvent += new System.Action(this.HandleHmdUnmounted);
    this._platformHelper.vrFocusWasCapturedEvent += new System.Action(this.HandleVrFocusWasCapturedEvent);
    this._platformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleInputFocusCaptured);
    this._platformHelper.inputFocusWasReleasedEvent += new System.Action(this.HandleInputFocusReleased);
    if (this._platformHelper.hasInputFocus)
      return;
    this.HandleInputFocusCaptured();
  }

  public virtual void OnDisable()
  {
    if ((UnityEngine.Object) this._gameSongController != (UnityEngine.Object) null)
      this._gameSongController.songDidFinishEvent -= new System.Action(this.HandleSongDidFinish);
    if ((UnityEngine.Object) this._gameEnergyCounter != (UnityEngine.Object) null)
      this._gameEnergyCounter.gameEnergyDidReach0Event -= new System.Action(this.HandleGameEnergyDidReach0);
    if ((UnityEngine.Object) this._inGameMenuController != (UnityEngine.Object) null)
    {
      this._inGameMenuController.didGiveUpEvent -= new System.Action(this.HandleInGameMenuControllerDidGiveUp);
      this._inGameMenuController.requestsDisconnectEvent -= new System.Action(this.HandleInGameMenuControllerRequestsDisconnect);
    }
    if (this._platformHelper == null)
      return;
    this._platformHelper.hmdUnmountedEvent -= new System.Action(this.HandleHmdUnmounted);
    this._platformHelper.vrFocusWasCapturedEvent -= new System.Action(this.HandleVrFocusWasCapturedEvent);
    this._platformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusCaptured);
    this._platformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
  }

  public virtual void PerformPlayerFail()
  {
    if (this._levelFinishedOrFailed)
      return;
    this._levelFinishedOrFailed = true;
    this._multiplayerSessionManager.SetLocalPlayerState("finished_level", false);
    this._multiplayerSessionManager.SetLocalPlayerState("is_active", false);
    this._multiplayerLevelEndActions.ReportPlayerDidFinish(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Failed, this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Failed, LevelCompletionResults.LevelEndAction.None)));
  }

  public virtual void PerformPlayerGivenUp()
  {
    if (this._levelFinishedOrFailed)
      return;
    this._levelFinishedOrFailed = true;
    this._multiplayerSessionManager.SetLocalPlayerState("finished_level", false);
    this._multiplayerSessionManager.SetLocalPlayerState("is_active", false);
    this._multiplayerLevelEndActions.ReportPlayerDidFinish(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.GivenUp, this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Incomplete, LevelCompletionResults.LevelEndAction.None)));
  }

  public virtual void HandleGameEnergyDidReach0()
  {
    if (!this._initData.failOn0Energy)
      return;
    this.PerformPlayerFail();
  }

  public virtual void HandleInGameMenuControllerDidGiveUp() => this.PerformPlayerGivenUp();

  public virtual void HandleInGameMenuControllerRequestsDisconnect() => this._disconnectHelper.Disconnect(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Incomplete, LevelCompletionResults.LevelEndAction.None));

  public virtual void HandleSongDidFinish()
  {
    if (this._levelFinishedOrFailed)
      return;
    this._levelFinishedOrFailed = true;
    this._multiplayerSessionManager.SetLocalPlayerState("finished_level", true);
    this._multiplayerSessionManager.SetLocalPlayerState("is_active", false);
    this._multiplayerLevelEndActions.ReportPlayerDidFinish(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.SongFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Cleared, this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Cleared, LevelCompletionResults.LevelEndAction.None)));
  }

  public virtual void HandleHmdUnmounted() => this.PerformPlayerFail();

  public virtual void HandleVrFocusWasCapturedEvent() => this.PerformPlayerFail();

  public virtual void HandleInputFocusCaptured() => this._saberManager.disableSabers = true;

  public virtual void HandleInputFocusReleased()
  {
    if (this._inGameMenuController.gameMenuIsShown)
      return;
    this._saberManager.disableSabers = false;
  }

  public class InitData
  {
    public readonly bool failOn0Energy;

    public InitData(bool failOn0Energy) => this.failOn0Energy = failOn0Energy;
  }
}
