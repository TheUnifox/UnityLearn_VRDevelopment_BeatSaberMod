// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLaserRotationSpeedCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ModifyHoveredLaserRotationSpeedCommand : ModifyEventCommand
  {
    [Inject]
    private readonly ModifyHoveredLaserRotationSpeedSignal _signal;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;

    protected override bool IsCorrectType(TrackToolbarType type) => type == TrackToolbarType.IntValue;

    protected override BasicEventEditorData GetOriginalEventData() => this._basicEventsState.currentHoverEvent;

    protected override BasicEventEditorData GetModifiedEventData(
      BasicEventEditorData originalBasicEventData)
    {
      int num = Mathf.Max(0, originalBasicEventData.value + (int) Mathf.Sign(this._signal.delta));
      return BasicEventEditorData.CreateNewWithId(originalBasicEventData.id, originalBasicEventData.type, originalBasicEventData.beat, num, originalBasicEventData.floatValue);
    }
  }
}
