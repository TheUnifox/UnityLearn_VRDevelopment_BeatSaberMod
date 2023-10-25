// Decompiled with JetBrains decompiler
// Type: NoAnalyticsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class NoAnalyticsModel : IAnalyticsModel
{
  public bool supportsOpenDataPrivacyPage => false;

  public virtual void OpenDataPrivacyPage()
  {
  }

  public virtual void LogEditAvatarEvent(string eventType, Dictionary<string, string> eventData)
  {
  }

  public virtual void LogEvent(string eventType, Dictionary<string, string> eventData)
  {
  }

  public virtual void LogClick(string clickType, Dictionary<string, string> clickData)
  {
  }

  public virtual void LogImpression(
    string impressionType,
    Dictionary<string, string> impressionData)
  {
  }

  public virtual void LogExposure(string exposureType, Dictionary<string, string> exposureData)
  {
  }
}
