// Decompiled with JetBrains decompiler
// Type: TestAnalyticsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestAnalyticsModel : IAnalyticsModel
{
  public bool supportsOpenDataPrivacyPage => false;

  public virtual void OpenDataPrivacyPage()
  {
  }

  public virtual void LogEvent(string eventType, Dictionary<string, string> eventData)
  {
    string str = string.Join("\n", eventData.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kvp => "\t" + kvp.Key + ": " + kvp.Value)));
    Debug.Log((object) (eventType + ":\n" + str));
  }

  public virtual void LogEditAvatarEvent(
    string eventType,
    Dictionary<string, string> avatarEventData)
  {
    this.LogEvent("Edit Avatar Event", new Dictionary<string, string>((IDictionary<string, string>) avatarEventData)
    {
      ["event_type"] = eventType
    });
  }

  public virtual void LogClick(string clickType, Dictionary<string, string> clickData) => this.LogEvent("Click", new Dictionary<string, string>((IDictionary<string, string>) clickData)
  {
    ["click_type"] = clickType
  });

  public virtual void LogImpression(
    string impressionType,
    Dictionary<string, string> impressionData)
  {
    this.LogEvent("Impression", new Dictionary<string, string>((IDictionary<string, string>) impressionData)
    {
      ["impression_type"] = impressionType
    });
  }

  public virtual void LogExposure(string exposureType, Dictionary<string, string> exposureData) => this.LogEvent("Exposure", new Dictionary<string, string>((IDictionary<string, string>) exposureData)
  {
    ["exposure_type"] = exposureType
  });
}
