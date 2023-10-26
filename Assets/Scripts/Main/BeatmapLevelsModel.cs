// Decompiled with JetBrains decompiler
// Type: BeatmapLevelsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BeatmapLevelsModel : MonoBehaviour
{
  [SerializeField]
  protected BeatmapLevelPackCollectionContainerSO _dlcLevelPackCollectionContainer;
  [SerializeField]
  protected BeatmapLevelPackCollectionSO _ostAndExtrasPackCollection;
  [SerializeField]
  protected BeatmapLevelDataLoaderSO _beatmapLevelDataLoader;
  [SerializeField]
  protected int _maxCachedBeatmapLevels = 10;
  [Inject]
  protected CustomLevelLoader _customLevelLoader;
  [Inject]
  protected AdditionalContentModel _additionalContentModel;
  [Inject]
  protected IBeatmapDataAssetFileModel _beatmapDataAssetFileModel;
  [Inject]
  protected AudioClipAsyncLoader _audioClipAsyncLoader;
  protected IBeatmapLevelPackCollection _allLoadedBeatmapLevelPackCollection;
  protected IBeatmapLevelPackCollection _allLoadedBeatmapLevelWithoutCustomLevelPackCollection;
  protected IBeatmapLevelPackCollection _customLevelPackCollection;
  protected HMCache<string, IBeatmapLevel> _loadedBeatmapLevels;
  protected Dictionary<string, IPreviewBeatmapLevel> _loadedPreviewBeatmapLevels = new Dictionary<string, IPreviewBeatmapLevel>();
  protected BeatmapLevelLoader _beatmapLevelLoader;

  public event System.Action<BeatmapLevelsModel.LevelDownloadingUpdate> levelDownloadingUpdateEvent;

  public BeatmapLevelPackCollectionSO ostAndExtrasPackCollection => this._ostAndExtrasPackCollection;

  public IBeatmapLevelPackCollection dlcBeatmapLevelPackCollection => (IBeatmapLevelPackCollection) this._dlcLevelPackCollectionContainer.beatmapLevelPackCollection;

  public IBeatmapLevelPackCollection allLoadedBeatmapLevelPackCollection => this._allLoadedBeatmapLevelPackCollection;

  public IBeatmapLevelPackCollection allLoadedBeatmapLevelWithoutCustomLevelPackCollection => this._allLoadedBeatmapLevelWithoutCustomLevelPackCollection;

  public IBeatmapLevelPackCollection customLevelPackCollection => this._customLevelPackCollection;

  [Inject]
  public virtual void Init()
  {
    this._beatmapLevelLoader = new BeatmapLevelLoader(this._beatmapLevelDataLoader, this._beatmapDataAssetFileModel);
    this._loadedBeatmapLevels = new HMCache<string, IBeatmapLevel>(this._maxCachedBeatmapLevels);
    this._loadedBeatmapLevels.itemWillBeRemovedFromCacheEvent += new System.Action<IBeatmapLevel>(this.HandleItemWillBeRemovedFromCache);
    this.UpdateLoadedPreviewLevels();
  }

  public virtual void Start() => this._beatmapDataAssetFileModel.levelDataAssetDownloadUpdateEvent += new System.Action<LevelDataAssetDownloadUpdate>(this.HandleLevelDataAssetDownloadUpdate);

  public virtual void OnDestroy()
  {
    this._beatmapDataAssetFileModel.levelDataAssetDownloadUpdateEvent -= new System.Action<LevelDataAssetDownloadUpdate>(this.HandleLevelDataAssetDownloadUpdate);
    this._loadedBeatmapLevels.itemWillBeRemovedFromCacheEvent -= new System.Action<IBeatmapLevel>(this.HandleItemWillBeRemovedFromCache);
  }

  public virtual void ClearLoadedBeatmapLevelsCaches()
  {
    this._customLevelLoader.ClearCache();
    this._loadedBeatmapLevels.Clear();
  }

  public virtual async Task<IBeatmapLevelPackCollection> ReloadCustomLevelPackCollectionAsync(
    CancellationToken cancellationToken)
  {
    CustomLevelLoader.CustomPackFolderInfo[] foldersInfosAsync = await this._customLevelLoader.GetSubFoldersInfosAsync("CustomLevels", cancellationToken);
    List<CustomLevelLoader.CustomPackFolderInfo> customPackFolderInfoList = new List<CustomLevelLoader.CustomPackFolderInfo>((IEnumerable<CustomLevelLoader.CustomPackFolderInfo>) new CustomLevelLoader.CustomPackFolderInfo[1]
    {
      new CustomLevelLoader.CustomPackFolderInfo("CustomLevels", Localization.Get("TITLE_CUSTOM_LEVELS"))
    });
    customPackFolderInfoList.AddRange((IEnumerable<CustomLevelLoader.CustomPackFolderInfo>) foldersInfosAsync);
    this._customLevelPackCollection = (IBeatmapLevelPackCollection) new BeatmapLevelPackCollection((IBeatmapLevelPack[]) await this._customLevelLoader.LoadCustomPreviewBeatmapLevelPacksAsync(customPackFolderInfoList.ToArray(), cancellationToken));
    this.UpdateLoadedPreviewLevels();
    return this._customLevelPackCollection;
  }

  public virtual IBeatmapLevelPack GetLevelPackForLevelId(string levelId)
  {
    foreach (IBeatmapLevelPack beatmapLevelPack in this._allLoadedBeatmapLevelPackCollection.beatmapLevelPacks)
    {
      foreach (IPreviewBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) beatmapLevelPack.beatmapLevelCollection.beatmapLevels)
      {
        if (beatmapLevel.levelID == levelId)
          return beatmapLevelPack;
      }
    }
    return (IBeatmapLevelPack) null;
  }

  public virtual IBeatmapLevelPack GetLevelPack(string levePacklId)
  {
    foreach (IBeatmapLevelPack beatmapLevelPack in this._allLoadedBeatmapLevelPackCollection.beatmapLevelPacks)
    {
      if (beatmapLevelPack.packID == levePacklId)
        return beatmapLevelPack;
    }
    return (IBeatmapLevelPack) null;
  }

  public virtual IPreviewBeatmapLevel GetLevelPreviewForLevelId(string levelId) => ((IEnumerable<IBeatmapLevelPack>) this._allLoadedBeatmapLevelPackCollection.beatmapLevelPacks).SelectMany<IBeatmapLevelPack, IPreviewBeatmapLevel>((Func<IBeatmapLevelPack, IEnumerable<IPreviewBeatmapLevel>>) (beatmapLevelPack => (IEnumerable<IPreviewBeatmapLevel>) beatmapLevelPack.beatmapLevelCollection.beatmapLevels)).FirstOrDefault<IPreviewBeatmapLevel>((Func<IPreviewBeatmapLevel, bool>) (beatmapLevel => beatmapLevel.levelID == levelId));

  public virtual bool IsBeatmapLevelLoaded(string levelId) => this._loadedPreviewBeatmapLevels.ContainsKey(levelId) && this._loadedPreviewBeatmapLevels[levelId] is IBeatmapLevel || this._loadedBeatmapLevels.IsInCache(levelId);

  public virtual IBeatmapLevel GetBeatmapLevelIfLoaded(string levelId)
  {
    if (this._loadedPreviewBeatmapLevels.ContainsKey(levelId))
    {
      IPreviewBeatmapLevel previewBeatmapLevel = this._loadedPreviewBeatmapLevels[levelId];
      if (previewBeatmapLevel is IBeatmapLevel)
        return (IBeatmapLevel) previewBeatmapLevel;
    }
    return this._loadedBeatmapLevels.GetFromCache(levelId);
  }

  public virtual async Task<BeatmapLevelsModel.GetBeatmapLevelResult> GetBeatmapLevelAsync(
    string levelID,
    CancellationToken cancellationToken)
  {
    if (await this._additionalContentModel.GetLevelEntitlementStatusAsync(levelID, cancellationToken) == AdditionalContentModel.EntitlementStatus.Owned)
    {
            if (this._loadedBeatmapLevels.IsInCache(levelID))
            {
                this._loadedBeatmapLevels.UpdateOrderInCache(levelID);
                return new BeatmapLevelsModel.GetBeatmapLevelResult(false, this._loadedBeatmapLevels.GetFromCache(levelID));
            }
            if (!this._loadedPreviewBeatmapLevels.ContainsKey(levelID))
            {
                return new BeatmapLevelsModel.GetBeatmapLevelResult(true, null);
            }
            IPreviewBeatmapLevel previewBeatmapLevel = this._loadedPreviewBeatmapLevels[levelID];
            IBeatmapLevel beatmapLevel;
            if ((beatmapLevel = (previewBeatmapLevel as IBeatmapLevel)) != null)
            {
                await this._audioClipAsyncLoader.LoadSong(beatmapLevel);
                this._loadedBeatmapLevels.PutToCache(levelID, beatmapLevel);
                return new BeatmapLevelsModel.GetBeatmapLevelResult(false, beatmapLevel);
            }
            if (previewBeatmapLevel is CustomPreviewBeatmapLevel)
            {
                CustomPreviewBeatmapLevel customPreviewBeatmapLevel = (CustomPreviewBeatmapLevel)previewBeatmapLevel;
                CustomBeatmapLevel customBeatmapLevel = await this._customLevelLoader.LoadCustomBeatmapLevelAsync(customPreviewBeatmapLevel, cancellationToken);
                if (customBeatmapLevel == null || customBeatmapLevel.beatmapLevelData == null)
                {
                    return new BeatmapLevelsModel.GetBeatmapLevelResult(true, null);
                }
                this._loadedBeatmapLevels.PutToCache(levelID, customBeatmapLevel);
                return new BeatmapLevelsModel.GetBeatmapLevelResult(false, customBeatmapLevel);
            }
            else
            {
                BeatmapLevelLoader.LoadBeatmapLevelResult loadLevelResult = await this._beatmapLevelLoader.LoadBeatmapLevelAsync(previewBeatmapLevel, cancellationToken);
                if (loadLevelResult.isError)
                {
                    return new BeatmapLevelsModel.GetBeatmapLevelResult(true, null);
                }
                if (loadLevelResult.beatmapLevel != null)
                {
                    await this._audioClipAsyncLoader.LoadSong(loadLevelResult.beatmapLevel);
                    this._loadedBeatmapLevels.PutToCache(levelID, loadLevelResult.beatmapLevel);
                    return new BeatmapLevelsModel.GetBeatmapLevelResult(false, loadLevelResult.beatmapLevel);
                }
                beatmapLevel = null;
                loadLevelResult = default(BeatmapLevelLoader.LoadBeatmapLevelResult);
            }
        }
    return new BeatmapLevelsModel.GetBeatmapLevelResult(true, (IBeatmapLevel) null);
  }

  public virtual void HandleLevelDataAssetDownloadUpdate(LevelDataAssetDownloadUpdate update)
  {
    BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState downloadingState = BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState.PreparingToDownload;
    switch (update.assetDownloadingState)
    {
      case LevelDataAssetDownloadUpdate.AssetDownloadingState.PreparingToDownload:
        downloadingState = BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState.PreparingToDownload;
        break;
      case LevelDataAssetDownloadUpdate.AssetDownloadingState.Downloading:
        downloadingState = BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState.Downloading;
        break;
      case LevelDataAssetDownloadUpdate.AssetDownloadingState.Completed:
        downloadingState = BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState.Completed;
        break;
    }
    System.Action<BeatmapLevelsModel.LevelDownloadingUpdate> downloadingUpdateEvent = this.levelDownloadingUpdateEvent;
    if (downloadingUpdateEvent == null)
      return;
    downloadingUpdateEvent(new BeatmapLevelsModel.LevelDownloadingUpdate(update.levelID, update.bytesTotal, update.bytesTransferred, downloadingState));
  }

  public virtual void HandleItemWillBeRemovedFromCache(IBeatmapLevel beatmapLevel) => this._audioClipAsyncLoader.UnloadSong(beatmapLevel);

  public virtual void UpdateLoadedPreviewLevels()
  {
    this.UpdateAllLoadedBeatmapLevelPacks();
    foreach (IAnnotatedBeatmapLevelCollection beatmapLevelPack in this._allLoadedBeatmapLevelPackCollection.beatmapLevelPacks)
    {
      foreach (IPreviewBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) beatmapLevelPack.beatmapLevelCollection.beatmapLevels)
        this._loadedPreviewBeatmapLevels[beatmapLevel.levelID] = beatmapLevel;
    }
  }

  public virtual void UpdateAllLoadedBeatmapLevelPacks()
  {
    List<IBeatmapLevelPack> beatmapLevelPackList = new List<IBeatmapLevelPack>((IEnumerable<IBeatmapLevelPack>) this._ostAndExtrasPackCollection.beatmapLevelPacks);
    beatmapLevelPackList.AddRange((IEnumerable<IBeatmapLevelPack>) this._dlcLevelPackCollectionContainer.beatmapLevelPackCollection.beatmapLevelPacks);
    this._allLoadedBeatmapLevelWithoutCustomLevelPackCollection = (IBeatmapLevelPackCollection) new BeatmapLevelPackCollection(beatmapLevelPackList.ToArray());
    if (this._customLevelPackCollection != null)
      beatmapLevelPackList.AddRange((IEnumerable<IBeatmapLevelPack>) this._customLevelPackCollection.beatmapLevelPacks);
    this._allLoadedBeatmapLevelPackCollection = (IBeatmapLevelPackCollection) new BeatmapLevelPackCollection(beatmapLevelPackList.ToArray());
  }

  public struct GetBeatmapLevelResult
  {
    public readonly bool isError;
    public readonly IBeatmapLevel beatmapLevel;

    public GetBeatmapLevelResult(bool isError, IBeatmapLevel beatmapLevel)
    {
      this.isError = isError;
      this.beatmapLevel = beatmapLevel;
    }
  }

  public struct LevelDownloadingUpdate
  {
    public readonly string levelID;
    public readonly uint bytesTotal;
    public readonly uint bytesTransferred;
    public readonly BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState downloadingState;

    public LevelDownloadingUpdate(
      string levelID,
      uint bytesTotal,
      uint bytesTransferred,
      BeatmapLevelsModel.LevelDownloadingUpdate.DownloadingState downloadingState)
    {
      this.levelID = levelID;
      this.bytesTotal = bytesTotal;
      this.bytesTransferred = bytesTransferred;
      this.downloadingState = downloadingState;
    }

    public enum DownloadingState
    {
      PreparingToDownload,
      Downloading,
      Completed,
    }
  }
}
