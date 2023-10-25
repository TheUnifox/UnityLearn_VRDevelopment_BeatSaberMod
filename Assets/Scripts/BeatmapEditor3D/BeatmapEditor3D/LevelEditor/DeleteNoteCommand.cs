// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.DeleteNoteCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class DeleteNoteCommand : DeleteBeatmapObjectCommand
  {
    [Inject]
    private readonly DeleteNoteSignal _signal;
    private NoteEditorData _note;

    protected override void GatherBeatmapObject() => this._note = this.beatmapLevelDataModel.GetNoteById(this._signal.id);

    protected override bool ShouldAddToHistory() => this._note != (NoteEditorData) null;

    protected override void DeselectBeatmapObjectIfSelected() => this.beatmapObjectsSelectionState.IsNoteSelected(this._note.id);

    protected override void RemoveFromBeatmapLevelDataModel() => this.beatmapLevelDataModel.RemoveNote(this._note);

    protected override void AddToBeatmapLevelDataModel() => this.beatmapLevelDataModel.AddNote(this._note);
  }
}
