// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PlaceLightColorEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class PlaceLightColorEventCommand : PlaceLightEventCommand
  {
    [Inject]
    private readonly PlaceLightColorEventSignal _signal;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;

    protected override float GetBeat() => this._signal.eventBoxGroupBeat;

    protected override BeatmapEditorObjectId GetEventBoxId() => this._signal.eventBoxId;

    protected override BaseEditorData CreateEventData(float beat) => !this._eventBoxGroupsState.eventBoxGroupExtension ? (BaseEditorData) LightColorBaseEditorData.CreateNew(beat, this._eventBoxGroupsState.lightColorBrightness, this._eventBoxGroupsState.lightColorTransitionType, this._eventBoxGroupsState.lightColorType, this._eventBoxGroupsState.lightStrobeBeatFrequency) : (BaseEditorData) LightColorBaseEditorData.CreateExtension(beat);
  }
}
