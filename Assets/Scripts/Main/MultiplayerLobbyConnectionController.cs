// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyConnectionController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Zenject;

public class MultiplayerLobbyConnectionController
{
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly IUnifiedNetworkPlayerModel _unifiedNetworkPlayerModel;
  [CompilerGenerated]
  protected MultiplayerLobbyConnectionController.LobbyConnectionState m_CconnectionState;
  [CompilerGenerated]
  protected MultiplayerLobbyConnectionController.LobbyConnectionType m_CconnectionType;
  [CompilerGenerated]
  protected ConnectionFailedReason m_CconnectionFailedReason;
  protected UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig _partyConfig;
  protected int _retryAttemptsLeft;

  public event System.Action connectionSuccessEvent;

  public event System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason> connectionFailedEvent;

  public MultiplayerLobbyConnectionController.LobbyConnectionState connectionState
  {
    get => this.m_CconnectionState;
    private set => this.m_CconnectionState = value;
  }

  public MultiplayerLobbyConnectionController.LobbyConnectionType connectionType
  {
    get => this.m_CconnectionType;
    private set => this.m_CconnectionType = value;
  }

  public ConnectionFailedReason connectionFailedReason
  {
    get => this.m_CconnectionFailedReason;
    private set => this.m_CconnectionFailedReason = value;
  }

  public virtual void CreateParty(CreateServerFormData data)
  {
    this._multiplayerSessionManager.connectedEvent += new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent += new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailed);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connecting;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.PartyHost;
    this._partyConfig = new UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig()
    {
      secret = NetworkUtility.GenerateId(),
      selectionMask = new BeatmapLevelSelectionMask(data.difficulties, data.modifiers, data.songPacks),
      configuration = new GameplayServerConfiguration(data.maxPlayers, data.netDiscoverable ? DiscoveryPolicy.Public : DiscoveryPolicy.WithCode, data.allowInviteOthers ? InvitePolicy.AnyoneCanInvite : InvitePolicy.OnlyConnectionOwnerCanInvite, data.gameplayServerMode, data.songSelectionMode, data.gameplayServerControlSettings)
    };
    if (!this._unifiedNetworkPlayerModel.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) this._partyConfig))
      this.HandleMultiplayerSessionManagerConnectionFailed(ConnectionFailedReason.Unknown);
    else
      this._multiplayerSessionManager.SetMaxPlayerCount(this._unifiedNetworkPlayerModel.configuration.maxPlayerCount);
  }

  public virtual void ConnectToParty(string serverCode)
  {
    this._multiplayerSessionManager.connectedEvent += new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent += new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailed);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connecting;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.PartyClient;
    this._partyConfig = new UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig()
    {
      code = serverCode,
      selectionMask = new BeatmapLevelSelectionMask(BeatmapDifficultyMask.All, GameplayModifierMask.All, SongPackMask.all),
      configuration = new GameplayServerConfiguration(5, DiscoveryPolicy.WithCode, InvitePolicy.AnyoneCanInvite, GameplayServerMode.Countdown, SongSelectionMode.Vote, GameplayServerControlSettings.All)
    };
    if (this._unifiedNetworkPlayerModel.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) this._partyConfig))
      return;
    this.HandleMultiplayerSessionManagerConnectionFailed(ConnectionFailedReason.Unknown);
  }

  public virtual void CreateOrConnectToDestinationParty(
    SelectMultiplayerLobbyDestination lobbyDestination)
  {
    this._partyConfig = new UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig()
    {
      secret = lobbyDestination.lobbySecret,
      code = lobbyDestination.lobbyCode,
      selectionMask = new BeatmapLevelSelectionMask(BeatmapDifficultyMask.All, GameplayModifierMask.All, SongPackMask.all),
      configuration = new GameplayServerConfiguration(5, DiscoveryPolicy.Hidden, InvitePolicy.AnyoneCanInvite, GameplayServerMode.Managed, SongSelectionMode.OwnerPicks, GameplayServerControlSettings.All)
    };
    this._multiplayerSessionManager.connectedEvent += new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent += new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailed);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connecting;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.PartyClient;
    if (!this._unifiedNetworkPlayerModel.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) this._partyConfig))
      this.HandleMultiplayerSessionManagerConnectionFailed(ConnectionFailedReason.Unknown);
    else
      this._multiplayerSessionManager.SetMaxPlayerCount(this._unifiedNetworkPlayerModel.configuration.maxPlayerCount);
  }

  public virtual void ConnectToServer(INetworkPlayer server, string password = null)
  {
    this._multiplayerSessionManager.connectedEvent += new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent += new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailed);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connecting;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.PartyClient;
    server.Join(password);
  }

  public virtual void ConnectToMatchmaking(
    BeatmapDifficultyMask beatmapDifficultyMask,
    SongPackMask songPackMask,
    bool allowSongSelection)
  {
    this._multiplayerSessionManager.connectedEvent += new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent += new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailedWithRetry);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connecting;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.QuickPlay;
    this._retryAttemptsLeft = 0;
    this._partyConfig = new UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig()
    {
      selectionMask = new BeatmapLevelSelectionMask(beatmapDifficultyMask, GameplayModifierMask.NoFail, songPackMask),
      configuration = new GameplayServerConfiguration(5, DiscoveryPolicy.Public, InvitePolicy.NobodyCanInvite, allowSongSelection ? GameplayServerMode.Countdown : GameplayServerMode.QuickStartOneSong, allowSongSelection ? SongSelectionMode.Vote : SongSelectionMode.Random, GameplayServerControlSettings.None)
    };
    if (this._unifiedNetworkPlayerModel.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) this._partyConfig))
      return;
    this.HandleMultiplayerSessionManagerConnectionFailed(ConnectionFailedReason.Unknown);
  }

  public virtual void LeaveLobby()
  {
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.None;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.None;
    this._unifiedNetworkPlayerModel.DestroyPartyConnection();
  }

  public virtual void ClearCurrentConnection()
  {
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.None;
    this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.None;
  }

  public virtual void HandleMultiplayerSessionManagerConnected()
  {
    this._multiplayerSessionManager.connectedEvent -= new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent -= new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailed);
    this._multiplayerSessionManager.connectionFailedEvent -= new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailedWithRetry);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connected;
    System.Action connectionSuccessEvent = this.connectionSuccessEvent;
    if (connectionSuccessEvent == null)
      return;
    connectionSuccessEvent();
  }

  public virtual void HandleMultiplayerSessionManagerConnectionFailed(ConnectionFailedReason reason)
  {
    this._multiplayerSessionManager.connectedEvent -= new System.Action(this.HandleMultiplayerSessionManagerConnected);
    this._multiplayerSessionManager.connectionFailedEvent -= new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailed);
    this._multiplayerSessionManager.connectionFailedEvent -= new System.Action<ConnectionFailedReason>(this.HandleMultiplayerSessionManagerConnectionFailedWithRetry);
    this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.ConnectionFailed;
    this.connectionFailedReason = reason;
    System.Action<MultiplayerLobbyConnectionController.LobbyConnectionType, ConnectionFailedReason> connectionFailedEvent = this.connectionFailedEvent;
    if (connectionFailedEvent == null)
      return;
    connectionFailedEvent(this.connectionType, reason);
  }

  public virtual void HandleMultiplayerSessionManagerConnectionFailedWithRetry(
    ConnectionFailedReason reason)
  {
    if (this._retryAttemptsLeft > 0 && this.connectionType == MultiplayerLobbyConnectionController.LobbyConnectionType.QuickPlay && (reason == ConnectionFailedReason.ServerAtCapacity || reason == ConnectionFailedReason.ServerIsTerminating || reason == ConnectionFailedReason.ServerUnreachable || reason == ConnectionFailedReason.MultiplayerApiUnreachable))
    {
      this.connectionState = MultiplayerLobbyConnectionController.LobbyConnectionState.Connecting;
      this.connectionType = MultiplayerLobbyConnectionController.LobbyConnectionType.QuickPlay;
      --this._retryAttemptsLeft;
      if (this._unifiedNetworkPlayerModel.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) this._partyConfig))
        return;
    }
    this.HandleMultiplayerSessionManagerConnectionFailed(reason);
  }

  public enum LobbyConnectionState
  {
    None,
    Connecting,
    Connected,
    ConnectionFailed,
  }

  public enum LobbyConnectionType
  {
    None,
    PartyHost,
    PartyClient,
    QuickPlay,
  }
}
