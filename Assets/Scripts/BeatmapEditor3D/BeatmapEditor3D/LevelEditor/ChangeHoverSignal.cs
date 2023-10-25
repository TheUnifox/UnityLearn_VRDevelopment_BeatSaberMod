// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeHoverSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeHoverSignal
  {
    public readonly BeatmapEditorObjectId hoveredId;
    public readonly BeatmapObjectCellData hoveredCellData;
    public readonly ChangeHoverSignal.HoverOrigin hoveredCellOrigin;

    public ChangeHoverSignal(
      BeatmapObjectCellData hoveredCellData,
      ChangeHoverSignal.HoverOrigin hoveredCellOrigin)
    {
      this.hoveredCellData = hoveredCellData;
      this.hoveredCellOrigin = hoveredCellOrigin;
    }

    public ChangeHoverSignal(
      BeatmapEditorObjectId hoveredId,
      BeatmapObjectCellData hoveredCellData,
      ChangeHoverSignal.HoverOrigin hoveredCellOrigin)
    {
      this.hoveredId = hoveredId;
      this.hoveredCellData = hoveredCellData;
      this.hoveredCellOrigin = hoveredCellOrigin;
    }

    public enum HoverOrigin
    {
      Object,
      Grid,
    }
  }
}
