// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectCellData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapObjectCellData
  {
    public Vector2Int cellPosition { get; }

    public float beat { get; }

    public BeatmapObjectCellData(Vector2Int cellPosition, float beat)
    {
      this.cellPosition = cellPosition;
      this.beat = beat;
    }

    public BeatmapObjectCellData(BaseBeatmapObjectEditorData beatmapObject)
    {
      this.cellPosition = new Vector2Int(beatmapObject.column, beatmapObject.row);
      this.beat = beatmapObject.beat;
    }

    public bool IsPositionSame(BaseBeatmapObjectEditorData beatmapObject)
    {
      if (AudioTimeHelper.IsBeatSame(beatmapObject.beat, this.beat))
      {
        int column = beatmapObject.column;
        Vector2Int cellPosition = this.cellPosition;
        int x = cellPosition.x;
        if (column == x)
        {
          int row = beatmapObject.row;
          cellPosition = this.cellPosition;
          int y = cellPosition.y;
          return row == y;
        }
      }
      return false;
    }
  }
}
