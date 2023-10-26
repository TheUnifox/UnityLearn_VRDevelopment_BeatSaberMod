// Decompiled with JetBrains decompiler
// Type: MultiplayerController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MultiplayerController : MonoBehaviour
{
  [SerializeField]
  protected GameObject _loadingEnvironment;
  [Space]
  [SerializeField]
  protected MultiplayerLevelScenesTransitionSetupDataSO _multiplayerLevelSceneSetupData;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly MultiplayerPlayersManager _playersManager;
  [Inject]
  protected readonly SceneStartSyncController _sceneStartSyncController;
  [Inject]
  protected readonly SongStartSyncController _songStartSyncController;
  [Inject]
  protected readonly MultiplayerLevelFinishedController _multiplayerLevelFinishedController;
  [Inject]
  protected readonly FadeInOutController _fadeInOutController;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly MultiplayerIntroAnimationController _introAnimationController;
  [Inject]
  protected readonly MultiplayerOutroAnimationController _outroAnimationController;
  [Inject]
  protected readonly IMenuRpcManager _menuRpcManager;
  [Inject]
  protected readonly IGameplayRpcManager _gameplayRpcManager;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;
  [Inject]
  protected readonly DiContainer _diContainer;
  [Inject]
  protected readonly MultiplayerBadgesProvider _badgesProvider;
  protected float _startTime;
  protected MultiplayerPlayerStartState _localPlayerSyncStartState;
  protected MultiplayerController.State _state;
  protected string _sessionGameId = string.Empty;
  protected MultiplayerResultsData _resultsData;
  protected PlayersSpecificSettingsAtGameStartModel _playersSpecificSettingsAtGameStartModel;
  protected Coroutine _timeoutGetGameStateCoroutine;
  protected const float kSongTimeToSongStartSyncTimeOffset = -0.6f;
  protected const float kMinAnimationDurationPercentage = 0.75f;
  protected const float kGetMultiplayerGameStateTimeout = 20f;

  public MultiplayerController.State state => this._state;

  public event System.Action<MultiplayerController.State> stateChangedEvent;

  public virtual void Start()
  {
    this._startTime = Time.time;
    this._gameplayRpcManager.returnToMenuEvent += new System.Action<string>(this.HandleRpcReturnToMenu);
    this._multiplayerSessionManager.disconnectedEvent += new System.Action<DisconnectedReason>(this.HandleDisconnected);
    this.CreateAndBindPlayersSpecificSettingsAtGameStartModel();
    this.ChangeState(MultiplayerController.State.CheckingLobbyState);
    this._timeoutGetGameStateCoroutine = this.StartCoroutine(CoroutineHelpers.ExecuteAfterDelayCoroutine(new System.Action(this.HandleSceneStartSyncControllerSyncStartDidFail), 20f));
    this._menuRpcManager.setMultiplayerGameStateEvent += new System.Action<string, MultiplayerGameState>(this.HandleSetMultiplayerGameState);
    this._menuRpcManager.GetMultiplayerGameState();
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._sceneStartSyncController != (UnityEngine.Object) null)
    {
      this._sceneStartSyncController.syncStartDidSuccessEvent -= new System.Action<string>(this.HandleSceneStartSyncControllerSyncStartDidSuccess);
      this._sceneStartSyncController.syncStartDidReceiveTooLateEvent -= new System.Action<string>(this.HandleSceneStartSyncControllerSyncStartDidReceiveTooLate);
      this._sceneStartSyncController.syncStartDidFailEvent -= new System.Action(this.HandleSceneStartSyncControllerSyncStartDidFail);
    }
    if ((UnityEngine.Object) this._songStartSyncController != (UnityEngine.Object) null)
    {
      this._songStartSyncController.syncStartSuccessEvent -= new System.Action<float>(this.HandleSongStartSyncControllerSyncStartSuccess);
      this._songStartSyncController.syncStartFailedEvent -= new System.Action(this.HandleSongStartSyncControllerSyncStartFailed);
      this._songStartSyncController.syncResumeEvent -= new System.Action<float>(this.HandleSongStartSyncControllerSyncResume);
    }
    if ((UnityEngine.Object) this._playersManager != (UnityEngine.Object) null)
      this._playersManager.didSwitchPlayerToInactiveEvent -= new System.Action(this.HandleDidSwitchPlayerToInactive);
    if ((UnityEngine.Object) this._multiplayerLevelFinishedController != (UnityEngine.Object) null)
      this._multiplayerLevelFinishedController.allResultsCollectedEvent -= new System.Action<MultiplayerLevelCompletionResults, Dictionary<string, MultiplayerLevelCompletionResults>>(this.HandleAllResultsCollected);
    if (this._menuRpcManager != null)
      this._menuRpcManager.setMultiplayerGameStateEvent -= new System.Action<string, MultiplayerGameState>(this.HandleSetMultiplayerGameState);
    if (this._gameplayRpcManager != null)
      this._gameplayRpcManager.returnToMenuEvent -= new System.Action<string>(this.HandleRpcReturnToMenu);
    if (this._multiplayerSessionManager == null)
      return;
    this._multiplayerSessionManager.disconnectedEvent -= new System.Action<DisconnectedReason>(this.HandleDisconnected);
  }

  public virtual void HandleSetMultiplayerGameState(string userId, MultiplayerGameState gameState)
  {
    this._menuRpcManager.setMultiplayerGameStateEvent -= new System.Action<string, MultiplayerGameState>(this.HandleSetMultiplayerGameState);
    if (this._timeoutGetGameStateCoroutine != null)
    {
      this.StopCoroutine(this._timeoutGetGameStateCoroutine);
      this._timeoutGetGameStateCoroutine = (Coroutine) null;
    }
    if (gameState == MultiplayerGameState.Game)
    {
      this.StartSceneLoadSync();
    }
    else
    {
      this.ChangeState(MultiplayerController.State.Finished);
      this._multiplayerLevelSceneSetupData.Finish(new MultiplayerResultsData(this._sessionGameId, new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotStarted, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.ConnectedAfterLevelEnded, (LevelCompletionResults) null), (Dictionary<string, MultiplayerLevelCompletionResults>) null, this._badgesProvider, this._multiplayerSessionManager));
    }
  }

  public virtual void CreateAndBindPlayersSpecificSettingsAtGameStartModel()
  {
    PlayerSpecificSettings specificSettings = this._sceneSetupData.playerSpecificSettings;
    ColorScheme colorScheme = this._sceneSetupData.colorScheme;
    this._playersSpecificSettingsAtGameStartModel = new PlayersSpecificSettingsAtGameStartModel(this._multiplayerSessionManager, new PlayerSpecificSettingsNetSerializable(this._multiplayerSessionManager.localPlayer.userId, this._multiplayerSessionManager.localPlayer.userName, specificSettings.leftHanded, specificSettings.automaticPlayerHeight, specificSettings.playerHeight, 0.1f, colorScheme.saberAColor, colorScheme.saberBColor, colorScheme.obstaclesColor, colorScheme.environmentColor0, colorScheme.environmentColor1, colorScheme.supportsEnvironmentColorBoost ? colorScheme.environmentColor0Boost : colorScheme.environmentColor0, colorScheme.supportsEnvironmentColorBoost ? colorScheme.environmentColor1Boost : colorScheme.environmentColor1));
    this._diContainer.Bind<PlayersSpecificSettingsAtGameStartModel>().FromInstance(this._playersSpecificSettingsAtGameStartModel).AsSingle();
  }

  public virtual void StartSceneLoadSync()
  {
    this.ChangeState(MultiplayerController.State.WaitingForPlayers);
    this._sceneStartSyncController.syncStartDidSuccessEvent += new System.Action<string>(this.HandleSceneStartSyncControllerSyncStartDidSuccess);
    this._sceneStartSyncController.syncStartDidReceiveTooLateEvent += new System.Action<string>(this.HandleSceneStartSyncControllerSyncStartDidReceiveTooLate);
    this._sceneStartSyncController.syncStartDidFailEvent += new System.Action(this.HandleSceneStartSyncControllerSyncStartDidFail);
    this._sceneStartSyncController.StartSceneLoadSync(this._playersSpecificSettingsAtGameStartModel);
  }

  public virtual IEnumerator PerformSongStartSync(MultiplayerPlayerStartState localPlayerSyncState)
  {
    MultiplayerController multiplayerController = this;
    multiplayerController._localPlayerSyncStartState = localPlayerSyncState;
    // ISSUE: explicit non-virtual call
    multiplayerController.ChangeState(MultiplayerController.State.SongStartSync);
    if ((double) Time.time - (double) multiplayerController._startTime > 0.10000000149011612)
    {
      multiplayerController._fadeInOutController.FadeOut(1.3f);
      yield return (object) new WaitForSeconds(1.3f);
    }
    multiplayerController._loadingEnvironment.gameObject.SetActive(false);
    multiplayerController._playersManager.SpawnPlayers(localPlayerSyncState, (IReadOnlyList<IConnectedPlayer>) multiplayerController._playersSpecificSettingsAtGameStartModel.playersAtGameStart);
    if ((UnityEngine.Object) multiplayerController._playersManager.activeLocalPlayerFacade != (UnityEngine.Object) null)
      multiplayerController._playersManager.activeLocalPlayerFacade.PauseSpawning();
    foreach (IConnectedPlayer atGameStartPlayer in (IEnumerable<IConnectedPlayer>) multiplayerController._playersManager.allActiveAtGameStartPlayers)
    {
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (multiplayerController._playersManager.TryGetConnectedPlayerController(atGameStartPlayer.userId, out connectedPlayerController))
        connectedPlayerController.PauseSpawning();
    }
    multiplayerController._playersManager.didSwitchPlayerToInactiveEvent += new System.Action(multiplayerController.HandleDidSwitchPlayerToInactive);
    multiplayerController._introAnimationController.SetBeforeIntroValue();
    multiplayerController._fadeInOutController.FadeIn(1.3f);
    yield return (object) multiplayerController._gameScenesManager.waitUntilSceneTransitionFinish;
    yield return (object) multiplayerController._playersManager.localPlayerStartSeekSongController.songController.waitUntilIsReadyToStartTheSong;
    multiplayerController._songStartSyncController.syncStartSuccessEvent += new System.Action<float>(multiplayerController.HandleSongStartSyncControllerSyncStartSuccess);
    multiplayerController._songStartSyncController.syncStartFailedEvent += new System.Action(multiplayerController.HandleSongStartSyncControllerSyncStartFailed);
    multiplayerController._songStartSyncController.syncResumeEvent += new System.Action<float>(multiplayerController.HandleSongStartSyncControllerSyncResume);
    multiplayerController._songStartSyncController.StartSong(multiplayerController._playersSpecificSettingsAtGameStartModel, multiplayerController._sessionGameId);
  }

  public virtual void HandleDidSwitchPlayerToInactive()
  {
    if (!this._songStartSyncController.isSongStarted)
      return;
    this._playersManager.localPlayerStartSeekSongController.songController.StartSong(this.GetCurrentSongTime(this.GetSongStartSyncTime(this._songStartSyncController.songStartSyncTime)));
  }

  public virtual void HandleSceneStartSyncControllerSyncStartDidSuccess(string sessionGameId)
  {
    this._sessionGameId = sessionGameId;
    this.StartCoroutine(this.PerformSongStartSync(MultiplayerPlayerStartState.InSync));
  }

  public virtual void HandleSceneStartSyncControllerSyncStartDidReceiveTooLate(string sessionGameId)
  {
    this._sessionGameId = sessionGameId;
    this.StartCoroutine(this.PerformSongStartSync(MultiplayerPlayerStartState.Late));
  }

  public virtual void HandleSceneStartSyncControllerSyncStartDidFail()
  {
    this._playersManager.ReportLocalPlayerNetworkDidFailed(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotStarted, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.StartupFailed, (LevelCompletionResults) null));
    this.EndGameplay(this._multiplayerLevelFinishedController.localPlayerResults, this._multiplayerLevelFinishedController.otherPlayersCompletionResults);
  }

  public virtual void HandleSongStartSyncControllerSyncStartSuccess(
    float introAnimationStartSyncTime)
  {
    if (this._multiplayerLevelFinishedController.gameResultsReady)
    {
      this.EndGameplay(this._multiplayerLevelFinishedController.localPlayerResults, this._multiplayerLevelFinishedController.otherPlayersCompletionResults);
    }
    else
    {
      this._multiplayerLevelFinishedController.allResultsCollectedEvent += new System.Action<MultiplayerLevelCompletionResults, Dictionary<string, MultiplayerLevelCompletionResults>>(this.HandleAllResultsCollected);
      float maxDesiredIntroAnimationDuration = (float) ((double) this.GetSongStartSyncTime(introAnimationStartSyncTime) - (double) this._multiplayerSessionManager.syncTime - 0.60000002384185791);
      if (this._localPlayerSyncStartState == MultiplayerPlayerStartState.InSync & (double) maxDesiredIntroAnimationDuration >= (double) this._introAnimationController.GetFullIntroAnimationTime() * 0.75)
      {
        this.ChangeState(MultiplayerController.State.Intro);
        this._introAnimationController.PlayIntroAnimation(maxDesiredIntroAnimationDuration, (System.Action) (() => this.StartGameplay(introAnimationStartSyncTime)));
      }
      else
      {
        this._introAnimationController.TransitionToAfterIntroAnimationState();
        this.StartGameplay(introAnimationStartSyncTime);
      }
    }
  }

  public virtual void HandleSongStartSyncControllerSyncResume(float introAnimationStartSyncTime)
  {
    float songStartSyncTime = this.GetSongStartSyncTime(introAnimationStartSyncTime);
    this._playersManager.localPlayerStartSeekSongController.songController.SeekTo(this.GetCurrentSongTime(songStartSyncTime));
    foreach (IConnectedPlayer atGameStartPlayer in (IEnumerable<IConnectedPlayer>) this._playersManager.allActiveAtGameStartPlayers)
    {
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (!atGameStartPlayer.IsFailed() && this._playersManager.TryGetConnectedPlayerController(atGameStartPlayer.userId, out connectedPlayerController))
        connectedPlayerController.SetSongStartSyncTime(songStartSyncTime);
    }
  }

  public virtual void HandleSongStartSyncControllerSyncStartFailed()
  {
    this._playersManager.ReportLocalPlayerNetworkDidFailed(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotStarted, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.StartupFailed, (LevelCompletionResults) null));
    this.EndGameplay(this._multiplayerLevelFinishedController.localPlayerResults, this._multiplayerLevelFinishedController.otherPlayersCompletionResults);
  }

  public virtual void StartGameplay(float introAnimationStartSyncTime)
  {
    float songStartSyncTime = this.GetSongStartSyncTime(introAnimationStartSyncTime);
    this._playersManager.localPlayerStartSeekSongController.songController.StartSong(this.GetCurrentSongTime(songStartSyncTime));
    foreach (IConnectedPlayer atGameStartPlayer in (IEnumerable<IConnectedPlayer>) this._playersManager.allActiveAtGameStartPlayers)
    {
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (!atGameStartPlayer.IsFailed() && this._playersManager.TryGetConnectedPlayerController(atGameStartPlayer.userId, out connectedPlayerController))
        connectedPlayerController.SetSongStartSyncTime(songStartSyncTime);
    }
    if ((UnityEngine.Object) this._playersManager.activeLocalPlayerFacade != (UnityEngine.Object) null)
      this._playersManager.activeLocalPlayerFacade.ResumeSpawning();
    foreach (IConnectedPlayer atGameStartPlayer in (IEnumerable<IConnectedPlayer>) this._playersManager.allActiveAtGameStartPlayers)
    {
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (this._playersManager.TryGetConnectedPlayerController(atGameStartPlayer.userId, out connectedPlayerController) && !atGameStartPlayer.IsFailed())
        connectedPlayerController.ResumeSpawning();
    }
    this.ChangeState(MultiplayerController.State.Gameplay);
  }

  public virtual void HandleAllResultsCollected(
    MultiplayerLevelCompletionResults localPlayerResults,
    Dictionary<string, MultiplayerLevelCompletionResults> otherPlayerResults)
  {
    this.EndGameplay(localPlayerResults, otherPlayerResults);
  }

  public virtual void EndGameplay(
    MultiplayerLevelCompletionResults localPlayerResults,
    Dictionary<string, MultiplayerLevelCompletionResults> otherPlayerResults)
  {
    this._resultsData = new MultiplayerResultsData(this._sessionGameId, localPlayerResults, otherPlayerResults, this._badgesProvider, this._multiplayerSessionManager);
    this.ChangeState(MultiplayerController.State.Outro);
    if (((otherPlayerResults.Values.Any<MultiplayerLevelCompletionResults>((Func<MultiplayerLevelCompletionResults, bool>) (result => result.playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Cleared)) ? 1 : (localPlayerResults.playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Cleared ? 1 : 0)) & (localPlayerResults.levelCompletionResults == null || localPlayerResults.levelCompletionResults.levelEndAction != LevelCompletionResults.LevelEndAction.None ? (localPlayerResults.playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.WasInactive ? 1 : 0) : (true ? 1 : 0))) != 0)
      this._outroAnimationController.AnimateOutro(this._resultsData, new System.Action(this.HandleOutroAnimationDidFinish));
    else
      this.HandleOutroAnimationDidFinish();
  }

  public virtual void HandleOutroAnimationDidFinish()
  {
    this.ChangeState(MultiplayerController.State.Finished);
    this._multiplayerLevelSceneSetupData.Finish(this._resultsData);
  }

  public virtual void HandleRpcReturnToMenu(string userId)
  {
    this.ChangeState(MultiplayerController.State.Finished);
    this._multiplayerLevelSceneSetupData.Finish(new MultiplayerResultsData(this._sessionGameId, !((UnityEngine.Object) this._playersManager.activeLocalPlayerFacade != (UnityEngine.Object) null) ? new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotStarted, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.HostEndedLevel, (LevelCompletionResults) null) : new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.HostEndedLevel, this._playersManager.activeLocalPlayerFacade.currentLocalPlayerLevelCompletionResult), this._multiplayerLevelFinishedController.otherPlayersCompletionResults, this._badgesProvider, this._multiplayerSessionManager));
  }

  public virtual void HandleDisconnected(DisconnectedReason disconnectedReason)
  {
    this.ChangeState(MultiplayerController.State.Finished);
    this._multiplayerLevelSceneSetupData.FinishWithDisconnect(disconnectedReason);
  }

  public virtual void ChangeState(MultiplayerController.State newState)
  {
    this._state = newState;
    System.Action<MultiplayerController.State> stateChangedEvent = this.stateChangedEvent;
    if (stateChangedEvent == null)
      return;
    stateChangedEvent(newState);
  }

  public virtual float GetCurrentSongTime(float songStartSyncTime) => this._multiplayerSessionManager.syncTime - songStartSyncTime;

  public virtual float GetSongStartSyncTime(float introAnimationStartSyncTime)
  {
    float introAnimationTime = this._introAnimationController.GetFullIntroAnimationTime();
    return (float) ((double) introAnimationStartSyncTime + (double) introAnimationTime - -0.60000002384185791);
  }

  public enum State
  {
    CheckingLobbyState,
    WaitingForPlayers,
    SongStartSync,
    Intro,
    Gameplay,
    Outro,
    Finished,
  }
}
