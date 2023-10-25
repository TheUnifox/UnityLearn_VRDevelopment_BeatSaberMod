// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsChangeRectangleSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsChangeRectangleSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly BeatmapObjectsChangeRectangleSelectionSignal _signal;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      bool flag = false;
      if (this._signal.changeType == BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.Start)
      {
        flag = !Mathf.Approximately(this._beatmapObjectsSelectionState.tempStartSelectionBeat, this._signal.beat);
        this._beatmapObjectsSelectionState.tempStartSelectionBeat = this._signal.beat;
        this._beatmapObjectsSelectionState.tempEndSelectionBeat = this._signal.beat;
        this._beatmapObjectsSelectionState.showSelection = true;
      }
      if (this._signal.changeType == BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.Drag || this._signal.changeType == BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.End)
      {
        flag = !Mathf.Approximately(this._beatmapObjectsSelectionState.tempEndSelectionBeat, this._signal.beat);
        this._beatmapObjectsSelectionState.tempEndSelectionBeat = this._signal.beat;
      }
      if (this._signal.changeType == BeatmapObjectsChangeRectangleSelectionSignal.ChangeType.End)
      {
        flag = true;
        this._beatmapObjectsSelectionState.showSelection = false;
      }
      this._beatmapObjectsSelectionState.startBeat = Mathf.Min(this._beatmapObjectsSelectionState.tempStartSelectionBeat, this._beatmapObjectsSelectionState.tempEndSelectionBeat);
      this._beatmapObjectsSelectionState.endBeat = Mathf.Max(this._beatmapObjectsSelectionState.tempStartSelectionBeat, this._beatmapObjectsSelectionState.tempEndSelectionBeat);
      if (!flag)
        return;
      this._signalBus.Fire<BeatmapObjectsRectangleSelectionChangedSignal>();
    }
  }
}
