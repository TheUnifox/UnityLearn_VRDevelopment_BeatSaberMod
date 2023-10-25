// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ArcsRepository
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class ArcsRepository
  {
    private readonly IntervalTree.IntervalTree<float, ArcEditorData> _tree = new IntervalTree.IntervalTree<float, ArcEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, ArcEditorData> _idToArcDictionary = (IDictionary<BeatmapEditorObjectId, ArcEditorData>) new Dictionary<BeatmapEditorObjectId, ArcEditorData>();

    public List<ArcEditorData> arcs => this._tree.Values.ToList<ArcEditorData>();

    public void Add(ArcEditorData arc)
    {
      this._idToArcDictionary.Add(arc.id, arc);
      this._tree.Add(arc.beat, arc.tailBeat, arc);
    }

    public void Add(IEnumerable<ArcEditorData> arcs)
    {
      foreach (ArcEditorData arc in arcs)
        this.Add(arc);
    }

    public void Remove(ArcEditorData arc)
    {
      ArcEditorData arcEditorData;
      if (!this._idToArcDictionary.TryGetValue(arc.id, out arcEditorData))
        return;
      this._idToArcDictionary.Remove(arc.id);
      this._tree.Remove(arcEditorData);
    }

    public ArcEditorData GetArcByHeadPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<ArcEditorData>((Func<ArcEditorData, bool>) (arc =>
    {
      if (AudioTimeHelper.IsBeatSame(cellData.beat, arc.beat))
      {
        int column = arc.column;
        Vector2Int cellPosition = cellData.cellPosition;
        int x = cellPosition.x;
        if (column == x)
        {
          int row = arc.row;
          cellPosition = cellData.cellPosition;
          int y = cellPosition.y;
          return row == y;
        }
      }
      return false;
    }));

    public ArcEditorData GetArcByTailPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<ArcEditorData>((Func<ArcEditorData, bool>) (arc =>
    {
      if (AudioTimeHelper.IsBeatSame(cellData.beat, arc.tailBeat))
      {
        int tailColumn = arc.tailColumn;
        Vector2Int cellPosition = cellData.cellPosition;
        int x = cellPosition.x;
        if (tailColumn == x)
        {
          int tailRow = arc.tailRow;
          cellPosition = cellData.cellPosition;
          int y = cellPosition.y;
          return tailRow == y;
        }
      }
      return false;
    }));

    public ArcEditorData GetArcById(BeatmapEditorObjectId arcId)
    {
      ArcEditorData arcEditorData;
      return !this._idToArcDictionary.TryGetValue(arcId, out arcEditorData) ? (ArcEditorData) null : arcEditorData;
    }

    public IEnumerable<ArcEditorData> GetArcsInterval(float startBeat, float endBeat) => (IEnumerable<ArcEditorData>) this._tree.Query(startBeat, endBeat).OrderBy<ArcEditorData, float>((Func<ArcEditorData, float>) (a => a.beat));

    public void Clear()
    {
      this._tree.Clear();
      this._idToArcDictionary.Clear();
    }
  }
}
