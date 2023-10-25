// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsSelectionState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsSelectionState : 
    IReadonlyBeatmapObjectsSelectionState,
    IInitializable,
    IDisposable
  {
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _notes = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _obstacles = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _waypoints = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _chains = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();
    private readonly BeatmapEditorObjectsCollection<BeatmapEditorObjectId> _arcs = new BeatmapEditorObjectsCollection<BeatmapEditorObjectId>();

    public IReadOnlyList<BeatmapEditorObjectId> notes => this._notes.items;

    public IReadOnlyList<BeatmapEditorObjectId> obstacles => this._obstacles.items;

    public IReadOnlyList<BeatmapEditorObjectId> waypoints => this._waypoints.items;

    public IReadOnlyList<BeatmapEditorObjectId> chains => this._chains.items;

    public IReadOnlyList<BeatmapEditorObjectId> arcs => this._arcs.items;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> notesCollection => this._notes;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> obstaclesCollection => this._obstacles;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> waypointsCollection => this._waypoints;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> chainsCollection => this._chains;

    public BeatmapEditorObjectsCollection<BeatmapEditorObjectId> arcsCollection => this._arcs;

    public bool showSelection { get; set; }

    public float startBeat { get; set; }

    public float endBeat { get; set; }

    public float tempStartSelectionBeat { get; set; }

    public float tempEndSelectionBeat { get; set; }

    public BeatmapObjectType draggedBeatmapObjectType { get; set; }

    public BeatmapEditorObjectId draggedBeatmapObjectId { get; set; }

    public BeatmapObjectCellData draggedBeatmapObjectCellData { get; set; }

    public bool? draggedHead { get; set; }

    public void Initialize() => this._signalBus.Subscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void Dispose() => this._signalBus.TryUnsubscribe<BeatmapLevelUpdatedSignal>(new Action(this.HandleBeatmapLevelUpdated));

    public void AddNote(BeatmapEditorObjectId item) => this._notes.Add(item);

    public void AddNotes(IEnumerable<BeatmapEditorObjectId> items) => this._notes.AddRange(items);

    public void AddObstacle(BeatmapEditorObjectId item) => this._obstacles.Add(item);

    public void AddObstacles(IEnumerable<BeatmapEditorObjectId> items) => this._obstacles.AddRange(items);

    public void AddWaypoint(BeatmapEditorObjectId item) => this._waypoints.Add(item);

    public void AddWaypoints(IEnumerable<BeatmapEditorObjectId> items) => this._waypoints.AddRange(items);

    public void AddChain(BeatmapEditorObjectId item) => this._chains.Add(item);

    public void AddChains(IEnumerable<BeatmapEditorObjectId> items) => this._chains.AddRange(items);

    public void AddArc(BeatmapEditorObjectId item) => this._arcs.Add(item);

    public void AddArcs(IEnumerable<BeatmapEditorObjectId> items) => this._arcs.AddRange(items);

    public void RemoveNote(BeatmapEditorObjectId item) => this._notes.Remove(item);

    public void RemoveNotes(IEnumerable<BeatmapEditorObjectId> items) => this._notes.RemoveRange(items);

    public void RemoveObstacle(BeatmapEditorObjectId item) => this._obstacles.Remove(item);

    public void RemoveObstacles(IEnumerable<BeatmapEditorObjectId> items) => this._obstacles.RemoveRange(items);

    public void RemoveWaypoint(BeatmapEditorObjectId item) => this._waypoints.Remove(item);

    public void RemoveWaypoints(IEnumerable<BeatmapEditorObjectId> items) => this._waypoints.RemoveRange(items);

    public void RemoveChain(BeatmapEditorObjectId item) => this._chains.Remove(item);

    public void RemoveChains(IEnumerable<BeatmapEditorObjectId> items) => this._chains.RemoveRange(items);

    public void RemoveArc(BeatmapEditorObjectId item) => this._arcs.Remove(item);

    public void RemoveArcs(IEnumerable<BeatmapEditorObjectId> items) => this._arcs.RemoveRange(items);

    public void Clear()
    {
      this._notes.Clear();
      this._obstacles.Clear();
      this._waypoints.Clear();
      this._chains.Clear();
      this._arcs.Clear();
    }

    public bool IsNoteSelected(BeatmapEditorObjectId item) => this._notes.Contains(item);

    public bool IsObstacleSelected(BeatmapEditorObjectId item) => this._obstacles.Contains(item);

    public bool IsWaypointSelected(BeatmapEditorObjectId item) => this._waypoints.Contains(item);

    public bool IsChainSelected(BeatmapEditorObjectId item) => this._chains.Contains(item);

    public bool IsArcSelected(BeatmapEditorObjectId item) => this._arcs.Contains(item);

    public bool IsAnythingSelected() => this._notes.count > 0 || this._obstacles.count > 0 || this._waypoints.count > 0 || this._chains.count > 0 || this._arcs.count > 0;

    private void HandleBeatmapLevelUpdated()
    {
      if (!this.IsAnythingSelected())
        return;
      bool flag = false;
      foreach (BeatmapEditorObjectId noteId in this._notes.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._beatmapLevelDataModel.GetNoteById(noteId) == (NoteEditorData) null)
        {
          this.RemoveNote(noteId);
          flag = true;
        }
      }
      foreach (BeatmapEditorObjectId obstacleId in this._obstacles.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._beatmapLevelDataModel.GetObstacleById(obstacleId) == (ObstacleEditorData) null)
        {
          this.RemoveObstacle(obstacleId);
          flag = true;
        }
      }
      foreach (BeatmapEditorObjectId waypointId in this._waypoints.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._beatmapLevelDataModel.GetWaypointById(waypointId) == (WaypointEditorData) null)
        {
          this.RemoveWaypoint(waypointId);
          flag = true;
        }
      }
      foreach (BeatmapEditorObjectId id in this._chains.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._beatmapLevelDataModel.GetChainById(id) == (ChainEditorData) null)
        {
          this.RemoveChain(id);
          flag = true;
        }
      }
      foreach (BeatmapEditorObjectId arcId in this._arcs.items.ToList<BeatmapEditorObjectId>())
      {
        if (this._beatmapLevelDataModel.GetArcById(arcId) == (ArcEditorData) null)
        {
          this.RemoveArc(arcId);
          flag = true;
        }
      }
      if (!flag)
        return;
      this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
    }
  }
}
