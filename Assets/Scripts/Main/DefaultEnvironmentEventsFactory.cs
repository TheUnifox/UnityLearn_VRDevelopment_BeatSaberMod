// Decompiled with JetBrains decompiler
// Type: DefaultEnvironmentEventsFactory
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class DefaultEnvironmentEventsFactory
{
  public static void InsertDefaultEnvironmentEvents(
    BeatmapData beatmapData,
    BeatmapEventDataBoxGroupLists beatmapEventDataBoxGroupLists,
    DefaultEnvironmentEvents defaultEnvironmentEvents,
    EnvironmentLightGroups environmentLightGroups)
  {
    if (defaultEnvironmentEvents == null || defaultEnvironmentEvents.isEmpty)
    {
      beatmapData.InsertBeatmapEventData((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event0, 1, 1f));
      beatmapData.InsertBeatmapEventData((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event4, 1, 1f));
    }
    else
    {
      if (defaultEnvironmentEvents.basicBeatmapEvents != null)
      {
        foreach (DefaultEnvironmentEvents.BasicBeatmapEvent basicBeatmapEvent in defaultEnvironmentEvents.basicBeatmapEvents)
          beatmapData.InsertBeatmapEventData((BeatmapEventData) new BasicBeatmapEventData(0.0f, basicBeatmapEvent.eventType, basicBeatmapEvent.value, basicBeatmapEvent.floatValue));
      }
      if (defaultEnvironmentEvents.lightGroupEvents == null)
        return;
      foreach (DefaultEnvironmentEvents.LightGroupEvent lightGroupEvent in defaultEnvironmentEvents.lightGroupEvents)
      {
        LightGroupSO dataForGroup = environmentLightGroups.GetDataForGroup(lightGroupEvent.lightGroup.groupId);
        if (!((Object) dataForGroup == (Object) null))
        {
          BeatmapEventDataBoxGroup eventDataBoxGroup = BeatmapEventDataBoxGroupFactory.CreateSingleLightBeatmapEventDataBoxGroup(0.0f, dataForGroup.numberOfElements, lightGroupEvent);
          beatmapEventDataBoxGroupLists.Insert(dataForGroup.groupId, eventDataBoxGroup);
        }
      }
    }
  }
}
