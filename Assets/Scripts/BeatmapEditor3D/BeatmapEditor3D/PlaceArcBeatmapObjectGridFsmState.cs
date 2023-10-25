// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceArcBeatmapObjectGridFsmState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class PlaceArcBeatmapObjectGridFsmState : BeatmapObjectGridFsmState
  {
    private bool _placingEndNote;
    private ColorType _colorType;
    private bool _startNote;
    private Vector2Int _startCoords;
    private NoteCutDirection _startCutDirection;
    private float _startBeat;
    private Vector2Int _endCoords;
    private float _endBeat;

    public override void Enter()
    {
      base.Enter();
      this._endBeat = this.beatmapState.beat;
      this.ResetPlacing();
    }

    public override void Exit() => this.hoverView.HidePreview();

    public override void HandleGridCellPointerEnter(int column, int row)
    {
      if (this._placingEndNote)
        this._endCoords = new Vector2Int(column, row);
      this.beatmapObjectEditGridView.HighlightCell(column, row);
      this.UpdatePreview();
    }

    public override void HandleGridCellPointerExit(int column, int row) => this.beatmapObjectEditGridView.ResetHighlights();

    public override void HandleBeatmapLevelStateTimeUpdated()
    {
      this._endBeat = this.beatmapState.beat;
      this.UpdatePreview();
    }

    public override void HandleBeatmapTimeScaleChanged() => this.UpdatePreview();

    public override void HandleGridCellPointerUp(int column, int row)
    {
      Vector2Int coords = new Vector2Int(column, row);
      ColorType colorType;
      NoteCutDirection cutDirection;
      bool arcBeatmapObject = this.GetArcBeatmapObject(coords, this.beatmapState.beat, out colorType, out cutDirection);
      NoteCutDirection tailCutDirection = arcBeatmapObject ? cutDirection : this.beatmapObjectsState.noteCutDirection;
      if (!this._placingEndNote)
      {
        this._colorType = arcBeatmapObject ? colorType : ColorType.None;
        this._startNote = arcBeatmapObject;
        this._startBeat = this.beatmapState.beat;
        this._startCoords = coords;
        this._startCutDirection = tailCutDirection;
        this._placingEndNote = true;
      }
      else
      {
        if (!this._startNote && !arcBeatmapObject || this._startNote & arcBeatmapObject && this._colorType != colorType || AudioTimeHelper.IsBeatSame(this._startBeat, this.beatmapState.beat))
          return;
        this.signalBus.Fire<PlaceArcObjectSignal>(new PlaceArcObjectSignal(arcBeatmapObject ? colorType : this._colorType, this._startBeat, this._startCoords.x, this._startCoords.y, this._startCutDirection, this.beatmapState.beat, coords.x, coords.y, tailCutDirection));
        this.ResetPlacing();
      }
    }

    public override void HandleCancelAction(bool pressed) => this.ResetPlacing();

    private void UpdatePreview()
    {
      if (!this._placingEndNote)
        return;
      ColorType colorType;
      NoteCutDirection cutDirection;
      bool arcBeatmapObject = this.GetArcBeatmapObject(this._endCoords, this._endBeat, out colorType, out cutDirection);
      this.hoverView.SetSliderData(this._colorType == ColorType.None & arcBeatmapObject ? colorType : this._colorType, this._startBeat, this._startCoords, this._startCutDirection, this._endBeat, this._endCoords, arcBeatmapObject ? cutDirection : this.beatmapObjectsState.noteCutDirection);
      this.hoverView.ShowPreview(BeatmapObjectType.Arc, new RectInt(this._endCoords.x, this._endCoords.y, 1, 1));
    }

    private void ResetPlacing()
    {
      this._placingEndNote = false;
      this._colorType = ColorType.None;
      this.hoverView.HidePreview();
    }

    private bool GetArcBeatmapObject(
      Vector2Int coords,
      float beat,
      out ColorType colorType,
      out NoteCutDirection cutDirection)
    {
      colorType = ColorType.None;
      cutDirection = NoteCutDirection.Any;
      BeatmapObjectCellData cellData = new BeatmapObjectCellData(coords, beat);
      NoteEditorData note = this.beatmapLevelDataModel.GetNote(cellData);
      if (note != (NoteEditorData) null)
      {
        colorType = note.type;
        cutDirection = note.cutDirection;
        return true;
      }
      ChainEditorData chainByHead = this.beatmapLevelDataModel.GetChainByHead(cellData);
      if (chainByHead != (ChainEditorData) null)
      {
        colorType = chainByHead.colorType;
        cutDirection = chainByHead.cutDirection;
        return true;
      }
      ChainEditorData chainByTail = this.beatmapLevelDataModel.GetChainByTail(cellData);
      if (!(chainByTail != (ChainEditorData) null))
        return false;
      colorType = chainByTail.colorType;
      cutDirection = this.beatmapObjectsState.noteCutDirection;
      return true;
    }

    public class Factory : PlaceholderFactory<PlaceArcBeatmapObjectGridFsmState>
    {
    }
  }
}
