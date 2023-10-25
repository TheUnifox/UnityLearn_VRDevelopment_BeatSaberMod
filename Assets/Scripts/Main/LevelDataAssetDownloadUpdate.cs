// Decompiled with JetBrains decompiler
// Type: LevelDataAssetDownloadUpdate
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public struct LevelDataAssetDownloadUpdate
{
  public readonly string levelID;
  public readonly uint bytesTotal;
  public readonly uint bytesTransferred;
  public readonly LevelDataAssetDownloadUpdate.AssetDownloadingState assetDownloadingState;

  public LevelDataAssetDownloadUpdate(
    string levelID,
    uint bytesTotal,
    uint bytesTransferred,
    LevelDataAssetDownloadUpdate.AssetDownloadingState assetDownloadingState)
  {
    this.levelID = levelID;
    this.bytesTotal = bytesTotal;
    this.bytesTransferred = bytesTransferred;
    this.assetDownloadingState = assetDownloadingState;
  }

  public enum AssetDownloadingState
  {
    PreparingToDownload,
    Downloading,
    Completed,
  }
}
