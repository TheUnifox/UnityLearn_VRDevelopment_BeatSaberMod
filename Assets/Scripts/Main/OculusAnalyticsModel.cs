// Decompiled with JetBrains decompiler
// Type: OculusAnalyticsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using System.Collections.Generic;

public class OculusAnalyticsModel : IAnalyticsModel
{
  public bool supportsOpenDataPrivacyPage => false;

  public virtual void OpenDataPrivacyPage()
  {
  }

  public virtual void LogEvent(string eventType, Dictionary<string, string> eventData)
  {
    eventData.Add("beat_saber_app_version", UnityEngine.Application.version);
    eventData.Add("beat_saber_app_custom_event_type", eventType);
    CAPI.LogNewEvent("oculus_beat_saber_app_custom_event", eventData);
  }

  public virtual void LogEditAvatarEvent(string eventType, Dictionary<string, string> eventData)
  {
    eventData.Add("beat_saber_app_version", UnityEngine.Application.version);
    eventData.Add("beat_saber_app_avatar_event_type", eventType);
    CAPI.LogNewEvent("oculus_beat_saber_avatar_event", eventData);
  }

  public virtual void LogClick(string clickType, Dictionary<string, string> clickData)
  {
    clickData["click_type"] = clickType;
    this.LogEvent("Click", clickData);
  }

  public virtual void LogImpression(
    string impressionType,
    Dictionary<string, string> impressionData)
  {
    impressionData["impression_type"] = impressionType;
    this.LogEvent("Impression", impressionData);
  }

  public virtual void LogExposure(string exposureType, Dictionary<string, string> exposureData)
  {
    exposureData["exposure_type"] = exposureType;
    this.LogEvent("Exposure", exposureData);
  }
}
