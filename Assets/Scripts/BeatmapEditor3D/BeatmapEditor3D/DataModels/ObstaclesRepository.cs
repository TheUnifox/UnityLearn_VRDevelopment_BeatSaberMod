// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.ObstaclesRepository
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class ObstaclesRepository
  {
    private readonly IntervalTree.IntervalTree<float, ObstacleEditorData> _tree = new IntervalTree.IntervalTree<float, ObstacleEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, ObstacleEditorData> _dictionary = (IDictionary<BeatmapEditorObjectId, ObstacleEditorData>) new Dictionary<BeatmapEditorObjectId, ObstacleEditorData>();

    public List<ObstacleEditorData> obstacles => this._tree.Values.ToList<ObstacleEditorData>();

    public void Add(ObstacleEditorData obstacle)
    {
      this._tree.Add(obstacle.beat, obstacle.beat + obstacle.duration, obstacle);
      this._dictionary.Add(obstacle.id, obstacle);
    }

    public void Add(IEnumerable<ObstacleEditorData> obstacles)
    {
      foreach (ObstacleEditorData obstacle in obstacles)
        this.Add(obstacle);
    }

    public void Remove(ObstacleEditorData obstacle)
    {
      ObstacleEditorData obstacleEditorData;
      if (!this._dictionary.TryGetValue(obstacle.id, out obstacleEditorData))
        return;
      this._tree.Remove(obstacleEditorData);
      this._dictionary.Remove(obstacle.id);
    }

    public ObstacleEditorData GetObstacleByPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (o =>
    {
      ObstacleEditorData obstacle = o;
      Vector2Int cellPosition = cellData.cellPosition;
      int x = cellPosition.x;
      cellPosition = cellData.cellPosition;
      int y = cellPosition.y;
      return ObstaclesRepository.ObstacleFilter(obstacle, x, y);
    }));

    public ObstacleEditorData GetObstacleById(BeatmapEditorObjectId obstacleId)
    {
      ObstacleEditorData obstacleEditorData;
      return !this._dictionary.TryGetValue(obstacleId, out obstacleEditorData) ? (ObstacleEditorData) null : obstacleEditorData;
    }

    public IEnumerable<ObstacleEditorData> GetObstaclesInterval(float startBeat, float endBeat) => (IEnumerable<ObstacleEditorData>) this._tree.Query(startBeat, endBeat).OrderBy<ObstacleEditorData, float>((Func<ObstacleEditorData, float>) (o => o.beat));

    public bool AnyObstacleExists(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow)
    {
      return this._tree.Query(startTime, endTime).Any<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (o => ObstaclesRepository.ObstacleFilter(o, startColumn, startRow, endColumn, endRow)));
    }

    public bool AnyObstacleExistsWithoutIntersecting(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow,
      BeatmapEditorObjectId avoidId)
    {
      return this._tree.Query(startTime, endTime).Where<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (n => ObstaclesRepository.ObstacleFilter(n, startColumn, startRow, endColumn, endRow))).Any<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (n => n.id != avoidId));
    }

    public void Clear()
    {
      this._dictionary.Clear();
      this._tree.Clear();
    }

    private static bool ObstacleFilter(ObstacleEditorData obstacle, int column, int row) => new Rect((float) obstacle.column, (float) obstacle.row, (float) obstacle.width, (float) obstacle.height).Contains((Vector2) new Vector2Int(column, row));

    private static bool ObstacleFilter(
      ObstacleEditorData obstacle,
      int startColumn,
      int startRow,
      int endColumn,
      int endRow)
    {
      return endColumn - startColumn == 0 && endRow - startRow == 0 ? new RectInt(obstacle.column, obstacle.row, obstacle.width, obstacle.height).Contains(new Vector2Int(startColumn, startRow)) : new RectInt(obstacle.column, obstacle.row, obstacle.width, obstacle.height).Overlaps(new RectInt(startColumn, startRow, endColumn - startColumn, endRow - startRow));
    }
  }
}
