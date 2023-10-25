// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.ChainsRepository
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using IntervalTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class ChainsRepository
  {
    private readonly IIntervalTree<float, ChainEditorData> _tree = (IIntervalTree<float, ChainEditorData>) new IntervalTree.IntervalTree<float, ChainEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, ChainEditorData> _idToChainDictionary = (IDictionary<BeatmapEditorObjectId, ChainEditorData>) new Dictionary<BeatmapEditorObjectId, ChainEditorData>();

    public List<ChainEditorData> chains => this._tree.Values.ToList<ChainEditorData>();

    public void Add(ChainEditorData chain)
    {
      this._idToChainDictionary.Add(chain.id, chain);
      this._tree.Add(chain.beat, chain.tailBeat, chain);
    }

    public void Add(IEnumerable<ChainEditorData> chains)
    {
      foreach (ChainEditorData chain in chains)
        this.Add(chain);
    }

    public void Remove(ChainEditorData chain)
    {
      ChainEditorData chainEditorData;
      if (!this._idToChainDictionary.TryGetValue(chain.id, out chainEditorData))
        return;
      this._idToChainDictionary.Remove(chain.id);
      this._tree.Remove(chainEditorData);
    }

    public ChainEditorData GetChainByHeadPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<ChainEditorData>((Func<ChainEditorData, bool>) (chain =>
    {
      if (AudioTimeHelper.IsBeatSame(cellData.beat, chain.beat))
      {
        int column = chain.column;
        Vector2Int cellPosition = cellData.cellPosition;
        int x = cellPosition.x;
        if (column == x)
        {
          int row = chain.row;
          cellPosition = cellData.cellPosition;
          int y = cellPosition.y;
          return row == y;
        }
      }
      return false;
    }));

    public ChainEditorData GetChainByTailPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<ChainEditorData>((Func<ChainEditorData, bool>) (chain =>
    {
      if (AudioTimeHelper.IsBeatSame(cellData.beat, chain.tailBeat))
      {
        int tailColumn = chain.tailColumn;
        Vector2Int cellPosition = cellData.cellPosition;
        int x = cellPosition.x;
        if (tailColumn == x)
        {
          int tailRow = chain.tailRow;
          cellPosition = cellData.cellPosition;
          int y = cellPosition.y;
          return tailRow == y;
        }
      }
      return false;
    }));

    public ChainEditorData GetChainById(BeatmapEditorObjectId id)
    {
      ChainEditorData chainEditorData;
      return !this._idToChainDictionary.TryGetValue(id, out chainEditorData) ? (ChainEditorData) null : chainEditorData;
    }

    public IEnumerable<ChainEditorData> GetChainsInterval(float startBeat, float endBeat) => (IEnumerable<ChainEditorData>) this._tree.Query(startBeat, endBeat).OrderBy<ChainEditorData, float>((Func<ChainEditorData, float>) (s => s.beat));

    public bool AnyChainExists(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow)
    {
      return this._tree.Query(startTime, endTime).Any<ChainEditorData>((Func<ChainEditorData, bool>) (c => ChainsRepository.Overlap(c, startColumn, startRow, endColumn, endRow)));
    }

    public bool AnyChainExistsWithoutIntersecting(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow,
      BeatmapEditorObjectId avoidId)
    {
      return this._tree.Query(startTime, endTime).Where<ChainEditorData>((Func<ChainEditorData, bool>) (n => ChainsRepository.Overlap(n, startColumn, startRow, endColumn, endRow))).Any<ChainEditorData>((Func<ChainEditorData, bool>) (n => n.id != avoidId));
    }

    public void Clear()
    {
      this._tree.Clear();
      this._idToChainDictionary.Clear();
    }

    private static bool Overlap(
      ChainEditorData note,
      int startColumn,
      int startRow,
      int endColumn,
      int endRow)
    {
      if (startColumn != endColumn || startRow != endRow)
        return new RectInt(startColumn, startRow, endColumn - startColumn, endRow - startRow).Contains(new Vector2Int(note.column, note.row));
      return note.column == startColumn && note.row == startRow;
    }
  }
}
