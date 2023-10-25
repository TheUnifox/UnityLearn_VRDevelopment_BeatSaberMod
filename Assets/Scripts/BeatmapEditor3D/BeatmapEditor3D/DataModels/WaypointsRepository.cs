// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.WaypointsRepository
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class WaypointsRepository
  {
    private readonly IntervalTree.IntervalTree<float, WaypointEditorData> _tree = new IntervalTree.IntervalTree<float, WaypointEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, WaypointEditorData> _dictionary = (IDictionary<BeatmapEditorObjectId, WaypointEditorData>) new Dictionary<BeatmapEditorObjectId, WaypointEditorData>();

    public List<WaypointEditorData> waypoints => this._tree.Values.ToList<WaypointEditorData>();

    public void Add(WaypointEditorData waypoint)
    {
      this._dictionary.Add(waypoint.id, waypoint);
      this._tree.Add(waypoint.beat, waypoint.beat, waypoint);
    }

    public void Add(IEnumerable<WaypointEditorData> waypoints)
    {
      foreach (WaypointEditorData waypoint in waypoints)
        this.Add(waypoint);
    }

    public void Remove(WaypointEditorData waypoint)
    {
      WaypointEditorData waypointEditorData;
      if (!this._dictionary.TryGetValue(waypoint.id, out waypointEditorData))
        return;
      this._dictionary.Remove(waypointEditorData.id);
      this._tree.Remove(waypointEditorData);
    }

    public WaypointEditorData GetWaypointByPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<WaypointEditorData>((Func<WaypointEditorData, bool>) (waypoint => waypoint.column == cellData.cellPosition.x && waypoint.row == cellData.cellPosition.y));

    public WaypointEditorData GetWaypointById(BeatmapEditorObjectId waypointId)
    {
      WaypointEditorData waypointEditorData;
      return !this._dictionary.TryGetValue(waypointId, out waypointEditorData) ? (WaypointEditorData) null : waypointEditorData;
    }

    public IEnumerable<WaypointEditorData> GetWaypointsInterval(float startBeat, float endBeat) => (IEnumerable<WaypointEditorData>) this._tree.Query(startBeat, endBeat).OrderBy<WaypointEditorData, float>((Func<WaypointEditorData, float>) (w => w.beat));

    public bool AnyWaypointExists(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow)
    {
      return this._tree.Query(startTime, endTime).Any<WaypointEditorData>((Func<WaypointEditorData, bool>) (w => WaypointsRepository.Overlap(w, startColumn, startRow, endColumn, endRow)));
    }

    public bool AnyWaypointExistsWithoutIntersecting(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow,
      BeatmapEditorObjectId avoidId)
    {
      return this._tree.Query(startTime, endTime).Where<WaypointEditorData>((Func<WaypointEditorData, bool>) (n => WaypointsRepository.Overlap(n, startColumn, startRow, endColumn, endRow))).Any<WaypointEditorData>((Func<WaypointEditorData, bool>) (n => n.id != avoidId));
    }

    public void Clear()
    {
      this._dictionary.Clear();
      this._tree.Clear();
    }

    private static bool Overlap(
      WaypointEditorData waypoint,
      int startColumn,
      int startRow,
      int endColumn,
      int endRow)
    {
      if (startColumn != endColumn || startRow != endRow)
        return new RectInt(startColumn, startRow, endColumn - startColumn, endRow - startRow).Contains(new Vector2Int(waypoint.column, waypoint.row));
      return waypoint.column == startColumn && waypoint.row == startRow;
    }
  }
}
