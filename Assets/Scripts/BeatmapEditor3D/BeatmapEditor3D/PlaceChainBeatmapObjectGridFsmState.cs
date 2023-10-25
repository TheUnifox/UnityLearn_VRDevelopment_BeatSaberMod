// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceChainBeatmapObjectGridFsmState
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
  public class PlaceChainBeatmapObjectGridFsmState : BeatmapObjectGridFsmState
  {
    [Inject]
    private readonly PlaceNoteBeatmapObjectGridFsmState.Factory _placeNoteBeatmapObjectGridFsmStateFactory;
    private readonly ColorType _colorType;
    private readonly NoteCutDirection _cutDirection;
    private readonly Vector2Int _startCellCoords;
    private readonly float _startBeat;
    private Vector2Int _endCellCoords;
    private float _endBeat;

    public PlaceChainBeatmapObjectGridFsmState(
      NoteEditorData note,
      int endColumn,
      int endRow,
      float endBeat)
    {
      this._colorType = note.type;
      this._cutDirection = note.cutDirection;
      this._startCellCoords = new Vector2Int(note.column, note.row);
      this._startBeat = note.beat;
      this._endCellCoords = new Vector2Int(endColumn, endRow);
      this._endBeat = endBeat;
    }

    public override void Enter()
    {
      this.beatmapObjectEditGridView.HighlightCell(this._endCellCoords.x, this._endCellCoords.y);
      this.UpdatePreview();
    }

    public override void HandleGridCellPointerUp(int column, int row)
    {
      if (this._startCellCoords.x == this._endCellCoords.x && this._startCellCoords.y == this._endCellCoords.y)
        this.signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal("Cannot place chain with same start and end cell"));
      else
        this.signalBus.Fire<PlaceChainObjectSignal>(new PlaceChainObjectSignal(this._colorType, this._startBeat, this._startCellCoords.x, this._startCellCoords.y, this._cutDirection, this._endBeat, this._endCellCoords.x, this._endCellCoords.y, this.CalculateSliceCount(this._startCellCoords, this._startBeat, this._endCellCoords, this._endBeat), this.beatmapObjectsState.sliderSquishAmount));
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
      this.nextState = (IBeatmapObjectGridFsmState) this._placeNoteBeatmapObjectGridFsmStateFactory.Create();
    }

    public override void HandleGridCellPointerEnter(int column, int row)
    {
      this.beatmapObjectEditGridView.HighlightCell(column, row);
      this._endCellCoords = new Vector2Int(column, row);
      this.UpdatePreview();
    }

    public override void HandleGridCellPointerExit(int column, int row) => this.beatmapObjectEditGridView.ResetHighlights();

    public override void HandleBeatmapLevelStateTimeUpdated()
    {
      this._endBeat = this.beatmapState.beat;
      this.UpdatePreview();
    }

    public override void HandleBeatmapTimeScaleChanged() => this.UpdatePreview();

    public override void HandleCancelAction(bool pressed)
    {
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
      this.nextState = (IBeatmapObjectGridFsmState) this._placeNoteBeatmapObjectGridFsmStateFactory.Create();
    }

    private void UpdatePreview()
    {
      this.hoverView.SetChainData(this._colorType, this._startBeat, this._startCellCoords, this._cutDirection, this.CalculateSliceCount(this._startCellCoords, this._startBeat, this._endCellCoords, this._endBeat), this._endBeat, this._endCellCoords);
      this.hoverView.ShowPreview(BeatmapObjectType.Chain, new RectInt(this._endCellCoords, Vector2Int.one));
    }

    private int CalculateSliceCount(
      Vector2Int startCoords,
      float startBeat,
      Vector2Int endCoords,
      float endBeat)
    {
      return 2 + Mathf.CeilToInt(Vector3.Distance(new Vector3((float) startCoords.x, (float) startCoords.y, startBeat), new Vector3((float) endCoords.x, (float) endCoords.y, endBeat)) * 3f);
    }

    public class Factory : 
      PlaceholderFactory<NoteEditorData, int, int, float, PlaceChainBeatmapObjectGridFsmState>
    {
    }
  }
}
