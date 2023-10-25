// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeHoveredNoteColorTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeHoveredNoteColorTypeCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeHoveredNoteColorTypeSignal _signal;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private NoteEditorData _originalNote;
    private NoteEditorData _changedNote;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._originalNote = this._beatmapLevelDataModel.GetNoteById(this._readonlyBeatmapObjectsState.hoveredBeatmapObjectId);
      if (this._originalNote == (NoteEditorData) null)
        return;
      this._changedNote = NoteEditorData.CopyWithModifications(this._originalNote, type: new ColorType?(this._signal.colorType));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.RemoveNote(this._changedNote);
      this._beatmapLevelDataModel.AddNote(this._originalNote);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveNote(this._originalNote);
      this._beatmapLevelDataModel.AddNote(this._changedNote);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
