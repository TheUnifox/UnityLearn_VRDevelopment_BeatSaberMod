// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventTrackDefinitionSO
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class EventTrackDefinitionSO : PersistentScriptableObject
  {
    [SerializeField]
    private EventTrackDefinitionSO.DataTransformationType _dataTransformationType;
    [SerializeField]
    private EventTrackDefinitionSO.MarkerType _markerType;
    [SerializeField]
    private bool _visible;
    [SerializeField]
    private bool _needsFiltering;

    public bool visible => this._visible;

    public EventTrackDefinitionSO.DataTransformationType dataTransformation => this._dataTransformationType;

    public EventTrackDefinitionSO.MarkerType markerType => this._markerType;

    public bool needsFiltering => this._needsFiltering;

    public enum DataTransformationType
    {
      NoTransformation,
      Light,
      DeltaRotation,
      Switch,
      TurnOffValueDuration,
      ValueDuration,
    }

    public enum MarkerType
    {
      BasicMarker,
      DurationMarker,
      LightMarker,
      TextMarker,
      TooltipMarker,
    }
  }
}
