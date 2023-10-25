// Decompiled with JetBrains decompiler
// Type: BeatmapLevelDataLoaderSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BeatmapLevelDataLoaderSO : PersistentScriptableObject
{
  [SerializeField]
  protected BeatmapCharacteristicCollectionSO _allBeatmapCharacteristicCollection;
  protected AsyncCache<string, IBeatmapLevel> _beatmapLevelsAsyncCache;
  protected readonly Dictionary<string, BeatmapLevelDataLoaderSO.AssetBundleLevelInfo> _bundleLevelInfos = new Dictionary<string, BeatmapLevelDataLoaderSO.AssetBundleLevelInfo>();

  public virtual async Task<IBeatmapLevel> LoadBeatmapLevelFormAssetBundleAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    string assetBundlePath,
    string levelDataAssetName,
    CancellationToken cancellationToken)
  {
    BeatmapLevelDataLoaderSO levelDataLoaderSo = this;
    string levelID = previewBeatmapLevel.levelID;
    if (!levelDataLoaderSo._bundleLevelInfos.ContainsKey(levelID))
    {
      BeatmapLevelDataLoaderSO.AssetBundleLevelInfo assetBundleLevelInfo = new BeatmapLevelDataLoaderSO.AssetBundleLevelInfo(assetBundlePath, levelDataAssetName, previewBeatmapLevel);
      levelDataLoaderSo._bundleLevelInfos[levelID] = assetBundleLevelInfo;
    }
    if (levelDataLoaderSo._beatmapLevelsAsyncCache == null)
      levelDataLoaderSo._beatmapLevelsAsyncCache = new AsyncCache<string, IBeatmapLevel>(new Func<string, Task<IBeatmapLevel>>(levelDataLoaderSo.LoadBeatmapLevelAsync));
    IBeatmapLevel beatmapLevel = await levelDataLoaderSo._beatmapLevelsAsyncCache[levelID];
    if (beatmapLevel == null)
      levelDataLoaderSo._beatmapLevelsAsyncCache.RemoveKey(levelID);
    cancellationToken.ThrowIfCancellationRequested();
    return beatmapLevel;
  }

  public virtual async Task<IBeatmapLevel> LoadBeatmapLevelAsync(string levelID)
  {
    int num;
    if (num != 0 && !this._bundleLevelInfos.ContainsKey(levelID))
      return (IBeatmapLevel) null;
    try
    {
      BeatmapLevelDataSO beatmapLevelData = await this.LoadBeatmapLevelDataAsync(this._bundleLevelInfos[levelID].assetBundlePath, this._bundleLevelInfos[levelID].levelDataAssetName);
      BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview levelFromPreview = new BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview(this._bundleLevelInfos[levelID].previewBeatmapLevel);
      levelFromPreview.LoadData(this._allBeatmapCharacteristicCollection, beatmapLevelData);
      return (IBeatmapLevel) levelFromPreview;
    }
    catch
    {
      return (IBeatmapLevel) null;
    }
  }

  public virtual async Task<BeatmapLevelDataSO> LoadBeatmapLevelDataAsync(
    string assetBundlePath,
    string levelDataAssetName)
  {
    TaskCompletionSource<BeatmapLevelDataSO> taskSource = new TaskCompletionSource<BeatmapLevelDataSO>();
    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(assetBundlePath);
    assetBundleCreateRequest.completed += (System.Action<AsyncOperation>) (asyncOperation =>
    {
      AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;
      try
      {
        AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync<BeatmapLevelDataSO>(levelDataAssetName);
        assetBundleRequest.completed += (System.Action<AsyncOperation>) (asyncOperation2 =>
        {
          BeatmapLevelDataSO asset = (BeatmapLevelDataSO) assetBundleRequest.asset;
          if ((UnityEngine.Object) asset == (UnityEngine.Object) null || (UnityEngine.Object) asset.audioClip == (UnityEngine.Object) null)
            assetBundle.Unload(true);
          taskSource.TrySetResult(asset);
        });
      }
      catch
      {
        taskSource.TrySetResult((BeatmapLevelDataSO) null);
      }
    });
    return await taskSource.Task;
  }

  public readonly struct AssetBundleLevelInfo
  {
    public readonly string assetBundlePath;
    public readonly string levelDataAssetName;
    public readonly IPreviewBeatmapLevel previewBeatmapLevel;

    public AssetBundleLevelInfo(
      string assetBundlePath,
      string levelDataAssetName,
      IPreviewBeatmapLevel previewBeatmapLevel)
    {
      this.assetBundlePath = assetBundlePath;
      this.levelDataAssetName = levelDataAssetName;
      this.previewBeatmapLevel = previewBeatmapLevel;
    }
  }

  public class BeatmapLevelFromPreview : 
    IBeatmapLevel,
    IPreviewBeatmapLevel,
    IAssetSongPreviewAudioClipProvider,
    IFilePathSongPreviewAudioClipProvider
  {
    protected readonly IPreviewBeatmapLevel _level;
    protected BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview.BeatmapLevelData _beatmapLevelData;

    public string levelID => this._level.levelID;

    public string songName => this._level.songName;

    public string songSubName => this._level.songSubName;

    public string songAuthorName => this._level.songAuthorName;

    public string levelAuthorName => this._level.levelAuthorName;

    public float beatsPerMinute => this._level.beatsPerMinute;

    public float songTimeOffset => this._level.songTimeOffset;

    public float songDuration => this._level.songDuration;

    public float shuffle => this._level.shuffle;

    public float shufflePeriod => this._level.shufflePeriod;

    public float previewStartTime => this._level.previewStartTime;

    public float previewDuration => this._level.previewDuration;

    public EnvironmentInfoSO environmentInfo => this._level.environmentInfo;

    public EnvironmentInfoSO allDirectionsEnvironmentInfo => this._level.allDirectionsEnvironmentInfo;

    public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => this._level.previewDifficultyBeatmapSets;

    public IBeatmapLevelData beatmapLevelData => (IBeatmapLevelData) this._beatmapLevelData;

    public AudioClip songPreviewAudioClip => !(this._level is IAssetSongPreviewAudioClipProvider level) ? (AudioClip) null : level.songPreviewAudioClip;

    public string songPreviewAudioClipPath => !(this._level is IFilePathSongPreviewAudioClipProvider level) ? string.Empty : level.songPreviewAudioClipPath;

    public virtual async Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken) => await this._level.GetCoverImageAsync(cancellationToken);

    public BeatmapLevelFromPreview(IPreviewBeatmapLevel previewLevel) => this._level = previewLevel;

    public virtual void LoadData(
      BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection,
      BeatmapLevelDataSO beatmapLevelData)
    {
      this._beatmapLevelData = new BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview.BeatmapLevelData(beatmapLevelData, beatmapCharacteristicCollection, (IBeatmapLevel) this);
    }

    public class BeatmapLevelData : IBeatmapLevelData
    {
      protected readonly AudioClip _audioClip;
      protected readonly List<BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview.BeatmapLevelData.DifficultyBeatmapSet> _difficultyBeatmapSets;

      public AudioClip audioClip => this._audioClip;

      public IReadOnlyList<IDifficultyBeatmapSet> difficultyBeatmapSets => (IReadOnlyList<IDifficultyBeatmapSet>) this._difficultyBeatmapSets;

      public BeatmapLevelData(
        BeatmapLevelDataSO beatmapLevelData,
        BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection,
        IBeatmapLevel parentLevel)
      {
        this._audioClip = beatmapLevelData.audioClip;
        this._difficultyBeatmapSets = new List<BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview.BeatmapLevelData.DifficultyBeatmapSet>(beatmapLevelData.difficultyBeatmapSets.Length);
        for (int index = 0; index < beatmapLevelData.difficultyBeatmapSets.Length; ++index)
          this._difficultyBeatmapSets.Add(new BeatmapLevelDataLoaderSO.BeatmapLevelFromPreview.BeatmapLevelData.DifficultyBeatmapSet(beatmapLevelData.difficultyBeatmapSets[index], beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(beatmapLevelData.difficultyBeatmapSets[index].beatmapCharacteristicSerializedName), parentLevel));
      }

      public class DifficultyBeatmapSet : IDifficultyBeatmapSet
      {
        protected readonly BeatmapCharacteristicSO _beatmapCharacteristic;
        protected readonly BeatmapLevelDataSO.DifficultyBeatmapSet _difficultyBeatmapSet;

        public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

        public IReadOnlyList<IDifficultyBeatmap> difficultyBeatmaps => (IReadOnlyList<IDifficultyBeatmap>) this._difficultyBeatmapSet.difficultyBeatmaps;

        public DifficultyBeatmapSet(
          BeatmapLevelDataSO.DifficultyBeatmapSet difficultyBeatmapSet,
          BeatmapCharacteristicSO beatmapCharacteristicSerializedName,
          IBeatmapLevel parentLevel)
        {
          this._difficultyBeatmapSet = difficultyBeatmapSet;
          this._beatmapCharacteristic = beatmapCharacteristicSerializedName;
          foreach (BeatmapLevelSO.DifficultyBeatmap difficultyBeatmap in difficultyBeatmapSet.difficultyBeatmaps)
            difficultyBeatmap.SetParents(parentLevel, (IDifficultyBeatmapSet) this);
        }
      }
    }
  }
}
