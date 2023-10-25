// Decompiled with JetBrains decompiler
// Type: DifficultyBeatmapSetExtensions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public static class DifficultyBeatmapSetExtensions
{
  public static IReadOnlyList<T> GetDifficultyBeatmapSetsWithout360Movement<T>(
    this IReadOnlyList<T> difficultyBeatmapSets)
    where T : IDifficultyBeatmapSet
  {
    List<T> without360Movement = new List<T>((IEnumerable<T>) difficultyBeatmapSets);
    int index = 0;
    while (index < without360Movement.Count)
    {
      if (without360Movement[index].beatmapCharacteristic.requires360Movement)
        without360Movement.RemoveAt(index);
      else
        ++index;
    }
    return (IReadOnlyList<T>) without360Movement;
  }

  public static IReadOnlyList<PreviewDifficultyBeatmapSet> GetPreviewDifficultyBeatmapSets<T>(
    this IReadOnlyList<T> difficultyBeatmapSets)
    where T : IDifficultyBeatmapSet
  {
    if (difficultyBeatmapSets == null)
      return (IReadOnlyList<PreviewDifficultyBeatmapSet>) null;
    PreviewDifficultyBeatmapSet[] difficultyBeatmapSets1 = new PreviewDifficultyBeatmapSet[difficultyBeatmapSets.Count];
    for (int index1 = 0; index1 < difficultyBeatmapSets.Count; ++index1)
    {
      T difficultyBeatmapSet = difficultyBeatmapSets[index1];
      BeatmapDifficulty[] beatmapDifficulties = new BeatmapDifficulty[difficultyBeatmapSet.difficultyBeatmaps.Count];
      for (int index2 = 0; index2 < difficultyBeatmapSet.difficultyBeatmaps.Count; ++index2)
        beatmapDifficulties[index2] = difficultyBeatmapSet.difficultyBeatmaps[index2].difficulty;
      difficultyBeatmapSets1[index1] = new PreviewDifficultyBeatmapSet(difficultyBeatmapSet.beatmapCharacteristic, beatmapDifficulties);
    }
    return (IReadOnlyList<PreviewDifficultyBeatmapSet>) difficultyBeatmapSets1;
  }
}
