// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceObstacleBeatmapObjectGridFsmState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class PlaceObstacleBeatmapObjectGridFsmState : BeatmapObjectGridFsmState
  {
    private bool _placingObstacle;
    private float _dragStartBeat;
    private float _dragEndBeat;
    private Vector2Int _startCellCoords;
    private Vector2Int _endCellCoords;

    private float obstacleStart => Mathf.Min(this._dragStartBeat, this._dragEndBeat);

    private float obstacleDuration => Mathf.Abs(this._dragEndBeat - this._dragStartBeat);

    public override void HandleGridCellPointerUp(int column, int row)
    {
      if (this._placingObstacle)
      {
        this._placingObstacle = false;
        if (Mathf.Approximately(this.beatmapObjectsState.obstacleDuration, 0.0f))
        {
          this.signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal("Cannot place obstacle with 0 length."));
        }
        else
        {
          RectInt obstacleRect = this.GetObstacleRect(this._startCellCoords, this._endCellCoords);
          this.signalBus.Fire<PlaceObstacleObjectSignal>(new PlaceObstacleObjectSignal(this.obstacleStart, obstacleRect.x, obstacleRect.y, obstacleRect.width, obstacleRect.height));
          this.beatmapObjectEditGridView.ResetHighlights();
          this.beatmapObjectEditGridView.HighlightCell(column, row);
        }
      }
      else
      {
        this._placingObstacle = true;
        this.SetDefaultCoordinates(column, row);
        this.UpdatePreview();
      }
    }

    public override void HandleGridCellPointerEnter(int column, int row)
    {
      if (!this._placingObstacle)
      {
        this.SetDefaultCoordinates(column, row);
        this.UpdatePreview();
      }
      else
      {
        this._endCellCoords = new Vector2Int(column, row);
        this.UpdatePreview();
      }
    }

    public override void HandleGridCellPointerExit(int column, int row)
    {
      if (this._placingObstacle)
        return;
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
    }

    public override void HandleBeatmapLevelStateTimeUpdated()
    {
      if (!this._placingObstacle)
        return;
      this._dragEndBeat = this.beatmapState.beat;
      this.UpdatePreview();
      this.signalBus.Fire<ChangeObstacleDurationSignal>(new ChangeObstacleDurationSignal(this.obstacleDuration));
    }

    public override void HandleCancelAction(bool pressed)
    {
      this._placingObstacle = false;
      this.hoverView.HidePreview();
      this.beatmapObjectEditGridView.ResetHighlights();
      this.beatmapObjectEditGridView.HighlightCell(this._endCellCoords.x, this._endCellCoords.y);
    }

    private void UpdatePreview()
    {
      RectInt obstacleRect = this.GetObstacleRect(this._startCellCoords, this._endCellCoords);
      this.beatmapObjectEditGridView.UpdateHighlightedArea(obstacleRect.position, obstacleRect.position + obstacleRect.size - Vector2Int.one);
      this.hoverView.SetObstacleData(this.obstacleStart, this.obstacleDuration);
      this.hoverView.ShowPreview(BeatmapObjectType.Obstacle, obstacleRect);
    }

    private void SetDefaultCoordinates(int column, int row)
    {
      this._startCellCoords = new Vector2Int(column, row);
      this._endCellCoords = this._startCellCoords;
      this._dragStartBeat = this.beatmapState.beat;
      this._dragEndBeat = this._dragStartBeat + this.beatmapObjectsState.obstacleDuration;
    }

    private RectInt GetObstacleRect(Vector2Int startCellCoords, Vector2Int endCellCoords)
    {
      if (startCellCoords.y != 2 || endCellCoords.y != 2)
        return new RectInt(startCellCoords.x >= endCellCoords.x ? endCellCoords.x : Math.Max(startCellCoords.x, endCellCoords.x - 1), 0, Mathf.Clamp(Mathf.Abs(startCellCoords.x - endCellCoords.x), 0, 1) + 1, 3);
      return new RectInt(Math.Min(startCellCoords.x, endCellCoords.x), 2, Mathf.Abs(startCellCoords.x - endCellCoords.x) + 1, 1);
    }

    public class Factory : PlaceholderFactory<PlaceObstacleBeatmapObjectGridFsmState>
    {
    }
  }
}
