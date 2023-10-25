// Decompiled with JetBrains decompiler
// Type: PreviewDifficultyBeatmapSetExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public static class PreviewDifficultyBeatmapSetExtensions
{
  public static BeatmapCharacteristicSO[] GetBeatmapCharacteristics(
    this PreviewDifficultyBeatmapSet[] previewDifficultyBeatmapSet)
  {
    if (previewDifficultyBeatmapSet == null)
      return (BeatmapCharacteristicSO[]) null;
    BeatmapCharacteristicSO[] beatmapCharacteristics = new BeatmapCharacteristicSO[previewDifficultyBeatmapSet.Length];
    for (int index = 0; index < previewDifficultyBeatmapSet.Length; ++index)
      beatmapCharacteristics[index] = previewDifficultyBeatmapSet[index].beatmapCharacteristic;
    return beatmapCharacteristics;
  }

  public static IReadOnlyList<PreviewDifficultyBeatmapSet> GetPreviewDifficultyBeatmapSetWithout360Movement(
    this IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSet)
  {
    if (previewDifficultyBeatmapSet == null)
      return (IReadOnlyList<PreviewDifficultyBeatmapSet>) null;
    List<PreviewDifficultyBeatmapSet> difficultyBeatmapSetList = new List<PreviewDifficultyBeatmapSet>((IEnumerable<PreviewDifficultyBeatmapSet>) previewDifficultyBeatmapSet);
    int index = 0;
    while (index < difficultyBeatmapSetList.Count)
    {
      if (difficultyBeatmapSetList[index].beatmapCharacteristic.requires360Movement)
        difficultyBeatmapSetList.RemoveAt(index);
      else
        ++index;
    }
    return (IReadOnlyList<PreviewDifficultyBeatmapSet>) difficultyBeatmapSetList.ToArray();
  }
}
