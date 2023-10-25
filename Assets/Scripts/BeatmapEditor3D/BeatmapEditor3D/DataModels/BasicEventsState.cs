// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BasicEventsState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D.DataModels
{
  public class BasicEventsState : IBasicEventsState
  {
    private readonly Dictionary<TrackToolbarType, (int value, float floatValue)> _selectedBeatmapTypeValues = new Dictionary<TrackToolbarType, (int, float)>();
    private readonly Dictionary<TrackToolbarType, object> _selectedBeatmapTypePayloads = new Dictionary<TrackToolbarType, object>();

    public EnvironmentTracksDefinitionSO.BasicEventTrackPage currentEventsPage { get; set; }

    public List<BasicBeatmapEventType> allEventTypes { get; set; }

    public List<BasicBeatmapEventType> activeEventTypes { get; set; }

    public float currentHoverBeat { get; set; }

    public int currentHoverPageTrackId { get; set; }

    public int currentHoverVisibleTrackId { get; set; }

    public BasicEventEditorData currentHoverEvent { get; set; }

    public LightEventsVersion lightEventsVersion { get; set; } = LightEventsVersion.Version2;

    public void SetSelectedBeatmapTypeValue(
      TrackToolbarType type,
      int value,
      float floatValue,
      object payload)
    {
      this._selectedBeatmapTypeValues[type] = (value, floatValue);
      this._selectedBeatmapTypePayloads[type] = payload;
    }

    public (int value, float floatValue) GetSelectedBeatmapTypeValue(TrackToolbarType type)
    {
      (int value, float floatValue) tuple;
      return !this._selectedBeatmapTypeValues.TryGetValue(type, out tuple) ? (0, 1f) : (tuple.value, tuple.floatValue);
    }

    public object GetSelectedBeatmapTypePayload(TrackToolbarType type)
    {
      object obj;
      return !this._selectedBeatmapTypePayloads.TryGetValue(type, out obj) ? (object) null : obj;
    }

    public bool IsEventTypeActive(BasicBeatmapEventType type)
    {
      List<BasicBeatmapEventType> activeEventTypes = this.activeEventTypes;
      // ISSUE: explicit non-virtual call
      return activeEventTypes != null && __nonvirtual (activeEventTypes.Contains(type));
    }

    public void Reset()
    {
      this._selectedBeatmapTypeValues.Clear();
      this._selectedBeatmapTypePayloads.Clear();
      this.currentEventsPage = EnvironmentTracksDefinitionSO.BasicEventTrackPage.Page1;
      this.allEventTypes.Clear();
      this.activeEventTypes.Clear();
      this.currentHoverBeat = 0.0f;
      this.currentHoverPageTrackId = 0;
      this.currentHoverEvent = (BasicEventEditorData) null;
    }
  }
}
