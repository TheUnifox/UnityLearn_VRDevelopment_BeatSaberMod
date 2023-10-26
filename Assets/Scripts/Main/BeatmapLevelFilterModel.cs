// Decompiled with JetBrains decompiler
// Type: BeatmapLevelFilterModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public abstract class BeatmapLevelFilterModel
{
  public static async Task<IBeatmapLevelCollection> FilerBeatmapLevelPackCollectionAsync(
    IBeatmapLevelPack[] beatmapLevelPacks,
    LevelFilterParams levelFilterParams,
    PlayerDataModel playerDataModel,
    AdditionalContentModel additionalContentModel,
    CancellationToken cancellationToken)
  {
    IEnumerable<IPreviewBeatmapLevel> beatmapLevelsAsync = (IEnumerable<IPreviewBeatmapLevel>) await BeatmapLevelFilterModel.GetAllBeatmapLevelsAsync(beatmapLevelPacks, levelFilterParams.filterBySongPacks ? levelFilterParams.filteredSongPacks : SongPackMask.all, levelFilterParams.filterByOwned, levelFilterParams.filterByNotOwned, additionalContentModel, cancellationToken);
    bool searchByText = !string.IsNullOrWhiteSpace(levelFilterParams.searchText);
    string[] strArray;
    if (!searchByText)
      strArray = (string[]) null;
    else
      strArray = levelFilterParams.searchText.Split(' ');
    string[] searchTexts = strArray;
    return (IBeatmapLevelCollection) new BeatmapLevelCollection((IReadOnlyList<IPreviewBeatmapLevel>) beatmapLevelsAsync.Where<IPreviewBeatmapLevel>((Func<IPreviewBeatmapLevel, bool>) (beatmapLevel =>
    {
      if (searchByText && !BeatmapLevelFilterModel.LevelContainsText(beatmapLevel, searchTexts) || levelFilterParams.filterByLevelIds && !levelFilterParams.beatmapLevelIds.Contains(beatmapLevel.levelID) || levelFilterParams.filterByMinBpm && (double) beatmapLevel.beatsPerMinute < (double) levelFilterParams.filteredMinBpm || levelFilterParams.filterByMaxBpm && (double) beatmapLevel.beatsPerMinute > (double) levelFilterParams.filteredMaxBpm)
        return false;
      bool flag = false;
      foreach (PreviewDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<PreviewDifficultyBeatmapSet>) beatmapLevel.previewDifficultyBeatmapSets)
      {
        if (!levelFilterParams.filterByCharacteristic || !((UnityEngine.Object) difficultyBeatmapSet.beatmapCharacteristic != (UnityEngine.Object) levelFilterParams.filteredCharacteristic))
        {
          if (!levelFilterParams.filterByDifficulty && !levelFilterParams.filterByNotPlayedYet)
            return true;
          foreach (BeatmapDifficulty beatmapDifficulty in difficultyBeatmapSet.beatmapDifficulties)
          {
            if (levelFilterParams.filterByNotPlayedYet && playerDataModel.playerData.GetPlayerLevelStatsData(beatmapLevel.levelID, beatmapDifficulty, difficultyBeatmapSet.beatmapCharacteristic).playCount > 0)
              return false;
            if (!levelFilterParams.filterByDifficulty || levelFilterParams.filteredDifficulty.Contains(beatmapDifficulty))
              flag = true;
          }
        }
      }
      return flag;
    })).ToArray<IPreviewBeatmapLevel>());
  }

  private static bool LevelContainsText(IPreviewBeatmapLevel beatmapLevel, string[] searchTexts)
  {
    foreach (string searchText in searchTexts)
    {
      if (!string.IsNullOrWhiteSpace(searchText) && (beatmapLevel.songName.IndexOf(searchText, 0, StringComparison.CurrentCultureIgnoreCase) != -1 || beatmapLevel.songSubName.IndexOf(searchText, 0, StringComparison.CurrentCultureIgnoreCase) != -1 || beatmapLevel.songAuthorName.IndexOf(searchText, 0, StringComparison.CurrentCultureIgnoreCase) != -1))
        return true;
    }
    return false;
  }

  private static async Task<List<IPreviewBeatmapLevel>> GetAllBeatmapLevelsAsync(
    IBeatmapLevelPack[] beatmapLevelPacks,
    SongPackMask songPackMask,
    bool onlyOwned,
    bool onlyNotOwned,
    AdditionalContentModel additionalContentModel,
    CancellationToken cancellationToken)
  {
    List<IPreviewBeatmapLevel> levels = new List<IPreviewBeatmapLevel>();
    IBeatmapLevelPack[] beatmapLevelPackArray = beatmapLevelPacks;
    for (int index = 0; index < beatmapLevelPackArray.Length; ++index)
    {
      IBeatmapLevelPack beatmapLevelPack = beatmapLevelPackArray[index];
      if (songPackMask.Contains((SongPackMask) beatmapLevelPack.packID))
      {
        foreach (IPreviewBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) beatmapLevelPack.beatmapLevelCollection.beatmapLevels)
        {
          if (onlyOwned)
          {
            if (await additionalContentModel.GetLevelEntitlementStatusAsync(beatmapLevel.levelID, cancellationToken) != AdditionalContentModel.EntitlementStatus.Owned)
              continue;
          }
          if (onlyNotOwned)
          {
            if (await additionalContentModel.GetLevelEntitlementStatusAsync(beatmapLevel.levelID, cancellationToken) == AdditionalContentModel.EntitlementStatus.Owned)
              continue;
          }
          levels.Add(beatmapLevel);
        }
      }
    }
    beatmapLevelPackArray = (IBeatmapLevelPack[]) null;
    return levels;
  }
}
