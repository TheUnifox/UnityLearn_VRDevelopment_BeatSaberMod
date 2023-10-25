// Decompiled with JetBrains decompiler
// Type: GameServerListItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class GameServerListItem
{
  public readonly string serverName;
  public readonly int capacity;
  public readonly int occupied;
  public readonly bool password;

  public GameServerListItem(string serverName, int capacity, int occupied, bool password)
  {
    this.serverName = serverName;
    this.capacity = capacity;
    this.occupied = occupied;
    this.password = password;
  }
}
