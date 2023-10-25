// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IBasicEventsState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D.DataModels
{
  public interface IBasicEventsState
  {
    EnvironmentTracksDefinitionSO.BasicEventTrackPage currentEventsPage { get; set; }

    float currentHoverBeat { get; set; }

    int currentHoverPageTrackId { get; set; }

    int currentHoverVisibleTrackId { get; set; }

    BasicEventEditorData currentHoverEvent { get; set; }

    List<BasicBeatmapEventType> allEventTypes { get; set; }

    List<BasicBeatmapEventType> activeEventTypes { get; set; }

    LightEventsVersion lightEventsVersion { get; set; }

    (int value, float floatValue) GetSelectedBeatmapTypeValue(TrackToolbarType type);

    object GetSelectedBeatmapTypePayload(TrackToolbarType type);

    bool IsEventTypeActive(BasicBeatmapEventType type);

    void Reset();
  }
}
