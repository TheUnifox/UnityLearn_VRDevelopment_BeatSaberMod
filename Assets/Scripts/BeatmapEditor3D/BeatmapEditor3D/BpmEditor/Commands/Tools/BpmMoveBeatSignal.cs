// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.Commands.Tools.BpmMoveBeatSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D.BpmEditor.Commands.Tools
{
  public class BpmMoveBeatSignal
  {
    public readonly int regionIdx;
    public readonly BpmRegion region;
    public readonly int beat;
    public readonly int newSample;

    public BpmMoveBeatSignal(int regionIdx, BpmRegion region, int beat, int newSample)
    {
      this.regionIdx = regionIdx;
      this.region = region;
      this.beat = beat;
      this.newSample = newSample;
    }
  }
}
