// Decompiled with JetBrains decompiler
// Type: StandardLevelInfoSaveData_V100
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

public class StandardLevelInfoSaveData_V100
{
  public const string kCurrentVersion = "1.0.0";
  [SerializeField]
  protected string _version;
  [SerializeField]
  protected string _songName;
  [SerializeField]
  protected string _songSubName;
  [FormerlySerializedAs("_authorName")]
  [SerializeField]
  protected string _songAuthorName;
  [SerializeField]
  protected string _levelAuthorName;
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
  [FormerlySerializedAs("_songFileName")]
  [SerializeField]
  protected string _songFilename;
  [FormerlySerializedAs("_coverImageFileName")]
  [SerializeField]
  protected string _coverImageFilename;
  [SerializeField]
  protected string _environmentName;
  [SerializeField]
  protected StandardLevelInfoSaveData_V100.DifficultyBeatmap[] _difficultyBeatmaps;

  public string version => this._version;

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

  public string songFilename => this._songFilename;

  public string coverImageFilename => this._coverImageFilename;

  public string environmentName => this._environmentName;

  public StandardLevelInfoSaveData_V100.DifficultyBeatmap[] difficultyBeatmaps => this._difficultyBeatmaps;

  public StandardLevelInfoSaveData_V100(
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
    string songFilename,
    string coverImageFilename,
    string environmentName,
    StandardLevelInfoSaveData_V100.DifficultyBeatmap[] difficultyBeatmaps)
  {
    this._version = "1.0.0";
    this._songName = songName;
    this._songSubName = songSubName;
    this._songAuthorName = songAuthorName;
    this._levelAuthorName = levelAuthorName;
    this._beatsPerMinute = beatsPerMinute;
    this._songTimeOffset = songTimeOffset;
    this._shuffle = shuffle;
    this._shufflePeriod = shufflePeriod;
    this._previewStartTime = previewStartTime;
    this._previewDuration = previewDuration;
    this._songFilename = songFilename;
    this._coverImageFilename = coverImageFilename;
    this._environmentName = environmentName;
    this._difficultyBeatmaps = difficultyBeatmaps;
  }

  public bool hasAllData => this._version != null && this._songName != null && this._songSubName != null && this._songAuthorName != null && this._levelAuthorName != null && (double) this._beatsPerMinute != 0.0 && this._songFilename != null && this._coverImageFilename != null && this._environmentName != null && this._difficultyBeatmaps != null;

  public virtual void SetSongFilename(string songFilename) => this._songFilename = songFilename;

  public virtual string SerializeToJSONString() => JsonUtility.ToJson((object) this);

  public static StandardLevelInfoSaveData DeserializeFromJSONString(string stringData)
  {
    StandardLevelInfoSaveData_V100.VersionCheck versionCheck = (StandardLevelInfoSaveData_V100.VersionCheck) null;
    try
    {
      versionCheck = JsonUtility.FromJson<StandardLevelInfoSaveData_V100.VersionCheck>(stringData);
    }
    catch
    {
    }
    if (versionCheck == null)
      return (StandardLevelInfoSaveData) null;
    StandardLevelInfoSaveData levelInfoSaveData = (StandardLevelInfoSaveData) null;
    if (versionCheck.version == "1.0.0")
    {
      try
      {
        levelInfoSaveData = JsonUtility.FromJson<StandardLevelInfoSaveData>(stringData);
      }
      catch
      {
      }
    }
    return levelInfoSaveData;
  }

  [Serializable]
  public class DifficultyBeatmap
  {
    [SerializeField]
    protected string _difficulty;
    [SerializeField]
    protected int _difficultyRank;
    [SerializeField]
    protected string _beatmapFilename;
    [SerializeField]
    protected float _noteJumpMovementSpeed;
    [SerializeField]
    protected int _noteJumpStartBeatOffset;

    public string difficulty => this._difficulty;

    public int difficultyRank => this._difficultyRank;

    public string beatmapFilename => this._beatmapFilename;

    public float noteJumpMovementSpeed => this._noteJumpMovementSpeed;

    public int noteJumpStartBeatOffset => this._noteJumpStartBeatOffset;

    public DifficultyBeatmap(
      string difficultyName,
      int difficultyRank,
      string beatmapFilename,
      float noteJumpMovementSpeed,
      int noteJumpStartBeatOffset)
    {
      this._difficulty = difficultyName;
      this._difficultyRank = difficultyRank;
      this._beatmapFilename = beatmapFilename;
      this._noteJumpMovementSpeed = noteJumpMovementSpeed;
      this._noteJumpStartBeatOffset = noteJumpStartBeatOffset;
    }
  }

  public class VersionCheck
  {
    [SerializeField]
    protected string _version;

    public string version => this._version;
  }
}
