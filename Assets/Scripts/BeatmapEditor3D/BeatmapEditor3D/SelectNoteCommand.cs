// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SelectNoteCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class SelectNoteCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectNoteSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      NoteEditorData noteById = this._beatmapLevelDataModel.GetNoteById(this._signal.id);
      if (noteById == (NoteEditorData) null)
        return;
      if (this._beatmapObjectsSelectionState.IsNoteSelected(this._signal.id))
        this._beatmapObjectsSelectionState.RemoveNote(this._signal.id);
      else
        this._beatmapObjectsSelectionState.AddNote(this._signal.id);
      ArcEditorData arcByHead = this._beatmapLevelDataModel.GetArcByHead(new BeatmapObjectCellData(new Vector2Int(noteById.column, noteById.row), noteById.beat));
      if (arcByHead != (ArcEditorData) null)
      {
        if (this._beatmapObjectsSelectionState.IsArcSelected(arcByHead.id))
          this._beatmapObjectsSelectionState.RemoveArc(arcByHead.id);
        else
          this._beatmapObjectsSelectionState.AddArc(arcByHead.id);
      }
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
    }
  }
}
