// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.EventBoxesChangeRectangleSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class EventBoxesChangeRectangleSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxesChangeRectangleSelectionSignal _signal;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;

    public void Execute()
    {
      switch (this._signal.changeType)
      {
        case EventBoxesChangeRectangleSelectionSignal.ChangeType.Start:
          this._selectionState.tempStartEventBoxIndex = this._signal.eventBoxIndex;
          this._selectionState.tempStartBeat = this._beatmapState.beat;
          this._selectionState.showSelection = true;
          break;
        case EventBoxesChangeRectangleSelectionSignal.ChangeType.Drag:
        case EventBoxesChangeRectangleSelectionSignal.ChangeType.End:
          this._selectionState.tempEndEventBoxIndex = this._signal.eventBoxIndex;
          this._selectionState.tempEndBeat = this._signal.beat;
          break;
      }
      if (this._signal.changeType == EventBoxesChangeRectangleSelectionSignal.ChangeType.End)
        this._selectionState.showSelection = false;
      this._selectionState.startEventBoxIndex = Mathf.Min(this._selectionState.tempStartEventBoxIndex, this._selectionState.tempEndEventBoxIndex);
      this._selectionState.endEventBoxIndex = Mathf.Max(this._selectionState.tempStartEventBoxIndex, this._selectionState.tempEndEventBoxIndex);
      this._selectionState.startBeat = Mathf.Min(this._selectionState.tempStartBeat, this._selectionState.tempEndBeat);
      this._selectionState.endBeat = Mathf.Max(this._selectionState.tempStartBeat, this._selectionState.tempEndBeat);
    }
  }
}
