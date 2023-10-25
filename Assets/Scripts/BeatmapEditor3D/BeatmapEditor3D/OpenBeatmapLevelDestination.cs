// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.OpenBeatmapLevelDestination
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public class OpenBeatmapLevelDestination : MenuDestination
  {
    public readonly string projectPath;
    public readonly bool ignoreTemp;
    public readonly BeatmapCharacteristicSO beatmapCharacteristic;
    public readonly BeatmapDifficulty beatmapDifficulty;

    public OpenBeatmapLevelDestination(
      string projectPath,
      bool ignoreTemp,
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      this.projectPath = projectPath;
      this.ignoreTemp = ignoreTemp;
      this.beatmapCharacteristic = beatmapCharacteristic;
      this.beatmapDifficulty = beatmapDifficulty;
    }
  }
}
