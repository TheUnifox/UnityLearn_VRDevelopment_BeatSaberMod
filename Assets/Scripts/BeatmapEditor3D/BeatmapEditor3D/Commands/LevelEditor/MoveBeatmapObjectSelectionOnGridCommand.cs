// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.LevelEditor.MoveBeatmapObjectSelectionOnGridCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands.LevelEditor
{
  public class MoveBeatmapObjectSelectionOnGridCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly MoveBeatmapObjectSelectionOnGridSignal _onGridSignal;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<NoteEditorData> _originalNotes;
    private List<ObstacleEditorData> _originalObstacles;
    private List<WaypointEditorData> _originalWaypoints;
    private List<ChainEditorData> _originalChains;
    private List<ArcEditorData> _originalArcs;
    private List<NoteEditorData> _movedNotes;
    private List<ObstacleEditorData> _movedObstacles;
    private List<WaypointEditorData> _movedWaypoints;
    private List<ChainEditorData> _movedChains;
    private List<ArcEditorData> _movedArcs;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._beatmapObjectsSelectionState.notes.Count == 0 && this._beatmapObjectsSelectionState.obstacles.Count == 0 && this._beatmapObjectsSelectionState.waypoints.Count == 0 && this._beatmapObjectsSelectionState.chains.Count == 0 && this._beatmapObjectsSelectionState.arcs.Count == 0)
        return;
      this._originalNotes = this._beatmapObjectsSelectionState.notes.Select<BeatmapEditorObjectId, NoteEditorData>((Func<BeatmapEditorObjectId, NoteEditorData>) (noteId => this._beatmapLevelDataModel.GetNoteById(noteId))).OrderBy<NoteEditorData, float>((Func<NoteEditorData, float>) (n => n.beat)).ThenBy<NoteEditorData, float>((Func<NoteEditorData, float>) (n => MoveBeatmapObjectSelectionOnGridCommand.GetBeatmapObjectOrderByValue(this._onGridSignal.moveDirection, (BaseBeatmapObjectEditorData) n))).ToList<NoteEditorData>();
      this._originalObstacles = this._beatmapObjectsSelectionState.obstacles.Select<BeatmapEditorObjectId, ObstacleEditorData>((Func<BeatmapEditorObjectId, ObstacleEditorData>) (obstacleId => this._beatmapLevelDataModel.GetObstacleById(obstacleId))).OrderBy<ObstacleEditorData, float>((Func<ObstacleEditorData, float>) (o => o.beat)).ThenBy<ObstacleEditorData, float>((Func<ObstacleEditorData, float>) (o => MoveBeatmapObjectSelectionOnGridCommand.GetBeatmapObjectOrderByValue(this._onGridSignal.moveDirection, (BaseBeatmapObjectEditorData) o))).ToList<ObstacleEditorData>();
      this._originalWaypoints = this._beatmapObjectsSelectionState.waypoints.Select<BeatmapEditorObjectId, WaypointEditorData>((Func<BeatmapEditorObjectId, WaypointEditorData>) (waypointId => this._beatmapLevelDataModel.GetWaypointById(waypointId))).OrderBy<WaypointEditorData, float>((Func<WaypointEditorData, float>) (w => w.beat)).ThenBy<WaypointEditorData, float>((Func<WaypointEditorData, float>) (w => MoveBeatmapObjectSelectionOnGridCommand.GetBeatmapObjectOrderByValue(this._onGridSignal.moveDirection, (BaseBeatmapObjectEditorData) w))).ToList<WaypointEditorData>();
      this._originalChains = this._beatmapObjectsSelectionState.chains.Select<BeatmapEditorObjectId, ChainEditorData>((Func<BeatmapEditorObjectId, ChainEditorData>) (chainId => this._beatmapLevelDataModel.GetChainById(chainId))).OrderBy<ChainEditorData, float>((Func<ChainEditorData, float>) (c => c.beat)).ThenBy<ChainEditorData, float>((Func<ChainEditorData, float>) (c => MoveBeatmapObjectSelectionOnGridCommand.GetBeatmapObjectOrderByValue(this._onGridSignal.moveDirection, (BaseBeatmapObjectEditorData) c))).ToList<ChainEditorData>();
      this._originalArcs = this._beatmapObjectsSelectionState.arcs.Select<BeatmapEditorObjectId, ArcEditorData>((Func<BeatmapEditorObjectId, ArcEditorData>) (arcId => this._beatmapLevelDataModel.GetArcById(arcId))).OrderBy<ArcEditorData, float>((Func<ArcEditorData, float>) (a => a.beat)).ThenBy<ArcEditorData, float>((Func<ArcEditorData, float>) (a => MoveBeatmapObjectSelectionOnGridCommand.GetBeatmapObjectOrderByValue(this._onGridSignal.moveDirection, (BaseBeatmapObjectEditorData) a))).ToList<ArcEditorData>();
      bool anyItemMoved = false;
      this._movedNotes = this.GetMovedNotes(ref anyItemMoved);
      this._movedObstacles = this.GetMovedObstacles(ref anyItemMoved);
      this._movedWaypoints = this.GetMovedWaypoints(ref anyItemMoved);
      this._movedChains = this.GetMovedChains(ref anyItemMoved);
      this._movedArcs = this.GetMovedArcs(ref anyItemMoved);
      if (!anyItemMoved)
        return;
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this.SwapBeatmapObjects(this._movedNotes, this._movedObstacles, this._movedWaypoints, this._movedChains, this._movedArcs, this._originalNotes, this._originalObstacles, this._originalWaypoints, this._originalChains, this._originalArcs);
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this.SwapBeatmapObjects(this._originalNotes, this._originalObstacles, this._originalWaypoints, this._originalChains, this._originalArcs, this._movedNotes, this._movedObstacles, this._movedWaypoints, this._movedChains, this._movedArcs);
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private void SwapBeatmapObjects(
      List<NoteEditorData> toRemoveNotes,
      List<ObstacleEditorData> toRemoveObstacles,
      List<WaypointEditorData> toRemoveWaypoints,
      List<ChainEditorData> toRemoveChains,
      List<ArcEditorData> toRemoveArcs,
      List<NoteEditorData> toAddNotes,
      List<ObstacleEditorData> toAddObstacles,
      List<WaypointEditorData> toAddWaypoints,
      List<ChainEditorData> toAddChains,
      List<ArcEditorData> toAddArcs)
    {
      this._beatmapObjectsSelectionState.Clear();
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) toRemoveArcs);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) toRemoveChains);
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) toRemoveNotes);
      this._beatmapLevelDataModel.RemoveObstacles((IEnumerable<ObstacleEditorData>) toRemoveObstacles);
      this._beatmapLevelDataModel.RemoveWaypoints((IEnumerable<WaypointEditorData>) toRemoveWaypoints);
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) toAddNotes);
      this._beatmapLevelDataModel.AddObstacles((IEnumerable<ObstacleEditorData>) toAddObstacles);
      this._beatmapLevelDataModel.AddWaypoints((IEnumerable<WaypointEditorData>) toAddWaypoints);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) toAddChains);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) toAddArcs);
      this._beatmapObjectsSelectionState.AddNotes(toAddNotes.Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id)));
      this._beatmapObjectsSelectionState.AddObstacles(toAddObstacles.Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (o => o.id)));
      this._beatmapObjectsSelectionState.AddWaypoints(toAddWaypoints.Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (w => w.id)));
      this._beatmapObjectsSelectionState.AddChains(toAddChains.Select<ChainEditorData, BeatmapEditorObjectId>((Func<ChainEditorData, BeatmapEditorObjectId>) (c => c.id)));
      this._beatmapObjectsSelectionState.AddArcs(toAddArcs.Select<ArcEditorData, BeatmapEditorObjectId>((Func<ArcEditorData, BeatmapEditorObjectId>) (a => a.id)));
    }

    private List<NoteEditorData> GetMovedNotes(ref bool anyItemMoved)
    {
      List<NoteEditorData> source = new List<NoteEditorData>(this._beatmapObjectsSelectionState.notes.Count);
      foreach (NoteEditorData originalNote in this._originalNotes)
      {
        (int column, int row) = MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalNote.column, originalNote.row);
        if (originalNote.row == row && originalNote.column == column)
        {
          source.Add(originalNote);
        }
        else
        {
          NoteEditorData noteEditorData1 = NoteEditorData.CopyWithModifications(originalNote, column: new int?(column), row: new int?(row));
          NoteEditorData noteEditorData2 = this.GetNote(originalNote.beat, column, row);
          int num = !(noteEditorData2 != (NoteEditorData) null) ? 0 : (this._originalNotes.Any<NoteEditorData>(new Func<NoteEditorData, bool>(((BaseBeatmapObjectEditorData) noteEditorData2).PositionEquals)) ? 1 : 0);
          bool flag = source.Any<NoteEditorData>(new Func<NoteEditorData, bool>(((BaseBeatmapObjectEditorData) noteEditorData1).PositionEquals));
          if (num != 0 && !flag)
            noteEditorData2 = (NoteEditorData) null;
          anyItemMoved |= noteEditorData2 == (NoteEditorData) null;
          source.Add(noteEditorData2 == (NoteEditorData) null ? noteEditorData1 : originalNote);
        }
      }
      return source;
    }

    private List<ObstacleEditorData> GetMovedObstacles(ref bool anyItemMoved)
    {
      List<ObstacleEditorData> source = new List<ObstacleEditorData>(this._beatmapObjectsSelectionState.obstacles.Count);
      foreach (ObstacleEditorData originalObstacle in this._originalObstacles)
      {
        int column = Mathf.Clamp(MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalObstacle.column, originalObstacle.row).column, 0, 4 - originalObstacle.width);
        if (originalObstacle.column == column)
        {
          source.Add(originalObstacle);
        }
        else
        {
          ObstacleEditorData obstacleEditorData1 = ObstacleEditorData.CopyWithModifications(originalObstacle, column: new int?(column));
          ObstacleEditorData obstacleEditorData2 = this.GetObstacle(originalObstacle.beat, column, originalObstacle.row);
          int num = !(obstacleEditorData2 != (ObstacleEditorData) null) ? 0 : (this._originalObstacles.Any<ObstacleEditorData>(new Func<ObstacleEditorData, bool>(((BaseBeatmapObjectEditorData) obstacleEditorData2).PositionEquals)) ? 1 : 0);
          bool flag = source.Any<ObstacleEditorData>(new Func<ObstacleEditorData, bool>(((BaseBeatmapObjectEditorData) obstacleEditorData1).PositionEquals));
          if (num != 0 && !flag)
            obstacleEditorData2 = (ObstacleEditorData) null;
          anyItemMoved |= obstacleEditorData2 == (ObstacleEditorData) null;
          source.Add(obstacleEditorData2 == (ObstacleEditorData) null ? obstacleEditorData1 : originalObstacle);
        }
      }
      return source;
    }

    private List<WaypointEditorData> GetMovedWaypoints(ref bool anyItemMoved)
    {
      List<WaypointEditorData> source = new List<WaypointEditorData>(this._beatmapObjectsSelectionState.waypoints.Count);
      foreach (WaypointEditorData originalWaypoint in this._originalWaypoints)
      {
        (int column, int row) = MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalWaypoint.column, originalWaypoint.row);
        if (originalWaypoint.column == column && originalWaypoint.row == row)
        {
          source.Add(originalWaypoint);
        }
        else
        {
          WaypointEditorData newWithId = WaypointEditorData.CreateNewWithId(originalWaypoint.id, originalWaypoint.beat, column, row, originalWaypoint.offsetDirection);
          WaypointEditorData waypointEditorData = this.GetWaypoint(originalWaypoint.beat, column, row);
          int num = !(waypointEditorData != (WaypointEditorData) null) ? 0 : (this._originalWaypoints.Any<WaypointEditorData>(new Func<WaypointEditorData, bool>(((BaseBeatmapObjectEditorData) waypointEditorData).PositionEquals)) ? 1 : 0);
          bool flag = source.Any<WaypointEditorData>(new Func<WaypointEditorData, bool>(((BaseBeatmapObjectEditorData) newWithId).PositionEquals));
          if (num != 0 && !flag)
            waypointEditorData = (WaypointEditorData) null;
          anyItemMoved |= waypointEditorData == (WaypointEditorData) null;
          source.Add(waypointEditorData == (WaypointEditorData) null ? newWithId : originalWaypoint);
        }
      }
      return source;
    }

    private List<ChainEditorData> GetMovedChains(ref bool anyItemMoved)
    {
      List<ChainEditorData> source = new List<ChainEditorData>(this._beatmapObjectsSelectionState.waypoints.Count);
      foreach (ChainEditorData originalChain in this._originalChains)
      {
        (int column1, int row1) = MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalChain.column, originalChain.row);
        (int column2, int row2) = MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalChain.tailColumn, originalChain.tailRow);
        if (originalChain.column == column1 && originalChain.row == row1 && originalChain.tailColumn == column2 && originalChain.tailRow == row2)
        {
          source.Add(originalChain);
        }
        else
        {
          ChainEditorData chainEditorData1 = ChainEditorData.CopyWithModifications(originalChain, column: new int?(column1), row: new int?(row1), tailColumn: new int?(column2), tailRow: new int?(row2));
          ChainEditorData chainEditorData2 = this.GetChain(originalChain.beat, column1, row1);
          int num = !(chainEditorData2 != (ChainEditorData) null) ? 0 : (this._originalChains.Any<ChainEditorData>(new Func<ChainEditorData, bool>(chainEditorData2.PositionEquals)) ? 1 : 0);
          bool flag = source.Any<ChainEditorData>(new Func<ChainEditorData, bool>(chainEditorData1.PositionEquals));
          if (num != 0 && !flag)
            chainEditorData2 = (ChainEditorData) null;
          anyItemMoved |= chainEditorData2 == (ChainEditorData) null;
          source.Add(chainEditorData2 == (ChainEditorData) null ? chainEditorData1 : originalChain);
        }
      }
      return source;
    }

    private List<ArcEditorData> GetMovedArcs(ref bool anyItemMoved)
    {
      List<ArcEditorData> source = new List<ArcEditorData>(this._beatmapObjectsSelectionState.arcs.Count);
      foreach (ArcEditorData originalArc in this._originalArcs)
      {
        (int column1, int row1) = MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalArc.column, originalArc.row);
        (int column2, int row2) = MoveBeatmapObjectSelectionOnGridCommand.GetNewPosition(this._onGridSignal.moveDirection, originalArc.tailColumn, originalArc.tailRow);
        if (originalArc.column == column1 && originalArc.row == row1 && originalArc.tailColumn == column2 && originalArc.tailRow == row2)
        {
          source.Add(originalArc);
        }
        else
        {
          ArcEditorData arcEditorData1 = ArcEditorData.CopyWithModifications(originalArc, column: new int?(column1), row: new int?(row1), tailColumn: new int?(column2), tailRow: new int?(row2));
          ArcEditorData arcEditorData2 = this.GetArc(originalArc.beat, column1, row1);
          int num = !(arcEditorData2 != (ArcEditorData) null) ? 0 : (this._originalArcs.Any<ArcEditorData>(new Func<ArcEditorData, bool>(arcEditorData2.PositionEquals)) ? 1 : 0);
          bool flag = source.Any<ArcEditorData>(new Func<ArcEditorData, bool>(arcEditorData1.PositionEquals));
          if (num != 0 && !flag)
            arcEditorData2 = (ArcEditorData) null;
          anyItemMoved |= arcEditorData2 == (ArcEditorData) null;
          source.Add(arcEditorData2 == (ArcEditorData) null ? arcEditorData1 : originalArc);
        }
      }
      return source;
    }

    private NoteEditorData GetNote(float time, int column, int row) => this._beatmapLevelDataModel.GetNote(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private ObstacleEditorData GetObstacle(float time, int column, int row) => this._beatmapLevelDataModel.GetObstacle(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private WaypointEditorData GetWaypoint(float time, int column, int row) => this._beatmapLevelDataModel.GetWaypoint(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private ChainEditorData GetChain(float time, int column, int row) => this._beatmapLevelDataModel.GetChainByHead(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private ArcEditorData GetArc(float time, int column, int row) => this._beatmapLevelDataModel.GetArcByHead(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private static float GetBeatmapObjectOrderByValue(
      MoveBeatmapObjectSelectionOnGridSignal.MoveDirection moveDirection,
      BaseBeatmapObjectEditorData data)
    {
      return moveDirection == MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Up || moveDirection == MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Down ? (float) (data.row * (moveDirection == MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Down ? 1 : -1)) : (float) (data.column * (moveDirection == MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Left ? 1 : -1));
    }

    private static (int column, int row) GetNewPosition(
      MoveBeatmapObjectSelectionOnGridSignal.MoveDirection moveDirection,
      int column,
      int row)
    {
      switch (moveDirection)
      {
        case MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Up:
          row = Mathf.Clamp(row + 1, 0, 2);
          break;
        case MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Right:
          column = Mathf.Clamp(column + 1, 0, 3);
          break;
        case MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Down:
          row = Mathf.Clamp(row - 1, 0, 2);
          break;
        case MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Left:
          column = Mathf.Clamp(column - 1, 0, 3);
          break;
      }
      return (column, row);
    }
  }
}
