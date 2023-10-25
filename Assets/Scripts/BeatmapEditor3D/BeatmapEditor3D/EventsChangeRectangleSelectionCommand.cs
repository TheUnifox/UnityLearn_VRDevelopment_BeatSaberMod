// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventsChangeRectangleSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class EventsChangeRectangleSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventsChangeRectangleSelectionSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;

    public void Execute()
    {
      bool flag = false;
      switch (this._signal.changeType)
      {
        case EventsChangeRectangleSelectionSignal.ChangeType.Start:
          flag = this._eventsSelectionState.tempStartTrackIndex != this._signal.trackIndex || !Mathf.Approximately(this._eventsSelectionState.tempStartBeat, this._signal.beat);
          this._eventsSelectionState.tempStartTrackIndex = this._signal.trackIndex;
          this._eventsSelectionState.tempStartBeat = this._signal.beat;
          this._eventsSelectionState.showSelection = true;
          break;
        case EventsChangeRectangleSelectionSignal.ChangeType.Drag:
        case EventsChangeRectangleSelectionSignal.ChangeType.End:
          flag = this._eventsSelectionState.tempEndTrackIndex != this._signal.trackIndex || !Mathf.Approximately(this._eventsSelectionState.tempEndBeat, this._signal.beat);
          this._eventsSelectionState.tempEndTrackIndex = this._signal.trackIndex;
          this._eventsSelectionState.tempEndBeat = this._signal.beat;
          break;
      }
      if (this._signal.changeType == EventsChangeRectangleSelectionSignal.ChangeType.End)
      {
        flag = true;
        this._eventsSelectionState.showSelection = false;
      }
      this._eventsSelectionState.startTrackIndex = Mathf.Min(this._eventsSelectionState.tempStartTrackIndex, this._eventsSelectionState.tempEndTrackIndex);
      this._eventsSelectionState.endTrackIndex = Mathf.Max(this._eventsSelectionState.tempStartTrackIndex, this._eventsSelectionState.tempEndTrackIndex);
      this._eventsSelectionState.startBeat = Mathf.Min(this._eventsSelectionState.tempStartBeat, this._eventsSelectionState.tempEndBeat);
      this._eventsSelectionState.endBeat = Mathf.Max(this._eventsSelectionState.tempStartBeat, this._eventsSelectionState.tempEndBeat);
      if (!flag)
        return;
      this._signalBus.Fire<EventsSelectionRectangleChangedSignal>();
    }
  }
}
