// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveChainOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveChainOnGridCommand : MoveBeatmapObjectOnGridCommand
  {
    [Inject]
    private readonly MoveChainOnGridSignal _signal;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    private ChainEditorData _originalChain;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private ChainEditorData _newChain;
    private ArcEditorData _newHeadArc;
    private ArcEditorData _newTailArc;

    public override bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is MoveChainOnGridCommand chainOnGridCommand && this._originalChain.id == chainOnGridCommand._newChain.id;
    }

    public override void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      if (!(previousCommand is MoveChainOnGridCommand chainOnGridCommand))
        return;
      this._originalChain = chainOnGridCommand._originalChain;
      this._originalHeadArc = chainOnGridCommand._originalHeadArc;
      this._originalTailArc = chainOnGridCommand._originalTailArc;
    }

    protected override void GatherBeatmapObject()
    {
      this._originalChain = this.beatmapLevelDataModel.GetChainById(this._signal.id);
      if (this._originalChain == (ChainEditorData) null || this._signal.cellData == null)
        return;
      bool flag = this._beatmapObjectsSelectionState.draggedHead ?? this._signal.cellData.IsPositionSame(this._originalChain);
      this._beatmapObjectsSelectionState.draggedHead = new bool?(flag);
      if (flag)
      {
        this._newChain = ChainEditorData.CopyWithModifications(this._originalChain, column: new int?(this._signal.position.x), row: new int?(this._signal.position.y));
        this._originalTailArc = this.beatmapLevelDataModel.GetArcByTail(new BeatmapObjectCellData( this._originalChain));
        if (!(this._originalTailArc != (ArcEditorData) null))
          return;
        ArcEditorData originalTailArc = this._originalTailArc;
        BeatmapEditorObjectId? id = new BeatmapEditorObjectId?();
        ColorType? colorType = new ColorType?();
        float? beat = new float?();
        int? column = new int?();
        int? row = new int?();
        NoteCutDirection? cutDirection = new NoteCutDirection?();
        float? controlPointLengthMultiplier = new float?();
        float? tailBeat = new float?();
        Vector2Int position = this._signal.position;
        int? tailColumn = new int?(position.x);
        position = this._signal.position;
        int? tailRow = new int?(position.y);
        NoteCutDirection? tailCutDirection = new NoteCutDirection?();
        float? tailControlPointLengthMultiplier = new float?();
        SliderMidAnchorMode? midAnchorMode = new SliderMidAnchorMode?();
        this._newTailArc = ArcEditorData.CopyWithModifications(originalTailArc, id, colorType, beat, column, row, cutDirection, controlPointLengthMultiplier, tailBeat, tailColumn, tailRow, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
      }
      else
      {
        ChainEditorData originalChain = this._originalChain;
        BeatmapEditorObjectId? id1 = new BeatmapEditorObjectId?();
        float? beat1 = new float?();
        ColorType? colorType1 = new ColorType?();
        int? column1 = new int?();
        int? row1 = new int?();
        NoteCutDirection? cutDirection1 = new NoteCutDirection?();
        float? tailBeat1 = new float?();
        Vector2Int position = this._signal.position;
        int? tailColumn1 = new int?(position.x);
        position = this._signal.position;
        int? tailRow1 = new int?(position.y);
        int? sliceCount = new int?();
        float? squishAmount = new float?();
        this._newChain = ChainEditorData.CopyWithModifications(originalChain, id1, beat1, colorType1, column1, row1, cutDirection1, tailBeat1, tailColumn1, tailRow1, sliceCount, squishAmount);
        this._originalHeadArc = this.beatmapLevelDataModel.GetArcByHead(new BeatmapObjectCellData(new Vector2Int(this._originalChain.tailColumn, this._originalChain.tailRow), this._originalChain.tailBeat));
        if (!(this._originalHeadArc != (ArcEditorData) null))
          return;
        ArcEditorData originalHeadArc = this._originalHeadArc;
        BeatmapEditorObjectId? id2 = new BeatmapEditorObjectId?();
        ColorType? colorType2 = new ColorType?();
        float? beat2 = new float?();
        position = this._signal.position;
        int? column2 = new int?(position.x);
        position = this._signal.position;
        int? row2 = new int?(position.y);
        NoteCutDirection? cutDirection2 = new NoteCutDirection?();
        float? controlPointLengthMultiplier = new float?();
        float? tailBeat2 = new float?();
        int? tailColumn2 = new int?();
        int? tailRow2 = new int?();
        NoteCutDirection? tailCutDirection = new NoteCutDirection?();
        float? tailControlPointLengthMultiplier = new float?();
        SliderMidAnchorMode? midAnchorMode = new SliderMidAnchorMode?();
        this._newHeadArc = ArcEditorData.CopyWithModifications(originalHeadArc, id2, colorType2, beat2, column2, row2, cutDirection2, controlPointLengthMultiplier, tailBeat2, tailColumn2, tailRow2, tailCutDirection, tailControlPointLengthMultiplier, midAnchorMode);
      }
    }

    protected override bool IsCommandValid() => this._newChain != (ChainEditorData) null;

    protected override (float, int, int, float, int, int) GetNewRectangle() => this._signal.cellData.IsPositionSame((BaseBeatmapObjectEditorData) this._originalChain) ? (this._newChain.beat, this._newChain.column, this._newChain.row, this._newChain.beat, this._newChain.column, this._newChain.row) : (this._newChain.tailBeat, this._newChain.tailColumn, this._newChain.tailRow, this._newChain.tailBeat, this._newChain.tailColumn, this._newChain.tailRow);

    protected override BeatmapEditorObjectId GetOriginalId() => this._originalChain.id;

    protected override BeatmapObjectCellData GetNewBeatmapObjectCellData() => new BeatmapObjectCellData(this._signal.position, this._signal.cellData.IsPositionSame((BaseBeatmapObjectEditorData) this._originalChain) ? this._originalChain.beat : this._originalChain.tailBeat);

    protected override void SwapNewWithOld()
    {
      this.beatmapLevelDataModel.RemoveChain(this._newChain);
      this.beatmapLevelDataModel.AddChain(this._originalChain);
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
      this.beatmapLevelDataModel.RemoveChain(this._originalChain);
      this.beatmapLevelDataModel.AddChain(this._newChain);
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
