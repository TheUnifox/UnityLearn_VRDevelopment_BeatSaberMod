// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyLightEventColorCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyLightEventColorCommand : ModifyEventCommand
  {
    [Inject]
    private readonly ModifyLightEventColorSignal _colorSignal;

    protected override bool IsCorrectType(TrackToolbarType type) => type == TrackToolbarType.Lights;

    protected override BasicEventEditorData GetOriginalEventData() => this._colorSignal.evt;

    protected override BasicEventEditorData GetModifiedEventData(
      BasicEventEditorData originalBasicEventData)
    {
      LightEventsPayload lightEventsPayload = new LightEventsPayload(this._colorSignal.evt.value, this._colorSignal.evt.floatValue);
      switch (lightEventsPayload.color)
      {
        case LightColor.Blue:
          lightEventsPayload.color = LightColor.Red;
          break;
        case LightColor.Red:
          lightEventsPayload.color = LightColor.White;
          break;
        case LightColor.White:
          lightEventsPayload.color = LightColor.Blue;
          break;
      }
      return BasicEventEditorData.CreateNewWithId(originalBasicEventData.id, this._colorSignal.evt.type, this._colorSignal.evt.beat, lightEventsPayload.ToValue(), lightEventsPayload.ToAltValue());
    }
  }
}
