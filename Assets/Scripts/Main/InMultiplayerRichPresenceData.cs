// Decompiled with JetBrains decompiler
// Type: InMultiplayerRichPresenceData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Runtime.CompilerServices;

public class InMultiplayerRichPresenceData : IMultiplayerRichPresenceData, IRichPresenceData
{
  [CompilerGenerated]
  protected string m_CapiName;
  [CompilerGenerated]
  protected string m_ClocalizedDescription;
  [CompilerGenerated]
  protected string m_CmultiplayerSecret;
  [CompilerGenerated]
  protected bool m_CatMaxPartySize;
  [CompilerGenerated]
  protected bool m_CcanInvite;
  [LocalizationKey]
  protected const string kInMultiplayerLobbyRichPresenceLocalizationKey = "IN_MULTIPLAYER_LOBBY_PRESENCE";

  public string apiName
  {
    get => this.m_CapiName;
    protected set => this.m_CapiName = value;
  }

  public string localizedDescription
  {
    get => this.m_ClocalizedDescription;
    protected set => this.m_ClocalizedDescription = value;
  }

  public string multiplayerSecret
  {
    get => this.m_CmultiplayerSecret;
    set => this.m_CmultiplayerSecret = value;
  }

  public bool atMaxPartySize
  {
    get => this.m_CatMaxPartySize;
    set => this.m_CatMaxPartySize = value;
  }

  public bool canInvite
  {
    get => this.m_CcanInvite;
    set => this.m_CcanInvite = value;
  }

  public bool isJoinable => this.canInvite && !this.atMaxPartySize && !string.IsNullOrEmpty(this.multiplayerSecret);

  public InMultiplayerRichPresenceData(
    string multiplayerSecret,
    bool canInvite,
    bool atMaxPartySize)
  {
    this.apiName = "multiplayer_lobby";
    this.localizedDescription = Localization.Get("IN_MULTIPLAYER_LOBBY_PRESENCE");
    this.multiplayerSecret = multiplayerSecret;
    this.canInvite = canInvite;
    this.atMaxPartySize = atMaxPartySize;
  }
}
