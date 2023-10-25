// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CopyBeatmapDifficultySignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public class CopyBeatmapDifficultySignal
  {
    public readonly BeatmapCharacteristicSO beatmapCharacteristic;
    public readonly BeatmapDifficulty beatmapDifficulty;
    public readonly bool copyNotes;
    public readonly bool copyObstacles;
    public readonly bool copyChains;
    public readonly bool copyArcs;
    public readonly bool copyEvents;

    public CopyBeatmapDifficultySignal(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      bool copyNotes,
      bool copyObstacles,
      bool copyChains,
      bool copyArcs,
      bool copyEvents)
    {
      this.beatmapCharacteristic = beatmapCharacteristic;
      this.beatmapDifficulty = beatmapDifficulty;
      this.copyNotes = copyNotes;
      this.copyObstacles = copyObstacles;
      this.copyChains = copyChains;
      this.copyArcs = copyArcs;
      this.copyEvents = copyEvents;
    }
  }
}
