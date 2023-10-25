// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChangeHoveredArcControlPointLengthCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ChangeHoveredArcControlPointLengthCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    private const float kControlPointChange = 0.1f;
    [Inject]
    private readonly ChangeHoveredArcControlPointLengthSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    private ArcEditorData _originalHeadArc;
    private ArcEditorData _originalTailArc;
    private ArcEditorData _changedHeadArc;
    private ArcEditorData _changedTailArc;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._originalHeadArc = this._beatmapLevelDataModel.GetArcByHead(this._signal.cellData);
      this._originalTailArc = this._beatmapLevelDataModel.GetArcByTail(this._signal.cellData);
      if (this._originalHeadArc == (ArcEditorData) null && this._originalTailArc == (ArcEditorData) null)
        return;
      if (this._originalHeadArc != (ArcEditorData) null)
        this._changedHeadArc = this.GetChangedArcData(this._originalHeadArc);
      if (this._originalTailArc != (ArcEditorData) null)
        this._changedTailArc = this.GetChangedArcData(this._originalTailArc);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(this._changedHeadArc);
        this._beatmapLevelDataModel.AddArc(this._originalHeadArc);
      }
      if (this._originalTailArc != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(this._changedTailArc);
        this._beatmapLevelDataModel.AddArc(this._originalTailArc);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      if (this._originalHeadArc != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(this._originalHeadArc);
        this._beatmapLevelDataModel.AddArc(this._changedHeadArc);
      }
      if (this._originalTailArc != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(this._originalTailArc);
        this._beatmapLevelDataModel.AddArc(this._changedTailArc);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      if (previousCommand is ChangeHoveredArcControlPointLengthCommand pointLengthCommand)
      {
        BeatmapEditorObjectId? id1 = this._originalHeadArc?.id;
        BeatmapEditorObjectId? id2 = pointLengthCommand._changedHeadArc?.id;
        if ((id1.HasValue == id2.HasValue ? (id1.HasValue ? (id1.GetValueOrDefault() == id2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
        {
          BeatmapEditorObjectId? id3 = this._originalTailArc?.id;
          id1 = pointLengthCommand._changedTailArc?.id;
          if (id3.HasValue != id1.HasValue)
            return false;
          return !id3.HasValue || id3.GetValueOrDefault() == id1.GetValueOrDefault();
        }
      }
      return false;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      ChangeHoveredArcControlPointLengthCommand pointLengthCommand = (ChangeHoveredArcControlPointLengthCommand) previousCommand;
      this._originalHeadArc = pointLengthCommand._originalHeadArc;
      this._originalTailArc = pointLengthCommand._originalTailArc;
    }

    private ArcEditorData GetChangedArcData(ArcEditorData original)
    {
      float num1 = original.controlPointLengthMultiplier;
      float num2 = original.tailControlPointLengthMultiplier;
      int num3;
      if (AudioTimeHelper.IsBeatSame(original.beat, this._signal.cellData.beat))
      {
        int column = original.column;
        Vector2Int cellPosition = this._signal.cellData.cellPosition;
        int x = cellPosition.x;
        if (column == x)
        {
          int row = original.row;
          cellPosition = this._signal.cellData.cellPosition;
          int y = cellPosition.y;
          num3 = row == y ? 1 : 0;
          goto label_4;
        }
      }
      num3 = 0;
label_4:
      if (num3 != 0)
        num1 = MathfExtra.Round(original.controlPointLengthMultiplier + Mathf.Sign(this._signal.delta) * 0.1f, 1);
      else
        num2 = MathfExtra.Round(original.tailControlPointLengthMultiplier + Mathf.Sign(this._signal.delta) * 0.1f, 1);
      return ArcEditorData.CopyWithModifications(original, controlPointLengthMultiplier: new float?(num1), tailControlPointLengthMultiplier: new float?(num2));
    }
  }
}
