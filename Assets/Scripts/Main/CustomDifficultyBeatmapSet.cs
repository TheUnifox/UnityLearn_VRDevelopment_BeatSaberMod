// Decompiled with JetBrains decompiler
// Type: CustomDifficultyBeatmapSet
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class CustomDifficultyBeatmapSet : IDifficultyBeatmapSet
{
  protected readonly BeatmapCharacteristicSO _beatmapCharacteristic;
  protected IReadOnlyList<CustomDifficultyBeatmap> _difficultyBeatmaps;

  public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

  public IReadOnlyList<IDifficultyBeatmap> difficultyBeatmaps => (IReadOnlyList<IDifficultyBeatmap>) this._difficultyBeatmaps;

  public CustomDifficultyBeatmapSet(BeatmapCharacteristicSO beatmapCharacteristic) => this._beatmapCharacteristic = beatmapCharacteristic;

  public virtual void SetCustomDifficultyBeatmaps(CustomDifficultyBeatmap[] difficultyBeatmaps) => this._difficultyBeatmaps = (IReadOnlyList<CustomDifficultyBeatmap>) difficultyBeatmaps;
}
