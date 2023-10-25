// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmMoveRegionsSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmMoveRegionsSignal
  {
    public readonly int leftRegionIdx;
    public readonly BpmRegion leftRegion;
    public readonly int rightRegionIdx;
    public readonly BpmRegion rightRegion;
    public readonly int sample;

    public BpmMoveRegionsSignal(
      int leftRegionIdx,
      BpmRegion leftRegion,
      int rightRegionIdx,
      BpmRegion rightRegion,
      int sample)
    {
      this.leftRegionIdx = leftRegionIdx;
      this.leftRegion = leftRegion;
      this.rightRegionIdx = rightRegionIdx;
      this.rightRegion = rightRegion;
      this.sample = sample;
    }
  }
}
