// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DeleteBeatmapObjectsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class DeleteBeatmapObjectsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    private List<NoteEditorData> _notes;
    private List<ObstacleEditorData> _obstacles;
    private List<WaypointEditorData> _waypoints;
    private List<ChainEditorData> _chains;
    private List<ArcEditorData> _arcs;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (!this._beatmapObjectsSelectionState.IsAnythingSelected())
        return;
      this._notes = this._beatmapObjectsSelectionState.notes.Select<BeatmapEditorObjectId, NoteEditorData>((Func<BeatmapEditorObjectId, NoteEditorData>) (noteId => this._beatmapLevelDataModel.GetNoteById(noteId))).ToList<NoteEditorData>();
      this._obstacles = this._beatmapObjectsSelectionState.obstacles.Select<BeatmapEditorObjectId, ObstacleEditorData>((Func<BeatmapEditorObjectId, ObstacleEditorData>) (obstacleId => this._beatmapLevelDataModel.GetObstacleById(obstacleId))).ToList<ObstacleEditorData>();
      this._waypoints = this._beatmapObjectsSelectionState.waypoints.Select<BeatmapEditorObjectId, WaypointEditorData>((Func<BeatmapEditorObjectId, WaypointEditorData>) (waypointId => this._beatmapLevelDataModel.GetWaypointById(waypointId))).ToList<WaypointEditorData>();
      this._chains = this._beatmapObjectsSelectionState.chains.Select<BeatmapEditorObjectId, ChainEditorData>((Func<BeatmapEditorObjectId, ChainEditorData>) (chainId => this._beatmapLevelDataModel.GetChainById(chainId))).ToList<ChainEditorData>();
      this._arcs = this._beatmapObjectsSelectionState.arcs.Select<BeatmapEditorObjectId, ArcEditorData>((Func<BeatmapEditorObjectId, ArcEditorData>) (arcId => this._beatmapLevelDataModel.GetArcById(arcId))).ToList<ArcEditorData>();
      IEnumerable<BeatmapEditorObjectId> items1 = this._notes.Where<NoteEditorData>((Func<NoteEditorData, bool>) (note => this._beatmapObjectsSelectionState.IsNoteSelected(note.id))).Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (note => note.id));
      IEnumerable<BeatmapEditorObjectId> items2 = this._obstacles.Where<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (obstacle => this._beatmapObjectsSelectionState.IsObstacleSelected(obstacle.id))).Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (obstacle => obstacle.id));
      IEnumerable<BeatmapEditorObjectId> items3 = this._waypoints.Where<WaypointEditorData>((Func<WaypointEditorData, bool>) (waypoint => this._beatmapObjectsSelectionState.IsWaypointSelected(waypoint.id))).Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (waypoint => waypoint.id));
      IEnumerable<BeatmapEditorObjectId> items4 = this._chains.Where<ChainEditorData>((Func<ChainEditorData, bool>) (chain => this._beatmapObjectsSelectionState.IsChainSelected(chain.id))).Select<ChainEditorData, BeatmapEditorObjectId>((Func<ChainEditorData, BeatmapEditorObjectId>) (chain => chain.id));
      IEnumerable<BeatmapEditorObjectId> items5 = this._arcs.Where<ArcEditorData>((Func<ArcEditorData, bool>) (arc => this._beatmapObjectsSelectionState.IsChainSelected(arc.id))).Select<ArcEditorData, BeatmapEditorObjectId>((Func<ArcEditorData, BeatmapEditorObjectId>) (arc => arc.id));
      this._beatmapObjectsSelectionState.RemoveNotes(items1);
      this._beatmapObjectsSelectionState.RemoveObstacles(items2);
      this._beatmapObjectsSelectionState.RemoveWaypoints(items3);
      this._beatmapObjectsSelectionState.RemoveChains(items4);
      this._beatmapObjectsSelectionState.RemoveArcs(items5);
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) this._notes);
      this._beatmapLevelDataModel.AddObstacles((IEnumerable<ObstacleEditorData>) this._obstacles);
      this._beatmapLevelDataModel.AddWaypoints((IEnumerable<WaypointEditorData>) this._waypoints);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) this._chains);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) this._arcs);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) this._notes);
      this._beatmapLevelDataModel.RemoveObstacles((IEnumerable<ObstacleEditorData>) this._obstacles);
      this._beatmapLevelDataModel.RemoveWaypoints((IEnumerable<WaypointEditorData>) this._waypoints);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) this._chains);
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) this._arcs);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Deleted {0} notes, {1} obstacles, {2} waypoints, {3} chains, {4} arcs.", (object) this._notes.Count, (object) this._obstacles.Count, (object) this._waypoints.Count, (object) this._chains.Count, (object) this._arcs.Count));
  }
}
