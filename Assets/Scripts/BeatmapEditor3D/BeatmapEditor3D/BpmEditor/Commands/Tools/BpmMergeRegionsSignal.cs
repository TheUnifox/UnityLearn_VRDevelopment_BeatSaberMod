// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmMergeRegionsSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmMergeRegionsSignal
  {
    public readonly int leftRegionIdx;
    public readonly int rightRegionIdx;
    public readonly int keepRegionIdx;

    public BpmMergeRegionsSignal(int leftRegionIdx, int rightRegionIdx, int keepRegionIdx)
    {
      this.leftRegionIdx = leftRegionIdx;
      this.rightRegionIdx = rightRegionIdx;
      this.keepRegionIdx = keepRegionIdx;
    }
  }
}
