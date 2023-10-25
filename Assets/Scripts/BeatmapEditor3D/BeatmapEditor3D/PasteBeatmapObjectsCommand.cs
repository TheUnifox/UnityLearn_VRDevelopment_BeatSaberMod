// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PasteBeatmapObjectsCommand
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
  public class PasteBeatmapObjectsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsClipboardState _beatmapObjectsClipboardState;
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
      this._notes = new List<NoteEditorData>(this._beatmapObjectsClipboardState.notes.Count);
      this._obstacles = new List<ObstacleEditorData>(this._beatmapObjectsClipboardState.obstacles.Count);
      this._waypoints = new List<WaypointEditorData>(this._beatmapObjectsClipboardState.waypoints.Count);
      this._chains = new List<ChainEditorData>(this._beatmapObjectsClipboardState.chains.Count);
      this._arcs = new List<ArcEditorData>(this._beatmapObjectsClipboardState.arcs.Count);
      float num1 = Mathf.Min(this._beatmapObjectsClipboardState.notes.Select<NoteEditorData, float>((Func<NoteEditorData, float>) (n => n.beat)).Prepend<float>(float.MaxValue).Min(), this._beatmapObjectsClipboardState.obstacles.Select<ObstacleEditorData, float>((Func<ObstacleEditorData, float>) (o => o.beat)).Prepend<float>(float.MaxValue).Min(), this._beatmapObjectsClipboardState.waypoints.Select<WaypointEditorData, float>((Func<WaypointEditorData, float>) (w => w.beat)).Prepend<float>(float.MaxValue).Min(), this._beatmapObjectsClipboardState.chains.Select<ChainEditorData, float>((Func<ChainEditorData, float>) (c => c.beat)).Prepend<float>(float.MaxValue).Min(), this._beatmapObjectsClipboardState.arcs.Select<ArcEditorData, float>((Func<ArcEditorData, float>) (a => a.beat)).Prepend<float>(float.MaxValue).Min());
      foreach (NoteEditorData note in (IEnumerable<NoteEditorData>) this._beatmapObjectsClipboardState.notes)
      {
        float num2 = this._beatmapState.beat + note.beat - num1;
        if ((double) num2 < (double) this._beatmapDataModel.bpmData.totalBeats)
          this._notes.Add(NoteEditorData.CopyWithModifications(note, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), new float?(num2)));
      }
      foreach (ObstacleEditorData obstacle in (IEnumerable<ObstacleEditorData>) this._beatmapObjectsClipboardState.obstacles)
      {
        float num3 = this._beatmapState.beat + obstacle.beat - num1;
        if ((double) num3 < (double) this._beatmapDataModel.bpmData.totalBeats)
          this._obstacles.Add(ObstacleEditorData.CopyWithModifications(obstacle, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), new float?(num3)));
      }
      foreach (WaypointEditorData waypoint in (IEnumerable<WaypointEditorData>) this._beatmapObjectsClipboardState.waypoints)
      {
        float num4 = this._beatmapState.beat + waypoint.beat - num1;
        if ((double) num4 < (double) this._beatmapDataModel.bpmData.totalBeats)
          this._waypoints.Add(WaypointEditorData.CopyWithModifications(waypoint, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), new float?(num4)));
      }
      foreach (ChainEditorData chain in (IEnumerable<ChainEditorData>) this._beatmapObjectsClipboardState.chains)
      {
        float num5 = this._beatmapState.beat + chain.beat - num1;
        float num6 = this._beatmapState.beat + chain.tailBeat - num1;
        if ((double) num6 < (double) this._beatmapDataModel.bpmData.totalBeats)
          this._chains.Add(ChainEditorData.CopyWithModifications(chain, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), new float?(num5), tailBeat: new float?(num6)));
      }
      foreach (ArcEditorData arc in (IEnumerable<ArcEditorData>) this._beatmapObjectsClipboardState.arcs)
      {
        float num7 = this._beatmapState.beat + arc.beat - num1;
        float num8 = this._beatmapState.beat + arc.tailBeat - num1;
        if ((double) num8 < (double) this._beatmapDataModel.bpmData.totalBeats)
          this._arcs.Add(ArcEditorData.CopyWithModifications(arc, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), beat: new float?(num7), tailBeat: new float?(num8)));
      }
      this._notes = this._notes.Where<NoteEditorData>((Func<NoteEditorData, bool>) (n => !this._beatmapLevelDataModel.AnyBeatmapObjectExists(n.beat, n.column, n.row, n.beat, n.column, n.row))).ToList<NoteEditorData>();
      this._obstacles = this._obstacles.Where<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (o => !this._beatmapLevelDataModel.AnyBeatmapObjectExists(o.beat, o.column, o.row, o.endBeat, o.endColumn, o.endRow))).ToList<ObstacleEditorData>();
      this._waypoints = this._waypoints.Where<WaypointEditorData>((Func<WaypointEditorData, bool>) (w => !this._beatmapLevelDataModel.AnyBeatmapObjectExists(w.beat, w.column, w.row, w.beat, w.column, w.row))).ToList<WaypointEditorData>();
      this._chains = this._chains.Where<ChainEditorData>((Func<ChainEditorData, bool>) (c => !this._beatmapLevelDataModel.AnyBeatmapObjectExists(c.beat, c.column, c.row, c.tailBeat, c.column, c.row))).ToList<ChainEditorData>();
      this._arcs = this._arcs.Where<ArcEditorData>((Func<ArcEditorData, bool>) (a => !this._beatmapLevelDataModel.AnyBeatmapObjectExists(a.beat, a.column, a.row, a.tailBeat, a.tailColumn, a.tailColumn))).ToList<ArcEditorData>();
      this.shouldAddToHistory = this._notes.Count > 0 || this._obstacles.Count > 0 || this._waypoints.Count > 0 || this._chains.Count > 0 || this._arcs.Count > 0;
      if (!this.shouldAddToHistory)
        return;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapObjectsSelectionState.Clear();
      this._beatmapLevelDataModel.RemoveNotes((IEnumerable<NoteEditorData>) this._notes);
      this._beatmapLevelDataModel.RemoveObstacles((IEnumerable<ObstacleEditorData>) this._obstacles);
      this._beatmapLevelDataModel.RemoveWaypoints((IEnumerable<WaypointEditorData>) this._waypoints);
      this._beatmapLevelDataModel.RemoveChains((IEnumerable<ChainEditorData>) this._chains);
      this._beatmapLevelDataModel.RemoveArcs((IEnumerable<ArcEditorData>) this._arcs);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapLevelDataModel.AddNotes((IEnumerable<NoteEditorData>) this._notes);
      this._beatmapLevelDataModel.AddObstacles((IEnumerable<ObstacleEditorData>) this._obstacles);
      this._beatmapLevelDataModel.AddWaypoints((IEnumerable<WaypointEditorData>) this._waypoints);
      this._beatmapLevelDataModel.AddChains((IEnumerable<ChainEditorData>) this._chains);
      this._beatmapLevelDataModel.AddArcs((IEnumerable<ArcEditorData>) this._arcs);
      this._beatmapObjectsSelectionState.Clear();
      this._beatmapObjectsSelectionState.AddNotes(this._notes.Select<NoteEditorData, BeatmapEditorObjectId>((Func<NoteEditorData, BeatmapEditorObjectId>) (n => n.id)));
      this._beatmapObjectsSelectionState.AddObstacles(this._obstacles.Select<ObstacleEditorData, BeatmapEditorObjectId>((Func<ObstacleEditorData, BeatmapEditorObjectId>) (o => o.id)));
      this._beatmapObjectsSelectionState.AddWaypoints(this._waypoints.Select<WaypointEditorData, BeatmapEditorObjectId>((Func<WaypointEditorData, BeatmapEditorObjectId>) (w => w.id)));
      this._beatmapObjectsSelectionState.AddChains(this._chains.Select<ChainEditorData, BeatmapEditorObjectId>((Func<ChainEditorData, BeatmapEditorObjectId>) (c => c.id)));
      this._beatmapObjectsSelectionState.AddArcs(this._arcs.Select<ArcEditorData, BeatmapEditorObjectId>((Func<ArcEditorData, BeatmapEditorObjectId>) (a => a.id)));
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Pasted {0} notes, {1} obstacles, {2} waypoints, {3} chains, {4} arcs.", (object) this._notes.Count, (object) this._obstacles.Count, (object) this._waypoints.Count, (object) this._chains.Count, (object) this._arcs.Count));
  }
}
