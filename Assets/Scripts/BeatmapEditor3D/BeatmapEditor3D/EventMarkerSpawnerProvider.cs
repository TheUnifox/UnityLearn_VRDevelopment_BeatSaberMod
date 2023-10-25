// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventMarkerSpawnerProvider
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public class EventMarkerSpawnerProvider
  {
    private readonly BasicEventMarkerSpawner _basicEventMarkerSpawner;
    private readonly LightEventMarkerSpawner _lightEventMarkerSpawner;
    private readonly DurationEventMarkerSpawner _durationEventMarkerSpawner;

    public EventMarkerSpawnerProvider(
      BasicEventMarkerSpawner basicEventMarkerSpawner,
      LightEventMarkerSpawner lightEventMarkerSpawner,
      DurationEventMarkerSpawner durationEventMarkerSpawner)
    {
      this._basicEventMarkerSpawner = basicEventMarkerSpawner;
      this._lightEventMarkerSpawner = lightEventMarkerSpawner;
      this._durationEventMarkerSpawner = durationEventMarkerSpawner;
    }

    public IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData> GetMarkerSpawner(
      EventTrackDefinitionSO.MarkerType type)
    {
      switch (type)
      {
        case EventTrackDefinitionSO.MarkerType.BasicMarker:
          return (IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>) this._basicEventMarkerSpawner;
        case EventTrackDefinitionSO.MarkerType.DurationMarker:
          return (IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>) this._durationEventMarkerSpawner;
        case EventTrackDefinitionSO.MarkerType.LightMarker:
          return (IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>) this._lightEventMarkerSpawner;
        default:
          return (IEventMarkerSpawner<BasicBeatmapEventType, BasicEventEditorData>) null;
      }
    }
  }
}
