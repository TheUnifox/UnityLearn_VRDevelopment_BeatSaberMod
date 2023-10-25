// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.MoveChainToBeatLineCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class MoveChainToBeatLineCommand : MoveBeatmapObjectToBeatLineCommand
  {
    [Inject]
    private readonly MoveChainToBeatLineSignal _signal;
    private ChainEditorData _originalChain;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private ChainEditorData _newChain;
    private ArcEditorData _newHeadArc;
    private ArcEditorData _newTailArc;

    protected override (BeatmapEditorObjectId id, BeatmapObjectType type) GetDraggedData() => (this._signal.id, BeatmapObjectType.Chain);

    protected override BeatmapObjectCellData GetObjectCellData() => this._signal.cellData;

    protected override bool IsCommandValid() => this._newChain != (ChainEditorData) null;

    protected override void GatherBeatmapObject(float beat, int column, int row)
    {
      this._originalChain = this.beatmapLevelDataModel.GetChainById(this._signal.id);
      bool flag = AudioTimeHelper.IsBeatSame(this._originalChain.beat, this._signal.cellData.beat) && this._signal.cellData.cellPosition.x == this._originalChain.column && this._signal.cellData.cellPosition.y == this._originalChain.row;
      if (!this._signal.moveSingleEnd)
      {
        float num = this._originalChain.tailBeat - this._originalChain.beat;
        this._originalHeadArc = this.beatmapLevelDataModel.GetArcByHead(new BeatmapObjectCellData(new Vector2Int(this._originalChain.column, this._originalChain.row), this._originalChain.beat));
        if (this._originalHeadArc != (ArcEditorData) null)
          this._newHeadArc = ArcEditorData.CopyWithModifications(this._originalHeadArc, beat: new float?(beat), column: new int?(column), row: new int?(row));
        this._originalTailArc = this.beatmapLevelDataModel.GetArcByTail(new BeatmapObjectCellData(new Vector2Int(this._originalChain.tailColumn, this._originalChain.tailRow), this._originalChain.tailBeat));
        if (this._originalTailArc != (ArcEditorData) null)
          this._newTailArc = ArcEditorData.CopyWithModifications(this._originalTailArc, tailBeat: new float?(beat), tailColumn: new int?(column), tailRow: new int?(row));
        this._newChain = flag ? ChainEditorData.CopyWithModifications(this._originalChain, beat: new float?(beat), column: new int?(column), row: new int?(row), tailBeat: new float?(beat + num)) : ChainEditorData.CopyWithModifications(this._originalChain, beat: new float?(beat - num), tailBeat: new float?(beat), tailColumn: new int?(column), tailRow: new int?(row));
      }
      else if (flag)
      {
        if ((double) beat > (double) this._originalChain.tailBeat)
          return;
        this._originalTailArc = this.beatmapLevelDataModel.GetArcByTail(new BeatmapObjectCellData(new Vector2Int(this._originalChain.column, this._originalChain.row), this._originalChain.beat));
        if (this._originalTailArc != (ArcEditorData) null)
          this._newTailArc = ArcEditorData.CopyWithModifications(this._originalTailArc, tailBeat: new float?(beat), tailColumn: new int?(column), tailRow: new int?(row));
        this._newChain = ChainEditorData.CopyWithModifications(this._originalChain, beat: new float?(beat), column: new int?(column), row: new int?(row));
      }
      else
      {
        if ((double) beat < (double) this._originalChain.beat)
          return;
        this._originalHeadArc = this.beatmapLevelDataModel.GetArcByHead(new BeatmapObjectCellData(new Vector2Int(this._originalChain.tailColumn, this._originalChain.tailRow), this._originalChain.tailBeat));
        if (this._originalHeadArc != (ArcEditorData) null)
          this._newHeadArc = ArcEditorData.CopyWithModifications(this._originalHeadArc, beat: new float?(beat), column: new int?(column), row: new int?(row));
        this._newChain = ChainEditorData.CopyWithModifications(this._originalChain, tailBeat: new float?(beat), tailColumn: new int?(column), tailRow: new int?(row));
      }
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
      this.beatmapLevelDataModel.RemoveChain(this._newChain);
      this.beatmapLevelDataModel.AddChain(this._originalChain);
      if (this._newHeadArc != (ArcEditorData) null && this._originalHeadArc != (ArcEditorData) null)
      {
        this.beatmapLevelDataModel.RemoveArc(this._newHeadArc);
        this.beatmapLevelDataModel.AddArc(this._originalHeadArc);
      }
      if (!(this._newTailArc != (ArcEditorData) null) || !(this._originalTailArc != (ArcEditorData) null))
        return;
      this.beatmapLevelDataModel.RemoveArc(this._newTailArc);
      this.beatmapLevelDataModel.AddArc(this._originalTailArc);
    }

    protected override void SwapOldWithNew()
    {
      this.beatmapLevelDataModel.RemoveChain(this._originalChain);
      this.beatmapLevelDataModel.AddChain(this._newChain);
      if (this._newHeadArc != (ArcEditorData) null && this._originalHeadArc != (ArcEditorData) null)
      {
        this.beatmapLevelDataModel.RemoveArc(this._originalHeadArc);
        this.beatmapLevelDataModel.AddArc(this._newHeadArc);
      }
      if (!(this._newTailArc != (ArcEditorData) null) || !(this._originalTailArc != (ArcEditorData) null))
        return;
      this.beatmapLevelDataModel.RemoveArc(this._originalTailArc);
      this.beatmapLevelDataModel.AddArc(this._newTailArc);
    }
  }
}
