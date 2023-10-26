// Decompiled with JetBrains decompiler
// Type: UnifiedNetworkPlayerModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using Zenject;

public class UnifiedNetworkPlayerModel : 
  IUnifiedNetworkPlayerModel,
  INetworkPlayerModel,
  IInitializable,
  IDisposable
{
  [Inject]
  protected readonly GameLiftNetworkPlayerModel _gameLiftNetworkPlayerModel;
  [Inject]
  protected readonly PlatformNetworkPlayerModel _platformNetworkPlayerModel;
  [Inject]
  protected readonly LocalNetworkPlayerModel _localNetworkPlayerModel;
  protected PartyMessageHandler _partyMessageHandler;
  protected PartyMessageHandler _friendPartyMessageHandler;
  protected PartyMessageHandler _localNetworkPartyMessageHandler;
  protected UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType _activeNetworkPlayerModelType;

  public event System.Action<INetworkPlayerModel> connectedPlayerManagerCreatedEvent;

  public event System.Action<INetworkPlayerModel> connectedPlayerManagerDestroyedEvent;

  public event System.Action<int> partySizeChangedEvent;

  public event System.Action partyRefreshingEvent;

  public event System.Action<INetworkPlayerModel> partyChangedEvent;

  public event System.Action<INetworkPlayer> joinRequestedEvent;

  public event System.Action<INetworkPlayer> inviteRequestedEvent;

  private INetworkPlayerModel activeNetworkPlayerModel
  {
    get
    {
      switch (this._activeNetworkPlayerModelType)
      {
        case UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.GameLift:
          return (INetworkPlayerModel) this._gameLiftNetworkPlayerModel;
        case UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.Platform:
          return (INetworkPlayerModel) this._platformNetworkPlayerModel;
        case UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.LocalNetwork:
          return (INetworkPlayerModel) this._localNetworkPlayerModel;
        default:
          return (INetworkPlayerModel) null;
      }
    }
  }

  public UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType activeNetworkPlayerModelType => this._activeNetworkPlayerModelType;

  public bool localPlayerIsPartyOwner
  {
    get
    {
      INetworkPlayerModel networkPlayerModel = this.activeNetworkPlayerModel;
      return networkPlayerModel != null && networkPlayerModel.localPlayerIsPartyOwner;
    }
  }

  public bool hasNetworkingFailed
  {
    get
    {
      INetworkPlayerModel networkPlayerModel = this.activeNetworkPlayerModel;
      return networkPlayerModel != null && networkPlayerModel.hasNetworkingFailed;
    }
  }

  public int currentPartySize => this.activeNetworkPlayerModel.currentPartySize;

  public BeatmapLevelSelectionMask selectionMask => this.activeNetworkPlayerModel.selectionMask;

  public GameplayServerConfiguration configuration => this.activeNetworkPlayerModel.configuration;

  public string secret => this._gameLiftNetworkPlayerModel.secret;

  public string code => this._gameLiftNetworkPlayerModel.code;

  public ConnectedPlayerManager connectedPlayerManager => this.activeNetworkPlayerModel.connectedPlayerManager;

  public IEnumerable<INetworkPlayer> publicServers => this._gameLiftNetworkPlayerModel.publicServers;

  public IEnumerable<INetworkPlayer> friends => this._platformNetworkPlayerModel.friends;

  public IEnumerable<INetworkPlayer> localNetworkPlayers => this._localNetworkPlayerModel.otherPlayers;

  public bool discoveryEnabled
  {
    get => this.activeNetworkPlayerModel.discoveryEnabled;
    set => this.activeNetworkPlayerModel.discoveryEnabled = value;
  }

  public bool enableFriends
  {
    get => this._platformNetworkPlayerModel.discoveryEnabled;
    set
    {
      this._platformNetworkPlayerModel.discoveryEnabled = value;
      this.RefreshAlternateDiscoveryModels();
    }
  }

  public bool enableLocalNetwork
  {
    get => this._localNetworkPlayerModel.discoveryEnabled;
    set
    {
      this._localNetworkPlayerModel.discoveryEnabled = value;
      this.RefreshAlternateDiscoveryModels();
    }
  }

  public virtual void Initialize()
  {
    this._localNetworkPlayerModel.inviteRequestedEvent += new System.Action<INetworkPlayer>(this.HandleInviteRequested);
    this._localNetworkPlayerModel.joinRequestedEvent += new System.Action<INetworkPlayer>(this.HandleJoinRequested);
    this._localNetworkPlayerModel.partyChangedEvent += new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    this._localNetworkPlayerModel.partySizeChangedEvent += new System.Action<int>(this.HandlePartySizeChanged);
    this._localNetworkPlayerModel.connectedPlayerManagerCreatedEvent += new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
    this._localNetworkPlayerModel.connectedPlayerManagerDestroyedEvent += new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerDestroyed);
    this._platformNetworkPlayerModel.inviteRequestedEvent += new System.Action<INetworkPlayer>(this.HandleInviteRequested);
    this._platformNetworkPlayerModel.joinRequestedEvent += new System.Action<INetworkPlayer>(this.HandleJoinRequested);
    this._platformNetworkPlayerModel.partyChangedEvent += new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    this._platformNetworkPlayerModel.partySizeChangedEvent += new System.Action<int>(this.HandlePartySizeChanged);
    this._platformNetworkPlayerModel.connectedPlayerManagerCreatedEvent += new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
    this._platformNetworkPlayerModel.connectedPlayerManagerDestroyedEvent += new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerDestroyed);
    this._gameLiftNetworkPlayerModel.inviteRequestedEvent += new System.Action<INetworkPlayer>(this.HandleInviteRequested);
    this._gameLiftNetworkPlayerModel.joinRequestedEvent += new System.Action<INetworkPlayer>(this.HandleJoinRequested);
    this._gameLiftNetworkPlayerModel.partyChangedEvent += new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    this._gameLiftNetworkPlayerModel.partySizeChangedEvent += new System.Action<int>(this.HandlePartySizeChanged);
    this._gameLiftNetworkPlayerModel.connectedPlayerManagerCreatedEvent += new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
    this._gameLiftNetworkPlayerModel.connectedPlayerManagerDestroyedEvent += new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerDestroyed);
    this._gameLiftNetworkPlayerModel.partyRefreshingEvent += new System.Action(this.HandlePartyRefreshing);
  }

  public virtual void Dispose()
  {
    this._localNetworkPlayerModel.inviteRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleInviteRequested);
    this._localNetworkPlayerModel.joinRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleJoinRequested);
    this._localNetworkPlayerModel.partyChangedEvent -= new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    this._localNetworkPlayerModel.partySizeChangedEvent -= new System.Action<int>(this.HandlePartySizeChanged);
    this._localNetworkPlayerModel.connectedPlayerManagerCreatedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
    this._localNetworkPlayerModel.connectedPlayerManagerDestroyedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerDestroyed);
    this._platformNetworkPlayerModel.inviteRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleInviteRequested);
    this._platformNetworkPlayerModel.joinRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleJoinRequested);
    this._platformNetworkPlayerModel.partyChangedEvent -= new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    this._platformNetworkPlayerModel.partySizeChangedEvent -= new System.Action<int>(this.HandlePartySizeChanged);
    this._platformNetworkPlayerModel.connectedPlayerManagerCreatedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
    this._platformNetworkPlayerModel.connectedPlayerManagerDestroyedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerDestroyed);
    this._gameLiftNetworkPlayerModel.inviteRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleInviteRequested);
    this._gameLiftNetworkPlayerModel.joinRequestedEvent -= new System.Action<INetworkPlayer>(this.HandleJoinRequested);
    this._gameLiftNetworkPlayerModel.partyChangedEvent -= new System.Action<INetworkPlayerModel>(this.HandlePartyChanged);
    this._gameLiftNetworkPlayerModel.partySizeChangedEvent -= new System.Action<int>(this.HandlePartySizeChanged);
    this._gameLiftNetworkPlayerModel.connectedPlayerManagerCreatedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerCreated);
    this._gameLiftNetworkPlayerModel.connectedPlayerManagerDestroyedEvent -= new System.Action<INetworkPlayerModel>(this.HandleConnectedPlayerManagerDestroyed);
    this._gameLiftNetworkPlayerModel.partyRefreshingEvent -= new System.Action(this.HandlePartyRefreshing);
  }

  public IEnumerable<INetworkPlayer> partyPlayers => this.activeNetworkPlayerModel.partyPlayers;

  public IEnumerable<INetworkPlayer> otherPlayers
  {
    get
    {
      foreach (INetworkPlayer otherPlayer in this.activeNetworkPlayerModel.otherPlayers)
        yield return otherPlayer;
      if (this._activeNetworkPlayerModelType != UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.Platform)
      {
        foreach (INetworkPlayer friend in this.friends)
          yield return friend;
      }
      if (this._activeNetworkPlayerModelType != UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.LocalNetwork)
      {
        foreach (INetworkPlayer localNetworkPlayer in this.localNetworkPlayers)
          yield return localNetworkPlayer;
      }
    }
  }

  public virtual void SetServerFilter(
    BeatmapLevelSelectionMask selectionMask,
    GameplayServerConfiguration configuration)
  {
    if (this.selectionMask.Equals(selectionMask) && this.configuration.Equals(configuration) || this.connectedPlayerManager != null && this.connectedPlayerManager.isConnectedOrConnecting)
      return;
    this.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) new UnifiedNetworkPlayerModel.StartClientPartyConfig()
    {
      selectionMask = selectionMask,
      configuration = configuration
    });
  }

  public virtual void RefreshAlternateDiscoveryModels()
  {
    if (this.hasNetworkingFailed)
      return;
    bool flag = this.localPlayerIsPartyOwner || this.activeNetworkPlayerModel.connectedPlayerManager != null && this.activeNetworkPlayerModel.connectedPlayerManager.isConnected && this.configuration.invitePolicy == InvitePolicy.AnyoneCanInvite;
    if (this._activeNetworkPlayerModelType != UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.LocalNetwork)
    {
      if (flag && this.enableLocalNetwork)
      {
        LocalNetworkPlayerModel networkPlayerModel = this._localNetworkPlayerModel;
        LocalNetworkPlayerModel.CreatePartyConfig createConfig = new LocalNetworkPlayerModel.CreatePartyConfig();
        createConfig.partyManager = (INetworkPlayerModel) this;
        createConfig.selectionMask = this.selectionMask;
        createConfig.configuration = this.configuration;
        networkPlayerModel.CreatePartyConnection<LocalNetworkPlayerModel>((INetworkPlayerModelPartyConfig<LocalNetworkPlayerModel>) createConfig);
      }
      else
        this._localNetworkPlayerModel.DestroyPartyConnection();
    }
    if (this._activeNetworkPlayerModelType == UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.Platform)
      return;
    if (flag && this.enableFriends)
    {
      PlatformNetworkPlayerModel networkPlayerModel = this._platformNetworkPlayerModel;
      PlatformNetworkPlayerModel.CreatePartyConfig createConfig = new PlatformNetworkPlayerModel.CreatePartyConfig();
      createConfig.partyManager = (INetworkPlayerModel) this;
      createConfig.selectionMask = this.selectionMask;
      createConfig.configuration = this.configuration;
      networkPlayerModel.CreatePartyConnection<PlatformNetworkPlayerModel>((INetworkPlayerModelPartyConfig<PlatformNetworkPlayerModel>) createConfig);
    }
    else
      this._platformNetworkPlayerModel.DestroyPartyConnection();
  }

  public virtual void HandlePlayersChanged()
  {
    this.RefreshAlternateDiscoveryModels();
    System.Action<int> sizeChangedEvent = this.partySizeChangedEvent;
    if (sizeChangedEvent != null)
      sizeChangedEvent(this.currentPartySize);
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  public virtual void HandleInviteRequested(INetworkPlayer player)
  {
    System.Action<INetworkPlayer> inviteRequestedEvent = this.inviteRequestedEvent;
    if (inviteRequestedEvent == null)
      return;
    inviteRequestedEvent(player);
  }

  public virtual void HandleJoinRequested(INetworkPlayer player)
  {
    System.Action<INetworkPlayer> joinRequestedEvent = this.joinRequestedEvent;
    if (joinRequestedEvent == null)
      return;
    joinRequestedEvent(player);
  }

  public virtual void HandlePartyChanged(INetworkPlayerModel playerModel)
  {
    System.Action<INetworkPlayerModel> partyChangedEvent = this.partyChangedEvent;
    if (partyChangedEvent == null)
      return;
    partyChangedEvent((INetworkPlayerModel) this);
  }

  public virtual void HandlePartySizeChanged(int size)
  {
    System.Action<int> sizeChangedEvent = this.partySizeChangedEvent;
    if (sizeChangedEvent == null)
      return;
    sizeChangedEvent(this.currentPartySize);
  }

  public virtual void HandlePartyRefreshing()
  {
    System.Action partyRefreshingEvent = this.partyRefreshingEvent;
    if (partyRefreshingEvent == null)
      return;
    partyRefreshingEvent();
  }

  public virtual void HandleLocalPlayerConnected(IConnectedPlayer player)
  {
    if (!this._localNetworkPlayerModel.localPlayerIsPartyOwner)
      return;
    this._localNetworkPartyMessageHandler.ConnectToMasterServer(this.secret);
  }

  public virtual void HandleFriendConnected(IConnectedPlayer player)
  {
    if (!this._platformNetworkPlayerModel.localPlayerIsPartyOwner)
      return;
    this._friendPartyMessageHandler.ConnectToMasterServer(this.secret);
  }

  public virtual void HandleLocalPlayerConnectToMasterServer(string secret)
  {
    this.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) new UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig()
    {
      secret = secret,
      selectionMask = this.selectionMask,
      configuration = this.configuration
    });
    this._localNetworkPlayerModel.DestroyPartyConnection();
    this.HandlePlayersChanged();
  }

  public virtual void HandleFriendConnectToMasterServer(string secret)
  {
    this.CreatePartyConnection<UnifiedNetworkPlayerModel>((INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>) new UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig()
    {
      secret = secret,
      selectionMask = this.selectionMask,
      configuration = this.configuration
    });
    this._platformNetworkPlayerModel.DestroyPartyConnection();
    this.HandlePlayersChanged();
  }

  public virtual void HandleConnectedPlayerManagerCreated(INetworkPlayerModel networkPlayerModel)
  {
    if (networkPlayerModel == this.activeNetworkPlayerModel)
    {
      this._partyMessageHandler = new PartyMessageHandler(this.connectedPlayerManager);
      System.Action<INetworkPlayerModel> managerCreatedEvent = this.connectedPlayerManagerCreatedEvent;
      if (managerCreatedEvent == null)
        return;
      managerCreatedEvent((INetworkPlayerModel) this);
    }
    else if ((object)networkPlayerModel == this._localNetworkPlayerModel)
    {
      this._localNetworkPartyMessageHandler = new PartyMessageHandler(networkPlayerModel.connectedPlayerManager);
      networkPlayerModel.connectedPlayerManager.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.HandleLocalPlayerConnected);
      this._localNetworkPartyMessageHandler.connectToMasterServerEvent += new PartyMessageHandler.ConnectToMasterServerDelegate(this.HandleLocalPlayerConnectToMasterServer);
    }
    else
    {
      if ((object)networkPlayerModel != this._platformNetworkPlayerModel)
        return;
      this._friendPartyMessageHandler = new PartyMessageHandler(networkPlayerModel.connectedPlayerManager);
      networkPlayerModel.connectedPlayerManager.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.HandleFriendConnected);
      this._friendPartyMessageHandler.connectToMasterServerEvent += new PartyMessageHandler.ConnectToMasterServerDelegate(this.HandleFriendConnectToMasterServer);
    }
  }

  public virtual void HandleConnectedPlayerManagerDestroyed(INetworkPlayerModel networkPlayerModel)
  {
    if (networkPlayerModel == this.activeNetworkPlayerModel)
    {
      if (this._partyMessageHandler != null)
      {
        this._partyMessageHandler.Dispose();
        this._partyMessageHandler = (PartyMessageHandler) null;
      }
      this.RefreshAlternateDiscoveryModels();
      System.Action<INetworkPlayerModel> managerDestroyedEvent = this.connectedPlayerManagerDestroyedEvent;
      if (managerDestroyedEvent == null)
        return;
      managerDestroyedEvent((INetworkPlayerModel) this);
    }
    else if ((object)networkPlayerModel == this._localNetworkPlayerModel)
    {
      this._localNetworkPartyMessageHandler?.Dispose();
      this._localNetworkPartyMessageHandler = (PartyMessageHandler) null;
    }
    else
    {
      if ((object)networkPlayerModel != this._platformNetworkPlayerModel)
        return;
      this._friendPartyMessageHandler?.Dispose();
      this._friendPartyMessageHandler = (PartyMessageHandler) null;
    }
  }

  public virtual void ResetMasterServerReachability() => this._gameLiftNetworkPlayerModel.ResetMasterServerReachability();

  public virtual bool CreatePartyConnection<T>(INetworkPlayerModelPartyConfig<T> partyConfig) where T : INetworkPlayerModel
  {
    switch (partyConfig)
    {
      case UnifiedNetworkPlayerModel.JoinMatchmakingPartyConfig matchmakingPartyConfig when this.activeNetworkPlayerModelType == UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.GameLift:
        GameLiftNetworkPlayerModel networkPlayerModel1 = this._gameLiftNetworkPlayerModel;
        NetworkPlayerModel<GameLiftConnectionManager>.JoinMatchmakingPartyConfig createConfig1 = new NetworkPlayerModel<GameLiftConnectionManager>.JoinMatchmakingPartyConfig();
        createConfig1.code = matchmakingPartyConfig.code;
        createConfig1.secret = matchmakingPartyConfig.secret;
        createConfig1.configuration = matchmakingPartyConfig.configuration;
        createConfig1.selectionMask = matchmakingPartyConfig.selectionMask;
        return networkPlayerModel1.CreatePartyConnection<NetworkPlayerModel<GameLiftConnectionManager>>((INetworkPlayerModelPartyConfig<NetworkPlayerModel<GameLiftConnectionManager>>) createConfig1);
      case UnifiedNetworkPlayerModel.StartClientPartyConfig clientPartyConfig:
        switch (this.activeNetworkPlayerModelType)
        {
          case UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.GameLift:
            GameLiftNetworkPlayerModel networkPlayerModel2 = this._gameLiftNetworkPlayerModel;
            NetworkPlayerModel<GameLiftConnectionManager>.StartClientPartyConfig createConfig2 = new NetworkPlayerModel<GameLiftConnectionManager>.StartClientPartyConfig();
            createConfig2.configuration = clientPartyConfig.configuration;
            createConfig2.selectionMask = clientPartyConfig.selectionMask;
            return networkPlayerModel2.CreatePartyConnection<NetworkPlayerModel<GameLiftConnectionManager>>((INetworkPlayerModelPartyConfig<NetworkPlayerModel<GameLiftConnectionManager>>) createConfig2);
          case UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.Platform:
            PlatformNetworkPlayerModel networkPlayerModel3 = this._platformNetworkPlayerModel;
            PlatformNetworkPlayerModel.CreatePartyConfig createConfig3 = new PlatformNetworkPlayerModel.CreatePartyConfig();
            createConfig3.selectionMask = clientPartyConfig.selectionMask;
            createConfig3.configuration = clientPartyConfig.configuration;
            return networkPlayerModel3.CreatePartyConnection<PlatformNetworkPlayerModel>((INetworkPlayerModelPartyConfig<PlatformNetworkPlayerModel>) createConfig3);
          case UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType.LocalNetwork:
            LocalNetworkPlayerModel networkPlayerModel4 = this._localNetworkPlayerModel;
            LocalNetworkPlayerModel.CreatePartyConfig createConfig4 = new LocalNetworkPlayerModel.CreatePartyConfig();
            createConfig4.selectionMask = clientPartyConfig.selectionMask;
            createConfig4.configuration = clientPartyConfig.configuration;
            return networkPlayerModel4.CreatePartyConnection<LocalNetworkPlayerModel>((INetworkPlayerModelPartyConfig<LocalNetworkPlayerModel>) createConfig4);
        }
        break;
    }
    return false;
  }

  public virtual void DestroyPartyConnection() => this.activeNetworkPlayerModel.DestroyPartyConnection();

  public virtual void SetActiveNetworkPlayerModelType(
    UnifiedNetworkPlayerModel.ActiveNetworkPlayerModelType activeNetworkPlayerModelType)
  {
    if (activeNetworkPlayerModelType == this._activeNetworkPlayerModelType)
      return;
    this.discoveryEnabled = false;
    this.DestroyPartyConnection();
    this._activeNetworkPlayerModelType = activeNetworkPlayerModelType;
  }

  public enum ActiveNetworkPlayerModelType
  {
    GameLift,
    Platform,
    LocalNetwork,
  }

  public class JoinMatchmakingPartyConfig : INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>
  {
    public BeatmapLevelSelectionMask selectionMask;
    public GameplayServerConfiguration configuration;
    public string secret;
    public string code;
  }

  public class StartClientPartyConfig : INetworkPlayerModelPartyConfig<UnifiedNetworkPlayerModel>
  {
    public BeatmapLevelSelectionMask selectionMask;
    public GameplayServerConfiguration configuration;
  }
}
