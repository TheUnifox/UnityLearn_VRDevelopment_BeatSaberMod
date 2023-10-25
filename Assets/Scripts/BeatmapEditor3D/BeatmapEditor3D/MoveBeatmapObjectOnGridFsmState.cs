// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.MoveBeatmapObjectOnGridFsmState
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
  public class MoveBeatmapObjectOnGridFsmState : BeatmapObjectGridFsmState
  {
    public override void HandleGridCellPointerEnter(int column, int row)
    {
      this.HighlightCellAndUpdateHover(column, row);
      if (this.beatmapState.interactionMode != InteractionMode.Modify || this.beatmapObjectsSelectionState.draggedBeatmapObjectId == BeatmapEditorObjectId.invalid)
        return;
      this.signalBus.Fire<MoveHoveredBeatmapObjectOnGridSignal>(new MoveHoveredBeatmapObjectOnGridSignal(new Vector2Int(column, row)));
    }

    public override void HandleGridCellPointerExit(int column, int row) => this.beatmapObjectEditGridView.ResetHighlights();

    private void HighlightCellAndUpdateHover(int column, int row)
    {
      this.beatmapObjectEditGridView.ResetHighlight(column, row);
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal(new BeatmapObjectCellData(new Vector2Int(column, row), this.beatmapState.beat), ChangeHoverSignal.HoverOrigin.Grid));
    }

    public class Factory : PlaceholderFactory<MoveBeatmapObjectOnGridFsmState>
    {
    }
  }
}
