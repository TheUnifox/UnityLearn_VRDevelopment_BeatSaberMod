// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceNoteBeatmapObjectGridFsmState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class PlaceNoteBeatmapObjectGridFsmState : BeatmapObjectGridFsmState
  {
    [Inject]
    private readonly PlaceChainBeatmapObjectGridFsmState.Factory _placeChainBeatmapObjectGridFsmStateFactory;
    private const float kMinDragDelta = 50f;
    private bool _placingNote;
    private bool _placingChain;
    private NoteEditorData _chainHeadNote;
    private bool _dragging;
    private bool _hoveringOverAnyGrid;
    private Vector3 _mouseStartDragPosition;
    private NoteCutDirection _noteCutDirection;
    private float _beat;
    private int _column;
    private int _row;
    private int _hoverColumn;
    private int _hoverRow;

    public override void Enter()
    {
      base.Enter();
      this._placingNote = false;
      this._placingChain = false;
      this._dragging = false;
      this._hoveringOverAnyGrid = false;
    }

    public override IBeatmapObjectGridFsmState Update()
    {
      Vector3 direction = Input.mousePosition - this._mouseStartDragPosition;
      if (this._dragging && (double) direction.magnitude > 50.0)
      {
        NoteCutDirection noteCutDirection = NoteCutDirectionExtensions.NoteCutDirectionFromDirection(direction);
        if (this._noteCutDirection != noteCutDirection)
        {
          this._noteCutDirection = noteCutDirection;
          this.signalBus.Fire<ChangeNoteCutDirectionSignal>(new ChangeNoteCutDirectionSignal(noteCutDirection));
        }
      }
      return base.Update();
    }

    public override void HandleMousePointerDown()
    {
      this._dragging = true;
      this._mouseStartDragPosition = Input.mousePosition;
    }

    public override void HandleMousePointerUp()
    {
      this._dragging = false;
      this._placingChain = false;
      if (!this._placingNote)
        return;
      this._placingNote = false;
      this.signalBus.Fire<PlaceNoteObjectSignal>(new PlaceNoteObjectSignal(this.beatmapState.beat, this._column, this._row));
    }

    public override void HandleGridCellPointerDown(int column, int row)
    {
      this._column = column;
      this._row = row;
      this._beat = this.beatmapState.beat;
      this._chainHeadNote = this.beatmapLevelDataModel.GetNote(new BeatmapObjectCellData(new Vector2Int(column, row), this.beatmapState.beat));
      this._placingChain = this._chainHeadNote != (NoteEditorData) null && this.beatmapState.interactionMode == InteractionMode.Place;
      this._placingNote = this._chainHeadNote == (NoteEditorData) null && this.beatmapState.interactionMode == InteractionMode.Place;
    }

    public override void HandleGridCellPointerEnter(int column, int row)
    {
      this._hoveringOverAnyGrid = true;
      this._hoverColumn = column;
      this._hoverRow = row;
      if (this._placingNote)
        return;
      if (this._placingChain)
      {
        this._placingChain = false;
        this.nextState = (IBeatmapObjectGridFsmState) this._placeChainBeatmapObjectGridFsmStateFactory.Create(this._chainHeadNote, column, row, this.beatmapState.beat);
      }
      else
        this.HighlightCellAndUpdateHover(column, row);
    }

    public override void HandleGridCellPointerExit(int column, int row)
    {
      this._hoveringOverAnyGrid = false;
      if (this._placingNote)
        return;
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal((BeatmapObjectCellData) null, ChangeHoverSignal.HoverOrigin.Grid));
    }

    public override void HandleBeatmapLevelStateTimeUpdated()
    {
      if (!this._placingChain)
        return;
      this.nextState = (IBeatmapObjectGridFsmState) this._placeChainBeatmapObjectGridFsmStateFactory.Create(this._chainHeadNote, this._hoverColumn, this._hoverRow, this.beatmapState.beat);
    }

    public override void HandleCancelAction(bool pressed)
    {
      this._placingNote = false;
      this._dragging = false;
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
      if (!this._hoveringOverAnyGrid)
        return;
      this.HighlightCellAndUpdateHover(this._hoverColumn, this._hoverRow);
    }

    private void HighlightCellAndUpdateHover(int column, int row)
    {
      this.beatmapObjectEditGridView.HighlightCell(column, row);
      this.hoverView.ShowPreview(this.beatmapObjectsState.beatmapObjectType, new RectInt(column, row, 1, 1));
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(new BeatmapObjectCellData(new Vector2Int(column, row), this.beatmapState.beat), ChangeHoverSignal.HoverOrigin.Grid));
    }

    public class Factory : PlaceholderFactory<PlaceNoteBeatmapObjectGridFsmState>
    {
    }
  }
}
