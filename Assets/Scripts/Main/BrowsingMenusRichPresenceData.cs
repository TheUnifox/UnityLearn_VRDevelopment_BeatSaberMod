// Decompiled with JetBrains decompiler
// Type: BrowsingMenusRichPresenceData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Runtime.CompilerServices;

public class BrowsingMenusRichPresenceData : IRichPresenceData
{
  [CompilerGenerated]
  protected string m_ClocalizedDescription;
  [LocalizationKey]
  protected const string kBrowsingMenusRichPresenceLocalizationKey = "BROWSING_MENUS_PRESENCE";

  public string apiName => "menu";

  public string localizedDescription
  {
    get => this.m_ClocalizedDescription;
    private set => this.m_ClocalizedDescription = value;
  }

  public BrowsingMenusRichPresenceData() => this.localizedDescription = Localization.Get("BROWSING_MENUS_PRESENCE");
}
