// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectGridFsmStateHidden
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectGridFsmStateHidden : BeatmapObjectGridFsmState
  {
    public override void Enter()
    {
      this.signalBus.Fire<ChangeHoverSignal>(new ChangeHoverSignal((BeatmapObjectCellData) null, ChangeHoverSignal.HoverOrigin.Grid));
      this.hoverView.HidePreview();
      this.beatmapObjectEditGridView.Hide();
      this.beatmapObjectEditGridView.ResetHighlights();
    }

    public override void Exit() => this.beatmapObjectEditGridView.Show();

    public class Factory : PlaceholderFactory<BeatmapObjectGridFsmStateHidden>
    {
    }
  }
}
