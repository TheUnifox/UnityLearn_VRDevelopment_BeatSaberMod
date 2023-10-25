// Decompiled with JetBrains decompiler
// Type: CustomLevelLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatmapSaveDataVersion3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CustomLevelLoader : MonoBehaviour
{
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  [SerializeField]
  protected EnvironmentInfoSO _defaultEnvironmentInfo;
  [SerializeField]
  protected EnvironmentInfoSO _defaultAllDirectionsEnvironmentInfo;
  [SerializeField]
  protected EnvironmentsListSO _environmentSceneInfoCollection;
  [SerializeField]
  protected Sprite _defaultPackCover;
  [SerializeField]
  protected Sprite _smallDefaultPackCover;
  [Inject]
  protected readonly CachedMediaAsyncLoader _cachedMediaAsyncLoader;
  [Inject]
  protected readonly AudioClipAsyncLoader _audioClipAsyncLoader;
  public const string kCustomLevelPrefixId = "custom_level_";
  public const string kCustomLevelPackPrefixId = "custom_levelpack_";
  protected readonly BeatmapDataLoader _beatmapDataLoader = new BeatmapDataLoader();

  public virtual void ClearCache() => this._cachedMediaAsyncLoader.ClearCache();

  public virtual async Task<CustomBeatmapLevelPack[]> LoadCustomPreviewBeatmapLevelPacksAsync(
    CustomLevelLoader.CustomPackFolderInfo[] customPackFolderInfos,
    CancellationToken cancellationToken)
  {
    int numberOfPacks = customPackFolderInfos.Length;
    List<CustomBeatmapLevelPack> customBeatmapLevelPacks = new List<CustomBeatmapLevelPack>(numberOfPacks);
    for (int i = 0; i < numberOfPacks; ++i)
    {
      string customLevelPackPath = Path.Combine(CustomLevelPathHelper.baseProjectPath, customPackFolderInfos[i].folderName);
      if (await this.CheckPathExistsAsync(customLevelPackPath))
      {
        CustomBeatmapLevelPack beatmapLevelPack = await this.LoadCustomBeatmapLevelPackAsync(customLevelPackPath, customPackFolderInfos[i].packName, cancellationToken);
        if (beatmapLevelPack != null && beatmapLevelPack.beatmapLevelCollection.beatmapLevels.Count > 0)
          customBeatmapLevelPacks.Add(beatmapLevelPack);
        customLevelPackPath = (string) null;
      }
    }
    return customBeatmapLevelPacks.ToArray();
  }

  public virtual async Task<CustomLevelLoader.CustomPackFolderInfo[]> GetSubFoldersInfosAsync(
    string rootPath,
    CancellationToken cancellationToken)
  {
    rootPath = Path.Combine(CustomLevelPathHelper.baseProjectPath, rootPath);
    if (!await this.CheckPathExistsAsync(rootPath))
      return new CustomLevelLoader.CustomPackFolderInfo[0];
    List<CustomLevelLoader.CustomPackFolderInfo> subDirFolderInfo = new List<CustomLevelLoader.CustomPackFolderInfo>();
    await Task.Run((System.Action) (() =>
    {
      foreach (string directory in Directory.GetDirectories(rootPath))
        subDirFolderInfo.Add(new CustomLevelLoader.CustomPackFolderInfo(Path.GetFullPath(directory), new DirectoryInfo(directory).Name));
    }));
    cancellationToken.ThrowIfCancellationRequested();
    return subDirFolderInfo.ToArray();
  }

  public virtual async Task<CustomBeatmapLevel> LoadCustomBeatmapLevelAsync(
    CustomPreviewBeatmapLevel customPreviewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    StandardLevelInfoSaveData levelInfoSaveData = customPreviewBeatmapLevel.standardLevelInfoSaveData;
    string customLevelPath = customPreviewBeatmapLevel.customLevelPath;
    CustomBeatmapLevel customBeatmapLevel = new CustomBeatmapLevel(customPreviewBeatmapLevel);
    customBeatmapLevel.SetBeatmapLevelData(await this.LoadBeatmapLevelDataAsync(customLevelPath, customBeatmapLevel, levelInfoSaveData, cancellationToken));
    return customBeatmapLevel;
  }

  public virtual async Task<bool> CheckPathExistsAsync(string fullCustomLevelPackPath)
  {
    bool pathExists = true;
    await Task.Run((System.Action) (() => pathExists = Directory.Exists(fullCustomLevelPackPath)));
    return pathExists;
  }

  public virtual async Task<CustomBeatmapLevelPack> LoadCustomBeatmapLevelPackAsync(
    string customLevelPackPath,
    string packName,
    CancellationToken cancellationToken)
  {
    CustomBeatmapLevelCollection beatmapLevelCollection = await this.LoadCustomBeatmapLevelCollectionAsync(customLevelPackPath, cancellationToken);
    return beatmapLevelCollection.beatmapLevels.Count != 0 ? new CustomBeatmapLevelPack("custom_levelpack_" + customLevelPackPath, packName, packName, this._defaultPackCover, this._smallDefaultPackCover, beatmapLevelCollection) : (CustomBeatmapLevelPack) null;
  }

  public virtual async Task<CustomBeatmapLevelCollection> LoadCustomBeatmapLevelCollectionAsync(
    string customLevelPackPath,
    CancellationToken cancellationToken)
  {
    return new CustomBeatmapLevelCollection(await this.LoadCustomPreviewBeatmapLevelsAsync(customLevelPackPath, cancellationToken));
  }

  public virtual async Task<CustomPreviewBeatmapLevel[]> LoadCustomPreviewBeatmapLevelsAsync(
    string customLevelPackPath,
    CancellationToken cancellationToken)
  {
    List<CustomPreviewBeatmapLevel> customPreviewBeatmapLevels = new List<CustomPreviewBeatmapLevel>();
    string[] customLevelPaths = new string[0];
    await Task.Run((System.Action) (() =>
    {
      if (!Directory.Exists(customLevelPackPath))
        return;
      customLevelPaths = Directory.GetDirectories(customLevelPackPath);
    }));
    string[] strArray = customLevelPaths;
    for (int index = 0; index < strArray.Length; ++index)
    {
      string customLevelPath = strArray[index];
      StandardLevelInfoSaveData standardLevelInfoSaveData = await this.LoadCustomLevelInfoSaveDataAsync(customLevelPath, cancellationToken);
      CustomPreviewBeatmapLevel previewBeatmapLevel = await this.LoadCustomPreviewBeatmapLevelAsync(customLevelPath, standardLevelInfoSaveData, cancellationToken);
      if (previewBeatmapLevel != null && previewBeatmapLevel.previewDifficultyBeatmapSets.Count > 0)
        customPreviewBeatmapLevels.Add(previewBeatmapLevel);
      customLevelPath = (string) null;
    }
    strArray = (string[]) null;
    return customPreviewBeatmapLevels.ToArray();
  }

  public virtual async Task<BeatmapLevelData> LoadBeatmapLevelDataAsync(
    string customLevelPath,
    CustomBeatmapLevel customBeatmapLevel,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    CancellationToken cancellationToken)
  {
    IDifficultyBeatmapSet[] difficultyBeatmapSets = await this.LoadDifficultyBeatmapSetsAsync(customLevelPath, customBeatmapLevel, standardLevelInfoSaveData, cancellationToken);
    AudioClip audioClip = await this._audioClipAsyncLoader.LoadSong((IBeatmapLevel) customBeatmapLevel);
    return !((UnityEngine.Object) audioClip == (UnityEngine.Object) null) ? new BeatmapLevelData(audioClip, (IReadOnlyList<IDifficultyBeatmapSet>) difficultyBeatmapSets) : (BeatmapLevelData) null;
  }

  public virtual async Task<IDifficultyBeatmapSet[]> LoadDifficultyBeatmapSetsAsync(
    string customLevelPath,
    CustomBeatmapLevel customBeatmapLevel,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    CancellationToken cancellationToken)
  {
    IDifficultyBeatmapSet[] difficultyBeatmapSets = new IDifficultyBeatmapSet[standardLevelInfoSaveData.difficultyBeatmapSets.Length];
    for (int i = 0; i < difficultyBeatmapSets.Length; ++i)
      difficultyBeatmapSets[i] = await this.LoadDifficultyBeatmapSetAsync(customLevelPath, customBeatmapLevel, standardLevelInfoSaveData, standardLevelInfoSaveData.difficultyBeatmapSets[i], cancellationToken);
    return difficultyBeatmapSets;
  }

  public virtual async Task<IDifficultyBeatmapSet> LoadDifficultyBeatmapSetAsync(
    string customLevelPath,
    CustomBeatmapLevel customBeatmapLevel,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    StandardLevelInfoSaveData.DifficultyBeatmapSet difficultyBeatmapSetSaveData,
    CancellationToken cancellationToken)
  {
    BeatmapCharacteristicSO bySerializedName = this._beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(difficultyBeatmapSetSaveData.beatmapCharacteristicName);
    CustomDifficultyBeatmap[] difficultyBeatmaps = new CustomDifficultyBeatmap[difficultyBeatmapSetSaveData.difficultyBeatmaps.Length];
    CustomDifficultyBeatmapSet difficultyBeatmapSet = new CustomDifficultyBeatmapSet(bySerializedName);
    for (int i = 0; i < difficultyBeatmapSetSaveData.difficultyBeatmaps.Length; ++i)
      difficultyBeatmaps[i] = await this.LoadDifficultyBeatmapAsync(customLevelPath, customBeatmapLevel, difficultyBeatmapSet, standardLevelInfoSaveData, difficultyBeatmapSetSaveData.difficultyBeatmaps[i], cancellationToken);
    difficultyBeatmapSet.SetCustomDifficultyBeatmaps(difficultyBeatmaps);
    return (IDifficultyBeatmapSet) difficultyBeatmapSet;
  }

  public virtual async Task<CustomDifficultyBeatmap> LoadDifficultyBeatmapAsync(
    string customLevelPath,
    CustomBeatmapLevel parentCustomBeatmapLevel,
    CustomDifficultyBeatmapSet parentDifficultyBeatmapSet,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    StandardLevelInfoSaveData.DifficultyBeatmap difficultyBeatmapSaveData,
    CancellationToken cancellationToken)
  {
    BeatmapSaveData beatmapSaveData1;
    BeatmapDataBasicInfo beatmapDataBasicInfo1;
    (await this.LoadBeatmapDataBasicInfoAsync(customLevelPath, difficultyBeatmapSaveData.beatmapFilename, standardLevelInfoSaveData, cancellationToken)).Deconstruct<BeatmapSaveData, BeatmapDataBasicInfo>(out beatmapSaveData1, out beatmapDataBasicInfo1);
    BeatmapSaveData beatmapSaveData2 = beatmapSaveData1;
    BeatmapDataBasicInfo beatmapDataBasicInfo2 = beatmapDataBasicInfo1;
    BeatmapDifficulty difficulty;
    difficultyBeatmapSaveData.difficulty.BeatmapDifficultyFromSerializedName(out difficulty);
    return new CustomDifficultyBeatmap((IBeatmapLevel) parentCustomBeatmapLevel, (IDifficultyBeatmapSet) parentDifficultyBeatmapSet, difficulty, difficultyBeatmapSaveData.difficultyRank, difficultyBeatmapSaveData.noteJumpMovementSpeed, difficultyBeatmapSaveData.noteJumpStartBeatOffset, standardLevelInfoSaveData.beatsPerMinute, beatmapSaveData2, (IBeatmapDataBasicInfo) beatmapDataBasicInfo2);
  }

  public virtual async Task<Tuple<BeatmapSaveData, BeatmapDataBasicInfo>> LoadBeatmapDataBasicInfoAsync(
    string customLevelPath,
    string difficultyFileName,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    CancellationToken cancellationToken)
  {
    Tuple<BeatmapSaveData, BeatmapDataBasicInfo> result = (Tuple<BeatmapSaveData, BeatmapDataBasicInfo>) null;
    await Task.Run((System.Action) (() => result = this.LoadBeatmapDataBasicInfo(customLevelPath, difficultyFileName, standardLevelInfoSaveData)), cancellationToken);
    cancellationToken.ThrowIfCancellationRequested();
    return result;
  }

  public virtual Tuple<BeatmapSaveData, BeatmapDataBasicInfo> LoadBeatmapDataBasicInfo(
    string customLevelPath,
    string difficultyFileName,
    StandardLevelInfoSaveData standardLevelInfoSaveData)
  {
    string path = Path.Combine(customLevelPath, difficultyFileName);
    if (!File.Exists(path))
      return (Tuple<BeatmapSaveData, BeatmapDataBasicInfo>) null;
    BeatmapSaveData beatmapSaveData = BeatmapSaveData.DeserializeFromJSONString(File.ReadAllText(path));
    return new Tuple<BeatmapSaveData, BeatmapDataBasicInfo>(beatmapSaveData, BeatmapDataLoader.GetBeatmapDataBasicInfoFromSaveData(beatmapSaveData));
  }

  public virtual EnvironmentInfoSO LoadEnvironmentInfo(string environmentName, bool allDirections)
  {
    EnvironmentInfoSO environmentInfoSo = this._environmentSceneInfoCollection.GetEnvironmentInfoBySerializedName(environmentName);
    if ((UnityEngine.Object) environmentInfoSo == (UnityEngine.Object) null)
      environmentInfoSo = allDirections ? this._defaultAllDirectionsEnvironmentInfo : this._defaultEnvironmentInfo;
    return environmentInfoSo;
  }

  public virtual async Task<CustomPreviewBeatmapLevel> LoadCustomPreviewBeatmapLevelAsync(
    string customLevelPath,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    CancellationToken cancellationToken)
  {
    try
    {
      string levelID = "custom_level_" + new DirectoryInfo(customLevelPath).Name;
      string songName = standardLevelInfoSaveData.songName;
      string songSubName = standardLevelInfoSaveData.songSubName;
      string songAuthorName = standardLevelInfoSaveData.songAuthorName;
      string levelAuthorName = standardLevelInfoSaveData.levelAuthorName;
      float beatsPerMinute = standardLevelInfoSaveData.beatsPerMinute;
      float songTimeOffset = standardLevelInfoSaveData.songTimeOffset;
      float shuffle = standardLevelInfoSaveData.shuffle;
      float shufflePeriod = standardLevelInfoSaveData.shufflePeriod;
      float previewStartTime = standardLevelInfoSaveData.previewStartTime;
      float previewDuration = standardLevelInfoSaveData.previewDuration;
      EnvironmentInfoSO environmentInfo = this.LoadEnvironmentInfo(standardLevelInfoSaveData.environmentName, false);
      EnvironmentInfoSO allDirectionsEnvironmentInfo = this.LoadEnvironmentInfo(standardLevelInfoSaveData.allDirectionsEnvironmentName, true);
      List<PreviewDifficultyBeatmapSet> difficultyBeatmapSetList = new List<PreviewDifficultyBeatmapSet>();
      foreach (StandardLevelInfoSaveData.DifficultyBeatmapSet difficultyBeatmapSet in standardLevelInfoSaveData.difficultyBeatmapSets)
      {
        BeatmapCharacteristicSO bySerializedName = this._beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(difficultyBeatmapSet.beatmapCharacteristicName);
        if ((UnityEngine.Object) bySerializedName != (UnityEngine.Object) null)
        {
          BeatmapDifficulty[] beatmapDifficulties = new BeatmapDifficulty[difficultyBeatmapSet.difficultyBeatmaps.Length];
          for (int index = 0; index < difficultyBeatmapSet.difficultyBeatmaps.Length; ++index)
          {
            BeatmapDifficulty difficulty;
            difficultyBeatmapSet.difficultyBeatmaps[index].difficulty.BeatmapDifficultyFromSerializedName(out difficulty);
            beatmapDifficulties[index] = difficulty;
          }
          difficultyBeatmapSetList.Add(new PreviewDifficultyBeatmapSet(bySerializedName, beatmapDifficulties));
        }
      }
      return await Task.FromResult<CustomPreviewBeatmapLevel>(new CustomPreviewBeatmapLevel(this._defaultPackCover, standardLevelInfoSaveData, customLevelPath, (ISpriteAsyncLoader) this._cachedMediaAsyncLoader, levelID, songName, songSubName, songAuthorName, levelAuthorName, beatsPerMinute, songTimeOffset, shuffle, shufflePeriod, previewStartTime, previewDuration, environmentInfo, allDirectionsEnvironmentInfo, (IReadOnlyList<PreviewDifficultyBeatmapSet>) difficultyBeatmapSetList.ToArray()));
    }
    catch
    {
      return (CustomPreviewBeatmapLevel) null;
    }
  }

  public virtual async Task<StandardLevelInfoSaveData> LoadCustomLevelInfoSaveDataAsync(
    string customLevelPath,
    CancellationToken cancellationToken)
  {
    StandardLevelInfoSaveData customLevelInfoSaveData = (StandardLevelInfoSaveData) null;
    await Task.Run((System.Action) (() => customLevelInfoSaveData = this.LoadCustomLevelInfoSaveData(customLevelPath)), cancellationToken);
    cancellationToken.ThrowIfCancellationRequested();
    return customLevelInfoSaveData;
  }

  public virtual StandardLevelInfoSaveData LoadCustomLevelInfoSaveData(string customLevelPath)
  {
    string path = Path.Combine(customLevelPath, "Info.dat");
    return File.Exists(path) ? StandardLevelInfoSaveData.DeserializeFromJSONString(File.ReadAllText(path)) : (StandardLevelInfoSaveData) null;
  }

  public struct CustomPackFolderInfo
  {
    public readonly string folderName;
    public readonly string packName;

    public CustomPackFolderInfo(string folderName, string packName)
    {
      this.folderName = folderName;
      this.packName = packName;
    }
  }
}
