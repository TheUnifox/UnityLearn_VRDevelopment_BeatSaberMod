// Decompiled with JetBrains decompiler
// Type: PS4BeatmapDataAssetFileModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;

public class PS4BeatmapDataAssetFileModel : IBeatmapDataAssetFileModel
{
  public event System.Action<LevelDataAssetDownloadUpdate> levelDataAssetDownloadUpdateEvent;

  public virtual async Task<GetAssetBundleFileResult> GetAssetBundleFileForPreviewLevelAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    return await Task.FromResult<GetAssetBundleFileResult>(new GetAssetBundleFileResult(true, (string) null));
  }

  public virtual async Task<bool> TryDeleteAssetBundleFileForPreviewLevelAsync(
    IPreviewBeatmapLevel previewBeatmapLevel,
    CancellationToken cancellationToken)
  {
    return await Task.FromResult<bool>(false);
  }
}
