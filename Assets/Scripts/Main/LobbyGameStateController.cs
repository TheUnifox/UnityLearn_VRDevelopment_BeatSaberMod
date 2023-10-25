// Decompiled with JetBrains decompiler
// Type: LobbyGameStateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LobbyGameStateController : ILobbyGameStateController, IDisposable
{
  public const float kShortTimerSeconds = 5f;
  public const float kLongTimerSeconds = 60f;
  [Inject]
  protected readonly ILobbyPlayersDataModel _lobbyPlayersDataModel;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly IMenuRpcManager _menuRpcManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly MultiplayerLevelLoader _multiplayerLevelLoader;
  [Inject]
  protected readonly MenuTransitionsHelper _menuTransitionsHelper;
  [Inject]
  protected readonly LobbyGameStateModel _lobbyGameStateModel;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;
  [Inject]
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;
  [Inject]
  protected readonly BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  [CompilerGenerated]
  protected bool m_ClevelStartInitiated;
  [CompilerGenerated]
  protected bool m_CcountdownStarted;
  [CompilerGenerated]
  protected float m_CcountdownEndTime;
  [CompilerGenerated]
  protected bool m_CisDisconnected;
  [CompilerGenerated]
  protected DisconnectedReason m_CdisconnectedReason = DisconnectedReason.Unknown;
  protected float _predictedStartTime;
  protected float _startTime;
  protected bool _levelStartedOnTime;
  protected MultiplayerLobbyState _state;
  protected CannotStartGameReason _cannotStartGameReason = CannotStartGameReason.NoSongSelected;
  protected readonly LevelGameplaySetupData _selectedLevelGameplaySetupData = new LevelGameplaySetupData();

  public event System.Action<ILevelGameplaySetupData> selectedLevelGameplaySetupDataChangedEvent;

  public event System.Action<ILevelGameplaySetupData> gameStartedEvent;

  public event System.Action gameStartCancelledEvent;

  public event System.Action countdownStartedEvent;

  public event System.Action countdownCancelledEvent;

  public event System.Action songStillDownloadingEvent;

  public event System.Action startTimeChangedEvent;

  public event System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> levelFinishedEvent;

  public event System.Action<DisconnectedReason> levelDidGetDisconnectedEvent;

  public event System.Action lobbyDisconnectedEvent;

  public event System.Action beforeSceneSwitchCallbackEvent;

  public event System.Action<MultiplayerLobbyState> lobbyStateChangedEvent;

  public event System.Action<CannotStartGameReason> startButtonEnabledEvent;

  public event System.Action<PlayersMissingEntitlementsNetSerializable> playerMissingEntitlementsChangedEvent;

  public float predictedCountdownEndTime => this._predictedStartTime;

  public float startTime
  {
    get => this._startTime;
    set
    {
      this._startTime = value;
      System.Action timeChangedEvent = this.startTimeChangedEvent;
      if (timeChangedEvent == null)
        return;
      timeChangedEvent();
    }
  }

  public bool levelStartInitiated
  {
    get => this.m_ClevelStartInitiated;
    private set => this.m_ClevelStartInitiated = value;
  }

  public ILevelGameplaySetupData selectedLevelGameplaySetupData => (ILevelGameplaySetupData) this._selectedLevelGameplaySetupData;

  public bool countdownStarted
  {
    get => this.m_CcountdownStarted;
    private set => this.m_CcountdownStarted = value;
  }

  public float countdownEndTime
  {
    get => this.m_CcountdownEndTime;
    private set => this.m_CcountdownEndTime = value;
  }

  public MultiplayerLobbyState state
  {
    get => this._state;
    set
    {
      if (value == this._state)
        return;
      this._state = value;
      System.Action<MultiplayerLobbyState> stateChangedEvent = this.lobbyStateChangedEvent;
      if (stateChangedEvent == null)
        return;
      stateChangedEvent(value);
    }
  }

  public CannotStartGameReason cannotStartGameReason => this._cannotStartGameReason;

  public bool isDisconnected
  {
    get => this.m_CisDisconnected;
    private set => this.m_CisDisconnected = value;
  }

  public DisconnectedReason disconnectedReason
  {
    get => this.m_CdisconnectedReason;
    private set => this.m_CdisconnectedReason = value;
  }

  public virtual void Activate()
  {
    IConnectedPlayer connectionOwner = this._multiplayerSessionManager.connectionOwner;
    if ((connectionOwner != null ? (connectionOwner.HasState("terminating") ? 1 : 0) : 0) != 0)
    {
      this.isDisconnected = true;
      this.disconnectedReason = DisconnectedReason.ServerTerminated;
    }
    else
    {
      this.state = MultiplayerLobbyState.LobbySetup;
      this._menuRpcManager.setStartGameTimeEvent += new System.Action<string, float>(this.HandleMenuRpcManagerSetStartGameTime);
      this._menuRpcManager.setIsStartButtonEnabledEvent += new System.Action<string, CannotStartGameReason>(this.HandleSetIsStartButtonEnabled);
      this._menuRpcManager.setPlayersMissingEntitlementsToLevelEvent += new System.Action<string, PlayersMissingEntitlementsNetSerializable>(this.HandleMenuRpcManagerSetPlayersMissingEntitlementsToLevel);
      this._menuRpcManager.setSelectedBeatmapEvent += new System.Action<string, BeatmapIdentifierNetSerializable>(this.HandleMenuRpcManagerSetSelectedBeatmap);
      this._menuRpcManager.setSelectedGameplayModifiersEvent += new System.Action<string, GameplayModifiers>(this.HandleMenuRpcManagerSetSelectedGameplayModifiers);
      this._menuRpcManager.clearSelectedBeatmapEvent += new System.Action<string>(this.HandleMenuRpcManagerClearSelectedBeatmap);
      this._menuRpcManager.clearSelectedGameplayModifiersEvent += new System.Action<string>(this.HandleMenuRpcManagerClearSelectedGameplayModifiers);
      this._multiplayerSessionManager.disconnectedEvent += new System.Action<DisconnectedReason>(this.HandleMultiplayerSessionManagerDisconnected);
      this._multiplayerSessionManager.connectionOwnerStateChangedEvent += new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerConnectionOwnerStateChanged);
      this._menuRpcManager.GetSelectedBeatmap();
      this._menuRpcManager.GetSelectedGameplayModifiers();
    }
  }

  public virtual void Deactivate()
  {
    this.state = MultiplayerLobbyState.None;
    this._lobbyGameStateModel.SetGameStateWithoutNotification(MultiplayerGameState.None);
    this._menuRpcManager.startedLevelEvent -= new Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(this.HandleMenuRpcManagerStartedLevel);
    this._menuRpcManager.cancelledLevelStartEvent -= new System.Action<string>(this.HandleMenuRpcManagerCancelledLevelStart);
    this._menuRpcManager.setCountdownEndTimeEvent -= new System.Action<string, float>(this.HandleMenuRpcManagerSetCountdownEndTime);
    this._menuRpcManager.setStartGameTimeEvent -= new System.Action<string, float>(this.HandleMenuRpcManagerSetStartGameTime);
    this._menuRpcManager.setIsStartButtonEnabledEvent -= new System.Action<string, CannotStartGameReason>(this.HandleSetIsStartButtonEnabled);
    this._menuRpcManager.setPlayersMissingEntitlementsToLevelEvent -= new System.Action<string, PlayersMissingEntitlementsNetSerializable>(this.HandleMenuRpcManagerSetPlayersMissingEntitlementsToLevel);
    this._menuRpcManager.setSelectedBeatmapEvent -= new System.Action<string, BeatmapIdentifierNetSerializable>(this.HandleMenuRpcManagerSetSelectedBeatmap);
    this._menuRpcManager.setSelectedGameplayModifiersEvent -= new System.Action<string, GameplayModifiers>(this.HandleMenuRpcManagerSetSelectedGameplayModifiers);
    this._menuRpcManager.clearSelectedBeatmapEvent -= new System.Action<string>(this.HandleMenuRpcManagerClearSelectedBeatmap);
    this._menuRpcManager.clearSelectedGameplayModifiersEvent -= new System.Action<string>(this.HandleMenuRpcManagerClearSelectedGameplayModifiers);
    this._multiplayerSessionManager.disconnectedEvent -= new System.Action<DisconnectedReason>(this.HandleMultiplayerSessionManagerDisconnected);
    this._multiplayerSessionManager.connectionOwnerStateChangedEvent -= new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerConnectionOwnerStateChanged);
    this.startTimeChangedEvent -= new System.Action(this.HandleStartTimeChanged);
    this.levelStartInitiated = false;
    this.countdownStarted = false;
    this._selectedLevelGameplaySetupData.ClearGameplaySetupData();
    this._startTime = 0.0f;
    this.countdownEndTime = 0.0f;
    this._multiplayerLevelLoader.ClearLoading();
    this.ClearDisconnectedState();
  }

  public virtual void Dispose() => this.Deactivate();

  public virtual void StartListeningToGameStart()
  {
    this._menuRpcManager.startedLevelEvent -= new Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(this.HandleMenuRpcManagerStartedLevel);
    this._menuRpcManager.startedLevelEvent += new Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(this.HandleMenuRpcManagerStartedLevel);
    this._menuRpcManager.cancelledLevelStartEvent -= new System.Action<string>(this.HandleMenuRpcManagerCancelledLevelStart);
    this._menuRpcManager.cancelledLevelStartEvent += new System.Action<string>(this.HandleMenuRpcManagerCancelledLevelStart);
    this._menuRpcManager.setCountdownEndTimeEvent -= new System.Action<string, float>(this.HandleMenuRpcManagerSetCountdownEndTime);
    this._menuRpcManager.setCountdownEndTimeEvent += new System.Action<string, float>(this.HandleMenuRpcManagerSetCountdownEndTime);
    this._menuRpcManager.cancelCountdownEvent -= new System.Action<string>(this.HandleMenuRpcManagerCancelCountdown);
    this._menuRpcManager.cancelCountdownEvent += new System.Action<string>(this.HandleMenuRpcManagerCancelCountdown);
    this._multiplayerSessionManager.disconnectedEvent -= new System.Action<DisconnectedReason>(this.HandleMultiplayerSessionManagerDisconnected);
    this._multiplayerSessionManager.disconnectedEvent += new System.Action<DisconnectedReason>(this.HandleMultiplayerSessionManagerDisconnected);
    this._menuRpcManager.GetCountdownEndTime();
  }

  public virtual void GetCurrentLevelIfGameStarted() => this._menuRpcManager.GetStartedLevel();

  public virtual void ClearDisconnectedState()
  {
    this.isDisconnected = false;
    this.disconnectedReason = DisconnectedReason.Unknown;
  }

  public virtual Task GetGameStateAndConfigurationAsync(CancellationToken cancellationToken)
  {
    TaskCompletionSource<bool> getGameStateAsyncTcs = new TaskCompletionSource<bool>();
    TaskCompletionSource<bool> getPlayerPermissionAsyncTcs = new TaskCompletionSource<bool>();
    this._menuRpcManager.setMultiplayerGameStateEvent += (System.Action<string, MultiplayerGameState>) ((userId1, newMultiplayerGameState1) =>
    {
      this._menuRpcManager.setMultiplayerGameStateEvent -= (System.Action<string, MultiplayerGameState>) ((userId2, newMultiplayerGameState2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      if (this._multiplayerSessionManager.connectionOwner?.userId != userId1)
        return;
      this._lobbyGameStateModel.SetGameState(newMultiplayerGameState1);
      getGameStateAsyncTcs?.TrySetResult(true);
    });
    this._menuRpcManager.GetMultiplayerGameState();
    this._menuRpcManager.setPlayersPermissionConfigurationEvent += (System.Action<string, PlayersLobbyPermissionConfigurationNetSerializable>) ((userId3, playersLobbyPermissionConfiguration1) =>
    {
      this._menuRpcManager.setPlayersPermissionConfigurationEvent -= (System.Action<string, PlayersLobbyPermissionConfigurationNetSerializable>) ((userId4, playersLobbyPermissionConfiguration2) =>
      {
        // ISSUE: unable to decompile the method.
      });
      PlayerLobbyPermissionConfigurationNetSerializable configurationNetSerializable = playersLobbyPermissionConfiguration1.playersPermission.FirstOrDefault<PlayerLobbyPermissionConfigurationNetSerializable>((Func<PlayerLobbyPermissionConfigurationNetSerializable, bool>) (p => p.userId == this._lobbyPlayersDataModel.localUserId));
      if (configurationNetSerializable != null)
        this._lobbyPlayerPermissionsModel.SetPlayerPermissions(configurationNetSerializable.isServerOwner, configurationNetSerializable.hasRecommendBeatmapsPermission, configurationNetSerializable.hasRecommendGameplayModifiersPermission, configurationNetSerializable.hasKickVotePermission, configurationNetSerializable.hasInvitePermission);
      getPlayerPermissionAsyncTcs?.TrySetResult(true);
    });
    this._menuRpcManager.GetPlayersPermissionConfiguration();
    return (Task) Task.WhenAll<bool>(getGameStateAsyncTcs.Task.WithCancellation<bool>(cancellationToken), getPlayerPermissionAsyncTcs.Task.WithCancellation<bool>(cancellationToken));
  }

  public virtual void PredictCountdownEndTime() => this._predictedStartTime = this._multiplayerSessionManager.syncTime + (this._lobbyPlayersDataModel.All<KeyValuePair<string, ILobbyPlayerData>>((Func<KeyValuePair<string, ILobbyPlayerData>, bool>) (pair => pair.Value.isReady || pair.Value.isPartyOwner)) ? 5f : 60f);

  public virtual bool IsCloseToStartGame() => (double) Mathf.Abs(this._predictedStartTime - this._multiplayerSessionManager.syncTime) <= 5.0;

  public virtual void HandleMultiplayerSessionManagerDisconnected(
    DisconnectedReason disconnectedReason)
  {
    this.isDisconnected = true;
    this.disconnectedReason = disconnectedReason;
    System.Action disconnectedEvent = this.lobbyDisconnectedEvent;
    if (disconnectedEvent == null)
      return;
    disconnectedEvent();
  }

  public virtual void HandleMultiplayerSessionManagerConnectionOwnerStateChanged(
    IConnectedPlayer connectedPlayer)
  {
    if (!connectedPlayer.HasState("terminating"))
      return;
    this.isDisconnected = true;
    this.disconnectedReason = DisconnectedReason.ServerTerminated;
    System.Action disconnectedEvent = this.lobbyDisconnectedEvent;
    if (disconnectedEvent == null)
      return;
    disconnectedEvent();
  }

  public virtual void StopListeningToGameStart()
  {
    this._menuRpcManager.startedLevelEvent -= new Action<string, BeatmapIdentifierNetSerializable, GameplayModifiers, float>(this.HandleMenuRpcManagerStartedLevel);
    this._menuRpcManager.setCountdownEndTimeEvent -= new System.Action<string, float>(this.HandleMenuRpcManagerSetCountdownEndTime);
    this._multiplayerSessionManager.disconnectedEvent -= new System.Action<DisconnectedReason>(this.HandleMultiplayerSessionManagerDisconnected);
  }

  public virtual void HandleMenuRpcManagerStartedLevel(
    string userId,
    BeatmapIdentifierNetSerializable beatmapId,
    GameplayModifiers gameplayModifiers,
    float startTime)
  {
    if (this.levelStartInitiated)
      return;
    this.levelStartInitiated = true;
    this.state = MultiplayerLobbyState.GameStarting;
    this._multiplayerLevelLoader.countdownFinishedEvent -= new System.Action<ILevelGameplaySetupData, IDifficultyBeatmap>(this.HandleMultiplayerLevelLoaderCountdownFinished);
    this._multiplayerLevelLoader.countdownFinishedEvent += new System.Action<ILevelGameplaySetupData, IDifficultyBeatmap>(this.HandleMultiplayerLevelLoaderCountdownFinished);
    this._multiplayerLevelLoader.stillDownloadingSongEvent -= new System.Action(this.HandleMultiplayerLevelLoaderStillDownloadingSong);
    this._multiplayerLevelLoader.stillDownloadingSongEvent += new System.Action(this.HandleMultiplayerLevelLoaderStillDownloadingSong);
    LevelGameplaySetupData gameplaySetupData = new LevelGameplaySetupData(beatmapId.ToPreviewDifficultyBeatmap(this._beatmapLevelsModel, this._beatmapCharacteristicCollection), gameplayModifiers);
    this.startTime = startTime;
    this._multiplayerLevelLoader.LoadLevel((ILevelGameplaySetupData) gameplaySetupData, startTime);
    this._levelStartedOnTime = this._lobbyGameStateModel.gameState == MultiplayerGameState.Lobby;
    if (!this._levelStartedOnTime)
      return;
    System.Action<ILevelGameplaySetupData> gameStartedEvent = this.gameStartedEvent;
    if (gameStartedEvent == null)
      return;
    gameStartedEvent((ILevelGameplaySetupData) gameplaySetupData);
  }

  public virtual void HandleMenuRpcManagerCancelledLevelStart(string userId)
  {
    if (!this.levelStartInitiated)
      return;
    this.levelStartInitiated = false;
    this.state = MultiplayerLobbyState.LobbySetup;
    this.StopLoading();
    if (this._lobbyGameStateModel.gameState != MultiplayerGameState.Lobby)
      return;
    System.Action startCancelledEvent = this.gameStartCancelledEvent;
    if (startCancelledEvent == null)
      return;
    startCancelledEvent();
  }

  public virtual void HandleMenuRpcManagerSetCountdownEndTime(string userId, float countdownTime)
  {
    if (this.countdownStarted)
      return;
    this.countdownStarted = true;
    this.state = MultiplayerLobbyState.LobbyCountdown;
    this._menuRpcManager.setCountdownEndTimeEvent -= new System.Action<string, float>(this.HandleMenuRpcManagerSetCountdownEndTime);
    this._menuRpcManager.cancelCountdownEvent += new System.Action<string>(this.HandleMenuRpcManagerCancelCountdown);
    this.countdownEndTime = countdownTime;
    if (this._lobbyGameStateModel.gameState != MultiplayerGameState.Lobby)
      return;
    System.Action countdownStartedEvent = this.countdownStartedEvent;
    if (countdownStartedEvent == null)
      return;
    countdownStartedEvent();
  }

  public virtual void HandleMenuRpcManagerCancelCountdown(string userId)
  {
    if (!this.countdownStarted)
      return;
    this.countdownStarted = false;
    this.state = MultiplayerLobbyState.LobbySetup;
    this._menuRpcManager.setCountdownEndTimeEvent += new System.Action<string, float>(this.HandleMenuRpcManagerSetCountdownEndTime);
    this._menuRpcManager.cancelCountdownEvent -= new System.Action<string>(this.HandleMenuRpcManagerCancelCountdown);
    this.countdownEndTime = float.MaxValue;
    if (this._lobbyGameStateModel.gameState != MultiplayerGameState.Lobby)
      return;
    System.Action countdownCancelledEvent = this.countdownCancelledEvent;
    if (countdownCancelledEvent == null)
      return;
    countdownCancelledEvent();
  }

  public virtual void HandleMenuRpcManagerSetStartGameTime(string userId, float startTime)
  {
    this.startTime = startTime;
    this._multiplayerLevelLoader.SetNewStartTime(startTime);
  }

  public virtual void HandleSetIsStartButtonEnabled(
    string userId,
    CannotStartGameReason cannotStartGameReason)
  {
    this._cannotStartGameReason = cannotStartGameReason;
    System.Action<CannotStartGameReason> buttonEnabledEvent = this.startButtonEnabledEvent;
    if (buttonEnabledEvent == null)
      return;
    buttonEnabledEvent(cannotStartGameReason);
  }

  public virtual void HandleMenuRpcManagerSetPlayersMissingEntitlementsToLevel(
    string userId,
    PlayersMissingEntitlementsNetSerializable playersMissingEntitlements)
  {
    System.Action<PlayersMissingEntitlementsNetSerializable> entitlementsChangedEvent = this.playerMissingEntitlementsChangedEvent;
    if (entitlementsChangedEvent == null)
      return;
    entitlementsChangedEvent(playersMissingEntitlements);
  }

  public virtual void HandleStartTimeChanged() => this._multiplayerLevelLoader.SetNewStartTime(this.startTime);

  public virtual void HandleMultiplayerLevelLoaderStillDownloadingSong()
  {
    System.Action downloadingEvent = this.songStillDownloadingEvent;
    if (downloadingEvent == null)
      return;
    downloadingEvent();
  }

  public virtual void HandleMenuRpcManagerSetSelectedBeatmap(
    string userId,
    BeatmapIdentifierNetSerializable beatmapId)
  {
    this._selectedLevelGameplaySetupData.SetBeatmapLevel(beatmapId.ToPreviewDifficultyBeatmap(this._beatmapLevelsModel, this._beatmapCharacteristicCollection));
    System.Action<ILevelGameplaySetupData> dataChangedEvent = this.selectedLevelGameplaySetupDataChangedEvent;
    if (dataChangedEvent == null)
      return;
    dataChangedEvent((ILevelGameplaySetupData) this._selectedLevelGameplaySetupData);
  }

  public virtual void HandleMenuRpcManagerSetSelectedGameplayModifiers(
    string userId,
    GameplayModifiers modifiers)
  {
    this._selectedLevelGameplaySetupData.SetGameplayModifiers(modifiers);
    System.Action<ILevelGameplaySetupData> dataChangedEvent = this.selectedLevelGameplaySetupDataChangedEvent;
    if (dataChangedEvent == null)
      return;
    dataChangedEvent((ILevelGameplaySetupData) this._selectedLevelGameplaySetupData);
  }

  public virtual void HandleMenuRpcManagerClearSelectedBeatmap(string userId)
  {
    this._selectedLevelGameplaySetupData.SetBeatmapLevel((PreviewDifficultyBeatmap) null);
    System.Action<ILevelGameplaySetupData> dataChangedEvent = this.selectedLevelGameplaySetupDataChangedEvent;
    if (dataChangedEvent == null)
      return;
    dataChangedEvent((ILevelGameplaySetupData) this._selectedLevelGameplaySetupData);
  }

  public virtual void HandleMenuRpcManagerClearSelectedGameplayModifiers(string userId)
  {
    this._selectedLevelGameplaySetupData.SetGameplayModifiers((GameplayModifiers) null);
    System.Action<ILevelGameplaySetupData> dataChangedEvent = this.selectedLevelGameplaySetupDataChangedEvent;
    if (dataChangedEvent == null)
      return;
    dataChangedEvent((ILevelGameplaySetupData) this._selectedLevelGameplaySetupData);
  }

  public virtual void HandleMultiplayerLevelLoaderCountdownFinished(
    ILevelGameplaySetupData gameplaySetupData,
    IDifficultyBeatmap difficultyBeatmap)
  {
    this.state = MultiplayerLobbyState.GameRunning;
    this._lobbyGameStateModel.SetGameState(MultiplayerGameState.Game);
    this.StopLoading();
    bool hasState = this._levelStartedOnTime && difficultyBeatmap != null && this._multiplayerSessionManager.localPlayer.WantsToPlayNextLevel();
    this._multiplayerSessionManager.SetLocalPlayerState("was_active_at_level_start", hasState);
    this._multiplayerSessionManager.SetLocalPlayerState("is_active", hasState);
    this._multiplayerSessionManager.SetLocalPlayerState("finished_level", false);
    this._lobbyPlayersDataModel.SetLocalPlayerIsInLobby(false);
    this.StartMultiplayerLevel(gameplaySetupData, difficultyBeatmap, this.beforeSceneSwitchCallbackEvent);
  }

  public virtual void StartMultiplayerLevel(
    ILevelGameplaySetupData gameplaySetupData,
    IDifficultyBeatmap difficultyBeatmap,
    System.Action beforeSceneSwitchCallback)
  {
    this.countdownStarted = false;
    this.StopListeningToGameStart();
    this._menuTransitionsHelper.StartMultiplayerLevel("Multiplayer", gameplaySetupData.beatmapLevel.beatmapLevel, gameplaySetupData.beatmapLevel.beatmapDifficulty, gameplaySetupData.beatmapLevel.beatmapCharacteristic, difficultyBeatmap, this._playerDataModel.playerData.colorSchemesSettings.GetOverrideColorScheme(), gameplaySetupData.gameplayModifiers, this._playerDataModel.playerData.playerSpecificSettings, (PracticeSettings) null, Localization.Get("BUTTON_MENU"), false, beforeSceneSwitchCallback, new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish), new System.Action<DisconnectedReason>(this.HandleMultiplayerLevelDidDisconnect));
  }

  public virtual void HandleMultiplayerLevelDidFinish(
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData,
    MultiplayerResultsData multiplayerResultsData)
  {
    this.levelStartInitiated = false;
    this.state = MultiplayerLobbyState.LobbySetup;
    this._lobbyGameStateModel.SetGameState(MultiplayerGameState.Lobby);
    System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> levelFinishedEvent = this.levelFinishedEvent;
    if (levelFinishedEvent == null)
      return;
    levelFinishedEvent(multiplayerLevelScenesTransitionSetupData, multiplayerResultsData);
  }

  public virtual void HandleMultiplayerLevelDidDisconnect(DisconnectedReason disconnectedReason)
  {
    this.levelStartInitiated = false;
    this.state = MultiplayerLobbyState.LobbySetup;
    this._lobbyGameStateModel.SetGameState(MultiplayerGameState.Lobby);
    System.Action<DisconnectedReason> disconnectedEvent = this.levelDidGetDisconnectedEvent;
    if (disconnectedEvent == null)
      return;
    disconnectedEvent(disconnectedReason);
  }

  public virtual void StopLoading()
  {
    this._multiplayerLevelLoader.ClearLoading();
    this._multiplayerLevelLoader.countdownFinishedEvent -= new System.Action<ILevelGameplaySetupData, IDifficultyBeatmap>(this.HandleMultiplayerLevelLoaderCountdownFinished);
    this._multiplayerLevelLoader.stillDownloadingSongEvent -= new System.Action(this.HandleMultiplayerLevelLoaderStillDownloadingSong);
  }
}
