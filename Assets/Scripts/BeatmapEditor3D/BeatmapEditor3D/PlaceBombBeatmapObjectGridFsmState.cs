// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.PlaceBombBeatmapObjectGridFsmState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class PlaceBombBeatmapObjectGridFsmState : BeatmapObjectGridFsmState
  {
    public override void HandleGridCellPointerUp(int column, int row) => this.signalBus.Fire<PlaceNoteObjectSignal>(new PlaceNoteObjectSignal(this.beatmapState.beat, column, row));

    public override void HandleGridCellPointerEnter(int column, int row)
    {
      this.beatmapObjectEditGridView.HighlightCell(column, row);
      this.hoverView.ShowPreview(BeatmapObjectType.Bomb, new RectInt(column, row, 1, 1));
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(new BeatmapObjectCellData(new Vector2Int(column, row), this.beatmapState.beat), ChangeHoverSignal.HoverOrigin.Grid));
    }

    public override void HandleGridCellPointerExit(int column, int row)
    {
      this.beatmapObjectEditGridView.ResetHighlights();
      this.hoverView.HidePreview();
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal((BeatmapObjectCellData) null, ChangeHoverSignal.HoverOrigin.Grid));
    }

    public class Factory : PlaceholderFactory<PlaceBombBeatmapObjectGridFsmState>
    {
    }
  }
}
