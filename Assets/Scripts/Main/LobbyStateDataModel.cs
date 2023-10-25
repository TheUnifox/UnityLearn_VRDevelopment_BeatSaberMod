// Decompiled with JetBrains decompiler
// Type: LobbyStateDataModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using Zenject;

public class LobbyStateDataModel : ILobbyStateDataModel, IDisposable
{
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly IUnifiedNetworkPlayerModel _unifiedNetworkPlayerModel;
  protected List<IConnectedPlayer> _connectedPlayers;
  protected Dictionary<string, IConnectedPlayer> _connectedPlayersById;
  protected GameplayServerConfiguration _configuration;

  public event System.Action<IConnectedPlayer> playerConnectedEvent;

  public event System.Action<IConnectedPlayer> playerDisconnectedEvent;

  public bool isConnected => this._multiplayerSessionManager.isConnectingOrConnected;

  public IConnectedPlayer localPlayer => this._multiplayerSessionManager.localPlayer;

  public List<IConnectedPlayer> connectedPlayers => this._connectedPlayers;

  public IReadOnlyList<IConnectedPlayer> rawConnectedPlayers => this._multiplayerSessionManager.connectedPlayers;

  public GameplayServerConfiguration configuration => this._configuration;

  public virtual void Activate()
  {
    this._connectedPlayers = new List<IConnectedPlayer>()
    {
      this.localPlayer
    };
    this._connectedPlayersById = new Dictionary<string, IConnectedPlayer>()
    {
      {
        this.localPlayer.userId,
        this.localPlayer
      }
    };
    foreach (IConnectedPlayer connectedPlayer in (IEnumerable<IConnectedPlayer>) this._multiplayerSessionManager.connectedPlayers)
    {
      this._connectedPlayers.Add(connectedPlayer);
      this._connectedPlayersById.Add(connectedPlayer.userId, connectedPlayer);
    }
    this._multiplayerSessionManager.playerConnectedEvent += new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerConnected);
    this._multiplayerSessionManager.playerDisconnectedEvent += new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerDisconnected);
    this._configuration = this._unifiedNetworkPlayerModel.configuration;
  }

  public virtual void Deactivate()
  {
    this._multiplayerSessionManager.playerConnectedEvent -= new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerConnected);
    this._multiplayerSessionManager.playerDisconnectedEvent -= new System.Action<IConnectedPlayer>(this.HandleMultiplayerSessionManagerPlayerDisconnected);
  }

  public virtual void Dispose() => this.Deactivate();

  public virtual IConnectedPlayer GetPlayerById(string userId)
  {
    IConnectedPlayer connectedPlayer;
    return !this._connectedPlayersById.TryGetValue(userId ?? "", out connectedPlayer) ? (IConnectedPlayer) null : connectedPlayer;
  }

  public virtual void HandleMultiplayerSessionManagerPlayerConnected(IConnectedPlayer player)
  {
    this._connectedPlayers.Add(player);
    this._connectedPlayersById.Add(player.userId, player);
    System.Action<IConnectedPlayer> playerConnectedEvent = this.playerConnectedEvent;
    if (playerConnectedEvent == null)
      return;
    playerConnectedEvent(player);
  }

  public virtual void HandleMultiplayerSessionManagerPlayerDisconnected(IConnectedPlayer player)
  {
    this._connectedPlayers.Remove(player);
    this._connectedPlayersById.Remove(player.userId);
    System.Action<IConnectedPlayer> disconnectedEvent = this.playerDisconnectedEvent;
    if (disconnectedEvent == null)
      return;
    disconnectedEvent(player);
  }
}
