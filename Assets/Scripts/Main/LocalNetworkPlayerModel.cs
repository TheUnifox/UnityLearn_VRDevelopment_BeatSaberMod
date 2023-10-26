// Decompiled with JetBrains decompiler
// Type: LocalNetworkPlayerModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Zenject;

public class LocalNetworkPlayerModel : BaseNetworkPlayerModel, INetworkPlayerModel
{
  [SerializeField]
  protected LocalNetworkDiscoveryManager _discoveryManager;
  [Inject]
  protected readonly IPlatformUserModel _platformUserModel;
  [Inject]
  protected readonly INetworkConfig _networkConfig;
  protected readonly List<LocalNetworkPlayerModel.LocalNetworkPlayer> _partyPlayers = new List<LocalNetworkPlayerModel.LocalNetworkPlayer>();
  protected readonly List<LocalNetworkPlayerModel.LocalNetworkPlayer> _otherPlayers = new List<LocalNetworkPlayerModel.LocalNetworkPlayer>();
  protected LocalNetworkPlayerModel.LocalNetworkPlayer _localPlayer;
  protected bool _networkingFailed;
  protected bool _partyEnabled;
  protected INetworkPlayerModel _partyManager;
  protected readonly BasicConnectionRequestHandler _connectionRequestHandler = new BasicConnectionRequestHandler();

  private bool canInvitePlayers => (this.localPlayerIsPartyOwner || this._localPlayer.configuration.invitePolicy == InvitePolicy.AnyoneCanInvite) && this._partyPlayers.Count < this._localPlayer.configuration.maxPlayerCount;

  private bool hasConnectedPeers => this._partyPlayers.Count > 1;

  public override int currentPartySize => this._partyPlayers.Count;

  public override bool discoveryEnabled
  {
    get => this._discoveryManager.enableBroadcasting;
    set => this._discoveryManager.enableBroadcasting = value;
  }

  public override event System.Action<int> partySizeChangedEvent;

  public override event System.Action<INetworkPlayerModel> partyChangedEvent;

  public override event System.Action<INetworkPlayer> joinRequestedEvent;

  public override event System.Action<INetworkPlayer> inviteRequestedEvent;

  public override bool localPlayerIsPartyOwner => this._localPlayer.isPartyOwner;

  public IEnumerable<INetworkPlayer> otherPlayers => this.GetOtherPlayers();

  public override bool hasNetworkingFailed => this._networkingFailed;

  public LiteNetLibConnectionManager liteNetLibConnectionManager => this.connectedPlayerManager?.GetConnectionManager<LiteNetLibConnectionManager>();

  protected override async void Start()
  {
    LocalNetworkPlayerModel playerModel = this;
    UserInfo userInfo = await playerModel._platformUserModel.GetUserInfo();
    if (userInfo == null || (UnityEngine.Object) playerModel == (UnityEngine.Object) null)
    {
      playerModel._networkingFailed = true;
    }
    else
    {
      string hashedUserId = NetworkUtility.GetHashedUserId(userInfo.platformUserId, userInfo.platform.ToAuthenticationTokenPlatform());
      playerModel._localPlayer = new LocalNetworkPlayerModel.LocalNetworkPlayer(playerModel, hashedUserId, userInfo.userName, isMe: true);
      playerModel._partyPlayers.Add(playerModel._localPlayer);
      playerModel._discoveryManager.peerUpdatedEvent += new LocalNetworkDiscoveryManager.PeerUpdatedDelegate(playerModel.HandlePeerUpdate);
      playerModel._discoveryManager.joinRequestedEvent += new LocalNetworkDiscoveryManager.JoinRequestedDelegate(playerModel.HandleJoinRequest);
      playerModel._discoveryManager.joinRespondedEvent += new LocalNetworkDiscoveryManager.JoinRespondedDelegate(playerModel.HandleJoinResponse);
      playerModel._discoveryManager.inviteRequestedEvent += new LocalNetworkDiscoveryManager.InviteRequestedDelegate(playerModel.HandleInviteRequest);
      playerModel._discoveryManager.inviteRespondedEvent += new LocalNetworkDiscoveryManager.InviteRespondedDelegate(playerModel.HandleInviteResponse);
      playerModel._discoveryManager.Init(playerModel._networkConfig.discoveryPort, hashedUserId, NetworkUtility.EncryptName(playerModel._localPlayer.userName));
      playerModel.RefreshLocalPlayer();
    }
  }

  protected override void Update()
  {
    base.Update();
    if (this._otherPlayers.Count == 0)
      return;
    bool forcePlayersChanged = false;
    for (int index = this._otherPlayers.Count - 1; index >= 0; --index)
    {
      LocalNetworkPlayerModel.LocalNetworkPlayer otherPlayer = this._otherPlayers[index];
      if (otherPlayer.HasFailedToConnect())
        forcePlayersChanged = true;
      if (otherPlayer.isTimedOut)
      {
        this._otherPlayers.RemoveAt(index);
        forcePlayersChanged = true;
      }
    }
    this.RefreshLocalPlayer(forcePlayersChanged);
  }

  protected override void OnDestroy()
  {
    this.DestroyPartyConnection();
    if (!((UnityEngine.Object) this._discoveryManager != (UnityEngine.Object) null))
      return;
    this._discoveryManager.peerUpdatedEvent -= new LocalNetworkDiscoveryManager.PeerUpdatedDelegate(this.HandlePeerUpdate);
    this._discoveryManager.joinRequestedEvent -= new LocalNetworkDiscoveryManager.JoinRequestedDelegate(this.HandleJoinRequest);
    this._discoveryManager.joinRespondedEvent -= new LocalNetworkDiscoveryManager.JoinRespondedDelegate(this.HandleJoinResponse);
    this._discoveryManager.inviteRequestedEvent -= new LocalNetworkDiscoveryManager.InviteRequestedDelegate(this.HandleInviteRequest);
    this._discoveryManager.inviteRespondedEvent -= new LocalNetworkDiscoveryManager.InviteRespondedDelegate(this.HandleInviteResponse);
  }

  protected override IEnumerable<INetworkPlayer> GetPartyPlayers() => (IEnumerable<INetworkPlayer>) this._partyPlayers;

  protected override IEnumerable<INetworkPlayer> GetOtherPlayers() => (IEnumerable<INetworkPlayer>) this._otherPlayers;

  public virtual bool TryGetPlayer(
    string userId,
    out LocalNetworkPlayerModel.LocalNetworkPlayer player)
  {
    for (int index = 0; index < this._partyPlayers.Count; ++index)
    {
      if (this._partyPlayers[index].userId == userId)
      {
        player = this._partyPlayers[index];
        return true;
      }
    }
    for (int index = 0; index < this._otherPlayers.Count; ++index)
    {
      if (this._otherPlayers[index].userId == userId)
      {
        player = this._otherPlayers[index];
        return true;
      }
    }
    player = (LocalNetworkPlayerModel.LocalNetworkPlayer) null;
    return false;
  }

  public virtual LocalNetworkPlayerModel.LocalNetworkPlayer GetPlayer(string userId)
  {
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    this.TryGetPlayer(userId, out player);
    return player;
  }

  public virtual void RefreshLocalPlayer(bool forcePlayersChanged = false)
  {
    if (this._localPlayer == null)
      return;
    bool isPartyOwner = this._partyEnabled && (this.connectedPlayerManager == null || this.connectedPlayerManager.isConnectionOwner);
    this._discoveryManager.isPartyOwner = isPartyOwner;
    if (!(this._localPlayer.Update(isPartyOwner, this._discoveryManager.currentPartySize, this._discoveryManager.selectionMask, this._discoveryManager.configuration) | forcePlayersChanged))
      return;
    this.HandlePlayersChanged();
  }

  public virtual void HandlePeerUpdate(
    string userId,
    IPAddress ipAddress,
    string encryptedUserName,
    int currentPartySize,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    bool flag = false;
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(userId, out player))
    {
      string userName = NetworkUtility.DecryptName(encryptedUserName);
      this.Log("[HandlePeerUpdate] Created new player with userId: " + userId + " and userName: " + userName);
      player = new LocalNetworkPlayerModel.LocalNetworkPlayer(this, userId, userName, ipAddress);
      this._otherPlayers.Add(player);
      flag = true;
    }
    if (!(flag | player.Update(isPartyOwner, currentPartySize, selectionMask, configuration)))
      return;
    this.HandlePlayersChanged();
  }

  public virtual void SendJoinRequest(LocalNetworkPlayerModel.LocalNetworkPlayer player)
  {
    this.Log("[SendJoinRequest] Sending Join Request");
    this._discoveryManager.SendJoinRequest(player.ipAddress);
  }

  public virtual void HandleJoinRequest(
    string userId,
    IPAddress ipAddress,
    string encryptedUserName)
  {
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(userId, out player))
    {
      string userName = NetworkUtility.DecryptName(encryptedUserName);
      player = new LocalNetworkPlayerModel.LocalNetworkPlayer(this, userId, userName, ipAddress);
    }
    player.SetJoinRequested();
    if (!this.localPlayerIsPartyOwner || player.isBlocked)
    {
      this.Log("[HandleJoinRequest] Declining request because we are connected to a server");
      player.SendJoinResponse(false);
    }
    else if (this._localPlayer.configuration.discoveryPolicy == DiscoveryPolicy.Public || player.allowedJoinToMyParty)
    {
      this.Log("[HandleJoinRequest] Accepting request because we allow anyone to join");
      player.SendJoinResponse(true);
    }
    else
    {
      this.Log("[HandleJoinRequest] Passing event to application to handle");
      System.Action<INetworkPlayer> joinRequestedEvent = this.joinRequestedEvent;
      if (joinRequestedEvent == null)
        return;
      joinRequestedEvent((INetworkPlayer) player);
    }
  }

  public virtual void SendJoinResponse(
    LocalNetworkPlayerModel.LocalNetworkPlayer player,
    bool allowJoin)
  {
    string secret = (string) null;
    int multiplayerPort = 0;
    if (allowJoin)
    {
      this.Log("[SendJoinResponse] They are allowed to join, so starting server");
      this.TryStartServer();
      if (this.liteNetLibConnectionManager != null)
      {
        secret = this._connectionRequestHandler.secret;
        multiplayerPort = this.liteNetLibConnectionManager.port;
      }
      else
      {
        this.Log("ConnectionManager is not a LiteNetLibConnectionManager! Disallowing join!");
        allowJoin = false;
      }
    }
    this.Log("[SendJoinResponse] Sending response: " + allowJoin.ToString());
    this._discoveryManager.SendJoinResponse(player.ipAddress, secret, multiplayerPort, player.isBlocked, this.localPlayerIsPartyOwner, this.selectionMask, this.configuration);
    this.HandlePlayersChanged();
  }

  public virtual void HandleJoinResponse(
    string id,
    string secret,
    int multiplayerPort,
    bool blocked,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(id, out player))
      return;
    this.Log("[HandleJoinResponse] Join was " + (string.IsNullOrEmpty(secret) ? "declined" : "accepted"));
    player.SetJoinResponse(isPartyOwner, selectionMask, configuration, secret, multiplayerPort, blocked);
    this.HandlePlayersChanged();
  }

  public virtual void SendInviteRequest(LocalNetworkPlayerModel.LocalNetworkPlayer player)
  {
    this.Log("[SendInviteRequest] Sending invite to player, starting server");
    this.TryStartServer();
    if (this.liteNetLibConnectionManager != null)
      this._discoveryManager.SendInviteRequest(player.ipAddress, this._connectionRequestHandler.secret, this.liteNetLibConnectionManager.port, this.localPlayerIsPartyOwner, this.selectionMask, this.configuration);
    this.HandlePlayersChanged();
  }

  public virtual void HandleInviteRequest(
    string userId,
    IPAddress ipAddress,
    string encryptedUserName,
    string secret,
    int multiplayerPort,
    bool isPartyOwner,
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    if (string.IsNullOrEmpty(secret))
      return;
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(userId, out player))
    {
      string userName = NetworkUtility.DecryptName(encryptedUserName);
      player = new LocalNetworkPlayerModel.LocalNetworkPlayer(this, userId, userName, ipAddress);
    }
    player.SetInvited(isPartyOwner, selectionMask, configuration, secret, multiplayerPort);
    if (player.isBlocked)
    {
      this.Log("[HandleInviteRequest] Declining invite because player is blocked");
      player.SendInviteResponse(false);
    }
    else
    {
      this.Log("[HandleInviteRequest] Allowing user to decide how to respond");
      System.Action<INetworkPlayer> inviteRequestedEvent = this.inviteRequestedEvent;
      if (inviteRequestedEvent != null)
        inviteRequestedEvent((INetworkPlayer) player);
    }
    this.HandlePlayersChanged();
  }

  public virtual void SendInviteResponse(
    LocalNetworkPlayerModel.LocalNetworkPlayer player,
    bool acceptInvite)
  {
    this.Log("[SendInviteResponse] Responding: " + acceptInvite.ToString());
    this._discoveryManager.SendInviteResponse(player.ipAddress, acceptInvite, player.isBlocked);
    this.HandlePlayersChanged();
  }

  public virtual void HandleInviteResponse(string userId, bool accepted, bool blocked)
  {
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(userId, out player))
      return;
    this.Log("[HandleInviteResponse] Invite was accepted! The player should already be joining");
    player.SetInviteResponse(accepted, blocked);
    this.HandlePlayersChanged();
  }

  public virtual bool ConnectToPeer(LocalNetworkPlayerModel.LocalNetworkPlayer player)
  {
    this.Log("[ConnectToPeer] Connecting to server " + player.userId);
    this._connectionRequestHandler.secret = player.secret;
    LiteNetLibConnectionManager.ConnectToServerParams initParams = new LiteNetLibConnectionManager.ConnectToServerParams();
    initParams.userId = this._localPlayer.userId;
    initParams.userName = this._localPlayer.userName;
    initParams.port = this._networkConfig.partyPort;
    initParams.connectionRequestHandler = (IConnectionRequestHandler) this._connectionRequestHandler;
    initParams.serverUserId = player.userId;
    initParams.serverUserName = player.userName;
    initParams.endPoint = new IPEndPoint(player.ipAddress, player.multiplayerPort);
    initParams.enableBackgroundSentry = true;
    return this.CreateConnectedPlayerManager<LiteNetLibConnectionManager>((IConnectionInitParams<LiteNetLibConnectionManager>) initParams);
  }

  protected override void PlayerConnected(IConnectedPlayer connectedPlayer)
  {
    this.Log("[HandleSynchronousPeerConnected] Synchronous player connected: " + connectedPlayer.userName + " (" + connectedPlayer.userId + ")");
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(connectedPlayer.userId, out player))
    {
      this.Log("[HandleSynchronousPeerConnected] Peer is new, adding to player list!");
      player = new LocalNetworkPlayerModel.LocalNetworkPlayer(this, connectedPlayer.userId, connectedPlayer.userName);
    }
    if (!player.SetConnected(connectedPlayer))
      return;
    this._otherPlayers.Remove(player);
    this._partyPlayers.Add(player);
    this.RefreshLocalPlayer();
  }

  protected override void ConnectionFailed(ConnectionFailedReason reason)
  {
    for (int index = 0; index < this._otherPlayers.Count; ++index)
    {
      LocalNetworkPlayerModel.LocalNetworkPlayer otherPlayer = this._otherPlayers[index];
      if (otherPlayer.isConnecting)
        otherPlayer.SetDisconnected();
    }
    this.HandlePlayersChanged();
  }

  protected override void PlayerDisconnected(IConnectedPlayer connectedPlayer)
  {
    LocalNetworkPlayerModel.LocalNetworkPlayer player;
    if (!this.TryGetPlayer(connectedPlayer.userId, out player))
      return;
    this.Log("[HandleSynchronousPeerDisconnected] Synchronous player disconnected: " + connectedPlayer.userName + " (" + connectedPlayer.userId + ")");
    if (!player.SetDisconnected())
      return;
    this._partyPlayers.Remove(player);
    if (!player.isTimedOut)
      this._otherPlayers.Add(player);
    this.RefreshLocalPlayer();
  }

  public virtual void DisconnectPeer(LocalNetworkPlayerModel.LocalNetworkPlayer player)
  {
    if (this.localPlayerIsPartyOwner)
    {
      this.connectedPlayerManager?.KickPlayer(player.userId);
    }
    else
    {
      if (!player.isMyPartyOwner)
        return;
      this.DestroyPartyConnection();
    }
  }

  public virtual void HandlePlayersChanged()
  {
    System.Action<int> sizeChangedEvent = this.partySizeChangedEvent;
    if (sizeChangedEvent != null)
      sizeChangedEvent(this.currentPartySize);
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  protected override void PartySizeChanged(int currentPartySize) => this._discoveryManager.currentPartySize = (int) (byte) currentPartySize;

  public override bool CreatePartyConnection<T>(INetworkPlayerModelPartyConfig<T> createConfig)
  {
    if (!base.CreatePartyConnection<T>(createConfig))
      return false;
    this._discoveryManager.isPartyOwner = true;
    this._discoveryManager.currentPartySize = this.partyManager.currentPartySize;
    this._discoveryManager.configuration = this.partyManager.configuration;
    this._discoveryManager.selectionMask = this.partyManager.selectionMask;
    return false;
  }

  public override void DestroyPartyConnection()
  {
    base.DestroyPartyConnection();
    this._partyEnabled = false;
    this._discoveryManager.isPartyOwner = false;
    this._discoveryManager.configuration = new GameplayServerConfiguration();
    this.RefreshLocalPlayer();
  }

  protected override void ConnectedPlayerManagerChanged() => this.RefreshLocalPlayer();

  public virtual bool TryStartServer()
  {
    if (this.isConnectedOrConnecting || !this._partyEnabled)
      return false;
    this._connectionRequestHandler.secret = NetworkUtility.GenerateId();
    LiteNetLibConnectionManager.StartServerParams initParams = new LiteNetLibConnectionManager.StartServerParams();
    initParams.userId = this._localPlayer.userId;
    initParams.userName = this._localPlayer.userName;
    initParams.connectionRequestHandler = (IConnectionRequestHandler) this._connectionRequestHandler;
    initParams.port = this._networkConfig.partyPort;
    initParams.enableBackgroundSentry = true;
    return this.CreateConnectedPlayerManager<LiteNetLibConnectionManager>((IConnectionInitParams<LiteNetLibConnectionManager>) initParams);
  }

  public class LocalNetworkPlayer : INetworkPlayer
  {
    protected const float kPeerBroadcastTimeout = 30f;
    protected const float kConnectionTimeout = 5f;
    protected const float kRequestTimeout = 120f;
    protected readonly LocalNetworkPlayerModel _playerModel;
    protected readonly string _userId;
    protected readonly string _userName;
    protected readonly IPAddress _ipAddress;
    protected readonly bool _isMe;
    protected bool _isPartyOwner;
    protected float _joinRequestTime;
    protected float _inviteSentTime;
    protected bool _isBlocked;
    protected bool _hasBlockedMe;
    protected string _secret;
    protected int _multiplayerPort;
    protected IConnectedPlayer _connectedPlayer;
    protected float _connectingStartTime;
    protected int _currentPartySize;
    protected BeatmapLevelSelectionMask _selectionMask;
    protected GameplayServerConfiguration _configuration;
    protected float _lastUpdateTime;
    protected bool _allowedJoinToMyParty;
    protected bool _requestedToJoinMyParty;
    protected bool _invitedMeToJoinTheirParty;

    public string userId => this._userId;

    public string userName => this._userName;

    public IPAddress ipAddress => this._ipAddress;

    public bool isMe => this._isMe;

    public bool isPartyOwner => this._isPartyOwner;

    public int currentPartySize => this._currentPartySize;

    public BeatmapLevelSelectionMask selectionMask => this._selectionMask;

    public GameplayServerConfiguration configuration => this._configuration;

    public IConnectedPlayer connectedPlayer => this._connectedPlayer;

    public bool isBlocked => this._isBlocked;

    public bool hasBlockedMe => this._hasBlockedMe;

    public string secret => this._secret;

    public int multiplayerPort => this._multiplayerPort;

    public bool isMyPartyOwner
    {
      get
      {
        if (!this.isPartyOwner)
          return false;
        return this.isMe || this.isConnected || this.isConnecting;
      }
    }

    public bool isConnected => this._connectedPlayer != null && this._connectedPlayer.isConnected;

    public bool allowedJoinToMyParty => this._allowedJoinToMyParty;

    public bool isConnecting => (double) this._connectingStartTime > 0.0 && (double) this._connectingStartTime > (double) Time.realtimeSinceStartup - 5.0;

    private bool wasConnecting => (double) this._connectingStartTime > 0.0 && (double) this._connectingStartTime <= (double) Time.realtimeSinceStartup - 5.0;

    public bool isTimedOut => !this.isMe && !this.isConnected && !this.isConnecting && (double) this._lastUpdateTime < (double) Time.realtimeSinceStartup - 30.0;

    public LocalNetworkPlayer(
      LocalNetworkPlayerModel playerModel,
      string userId,
      string userName,
      IPAddress ipAddress = null,
      bool isMe = false)
    {
      this._playerModel = playerModel;
      this._userId = userId;
      this._userName = userName;
      this._ipAddress = ipAddress;
      this._isMe = isMe;
    }

    public bool isWaitingOnJoin => (double) this._inviteSentTime > 0.0 && (double) this._inviteSentTime > (double) Time.realtimeSinceStartup - 120.0;

    public bool canJoin => !this.hasBlockedMe && this.isPartyOwner && (!this.isWaitingOnJoin || this._invitedMeToJoinTheirParty) && !this.isMe && this.ipAddress != null && !this.isConnected && !this.isConnecting && this._currentPartySize < this._configuration.maxPlayerCount;

    public virtual void Join()
    {
      if (!this.canJoin)
        return;
      if (this._invitedMeToJoinTheirParty)
        this.SendInviteResponse(true);
      this._playerModel.SendJoinRequest(this);
      this._joinRequestTime = Time.realtimeSinceStartup;
    }

    public bool requiresPassword => false;

    public virtual void Join(string password)
    {
    }

    public bool isWaitingOnInvite => (double) this._inviteSentTime > 0.0 && (double) this._inviteSentTime > (double) Time.realtimeSinceStartup - 120.0;

    public bool canInvite => !this.hasBlockedMe && (!this.isWaitingOnInvite || this._requestedToJoinMyParty) && !this.isMe && this.ipAddress != null && !this.isConnected && !this.isConnecting && this._playerModel.canInvitePlayers;

    public virtual void Invite()
    {
      if (!this.canInvite)
        return;
      if (this._requestedToJoinMyParty)
      {
        this.SendJoinResponse(true);
      }
      else
      {
        this._allowedJoinToMyParty = true;
        this._inviteSentTime = Time.realtimeSinceStartup;
        this._playerModel.SendInviteRequest(this);
      }
    }

    public bool canKick => this.isConnected && this._playerModel.localPlayerIsPartyOwner;

    public virtual void Kick()
    {
      this._allowedJoinToMyParty = false;
      this._playerModel.DisconnectPeer(this);
    }

    public bool canLeave
    {
      get
      {
        if (this._isMe && this._playerModel.hasConnectedPeers)
          return true;
        return !this._isMe && this.isMyPartyOwner;
      }
    }

    public virtual void Leave() => this._playerModel.DestroyPartyConnection();

    public bool canBlock => !this._isMe && !this.isBlocked && !this.isConnected && !this.isConnecting;

    public virtual void Block() => this._isBlocked = true;

    public bool canUnblock => this.isBlocked;

    public virtual void Unblock() => this._isBlocked = false;

    public virtual void SendJoinResponse(bool accept)
    {
      if (!this._requestedToJoinMyParty)
        return;
      this._requestedToJoinMyParty = false;
      this._allowedJoinToMyParty = accept;
      this._playerModel.SendJoinResponse(this, accept);
    }

    public virtual void SendInviteResponse(bool accept)
    {
      if (!this._invitedMeToJoinTheirParty)
        return;
      this._invitedMeToJoinTheirParty = false;
      if (accept)
        this.Connect();
      else
        this._secret = (string) null;
      this._playerModel.SendInviteResponse(this, accept);
    }

    public virtual bool Update(
      bool isPartyOwner,
      int currentPartySize,
      BeatmapLevelSelectionMask selectionMask,
      GameplayServerConfiguration configuration)
    {
      bool flag = false;
      if (this._isPartyOwner != isPartyOwner)
      {
        this._isPartyOwner = isPartyOwner;
        flag = true;
      }
      if (this._currentPartySize != currentPartySize)
      {
        this._currentPartySize = currentPartySize;
        flag = true;
      }
      if (this._selectionMask != selectionMask)
      {
        this._selectionMask = selectionMask;
        flag = true;
      }
      if (this._configuration != configuration)
      {
        this._configuration = configuration;
        flag = true;
      }
      this._lastUpdateTime = Time.realtimeSinceStartup;
      return flag;
    }

    public virtual bool HasFailedToConnect()
    {
      if (this.isMe || this.isConnecting || this.isConnected || !this.wasConnecting)
        return false;
      this._connectingStartTime = 0.0f;
      this._secret = (string) null;
      return true;
    }

    public virtual void SetInvited(
      bool isPartyOwner,
      BeatmapLevelSelectionMask selectionMask,
      GameplayServerConfiguration configuration,
      string secret,
      int multiplayerPort)
    {
      this._isPartyOwner = isPartyOwner;
      this._selectionMask = selectionMask;
      this._configuration = configuration;
      this._invitedMeToJoinTheirParty = true;
      this._secret = secret;
      this._multiplayerPort = multiplayerPort;
    }

    public virtual void SetJoinResponse(
      bool isPartyOwner,
      BeatmapLevelSelectionMask selectionMask,
      GameplayServerConfiguration configuration,
      string secret,
      int multiplayerPort,
      bool blocked)
    {
      this._joinRequestTime = 0.0f;
      this._isPartyOwner = isPartyOwner;
      this._selectionMask = selectionMask;
      this._configuration = configuration;
      this._secret = secret;
      this._multiplayerPort = multiplayerPort;
      this._hasBlockedMe = blocked;
      this.Connect();
    }

    public virtual void SetInviteResponse(bool accepted, bool blocked)
    {
      this._inviteSentTime = 0.0f;
      this._hasBlockedMe = blocked;
      if (!accepted)
        return;
      this._connectingStartTime = Time.realtimeSinceStartup;
    }

    public virtual void SetJoinRequested() => this._requestedToJoinMyParty = true;

    public virtual bool SetConnected(IConnectedPlayer connectedPlayer)
    {
      if (this._connectedPlayer == connectedPlayer)
        return false;
      this._connectedPlayer = connectedPlayer;
      return true;
    }

    public virtual bool SetDisconnected()
    {
      this._secret = (string) null;
      this._connectingStartTime = 0.0f;
      if (this._connectedPlayer == null)
        return false;
      this._connectedPlayer = (IConnectedPlayer) null;
      return true;
    }

    private bool isConnectable => !this.hasBlockedMe && !string.IsNullOrEmpty(this.secret) && this.ipAddress != null;

    public virtual void Connect()
    {
      if (!this.isConnectable)
        return;
      this._connectingStartTime = Time.realtimeSinceStartup;
      this._playerModel.ConnectToPeer(this);
    }
  }

  public class CreatePartyConfig : 
    BaseNetworkPlayerModel.PartyConfig,
    INetworkPlayerModelPartyConfig<LocalNetworkPlayerModel>
  {
  }
}
