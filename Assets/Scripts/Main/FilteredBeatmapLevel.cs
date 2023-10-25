// Decompiled with JetBrains decompiler
// Type: FilteredBeatmapLevel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FilteredBeatmapLevel : 
  IBeatmapLevel,
  IPreviewBeatmapLevel,
  IAssetSongPreviewAudioClipProvider,
  IAssetSongAudioClipProvider,
  IFilePathSongPreviewAudioClipProvider,
  IFilePathSongAudioClipProvider
{
  [CompilerGenerated]
  protected readonly IReadOnlyList<PreviewDifficultyBeatmapSet> m_CpreviewDifficultyBeatmapSets;
  [CompilerGenerated]
  protected readonly IBeatmapLevelData m_CbeatmapLevelData;
  protected readonly IBeatmapLevel _beatmapLevel;

  public string levelID => this._beatmapLevel.levelID;

  public string songName => this._beatmapLevel.songName;

  public string songSubName => this._beatmapLevel.songSubName;

  public string songAuthorName => this._beatmapLevel.songAuthorName;

  public string levelAuthorName => this._beatmapLevel.levelAuthorName;

  public float beatsPerMinute => this._beatmapLevel.beatsPerMinute;

  public float songTimeOffset => this._beatmapLevel.songTimeOffset;

  public float shuffle => this._beatmapLevel.shuffle;

  public float shufflePeriod => this._beatmapLevel.shufflePeriod;

  public float previewStartTime => this._beatmapLevel.previewStartTime;

  public float previewDuration => this._beatmapLevel.previewDuration;

  public float songDuration => this._beatmapLevel.songDuration;

  public EnvironmentInfoSO environmentInfo => this._beatmapLevel.environmentInfo;

  public EnvironmentInfoSO allDirectionsEnvironmentInfo => this._beatmapLevel.allDirectionsEnvironmentInfo;

  public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => this.m_CpreviewDifficultyBeatmapSets;

  public AudioClip songPreviewAudioClip => !(this._beatmapLevel is IAssetSongPreviewAudioClipProvider beatmapLevel) ? (AudioClip) null : beatmapLevel.songPreviewAudioClip;

  public AudioClip songAudioClip => !(this._beatmapLevel is IAssetSongAudioClipProvider beatmapLevel) ? (AudioClip) null : beatmapLevel.songAudioClip;

  public string songPreviewAudioClipPath => !(this._beatmapLevel is IFilePathSongPreviewAudioClipProvider beatmapLevel) ? string.Empty : beatmapLevel.songPreviewAudioClipPath;

  public string songAudioClipPath => !(this._beatmapLevel is IFilePathSongAudioClipProvider beatmapLevel) ? string.Empty : beatmapLevel.songAudioClipPath;

  public virtual Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken) => this._beatmapLevel.GetCoverImageAsync(cancellationToken);

  public IBeatmapLevelData beatmapLevelData => this.m_CbeatmapLevelData;

  public bool isEmpty => this.beatmapLevelData.difficultyBeatmapSets.Count == 0;

  public FilteredBeatmapLevel(
    IBeatmapLevel beatmapLevel,
    BeatmapDifficultyMask allowedBeatmapDifficultyMask,
    HashSet<BeatmapCharacteristicSO> notAllowedCharacteristics)
  {
    this._beatmapLevel = beatmapLevel;
    this.m_CbeatmapLevelData = (IBeatmapLevelData) new FilteredBeatmapLevel.FilteredBeatmapLevelData(beatmapLevel.beatmapLevelData, allowedBeatmapDifficultyMask, notAllowedCharacteristics);
    this.m_CpreviewDifficultyBeatmapSets = this.beatmapLevelData.difficultyBeatmapSets.GetPreviewDifficultyBeatmapSets<IDifficultyBeatmapSet>();
  }

  public class FilteredBeatmapLevelData : IBeatmapLevelData
  {
    [CompilerGenerated]
    protected readonly IReadOnlyList<IDifficultyBeatmapSet> m_CdifficultyBeatmapSets;
    protected readonly IBeatmapLevelData _beatmapLevelData;

    public AudioClip audioClip => this._beatmapLevelData.audioClip;

    public IReadOnlyList<IDifficultyBeatmapSet> difficultyBeatmapSets => this.m_CdifficultyBeatmapSets;

    public FilteredBeatmapLevelData(
      IBeatmapLevelData beatmapLevelData,
      BeatmapDifficultyMask allowedBeatmapDifficultyMask,
      HashSet<BeatmapCharacteristicSO> notAllowedCharacteristics)
    {
      this._beatmapLevelData = beatmapLevelData;
      List<IDifficultyBeatmapSet> difficultyBeatmapSetList = new List<IDifficultyBeatmapSet>();
      foreach (IDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<IDifficultyBeatmapSet>) this._beatmapLevelData.difficultyBeatmapSets)
      {
        if (!notAllowedCharacteristics.Contains(difficultyBeatmapSet.beatmapCharacteristic))
        {
          List<IDifficultyBeatmap> difficultyBeatmapList = new List<IDifficultyBeatmap>();
          foreach (IDifficultyBeatmap difficultyBeatmap in (IEnumerable<IDifficultyBeatmap>) difficultyBeatmapSet.difficultyBeatmaps)
          {
            if (allowedBeatmapDifficultyMask.Contains(difficultyBeatmap.difficulty))
              difficultyBeatmapList.Add(difficultyBeatmap);
          }
          if (difficultyBeatmapList.Count > 0)
            difficultyBeatmapSetList.Add((IDifficultyBeatmapSet) new DifficultyBeatmapSet(difficultyBeatmapSet.beatmapCharacteristic, (IReadOnlyList<IDifficultyBeatmap>) difficultyBeatmapList.ToArray()));
        }
      }
      this.m_CdifficultyBeatmapSets = (IReadOnlyList<IDifficultyBeatmapSet>) difficultyBeatmapSetList.ToArray();
    }
  }
}
