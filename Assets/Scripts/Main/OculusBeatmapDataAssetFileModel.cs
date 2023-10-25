// Decompiled with JetBrains decompiler
// Type: OculusBeatmapDataAssetFileModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class OculusBeatmapDataAssetFileModel : IBeatmapDataAssetFileModel
{
  protected Dictionary<ulong, OculusBeatmapDataAssetFileModel.LevelDownloadingData> _assetIdToDownloadinData = new Dictionary<ulong, OculusBeatmapDataAssetFileModel.LevelDownloadingData>();
  protected Dictionary<string, string> _downloadedAssetBundleFiles = new Dictionary<string, string>();
  protected Dictionary<ulong, AssetFileDownloadUpdate> _lastAssetFileDownloadUpdateForAssetIds = new Dictionary<ulong, AssetFileDownloadUpdate>();
  protected const float kMaxTimeOutBeforeFail = 120f;
  protected float _lastAssetFileDownloadUpdateTime;
  protected SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
  protected Dictionary<string, AssetDetails> _assetFileToAssetDetails = new Dictionary<string, AssetDetails>();
  protected OculusLevelProductsModelSO _oculusLevelProductsModel;

  public event System.Action<LevelDataAssetDownloadUpdate> levelDataAssetDownloadUpdateEvent;

  public OculusBeatmapDataAssetFileModel(
    OculusLevelProductsModelSO oculusLevelProductsModel)
  {
    this._oculusLevelProductsModel = oculusLevelProductsModel;
    AssetFile.SetDownloadUpdateNotificationCallback((Message<AssetFileDownloadUpdate>.Callback) (msg => this.HandleAssetFileDownloadUpdate(msg)));
  }

  public virtual async Task<bool> TryDeleteAssetBundleFileForPreviewLevelAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    string levelId = previewBeatmapLevel.levelID;
    string assetFileName = BeatmapDataAssetsModel.AssetBundleNameForBeatmapLevel(levelId);
    OculusLevelProductsModelSO.LevelProductData levelProductData = this._oculusLevelProductsModel.GetLevelProductData(levelId);
    if (levelProductData != null)
      this._assetFileToAssetDetails.ContainsKey(levelProductData.assetFile);
    TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
    AssetFile.DeleteByName(assetFileName).OnComplete((Message<AssetFileDeleteResult>.Callback) (msg =>
    {
      if (msg.Data != null && this._lastAssetFileDownloadUpdateForAssetIds.ContainsKey(msg.Data.AssetFileId))
        this._lastAssetFileDownloadUpdateForAssetIds.Remove(msg.Data.AssetFileId);
      this._downloadedAssetBundleFiles.Remove(levelId);
      if (cancellationToken.IsCancellationRequested)
        taskSource.TrySetCanceled();
      else
        taskSource.TrySetResult(!msg.IsError);
    }));
    bool deleted = await taskSource.Task;
    int num = await this.ReloadAssetDetailsForAllLevelsAsync(cancellationToken) ? 1 : 0;
    return deleted;
  }

  public virtual async Task<GetAssetBundleFileResult> GetAssetBundleFileForPreviewLevelAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    string levelId = previewBeatmapLevel.levelID;
    OculusLevelProductsModelSO.LevelProductData levelProductData = this._oculusLevelProductsModel.GetLevelProductData(levelId);
    if (levelProductData == null)
      return new GetAssetBundleFileResult(true, (string) null);
    string assetFile = levelProductData.assetFile;
    BeatmapDataAssetsModel.AssetBundleNameForBeatmapLevel(previewBeatmapLevel.levelID);
    if (this._downloadedAssetBundleFiles.ContainsKey(levelId))
      return new GetAssetBundleFileResult(false, this._downloadedAssetBundleFiles[levelId]);
    if ((double) Time.realtimeSinceStartup - (double) this._lastAssetFileDownloadUpdateTime > 120.0)
    {
      this._assetFileToAssetDetails.Clear();
      bool flag = false;
      foreach (KeyValuePair<ulong, OculusBeatmapDataAssetFileModel.LevelDownloadingData> keyValuePair in this._assetIdToDownloadinData)
      {
        keyValuePair.Value.downloadAssetBundleFileTCS.TrySetResult(new GetAssetBundleFileResult(true, (string) null));
        if (keyValuePair.Value.levelId == levelId)
          flag = true;
      }
      this._assetIdToDownloadinData.Clear();
      this._lastAssetFileDownloadUpdateForAssetIds.Clear();
      this._lastAssetFileDownloadUpdateTime = Time.realtimeSinceStartup;
      if (flag)
        return new GetAssetBundleFileResult(true, (string) null);
    }
    if (!this._assetFileToAssetDetails.ContainsKey(assetFile))
    {
      bool flag = false;
      await this._semaphoreSlim.WaitAsync(cancellationToken);
      try
      {
        flag = await this.ReloadAssetDetailsForAllLevelsAsync(cancellationToken);
      }
      finally
      {
        this._semaphoreSlim.Release();
      }
      if (!flag || !this._assetFileToAssetDetails.ContainsKey(assetFile))
        return new GetAssetBundleFileResult(true, (string) null);
    }
    AssetDetails fileToAssetDetail = this._assetFileToAssetDetails[assetFile];
    cancellationToken.ThrowIfCancellationRequested();
    GetAssetBundleFileResult assetBundleFileAsync = await this.GetDownloadAssetBundleFileAsync(levelId, fileToAssetDetail, cancellationToken);
    cancellationToken.ThrowIfCancellationRequested();
    return assetBundleFileAsync;
  }

  public virtual async Task<bool> ReloadAssetDetailsForAllLevelsAsync(
    CancellationToken cancellationToken)
  {
    TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
    AssetFile.GetList().OnComplete((Message<AssetDetailsList>.Callback) (getListMsg =>
    {
      if (cancellationToken.IsCancellationRequested)
        taskSource.TrySetCanceled();
      else if (getListMsg.IsError)
      {
        taskSource.TrySetResult(false);
      }
      else
      {
        foreach (AssetDetails assetDetails in (DeserializableList<AssetDetails>) getListMsg.Data)
          this._assetFileToAssetDetails[Path.GetFileName(assetDetails.Filepath)] = assetDetails;
        taskSource.TrySetResult(true);
      }
    }));
    return await taskSource.Task;
  }

  public virtual async Task<GetAssetBundleFileResult> GetDownloadAssetBundleFileAsync(
    string levelId,
    AssetDetails assetDetails,
    CancellationToken cancellationToken)
  {
    if (assetDetails.DownloadStatus == "installed")
    {
      this._downloadedAssetBundleFiles[levelId] = assetDetails.Filepath;
      if (this._assetIdToDownloadinData.ContainsKey(assetDetails.AssetId))
      {
        OculusBeatmapDataAssetFileModel.LevelDownloadingData levelDownloadingData = this._assetIdToDownloadinData[assetDetails.AssetId];
        levelDownloadingData.downloadAssetBundleFileTCS.TrySetResult(new GetAssetBundleFileResult(false, levelDownloadingData.assetBundlePath));
        this._assetIdToDownloadinData.Remove(assetDetails.AssetId);
      }
      return new GetAssetBundleFileResult(false, assetDetails.Filepath);
    }
    bool flag = !this._assetIdToDownloadinData.ContainsKey(assetDetails.AssetId);
    TaskCompletionSource<GetAssetBundleFileResult> taskSource = this.GetTaskCompletionSourceForDownload(levelId, assetDetails);
    if (this._lastAssetFileDownloadUpdateForAssetIds.ContainsKey(assetDetails.AssetId))
    {
      AssetFileDownloadUpdate updateForAssetId = this._lastAssetFileDownloadUpdateForAssetIds[assetDetails.AssetId];
      if (updateForAssetId.Completed)
        return new GetAssetBundleFileResult(false, assetDetails.Filepath);
      LevelDataAssetDownloadUpdate.AssetDownloadingState assetDownloadingState = updateForAssetId.Completed ? LevelDataAssetDownloadUpdate.AssetDownloadingState.Completed : LevelDataAssetDownloadUpdate.AssetDownloadingState.Downloading;
      System.Action<LevelDataAssetDownloadUpdate> downloadUpdateEvent = this.levelDataAssetDownloadUpdateEvent;
      if (downloadUpdateEvent != null)
        downloadUpdateEvent(new LevelDataAssetDownloadUpdate(levelId, (uint) updateForAssetId.BytesTotal, (uint) updateForAssetId.BytesTransferred, assetDownloadingState));
      return await taskSource.Task;
    }
    System.Action<LevelDataAssetDownloadUpdate> downloadUpdateEvent1 = this.levelDataAssetDownloadUpdateEvent;
    if (downloadUpdateEvent1 != null)
      downloadUpdateEvent1(new LevelDataAssetDownloadUpdate(levelId, 0U, 0U, LevelDataAssetDownloadUpdate.AssetDownloadingState.PreparingToDownload));
    if (flag)
      AssetFile.DownloadById(assetDetails.AssetId).OnComplete((Message<AssetFileDownloadResult>.Callback) (msg =>
      {
        if (!msg.IsError)
          return;
        taskSource.TrySetResult(new GetAssetBundleFileResult(true, (string) null));
        this._assetIdToDownloadinData.Remove(assetDetails.AssetId);
      }));
    return await taskSource.Task;
  }

  public virtual TaskCompletionSource<GetAssetBundleFileResult> GetTaskCompletionSourceForDownload(
    string levelId,
    AssetDetails assetDetail)
  {
    if (!this._assetIdToDownloadinData.ContainsKey(assetDetail.AssetId))
      this._assetIdToDownloadinData[assetDetail.AssetId] = new OculusBeatmapDataAssetFileModel.LevelDownloadingData(levelId, assetDetail.Filepath);
    return this._assetIdToDownloadinData[assetDetail.AssetId].downloadAssetBundleFileTCS;
  }

  public virtual void HandleAssetFileDownloadUpdate(Message<AssetFileDownloadUpdate> msg)
  {
    this._lastAssetFileDownloadUpdateTime = Time.realtimeSinceStartup;
    int num = msg.IsError ? 1 : 0;
    if (msg.Data.AssetId == 0UL)
      return;
    if (msg.IsError)
    {
      this._assetFileToAssetDetails.Clear();
      foreach (KeyValuePair<ulong, OculusBeatmapDataAssetFileModel.LevelDownloadingData> keyValuePair in this._assetIdToDownloadinData)
        keyValuePair.Value.downloadAssetBundleFileTCS.TrySetResult(new GetAssetBundleFileResult(true, (string) null));
      this._assetIdToDownloadinData.Clear();
      this._lastAssetFileDownloadUpdateForAssetIds.Clear();
      this._lastAssetFileDownloadUpdateTime = Time.realtimeSinceStartup;
    }
    else
    {
      if (msg.IsError)
        return;
      ulong assetId = msg.Data.AssetId;
      this._lastAssetFileDownloadUpdateForAssetIds[assetId] = msg.Data;
      if (!this._assetIdToDownloadinData.ContainsKey(assetId))
        return;
      OculusBeatmapDataAssetFileModel.LevelDownloadingData levelDownloadingData = this._assetIdToDownloadinData[assetId];
      if (msg.Data.AssetId != 0UL && (msg.Data.BytesTransferred == -1L || msg.Data.BytesTotal == (ulong) uint.MaxValue))
      {
        levelDownloadingData.downloadAssetBundleFileTCS.TrySetResult(new GetAssetBundleFileResult(true, (string) null));
        this._assetIdToDownloadinData.Remove(assetId);
      }
      else
      {
        LevelDataAssetDownloadUpdate.AssetDownloadingState assetDownloadingState = msg.Data.Completed ? LevelDataAssetDownloadUpdate.AssetDownloadingState.Completed : LevelDataAssetDownloadUpdate.AssetDownloadingState.Downloading;
        System.Action<LevelDataAssetDownloadUpdate> downloadUpdateEvent = this.levelDataAssetDownloadUpdateEvent;
        if (downloadUpdateEvent != null)
          downloadUpdateEvent(new LevelDataAssetDownloadUpdate(levelDownloadingData.levelId, (uint) msg.Data.BytesTotal, (uint) msg.Data.BytesTransferred, assetDownloadingState));
        if (!msg.Data.Completed)
          return;
        this._downloadedAssetBundleFiles[levelDownloadingData.levelId] = levelDownloadingData.assetBundlePath;
        levelDownloadingData.downloadAssetBundleFileTCS.TrySetResult(new GetAssetBundleFileResult(false, levelDownloadingData.assetBundlePath));
        this._assetIdToDownloadinData.Remove(assetId);
      }
    }
  }

  [CompilerGenerated]
  public virtual void m_ctorm_Eb__12_0(Message<AssetFileDownloadUpdate> msg) => this.HandleAssetFileDownloadUpdate(msg);

  public class LevelDownloadingData
  {
    public readonly string levelId;
    public readonly string assetBundlePath;
    public readonly TaskCompletionSource<GetAssetBundleFileResult> downloadAssetBundleFileTCS;

    public LevelDownloadingData(string levelId, string assetBundlePath)
    {
      this.levelId = levelId;
      this.assetBundlePath = assetBundlePath;
      this.downloadAssetBundleFileTCS = new TaskCompletionSource<GetAssetBundleFileResult>();
    }
  }
}
