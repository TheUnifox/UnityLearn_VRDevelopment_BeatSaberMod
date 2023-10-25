// Decompiled with JetBrains decompiler
// Type: LobbyDataModelsManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class LobbyDataModelsManager
{
  [Inject]
  protected readonly ILobbyStateDataModel _lobbyStateDataModel;
  [Inject]
  protected readonly ILobbyPlayersDataModel _lobbyPlayersDataModel;
  [Inject]
  protected readonly ILobbyGameStateController _lobbyGameStateController;
  [Inject]
  protected readonly INodePoseSyncStateManager _nodePoseSyncStateManager;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;

  public virtual void Activate()
  {
    this._lobbyStateDataModel.Activate();
    this._lobbyPlayersDataModel.Activate();
    this._lobbyGameStateController.Activate();
    this._lobbyPlayerPermissionsModel.Activate();
    this._nodePoseSyncStateManager.ClearBufferedStates();
  }

  public virtual void Deactivate()
  {
    this._lobbyStateDataModel.Deactivate();
    this._lobbyPlayersDataModel.Deactivate();
    this._lobbyGameStateController.Deactivate();
    this._lobbyPlayerPermissionsModel.Deactivate();
  }
}
