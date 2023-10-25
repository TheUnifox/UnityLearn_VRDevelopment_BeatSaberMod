// Decompiled with JetBrains decompiler
// Type: CustomBeatmapLevel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class CustomBeatmapLevel : 
  CustomPreviewBeatmapLevel,
  IBeatmapLevel,
  IPreviewBeatmapLevel,
  IFilePathSongAudioClipProvider
{
  protected BeatmapLevelData _beatmapLevelData;

  public IBeatmapLevelData beatmapLevelData => (IBeatmapLevelData) this._beatmapLevelData;

  public string songAudioClipPath => this.songPreviewAudioClipPath;

  public CustomBeatmapLevel(
    CustomPreviewBeatmapLevel customPreviewBeatmapLevel)
    : base(customPreviewBeatmapLevel.defaultCoverImage, customPreviewBeatmapLevel.standardLevelInfoSaveData, customPreviewBeatmapLevel.customLevelPath, customPreviewBeatmapLevel.spriteAsyncLoader, customPreviewBeatmapLevel.levelID, customPreviewBeatmapLevel.songName, customPreviewBeatmapLevel.songSubName, customPreviewBeatmapLevel.songAuthorName, customPreviewBeatmapLevel.levelAuthorName, customPreviewBeatmapLevel.beatsPerMinute, customPreviewBeatmapLevel.songTimeOffset, customPreviewBeatmapLevel.shuffle, customPreviewBeatmapLevel.shufflePeriod, customPreviewBeatmapLevel.previewStartTime, customPreviewBeatmapLevel.previewDuration, customPreviewBeatmapLevel.environmentInfo, customPreviewBeatmapLevel.allDirectionsEnvironmentInfo, customPreviewBeatmapLevel.previewDifficultyBeatmapSets)
  {
  }

  public virtual void SetBeatmapLevelData(BeatmapLevelData beatmapLevelData) => this._beatmapLevelData = beatmapLevelData;
}
