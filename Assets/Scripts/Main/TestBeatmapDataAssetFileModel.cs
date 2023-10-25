// Decompiled with JetBrains decompiler
// Type: TestBeatmapDataAssetFileModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class TestBeatmapDataAssetFileModel : IBeatmapDataAssetFileModel
{
  protected const string kAssetsDir = "BeatmapDataAssets";

  public event System.Action<LevelDataAssetDownloadUpdate> levelDataAssetDownloadUpdateEvent;

  public virtual async Task<GetAssetBundleFileResult> GetAssetBundleFileForPreviewLevelAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    string path = Path.Combine("BeatmapDataAssets", previewBeatmapLevel.levelID.ToLower());
    System.Action<LevelDataAssetDownloadUpdate> downloadUpdateEvent1 = this.levelDataAssetDownloadUpdateEvent;
    if (downloadUpdateEvent1 != null)
      downloadUpdateEvent1(new LevelDataAssetDownloadUpdate(previewBeatmapLevel.levelID, 0U, 0U, LevelDataAssetDownloadUpdate.AssetDownloadingState.PreparingToDownload));
    await Task.Delay(50);
    for (uint i = 0; i < 20U; ++i)
    {
      await Task.Delay(50);
      System.Action<LevelDataAssetDownloadUpdate> downloadUpdateEvent2 = this.levelDataAssetDownloadUpdateEvent;
      if (downloadUpdateEvent2 != null)
        downloadUpdateEvent2(new LevelDataAssetDownloadUpdate(previewBeatmapLevel.levelID, 20U, i, LevelDataAssetDownloadUpdate.AssetDownloadingState.Downloading));
      cancellationToken.ThrowIfCancellationRequested();
    }
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.FromResult<GetAssetBundleFileResult>(new GetAssetBundleFileResult(false, path));
  }

  public virtual async Task<bool> TryDeleteAssetBundleFileForPreviewLevelAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    return await Task.FromResult<bool>(false);
  }
}
