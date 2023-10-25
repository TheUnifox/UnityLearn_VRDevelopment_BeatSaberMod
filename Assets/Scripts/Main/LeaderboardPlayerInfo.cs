// Decompiled with JetBrains decompiler
// Type: LeaderboardPlayerInfo
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class LeaderboardPlayerInfo
{
  public string serverKey;
  [CompilerGenerated]
  protected string m_CplayerId;
  [CompilerGenerated]
  protected string m_CplayerName;
  [CompilerGenerated]
  protected string m_CplayerKey;
  [CompilerGenerated]
  protected string m_CauthType;
  [CompilerGenerated]
  protected string m_CplayerFriends;
  [CompilerGenerated]
  protected bool m_Csucceeded;

  public string playerId
  {
    get => this.m_CplayerId;
    private set => this.m_CplayerId = value;
  }

  public string playerName
  {
    get => this.m_CplayerName;
    private set => this.m_CplayerName = value;
  }

  public string playerKey
  {
    get => this.m_CplayerKey;
    private set => this.m_CplayerKey = value;
  }

  public string authType
  {
    get => this.m_CauthType;
    private set => this.m_CauthType = value;
  }

  public string playerFriends
  {
    get => this.m_CplayerFriends;
    private set => this.m_CplayerFriends = value;
  }

  public bool succeeded
  {
    get => this.m_Csucceeded;
    private set => this.m_Csucceeded = value;
  }

  public LeaderboardPlayerInfo(
    bool succeeded,
    string playerId,
    string playerName,
    string playerKey,
    string authType,
    string playerFriends)
  {
    this.playerId = playerId;
    this.playerName = playerName;
    this.playerKey = playerKey;
    this.authType = authType;
    this.playerFriends = playerFriends;
    this.succeeded = succeeded;
  }
}
