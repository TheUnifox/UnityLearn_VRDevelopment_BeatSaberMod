// Decompiled with JetBrains decompiler
// Type: BeatmapDataCache
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading.Tasks;
using UnityEngine;

public class BeatmapDataCache
{
  protected IDifficultyBeatmap difficultyBeatmap;
  protected EnvironmentInfoSO environmentInfo;
  protected PlayerSpecificSettings playerSpecificSettings;
  protected IReadonlyBeatmapData cachedMap;

  public virtual async Task<IReadonlyBeatmapData> GetBeatmapData(
    IDifficultyBeatmap difficultyBeatmap,
    EnvironmentInfoSO environmentInfo,
    PlayerSpecificSettings playerSpecificSettings)
  {
    if (difficultyBeatmap == this.difficultyBeatmap && (Object) environmentInfo == (Object) this.environmentInfo && playerSpecificSettings == this.playerSpecificSettings)
    {
      Debug.Log((object) ("Served beatmap data for " + difficultyBeatmap.level.songName + " from the cache"));
      return this.cachedMap;
    }
    IReadonlyBeatmapData beatmapDataAsync = await difficultyBeatmap.GetBeatmapDataAsync(environmentInfo, playerSpecificSettings);
    if (beatmapDataAsync != null)
    {
      this.difficultyBeatmap = difficultyBeatmap;
      this.environmentInfo = environmentInfo;
      this.playerSpecificSettings = playerSpecificSettings;
      this.cachedMap = beatmapDataAsync;
    }
    return this.cachedMap;
  }
}
