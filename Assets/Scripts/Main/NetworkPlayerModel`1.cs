// Decompiled with JetBrains decompiler
// Type: NetworkPlayerModel`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public abstract class NetworkPlayerModel<T> : BaseNetworkPlayerModel, INetworkPlayerModel where T : class, IConnectionManager, new()
{
  private const float kServerRefreshFrequency = 10f;
  private const float kServerTimeoutPeriod = 21f;
  [Inject]
  private readonly IPlatformUserModel _platformUserModel;
  protected Task<IAuthenticationTokenProvider> authenticationTokenProviderTask;
  private NetworkPlayerModel<T>.NetworkPlayer _localPlayer;
  private bool _networkingFailed;
  private bool _masterServerUnreachable;
  private int _currentPlayerCount;
  private readonly List<NetworkPlayerModel<T>.NetworkPlayer> _partyPlayers = new List<NetworkPlayerModel<T>.NetworkPlayer>();
  private readonly List<NetworkPlayerModel<T>.NetworkServer> _publicServers = new List<NetworkPlayerModel<T>.NetworkServer>();
  private float _lastServerRefresh = -10f;
  private bool _isRefreshing;
  private bool _filterChanged;

  public override event System.Action<int> partySizeChangedEvent;

  public event System.Action partyRefreshingEvent;

  public override event System.Action<INetworkPlayerModel> partyChangedEvent;

  public override event System.Action<INetworkPlayer> joinRequestedEvent;

  public override event System.Action<INetworkPlayer> inviteRequestedEvent;

  public override bool localPlayerIsPartyOwner
  {
    get
    {
      NetworkPlayerModel<T>.NetworkPlayer localPlayer = this._localPlayer;
      return localPlayer != null && __nonvirtual (localPlayer.isMyPartyOwner);
    }
  }

  public override bool hasNetworkingFailed => this._networkingFailed;

  public abstract string secret { get; }

  public abstract string code { get; }

  public abstract string partyOwnerId { get; }

  public override int currentPartySize => this._currentPlayerCount;

  public IEnumerable<INetworkPlayer> publicServers => (IEnumerable<INetworkPlayer>) this._publicServers;

  protected T connectionManager
  {
    get
    {
      ConnectedPlayerManager connectedPlayerManager = this.connectedPlayerManager;
      return connectedPlayerManager == null ? default (T) : connectedPlayerManager.GetConnectionManager<T>();
    }
  }

  protected override void Start() => this.authenticationTokenProviderTask = this.InitAuthenticationTokenProvider();

  protected override void Update()
  {
    base.Update();
    if (this.discoveryEnabled && !this.isConnectedOrConnecting && (double) this._lastServerRefresh < (double) Time.realtimeSinceStartup - 10.0)
      this.Refresh();
    bool flag = false;
    for (int index = this._publicServers.Count - 1; index >= 0; --index)
    {
      if (this._publicServers[index].hasTimedOut)
      {
        this._publicServers.RemoveAt(index);
        flag = true;
      }
    }
    if (!flag)
      return;
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  public override bool CreatePartyConnection<T2>(INetworkPlayerModelPartyConfig<T2> config)
  {
    if (!base.CreatePartyConnection<T2>(config))
      return false;
    switch (config)
    {
      case NetworkPlayerModel<T>.JoinMatchmakingPartyConfig matchmakingPartyConfig:
        return this.CreateConnectedPlayerManager<T>(this.GetConnectToServerParams(matchmakingPartyConfig.selectionMask, matchmakingPartyConfig.configuration, matchmakingPartyConfig.secret, matchmakingPartyConfig.code));
      case NetworkPlayerModel<T>.StartClientPartyConfig clientPartyConfig:
        // ISSUE: explicit reference operation
        if (clientPartyConfig.selectionMask != this.selectionMask || clientPartyConfig.configuration != this.configuration)
        {
          this._lastServerRefresh = -10f;
          this._filterChanged = true;
        }
        return this.CreateConnectedPlayerManager<T>(this.GetStartClientParams(clientPartyConfig.selectionMask, clientPartyConfig.configuration));
      default:
        return false;
    }
  }

  protected abstract IConnectionInitParams<T> GetConnectToServerParams(
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration,
    string secret = null,
    string code = null);

  protected abstract IConnectionInitParams<T> GetStartClientParams(
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration);

  protected abstract void RefreshPublicServers(
    BeatmapLevelSelectionMask localSelectionMask,
    GameplayServerConfiguration localConfiguration,
    System.Action<IReadOnlyList<PublicServerInfo>> onSuccess,
    System.Action<ConnectionFailedReason> onFailure);

  private async Task<IAuthenticationTokenProvider> InitAuthenticationTokenProvider()
  {
    NetworkPlayerModel<T> networkPlayerModel = this;
    UserInfo userInfo = await networkPlayerModel._platformUserModel.GetUserInfo();
    if (userInfo == null || (UnityEngine.Object) networkPlayerModel == (UnityEngine.Object) null)
    {
      networkPlayerModel._networkingFailed = true;
      throw new Exception("No authentication token provider could be created");
    }
    networkPlayerModel.HandlePlayersChanged();
    return (IAuthenticationTokenProvider) new PlatformAuthenticationTokenProvider(networkPlayerModel._platformUserModel, userInfo);
  }

  protected override IEnumerable<INetworkPlayer> GetPartyPlayers()
  {
    for (int i = 0; i < this._partyPlayers.Count; ++i)
    {
      if (this._partyPlayers[i].sortIndex != -1)
        yield return (INetworkPlayer) this._partyPlayers[i];
    }
  }

  protected override IEnumerable<INetworkPlayer> GetOtherPlayers()
  {
    foreach (INetworkPlayer publicServer in this._publicServers)
      yield return publicServer;
  }

  private void Refresh()
  {
    if (this._isRefreshing || this.hasNetworkingFailed || this.authenticationTokenProviderTask?.Result == null || this._masterServerUnreachable || (object) this.connectionManager == null)
      return;
    bool clearCurrentList = this._filterChanged;
    this._filterChanged = false;
    this._lastServerRefresh = Time.realtimeSinceStartup;
    this._isRefreshing = true;
    System.Action partyRefreshingEvent = this.partyRefreshingEvent;
    if (partyRefreshingEvent != null)
      partyRefreshingEvent();
    BeatmapLevelSelectionMask localSelectionMask = this.selectionMask;
    GameplayServerConfiguration localConfiguration = this.configuration;
    this.RefreshPublicServers(localSelectionMask, localConfiguration, (System.Action<IReadOnlyList<PublicServerInfo>>) (servers =>
    {
      if (clearCurrentList)
        this._publicServers.Clear();
      for (int index = 0; index < servers.Count; ++index)
      {
        PublicServerInfo server = servers[index];
        NetworkPlayerModel<T>.NetworkServer networkServer = this.GetServer(server.code);
        if (networkServer == null)
        {
          networkServer = new NetworkPlayerModel<T>.NetworkServer(this, server.code, localSelectionMask, localConfiguration);
          this._publicServers.Add(networkServer);
        }
        networkServer.Update(server.currentPlayerCount);
      }
      this._isRefreshing = false;
      System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
      if (partyChangedEvent == null)
        return;
      partyChangedEvent((INetworkPlayerModel) this);
    }), (System.Action<ConnectionFailedReason>) (reason =>
    {
      if (reason == ConnectionFailedReason.MultiplayerApiUnreachable)
        this._masterServerUnreachable = true;
      this._isRefreshing = false;
      System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
      if (partyChangedEvent == null)
        return;
      partyChangedEvent((INetworkPlayerModel) this);
    }));
  }

  private void HandlePlayersChanged()
  {
    this._currentPlayerCount = 0;
    for (int index = 0; index < this._partyPlayers.Count; ++index)
    {
      if (this._partyPlayers[index].sortIndex != -1)
        ++this._currentPlayerCount;
    }
    System.Action<int> sizeChangedEvent = this.partySizeChangedEvent;
    if (sizeChangedEvent != null)
      sizeChangedEvent(this.currentPartySize);
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  private void HandleInviteRequested(INetworkPlayer player)
  {
    System.Action<INetworkPlayer> inviteRequestedEvent = this.inviteRequestedEvent;
    if (inviteRequestedEvent == null)
      return;
    inviteRequestedEvent(player);
  }

  private void HandleJoinRequested(INetworkPlayer player)
  {
    System.Action<INetworkPlayer> joinRequestedEvent = this.joinRequestedEvent;
    if (joinRequestedEvent == null)
      return;
    joinRequestedEvent(player);
  }

  private void HandlePartyChanged(INetworkPlayerModel playerModel)
  {
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  public void ResetMasterServerReachability()
  {
    this._lastServerRefresh = -10f;
    this._filterChanged = true;
    this._masterServerUnreachable = false;
  }

  protected override void Connected()
  {
    this._localPlayer = new NetworkPlayerModel<T>.NetworkPlayer(this, this.connectedPlayerManager.localPlayer);
    this._partyPlayers.Add(this._localPlayer);
    this.HandlePlayersChanged();
  }

  protected override void Disconnected(DisconnectedReason disconnectedReason)
  {
    if (this._localPlayer != null)
    {
      this._partyPlayers.Remove(this._localPlayer);
      this._localPlayer = (NetworkPlayerModel<T>.NetworkPlayer) null;
      this.HandlePlayersChanged();
    }
    this.DestroyPartyConnection();
    this._lastServerRefresh = -10f;
    this._filterChanged = true;
  }

  protected override void ConnectionFailed(ConnectionFailedReason reason)
  {
    if (reason == ConnectionFailedReason.MultiplayerApiUnreachable)
      this._masterServerUnreachable = true;
    this.DestroyPartyConnection();
  }

  protected override void PlayerConnected(IConnectedPlayer player)
  {
    this._partyPlayers.Add(new NetworkPlayerModel<T>.NetworkPlayer(this, player));
    this.HandlePlayersChanged();
  }

  protected override void PlayerDisconnected(IConnectedPlayer player)
  {
    for (int index = 0; index < this._partyPlayers.Count; ++index)
    {
      if (this._partyPlayers[index].userId == player.userId)
        this._partyPlayers.RemoveAt(index);
    }
    this.HandlePlayersChanged();
  }

  protected override void PlayerStateChanged(IConnectedPlayer connectedPlayer) => this.HandlePlayersChanged();

  protected override void PlayerOrderChanged(IConnectedPlayer connectedPlayer)
  {
    this._partyPlayers.Sort((Comparison<NetworkPlayerModel<T>.NetworkPlayer>) ((a, b) => a.sortIndex.CompareTo(b.sortIndex)));
    this.HandlePlayersChanged();
  }

  private NetworkPlayerModel<T>.NetworkPlayer GetPlayer(string userId)
  {
    for (int index = 0; index < this._partyPlayers.Count; ++index)
    {
      if (this._partyPlayers[index].userId == userId)
        return this._partyPlayers[index];
    }
    return (NetworkPlayerModel<T>.NetworkPlayer) null;
  }

  private NetworkPlayerModel<T>.NetworkServer GetServer(string code)
  {
    for (int index = 0; index < this._publicServers.Count; ++index)
    {
      if (this._publicServers[index].code == code)
        return this._publicServers[index];
    }
    return (NetworkPlayerModel<T>.NetworkServer) null;
  }

  private class NetworkPlayer : INetworkPlayer
  {
    private readonly NetworkPlayerModel<T> _playerModel;
    private readonly IConnectedPlayer _connectedPlayer;

    public NetworkPlayer(NetworkPlayerModel<T> playerModel, IConnectedPlayer connectedPlayer)
    {
      this._playerModel = playerModel;
      this._connectedPlayer = connectedPlayer;
    }

    public string userId => this._connectedPlayer.userId;

    public string userName => this._connectedPlayer.userName;

    public bool isMe => this._connectedPlayer.isMe;

    public int sortIndex => this._connectedPlayer.sortIndex;

    public int currentPartySize => this._playerModel.currentPartySize;

    public BeatmapLevelSelectionMask selectionMask => this._playerModel.selectionMask;

    public GameplayServerConfiguration configuration => this._playerModel.configuration;

    public bool isMyPartyOwner => this._connectedPlayer.isConnectionOwner || this._playerModel.partyOwnerId == this.userId;

    public IConnectedPlayer connectedPlayer => this._connectedPlayer;

    public bool canJoin => false;

    public void Join()
    {
    }

    public bool requiresPassword => false;

    public void Join(string password)
    {
    }

    public bool isWaitingOnJoin => false;

    public bool canInvite => false;

    public void Invite()
    {
    }

    public bool isWaitingOnInvite => false;

    public bool canKick => this._playerModel.localPlayerIsPartyOwner && !this.isMe;

    public void Kick() => this._playerModel.connectedPlayerManager?.KickPlayer(this.connectedPlayer.userId);

    public bool canLeave => this._playerModel.isConnectedOrConnecting;

    public void Leave() => this._playerModel.DestroyPartyConnection();

    public bool canBlock => false;

    public void Block()
    {
    }

    public bool canUnblock => false;

    public void Unblock()
    {
    }

    public void SendJoinResponse(bool accept)
    {
    }

    public void SendInviteResponse(bool accept)
    {
    }
  }

  public class JoinMatchmakingPartyConfig : 
    BaseNetworkPlayerModel.PartyConfig,
    INetworkPlayerModelPartyConfig<NetworkPlayerModel<T>>
  {
    public string secret;
    public string code;
  }

  public class StartClientPartyConfig : 
    BaseNetworkPlayerModel.PartyConfig,
    INetworkPlayerModelPartyConfig<NetworkPlayerModel<T>>
  {
  }

  private class NetworkServer : INetworkPlayer
  {
    private readonly NetworkPlayerModel<T> _playerModel;
    private readonly string _code;
    private readonly BeatmapLevelSelectionMask _selectionMask;
    private readonly GameplayServerConfiguration _configuration;
    private int _currentPlayerCount;
    private float _lastUpdateTime;

    public NetworkServer(
      NetworkPlayerModel<T> playerModel,
      string code,
      BeatmapLevelSelectionMask selectionMask,
      GameplayServerConfiguration configuration)
    {
      this._playerModel = playerModel;
      this._code = code;
      this._selectionMask = selectionMask;
      this._configuration = configuration;
    }

    public void Update(int currentPlayerCount)
    {
      this._lastUpdateTime = Time.realtimeSinceStartup;
      this._currentPlayerCount = currentPlayerCount;
    }

    string INetworkPlayer.userId => this._code;

    public string code => this._code;

    string INetworkPlayer.userName => this._code;

    public string serverName => this._code;

    public bool isMe => false;

    public int currentPartySize => this._currentPlayerCount;

    public BeatmapLevelSelectionMask selectionMask => this._selectionMask;

    public GameplayServerConfiguration configuration => this._configuration;

    public IConnectedPlayer connectedPlayer => (IConnectedPlayer) null;

    public bool isMyPartyOwner => false;

    public bool hasTimedOut => (double) this._lastUpdateTime < (double) Time.realtimeSinceStartup - 21.0;

    public bool canJoin => true;

    public void Join() => this._playerModel.CreatePartyConnection<NetworkPlayerModel<T>>((INetworkPlayerModelPartyConfig<NetworkPlayerModel<T>>) new NetworkPlayerModel<T>.JoinMatchmakingPartyConfig()
    {
      code = this.code
    });

    public bool requiresPassword => false;

    public void Join(string password) => this.Join();

    public bool isWaitingOnJoin => false;

    public bool canInvite => false;

    public void Invite()
    {
    }

    public bool isWaitingOnInvite => false;

    public bool canKick => false;

    public void Kick()
    {
    }

    public bool canLeave => false;

    public void Leave()
    {
    }

    public bool canBlock => false;

    public void Block()
    {
    }

    public bool canUnblock => false;

    public void Unblock()
    {
    }

    public void SendJoinResponse(bool accept)
    {
    }

    public void SendInviteResponse(bool accept)
    {
    }
  }
}
