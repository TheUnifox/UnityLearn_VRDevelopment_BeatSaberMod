// Decompiled with JetBrains decompiler
// Type: BeatmapLevelDataExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public static class BeatmapLevelDataExtensions
{
  public static IDifficultyBeatmap GetDifficultyBeatmap(
    this IBeatmapLevelData beatmapLevelData,
    BeatmapCharacteristicSO beatmapCharacteristic,
    BeatmapDifficulty difficulty)
  {
    if (beatmapLevelData != null)
    {
      IDifficultyBeatmapSet difficultyBeatmapSet = beatmapLevelData.GetDifficultyBeatmapSet(beatmapCharacteristic);
      if (difficultyBeatmapSet != null)
      {
        foreach (IDifficultyBeatmap difficultyBeatmap in (IEnumerable<IDifficultyBeatmap>) difficultyBeatmapSet.difficultyBeatmaps)
        {
          if (difficultyBeatmap.difficulty == difficulty)
            return difficultyBeatmap;
        }
      }
    }
    return (IDifficultyBeatmap) null;
  }

  public static IDifficultyBeatmapSet GetDifficultyBeatmapSet(
    this IBeatmapLevelData beatmapLevelData,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    if (beatmapLevelData != null && beatmapLevelData.difficultyBeatmapSets != null)
    {
      foreach (IDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<IDifficultyBeatmapSet>) beatmapLevelData.difficultyBeatmapSets)
      {
        if ((Object) difficultyBeatmapSet.beatmapCharacteristic == (Object) beatmapCharacteristic)
          return difficultyBeatmapSet;
      }
    }
    return (IDifficultyBeatmapSet) null;
  }

  public static IDifficultyBeatmap GetDifficultyBeatmap(
    this IBeatmapLevelData beatmapLevelData,
    PreviewDifficultyBeatmap previewDifficultyBeatmap)
  {
    return beatmapLevelData.GetDifficultyBeatmap(previewDifficultyBeatmap.beatmapCharacteristic, previewDifficultyBeatmap.beatmapDifficulty);
  }

  public static IDifficultyBeatmapSet GetDifficultyBeatmapSet(
    this IBeatmapLevelData beatmapLevelData,
    string beatmapCharacteristicName)
  {
    if (beatmapLevelData != null && beatmapLevelData.difficultyBeatmapSets != null)
    {
      foreach (IDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<IDifficultyBeatmapSet>) beatmapLevelData.difficultyBeatmapSets)
      {
        if (difficultyBeatmapSet.beatmapCharacteristic.serializedName == beatmapCharacteristicName)
          return difficultyBeatmapSet;
      }
    }
    return (IDifficultyBeatmapSet) null;
  }
}
