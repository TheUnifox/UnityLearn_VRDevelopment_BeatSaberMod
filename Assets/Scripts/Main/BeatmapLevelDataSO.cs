// Decompiled with JetBrains decompiler
// Type: BeatmapLevelDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class BeatmapLevelDataSO : PersistentScriptableObject
{
  [SerializeField]
  protected AudioClip _audioClip;
  [SerializeField]
  protected BeatmapLevelDataSO.DifficultyBeatmapSet[] _difficultyBeatmapSets;
  protected BeatmapLevelDataSO.DifficultyBeatmapSet[] _no360MovementDifficultyBeatmapSets;

  public AudioClip audioClip => this._audioClip;

  public BeatmapLevelDataSO.DifficultyBeatmapSet[] difficultyBeatmapSets => this._difficultyBeatmapSets;

  [Serializable]
  public class DifficultyBeatmapSet
  {
    [SerializeField]
    protected string _beatmapCharacteristicSerializedName;
    [SerializeField]
    protected BeatmapLevelSO.DifficultyBeatmap[] _difficultyBeatmaps;

    public string beatmapCharacteristicSerializedName => this._beatmapCharacteristicSerializedName;

    public BeatmapLevelSO.DifficultyBeatmap[] difficultyBeatmaps => this._difficultyBeatmaps;

    public DifficultyBeatmapSet(
      string beatmapCharacteristicSerializedName,
      BeatmapLevelSO.DifficultyBeatmap[] difficultyBeatmaps)
    {
      this._beatmapCharacteristicSerializedName = beatmapCharacteristicSerializedName;
      this._difficultyBeatmaps = difficultyBeatmaps;
    }
  }
}
