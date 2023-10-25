// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveNoteOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveNoteOnGridCommand : MoveBeatmapObjectOnGridCommand
  {
    [Inject]
    private readonly MoveNoteOnGridSignal _signal;
    private NoteEditorData _originalNote;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private NoteEditorData _newNote;
    private ArcEditorData _newHeadArc;
    private ArcEditorData _newTailArc;

    public override bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is MoveNoteOnGridCommand noteOnGridCommand && this._originalNote.id == noteOnGridCommand._newNote.id;
    }

    public override void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      if (!(previousCommand is MoveNoteOnGridCommand noteOnGridCommand))
        return;
      this._originalNote = noteOnGridCommand._originalNote;
      this._originalHeadArc = noteOnGridCommand._originalHeadArc;
      this._originalTailArc = noteOnGridCommand._originalTailArc;
    }

    protected override void GatherBeatmapObject()
    {
      this._originalNote = this.beatmapLevelDataModel.GetNoteById(this._signal.id);
      if (this._originalNote == (NoteEditorData) null)
        return;
      NoteEditorData originalNote = this._originalNote;
      BeatmapEditorObjectId? id1 = new BeatmapEditorObjectId?();
      float? beat1 = new float?();
      Vector2Int position = this._signal.position;
      int? column1 = new int?(position.x);
      position = this._signal.position;
      int? row1 = new int?(position.y);
      ColorType? type = new ColorType?();
      NoteType? noteType = new NoteType?();
      NoteCutDirection? cutDirection1 = new NoteCutDirection?();
      int? angle = new int?();
      this._newNote = NoteEditorData.CopyWithModifications(originalNote, id1, beat1, column1, row1, type, noteType, cutDirection1, angle);
      BeatmapObjectCellData cellData = new BeatmapObjectCellData(new Vector2Int(this._originalNote.column, this._originalNote.row), this._originalNote.beat);
      this._originalHeadArc = this.beatmapLevelDataModel.GetArcByHead(cellData);
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        ArcEditorData originalHeadArc = this._originalHeadArc;
        BeatmapEditorObjectId? id2 = new BeatmapEditorObjectId?();
        ColorType? colorType = new ColorType?();
        float? beat2 = new float?();
        position = this._signal.position;
        int? column2 = new int?(position.x);
        position = this._signal.position;
        int? row2 = new int?(position.y);
        NoteCutDirection? cutDirection2 = new NoteCutDirection?();
        float? controlPointLengthMultiplier = new float?();
        float? tailBeat = new float?();
        int? tailColumn = new int?();
        int? tailRow = new int?();
        NoteCutDirection? tailCutDirection = new NoteCutDirection?();
        float? tailControlPointLengthMultiplier = new float?();
        SliderMidAnchorMode? midAnchorMode = new SliderMidAnchorMode?();
        this._newHeadArc = ArcEditorData.CopyWithModifications(originalHeadArc, id2, colorType, beat2, column2, row2, cutDirection2, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
      }
      this._originalTailArc = this.beatmapLevelDataModel.GetArcByTail(cellData);
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      ArcEditorData originalTailArc = this._originalTailArc;
      BeatmapEditorObjectId? id3 = new BeatmapEditorObjectId?();
      ColorType? colorType1 = new ColorType?();
      float? beat3 = new float?();
      int? column3 = new int?();
      int? row3 = new int?();
      NoteCutDirection? cutDirection3 = new NoteCutDirection?();
      float? controlPointLengthMultiplier1 = new float?();
      float? tailBeat1 = new float?();
      position = this._signal.position;
      int? tailColumn1 = new int?(position.x);
      position = this._signal.position;
      int? tailRow1 = new int?(position.y);
      NoteCutDirection? tailCutDirection1 = new NoteCutDirection?();
      float? tailControlPointLengthMultiplier1 = new float?();
      SliderMidAnchorMode? midAnchorMode1 = new SliderMidAnchorMode?();
      this._newTailArc = ArcEditorData.CopyWithModifications(originalTailArc, id3, colorType1, beat3, column3, row3, cutDirection3, controlPointLengthMultiplier1, tailBeat1, tailColumn1, tailRow1, tailCutDirection1, tailControlPointLengthMultiplier1, midAnchorMode1);
    }

    protected override bool IsCommandValid() => this._newNote != (NoteEditorData) null;

    protected override (float, int, int, float, int, int) GetNewRectangle()
    {
      double beat1 = (double) this._originalNote.beat;
      Vector2Int position = this._signal.position;
      int x1 = position.x;
      position = this._signal.position;
      int y1 = position.y;
      double beat2 = (double) this._originalNote.beat;
      position = this._signal.position;
      int x2 = position.x;
      position = this._signal.position;
      int y2 = position.y;
      return ((float) beat1, x1, y1, (float) beat2, x2, y2);
    }

    protected override BeatmapEditorObjectId GetOriginalId() => this._originalNote.id;

    protected override BeatmapObjectCellData GetNewBeatmapObjectCellData() => new BeatmapObjectCellData(this._signal.position, this._originalNote.beat);

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
