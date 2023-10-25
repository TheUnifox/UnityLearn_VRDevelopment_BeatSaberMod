// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.LevelEditor.MoveBeatmapObjectSelectionInTimeCommand
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
  public class MoveBeatmapObjectSelectionInTimeCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly MoveBeatmapObjectSelectionInTimeSignal _signal;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private IEnumerable<NoteEditorData> _originalNotes;
    private IEnumerable<ObstacleEditorData> _originalObstacles;
    private IEnumerable<WaypointEditorData> _originalWaypoints;
    private IEnumerable<ChainEditorData> _originalChains;
    private IEnumerable<ArcEditorData> _originalArcs;
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
      this._originalNotes = (IEnumerable<NoteEditorData>) this._beatmapObjectsSelectionState.notes.Select<BeatmapEditorObjectId, NoteEditorData>((Func<BeatmapEditorObjectId, NoteEditorData>) (noteId => this._beatmapLevelDataModel.GetNoteById(noteId))).OrderBy<NoteEditorData, float>((Func<NoteEditorData, float>) (n => n.beat)).ToList<NoteEditorData>();
      this._originalObstacles = (IEnumerable<ObstacleEditorData>) this._beatmapObjectsSelectionState.obstacles.Select<BeatmapEditorObjectId, ObstacleEditorData>((Func<BeatmapEditorObjectId, ObstacleEditorData>) (obstacleId => this._beatmapLevelDataModel.GetObstacleById(obstacleId))).OrderBy<ObstacleEditorData, float>((Func<ObstacleEditorData, float>) (o => o.beat)).ToList<ObstacleEditorData>();
      this._originalWaypoints = (IEnumerable<WaypointEditorData>) this._beatmapObjectsSelectionState.waypoints.Select<BeatmapEditorObjectId, WaypointEditorData>((Func<BeatmapEditorObjectId, WaypointEditorData>) (waypointId => this._beatmapLevelDataModel.GetWaypointById(waypointId))).OrderBy<WaypointEditorData, float>((Func<WaypointEditorData, float>) (w => w.beat)).ToList<WaypointEditorData>();
      this._originalChains = (IEnumerable<ChainEditorData>) this._beatmapObjectsSelectionState.chains.Select<BeatmapEditorObjectId, ChainEditorData>((Func<BeatmapEditorObjectId, ChainEditorData>) (chainId => this._beatmapLevelDataModel.GetChainById(chainId))).OrderBy<ChainEditorData, float>((Func<ChainEditorData, float>) (c => c.beat)).ToList<ChainEditorData>();
      this._originalArcs = (IEnumerable<ArcEditorData>) this._beatmapObjectsSelectionState.arcs.Select<BeatmapEditorObjectId, ArcEditorData>((Func<BeatmapEditorObjectId, ArcEditorData>) (arcId => this._beatmapLevelDataModel.GetArcById(arcId))).OrderBy<ArcEditorData, float>((Func<ArcEditorData, float>) (a => a.beat)).ToList<ArcEditorData>();
      bool anyItemMoved = false;
      this._movedNotes = this.GetMovedNotes(ref anyItemMoved).Values.ToList<NoteEditorData>();
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
      this._beatmapObjectsSelectionState.Clear();
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) this._movedNotes);
      this._beatmapLevelDataModel.RemoveObstacles((IEnumerable<ObstacleEditorData>) this._movedObstacles);
      this._beatmapLevelDataModel.RemoveWaypoints((IEnumerable<WaypointEditorData>) this._movedWaypoints);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) this._movedChains);
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) this._movedArcs);
      this._beatmapLevelDataModel.AddNotes(this._originalNotes);
      this._beatmapLevelDataModel.AddObstacles(this._originalObstacles);
      this._beatmapLevelDataModel.AddWaypoints(this._originalWaypoints);
      this._beatmapLevelDataModel.AddChains(this._originalChains);
      this._beatmapLevelDataModel.AddArcs(this._originalArcs);
      this._beatmapObjectsSelectionState.AddNotes(this._originalNotes.Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id)));
      this._beatmapObjectsSelectionState.AddObstacles(this._originalObstacles.Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (o => o.id)));
      this._beatmapObjectsSelectionState.AddWaypoints(this._originalWaypoints.Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (w => w.id)));
      this._beatmapObjectsSelectionState.AddChains(this._originalChains.Select<ChainEditorData, BeatmapEditorObjectId>((Func<ChainEditorData, BeatmapEditorObjectId>) (c => c.id)));
      this._beatmapObjectsSelectionState.AddArcs(this._originalArcs.Select<ArcEditorData, BeatmapEditorObjectId>((Func<ArcEditorData, BeatmapEditorObjectId>) (a => a.id)));
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapObjectsSelectionState.Clear();
      this._beatmapLevelDataModel.RemoveNotes(this._originalNotes);
      this._beatmapLevelDataModel.RemoveObstacles(this._originalObstacles);
      this._beatmapLevelDataModel.RemoveWaypoints(this._originalWaypoints);
      this._beatmapLevelDataModel.RemoveChains(this._originalChains);
      this._beatmapLevelDataModel.RemoveArcs(this._originalArcs);
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) this._movedNotes);
      this._beatmapLevelDataModel.AddObstacles((IEnumerable<ObstacleEditorData>) this._movedObstacles);
      this._beatmapLevelDataModel.AddWaypoints((IEnumerable<WaypointEditorData>) this._movedWaypoints);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) this._movedChains);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) this._movedArcs);
      this._beatmapObjectsSelectionState.AddNotes(this._movedNotes.Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id)));
      this._beatmapObjectsSelectionState.AddObstacles(this._movedObstacles.Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (o => o.id)));
      this._beatmapObjectsSelectionState.AddWaypoints(this._movedWaypoints.Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (w => w.id)));
      this._beatmapObjectsSelectionState.AddChains(this._movedChains.Select<ChainEditorData, BeatmapEditorObjectId>((Func<ChainEditorData, BeatmapEditorObjectId>) (c => c.id)));
      this._beatmapObjectsSelectionState.AddArcs(this._movedArcs.Select<ArcEditorData, BeatmapEditorObjectId>((Func<ArcEditorData, BeatmapEditorObjectId>) (a => a.id)));
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    private Dictionary<BeatmapEditorObjectId, NoteEditorData> GetMovedNotes(ref bool anyItemMoved)
    {
      List<NoteEditorData> source = new List<NoteEditorData>(this._beatmapObjectsSelectionState.notes.Count);
      foreach (NoteEditorData originalNote in this._originalNotes)
      {
        float newBeat = this.GetNewBeat(this._signal.direction, originalNote.beat);
        NoteEditorData noteEditorData1 = NoteEditorData.CopyWithModifications(originalNote, beat: new float?(newBeat));
        NoteEditorData noteEditorData2 = this.GetNote(newBeat, originalNote.column, originalNote.row);
        int num = !(noteEditorData2 != (NoteEditorData) null) ? 0 : (this._originalNotes.Any<NoteEditorData>(new Func<NoteEditorData, bool>(((BaseBeatmapObjectEditorData) noteEditorData2).PositionEquals)) ? 1 : 0);
        bool flag = source.Any<NoteEditorData>(new Func<NoteEditorData, bool>(((BaseBeatmapObjectEditorData) noteEditorData1).PositionEquals));
        if (num != 0 && !flag)
          noteEditorData2 = (NoteEditorData) null;
        anyItemMoved |= noteEditorData2 == (NoteEditorData) null;
        source.Add(noteEditorData2 == (NoteEditorData) null ? noteEditorData1 : originalNote);
      }
      return source.ToDictionary<NoteEditorData, BeatmapEditorObjectId, NoteEditorData>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id), (Func<NoteEditorData, NoteEditorData>) (n => n));
    }

    private List<ObstacleEditorData> GetMovedObstacles(ref bool anyItemMoved)
    {
      List<ObstacleEditorData> source = new List<ObstacleEditorData>(this._beatmapObjectsSelectionState.obstacles.Count);
      foreach (ObstacleEditorData originalObstacle in this._originalObstacles)
      {
        float newBeat = this.GetNewBeat(this._signal.direction, originalObstacle.beat);
        ObstacleEditorData obstacleEditorData1 = ObstacleEditorData.CopyWithModifications(originalObstacle, beat: new float?(newBeat));
        ObstacleEditorData obstacleEditorData2 = this.GetObstacle(newBeat, originalObstacle.column, originalObstacle.row);
        int num = !(obstacleEditorData2 != (ObstacleEditorData) null) ? 0 : (this._originalObstacles.Any<ObstacleEditorData>(new Func<ObstacleEditorData, bool>(((BaseBeatmapObjectEditorData) obstacleEditorData2).PositionEquals)) ? 1 : 0);
        bool flag = source.Any<ObstacleEditorData>(new Func<ObstacleEditorData, bool>(((BaseBeatmapObjectEditorData) obstacleEditorData1).PositionEquals));
        if (num != 0 && !flag)
          obstacleEditorData2 = (ObstacleEditorData) null;
        anyItemMoved |= obstacleEditorData2 == (ObstacleEditorData) null;
        source.Add(obstacleEditorData2 == (ObstacleEditorData) null ? obstacleEditorData1 : originalObstacle);
      }
      return source;
    }

    private List<WaypointEditorData> GetMovedWaypoints(ref bool anyItemMoved)
    {
      List<WaypointEditorData> source = new List<WaypointEditorData>(this._beatmapObjectsSelectionState.waypoints.Count);
      foreach (WaypointEditorData originalWaypoint in this._originalWaypoints)
      {
        float newBeat = this.GetNewBeat(this._signal.direction, originalWaypoint.beat);
        WaypointEditorData newWithId = WaypointEditorData.CreateNewWithId(originalWaypoint.id, newBeat, originalWaypoint.column, originalWaypoint.row, originalWaypoint.offsetDirection);
        WaypointEditorData waypointEditorData = this.GetWaypoint(newBeat, originalWaypoint.column, originalWaypoint.row);
        int num = !(waypointEditorData != (WaypointEditorData) null) ? 0 : (this._originalWaypoints.Any<WaypointEditorData>(new Func<WaypointEditorData, bool>(((BaseBeatmapObjectEditorData) waypointEditorData).PositionEquals)) ? 1 : 0);
        bool flag = source.Any<WaypointEditorData>(new Func<WaypointEditorData, bool>(((BaseBeatmapObjectEditorData) newWithId).PositionEquals));
        if (num != 0 && !flag)
          waypointEditorData = (WaypointEditorData) null;
        anyItemMoved |= waypointEditorData == (WaypointEditorData) null;
        source.Add(waypointEditorData == (WaypointEditorData) null ? newWithId : originalWaypoint);
      }
      return source;
    }

    private List<ChainEditorData> GetMovedChains(ref bool anyItemMoved)
    {
      List<ChainEditorData> source = new List<ChainEditorData>(this._beatmapObjectsSelectionState.chains.Count);
      foreach (ChainEditorData originalChain in this._originalChains)
      {
        float newBeat = this.GetNewBeat(this._signal.direction, originalChain.beat);
        float num1 = originalChain.tailBeat - originalChain.beat;
        ChainEditorData chainEditorData1 = ChainEditorData.CopyWithModifications(originalChain, beat: new float?(newBeat), tailBeat: new float?(newBeat + num1));
        ChainEditorData chainEditorData2 = this.GetChain(newBeat, originalChain.column, originalChain.row);
        int num2 = !(chainEditorData2 != (ChainEditorData) null) ? 0 : (this._originalChains.Any<ChainEditorData>(new Func<ChainEditorData, bool>(chainEditorData2.PositionEquals)) ? 1 : 0);
        bool flag = source.Any<ChainEditorData>(new Func<ChainEditorData, bool>(chainEditorData1.PositionEquals));
        if (num2 != 0 && !flag)
          chainEditorData2 = (ChainEditorData) null;
        anyItemMoved |= chainEditorData2 == (ChainEditorData) null;
        source.Add(chainEditorData2 == (ChainEditorData) null ? chainEditorData1 : originalChain);
      }
      return source;
    }

    private List<ArcEditorData> GetMovedArcs(ref bool anyItemMoved)
    {
      List<ArcEditorData> source = new List<ArcEditorData>(this._beatmapObjectsSelectionState.arcs.Count);
      foreach (ArcEditorData originalArc in this._originalArcs)
      {
        float newBeat = this.GetNewBeat(this._signal.direction, originalArc.beat);
        float num1 = originalArc.tailBeat - originalArc.beat;
        ArcEditorData arcEditorData1 = ArcEditorData.CopyWithModifications(originalArc, beat: new float?(newBeat), tailBeat: new float?(newBeat + num1));
        ArcEditorData arcEditorData2 = this.GetArc(newBeat, originalArc.column, originalArc.row);
        int num2 = !(arcEditorData2 != (ArcEditorData) null) ? 0 : (this._originalArcs.Any<ArcEditorData>(new Func<ArcEditorData, bool>(arcEditorData2.PositionEquals)) ? 1 : 0);
        bool flag = source.Any<ArcEditorData>(new Func<ArcEditorData, bool>(arcEditorData1.PositionEquals));
        if (num2 != 0 && !flag)
          arcEditorData2 = (ArcEditorData) null;
        anyItemMoved |= arcEditorData2 == (ArcEditorData) null;
        source.Add(arcEditorData2 == (ArcEditorData) null ? arcEditorData1 : originalArc);
      }
      return source;
    }

    private NoteEditorData GetNote(float time, int column, int row) => this._beatmapLevelDataModel.GetNote(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private ObstacleEditorData GetObstacle(float time, int column, int row) => this._beatmapLevelDataModel.GetObstacle(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private WaypointEditorData GetWaypoint(float time, int column, int row) => this._beatmapLevelDataModel.GetWaypoint(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private ChainEditorData GetChain(float time, int column, int row) => this._beatmapLevelDataModel.GetChainByHead(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private ArcEditorData GetArc(float time, int column, int row) => this._beatmapLevelDataModel.GetArcByHead(new BeatmapObjectCellData(new Vector2Int(column, row), time));

    private float GetNewBeat(
      MoveBeatmapObjectSelectionInTimeSignal.Direction direction,
      float beat)
    {
      int num = direction == MoveBeatmapObjectSelectionInTimeSignal.Direction.Forward ? 1 : -1;
      return AudioTimeHelper.ChangeBeatBySubdivision(beat, this._beatmapState.subdivision * 128, num * 128);
    }
  }
}
