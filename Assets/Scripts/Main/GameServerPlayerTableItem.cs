// Decompiled with JetBrains decompiler
// Type: GameServerPlayerTableItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class GameServerPlayerTableItem
{
  public readonly string playerName;
  public readonly string suggestedLevel;
  public readonly string suggestedModifiers;
  public readonly bool isReady;

  public GameServerPlayerTableItem(
    string playerName,
    string suggestedLevel,
    string suggestedModifiers,
    bool isReady)
  {
    this.playerName = playerName;
    this.suggestedLevel = suggestedLevel;
    this.suggestedModifiers = suggestedModifiers;
    this.isReady = isReady;
  }
}
