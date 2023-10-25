// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataTransformerProvider
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public class DataTransformerProvider
  {
    private readonly IEventDataTransformer _defaultTransformer = (IEventDataTransformer) new EmptyEventDataTransformer();
    private readonly LightEventDataTransformer _lightEventDataTransformer = new LightEventDataTransformer();
    private readonly DeltaRotationDataTransformer _deltaRotationDataTransformer = new DeltaRotationDataTransformer();
    private readonly TriggerSwitchDataTransformer _triggerSwitchDataTransformer = new TriggerSwitchDataTransformer();
    private readonly TurnOffValueDurationDataTransformer _turnOffValueDurationDataTransformer = new TurnOffValueDurationDataTransformer();
    private readonly ValueDurationDataTransformer _valueDurationDataTransformer = new ValueDurationDataTransformer();

    public IEventDataTransformer GetDataTransformer(
      EventTrackDefinitionSO.DataTransformationType type)
    {
      switch (type)
      {
        case EventTrackDefinitionSO.DataTransformationType.Light:
          return (IEventDataTransformer) this._lightEventDataTransformer;
        case EventTrackDefinitionSO.DataTransformationType.DeltaRotation:
          return (IEventDataTransformer) this._deltaRotationDataTransformer;
        case EventTrackDefinitionSO.DataTransformationType.Switch:
          return (IEventDataTransformer) this._triggerSwitchDataTransformer;
        case EventTrackDefinitionSO.DataTransformationType.TurnOffValueDuration:
          return (IEventDataTransformer) this._turnOffValueDurationDataTransformer;
        case EventTrackDefinitionSO.DataTransformationType.ValueDuration:
          return (IEventDataTransformer) this._valueDurationDataTransformer;
        default:
          return this._defaultTransformer;
      }
    }
  }
}
