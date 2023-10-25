// Decompiled with JetBrains decompiler
// Type: BaseNetworkPlayerModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Diagnostics;

public abstract class BaseNetworkPlayerModel : StandaloneMonobehavior, INetworkPlayerModel
{
  private ConnectedPlayerManager _connectedPlayerManager;
  private INetworkPlayerModel _partyManager;
  private GameplayServerConfiguration _configuration;
  private BeatmapLevelSelectionMask _selectionMask;

  public ConnectedPlayerManager connectedPlayerManager => this._connectedPlayerManager;

  public INetworkPlayerModel partyManager => this._partyManager ?? (INetworkPlayerModel) this;

  protected bool isConnectedOrConnecting => this._connectedPlayerManager != null && this._connectedPlayerManager.isConnectedOrConnecting;

  protected bool isConnectionOwner => this._connectedPlayerManager != null && this._connectedPlayerManager.isConnectionOwner;

  public virtual bool discoveryEnabled { get; set; }

  public virtual bool hasNetworkingFailed => false;

  public virtual int currentPartySize => 0;

  public virtual GameplayServerConfiguration configuration => this._configuration;

  public virtual BeatmapLevelSelectionMask selectionMask => this._selectionMask;

  public event System.Action<INetworkPlayerModel> connectedPlayerManagerCreatedEvent;

  public event System.Action<INetworkPlayerModel> connectedPlayerManagerDestroyedEvent;

  public virtual event System.Action<INetworkPlayerModel> partyChangedEvent
  {
    add
    {
    }
    remove
    {
    }
  }

  public virtual event System.Action<int> partySizeChangedEvent
  {
    add
    {
    }
    remove
    {
    }
  }

  public virtual event System.Action<INetworkPlayer> joinRequestedEvent
  {
    add
    {
    }
    remove
    {
    }
  }

  public virtual event System.Action<INetworkPlayer> inviteRequestedEvent
  {
    add
    {
    }
    remove
    {
    }
  }

  public IEnumerable<INetworkPlayer> partyPlayers => this.GetPartyPlayers();

  IEnumerable<INetworkPlayer> INetworkPlayerModel.otherPlayers => this.GetOtherPlayers();

  public virtual bool localPlayerIsPartyOwner => true;

  protected bool isServer => this.isConnectedOrConnecting && this.isConnectionOwner;

  protected bool isClient => this.isConnectedOrConnecting && !this.isConnectionOwner;

  protected override void OnDestroy() => this.DestroyConnectedPlayerManager();

  protected override void Update() => this._connectedPlayerManager?.PollUpdate(this.frameCount);

  protected virtual IEnumerable<INetworkPlayer> GetPartyPlayers()
  {
    yield break;
  }

  protected virtual IEnumerable<INetworkPlayer> GetOtherPlayers()
  {
    yield break;
  }

  protected virtual void ConnectionFailed(ConnectionFailedReason reason) => this.DestroyPartyConnection();

  protected virtual void PlayerConnected(IConnectedPlayer player)
  {
  }

  protected virtual void PlayerDisconnected(IConnectedPlayer player)
  {
  }

  protected virtual void PlayerStateChanged(IConnectedPlayer player)
  {
  }

  protected virtual void ConnectedPlayerManagerChanged()
  {
  }

  protected virtual void PlayerOrderChanged(IConnectedPlayer player)
  {
  }

  protected virtual void PartySizeChanged(int currentPartySize)
  {
  }

  protected virtual void Connected()
  {
  }

  protected virtual void Disconnected(DisconnectedReason disconnectedReason) => this.DestroyPartyConnection();

  public virtual bool CreatePartyConnection<T>(INetworkPlayerModelPartyConfig<T> createConfig) where T : INetworkPlayerModel
  {
    if (!(createConfig is BaseNetworkPlayerModel.PartyConfig partyConfig))
      return false;
    this._configuration = partyConfig.configuration;
    this._selectionMask = partyConfig.selectionMask;
    this.partyManager.partySizeChangedEvent -= new System.Action<int>(this.PartySizeChanged);
    this._partyManager = partyConfig.partyManager;
    this.partyManager.partySizeChangedEvent += new System.Action<int>(this.PartySizeChanged);
    return true;
  }

  public virtual void DestroyPartyConnection()
  {
    this.partyManager.partySizeChangedEvent -= new System.Action<int>(this.PartySizeChanged);
    this._partyManager = (INetworkPlayerModel) null;
    this.connectedPlayerManager?.Disconnect();
  }

  protected void DestroyConnectedPlayerManager()
  {
    if (this._connectedPlayerManager == null)
      return;
    this._connectedPlayerManager.connectedEvent -= new System.Action(this.Connected);
    this._connectedPlayerManager.playerConnectedEvent -= new System.Action<IConnectedPlayer>(this.PlayerConnected);
    this._connectedPlayerManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.PlayerDisconnected);
    this._connectedPlayerManager.playerStateChangedEvent -= new System.Action<IConnectedPlayer>(this.PlayerStateChanged);
    this._connectedPlayerManager.playerOrderChangedEvent -= new System.Action<IConnectedPlayer>(this.PlayerOrderChanged);
    this._connectedPlayerManager.connectionFailedEvent -= new System.Action<ConnectionFailedReason>(this.ConnectionFailed);
    this._connectedPlayerManager.disconnectedEvent -= new System.Action<DisconnectedReason>(this.Disconnected);
    for (int index = 0; index < this._connectedPlayerManager.connectedPlayerCount; ++index)
      this.PlayerDisconnected(this._connectedPlayerManager.GetConnectedPlayer(index));
    ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
    this._connectedPlayerManager = (ConnectedPlayerManager) null;
    this.ConnectedPlayerManagerChanged();
    connectedPlayerManager.Dispose();
    System.Action<INetworkPlayerModel> managerDestroyedEvent = this.connectedPlayerManagerDestroyedEvent;
    if (managerDestroyedEvent == null)
      return;
    managerDestroyedEvent((INetworkPlayerModel) this);
  }

  protected bool CreateConnectedPlayerManager<T>(IConnectionInitParams<T> initParams) where T : class, IConnectionManager, new()
  {
    ConnectedPlayerManager connectedPlayerManager = this._connectedPlayerManager;
    int num;
    if (connectedPlayerManager == null)
    {
      num = 0;
    }
    else
    {
      bool? nullable = connectedPlayerManager.GetConnectionManager<T>()?.Init<T>(initParams);
      bool flag = true;
      num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
    }
    if (num != 0)
      return true;
    this.DestroyConnectedPlayerManager();
    this.Log("Create ConnectedPlayerManager. params: " + (object) initParams);
    T obj = new T();
    if (!obj.Init<T>(initParams))
      return false;
    this._connectedPlayerManager = new ConnectedPlayerManager((IConnectionManager) obj);
    this._connectedPlayerManager.connectedEvent += new System.Action(this.Connected);
    this._connectedPlayerManager.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.PlayerConnected);
    this._connectedPlayerManager.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.PlayerDisconnected);
    this._connectedPlayerManager.playerStateChangedEvent += new System.Action<IConnectedPlayer>(this.PlayerStateChanged);
    this._connectedPlayerManager.playerOrderChangedEvent += new System.Action<IConnectedPlayer>(this.PlayerOrderChanged);
    this._connectedPlayerManager.connectionFailedEvent += new System.Action<ConnectionFailedReason>(this.ConnectionFailed);
    this._connectedPlayerManager.disconnectedEvent += new System.Action<DisconnectedReason>(this.Disconnected);
    for (int index = 0; index < this._connectedPlayerManager.connectedPlayerCount; ++index)
      this.PlayerConnected(this._connectedPlayerManager.GetConnectedPlayer(index));
    this.ConnectedPlayerManagerChanged();
    System.Action<INetworkPlayerModel> managerCreatedEvent = this.connectedPlayerManagerCreatedEvent;
    if (managerCreatedEvent != null)
      managerCreatedEvent((INetworkPlayerModel) this);
    return true;
  }

  [Conditional("VERBOSE_LOGGING")]
  protected void Log(string message) => BGNet.Logging.Debug.Log("[" + ((object) this).GetType().Name + "] " + message);

  public class PartyConfig
  {
    public BeatmapLevelSelectionMask selectionMask;
    public GameplayServerConfiguration configuration;
    public INetworkPlayerModel partyManager;
  }
}
