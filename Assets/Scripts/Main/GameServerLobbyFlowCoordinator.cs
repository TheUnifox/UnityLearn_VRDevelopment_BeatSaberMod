// Decompiled with JetBrains decompiler
// Type: GameServerLobbyFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class GameServerLobbyFlowCoordinator : FlowCoordinator
{
  [LocalizationKey]
  protected const string kPlayersMissingEntitlementKey = "LABEL_PLAYERS_MISSING_ENTITLEMENT";
  protected const float kMaxPredictedStartTimeDifference = 1.5f;
  [SerializeField]
  protected ScreenModeSO _screenMode;
  [SerializeField]
  protected AudioClip _ambienceAudioClip;
  [Inject]
  protected readonly ServerPlayerListViewController _serverPlayerListViewController;
  [Inject]
  protected readonly SelectModifiersViewController _selectModifiersViewController;
  [Inject]
  protected readonly MultiplayerLevelSelectionFlowCoordinator _multiplayerLevelSelectionFlowCoordinator;
  [Inject]
  protected readonly MultiplayerResultsViewController _multiplayerResultsViewController;
  [Inject]
  protected readonly SimpleDialogPromptViewController _simpleDialogPromptViewController;
  [Inject]
  protected readonly ConnectionErrorDialogViewController _connectionErrorDialogViewController;
  [Inject]
  protected readonly MultiplayerSettingsPanelController _multiplayerSettingsPanelController;
  [Inject]
  protected readonly GameplaySetupViewController _gameplaySetupViewController;
  [Inject]
  protected readonly MultiplayerLobbyController _multiplayerLobbyController;
  [Inject]
  protected readonly FadeInOutController _fadeInOutController;
  [Inject]
  protected readonly CenterStageScreenController _centerStageScreenController;
  [Inject]
  protected readonly ILobbyStateDataModel _lobbyStateDataModel;
  [Inject]
  protected readonly LobbyGameStateModel _lobbyGameStateModel;
  [Inject]
  protected readonly ILobbyPlayersDataModel _lobbyPlayersDataModel;
  [Inject]
  protected readonly ILobbyGameStateController _lobbyGameStateController;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;
  [Inject]
  protected readonly LobbySetupViewController _lobbySetupViewController;
  [Inject]
  protected readonly IUnifiedNetworkPlayerModel _unifiedNetworkPlayerModel;
  [Inject]
  protected readonly ScreenModeController _screenModeController;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly PlatformLeaderboardsModel _platformLeaderboardsModel;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  protected LevelSelectionFlowCoordinator.State _lastSimpleLevelSelectionFlowCoordinatorState;
  protected bool _isAlreadyFinishing;
  protected readonly StringBuilder _stringBuilder = new StringBuilder();
  protected CancellationTokenSource _canStartGameCts;
  [CompilerGenerated]
  protected bool m_CrejoinQuickPlay;

  private bool isPartyOwner => this._lobbyPlayerPermissionsModel.isPartyOwner;

  private bool isPublicGame => this._lobbyStateDataModel.configuration.discoveryPolicy == DiscoveryPolicy.Public;

  private bool isManaged => this._lobbyStateDataModel.configuration.gameplayServerMode == GameplayServerMode.Managed;

  private bool isQuickStartServer => this._lobbyStateDataModel.configuration.gameplayServerMode == GameplayServerMode.QuickStartOneSong;

  private bool isQuickPlayServer => this._lobbyStateDataModel.configuration.gameplayServerMode == GameplayServerMode.Countdown;

  public event System.Action willFinishEvent;

  public event System.Action didFinishEvent;

  public event System.Action startGameOrReadyEvent;

  public event System.Action didSetupEvent;

  public event System.Action didOpenInvitePanelEvent;

  public bool rejoinQuickPlay
  {
    get => this.m_CrejoinQuickPlay;
    private set => this.m_CrejoinQuickPlay = value;
  }

  public virtual GameServerLobbyFlowCoordinator.LobbyType GetLobbyType()
  {
    if (this.isPartyOwner)
      return GameServerLobbyFlowCoordinator.LobbyType.HostSetup;
    if (this.isManaged)
      return GameServerLobbyFlowCoordinator.LobbyType.ClientSetup;
    return this.isPublicGame ? GameServerLobbyFlowCoordinator.LobbyType.QuickPlayLobby : GameServerLobbyFlowCoordinator.LobbyType.Party;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.showBackButton = true;
    this._isAlreadyFinishing = false;
    this.rejoinQuickPlay = false;
    this._songPreviewPlayer.CrossfadeToNewDefault(this._ambienceAudioClip);
    if (addedToHierarchy | screenSystemEnabling)
      this._lobbyGameStateController.lobbyDisconnectedEvent += new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
    bool hide = (this._unifiedNetworkPlayerModel.configuration.gameplayServerControlSettings & GameplayServerControlSettings.AllowSpectate) == GameplayServerControlSettings.None;
    bool flag = this._unifiedNetworkPlayerModel.configuration.discoveryPolicy == DiscoveryPolicy.Hidden || this._unifiedNetworkPlayerModel.configuration.invitePolicy == InvitePolicy.NobodyCanInvite;
    if (addedToHierarchy)
    {
      this._lastSimpleLevelSelectionFlowCoordinatorState = (LevelSelectionFlowCoordinator.State) null;
      this._gameplaySetupViewController.Setup(false, false, true, !hide || !flag, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Multiplayer);
      this._centerStageScreenController.Setup(this._lobbyPlayerPermissionsModel.hasRecommendModifiersPermission);
      this._screenModeController.SetMode(this._screenMode.data);
      this._multiplayerSettingsPanelController.playerActiveStateChangedEvent += new System.Action<bool>(this.HandleMultiplayerSettingsPanelControllerPlayerActiveStateChanged);
      this._multiplayerResultsViewController.backToLobbyPressedEvent += new System.Action<MultiplayerResultsViewController>(this.HandleMultiplayerResultsViewControllerBackToLobbyPressed);
      this._multiplayerResultsViewController.backToMenuPressedEvent += new System.Action<MultiplayerResultsViewController>(this.HandleMultiplayerResultsViewControllerBackToMenuPressed);
      this._lobbyPlayersDataModel.didChangeEvent += new System.Action<string>(this.HandleLobbyPlayersDataModelDidChange);
      this._lobbySetupViewController.selectBeatmapEvent += new System.Action(this.HandleLobbySetupViewControllerSelectBeatmap);
      this._lobbySetupViewController.selectModifiersEvent += new System.Action(this.HandleLobbySetupViewControllerSelectModifiers);
      this._lobbySetupViewController.startGameOrReadyEvent += new System.Action(this.HandleLobbySetupViewControllerStartGameOrReady);
      this._lobbySetupViewController.cancelGameOrUnreadyEvent += new System.Action(this.HandleLobbySetupViewControllerCancelGameOrUnready);
      this._lobbySetupViewController.clearSuggestedBeatmapEvent += new System.Action(this.HandleLobbySetupViewControllerClearSelectedBeatmap);
      this._lobbySetupViewController.clearSuggestedModifiersEvent += new System.Action(this.HandleLobbySetupViewControllerClearSelectedModifiers);
      this._multiplayerLevelSelectionFlowCoordinator.didSelectLevelEvent += new System.Action<LevelSelectionFlowCoordinator.State>(this.HandleMultiplayerLevelSelectionFlowCoordinatorDidSelectLevel);
      this._multiplayerLevelSelectionFlowCoordinator.didFinishedEvent += new System.Action(this.HandleMultiplayerLevelSelectionFlowCoordinatorCancelSelectLevel);
      this._serverPlayerListViewController.selectSuggestedBeatmapEvent += new System.Action<PreviewDifficultyBeatmap>(this.HandleServerPlayerListViewControllerSelectSuggestedBeatmap);
      this._serverPlayerListViewController.selectSuggestedGameplayModifiersEvent += new System.Action<GameplayModifiers>(this.HandleServerPlayerListViewControllerSelectSuggestedGameplayModifiers);
      this._serverPlayerListViewController.kickPlayerEvent += new System.Action<string>(this.HandleServerPlayerListViewControllerKickPlayer);
      this._serverPlayerListViewController.didOpenInvitePanelEvent += new System.Action(this.HandleServerPlayerListViewControllerDidOpenInvitePanel);
      this._lobbyGameStateController.levelFinishedEvent += new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleLobbyGameStateControllerLevelFinished);
      this._lobbyGameStateController.levelDidGetDisconnectedEvent += new System.Action<DisconnectedReason>(this.HandleLobbyGameStateControllerLevelDidGetDisconnected);
      this._lobbyGameStateController.beforeSceneSwitchCallbackEvent += new System.Action(this.HandleLobbyGameBeforeSceneSwitchCallback);
      this._lobbyGameStateController.startButtonEnabledEvent += new System.Action<CannotStartGameReason>(this.HandleLobbyGameStateStartButtonEnabled);
      this._lobbyGameStateController.countdownStartedEvent += new System.Action(this.HandleLobbyGameStateControllerCountdownStarted);
      this._lobbyGameStateController.countdownCancelledEvent += new System.Action(this.HandleLobbyGameStateControllerCountdownCancelled);
      this._lobbyGameStateController.startTimeChangedEvent += new System.Action(this.HandleLobbyGameStateControllerStartTimeChanged);
      this._lobbyGameStateController.gameStartedEvent += new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerGameStarted);
      this._lobbyGameStateController.gameStartCancelledEvent += new System.Action(this.HandleLobbyGameStateControllerCancelStartTime);
      this._lobbyGameStateController.lobbyStateChangedEvent += new System.Action<MultiplayerLobbyState>(this.HandleLobbyGameStateControllerGameStateChanged);
      this._lobbyGameStateController.playerMissingEntitlementsChangedEvent += new System.Action<PlayersMissingEntitlementsNetSerializable>(this.HandleMenuRpcManagerSetPlayersMissingEntitlementsToLevel);
      this._lobbyPlayerPermissionsModel.permissionsChangedEvent += new System.Action(this.HandleLobbyPlayerPermissionsModelPermissionsChanged);
      this.ProvideInitialViewControllers((ViewController) this._lobbySetupViewController);
      System.Action didSetupEvent = this.didSetupEvent;
      if (didSetupEvent != null)
        didSetupEvent();
    }
    this.SetLobbyPlayerDataToViews(this._lobbyPlayersDataModel.localUserId);
    this._multiplayerSettingsPanelController.HideSpectateSettings(hide);
    this._lobbySetupViewController.SetLobbyState(this._lobbyGameStateController.state);
    this.SetupLobbyWithPermissions();
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (removedFromHierarchy | screenSystemDisabling)
      this._lobbyGameStateController.lobbyDisconnectedEvent -= new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
    if (!removedFromHierarchy)
      return;
    this._screenModeController.SetDefaultMode();
    this._multiplayerSettingsPanelController.playerActiveStateChangedEvent -= new System.Action<bool>(this.HandleMultiplayerSettingsPanelControllerPlayerActiveStateChanged);
    this._multiplayerResultsViewController.backToLobbyPressedEvent -= new System.Action<MultiplayerResultsViewController>(this.HandleMultiplayerResultsViewControllerBackToLobbyPressed);
    this._multiplayerResultsViewController.backToMenuPressedEvent -= new System.Action<MultiplayerResultsViewController>(this.HandleMultiplayerResultsViewControllerBackToMenuPressed);
    this._multiplayerLobbyController.DeactivateMultiplayerLobby();
    this._lobbyPlayersDataModel.didChangeEvent -= new System.Action<string>(this.HandleLobbyPlayersDataModelDidChange);
    this._lobbySetupViewController.selectBeatmapEvent -= new System.Action(this.HandleLobbySetupViewControllerSelectBeatmap);
    this._lobbySetupViewController.selectModifiersEvent -= new System.Action(this.HandleLobbySetupViewControllerSelectModifiers);
    this._lobbySetupViewController.startGameOrReadyEvent -= new System.Action(this.HandleLobbySetupViewControllerStartGameOrReady);
    this._lobbySetupViewController.cancelGameOrUnreadyEvent -= new System.Action(this.HandleLobbySetupViewControllerCancelGameOrUnready);
    this._lobbySetupViewController.clearSuggestedBeatmapEvent -= new System.Action(this.HandleLobbySetupViewControllerClearSelectedBeatmap);
    this._lobbySetupViewController.clearSuggestedModifiersEvent -= new System.Action(this.HandleLobbySetupViewControllerClearSelectedModifiers);
    this._multiplayerLevelSelectionFlowCoordinator.didSelectLevelEvent -= new System.Action<LevelSelectionFlowCoordinator.State>(this.HandleMultiplayerLevelSelectionFlowCoordinatorDidSelectLevel);
    this._multiplayerLevelSelectionFlowCoordinator.didFinishedEvent -= new System.Action(this.HandleMultiplayerLevelSelectionFlowCoordinatorCancelSelectLevel);
    this._serverPlayerListViewController.selectSuggestedBeatmapEvent -= new System.Action<PreviewDifficultyBeatmap>(this.HandleServerPlayerListViewControllerSelectSuggestedBeatmap);
    this._serverPlayerListViewController.selectSuggestedGameplayModifiersEvent -= new System.Action<GameplayModifiers>(this.HandleServerPlayerListViewControllerSelectSuggestedGameplayModifiers);
    this._serverPlayerListViewController.kickPlayerEvent -= new System.Action<string>(this.HandleServerPlayerListViewControllerKickPlayer);
    this._serverPlayerListViewController.didOpenInvitePanelEvent -= new System.Action(this.HandleServerPlayerListViewControllerDidOpenInvitePanel);
    this._lobbyGameStateController.levelFinishedEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleLobbyGameStateControllerLevelFinished);
    this._lobbyGameStateController.levelDidGetDisconnectedEvent -= new System.Action<DisconnectedReason>(this.HandleLobbyGameStateControllerLevelDidGetDisconnected);
    this._lobbyGameStateController.beforeSceneSwitchCallbackEvent -= new System.Action(this.HandleLobbyGameBeforeSceneSwitchCallback);
    this._lobbyGameStateController.startButtonEnabledEvent -= new System.Action<CannotStartGameReason>(this.HandleLobbyGameStateStartButtonEnabled);
    this._lobbyGameStateController.countdownStartedEvent -= new System.Action(this.HandleLobbyGameStateControllerCountdownStarted);
    this._lobbyGameStateController.countdownCancelledEvent -= new System.Action(this.HandleLobbyGameStateControllerCountdownCancelled);
    this._lobbyGameStateController.startTimeChangedEvent -= new System.Action(this.HandleLobbyGameStateControllerStartTimeChanged);
    this._lobbyGameStateController.gameStartedEvent -= new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerGameStarted);
    this._lobbyGameStateController.gameStartCancelledEvent -= new System.Action(this.HandleLobbyGameStateControllerCancelStartTime);
    this._lobbyGameStateController.lobbyStateChangedEvent -= new System.Action<MultiplayerLobbyState>(this.HandleLobbyGameStateControllerGameStateChanged);
    this._lobbyGameStateController.playerMissingEntitlementsChangedEvent -= new System.Action<PlayersMissingEntitlementsNetSerializable>(this.HandleMenuRpcManagerSetPlayersMissingEntitlementsToLevel);
    this._lobbyPlayerPermissionsModel.permissionsChangedEvent -= new System.Action(this.HandleLobbyPlayerPermissionsModelPermissionsChanged);
  }

  protected override void InitialViewControllerWasPresented()
  {
    if (!this._lobbyStateDataModel.isConnected)
    {
      System.Action didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent();
    }
    else
      this.GetInitialGameState();
  }

  protected override void TransitionDidStart()
  {
    base.TransitionDidStart();
    if (!this.IsFlowCoordinatorInHierarchy((FlowCoordinator) this))
      return;
    this._lobbyGameStateController.lobbyDisconnectedEvent -= new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
    this._lobbyGameStateController.gameStartedEvent -= new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerGameStartedPresentView);
  }

  protected override void TransitionDidFinish()
  {
    base.TransitionDidFinish();
    if (!this.IsFlowCoordinatorInHierarchy((FlowCoordinator) this) || (UnityEngine.Object) this.childFlowCoordinator != (UnityEngine.Object) null)
      return;
    if (!this._isAlreadyFinishing && (UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._connectionErrorDialogViewController && (UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._multiplayerResultsViewController && this._lobbyGameStateController.isDisconnected)
    {
      this.ShowDisconnectDialogAndFinish(this._lobbyGameStateController.disconnectedReason);
    }
    else
    {
      if (!this._isAlreadyFinishing && (UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._simpleDialogPromptViewController && (UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._multiplayerResultsViewController)
        this._lobbyGameStateController.lobbyDisconnectedEvent += new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
      if ((UnityEngine.Object) this.topViewController == (UnityEngine.Object) this._selectModifiersViewController && this._lobbyGameStateController.state == MultiplayerLobbyState.GameStarting)
        this.DismissViewController((ViewController) this._selectModifiersViewController);
      else if ((UnityEngine.Object) this.topViewController == (UnityEngine.Object) this._multiplayerResultsViewController && this._lobbyGameStateController.state == MultiplayerLobbyState.GameStarting)
        this.DismissViewController(this.topViewController);
      else
        this._lobbyGameStateController.gameStartedEvent += new System.Action<ILevelGameplaySetupData>(this.HandleLobbyGameStateControllerGameStartedPresentView);
    }
  }

  protected override void TopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    this.ShowSideViewControllers((UnityEngine.Object) newViewController == (UnityEngine.Object) this._lobbySetupViewController, animationType);
    this.ShowBackButton((UnityEngine.Object) newViewController == (UnityEngine.Object) this._lobbySetupViewController);
    this.SetTitle(newViewController, animationType);
  }

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    if ((UnityEngine.Object) topViewController == (UnityEngine.Object) this._selectModifiersViewController)
    {
      this._lobbyPlayersDataModel.SetLocalPlayerGameplayModifiers(this._selectModifiersViewController.gameplayModifiers);
      this.DismissViewController((ViewController) this._selectModifiersViewController);
    }
    else if ((UnityEngine.Object) topViewController != (UnityEngine.Object) this._lobbySetupViewController)
      this.DismissViewController(topViewController);
    else
      this.PresentBackButtonConfirmationDialog();
  }

  public virtual void PresentBackButtonConfirmationDialog()
  {
    this._lobbyGameStateController.lobbyDisconnectedEvent -= new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
    this.SetTitle("");
    this.showBackButton = false;
    this._simpleDialogPromptViewController.Init(Localization.Get("TITLE_QUIT_LOBBY_CONFIRMATION"), Localization.Get("TEXT_QUIT_LOBBY_CONFIRMATION"), Localization.Get("BUTTON_CANCEL"), Localization.Get("BUTTON_OK"), (System.Action<int>) (btnIndex =>
    {
      if (btnIndex != 0)
      {
        if (btnIndex != 1)
          return;
        this._isAlreadyFinishing = true;
        this.Finish();
      }
      else
      {
        this._lobbyGameStateController.lobbyDisconnectedEvent += new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
        this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);
      }
    }));
    this.PresentViewController((ViewController) this._simpleDialogPromptViewController, animationDirection: ViewController.AnimationDirection.Vertical);
  }

  public virtual void Finish(System.Action finishedCallback = null, bool withFadeOut = true)
  {
    System.Action willFinishEvent = this.willFinishEvent;
    if (willFinishEvent != null)
      willFinishEvent();
    this._lobbyGameStateController.ClearDisconnectedState();
    if (withFadeOut)
    {
      EventSystem eventSystem = EventSystem.current;
      eventSystem.enabled = false;
      this._fadeInOutController.FadeOut((System.Action) (() =>
      {
        this.DismissViewControllersAndCoordinators();
        eventSystem.enabled = true;
        System.Action action = finishedCallback;
        if (action != null)
          action();
        System.Action didFinishEvent = this.didFinishEvent;
        if (didFinishEvent == null)
          return;
        didFinishEvent();
      }));
    }
    else
    {
      this.DismissViewControllersAndCoordinators();
      System.Action action = finishedCallback;
      if (action != null)
        action();
      System.Action didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent();
    }
  }

  public virtual void GetInitialGameState()
  {
    this._lobbyGameStateController.StartListeningToGameStart();
    this._lobbyGameStateController.GetCurrentLevelIfGameStarted();
    if (this._lobbyGameStateModel.gameState != MultiplayerGameState.Lobby)
      return;
    this._multiplayerLobbyController.ActivateMultiplayerLobby();
    this._fadeInOutController.FadeIn();
  }

  public virtual void HandleLobbyPlayersDataModelDidChange(string userId) => this.SetLobbyPlayerDataToViews(userId);

  public virtual void HandleServerPlayerListViewControllerDidOpenInvitePanel()
  {
    System.Action invitePanelEvent = this.didOpenInvitePanelEvent;
    if (invitePanelEvent == null)
      return;
    invitePanelEvent();
  }

  public virtual void HandleLobbySetupViewControllerSelectBeatmap()
  {
    this._multiplayerLevelSelectionFlowCoordinator.Setup(this._lastSimpleLevelSelectionFlowCoordinatorState, this._unifiedNetworkPlayerModel.selectionMask.songPacks, this._unifiedNetworkPlayerModel.selectionMask.difficulties, Localization.Get("BUTTON_SELECT"), Localization.Get("TITLE_SELECT_LEVEL"));
    this.PresentFlowCoordinator((FlowCoordinator) this._multiplayerLevelSelectionFlowCoordinator);
  }

  public virtual void HandleMultiplayerSettingsPanelControllerPlayerActiveStateChanged(bool isActive)
  {
    this._lobbySetupViewController.SetPlayerActiveState(isActive);
    bool isReady = this._lobbyPlayersDataModel[this._lobbyPlayersDataModel.localUserId].isReady;
    if (!isActive & isReady)
      this._lobbyPlayersDataModel.SetLocalPlayerIsReady(true);
    this.UpdateLocalPlayerIsActiveState(isActive);
  }

  public virtual void HandleLobbyGameStateControllerLobbyDisconnected() => this.ShowDisconnectDialogAndFinish(this._lobbyGameStateController.disconnectedReason);

  public virtual void HandleMultiplayerLevelSelectionFlowCoordinatorDidSelectLevel(
    LevelSelectionFlowCoordinator.State state)
  {
    this._lastSimpleLevelSelectionFlowCoordinatorState = state;
    this._lobbyPlayersDataModel.SetLocalPlayerBeatmapLevel(state.difficultyBeatmap != null ? new PreviewDifficultyBeatmap((IPreviewBeatmapLevel) state.difficultyBeatmap.level, state.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, state.difficultyBeatmap.difficulty) : (PreviewDifficultyBeatmap) null);
    this.SetRightScreenViewController((ViewController) this._serverPlayerListViewController, ViewController.AnimationType.None);
    this.DismissFlowCoordinator((FlowCoordinator) this._multiplayerLevelSelectionFlowCoordinator);
  }

  public virtual void HandleMultiplayerLevelSelectionFlowCoordinatorCancelSelectLevel()
  {
    this.SetRightScreenViewController((ViewController) this._serverPlayerListViewController, ViewController.AnimationType.None);
    this.DismissFlowCoordinator((FlowCoordinator) this._multiplayerLevelSelectionFlowCoordinator);
  }

  public virtual void HandleLobbySetupViewControllerSelectModifiers()
  {
    this._selectModifiersViewController.Setup(this._lobbyPlayersDataModel[this._lobbyPlayersDataModel.localUserId]?.gameplayModifiers ?? GameplayModifiers.noModifiers);
    this.PresentViewController((ViewController) this._selectModifiersViewController);
  }

  public virtual void HandleLobbySetupViewControllerClearSelectedBeatmap() => this._lobbyPlayersDataModel.ClearLocalPlayerBeatmapLevel();

  public virtual void HandleLobbySetupViewControllerClearSelectedModifiers() => this._lobbyPlayersDataModel.ClearLocalPlayerGameplayModifiers();

  public virtual void HandleLobbyGameStateControllerGameStateChanged(MultiplayerLobbyState state) => this._lobbySetupViewController.SetLobbyState(state);

  public virtual void HandleServerPlayerListViewControllerSelectSuggestedBeatmap(
    PreviewDifficultyBeatmap beatmapLevel)
  {
    this._lobbyPlayersDataModel.SetLocalPlayerBeatmapLevel(beatmapLevel);
  }

  public virtual void HandleServerPlayerListViewControllerSelectSuggestedGameplayModifiers(
    GameplayModifiers modifiers)
  {
    this._lobbyPlayersDataModel.SetLocalPlayerGameplayModifiers(modifiers);
  }

  public virtual void HandleServerPlayerListViewControllerKickPlayer(string userId)
  {
    IConnectedPlayer playerById = this._lobbyStateDataModel.GetPlayerById(userId);
    if (playerById == null)
      return;
    this._simpleDialogPromptViewController.Init(Localization.Get("CONFIRM_KICK"), Localization.GetFormat("LABEL_KICK_PLAYER_PROMPT", (object) playerById.userName), Localization.Get("PROMPT_NO"), Localization.Get("PROMPT_YES"), (System.Action<int>) (btnId =>
    {
      if (btnId == 1)
        this._lobbyPlayersDataModel.RequestKickPlayer(userId);
      this.SetRightScreenViewController((ViewController) this._serverPlayerListViewController, ViewController.AnimationType.Out);
    }));
    this.SetRightScreenViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationType.In);
  }

  public virtual void HandleLobbyGameStateControllerCountdownStarted()
  {
    if (this._lobbyPlayerPermissionsModel.isPartyOwner && (double) Mathf.Abs(this._lobbyGameStateController.countdownEndTime - this._lobbyGameStateController.predictedCountdownEndTime) <= 1.5)
      return;
    if (this._lobbyPlayerPermissionsModel.isPartyOwner)
      this._centerStageScreenController.SetCountdownEndTime(this._lobbyGameStateController.countdownEndTime);
    else
      this._centerStageScreenController.ShowCountdown(this._lobbyGameStateController.countdownEndTime);
  }

  public virtual void HandleLobbyGameStateControllerCountdownCancelled()
  {
    if (this._lobbyPlayerPermissionsModel.isPartyOwner)
      return;
    this._centerStageScreenController.HideCountdown();
  }

  public virtual void HandleLobbyGameStateControllerGameStarted(
    ILevelGameplaySetupData levelGameplaySetupData)
  {
    this._lobbyGameStateController.songStillDownloadingEvent += new System.Action(this.HandleLobbyGameStateControllerSongStillDownloading);
    this._centerStageScreenController.SetNextGameplaySetupData(levelGameplaySetupData);
    if (this._lobbyPlayerPermissionsModel.isPartyOwner && (double) Mathf.Abs(this._lobbyGameStateController.startTime - this._lobbyGameStateController.predictedCountdownEndTime) <= 1.5)
      return;
    this._centerStageScreenController.ShowCountdown(this._lobbyGameStateController.startTime);
    this._centerStageScreenController.ShowCountdownColorPreset();
  }

  public virtual void HandleLobbyGameStateControllerStartTimeChanged()
  {
    if (this._lobbyPlayerPermissionsModel.isPartyOwner && (double) Mathf.Abs(this._lobbyGameStateController.startTime - this._lobbyGameStateController.predictedCountdownEndTime) <= 1.5)
      return;
    this._centerStageScreenController.SetCountdownEndTime(this._lobbyGameStateController.startTime);
  }

  public virtual void HandleLobbyGameStateControllerSongStillDownloading()
  {
  }

  public virtual void HandleLobbySetupViewControllerStartGameOrReady()
  {
    if (this._lobbyPlayerPermissionsModel.isPartyOwner)
    {
      this._lobbyGameStateController.PredictCountdownEndTime();
      this._centerStageScreenController.ShowCountdown(this._lobbyGameStateController.predictedCountdownEndTime);
      if (this._lobbyGameStateController.IsCloseToStartGame())
      {
        this._lobbyGameStateController.state = MultiplayerLobbyState.GameStarting;
        this._centerStageScreenController.ShowCountdownColorPreset();
      }
      else
        this._lobbyGameStateController.state = MultiplayerLobbyState.LobbyCountdown;
    }
    this._lobbyPlayersDataModel.SetLocalPlayerIsReady(true);
    System.Action gameOrReadyEvent = this.startGameOrReadyEvent;
    if (gameOrReadyEvent == null)
      return;
    gameOrReadyEvent();
  }

  public virtual void HandleLobbySetupViewControllerCancelGameOrUnready()
  {
    if (this._lobbyPlayerPermissionsModel.isPartyOwner)
    {
      this._lobbyGameStateController.state = MultiplayerLobbyState.LobbySetup;
      this._centerStageScreenController.HideCountdown();
    }
    if (this.isQuickStartServer)
    {
      this.rejoinQuickPlay = true;
      this.Finish();
    }
    else
      this._lobbyPlayersDataModel.SetLocalPlayerIsReady(false);
  }

  public virtual void HandleLobbyGameStateControllerCancelStartTime()
  {
    this._centerStageScreenController.HideCountdown();
    this._centerStageScreenController.ShowLobbyColorPreset();
  }

  public virtual void HandleLobbyGameStateControllerGameStartedPresentView(
    ILevelGameplaySetupData levelGameplaySetupData)
  {
    if (!((UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._lobbySetupViewController))
      return;
    this.DismissViewController(this.topViewController);
  }

  public virtual void HandleLobbyGameStateControllerLevelFinished(
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData,
    MultiplayerResultsData multiplayerResultsData)
  {
    MultiplayerLevelCompletionResults completionResults1 = multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults;
    LevelCompletionResults completionResults2 = completionResults1.levelCompletionResults;
    IDifficultyBeatmap difficultyBeatmap = multiplayerLevelScenesTransitionSetupData.difficultyBeatmap;
    if (difficultyBeatmap != null && completionResults1.hasAnyResults)
      this._playerDataModel.playerData.playerAllOverallStatsData.UpdateOnlinePlayOverallStatsData(completionResults2, difficultyBeatmap);
    if (completionResults1.playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Quit)
    {
      this.Finish(withFadeOut: false);
    }
    else
    {
      this._lobbyPlayersDataModel.SetLocalPlayerIsInLobby(true);
      this._lobbyGameStateController.StartListeningToGameStart();
      this._multiplayerLobbyController.ActivateMultiplayerLobby();
      this._lobbyPlayersDataModel.SetLocalPlayerIsReady(false);
      if (completionResults1.playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.HostEndedLevel || completionResults1.playerLevelEndReason == MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.ConnectedAfterLevelEnded)
        return;
      IPreviewBeatmapLevel previewBeatmapLevel = multiplayerLevelScenesTransitionSetupData.previewBeatmapLevel;
      BeatmapDifficulty beatmapDifficulty = multiplayerLevelScenesTransitionSetupData.beatmapDifficulty;
      BeatmapCharacteristicSO beatmapCharacteristic = multiplayerLevelScenesTransitionSetupData.beatmapCharacteristic;
      IReadonlyBeatmapData transformedBeatmapData = multiplayerLevelScenesTransitionSetupData.transformedBeatmapData;
      if (completionResults1.hasAnyResults && difficultyBeatmap != null)
      {
        LevelCompletionResultsHelper.ProcessScore(this._playerDataModel.playerData, this._playerDataModel.playerData.GetPlayerLevelStatsData(difficultyBeatmap), completionResults2, transformedBeatmapData, difficultyBeatmap, this._platformLeaderboardsModel);
        this._playerDataModel.Save();
      }
      this._multiplayerResultsViewController.Init(multiplayerResultsData, previewBeatmapLevel, beatmapDifficulty, beatmapCharacteristic, true, !this.isManaged);
      this.PresentViewController((ViewController) this._multiplayerResultsViewController, immediately: true);
    }
  }

  public virtual void HandleLobbyGameStateControllerLevelDidGetDisconnected(
    DisconnectedReason disconnectedReason)
  {
    this.ShowDisconnectDialogAndFinish(disconnectedReason);
  }

  public virtual void HandleMultiplayerResultsViewControllerBackToLobbyPressed(
    MultiplayerResultsViewController viewController)
  {
    if (this._lobbyGameStateController.isDisconnected)
      this.ShowDisconnectDialogAndFinish(this._lobbyGameStateController.disconnectedReason);
    else
      this.DismissViewController((ViewController) viewController);
  }

  public virtual void HandleMultiplayerResultsViewControllerBackToMenuPressed(
    MultiplayerResultsViewController viewController)
  {
    this.Finish();
  }

  public virtual void SetupLobbyWithPermissions()
  {
    this.SetTitle(this.topViewController, ViewController.AnimationType.None);
    this._lobbySetupViewController.Setup(this._unifiedNetworkPlayerModel.selectionMask, this.isPartyOwner, this._lobbyPlayerPermissionsModel.hasRecommendBeatmapPermission, this._lobbyPlayerPermissionsModel.hasRecommendModifiersPermission, this.isManaged, this.isQuickStartServer);
    this._lobbySetupViewController.SetLobbyState(this._lobbyGameStateController.state);
    this._lobbySetupViewController.SetLobbyPlayerData(this._lobbyPlayersDataModel[this._lobbyPlayersDataModel.localUserId]);
    this._multiplayerSettingsPanelController.HideConnectionSettings(!this._lobbyPlayerPermissionsModel.hasInvitePermission || this._lobbyStateDataModel.configuration.discoveryPolicy == DiscoveryPolicy.Hidden);
    this._lobbySetupViewController.SetPlayersMissingLevelText((string) null);
    if (!this._lobbyPlayerPermissionsModel.isPartyOwner)
      return;
    this._lobbyPlayersDataModel.SetLocalPlayerIsReady(false);
    this._lobbySetupViewController.SetStartGameEnabled(this._lobbyGameStateController.cannotStartGameReason);
  }

  public virtual void SetLobbyPlayerDataToViews(string userId)
  {
    if (userId != this._lobbyPlayersDataModel.localUserId)
      return;
    ILobbyPlayerData lobbyPlayerData = this._lobbyPlayersDataModel[userId];
    if (lobbyPlayerData == null)
      return;
    this._lobbySetupViewController.SetLobbyPlayerData(lobbyPlayerData);
    this._multiplayerSettingsPanelController.SetLobbyPlayerDataModel(lobbyPlayerData);
    this._multiplayerSettingsPanelController.SetLobbyCode(this._unifiedNetworkPlayerModel.code);
  }

  public virtual void ShowSideViewControllers(
    bool showSideViewControllers,
    ViewController.AnimationType animationType)
  {
    if (showSideViewControllers)
    {
      this.SetLeftScreenViewController((ViewController) this._gameplaySetupViewController, animationType);
      this.SetRightScreenViewController((ViewController) this._serverPlayerListViewController, animationType);
    }
    else
    {
      this.SetLeftScreenViewController((ViewController) null, animationType);
      this.SetRightScreenViewController((ViewController) null, animationType);
    }
  }

  public virtual void ShowBackButton(bool show) => this.showBackButton = show;

  public virtual string GetLocalizedTitle()
  {
    switch (this.GetLobbyType())
    {
      case GameServerLobbyFlowCoordinator.LobbyType.HostSetup:
        return Localization.Get("TITLE_HOST_SETUP");
      case GameServerLobbyFlowCoordinator.LobbyType.ClientSetup:
        return Localization.Get("TITLE_CLIENT_SETUP");
      case GameServerLobbyFlowCoordinator.LobbyType.QuickPlayLobby:
        return Localization.Get("TITLE_QUICK_PLAY_LOBBY");
      default:
        return Localization.Get("TITLE_PARTY");
    }
  }

  public virtual void SetTitle(
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._lobbySetupViewController)
      this.SetTitle(this.GetLocalizedTitle(), animationType);
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._selectModifiersViewController)
    {
      this.showBackButton = true;
      this.SetTitle(this.isPartyOwner ? Localization.Get("TITLE_SELECT_MODIFIERS") : Localization.Get("TITLE_SUGGEST_MODIFIERS"), animationType);
    }
    else
      this.SetTitle("", animationType);
  }

  public virtual void ShowDisconnectDialogAndFinish(DisconnectedReason disconnectedReason)
  {
    this._lobbyGameStateController.Deactivate();
    this._lobbyGameStateController.lobbyDisconnectedEvent -= new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
    this._isAlreadyFinishing = true;
    this._multiplayerLobbyController.DeactivateMultiplayerLobby();
    switch (disconnectedReason)
    {
      case DisconnectedReason.UserInitiated:
        this.Finish();
        return;
      case DisconnectedReason.ServerTerminated:
        if (this.isQuickStartServer)
        {
          this.rejoinQuickPlay = true;
          this.Finish();
          return;
        }
        break;
    }
    this._analyticsModel.LogEvent("Multiplayer Disconnected", new Dictionary<string, string>()
    {
      {
        "reason",
        string.Format("{0}", (object) disconnectedReason)
      },
      {
        "connectionType",
        this.isPartyOwner ? "PartyHost" : (this.isManaged ? "PartyClient" : "QuickPlay")
      }
    });
    this._connectionErrorDialogViewController.Init(disconnectedReason, (System.Action) (() => this.Finish()));
    this.PresentViewController((ViewController) this._connectionErrorDialogViewController, animationDirection: ViewController.AnimationDirection.Vertical);
  }

  public virtual void UpdateLocalPlayerIsActiveState(bool isActive)
  {
    if (this._lobbyPlayersDataModel[this._lobbyPlayersDataModel.localUserId].isActive == isActive)
      return;
    this._lobbyPlayersDataModel.SetLocalPlayerIsActive(isActive);
  }

  public virtual void HandleLobbyGameBeforeSceneSwitchCallback()
  {
    if ((UnityEngine.Object) this.topViewController == (UnityEngine.Object) this._simpleDialogPromptViewController)
      this.DismissViewController((ViewController) this._simpleDialogPromptViewController, immediately: true);
    this._lobbyGameStateController.songStillDownloadingEvent -= new System.Action(this.HandleLobbyGameStateControllerSongStillDownloading);
    if (this._multiplayerLobbyController.lobbyActivated)
      this._multiplayerLobbyController.DeactivateMultiplayerLobby();
    this._lobbyPlayersDataModel.ClearLocalPlayerBeatmapLevel();
  }

  public virtual void HandleLobbyGameStateStartButtonEnabled(
    CannotStartGameReason cannotStartGameReason)
  {
    if (!this.isPartyOwner)
      return;
    this._lobbySetupViewController.SetStartGameEnabled(cannotStartGameReason);
  }

  public virtual void HandleMenuRpcManagerSetPlayersMissingEntitlementsToLevel(
    PlayersMissingEntitlementsNetSerializable playersMissingEntitlements)
  {
    if (!this._lobbyPlayerPermissionsModel.isPartyOwner)
      return;
    if (playersMissingEntitlements.playersWithoutEntitlements.Count == 0)
    {
      this._lobbySetupViewController.SetPlayersMissingLevelText((string) null);
    }
    else
    {
      this._stringBuilder.Clear();
      this._stringBuilder.AppendLine(Localization.Get("LABEL_PLAYERS_MISSING_ENTITLEMENT") + ": ");
      for (int index = 0; index < playersMissingEntitlements.playersWithoutEntitlements.Count; ++index)
      {
        IConnectedPlayer playerById = this._lobbyStateDataModel.GetPlayerById(playersMissingEntitlements.playersWithoutEntitlements[index]);
        if (playerById != null)
        {
          this._stringBuilder.Append(playerById.userName);
          if (index < playersMissingEntitlements.playersWithoutEntitlements.Count - 1)
            this._stringBuilder.Append(", ");
        }
      }
      this._lobbySetupViewController.SetPlayersMissingLevelText(this._stringBuilder.ToString());
    }
  }

  public virtual void HandleLobbyPlayerPermissionsModelPermissionsChanged() => this.SetupLobbyWithPermissions();

  public virtual void DismissViewControllersAndCoordinators()
  {
    while ((UnityEngine.Object) this.childFlowCoordinator != (UnityEngine.Object) null)
      this.DismissFlowCoordinator(this.childFlowCoordinator, immediately: true);
    while ((UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._lobbySetupViewController)
      this.DismissViewController(this.topViewController, immediately: true);
  }

  [CompilerGenerated]
  public virtual void m_CPresentBackButtonConfirmationDialogm_Eb__69_0(int btnIndex)
  {
    if (btnIndex != 0)
    {
      if (btnIndex != 1)
        return;
      this._isAlreadyFinishing = true;
      this.Finish();
    }
    else
    {
      this._lobbyGameStateController.lobbyDisconnectedEvent += new System.Action(this.HandleLobbyGameStateControllerLobbyDisconnected);
      this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);
    }
  }

  [CompilerGenerated]
  public virtual void m_CShowDisconnectDialogAndFinishm_Eb__105_0() => this.Finish();

  public enum LobbyType
  {
    HostSetup,
    ClientSetup,
    QuickPlayLobby,
    Party,
  }
}
