// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyAnalytics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MultiplayerLobbyAnalytics : MonoBehaviour
{
  [SerializeField]
  protected GameServerLobbyFlowCoordinator _gameServerLobbyFlowCoordinator;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;

  public virtual void Awake()
  {
    this._gameServerLobbyFlowCoordinator.didFinishEvent += new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidFinish);
    this._gameServerLobbyFlowCoordinator.didSetupEvent += new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidSetupEvent);
    this._gameServerLobbyFlowCoordinator.startGameOrReadyEvent += new System.Action(this.HandleGameServerLobbyFlowCoordinatorStartGameOrReady);
    this._gameServerLobbyFlowCoordinator.didOpenInvitePanelEvent += new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidOpenInvitePanel);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._gameServerLobbyFlowCoordinator != (UnityEngine.Object) null))
      return;
    this._gameServerLobbyFlowCoordinator.didFinishEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidFinish);
    this._gameServerLobbyFlowCoordinator.didSetupEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidSetupEvent);
    this._gameServerLobbyFlowCoordinator.startGameOrReadyEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorStartGameOrReady);
    this._gameServerLobbyFlowCoordinator.didOpenInvitePanelEvent -= new System.Action(this.HandleGameServerLobbyFlowCoordinatorDidOpenInvitePanel);
  }

  public virtual void HandleGameServerLobbyFlowCoordinatorDidSetupEvent() => this._analyticsModel.LogImpression(this.GetEventTypeFromLobbyType(this._gameServerLobbyFlowCoordinator.GetLobbyType()), new Dictionary<string, string>()
  {
    ["page"] = "Multiplayer"
  });

  public virtual void HandleGameServerLobbyFlowCoordinatorDidFinish() => this.LogClick(new Dictionary<string, string>()
  {
    ["page"] = "Multiplayer",
    ["custom_text"] = "Exit Lobby"
  });

  public virtual void HandleGameServerLobbyFlowCoordinatorStartGameOrReady() => this.LogClick(new Dictionary<string, string>()
  {
    ["page"] = "Multiplayer",
    ["custom_text"] = "Start or Ready"
  });

  public virtual void HandleGameServerLobbyFlowCoordinatorDidOpenInvitePanel() => this.LogClick(new Dictionary<string, string>()
  {
    ["page"] = "Multiplayer",
    ["custom_text"] = "Player Invite"
  });

  public virtual void LogClick(Dictionary<string, string> clickData) => this._analyticsModel.LogClick(this.GetEventTypeFromLobbyType(this._gameServerLobbyFlowCoordinator.GetLobbyType()), clickData);

  public virtual string GetEventTypeFromLobbyType(GameServerLobbyFlowCoordinator.LobbyType lobbyType)
  {
    switch (lobbyType)
    {
      case GameServerLobbyFlowCoordinator.LobbyType.HostSetup:
        return "Host Setup";
      case GameServerLobbyFlowCoordinator.LobbyType.ClientSetup:
        return "Client Setup";
      case GameServerLobbyFlowCoordinator.LobbyType.QuickPlayLobby:
        return "Quick Play Lobby";
      default:
        return "Party Lobby";
    }
  }
}
