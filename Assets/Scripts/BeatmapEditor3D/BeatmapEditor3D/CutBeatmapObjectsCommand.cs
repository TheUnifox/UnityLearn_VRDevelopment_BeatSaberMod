// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CutBeatmapObjectsCommand
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

namespace BeatmapEditor3D
{
  public class CutBeatmapObjectsCommand : 
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
    [Inject]
    private readonly BeatmapObjectsClipboardState _beatmapObjectsClipboardState;
    private List<NoteEditorData> _notes;
    private List<ObstacleEditorData> _obstacles;
    private List<WaypointEditorData> _waypoints;
    private List<ChainEditorData> _chains;
    private List<ArcEditorData> _arcs;

    public bool shouldAddToHistory => true;

    public void Execute()
    {
      if (!this._beatmapObjectsSelectionState.IsAnythingSelected())
        return;
      this._beatmapObjectsClipboardState.Clear();
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.notes.Select<BeatmapEditorObjectId, NoteEditorData>((Func<BeatmapEditorObjectId, NoteEditorData>) (noteId => this._beatmapLevelDataModel.GetNoteById(noteId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.obstacles.Select<BeatmapEditorObjectId, ObstacleEditorData>((Func<BeatmapEditorObjectId, ObstacleEditorData>) (obstacleId => this._beatmapLevelDataModel.GetObstacleById(obstacleId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.waypoints.Select<BeatmapEditorObjectId, WaypointEditorData>((Func<BeatmapEditorObjectId, WaypointEditorData>) (waypointId => this._beatmapLevelDataModel.GetWaypointById(waypointId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.chains.Select<BeatmapEditorObjectId, ChainEditorData>((Func<BeatmapEditorObjectId, ChainEditorData>) (chainId => this._beatmapLevelDataModel.GetChainById(chainId))));
      this._beatmapObjectsClipboardState.AddRange(this._beatmapObjectsSelectionState.arcs.Select<BeatmapEditorObjectId, ArcEditorData>((Func<BeatmapEditorObjectId, ArcEditorData>) (arcId => this._beatmapLevelDataModel.GetArcById(arcId))));
      this._beatmapObjectsClipboardState.startBeat = Mathf.Min(this._beatmapObjectsClipboardState.notes.Count > 0 ? this._beatmapObjectsClipboardState.notes.Min<NoteEditorData>((Func<NoteEditorData, float>) (n => n.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.obstacles.Count > 0 ? this._beatmapObjectsClipboardState.obstacles.Min<ObstacleEditorData>((Func<ObstacleEditorData, float>) (o => o.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.waypoints.Count > 0 ? this._beatmapObjectsClipboardState.waypoints.Min<WaypointEditorData>((Func<WaypointEditorData, float>) (w => w.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.chains.Count > 0 ? this._beatmapObjectsClipboardState.chains.Min<ChainEditorData>((Func<ChainEditorData, float>) (c => c.beat)) : float.MaxValue, this._beatmapObjectsClipboardState.arcs.Count > 0 ? this._beatmapObjectsClipboardState.arcs.Min<ArcEditorData>((Func<ArcEditorData, float>) (a => a.beat)) : float.MaxValue);
      this._notes = new List<NoteEditorData>((IEnumerable<NoteEditorData>) this._beatmapObjectsClipboardState.notes);
      this._obstacles = new List<ObstacleEditorData>((IEnumerable<ObstacleEditorData>) this._beatmapObjectsClipboardState.obstacles);
      this._waypoints = new List<WaypointEditorData>((IEnumerable<WaypointEditorData>) this._beatmapObjectsClipboardState.waypoints);
      this._chains = new List<ChainEditorData>((IEnumerable<ChainEditorData>) this._beatmapObjectsClipboardState.chains);
      this._arcs = new List<ArcEditorData>((IEnumerable<ArcEditorData>) this._beatmapObjectsClipboardState.arcs);
      this._beatmapObjectsSelectionState.Clear();
      this._signalBus.Fire<BeatmapObjectsClipboardStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
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

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Cut {0} notes, {1} obstacles, {2} waypoints, {3} chains, {4} arcs.", (object) this._notes.Count, (object) this._obstacles.Count, (object) this._waypoints.Count, (object) this._chains.Count, (object) this._arcs.Count));
  }
}
