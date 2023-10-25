// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.MoveNoteToBeatLineCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class MoveNoteToBeatLineCommand : MoveBeatmapObjectToBeatLineCommand
  {
    [Inject]
    private readonly MoveNoteToBeatLineSignal _signal;
    private NoteEditorData _originalNote;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private NoteEditorData _newNote;
    private ArcEditorData _newHeadArc;
    private ArcEditorData _newTailArc;

    protected override (BeatmapEditorObjectId id, BeatmapObjectType type) GetDraggedData() => (this._signal.id, BeatmapObjectType.Note);

    protected override BeatmapObjectCellData GetObjectCellData() => this._signal.cellData;

    protected override void GatherBeatmapObject(float beat, int column, int row)
    {
      this._originalNote = this.beatmapLevelDataModel.GetNoteById(this._signal.id);
      this._newNote = NoteEditorData.CopyWithModifications(this._originalNote, beat: new float?(beat), column: new int?(column), row: new int?(row));
      this._originalHeadArc = this.beatmapLevelDataModel.GetArcByHead(this._signal.cellData);
      this._originalTailArc = this.beatmapLevelDataModel.GetArcByTail(this._signal.cellData);
      if (this._originalHeadArc != (ArcEditorData) null)
        this._newHeadArc = ArcEditorData.CopyWithModifications(this._originalHeadArc, beat: new float?(beat), column: new int?(column), row: new int?(row));
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      this._newTailArc = ArcEditorData.CopyWithModifications(this._originalTailArc, tailBeat: new float?(beat), tailColumn: new int?(column), tailRow: new int?(row));
    }

    protected override (float, int, int, float, int, int) GetNewRectangle(
      float beat,
      int column,
      int row)
    {
      return (beat, column, row, beat, column, row);
    }

    protected override void SwapNewWithOld()
    {
      this.beatmapLevelDataModel.RemoveNote(this._newNote);
      this.beatmapLevelDataModel.AddNote(this._originalNote);
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        this.beatmapLevelDataModel.RemoveArc(this._newHeadArc);
        this.beatmapLevelDataModel.AddArc(this._originalHeadArc);
      }
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      this.beatmapLevelDataModel.RemoveArc(this._newTailArc);
      this.beatmapLevelDataModel.AddArc(this._originalTailArc);
    }

    protected override void SwapOldWithNew()
    {
      this.beatmapLevelDataModel.RemoveNote(this._originalNote);
      this.beatmapLevelDataModel.AddNote(this._newNote);
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        this.beatmapLevelDataModel.RemoveArc(this._originalHeadArc);
        this.beatmapLevelDataModel.AddArc(this._newHeadArc);
      }
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      this.beatmapLevelDataModel.RemoveArc(this._originalTailArc);
      this.beatmapLevelDataModel.AddArc(this._newTailArc);
    }
  }
}
