// Decompiled with JetBrains decompiler
// Type: LobbyPlayerData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class LobbyPlayerData : LevelGameplaySetupData, ILobbyPlayerData, ILevelGameplaySetupData
{
  [CompilerGenerated]
  protected bool m_CisPartyOwner;
  [CompilerGenerated]
  protected bool m_CisActive;
  [CompilerGenerated]
  protected bool m_CisReady;
  [CompilerGenerated]
  protected bool m_CisInLobby;

  public bool isPartyOwner
  {
    get => this.m_CisPartyOwner;
    set => this.m_CisPartyOwner = value;
  }

  public bool isActive
  {
    get => this.m_CisActive;
    set => this.m_CisActive = value;
  }

  public bool isReady
  {
    get => this.m_CisReady;
    set => this.m_CisReady = value;
  }

  public bool isInLobby
  {
    get => this.m_CisInLobby;
    set => this.m_CisInLobby = value;
  }
}
