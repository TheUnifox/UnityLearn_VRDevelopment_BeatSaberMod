// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightEventDeltaIntensityCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLightEventDeltaIntensityCommand : ModifyEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLightEventDeltaIntensitySignal _signal;
    [Inject]
    private readonly IBasicEventsState _basicEvents;
    private const float kStep = 0.01f;

    protected override bool IsCorrectType(TrackToolbarType type) => type == TrackToolbarType.Lights;

    protected override BasicEventEditorData GetOriginalEventData() => this._basicEvents.currentHoverEvent;

    protected override BasicEventEditorData GetModifiedEventData(
      BasicEventEditorData originalBasicEventData)
    {
      double floatValue = Math.Round((double) Mathf.Max(0.0f, originalBasicEventData.floatValue + Mathf.Sign(this._signal.newDeltaIntensity) * 0.01f) * 100.0) / 100.0;
      LightEventsPayload lightEventsPayload = new LightEventsPayload(originalBasicEventData.value, (float) floatValue);
      return BasicEventEditorData.CreateNewWithId(originalBasicEventData.id, originalBasicEventData.type, originalBasicEventData.beat, lightEventsPayload.ToValue(), lightEventsPayload.ToAltValue());
    }
  }
}
