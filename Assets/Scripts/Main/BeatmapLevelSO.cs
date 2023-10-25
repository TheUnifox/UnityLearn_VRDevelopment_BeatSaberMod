// Decompiled with JetBrains decompiler
// Type: BeatmapLevelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BeatmapLevelSO : 
  PersistentScriptableObject,
  IBeatmapLevel,
  IPreviewBeatmapLevel,
  IAssetSongAudioClipProvider,
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
  protected AudioClip _audioClip;
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
  protected Sprite _coverImage;
  [SerializeField]
  protected EnvironmentInfoSO _environmentInfo;
  [SerializeField]
  protected EnvironmentInfoSO _allDirectionsEnvironmentInfo;
  [SerializeField]
  protected BeatmapLevelSO.DifficultyBeatmapSet[] _difficultyBeatmapSets;
  public bool _ignore360MovementBeatmaps;
  protected IReadOnlyList<BeatmapLevelSO.DifficultyBeatmapSet> _no360MovementDifficultyBeatmapSets;
  protected IReadOnlyList<PreviewDifficultyBeatmapSet> _previewDifficultyBeatmapSets;
  protected IReadOnlyList<PreviewDifficultyBeatmapSet> _no360MovementPreviewDifficultyBeatmapSets;
  protected IBeatmapLevelData _beatmapLevelData;
  protected BeatmapLevelSO.GetBeatmapLevelDataResult _getBeatmapLevelDataResult;

  public string levelID => this._levelID;

  public string songName => this._songName;

  public string songSubName => this._songSubName;

  public string songAuthorName => this._songAuthorName;

  public string levelAuthorName => this._levelAuthorName;

  public float beatsPerMinute => this._beatsPerMinute;

  public float songTimeOffset => this._songTimeOffset;

  public float shuffle => this._shuffle;

  public float shufflePeriod => this._shufflePeriod;

  public float previewStartTime => this._previewStartTime;

  public float previewDuration => this._previewDuration;

  public Sprite coverImage => this._coverImage;

  public EnvironmentInfoSO environmentInfo => this._environmentInfo;

  public EnvironmentInfoSO allDirectionsEnvironmentInfo => this._allDirectionsEnvironmentInfo;

  public IReadOnlyList<IDifficultyBeatmapSet> difficultyBeatmapSets => !this._ignore360MovementBeatmaps ? (IReadOnlyList<IDifficultyBeatmapSet>) this._difficultyBeatmapSets : (IReadOnlyList<IDifficultyBeatmapSet>) this._no360MovementDifficultyBeatmapSets;

  public float songDuration => this._audioClip.length;

  public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => !this._ignore360MovementBeatmaps ? this._previewDifficultyBeatmapSets : this._no360MovementPreviewDifficultyBeatmapSets;

  public AudioClip songPreviewAudioClip => this._audioClip;

  public AudioClip songAudioClip => this._audioClip;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.InitData();
  }

  public virtual void InitData()
  {
    if (this._difficultyBeatmapSets == null)
      return;
    foreach (BeatmapLevelSO.DifficultyBeatmapSet difficultyBeatmapSet in this._difficultyBeatmapSets)
      difficultyBeatmapSet.SetParentLevel((IBeatmapLevel) this);
    this._previewDifficultyBeatmapSets = ((IReadOnlyList<BeatmapLevelSO.DifficultyBeatmapSet>) this._difficultyBeatmapSets).GetPreviewDifficultyBeatmapSets<BeatmapLevelSO.DifficultyBeatmapSet>();
    this._no360MovementPreviewDifficultyBeatmapSets = this._previewDifficultyBeatmapSets.GetPreviewDifficultyBeatmapSetWithout360Movement();
    this._no360MovementDifficultyBeatmapSets = ((IReadOnlyList<BeatmapLevelSO.DifficultyBeatmapSet>) this._difficultyBeatmapSets).GetDifficultyBeatmapSetsWithout360Movement<BeatmapLevelSO.DifficultyBeatmapSet>();
    this._beatmapLevelData = (IBeatmapLevelData) new BeatmapLevelData(this._audioClip, this.difficultyBeatmapSets);
  }

  public virtual async Task<AudioClip> GetPreviewAudioClipAsync(CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.FromResult<AudioClip>(this._audioClip);
  }

  public virtual async Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.FromResult<Sprite>(this._coverImage);
  }

  public IBeatmapLevelData beatmapLevelData
  {
    get
    {
      if (this._beatmapLevelData == null)
        this._beatmapLevelData = (IBeatmapLevelData) new BeatmapLevelData(this._audioClip, (IReadOnlyList<IDifficultyBeatmapSet>) this._difficultyBeatmapSets);
      return this._beatmapLevelData;
    }
  }

  public virtual async Task<BeatmapLevelSO.GetBeatmapLevelDataResult> GetBeatmapLevelDataAsync(
    CancellationToken token)
  {
    if (this._beatmapLevelData == null)
    {
      this._beatmapLevelData = (IBeatmapLevelData) new BeatmapLevelData(this._audioClip, (IReadOnlyList<IDifficultyBeatmapSet>) this._difficultyBeatmapSets);
      this._getBeatmapLevelDataResult = new BeatmapLevelSO.GetBeatmapLevelDataResult(BeatmapLevelSO.GetBeatmapLevelDataResult.Result.OK, this._beatmapLevelData);
    }
    return await Task.FromResult<BeatmapLevelSO.GetBeatmapLevelDataResult>(this._getBeatmapLevelDataResult);
  }

  public virtual IDifficultyBeatmap GetDifficultyBeatmap(
    BeatmapCharacteristicSO characteristic,
    BeatmapDifficulty difficulty)
  {
    foreach (IDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<IDifficultyBeatmapSet>) this.difficultyBeatmapSets)
    {
      if ((UnityEngine.Object) difficultyBeatmapSet.beatmapCharacteristic == (UnityEngine.Object) characteristic)
      {
        foreach (IDifficultyBeatmap difficultyBeatmap in (IEnumerable<IDifficultyBeatmap>) difficultyBeatmapSet.difficultyBeatmaps)
        {
          if (difficultyBeatmap.difficulty == difficulty)
            return difficultyBeatmap;
        }
        return (IDifficultyBeatmap) null;
      }
    }
    return (IDifficultyBeatmap) null;
  }

  public virtual void InitFull(
    string levelID,
    string songName,
    string songSubName,
    string songAuthorName,
    string levelAuthorName,
    AudioClip audioClip,
    float beatsPerMinute,
    float songTimeOffset,
    float shuffle,
    float shufflePeriod,
    float previewStartTime,
    float previewDuration,
    Sprite coverImage,
    EnvironmentInfoSO environmentInfo,
    EnvironmentInfoSO allDirectionsEnvironmentInfo,
    BeatmapLevelSO.DifficultyBeatmapSet[] difficultyBeatmapSets)
  {
    this._levelID = levelID;
    this._songName = songName;
    this._songSubName = songSubName;
    this._songAuthorName = songAuthorName;
    this._levelAuthorName = levelAuthorName;
    this._audioClip = audioClip;
    this._beatsPerMinute = beatsPerMinute;
    this._songTimeOffset = songTimeOffset;
    this._shuffle = shuffle;
    this._shufflePeriod = shufflePeriod;
    this._previewStartTime = previewStartTime;
    this._previewDuration = previewDuration;
    this._coverImage = coverImage;
    this._environmentInfo = environmentInfo;
    this._allDirectionsEnvironmentInfo = allDirectionsEnvironmentInfo;
    this._difficultyBeatmapSets = difficultyBeatmapSets;
    this.InitData();
  }

  [Serializable]
  public class DifficultyBeatmapSet : IDifficultyBeatmapSet
  {
    [SerializeField]
    protected BeatmapCharacteristicSO _beatmapCharacteristic;
    [SerializeField]
    protected BeatmapLevelSO.DifficultyBeatmap[] _difficultyBeatmaps;

    public BeatmapCharacteristicSO beatmapCharacteristic => this._beatmapCharacteristic;

    public IReadOnlyList<IDifficultyBeatmap> difficultyBeatmaps => (IReadOnlyList<IDifficultyBeatmap>) this._difficultyBeatmaps;

    public DifficultyBeatmapSet(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapLevelSO.DifficultyBeatmap[] difficultyBeatmaps)
    {
      this._beatmapCharacteristic = beatmapCharacteristic;
      this._difficultyBeatmaps = difficultyBeatmaps;
    }

    public virtual void SetParentLevel(IBeatmapLevel level)
    {
      foreach (BeatmapLevelSO.DifficultyBeatmap difficultyBeatmap in this._difficultyBeatmaps)
        difficultyBeatmap.SetParents(level, (IDifficultyBeatmapSet) this);
    }
  }

  [Serializable]
  public class DifficultyBeatmap : IDifficultyBeatmap
  {
    [SerializeField]
    protected BeatmapDifficulty _difficulty;
    [SerializeField]
    protected int _difficultyRank;
    [SerializeField]
    protected float _noteJumpMovementSpeed;
    [SerializeField]
    protected float _noteJumpStartBeatOffset;
    [SerializeField]
    protected BeatmapDataSO _beatmapData;
    protected IBeatmapLevel _parentLevel;
    protected IDifficultyBeatmapSet _parentDifficultyBeatmapSet;

    public BeatmapDifficulty difficulty => this._difficulty;

    public int difficultyRank => this._difficultyRank;

    public float noteJumpMovementSpeed => this._noteJumpMovementSpeed;

    public float noteJumpStartBeatOffset => this._noteJumpStartBeatOffset;

    public virtual async Task<IBeatmapDataBasicInfo> GetBeatmapDataBasicInfoAsync() => await this._beatmapData.GetBeatmapDataBasicInfoAsync();

    public virtual async Task<IReadonlyBeatmapData> GetBeatmapDataAsync(
      EnvironmentInfoSO environmentInfo,
      PlayerSpecificSettings playerSpecificSettings)
    {
      return await this._beatmapData.GetBeatmapDataAsync(this._difficulty, this.level.beatsPerMinute, this._parentLevel.environmentInfo.serializedName == environmentInfo.serializedName, environmentInfo, playerSpecificSettings);
    }

    public IBeatmapLevel level => this._parentLevel;

    public IDifficultyBeatmapSet parentDifficultyBeatmapSet => this._parentDifficultyBeatmapSet;

    public virtual void SetParents(
      IBeatmapLevel parentLevel,
      IDifficultyBeatmapSet parentDifficultyBeatmapSet)
    {
      this._parentLevel = parentLevel;
      this._parentDifficultyBeatmapSet = parentDifficultyBeatmapSet;
    }

    public DifficultyBeatmap(
      IBeatmapLevel parentLevel,
      BeatmapDifficulty difficulty,
      int difficultyRank,
      float noteJumpMovementSpeed,
      float noteJumpStartBeatOffset,
      BeatmapDataSO beatmapData)
    {
      this._parentLevel = parentLevel;
      this._difficulty = difficulty;
      this._difficultyRank = difficultyRank;
      this._noteJumpMovementSpeed = noteJumpMovementSpeed;
      this._noteJumpStartBeatOffset = noteJumpStartBeatOffset;
      this._beatmapData = beatmapData;
    }
  }

  public readonly struct GetBeatmapLevelDataResult
  {
    public readonly BeatmapLevelSO.GetBeatmapLevelDataResult.Result result;
    public readonly IBeatmapLevelData beatmapLevelData;

    public GetBeatmapLevelDataResult(
      BeatmapLevelSO.GetBeatmapLevelDataResult.Result result,
      IBeatmapLevelData beatmapLevelData)
    {
      this.result = result;
      this.beatmapLevelData = beatmapLevelData;
    }

    public enum Result
    {
      OK,
      NotOwned,
      Fail,
    }
  }
}
