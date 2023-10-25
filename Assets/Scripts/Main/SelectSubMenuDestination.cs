// Decompiled with JetBrains decompiler
// Type: SelectSubMenuDestination
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class SelectSubMenuDestination : MenuDestination
{
  public readonly SelectSubMenuDestination.Destination menuDestination;

  public SelectSubMenuDestination(
    SelectSubMenuDestination.Destination menuDestination)
  {
    this.menuDestination = menuDestination;
  }

  public enum Destination
  {
    MainMenu,
    Campaign,
    SoloFreePlay,
    PartyFreePlay,
    Settings,
    Tutorial,
    Multiplayer,
  }
}
