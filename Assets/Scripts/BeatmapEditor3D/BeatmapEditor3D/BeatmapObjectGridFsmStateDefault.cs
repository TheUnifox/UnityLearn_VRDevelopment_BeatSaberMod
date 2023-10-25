// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectGridFsmStateDefault
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectGridFsmStateDefault : BeatmapObjectGridFsmState
  {
    [Inject]
    private readonly PlaceNoteBeatmapObjectGridFsmState.Factory _beatmapObjectGridFsmStatePlaceNoteFactory;
    [Inject]
    private readonly PlaceObstacleBeatmapObjectGridFsmState.Factory _beatmapObjectGridFsmStatePlaceObstacleFactory;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;

    public override void Exit()
    {
    }

    public override void HandleGridCellPointerDown(int column, int row)
    {
      if (this._beatmapState.interactionMode != InteractionMode.Place || this._beatmapLevelDataModel.AnyBeatmapObjectExists(this.beatmapState.beat, column, row, this.beatmapState.beat, column, row))
        return;
      switch (this._beatmapObjectsState.beatmapObjectType)
      {
        case BeatmapObjectType.Note:
        case BeatmapObjectType.Bomb:
          this.nextState = (IBeatmapObjectGridFsmState) this._beatmapObjectGridFsmStatePlaceNoteFactory.Create();
          break;
      }
    }

    public override void HandleGridCellPointerUp(int column, int row)
    {
      if (this._beatmapState.interactionMode != InteractionMode.Place)
        return;
      if (this._beatmapObjectsState.beatmapObjectType == BeatmapObjectType.Obstacle)
        this.nextState = (IBeatmapObjectGridFsmState) this._beatmapObjectGridFsmStatePlaceObstacleFactory.Create();
    }

    public override void HandleBeatmapObjectModeChanged(
      InteractionModeChangedSignal interactionModeChangedSignal)
    {
      base.HandleBeatmapObjectModeChanged(interactionModeChangedSignal);
    }

    public override void HandleLevelEditorZenModeUpdated(LevelEditorStateZenModeUpdatedSignal signal)
    {
      if (!signal.zenModeIsOn)
        return;
      this.nextState = (IBeatmapObjectGridFsmState) this.beatmapObjectGridFsmStateHiddenFactory.Create();
    }

    public override void HandleBeatmapLevelStateTimeUpdated()
    {
      if (this._beatmapObjectsState.hoveredGridCellData == null)
        return;
      this.UpdateHoveredCell();
    }

    public override void HandleBeatmapTimeScaleChanged()
    {
      if (this._beatmapObjectsState.hoveredGridCellData == null)
        return;
      this.UpdateHoveredCell();
    }

    public override void HandleGridCellPointerEnter(int column, int row)
    {
      if (this.beatmapObjectsSelectionState.draggedBeatmapObjectId != BeatmapEditorObjectId.invalid)
        this.signalBus.Fire<MoveHoveredBeatmapObjectOnGridSignal>(new MoveHoveredBeatmapObjectOnGridSignal(new Vector2Int(column, row)));
      this.beatmapObjectEditGridView.HighlightCell(column, row);
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(new BeatmapObjectCellData(new Vector2Int(column, row), this.beatmapState.beat), ChangeHoverSignal.HoverOrigin.Grid));
      if (this._beatmapObjectsState.beatmapObjectType == BeatmapObjectType.Arc)
        return;
      if (this._beatmapObjectsState.beatmapObjectType == BeatmapObjectType.Obstacle)
        this.hoverView.SetObstacleData(this.beatmapState.beat, this._beatmapObjectsState.obstacleDuration);
      this.hoverView.ShowPreview(BeatmapObjectType.Note, new RectInt(column, row, 1, 1));
    }

    public override void HandleGridCellPointerExit(int column, int row)
    {
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal((BeatmapObjectCellData) null, ChangeHoverSignal.HoverOrigin.Grid));
    }

    private void UpdateHoveredCell() => this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(new BeatmapObjectCellData(this._beatmapObjectsState.hoveredGridCellData.cellPosition, this.beatmapState.beat), ChangeHoverSignal.HoverOrigin.Grid));

    public class Factory : PlaceholderFactory<BeatmapObjectGridFsmStateDefault>
    {
    }
  }
}
