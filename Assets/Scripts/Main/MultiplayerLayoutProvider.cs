// Decompiled with JetBrains decompiler
// Type: MultiplayerLayoutProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class MultiplayerLayoutProvider
{
  [CompilerGenerated]
  protected MultiplayerPlayerLayout m_Clayout;
  [CompilerGenerated]
  protected int m_CactivePlayerSpotsCount;

  public MultiplayerPlayerLayout layout
  {
    get => this.m_Clayout;
    private set => this.m_Clayout = value;
  }

  public int activePlayerSpotsCount
  {
    get => this.m_CactivePlayerSpotsCount;
    private set => this.m_CactivePlayerSpotsCount = value;
  }

  public event System.Action<MultiplayerPlayerLayout, int> playersLayoutWasCalculatedEvent;

  public virtual MultiplayerPlayerLayout CalculateLayout(int activePlayersCount)
  {
    this.layout = activePlayersCount != 2 ? MultiplayerPlayerLayout.Circle : MultiplayerPlayerLayout.Duel;
    if (activePlayersCount % 2 == 0 && this.layout != MultiplayerPlayerLayout.Duel)
      ++activePlayersCount;
    this.activePlayerSpotsCount = activePlayersCount;
    System.Action<MultiplayerPlayerLayout, int> wasCalculatedEvent = this.playersLayoutWasCalculatedEvent;
    if (wasCalculatedEvent != null)
      wasCalculatedEvent(this.layout, activePlayersCount);
    return this.layout;
  }
}
