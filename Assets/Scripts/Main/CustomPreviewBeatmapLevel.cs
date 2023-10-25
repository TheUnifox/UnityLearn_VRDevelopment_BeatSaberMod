// Decompiled with JetBrains decompiler
// Type: CustomPreviewBeatmapLevel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CustomPreviewBeatmapLevel : IPreviewBeatmapLevel, IFilePathSongPreviewAudioClipProvider
{
  [CompilerGenerated]
  protected readonly ISpriteAsyncLoader m_CspriteAsyncLoader;
  [CompilerGenerated]
  protected readonly StandardLevelInfoSaveData m_CstandardLevelInfoSaveData;
  [CompilerGenerated]
  protected readonly string m_CcustomLevelPath;
  [CompilerGenerated]
  protected readonly string m_ClevelID;
  [CompilerGenerated]
  protected readonly string m_CsongName;
  [CompilerGenerated]
  protected readonly string m_CsongSubName;
  [CompilerGenerated]
  protected readonly string m_CsongAuthorName;
  [CompilerGenerated]
  protected readonly string m_ClevelAuthorName;
  [CompilerGenerated]
  protected readonly float m_CbeatsPerMinute;
  [CompilerGenerated]
  protected readonly float m_CsongTimeOffset;
  [CompilerGenerated]
  protected readonly float m_CsongDuration;
  [CompilerGenerated]
  protected readonly float m_Cshuffle;
  [CompilerGenerated]
  protected readonly float m_CshufflePeriod;
  [CompilerGenerated]
  protected readonly float m_CpreviewStartTime;
  [CompilerGenerated]
  protected readonly float m_CpreviewDuration;
  [CompilerGenerated]
  protected readonly EnvironmentInfoSO m_CenvironmentInfo;
  [CompilerGenerated]
  protected readonly EnvironmentInfoSO m_CallDirectionsEnvironmentInfo;
  [CompilerGenerated]
  protected readonly Sprite m_CdefaultCoverImage;
  [CompilerGenerated]
  protected readonly IReadOnlyList<PreviewDifficultyBeatmapSet> m_CpreviewDifficultyBeatmapSets;
  protected Sprite _coverImage;

  public ISpriteAsyncLoader spriteAsyncLoader => this.m_CspriteAsyncLoader;

  public StandardLevelInfoSaveData standardLevelInfoSaveData => this.m_CstandardLevelInfoSaveData;

  public string customLevelPath => this.m_CcustomLevelPath;

  public string levelID => this.m_ClevelID;

  public string songName => this.m_CsongName;

  public string songSubName => this.m_CsongSubName;

  public string songAuthorName => this.m_CsongAuthorName;

  public string levelAuthorName => this.m_ClevelAuthorName;

  public float beatsPerMinute => this.m_CbeatsPerMinute;

  public float songTimeOffset => this.m_CsongTimeOffset;

  public float songDuration => this.m_CsongDuration;

  public float shuffle => this.m_Cshuffle;

  public float shufflePeriod => this.m_CshufflePeriod;

  public float previewStartTime => this.m_CpreviewStartTime;

  public float previewDuration => this.m_CpreviewDuration;

  public EnvironmentInfoSO environmentInfo => this.m_CenvironmentInfo;

  public EnvironmentInfoSO allDirectionsEnvironmentInfo => this.m_CallDirectionsEnvironmentInfo;

  public Sprite defaultCoverImage => this.m_CdefaultCoverImage;

  public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => this.m_CpreviewDifficultyBeatmapSets;

  public string songPreviewAudioClipPath => Path.Combine(this.customLevelPath, this.standardLevelInfoSaveData.songFilename);

  public virtual async Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken)
  {
    if ((Object) this._coverImage != (Object) null)
      return this._coverImage;
    Sprite sprite = (Sprite) null;
    if (!string.IsNullOrEmpty(this.standardLevelInfoSaveData.coverImageFilename))
      sprite = await this.spriteAsyncLoader.LoadSpriteAsync(Path.Combine(this.customLevelPath, this.standardLevelInfoSaveData.coverImageFilename), cancellationToken);
    this._coverImage = (Object) sprite != (Object) null ? sprite : this.defaultCoverImage;
    return this._coverImage;
  }

  public CustomPreviewBeatmapLevel(
    Sprite defaultCoverImage,
    StandardLevelInfoSaveData standardLevelInfoSaveData,
    string customLevelPath,
    ISpriteAsyncLoader spriteAsyncLoader,
    string levelID,
    string songName,
    string songSubName,
    string songAuthorName,
    string levelAuthorName,
    float beatsPerMinute,
    float songTimeOffset,
    float shuffle,
    float shufflePeriod,
    float previewStartTime,
    float previewDuration,
    EnvironmentInfoSO environmentInfo,
    EnvironmentInfoSO allDirectionsEnvironmentInfo,
    IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets)
  {
    this.m_CdefaultCoverImage = defaultCoverImage;
    this.m_CstandardLevelInfoSaveData = standardLevelInfoSaveData;
    this.m_CcustomLevelPath = customLevelPath;
    this.m_CspriteAsyncLoader = spriteAsyncLoader;
    this.m_ClevelID = levelID;
    this.m_CsongName = songName;
    this.m_CsongSubName = songSubName;
    this.m_CsongAuthorName = songAuthorName;
    this.m_ClevelAuthorName = levelAuthorName;
    this.m_CbeatsPerMinute = beatsPerMinute;
    this.m_CsongTimeOffset = songTimeOffset;
    this.m_CsongDuration = float.NaN;
    this.m_Cshuffle = shuffle;
    this.m_CshufflePeriod = shufflePeriod;
    this.m_CpreviewStartTime = previewStartTime;
    this.m_CpreviewDuration = previewDuration;
    this.m_CenvironmentInfo = environmentInfo;
    this.m_CallDirectionsEnvironmentInfo = allDirectionsEnvironmentInfo;
    this.m_CpreviewDifficultyBeatmapSets = previewDifficultyBeatmapSets;
  }
}
