// Decompiled with JetBrains decompiler
// Type: SelectMultiplayerLobbyDestination
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class SelectMultiplayerLobbyDestination : MenuDestination
{
  public readonly string lobbySecret;
  public readonly string lobbyCode;

  public SelectMultiplayerLobbyDestination(string lobbySecret, string lobbyCode)
  {
    this.lobbySecret = lobbySecret;
    this.lobbyCode = lobbyCode;
  }

  public SelectMultiplayerLobbyDestination(ulong roomId) => this.lobbySecret = NetworkUtility.GetHashBase64(string.Format("Room {0}", (object) roomId));

  public SelectMultiplayerLobbyDestination(string lobbyCode) => this.lobbyCode = lobbyCode;
}
