// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalPlayerDisconnectHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class MultiplayerLocalPlayerDisconnectHelper
{
  [Inject]
  protected readonly IGameplayRpcManager _gameplayRpcManager;
  [Inject]
  protected readonly IMultiplayerLevelEndActionsListener _multiplayerLevelEndActions;
  [Inject]
  protected readonly LobbyPlayerPermissionsModel _lobbyPlayerPermissionsModel;
  protected const string kDisconnectLabel = "BUTTON_DISCONNECT";
  protected const string kEndGameLabel = "BUTTON_END_GAME";

  public virtual void Disconnect(
    MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState playerLevelEndState,
    LevelCompletionResults levelCompletionResults)
  {
    if (this._lobbyPlayerPermissionsModel.isPartyOwner)
    {
      this._multiplayerLevelEndActions.ReportPlayerNetworkDidFailed(new MultiplayerLevelCompletionResults(playerLevelEndState, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.HostEndedLevel, levelCompletionResults));
      this._gameplayRpcManager.RequestReturnToMenu();
    }
    else
      this._multiplayerLevelEndActions.ReportPlayerNetworkDidFailed(new MultiplayerLevelCompletionResults(playerLevelEndState, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Quit, levelCompletionResults));
  }

  public virtual string ResolveDisconnectButtonString() => !this._lobbyPlayerPermissionsModel.isPartyOwner ? "BUTTON_DISCONNECT" : "BUTTON_END_GAME";
}
