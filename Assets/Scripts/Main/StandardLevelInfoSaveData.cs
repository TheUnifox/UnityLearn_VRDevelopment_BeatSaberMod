// Decompiled with JetBrains decompiler
// Type: StandardLevelInfoSaveData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class StandardLevelInfoSaveData
{
  protected const string kCurrentVersion = "2.0.0";
  protected const string kDefaultBeatmapCharacteristicName = "Standard";
  [SerializeField]
  protected string _version;
  [SerializeField]
  protected string _songName;
  [SerializeField]
  protected string _songSubName;
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
  [SerializeField]
  protected string _songFilename;
  [SerializeField]
  protected string _coverImageFilename;
  [SerializeField]
  protected string _environmentName;
  [SerializeField]
  protected string _allDirectionsEnvironmentName;
  [SerializeField]
  protected StandardLevelInfoSaveData.DifficultyBeatmapSet[] _difficultyBeatmapSets;

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

  public string allDirectionsEnvironmentName => this._allDirectionsEnvironmentName;

  public StandardLevelInfoSaveData.DifficultyBeatmapSet[] difficultyBeatmapSets => this._difficultyBeatmapSets;

  public StandardLevelInfoSaveData(
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
    string allDirectionsEnvironmentName,
    StandardLevelInfoSaveData.DifficultyBeatmapSet[] difficultyBeatmapSets)
  {
    this._version = "2.0.0";
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
    this._allDirectionsEnvironmentName = allDirectionsEnvironmentName;
    this._difficultyBeatmapSets = difficultyBeatmapSets;
  }

  public bool hasAllData => this._version != null && this._songName != null && this._songSubName != null && this._songAuthorName != null && this._levelAuthorName != null && (double) this._beatsPerMinute != 0.0 && this._songFilename != null && this._coverImageFilename != null && this._environmentName != null && this._difficultyBeatmapSets != null;

  public virtual void SetSongFilename(string songFilename) => this._songFilename = songFilename;

  public virtual string SerializeToJSONString() => JsonUtility.ToJson((object) this);

  public static StandardLevelInfoSaveData DeserializeFromJSONString(string stringData)
  {
    StandardLevelInfoSaveData.VersionCheck versionCheck = (StandardLevelInfoSaveData.VersionCheck) null;
    try
    {
      versionCheck = JsonUtility.FromJson<StandardLevelInfoSaveData.VersionCheck>(stringData);
    }
    catch
    {
    }
    if (versionCheck == null)
      return (StandardLevelInfoSaveData) null;
    StandardLevelInfoSaveData levelInfoSaveData = (StandardLevelInfoSaveData) null;
    if (versionCheck.version == "2.0.0")
    {
      try
      {
        levelInfoSaveData = JsonUtility.FromJson<StandardLevelInfoSaveData>(stringData);
      }
      catch
      {
        return (StandardLevelInfoSaveData) null;
      }
    }
    else if (versionCheck.version == "1.0.0")
    {
      StandardLevelInfoSaveData_V100 infoSaveDataV100;
      try
      {
        infoSaveDataV100 = JsonUtility.FromJson<StandardLevelInfoSaveData_V100>(stringData);
      }
      catch
      {
        infoSaveDataV100 = (StandardLevelInfoSaveData_V100) null;
      }
      if (infoSaveDataV100 != null)
      {
        StandardLevelInfoSaveData.DifficultyBeatmapSet[] difficultyBeatmapSets = new StandardLevelInfoSaveData.DifficultyBeatmapSet[1];
        StandardLevelInfoSaveData.DifficultyBeatmap[] difficultyBeatmaps = new StandardLevelInfoSaveData.DifficultyBeatmap[infoSaveDataV100.difficultyBeatmaps.Length];
        for (int index = 0; index < difficultyBeatmaps.Length; ++index)
          difficultyBeatmaps[index] = new StandardLevelInfoSaveData.DifficultyBeatmap(infoSaveDataV100.difficultyBeatmaps[index].difficulty, infoSaveDataV100.difficultyBeatmaps[index].difficultyRank, infoSaveDataV100.difficultyBeatmaps[index].beatmapFilename, infoSaveDataV100.difficultyBeatmaps[index].noteJumpMovementSpeed, (float) infoSaveDataV100.difficultyBeatmaps[index].noteJumpStartBeatOffset);
        difficultyBeatmapSets[0] = new StandardLevelInfoSaveData.DifficultyBeatmapSet("Standard", difficultyBeatmaps);
        levelInfoSaveData = new StandardLevelInfoSaveData(infoSaveDataV100.songName, infoSaveDataV100.songSubName, infoSaveDataV100.songAuthorName, infoSaveDataV100.levelAuthorName, infoSaveDataV100.beatsPerMinute, infoSaveDataV100.songTimeOffset, infoSaveDataV100.shuffle, infoSaveDataV100.shufflePeriod, infoSaveDataV100.previewStartTime, infoSaveDataV100.previewDuration, infoSaveDataV100.songFilename, infoSaveDataV100.coverImageFilename, infoSaveDataV100.environmentName, (string) null, difficultyBeatmapSets);
      }
    }
    return levelInfoSaveData;
  }

  [Serializable]
  public class DifficultyBeatmapSet
  {
    [SerializeField]
    protected string _beatmapCharacteristicName;
    [SerializeField]
    protected StandardLevelInfoSaveData.DifficultyBeatmap[] _difficultyBeatmaps;

    public string beatmapCharacteristicName => this._beatmapCharacteristicName;

    public StandardLevelInfoSaveData.DifficultyBeatmap[] difficultyBeatmaps => this._difficultyBeatmaps;

    public DifficultyBeatmapSet(
      string beatmapCharacteristicName,
      StandardLevelInfoSaveData.DifficultyBeatmap[] difficultyBeatmaps)
    {
      this._beatmapCharacteristicName = beatmapCharacteristicName;
      this._difficultyBeatmaps = difficultyBeatmaps;
    }
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
    protected float _noteJumpStartBeatOffset;

    public string difficulty => this._difficulty;

    public int difficultyRank => this._difficultyRank;

    public string beatmapFilename => this._beatmapFilename;

    public float noteJumpMovementSpeed => this._noteJumpMovementSpeed;

    public float noteJumpStartBeatOffset => this._noteJumpStartBeatOffset;

    public DifficultyBeatmap(
      string difficultyName,
      int difficultyRank,
      string beatmapFilename,
      float noteJumpMovementSpeed,
      float noteJumpStartBeatOffset)
    {
      this._difficulty = difficultyName;
      this._difficultyRank = difficultyRank;
      this._beatmapFilename = beatmapFilename;
      this._noteJumpMovementSpeed = noteJumpMovementSpeed;
      this._noteJumpStartBeatOffset = noteJumpStartBeatOffset;
    }
  }

  [Serializable]
  public class VersionCheck
  {
    [SerializeField]
    protected string _version;

    public string version => this._version;
  }
}
