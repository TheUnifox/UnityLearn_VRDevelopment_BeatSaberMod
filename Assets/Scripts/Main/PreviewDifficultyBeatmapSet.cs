// Decompiled with JetBrains decompiler
// Type: PreviewDifficultyBeatmapSet
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class PreviewDifficultyBeatmapSet
{
  [SerializeField]
  protected BeatmapCharacteristicSO _beatmapCharacteristic;
  [SerializeField]
  protected BeatmapDifficulty[] _beatmapDifficulties;

  public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

  public BeatmapDifficulty[] beatmapDifficulties => this._beatmapDifficulties;

  public PreviewDifficultyBeatmapSet(
    BeatmapCharacteristicSO beatmapCharacteristic,
    BeatmapDifficulty[] beatmapDifficulties)
  {
    this._beatmapCharacteristic = beatmapCharacteristic;
    this._beatmapDifficulties = beatmapDifficulties;
  }
}
