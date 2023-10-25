// Decompiled with JetBrains decompiler
// Type: MultiplayerModeSelectionFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MultiplayerModeSelectionFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected AudioClip _ambienceAudioClip;
  [SerializeField]
  protected SongPackMaskModelSO _songPackMaskModel;
  [Inject]
  protected readonly GameServerBrowserFlowCoordinator _gameServerBrowserFlowCoordinator;
  [Inject]
  protected readonly GameServerLobbyFlowCoordinator _gameServerLobbyFlowCoordinator;
  [Inject]
  protected readonly MultiplayerModeSelectionViewController _multiplayerModeSelectionViewController;
  [Inject]
  protected readonly CreateServerViewController _createServerViewController;
  [Inject]
  protected readonly JoinQuickPlayViewController _joinQuickPlayViewController;
  [Inject]
  protected readonly ServerCodeEntryViewController _serverCodeEntryViewController;
  [Inject]
  protected readonly SimpleDialogPromptViewController _simpleDialogPromptViewController;
  [Inject]
  protected readonly JoiningLobbyViewController _joiningLobbyViewController;
  [Inject]
  protected readonly IUnifiedNetworkPlayerModel _unifiedNetworkPlayerModel;
  [Inject]
  protected readonly AvatarDataModel _avatarDataModel;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly FadeInOutController _fadeInOutController;
  [Inject]
  protected readonly LobbyDataModelsManager _lobbyDataModelsManager;
  [Inject]
  protected readonly MultiplayerLobbyConnectionController _multiplayerLobbyConnectionController;
  [Inject]
  protected readonly IMultiplayerStatusModel _multiplayerStatusModel;
  [Inject]
  protected readonly IQuickPlaySetupModel _quickPlaySetupModel;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  [Inject]
  protected readonly ILobbyGameStateController _lobbyGameStateController;
  protected CancellationTokenSource _joiningLobbyCancellationTokenSource;
  protected CancellationTokenSource _cancellationTokenSource;
  protected TaskCompletionSource<bool> _transitionFinishedTaskSource;
  protected QuickPlaySetupData _quickPlaySetupData;
  protected SelectMultiplayerLobbyDestination _lobbyDestination;

  public event System.Action<MultiplayerModeSelectionFlowCoordinator> didFinishEvent;

  public virtual void Setup(SelectMultiplayerLobbyDestination lobbyDestination) => this._lobbyDestination = lobbyDestination;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._songPreviewPlayer.CrossfadeToNewDefault(this._ambienceAudioClip);
    this._multiplayerLobbyConnectionController.connectionSuccessEvent += new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccessActivateModel);
    this._multiplayerLobbyConnectionController.connectionSuccessEvent += new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccess);
    this._multiplayerLobbyConnectionController.connectionFailedEvent += new System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason>(this.HandleMultiplayerLobbyConnectionControllerConnectionFailed);
    this._multiplayerModeSelectionViewController.didFinishEvent += new System.Action<MultiplayerModeSelectionViewController, MultiplayerModeSelectionViewController.MenuButton>(this.HandleMultiplayerLobbyControllerDidFinish);
    this._createServerViewController.didFinishEvent += new System.Action<bool, CreateServerFormData>(this.HandleCreateServerViewControllerDidFinish);
    this._joinQuickPlayViewController.didFinishEvent += new System.Action<bool>(this.HandleJoinQuickPlayViewControllerDidFinish);
    this._serverCodeEntryViewController.didFinishEvent += new System.Action<bool, string>(this.HandleServerCodeEntryViewControllerDidFinish);
    this._joiningLobbyViewController.didCancelEvent += new System.Action(this.HandleJoiningLobbyViewControllerDidCancel);
    this._cancellationTokenSource = new CancellationTokenSource();
    this._transitionFinishedTaskSource = new TaskCompletionSource<bool>();
    if (addedToHierarchy)
      this.TryShowModeSelection(true);
    this._analyticsModel.LogImpression("Multiplayer Mode Selection", new Dictionary<string, string>()
    {
      ["page"] = "Multiplayer"
    });
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    this._multiplayerLobbyConnectionController.connectionSuccessEvent -= new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccessActivateModel);
    this._multiplayerLobbyConnectionController.connectionSuccessEvent -= new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccess);
    this._multiplayerLobbyConnectionController.connectionFailedEvent -= new System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason>(this.HandleMultiplayerLobbyConnectionControllerConnectionFailed);
    this._multiplayerModeSelectionViewController.didFinishEvent -= new System.Action<MultiplayerModeSelectionViewController, MultiplayerModeSelectionViewController.MenuButton>(this.HandleMultiplayerLobbyControllerDidFinish);
    this._createServerViewController.didFinishEvent -= new System.Action<bool, CreateServerFormData>(this.HandleCreateServerViewControllerDidFinish);
    this._joinQuickPlayViewController.didFinishEvent -= new System.Action<bool>(this.HandleJoinQuickPlayViewControllerDidFinish);
    this._serverCodeEntryViewController.didFinishEvent -= new System.Action<bool, string>(this.HandleServerCodeEntryViewControllerDidFinish);
    this._joiningLobbyViewController.didCancelEvent -= new System.Action(this.HandleJoiningLobbyViewControllerDidCancel);
    this._cancellationTokenSource.Cancel();
    if (!removedFromHierarchy)
      return;
    this._unifiedNetworkPlayerModel.connectedPlayerManagerCreatedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
  }

  protected override void BackButtonWasPressed(ViewController topViewController)
  {
    if (this._multiplayerLobbyConnectionController.connectionState == MultiplayerLobbyConnectionController.LobbyConnectionState.Connected)
      return;
    if ((UnityEngine.Object) topViewController != (UnityEngine.Object) this._multiplayerModeSelectionViewController)
    {
      this.DismissViewController(topViewController);
    }
    else
    {
      System.Action<MultiplayerModeSelectionFlowCoordinator> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(this);
    }
  }

  protected override void TopViewControllerWillChange(
    ViewController oldViewController,
    ViewController newViewController,
    ViewController.AnimationType animationType)
  {
    base.TopViewControllerWillChange(oldViewController, newViewController, animationType);
    this.showBackButton = true;
    if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._multiplayerModeSelectionViewController)
      this.SetTitle(Localization.Get("LABEL_MULTIPLAYER_MODE_SELECTION"), animationType);
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._createServerViewController)
    {
      this.SetTitle(Localization.Get("LABEL_CREATE_SERVER"), animationType);
      this.showBackButton = false;
    }
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._joinQuickPlayViewController)
    {
      this.SetTitle(Localization.Get("LABEL_JOIN_QUICK_PLAY"), animationType);
      this.showBackButton = false;
    }
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._serverCodeEntryViewController)
    {
      this.SetTitle(Localization.Get("LABEL_JOIN_VIA_CODE"), animationType);
      this.showBackButton = false;
    }
    else if ((UnityEngine.Object) newViewController == (UnityEngine.Object) this._joiningLobbyViewController)
    {
      this.SetTitle(Localization.Get("LABEL_JOINING_LOBBY"), animationType);
      this.showBackButton = false;
    }
    else
    {
      if (!((UnityEngine.Object) newViewController == (UnityEngine.Object) this._simpleDialogPromptViewController))
        return;
      this.SetTitle("", animationType);
      this.showBackButton = false;
    }
  }

  protected override void TransitionDidStart()
  {
    base.TransitionDidStart();
    if (!this.IsFlowCoordinatorInHierarchy((FlowCoordinator) this) || (UnityEngine.Object) this.childFlowCoordinator != (UnityEngine.Object) null)
      return;
    this._multiplayerLobbyConnectionController.connectionSuccessEvent -= new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccess);
    this._multiplayerLobbyConnectionController.connectionFailedEvent -= new System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason>(this.HandleMultiplayerLobbyConnectionControllerConnectionFailed);
  }

  protected override void TransitionDidFinish()
  {
    base.TransitionDidFinish();
    if (!this.IsFlowCoordinatorInHierarchy((FlowCoordinator) this) || (UnityEngine.Object) this.childFlowCoordinator != (UnityEngine.Object) null)
      return;
    this._transitionFinishedTaskSource?.TrySetResult(true);
    switch (this._multiplayerLobbyConnectionController.connectionState)
    {
      case MultiplayerLobbyConnectionController.LobbyConnectionState.Connected:
        if (!this._joiningLobbyCancellationTokenSource.IsCancellationRequested)
        {
          this.ResolveAndPresentNextFlowCoordinator();
          break;
        }
        break;
      case MultiplayerLobbyConnectionController.LobbyConnectionState.ConnectionFailed:
        if ((UnityEngine.Object) this.topViewController != (UnityEngine.Object) this._simpleDialogPromptViewController)
        {
          this.PresentConnectionErrorDialog(this._multiplayerLobbyConnectionController.connectionType, this._multiplayerLobbyConnectionController.connectionFailedReason);
          break;
        }
        break;
    }
    this._multiplayerLobbyConnectionController.connectionSuccessEvent -= new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccess);
    this._multiplayerLobbyConnectionController.connectionSuccessEvent += new System.Action(this.HandleMultiplayerLobbyConnectionControllerConnectionSuccess);
    this._multiplayerLobbyConnectionController.connectionFailedEvent -= new System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason>(this.HandleMultiplayerLobbyConnectionControllerConnectionFailed);
    this._multiplayerLobbyConnectionController.connectionFailedEvent += new System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason>(this.HandleMultiplayerLobbyConnectionControllerConnectionFailed);
  }

  public virtual void HandleMultiplayerLobbyConnectionControllerConnectionSuccessActivateModel() => this._lobbyDataModelsManager.Activate();

  public virtual void HandleMultiplayerLobbyConnectionControllerConnectionSuccess() => this.ResolveAndPresentNextFlowCoordinator();

  public virtual void HandleMultiplayerLobbyConnectionControllerConnectionFailed(
    MultiplayerLobbyConnectionController.LobbyConnectionType connectionType,
    ConnectionFailedReason reason)
  {
    this._unifiedNetworkPlayerModel.ResetMasterServerReachability();
    this.PresentConnectionErrorDialog(connectionType, reason);
  }

  public virtual void HandleMultiplayerLobbyControllerDidFinish(
    MultiplayerModeSelectionViewController viewController,
    MultiplayerModeSelectionViewController.MenuButton menuButton)
  {
    this._analyticsModel.LogClick("Multiplayer Mode Selection", new Dictionary<string, string>()
    {
      ["page"] = "Multiplayer",
      ["custom_text"] = string.Format("{0}", (object) menuButton)
    });
    switch (menuButton)
    {
      case MultiplayerModeSelectionViewController.MenuButton.QuickPlay:
        this._joinQuickPlayViewController.Setup(this._quickPlaySetupData, this._playerDataModel.playerData.multiplayerModeSettings);
        this.PresentViewController((ViewController) this._joinQuickPlayViewController, animationDirection: ViewController.AnimationDirection.Vertical);
        BeatmapDifficulty beatmapDifficulty = this._playerDataModel.playerData.multiplayerModeSettings.quickPlayBeatmapDifficulty.FromMask();
        this._analyticsModel.LogImpression("Join Quick Play", new Dictionary<string, string>()
        {
          ["page"] = "Multiplayer",
          ["difficulty"] = string.Format("{0}", (object) beatmapDifficulty)
        });
        break;
      case MultiplayerModeSelectionViewController.MenuButton.CreateServer:
        this._createServerViewController.Setup(this._playerDataModel.playerData.multiplayerModeSettings);
        this.PresentViewController((ViewController) this._createServerViewController, animationDirection: ViewController.AnimationDirection.Vertical);
        this._analyticsModel.LogImpression("Create Server", new Dictionary<string, string>()
        {
          ["page"] = "Multiplayer",
          ["player_count"] = string.Format("{0}", (object) this._playerDataModel.playerData.multiplayerModeSettings.createServerPlayersCount)
        });
        break;
      case MultiplayerModeSelectionViewController.MenuButton.JoinWithCode:
        this.PresentViewController((ViewController) this._serverCodeEntryViewController, animationDirection: ViewController.AnimationDirection.Vertical);
        this._analyticsModel.LogImpression("Join Via Code", new Dictionary<string, string>()
        {
          ["page"] = "Multiplayer"
        });
        break;
      case MultiplayerModeSelectionViewController.MenuButton.GameBrowser:
        this.PresentFlowCoordinator((FlowCoordinator) this._gameServerBrowserFlowCoordinator);
        break;
    }
  }

  public virtual void HandleGameServerBrowserFlowCoordinatorDidFinish(
    GameServerBrowserFlowCoordinator flowCoordinator)
  {
    this.DismissFlowCoordinator((FlowCoordinator) flowCoordinator);
  }

  public virtual void HandleJoiningLobbyViewControllerDidCancel()
  {
    this._cancellationTokenSource?.Cancel();
    this._joiningLobbyCancellationTokenSource?.Cancel();
    this._unifiedNetworkPlayerModel.DestroyPartyConnection();
  }

  public virtual void HandleJoinQuickPlayViewControllerDidFinish(bool success)
  {
    this._playerDataModel.playerData.SetMultiplayerModeSettings(this._joinQuickPlayViewController.multiplayerModeSettings);
    this._playerDataModel.Save();
    BeatmapDifficulty beatmapDifficulty = this._playerDataModel.playerData.multiplayerModeSettings.quickPlayBeatmapDifficulty.FromMask();
    string str = success ? "Join" : "Cancel";
    this._analyticsModel.LogClick("Join Quick Play", new Dictionary<string, string>()
    {
      ["page"] = "Multiplayer",
      ["difficulty"] = string.Format("{0}", (object) beatmapDifficulty),
      ["custom_text"] = str ?? ""
    });
    if (!success)
    {
      this.DismissViewController((ViewController) this._joinQuickPlayViewController, ViewController.AnimationDirection.Vertical);
    }
    else
    {
      this._joiningLobbyCancellationTokenSource?.Cancel();
      this._joiningLobbyCancellationTokenSource = new CancellationTokenSource();
      this._multiplayerLobbyConnectionController.ConnectToMatchmaking(this._joinQuickPlayViewController.multiplayerModeSettings.quickPlayBeatmapDifficulty, this._songPackMaskModel.ToSongPackMask(this._joinQuickPlayViewController.multiplayerModeSettings.quickPlaySongPackMaskSerializedName), this._joinQuickPlayViewController.multiplayerModeSettings.quickPlayEnableLevelSelection);
      this._joiningLobbyViewController.Init(Localization.Get("LABEL_JOINING_QUICK_PLAY"));
      this.ReplaceTopViewController((ViewController) this._joiningLobbyViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    }
  }

  public virtual void HandleServerCodeEntryViewControllerDidFinish(bool success, string code)
  {
    if (!success)
    {
      this.DismissViewController((ViewController) this._serverCodeEntryViewController, ViewController.AnimationDirection.Vertical);
    }
    else
    {
      this._joiningLobbyCancellationTokenSource?.Cancel();
      this._joiningLobbyCancellationTokenSource = new CancellationTokenSource();
      this._multiplayerLobbyConnectionController.ConnectToParty(code.ToUpper());
      this._joiningLobbyViewController.Init(Localization.Get("LABEL_JOINING_GAME"));
      this.ReplaceTopViewController((ViewController) this._joiningLobbyViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    }
  }

  public virtual void HandleCreateServerViewControllerDidFinish(
    bool success,
    CreateServerFormData data)
  {
    this._playerDataModel.playerData.SetMultiplayerModeSettings(this._createServerViewController.multiplayerModeSettings);
    this._playerDataModel.Save();
    string str = success ? "Create" : "Cancel";
    this._analyticsModel.LogClick("Create Server", new Dictionary<string, string>()
    {
      ["page"] = "Multiplayer",
      ["player_count"] = string.Format("{0}", (object) this._playerDataModel.playerData.multiplayerModeSettings.createServerPlayersCount),
      ["custom_text"] = str ?? ""
    });
    if (!success)
    {
      this.DismissViewController((ViewController) this._createServerViewController, ViewController.AnimationDirection.Vertical);
    }
    else
    {
      this._joiningLobbyCancellationTokenSource?.Cancel();
      this._joiningLobbyCancellationTokenSource = new CancellationTokenSource();
      this._multiplayerLobbyConnectionController.CreateParty(data);
      this._joiningLobbyViewController.Init(Localization.Get("LABEL_CREATING_SERVER"));
      this.ReplaceTopViewController((ViewController) this._joiningLobbyViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    }
  }

  public virtual void HandleGameServerLobbyFlowCoordinatorDidFinish()
  {
    this._gameServerLobbyFlowCoordinator.didFinishEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidFinish);
    this._multiplayerLobbyConnectionController.LeaveLobby();
    this._lobbyDataModelsManager.Deactivate();
    this.DismissFlowCoordinator((FlowCoordinator) this._gameServerLobbyFlowCoordinator, immediately: true);
    this._unifiedNetworkPlayerModel.ResetMasterServerReachability();
    this._fadeInOutController.FadeIn();
    if (!this._gameServerLobbyFlowCoordinator.rejoinQuickPlay)
      return;
    this._joiningLobbyCancellationTokenSource?.Cancel();
    this._joiningLobbyCancellationTokenSource = new CancellationTokenSource();
    this._joiningLobbyViewController.Init(Localization.Get("LABEL_JOINING_QUICK_PLAY"));
    this.PresentViewController((ViewController) this._joiningLobbyViewController, immediately: true);
    this._multiplayerLobbyConnectionController.ConnectToMatchmaking(this._joinQuickPlayViewController.multiplayerModeSettings.quickPlayBeatmapDifficulty, this._songPackMaskModel.ToSongPackMask(this._joinQuickPlayViewController.multiplayerModeSettings.quickPlaySongPackMaskSerializedName), this._joinQuickPlayViewController.multiplayerModeSettings.quickPlayEnableLevelSelection);
  }

  public virtual void HandleGameServerLobbyFlowCoordinatorWillFinish()
  {
    this._gameServerLobbyFlowCoordinator.willFinishEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorWillFinish);
    this._lobbyDataModelsManager.Deactivate();
  }

  public virtual void HandleConnectedPlayerManagerCreated(INetworkPlayerModel networkPlayerModel)
  {
    networkPlayerModel.connectedPlayerManager.SetLocalPlayerAvatar(this._avatarDataModel.avatarData.CreateMultiplayerAvatarData());
    this._multiplayerSessionManager.StartSession(MultiplayerSessionManager.SessionType.Player, networkPlayerModel.connectedPlayerManager);
  }

  public virtual async void TryShowModeSelection(bool shouldProvideInitialViewControllers)
  {
    MultiplayerModeSelectionFlowCoordinator selectionFlowCoordinator = this;
    selectionFlowCoordinator._joiningLobbyViewController.Init(Localization.Get("LABEL_CHECKING_SERVER_STATUS"));
    selectionFlowCoordinator.showBackButton = false;
    selectionFlowCoordinator.SetTitle(Localization.Get("LABEL_CHECKING_SERVER_STATUS"));
    selectionFlowCoordinator._transitionFinishedTaskSource = new TaskCompletionSource<bool>();
    selectionFlowCoordinator.ProvideInitialViewControllers((ViewController) selectionFlowCoordinator._joiningLobbyViewController);
    int num = await selectionFlowCoordinator._transitionFinishedTaskSource.Task ? 1 : 0;
    MultiplayerStatusData multiplayerStatusData = (MultiplayerStatusData) null;
    Exception exception = (Exception) null;
    try
    {
      multiplayerStatusData = await selectionFlowCoordinator._multiplayerStatusModel.GetMultiplayerStatusAsync(selectionFlowCoordinator._cancellationTokenSource.Token);
    }
    catch (TaskCanceledException ex)
    {
      System.Action<MultiplayerModeSelectionFlowCoordinator> didFinishEvent = selectionFlowCoordinator.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(selectionFlowCoordinator);
      return;
    }
    catch (Exception ex)
    {
      exception = ex;
    }
    MultiplayerUnavailableReason reason;
    if (MultiplayerUnavailableReasonMethods.TryGetMultiplayerUnavailableReason(multiplayerStatusData, out reason))
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (selectionFlowCoordinator.PresentMasterServerUnavailableErrorDialog(reason, exception, multiplayerStatusData?.maintenanceEndTime, multiplayerStatusData.GetLocalizedMessage(Localization.Instance.SelectedLanguage)));
    }
    else
    {
      try
      {
        QuickPlaySetupData quickPlaySetupAsync = await selectionFlowCoordinator._quickPlaySetupModel.GetQuickPlaySetupAsync(selectionFlowCoordinator._cancellationTokenSource.Token);
        selectionFlowCoordinator._quickPlaySetupData = quickPlaySetupAsync;
      }
      catch (Exception ex)
      {
        selectionFlowCoordinator._quickPlaySetupData = new QuickPlaySetupData();
      }
      selectionFlowCoordinator._unifiedNetworkPlayerModel.SetActiveNetworkPlayerModelType(UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.GameLift);
      selectionFlowCoordinator._unifiedNetworkPlayerModel.connectedPlayerManagerCreatedEvent += new System.Action<INetworkPlayerModel>(selectionFlowCoordinator.HandleConnectedPlayerManagerCreated);
      if (selectionFlowCoordinator._unifiedNetworkPlayerModel.connectedPlayerManager != null)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (selectionFlowCoordinator.HandleConnectedPlayerManagerCreated((INetworkPlayerModel) selectionFlowCoordinator._unifiedNetworkPlayerModel));
      }
      selectionFlowCoordinator._multiplayerModeSelectionViewController.SetData(multiplayerStatusData);
      selectionFlowCoordinator.ReplaceTopViewController((ViewController) selectionFlowCoordinator._multiplayerModeSelectionViewController, new System.Action(selectionFlowCoordinator.ProcessDeeplinkingToLobby));
    }
  }

  public virtual async void ResolveAndPresentNextFlowCoordinator()
  {
    MultiplayerModeSelectionFlowCoordinator selectionFlowCoordinator = this;
    try
    {
      await selectionFlowCoordinator._lobbyGameStateController.GetGameStateAndConfigurationAsync(selectionFlowCoordinator._joiningLobbyCancellationTokenSource.Token);
      selectionFlowCoordinator._joiningLobbyViewController.HideLoading();
      selectionFlowCoordinator._fadeInOutController.FadeOut(new System.Action(selectionFlowCoordinator.m_CResolveAndPresentNextFlowCoordinatorm_Eb__50_0));
    }
    catch (TaskCanceledException ex)
    {
      selectionFlowCoordinator._multiplayerLobbyConnectionController.LeaveLobby();
      selectionFlowCoordinator._joiningLobbyViewController.HideLoading();
      selectionFlowCoordinator.DismissViewController((ViewController) selectionFlowCoordinator._joiningLobbyViewController, ViewController.AnimationDirection.Vertical);
    }
  }

  public virtual void PresentConnectionErrorDialog(
    MultiplayerLobbyConnectionController.LobbyConnectionType connectionType,
    ConnectionFailedReason reason)
  {
    this._multiplayerLobbyConnectionController.LeaveLobby();
    this._joiningLobbyViewController.HideLoading();
    if (reason == ConnectionFailedReason.InvalidPassword)
    {
      this._simpleDialogPromptViewController.Init(Localization.Get("LABEL_CONNECTION_ERROR"), Localization.Get("TEXT_INVALID_PASSWORD"), Localization.Get("BUTTON_OK"), (System.Action<int>) (btnId => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical)));
      this.ReplaceTopViewController((ViewController) this._simpleDialogPromptViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    }
    else if (reason != ConnectionFailedReason.ConnectionCanceled)
    {
      this._analyticsModel.LogEvent("Multiplayer Connection Failed", new Dictionary<string, string>()
      {
        {
          nameof (reason),
          string.Format("{0}", (object) reason)
        },
        {
          nameof (connectionType),
          string.Format("{0}", (object) connectionType)
        }
      });
      this._simpleDialogPromptViewController.Init(Localization.Get("LABEL_CONNECTION_ERROR"), Localization.Get(reason.LocalizedKey()) + " (" + reason.ErrorCode() + ")", Localization.Get("BUTTON_OK"), (System.Action<int>) (btnId => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical)));
      this.ReplaceTopViewController((ViewController) this._simpleDialogPromptViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    }
    else
      this.DismissViewController((ViewController) this._joiningLobbyViewController, ViewController.AnimationDirection.Vertical);
  }

  public virtual void PresentMasterServerUnavailableErrorDialog(
    MultiplayerUnavailableReason reason,
    Exception exception,
    long? maintenanceWindowEndTime = null,
    string remoteLocalizedMessage = null)
  {
    this._analyticsModel.LogEvent("Multiplayer Unavailable", new Dictionary<string, string>()
    {
      {
        nameof (reason),
        string.Format("{0}", (object) reason)
      },
      {
        nameof (exception),
        string.Format("{0}", (object) exception)
      }
    });
    SimpleDialogPromptViewController promptViewController = this._simpleDialogPromptViewController;
    string title = Localization.Get("LABEL_CONNECTION_ERROR");
    string message = remoteLocalizedMessage;
    if (message == null)
    {
      if (reason != MultiplayerUnavailableReason.MaintenanceMode)
        message = Localization.Get(reason.LocalizedKey()) + " (" + reason.ErrorCode() + ")";
      else
        message = Localization.GetFormat(reason.LocalizedKey(), (object) (maintenanceWindowEndTime.GetValueOrDefault().AsUnixTime() - DateTime.UtcNow).ToString("h':'mm"));
    }
    string buttonText = Localization.Get("BUTTON_OK");
    System.Action<int> didFinishAction = (System.Action<int>) (btnId =>
    {
      System.Action<MultiplayerModeSelectionFlowCoordinator> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(this);
    });
    promptViewController.Init(title, message, buttonText, didFinishAction);
    this.ReplaceTopViewController((ViewController) this._simpleDialogPromptViewController, animationDirection: ViewController.AnimationDirection.Vertical);
  }

  public virtual void ProcessDeeplinkingToLobby()
  {
    if (this._lobbyDestination == null)
      return;
    this.TransitionDidFinish();
    this._joiningLobbyCancellationTokenSource?.Cancel();
    this._joiningLobbyCancellationTokenSource = new CancellationTokenSource();
    this._multiplayerLobbyConnectionController.CreateOrConnectToDestinationParty(this._lobbyDestination);
    this._joiningLobbyViewController.Init(Localization.Get("LABEL_JOINING_LOBBY"));
    this.PresentViewController((ViewController) this._joiningLobbyViewController, animationDirection: ViewController.AnimationDirection.Vertical);
    this._lobbyDestination = (SelectMultiplayerLobbyDestination) null;
  }

  [CompilerGenerated]
  public virtual void m_CResolveAndPresentNextFlowCoordinatorm_Eb__50_0()
  {
    this._multiplayerLobbyConnectionController.ClearCurrentConnection();
    this._gameServerLobbyFlowCoordinator.didFinishEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidFinish);
    this._gameServerLobbyFlowCoordinator.didFinishEvent += new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidFinish);
    this._gameServerLobbyFlowCoordinator.willFinishEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorWillFinish);
    this._gameServerLobbyFlowCoordinator.willFinishEvent += new System.Action(this.HandleGameServerLobbyFlowCoordinatorWillFinish);
    this.PresentFlowCoordinator((FlowCoordinator) this._gameServerLobbyFlowCoordinator, immediately: true, replaceTopViewController: true);
  }

  [CompilerGenerated]
  public virtual void m_CPresentConnectionErrorDialogm_Eb__51_0(int btnId) => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);

  [CompilerGenerated]
  public virtual void m_CPresentConnectionErrorDialogm_Eb__51_1(int btnId) => this.DismissViewController((ViewController) this._simpleDialogPromptViewController, ViewController.AnimationDirection.Vertical);

  [CompilerGenerated]
  public virtual void m_CPresentMasterServerUnavailableErrorDialogm_Eb__52_0(int btnId)
  {
    System.Action<MultiplayerModeSelectionFlowCoordinator> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }
}
