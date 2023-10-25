// Decompiled with JetBrains decompiler
// Type: PlayingTutorialPresenceData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Runtime.CompilerServices;

public class PlayingTutorialPresenceData : IRichPresenceData
{
  [CompilerGenerated]
  protected string m_ClocalizedDescription;
  [LocalizationKey]
  protected const string kPlayingCampaignRichPresenceLocalizationKey = "PLAYING_TUTORIAL_PRESENCE";

  public string apiName => "tutorial";

  public string localizedDescription
  {
    get => this.m_ClocalizedDescription;
    private set => this.m_ClocalizedDescription = value;
  }

  public PlayingTutorialPresenceData() => this.localizedDescription = Localization.Get("PLAYING_TUTORIAL_PRESENCE");
}
