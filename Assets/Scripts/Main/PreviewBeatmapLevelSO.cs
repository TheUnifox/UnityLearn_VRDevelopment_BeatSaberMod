// Decompiled with JetBrains decompiler
// Type: PreviewBeatmapLevelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PreviewBeatmapLevelSO : 
  PersistentScriptableObject,
  IPreviewBeatmapLevel,
  IAssetSongPreviewAudioClipProvider
{
  [SerializeField]
  protected string _levelID;
  [SerializeField]
  protected string _songName;
  [SerializeField]
  protected string _songSubName;
  [SerializeField]
  protected string _songAuthorName;
  [SerializeField]
  protected string _levelAuthorName;
  [SerializeField]
  protected AudioClip _previewAudioClip;
  [SerializeField]
  protected float _beatsPerMinute;
  [SerializeField]
  protected float _songTimeOffset;
  [SerializeField]
  protected float _shuffle;
  [SerializeField]
  protected float _shufflePeriod;
  [SerializeField]
  protected float _previewStartTime;
  [SerializeField]
  protected float _previewDuration;
  [SerializeField]
  protected float _songDuration;
  [SerializeField]
  protected Sprite _coverImage;
  [SerializeField]
  protected EnvironmentInfoSO _environmentInfo;
  [SerializeField]
  protected EnvironmentInfoSO _allDirectionsEnvironmentInfo;
  [SerializeField]
  protected PreviewDifficultyBeatmapSet[] _previewDifficultyBeatmapSets;
  public bool _ignore360MovementBeatmaps;
  protected IReadOnlyList<PreviewDifficultyBeatmapSet> _no360MovementPreviewDifficultyBeatmapSets;

  public string levelID => this._levelID;

  public string songName => this._songName;

  public string songSubName => this._songSubName;

  public string songAuthorName => this._songAuthorName;

  public string levelAuthorName => this._levelAuthorName;

  public float beatsPerMinute => this._beatsPerMinute;

  public float songTimeOffset => this._songTimeOffset;

  public float songDuration => this._songDuration;

  public float shuffle => this._shuffle;

  public float shufflePeriod => this._shufflePeriod;

  public float previewStartTime => this._previewStartTime;

  public float previewDuration => this._previewDuration;

  public EnvironmentInfoSO environmentInfo => this._environmentInfo;

  public EnvironmentInfoSO allDirectionsEnvironmentInfo => this._allDirectionsEnvironmentInfo;

  public AudioClip songPreviewAudioClip => this._previewAudioClip;

  public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => !this._ignore360MovementBeatmaps ? (IReadOnlyList<PreviewDifficultyBeatmapSet>) this._previewDifficultyBeatmapSets : this._no360MovementPreviewDifficultyBeatmapSets;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.InitData();
  }

  public virtual void InitData() => this._no360MovementPreviewDifficultyBeatmapSets = ((IReadOnlyList<PreviewDifficultyBeatmapSet>) this._previewDifficultyBeatmapSets).GetPreviewDifficultyBeatmapSetWithout360Movement();

  public virtual async Task<AudioClip> GetPreviewAudioClipAsync(CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.FromResult<AudioClip>(this._previewAudioClip);
  }

  public virtual async Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.FromResult<Sprite>(this._coverImage);
  }
}
