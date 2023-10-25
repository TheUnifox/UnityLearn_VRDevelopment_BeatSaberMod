// Decompiled with JetBrains decompiler
// Type: BeatmapLevelData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BeatmapLevelData : IBeatmapLevelData
{
  protected readonly AudioClip _audioClip;
  protected readonly IReadOnlyList<IDifficultyBeatmapSet> _difficultyBeatmapSets;

  public AudioClip audioClip => this._audioClip;

  public IReadOnlyList<IDifficultyBeatmapSet> difficultyBeatmapSets => this._difficultyBeatmapSets;

  public BeatmapLevelData(
    AudioClip audioClip,
    IReadOnlyList<IDifficultyBeatmapSet> difficultyBeatmapSets)
  {
    this._audioClip = audioClip;
    this._difficultyBeatmapSets = difficultyBeatmapSets;
  }
}
