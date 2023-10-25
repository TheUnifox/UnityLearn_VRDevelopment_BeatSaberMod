// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeHoveredDotNoteAngleCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeHoveredDotNoteAngleCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    private NoteEditorData _originalNoteData;
    private NoteEditorData _modifiedNoteData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._readonlyBeatmapObjectsState.hoveredNoteCellData == null)
        return;
      this._originalNoteData = this._beatmapLevelDataModel.GetNote(this._readonlyBeatmapObjectsState.hoveredNoteCellData);
      if (this._originalNoteData == (NoteEditorData) null || this._originalNoteData.cutDirection != NoteCutDirection.Any)
        return;
      this._modifiedNoteData = NoteEditorData.CopyWithModifications(this._originalNoteData, angle: new int?(this._originalNoteData.angle == 0 ? 45 : 0));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveNote(this._modifiedNoteData);
      this._beatmapLevelDataModel.AddNote(this._originalNoteData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveNote(this._originalNoteData);
      this._beatmapLevelDataModel.AddNote(this._modifiedNoteData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
