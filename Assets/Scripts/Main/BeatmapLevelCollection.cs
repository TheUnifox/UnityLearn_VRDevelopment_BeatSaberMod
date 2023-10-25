// Decompiled with JetBrains decompiler
// Type: BeatmapLevelCollection
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class BeatmapLevelCollection : IBeatmapLevelCollection
{
  protected readonly IReadOnlyList<IPreviewBeatmapLevel> _levels;

  public IReadOnlyList<IPreviewBeatmapLevel> beatmapLevels => this._levels;

  public BeatmapLevelCollection(IReadOnlyList<IPreviewBeatmapLevel> levels) => this._levels = levels;

  public static BeatmapLevelCollection CreateBeatmapLevelCollectionByUsingBeatmapCharacteristicFiltering(
    IBeatmapLevelCollection beatmapLevelCollection,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    IReadOnlyList<IPreviewBeatmapLevel> beatmapLevels = beatmapLevelCollection.beatmapLevels;
    List<IPreviewBeatmapLevel> levels = new List<IPreviewBeatmapLevel>();
    foreach (IPreviewBeatmapLevel previewBeatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) beatmapLevels)
    {
      foreach (PreviewDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<PreviewDifficultyBeatmapSet>) previewBeatmapLevel.previewDifficultyBeatmapSets)
      {
        if ((Object) difficultyBeatmapSet.beatmapCharacteristic == (Object) beatmapCharacteristic)
        {
          levels.Add(previewBeatmapLevel);
          break;
        }
      }
    }
    return new BeatmapLevelCollection((IReadOnlyList<IPreviewBeatmapLevel>) levels);
  }
}
