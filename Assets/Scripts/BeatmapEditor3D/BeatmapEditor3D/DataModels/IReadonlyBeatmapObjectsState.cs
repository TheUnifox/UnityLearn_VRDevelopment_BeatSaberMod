// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IReadonlyBeatmapObjectsState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;

namespace BeatmapEditor3D.DataModels
{
  public interface IReadonlyBeatmapObjectsState
  {
    BeatmapObjectType beatmapObjectType { get; }

    BeatmapEditorObjectId hoveredBeatmapObjectId { get; }

    BeatmapObjectCellData hoveredCellData { get; }

    BeatmapObjectCellData hoveredNoteCellData { get; }

    BeatmapObjectCellData hoveredGridCellData { get; }

    float playStartBeat { get; }

    ColorType noteColorType { get; }

    NoteCutDirection noteCutDirection { get; }

    int noteAngle { get; }

    float obstacleDuration { get; }

    int sliderSliceCount { get; }

    float sliderSquishAmount { get; }

    SliderMidAnchorMode arcMidAnchorMode { get; }

    SliderData.Type sliderType { get; }

    float arcControlPointLengthMultiplier { get; }

    float arcTailControlPointLengthMultiplier { get; }

    void Reset();
  }
}
