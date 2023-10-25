// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChangeHoveredArcControlPointCutDirectionAndLengthCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ChangeHoveredArcControlPointCutDirectionAndLengthCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeHoveredArcControlPointCutDirectionAndLengthSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    private ArcEditorData _originalArc;
    private ArcEditorData _changedArc;
    private ArcEditorData _connectedOriginalArc;
    private ArcEditorData _connectedChangedArc;
    private NoteEditorData _originalNote;
    private NoteEditorData _changedNote;
    private ChainEditorData _originalChain;
    private ChainEditorData _changedChain;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._originalArc = this._beatmapLevelDataModel.GetArcById(this._signal.id);
      if (this._originalArc == (ArcEditorData) null)
        return;
      NoteCutDirection cutDirection = this._originalArc.cutDirection;
      NoteCutDirection noteCutDirection = this._originalArc.tailCutDirection;
      float lengthMultiplier1 = this._originalArc.controlPointLengthMultiplier;
      float lengthMultiplier2 = this._originalArc.tailControlPointLengthMultiplier;
      int num;
      if (AudioTimeHelper.IsBeatSame(this._originalArc.beat, this._signal.cellData.beat))
      {
        int column = this._originalArc.column;
        Vector2Int cellPosition = this._signal.cellData.cellPosition;
        int x = cellPosition.x;
        if (column == x)
        {
          int row = this._originalArc.row;
          cellPosition = this._signal.cellData.cellPosition;
          int y = cellPosition.y;
          num = row == y ? 1 : 0;
          goto label_5;
        }
      }
      num = 0;
label_5:
      bool flag = num != 0;
      if (flag)
      {
        cutDirection = this._signal.cutDirection;
        lengthMultiplier1 = this._signal.controlPointLengthMultiplier;
        BeatmapObjectCellData cellData = new BeatmapObjectCellData(new Vector2Int(this._originalArc.column, this._originalArc.row), this._originalArc.beat);
        this._originalNote = this._beatmapLevelDataModel.GetNote(cellData);
        this._originalChain = this._beatmapLevelDataModel.GetChainByHead(cellData);
        this._connectedOriginalArc = this._beatmapLevelDataModel.GetArcByTail(cellData);
      }
      else
      {
        noteCutDirection = this._signal.cutDirection;
        lengthMultiplier2 = this._signal.controlPointLengthMultiplier;
        BeatmapObjectCellData cellData = new BeatmapObjectCellData(new Vector2Int(this._originalArc.tailColumn, this._originalArc.tailRow), this._originalArc.tailBeat);
        this._originalNote = this._beatmapLevelDataModel.GetNote(cellData);
        this._originalChain = this._beatmapLevelDataModel.GetChainByHead(cellData);
        this._connectedOriginalArc = this._beatmapLevelDataModel.GetArcByHead(cellData);
      }
      this._changedArc = ArcEditorData.CopyWithModifications(this._originalArc, cutDirection: new NoteCutDirection?(cutDirection), controlPointLengthMultiplier: new float?(lengthMultiplier1), tailCutDirection: new NoteCutDirection?(noteCutDirection), tailControlPointLengthMultiplier: new float?(lengthMultiplier2));
      if (this._connectedOriginalArc != (ArcEditorData) null)
        this._connectedChangedArc = ArcEditorData.CopyWithModifications(this._connectedOriginalArc, cutDirection: new NoteCutDirection?(flag ? this._connectedOriginalArc.cutDirection : this._signal.cutDirection), tailCutDirection: new NoteCutDirection?(flag ? this._signal.cutDirection : this._connectedOriginalArc.tailCutDirection));
      if (this._originalNote != (NoteEditorData) null)
        this._changedNote = NoteEditorData.CopyWithModifications(this._originalNote, cutDirection: new NoteCutDirection?(flag ? cutDirection : noteCutDirection));
      if (this._originalChain != (ChainEditorData) null)
        this._changedChain = ChainEditorData.CopyWithModifications(this._originalChain, cutDirection: new NoteCutDirection?(flag ? cutDirection : noteCutDirection));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      if (this._originalNote != (NoteEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveNote(this._changedNote);
        this._beatmapLevelDataModel.AddNote(this._originalNote);
      }
      if (this._originalChain != (ChainEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveChain(this._changedChain);
        this._beatmapLevelDataModel.AddChain(this._originalChain);
      }
      if (this._connectedOriginalArc != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(this._connectedChangedArc);
        this._beatmapLevelDataModel.AddArc(this._connectedOriginalArc);
      }
      this._beatmapLevelDataModel.RemoveArc(this._changedArc);
      this._beatmapLevelDataModel.AddArc(this._originalArc);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      if (this._originalNote != (NoteEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveNote(this._originalNote);
        this._beatmapLevelDataModel.AddNote(this._changedNote);
      }
      if (this._originalChain != (ChainEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveChain(this._originalChain);
        this._beatmapLevelDataModel.AddChain(this._changedChain);
      }
      if (this._connectedOriginalArc != (ArcEditorData) null)
      {
        this._beatmapLevelDataModel.RemoveArc(this._connectedOriginalArc);
        this._beatmapLevelDataModel.AddArc(this._connectedChangedArc);
      }
      this._beatmapLevelDataModel.RemoveArc(this._originalArc);
      this._beatmapLevelDataModel.AddArc(this._changedArc);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is ChangeHoveredArcControlPointCutDirectionAndLengthCommand andLengthCommand && this._originalArc.id == andLengthCommand._changedArc.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._originalArc = ((ChangeHoveredArcControlPointCutDirectionAndLengthCommand) previousCommand)._originalArc;
    }
  }
}
