// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectsClipboardState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public class BeatmapObjectsClipboardState : IReadonlyBeatmapObjectsClipboardState
  {
    private readonly BeatmapEditorObjectsCollection<NoteEditorData> _notes = new BeatmapEditorObjectsCollection<NoteEditorData>();
    private readonly BeatmapEditorObjectsCollection<ObstacleEditorData> _obstacles = new BeatmapEditorObjectsCollection<ObstacleEditorData>();
    private readonly BeatmapEditorObjectsCollection<WaypointEditorData> _waypoints = new BeatmapEditorObjectsCollection<WaypointEditorData>();
    private readonly BeatmapEditorObjectsCollection<ChainEditorData> _chains = new BeatmapEditorObjectsCollection<ChainEditorData>();
    private readonly BeatmapEditorObjectsCollection<ArcEditorData> _arcs = new BeatmapEditorObjectsCollection<ArcEditorData>();

    public float startBeat { get; set; }

    public IReadOnlyList<NoteEditorData> notes => this._notes.items;

    public IReadOnlyList<ObstacleEditorData> obstacles => this._obstacles.items;

    public IReadOnlyList<WaypointEditorData> waypoints => this._waypoints.items;

    public IReadOnlyList<ChainEditorData> chains => this._chains.items;

    public IReadOnlyList<ArcEditorData> arcs => this._arcs.items;

    public void Add(NoteEditorData note) => this._notes.Add(note);

    public void AddRange(IEnumerable<NoteEditorData> notes) => this._notes.AddRange(notes);

    public void Add(ObstacleEditorData obstacle) => this._obstacles.Add(obstacle);

    public void AddRange(IEnumerable<ObstacleEditorData> obstacles) => this._obstacles.AddRange(obstacles);

    public void Add(WaypointEditorData waypoint) => this._waypoints.Add(waypoint);

    public void AddRange(IEnumerable<WaypointEditorData> waypoints) => this._waypoints.AddRange(waypoints);

    public void Add(ChainEditorData chain) => this._chains.Add(chain);

    public void AddRange(IEnumerable<ChainEditorData> chains) => this._chains.AddRange(chains);

    public void Add(ArcEditorData arc) => this._arcs.Add(arc);

    public void AddRange(IEnumerable<ArcEditorData> arc) => this._arcs.AddRange(arc);

    public void Clear()
    {
      this._notes.Clear();
      this._obstacles.Clear();
      this._waypoints.Clear();
      this._chains.Clear();
      this._arcs.Clear();
    }
  }
}
