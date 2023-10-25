// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.UpdatePlayHeadSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public class UpdatePlayHeadSignal
  {
    public UpdatePlayHeadSignal.UpdateType updateType { get; }

    public int sample { get; }

    public float beat { get; }

    public UpdatePlayHeadSignal.SnapType snapType { get; }

    public bool restartPlayback { get; }

    public UpdatePlayHeadSignal(
      float beat,
      UpdatePlayHeadSignal.SnapType snapType,
      bool restartPlayback = false)
    {
      this.updateType = UpdatePlayHeadSignal.UpdateType.Beat;
      this.beat = beat;
      this.snapType = snapType;
      this.restartPlayback = restartPlayback;
    }

    public UpdatePlayHeadSignal(
      int sample,
      UpdatePlayHeadSignal.SnapType snapType,
      bool restartPlayback = false)
    {
      this.updateType = UpdatePlayHeadSignal.UpdateType.Sample;
      this.sample = sample;
      this.snapType = snapType;
      this.restartPlayback = restartPlayback;
    }

    public enum UpdateType
    {
      Sample,
      Beat,
    }

    public enum SnapType
    {
      None,
      CurrentSubdivision,
      SmallestSubdivision,
    }
  }
}
