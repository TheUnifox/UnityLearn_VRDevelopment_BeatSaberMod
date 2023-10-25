// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerObservable
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class MultiplayerConnectedPlayerObservable : IMultiplayerObservable
{
  protected readonly IConnectedPlayer _connectedPlayer;

  public MultiplayerConnectedPlayerObservable(IConnectedPlayer connectedPlayer) => this._connectedPlayer = connectedPlayer;

  public float offsetSyncTime => this._connectedPlayer.offsetSyncTime;

  public bool isFailed => this._connectedPlayer.IsFailed();
}
