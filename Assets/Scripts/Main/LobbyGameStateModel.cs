// Decompiled with JetBrains decompiler
// Type: LobbyGameStateModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class LobbyGameStateModel
{
  protected MultiplayerGameState _gameState;

  public MultiplayerGameState gameState => this._gameState;

  public event System.Action<MultiplayerGameState> gameStateDidChangeEvent;

  public event System.Action<MultiplayerGameState> gameStateDidChangeAlwaysSentEvent;

  public virtual void SetGameState(MultiplayerGameState newGameState) => this.SetGameState(newGameState, true);

  public virtual void SetGameStateWithoutNotification(MultiplayerGameState newGameState) => this.SetGameState(newGameState, false);

  public virtual void SetGameState(MultiplayerGameState newGameState, bool sendNotification)
  {
    if (newGameState == this._gameState)
      return;
    this._gameState = newGameState;
    if (sendNotification)
    {
      System.Action<MultiplayerGameState> stateDidChangeEvent = this.gameStateDidChangeEvent;
      if (stateDidChangeEvent != null)
        stateDidChangeEvent(newGameState);
    }
    System.Action<MultiplayerGameState> changeAlwaysSentEvent = this.gameStateDidChangeAlwaysSentEvent;
    if (changeAlwaysSentEvent == null)
      return;
    changeAlwaysSentEvent(newGameState);
  }
}
