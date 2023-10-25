// Decompiled with JetBrains decompiler
// Type: DifficultyBeatmapSet
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class DifficultyBeatmapSet : IDifficultyBeatmapSet
{
  [CompilerGenerated]
  protected readonly BeatmapCharacteristicSO m_CbeatmapCharacteristic;
  [CompilerGenerated]
  protected readonly IReadOnlyList<IDifficultyBeatmap> m_CdifficultyBeatmaps;

  public BeatmapCharacteristicSO beatmapCharacteristic => this.m_CbeatmapCharacteristic;

  public IReadOnlyList<IDifficultyBeatmap> difficultyBeatmaps => this.m_CdifficultyBeatmaps;

  public DifficultyBeatmapSet(
    BeatmapCharacteristicSO beatmapCharacteristic,
    IReadOnlyList<IDifficultyBeatmap> difficultyBeatmaps)
  {
    this.m_CbeatmapCharacteristic = beatmapCharacteristic;
    this.m_CdifficultyBeatmaps = difficultyBeatmaps;
  }
}
