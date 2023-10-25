// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveArcOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveArcOnGridCommand : MoveBeatmapObjectOnGridCommand
  {
    [Inject]
    private readonly MoveArcOnGridSignal _signal;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private ArcEditorData _changedHeadArc;
    private ArcEditorData _changedTailArc;

    public override bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      if (!(previousCommand is MoveArcOnGridCommand arcOnGridCommand) || !(this._originalHeadArc == (ArcEditorData) null) && !(arcOnGridCommand._changedHeadArc == (ArcEditorData) null) && !(this._originalHeadArc.id == arcOnGridCommand._changedHeadArc.id))
        return false;
      return this._originalTailArc == (ArcEditorData) null || arcOnGridCommand._changedTailArc == (ArcEditorData) null || this._originalTailArc.id == arcOnGridCommand._changedTailArc.id;
    }

    public override void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      if (!(previousCommand is MoveArcOnGridCommand arcOnGridCommand))
        return;
      this._originalHeadArc = arcOnGridCommand._originalHeadArc;
      this._originalTailArc = arcOnGridCommand._originalTailArc;
    }

    protected override void GatherBeatmapObject()
    {
      if (this._signal.cellData == null)
        return;
      this._originalHeadArc = this.beatmapLevelDataModel.GetArcByHead(this._signal.cellData);
      Vector2Int position;
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        ArcEditorData originalHeadArc = this._originalHeadArc;
        BeatmapEditorObjectId? id = new BeatmapEditorObjectId?();
        ColorType? colorType = new ColorType?();
        float? beat = new float?(this._signal.cellData.beat);
        position = this._signal.position;
        int? column = new int?(position.x);
        position = this._signal.position;
        int? row = new int?(position.y);
        NoteCutDirection? cutDirection = new NoteCutDirection?();
        float? controlPointLengthMultiplier = new float?();
        float? tailBeat = new float?();
        int? tailColumn = new int?();
        int? tailRow = new int?();
        NoteCutDirection? tailCutDirection = new NoteCutDirection?();
        float? tailControlPointLengthMultiplier = new float?();
        SliderMidAnchorMode? midAnchorMode = new SliderMidAnchorMode?();
        this._changedHeadArc = ArcEditorData.CopyWithModifications(originalHeadArc, id, colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
      }
      this._originalTailArc = this.beatmapLevelDataModel.GetArcByTail(this._signal.cellData);
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      ArcEditorData originalTailArc = this._originalTailArc;
      BeatmapEditorObjectId? id1 = new BeatmapEditorObjectId?();
      ColorType? colorType1 = new ColorType?();
      float? beat1 = new float?();
      int? column1 = new int?();
      int? row1 = new int?();
      NoteCutDirection? cutDirection1 = new NoteCutDirection?();
      float? controlPointLengthMultiplier1 = new float?();
      float? tailBeat1 = new float?(this._signal.cellData.beat);
      position = this._signal.position;
      int? tailColumn1 = new int?(position.x);
      position = this._signal.position;
      int? tailRow1 = new int?(position.y);
      NoteCutDirection? tailCutDirection1 = new NoteCutDirection?();
      float? tailControlPointLengthMultiplier1 = new float?();
      SliderMidAnchorMode? midAnchorMode1 = new SliderMidAnchorMode?();
      this._changedTailArc = ArcEditorData.CopyWithModifications(originalTailArc, id1, colorType1, beat1, column1, row1, cutDirection1, controlPointLengthMultiplier1, tailBeat1, tailColumn1, tailRow1, tailCutDirection1, tailControlPointLengthMultiplier1, midAnchorMode1);
    }

    protected override bool IsCommandValid() => this._originalHeadArc != (ArcEditorData) null || this._originalTailArc != (ArcEditorData) null;

    protected override (float, int, int, float, int, int) GetNewRectangle()
    {
      double beat1 = (double) this._signal.cellData.beat;
      Vector2Int position = this._signal.position;
      int x1 = position.x;
      position = this._signal.position;
      int y1 = position.y;
      double beat2 = (double) this._signal.cellData.beat;
      position = this._signal.position;
      int x2 = position.x;
      position = this._signal.position;
      int y2 = position.y;
      return ((float) beat1, x1, y1, (float) beat2, x2, y2);
    }

    protected override BeatmapEditorObjectId GetOriginalId() => BeatmapEditorObjectId.invalid;

    protected override BeatmapObjectCellData GetNewBeatmapObjectCellData() => new BeatmapObjectCellData(this._signal.position, this._signal.cellData.beat);

    protected override void SwapNewWithOld()
    {
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        this.beatmapLevelDataModel.RemoveArc(this._changedHeadArc);
        this.beatmapLevelDataModel.AddArc(this._originalHeadArc);
      }
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      this.beatmapLevelDataModel.RemoveArc(this._changedTailArc);
      this.beatmapLevelDataModel.AddArc(this._originalTailArc);
    }

    protected override void SwapOldWithNew()
    {
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        this.beatmapLevelDataModel.RemoveArc(this._originalHeadArc);
        this.beatmapLevelDataModel.AddArc(this._changedHeadArc);
      }
      if (!(this._originalTailArc != (ArcEditorData) null))
        return;
      this.beatmapLevelDataModel.RemoveArc(this._originalTailArc);
      this.beatmapLevelDataModel.AddArc(this._changedTailArc);
    }
  }
}
