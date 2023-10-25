// Decompiled with JetBrains decompiler
// Type: BeatmapLevelLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class BeatmapLevelLoader
{
  protected BeatmapLevelDataLoaderSO _beatmapLevelDataLoader;
  protected IBeatmapDataAssetFileModel _beatmapDataAssetFileModel;

  public BeatmapLevelLoader(
    BeatmapLevelDataLoaderSO beatmapLevelDataLoader,
    IBeatmapDataAssetFileModel beatmapDataAssetFileModel)
  {
    this._beatmapLevelDataLoader = beatmapLevelDataLoader;
    this._beatmapDataAssetFileModel = beatmapDataAssetFileModel;
  }

  public virtual async Task<BeatmapLevelLoader.LoadBeatmapLevelResult> LoadBeatmapLevelAsync(
    IPreviewBeatmapLevel previewLevel,
    CancellationToken cancellationToken)
  {
    GetAssetBundleFileResult previewLevelAsync = await this._beatmapDataAssetFileModel.GetAssetBundleFileForPreviewLevelAsync(previewLevel, cancellationToken);
    if (previewLevelAsync.isError)
      return new BeatmapLevelLoader.LoadBeatmapLevelResult(true, (IBeatmapLevel) null);
    if (!File.Exists(previewLevelAsync.assetBundlePath))
    {
      int num = await this._beatmapDataAssetFileModel.TryDeleteAssetBundleFileForPreviewLevelAsync(previewLevel, cancellationToken) ? 1 : 0;
      return new BeatmapLevelLoader.LoadBeatmapLevelResult(true, (IBeatmapLevel) null);
    }
    string assetBundlePath = previewLevelAsync.assetBundlePath;
    string levelDataAssetName = BeatmapDataAssetsModel.BeatmapLevelDataAssetNameForBeatmapLevel(previewLevel.levelID);
    IBeatmapLevel level = await this._beatmapLevelDataLoader.LoadBeatmapLevelFormAssetBundleAsync(previewLevel, assetBundlePath, levelDataAssetName, cancellationToken);
    if (level == null)
    {
      int num1 = await this._beatmapDataAssetFileModel.TryDeleteAssetBundleFileForPreviewLevelAsync(previewLevel, cancellationToken) ? 1 : 0;
    }
    return new BeatmapLevelLoader.LoadBeatmapLevelResult(level == null, level);
  }

  public struct LoadBeatmapLevelResult
  {
    public readonly bool isError;
    public readonly IBeatmapLevel beatmapLevel;

    public LoadBeatmapLevelResult(bool isError, IBeatmapLevel beatmapLevel)
    {
      this.isError = isError;
      this.beatmapLevel = beatmapLevel;
    }
  }
}
