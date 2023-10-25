// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapLevelDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapLevelDataModel : IBeatmapLevelDataModel, IReadonlyBeatmapLevelDataModel
  {
    private bool _isDirty;
    private readonly NotesRepository _notesRepository = new NotesRepository();
    private readonly ObstaclesRepository _obstaclesRepository = new ObstaclesRepository();
    private readonly WaypointsRepository _waypointsRepository = new WaypointsRepository();
    private readonly ArcsRepository _arcsRepository = new ArcsRepository();
    private readonly ChainsRepository _chainsRepository = new ChainsRepository();

    public bool beatmapLevelLoaded { get; set; }

    public BeatmapCharacteristicSO beatmapCharacteristic { get; private set; }

    public BeatmapDifficulty beatmapDifficulty { get; private set; }

    public IReadOnlyList<NoteEditorData> notes => (IReadOnlyList<NoteEditorData>) this._notesRepository.notes;

    public IReadOnlyList<WaypointEditorData> waypoints => (IReadOnlyList<WaypointEditorData>) this._waypointsRepository.waypoints;

    public IReadOnlyList<ObstacleEditorData> obstacles => (IReadOnlyList<ObstacleEditorData>) this._obstaclesRepository.obstacles;

    public IReadOnlyList<ArcEditorData> arcs => (IReadOnlyList<ArcEditorData>) this._arcsRepository.arcs;

    public IReadOnlyList<ChainEditorData> chains => (IReadOnlyList<ChainEditorData>) this._chainsRepository.chains;

    public bool isDirty => this._isDirty;

    public void UpdateWith(
      BeatmapCharacteristicSO beatmapCharacteristic = null,
      BeatmapDifficulty? beatmapDifficulty = null,
      List<NoteEditorData> notes = null,
      List<WaypointEditorData> waypoints = null,
      List<ObstacleEditorData> obstacles = null,
      List<ArcEditorData> arcs = null,
      List<ChainEditorData> chains = null)
    {
      this.beatmapCharacteristic = (UnityEngine.Object) beatmapCharacteristic != (UnityEngine.Object) null ? beatmapCharacteristic : this.beatmapCharacteristic;
      this.beatmapDifficulty = (BeatmapDifficulty) ((int) beatmapDifficulty ?? (int) this.beatmapDifficulty);
      if (notes != null)
      {
        this._notesRepository.Clear();
        this.AddNotes((IEnumerable<NoteEditorData>) notes);
      }
      if (waypoints != null)
      {
        this._waypointsRepository.Clear();
        this.AddWaypoints((IEnumerable<WaypointEditorData>) waypoints);
      }
      if (obstacles != null)
      {
        this._obstaclesRepository.Clear();
        this.AddObstacles((IEnumerable<ObstacleEditorData>) obstacles);
      }
      if (chains != null)
      {
        this.RemoveNotes(chains.Select<ChainEditorData, NoteEditorData>((Func<ChainEditorData, NoteEditorData>) (chain => this.GetNote(new BeatmapObjectCellData(new Vector2Int(chain.column, chain.row), chain.beat)))).Where<NoteEditorData>((Func<NoteEditorData, bool>) (note => note != (NoteEditorData) null)));
        this._chainsRepository.Clear();
        this.AddChains((IEnumerable<ChainEditorData>) chains);
      }
      if (arcs != null)
      {
        this._arcsRepository.Clear();
        this.AddArcs((IEnumerable<ArcEditorData>) arcs);
      }
      this._isDirty = false;
    }

    public void ClearDirty() => this._isDirty = false;

    public void Clear()
    {
      this._notesRepository.Clear();
      this._waypointsRepository.Clear();
      this._obstaclesRepository.Clear();
      this._arcsRepository.Clear();
      this._isDirty = false;
      this.beatmapLevelLoaded = false;
    }

    public void AddNote(NoteEditorData n)
    {
      this._notesRepository.Add(n);
      this._isDirty = true;
    }

    public void AddNotes(IEnumerable<NoteEditorData> notes)
    {
      foreach (NoteEditorData note in notes)
        this.AddNote(note);
      this._isDirty = true;
    }

    public void RemoveNote(NoteEditorData note)
    {
      this._notesRepository.Remove(note);
      this._isDirty = true;
    }

    public void RemoveNotes(IEnumerable<NoteEditorData> notes)
    {
      foreach (NoteEditorData note in notes)
        this.RemoveNote(note);
      this._isDirty = true;
    }

    public void AddObstacle(ObstacleEditorData o)
    {
      this._obstaclesRepository.Add(o);
      this._isDirty = true;
    }

    public void AddObstacles(IEnumerable<ObstacleEditorData> obstacles)
    {
      foreach (ObstacleEditorData obstacle in obstacles)
        this.AddObstacle(obstacle);
      this._isDirty = true;
    }

    public void RemoveObstacle(ObstacleEditorData obstacle)
    {
      this._obstaclesRepository.Remove(obstacle);
      this._isDirty = true;
    }

    public void RemoveObstacles(IEnumerable<ObstacleEditorData> obstacles)
    {
      foreach (ObstacleEditorData obstacle in obstacles)
        this.RemoveObstacle(obstacle);
      this._isDirty = true;
    }

    public void AddWaypoint(WaypointEditorData w)
    {
      this._waypointsRepository.Add(w);
      this._isDirty = true;
    }

    public void AddWaypoints(IEnumerable<WaypointEditorData> waypoints)
    {
      foreach (WaypointEditorData waypoint in waypoints)
        this.AddWaypoint(waypoint);
      this._isDirty = true;
    }

    public void RemoveWaypoint(WaypointEditorData waypoint)
    {
      this._waypointsRepository.Remove(waypoint);
      this._isDirty = true;
    }

    public void RemoveWaypoints(IEnumerable<WaypointEditorData> waypoints)
    {
      foreach (WaypointEditorData waypoint in waypoints)
        this.RemoveWaypoint(waypoint);
      this._isDirty = true;
    }

    public void AddArc(ArcEditorData arc)
    {
      this._arcsRepository.Add(arc);
      this._isDirty = true;
    }

    public void AddArcs(IEnumerable<ArcEditorData> arcs)
    {
      foreach (ArcEditorData arc in arcs)
        this.AddArc(arc);
      this._isDirty = true;
    }

    public void RemoveArc(ArcEditorData arc)
    {
      this._arcsRepository.Remove(arc);
      this._isDirty = true;
    }

    public void RemoveArcs(IEnumerable<ArcEditorData> arcs)
    {
      foreach (ArcEditorData arc in arcs)
        this.RemoveArc(arc);
      this._isDirty = true;
    }

    public void AddChain(ChainEditorData chain)
    {
      this._chainsRepository.Add(chain);
      this._isDirty = true;
    }

    public void AddChains(IEnumerable<ChainEditorData> chains)
    {
      foreach (ChainEditorData chain in chains)
        this.AddChain(chain);
    }

    public void RemoveChain(ChainEditorData chain)
    {
      this._chainsRepository.Remove(chain);
      this._isDirty = true;
    }

    public void RemoveChains(IEnumerable<ChainEditorData> chains)
    {
      foreach (ChainEditorData chain in chains)
        this._chainsRepository.Remove(chain);
      this._isDirty = true;
    }

    public IEnumerable<NoteEditorData> GetNotesInterval(float startBeat, float endBeat) => this._notesRepository.GetNotesInterval(startBeat, endBeat);

    public NoteEditorData GetNote(BeatmapObjectCellData cellData) => this._notesRepository.GetNoteByPosition(cellData);

    public NoteEditorData GetNoteById(BeatmapEditorObjectId noteId) => this._notesRepository.GetNoteById(noteId);

    public IEnumerable<ObstacleEditorData> GetObstaclesInterval(float startBeat, float endBeat) => this._obstaclesRepository.GetObstaclesInterval(startBeat, endBeat);

    public ObstacleEditorData GetObstacle(BeatmapObjectCellData cellData) => this._obstaclesRepository.GetObstacleByPosition(cellData);

    public ObstacleEditorData GetObstacleById(BeatmapEditorObjectId obstacleId) => this._obstaclesRepository.GetObstacleById(obstacleId);

    public WaypointEditorData GetWaypoint(BeatmapObjectCellData cellData) => this._waypointsRepository.GetWaypointByPosition(cellData);

    public IEnumerable<WaypointEditorData> GetWaypointsInterval(float startBeat, float endBeat) => this._waypointsRepository.GetWaypointsInterval(startBeat, endBeat);

    public WaypointEditorData GetWaypointById(BeatmapEditorObjectId waypointId) => this._waypointsRepository.GetWaypointById(waypointId);

    public ArcEditorData GetArcByHead(BeatmapObjectCellData cellData) => this._arcsRepository.GetArcByHeadPosition(cellData);

    public ArcEditorData GetArcByTail(BeatmapObjectCellData cellData) => this._arcsRepository.GetArcByTailPosition(cellData);

    public ArcEditorData GetArcById(BeatmapEditorObjectId arcId) => this._arcsRepository.GetArcById(arcId);

    public IEnumerable<ArcEditorData> GetArcsInterval(float startBeat, float endBeat) => this._arcsRepository.GetArcsInterval(startBeat, endBeat);

    public ChainEditorData GetChainByHead(BeatmapObjectCellData cellData) => this._chainsRepository.GetChainByHeadPosition(cellData);

    public ChainEditorData GetChainByTail(BeatmapObjectCellData cellData) => this._chainsRepository.GetChainByTailPosition(cellData);

    public ChainEditorData GetChainById(BeatmapEditorObjectId id) => this._chainsRepository.GetChainById(id);

    public IEnumerable<ChainEditorData> GetChainsInterval(float startBeat, float endBeat) => this._chainsRepository.GetChainsInterval(startBeat, endBeat);

    public bool AnyBeatmapObjectExists(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow)
    {
      return this._notesRepository.AnyNoteExists(startTime, startColumn, startRow, endTime, endColumn, endRow) || this._waypointsRepository.AnyWaypointExists(startTime, startColumn, startRow, endTime, endColumn, endRow) || this._obstaclesRepository.AnyObstacleExists(startTime, startColumn, startRow, endTime, endColumn, endRow) || this._chainsRepository.AnyChainExists(startTime, startColumn, startRow, endTime, endColumn, endRow);
    }

    public bool AnyBeatmapObjectExistsWithoutIntersecting(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow,
      BeatmapEditorObjectId avoidId)
    {
      return this._notesRepository.AnyNoteExistsWithoutIntersecting(startTime, startColumn, startRow, endTime, endColumn, endRow, avoidId) || this._waypointsRepository.AnyWaypointExistsWithoutIntersecting(startTime, startColumn, startRow, endTime, endColumn, endRow, avoidId) || this._obstaclesRepository.AnyObstacleExistsWithoutIntersecting(startTime, startColumn, startRow, endTime, endColumn, endRow, avoidId) || this._chainsRepository.AnyChainExistsWithoutIntersecting(startTime, startColumn, startRow, endTime, endColumn, endRow, avoidId);
    }

    public bool HasNoteOrChain(BeatmapObjectCellData cellData) => this.GetNote(cellData) != (NoteEditorData) null || this.GetChainByHead(cellData) != (ChainEditorData) null || this.GetChainByTail(cellData) != (ChainEditorData) null;
  }
}
