// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightEventTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightEventTypeCommand : ModifyEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightEventTypeSignal _signal;
    [Inject]
    private readonly IBasicEventsState _basicEvents;

    protected override bool IsCorrectType(TrackToolbarType type) => type == TrackToolbarType.Lights;

    protected override BasicEventEditorData GetOriginalEventData() => this._basicEvents.currentHoverEvent;

    protected override BasicEventEditorData GetModifiedEventData(
      BasicEventEditorData originalBasicEventData)
    {
      LightEventsPayload lightEventsPayload = new LightEventsPayload(originalBasicEventData.value, originalBasicEventData.floatValue)
      {
        type = this._signal.newType
      };
      return BasicEventEditorData.CreateNewWithId(originalBasicEventData.id, originalBasicEventData.type, originalBasicEventData.beat, lightEventsPayload.ToValue(), lightEventsPayload.ToAltValue());
    }
  }
}
