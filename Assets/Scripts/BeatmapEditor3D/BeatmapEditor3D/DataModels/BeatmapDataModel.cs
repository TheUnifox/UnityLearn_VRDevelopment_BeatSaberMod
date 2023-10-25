// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapDataModel : IBeatmapDataModel, IReadonlyBeatmapDataModel
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly AudioClipLoader _audioClipLoader;
    [Inject]
    private readonly EnvironmentsListSO _environmentsList;
    [Inject]
    private readonly EnvironmentDefinitionsListSO _environmentDefinitions;
    [Inject]
    private readonly WaveformDataModel _waveformDataModel;
    private bool _audioLoaded;
    private bool _bpmLoaded;

    public bool isDirty { get; private set; }

    public bool beatmapDataLoaded => this._audioLoaded && this._bpmLoaded;

    public string version { get; set; }

    public string songName { get; set; }

    public string songSubName { get; set; }

    public string songAuthorName { get; set; }

    public string levelAuthorName { get; set; }

    public float beatsPerMinute { get; set; }

    public float songTimeOffset { get; set; }

    public float shuffle { get; set; }

    public float shufflePeriod { get; set; }

    public float previewStartTime { get; set; }

    public float previewDuration { get; set; }

    public string songFilename { get; set; }

    public string songFilePath { get; set; }

    public AudioClip audioClip { get; set; }

    public BpmData bpmData { get; set; }

    public string coverImageFilename { get; set; }

    public string coverImageFilePath { get; set; }

    public Texture2D coverImage { get; set; }

    public string environmentName { get; set; }

    public EnvironmentInfoSO environment { get; set; }

    public EnvironmentTracksDefinitionSO environmentTrackDefinition { get; set; }

    public string allDirectionsEnvironmentName { get; set; }

    public EnvironmentInfoSO allDirectionsEnvironment { get; set; }

    public EnvironmentTracksDefinitionSO allDirectionsEnvironmentTrackDefinition { get; set; }

    public IDictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData> difficultyBeatmapSets { get; set; }

    public void UpdateWith(
      string songName = null,
      string songSubName = null,
      string songAuthorName = null,
      string levelAuthorName = null,
      float? beatsPerMinute = null,
      float? songTimeOffset = null,
      float? shuffle = null,
      float? shufflePeriod = null,
      float? previewStartTime = null,
      float? previewDuration = null,
      string songFilename = null,
      string songFilePath = null,
      string coverImageFilename = null,
      string coverImageFilePath = null,
      string environmentName = null,
      string allDirectionsEnvironmentName = null,
      Dictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData> difficultyBeatmapSets = null,
      bool clearDirty = false)
    {
      this.songName = songName ?? this.songName;
      this.songSubName = songSubName ?? this.songSubName;
      this.songAuthorName = songAuthorName ?? this.songAuthorName;
      this.levelAuthorName = levelAuthorName ?? this.levelAuthorName;
      float? nullable = beatsPerMinute;
      this.beatsPerMinute = (float) ((double) nullable ?? (double) this.beatsPerMinute);
      nullable = songTimeOffset;
      this.songTimeOffset = (float) ((double) nullable ?? (double) this.songTimeOffset);
      nullable = shuffle;
      this.shuffle = (float) ((double) nullable ?? (double) this.shuffle);
      nullable = shufflePeriod;
      this.shufflePeriod = (float) ((double) nullable ?? (double) this.shufflePeriod);
      nullable = previewStartTime;
      this.previewStartTime = (float) ((double) nullable ?? (double) this.previewStartTime);
      nullable = previewDuration;
      this.previewDuration = (float) ((double) nullable ?? (double) this.previewDuration);
      this.environmentName = environmentName ?? this.environmentName;
      this.allDirectionsEnvironmentName = allDirectionsEnvironmentName ?? this.allDirectionsEnvironmentName;
      this.difficultyBeatmapSets = (IDictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData>) difficultyBeatmapSets ?? this.difficultyBeatmapSets;
      this.environment = this.GetEnvironment(this.environmentName);
      this.environmentTrackDefinition = this._environmentDefinitions[this.environment];
      this.allDirectionsEnvironment = this.GetEnvironment(this.allDirectionsEnvironmentName);
      this.allDirectionsEnvironmentTrackDefinition = this._environmentDefinitions[this.allDirectionsEnvironment];
      if ((songFilePath ?? this.songFilePath) != this.songFilePath)
      {
        this._audioLoaded = false;
        this.LoadAudioClip(songFilePath);
      }
      this.songFilename = songFilename ?? this.songFilename;
      this.songFilePath = songFilePath;
      if ((coverImageFilePath ?? this.coverImageFilePath) != this.coverImageFilePath)
        this.LoadCoverImage(coverImageFilePath);
      this.coverImageFilename = coverImageFilename ?? this.coverImageFilename;
      this.coverImageFilePath = coverImageFilePath;
      this.isDirty = true;
      if (!clearDirty)
        return;
      this.isDirty = false;
    }

    public void UpdateSong(string songFilePath, string songFilename, AudioClip audioClip)
    {
      this.songFilePath = songFilePath;
      this.songFilename = songFilename;
      this.audioClip = audioClip;
      this._audioLoaded = true;
      this.isDirty = true;
    }

    public void UpdateCoverImage(
      string coverImageFilePath,
      string coverImageFilename,
      Texture2D coverImage)
    {
      this.coverImageFilePath = coverImageFilePath;
      this.coverImageFilename = coverImageFilename;
      this.coverImage = coverImage;
      this.isDirty = true;
    }

    public void LoadBpmData(BpmData inputBpmData, bool triggerUpdate)
    {
      if (inputBpmData == null)
        return;
      this.bpmData = inputBpmData;
      this._bpmLoaded = this.bpmData != null;
      this.isDirty = true;
      if (!triggerUpdate)
        return;
      this.CheckModelLoaded();
    }

    public void AddDifficultyBeatmap(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      BeatmapDataModel.DifficultyBeatmapData difficultyBeatmapData = new BeatmapDataModel.DifficultyBeatmapData(beatmapDifficulty, BeatmapFileUtils.GetBeatmapLevelName(beatmapCharacteristic, beatmapDifficulty));
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      if (!this.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData))
      {
        difficultyBeatmapSetData = (IDifficultyBeatmapSetData) new BeatmapDataModel.DifficultyBeatmapSetData(beatmapCharacteristic, (IDictionary<BeatmapDifficulty, IDifficultyBeatmapData>) new Dictionary<BeatmapDifficulty, IDifficultyBeatmapData>());
        this.difficultyBeatmapSets[beatmapCharacteristic] = difficultyBeatmapSetData;
      }
      difficultyBeatmapSetData.difficultyBeatmaps[beatmapDifficulty] = (IDifficultyBeatmapData) difficultyBeatmapData;
      this.isDirty = true;
    }

    public void RemoveDifficultyBeatmap(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      if (!this.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.ContainsKey(beatmapDifficulty))
        return;
      difficultyBeatmapSetData.difficultyBeatmaps.Remove(beatmapDifficulty);
      if (difficultyBeatmapSetData.difficultyBeatmaps.Count == 0)
        this.difficultyBeatmapSets.Remove(beatmapCharacteristic);
      this.isDirty = true;
    }

    public void UpdateBeatmapDifficulty(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty,
      float njs,
      float offset)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      if (!this.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.ContainsKey(beatmapDifficulty))
        return;
      difficultyBeatmapSetData.difficultyBeatmaps[beatmapDifficulty].Update(njs, offset);
      this.isDirty = true;
    }

    public void Close()
    {
      this.isDirty = false;
      this._audioLoaded = false;
      this._bpmLoaded = false;
      this.songFilePath = "";
      this.coverImageFilePath = "";
      this.bpmData = (BpmData) null;
      this._waveformDataModel.Close();
    }

    private void LoadAudioClip(string path) => this._audioClipLoader.LoadAudioFile(path, (Action<AudioClip>) (loadedAudioClip =>
    {
      this.audioClip = loadedAudioClip;
      if ((UnityEngine.Object) this.audioClip == (UnityEngine.Object) null)
      {
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapLoadErrorSignal>(new BeatmapDataModelSignals.BeatmapLoadErrorSignal(BeatmapDataModelSignals.BeatmapLoadErrorSignal.ErrorType.UnableToLoadAudio));
      }
      else
      {
        this._waveformDataModel.PrepareWaveformData(this.audioClip);
        this._audioLoaded = true;
        if (this.bpmData == null)
          this.bpmData = new BpmData(this.beatsPerMinute, this.audioClip.samples, this.audioClip.frequency, AudioTimeHelper.SecondsToSamples(this.songTimeOffset, this.audioClip.frequency));
        this._bpmLoaded = this.bpmData != null;
        this.CheckModelLoaded();
      }
    }));

    private void LoadCoverImage(string path)
    {
      if (string.IsNullOrEmpty(path))
        this.coverImage = (Texture2D) null;
      else
        SimpleTextureLoader.LoadTexture(path, false, (Action<Texture2D>) (texture => this.coverImage = texture));
    }

    private EnvironmentInfoSO GetEnvironment(string environmentName)
    {
      EnvironmentInfoSO environmentInfoSo = ((IEnumerable<EnvironmentInfoSO>) this._environmentsList.environmentInfos).FirstOrDefault<EnvironmentInfoSO>((Func<EnvironmentInfoSO, bool>) (info => info.serializedName == environmentName));
      return !((UnityEngine.Object) environmentInfoSo == (UnityEngine.Object) null) ? environmentInfoSo : this._environmentsList.environmentInfos[0];
    }

    private void CheckModelLoaded()
    {
      if (!this.beatmapDataLoaded)
        return;
      this._signalBus.Fire<BeatmapDataModelSignals.BeatmapLoadedSignal>();
    }

    public class DifficultyBeatmapSetData : IDifficultyBeatmapSetData
    {
      public BeatmapCharacteristicSO beatmapCharacteristic { get; }

      public IDictionary<BeatmapDifficulty, IDifficultyBeatmapData> difficultyBeatmaps { get; }

      public DifficultyBeatmapSetData(
        BeatmapCharacteristicSO characteristic,
        IDictionary<BeatmapDifficulty, IDifficultyBeatmapData> df)
      {
        this.beatmapCharacteristic = characteristic;
        this.difficultyBeatmaps = df;
      }
    }

    public class DifficultyBeatmapData : IDifficultyBeatmapData
    {
      public BeatmapDifficulty difficulty { get; }

      public int difficultyRank { get; }

      public string beatmapFilename { get; }

      public float noteJumpMovementSpeed { get; private set; }

      public float noteJumpStartBeatOffset { get; private set; }

      public DifficultyBeatmapData(
        StandardLevelInfoSaveData.DifficultyBeatmap serializedData)
      {
        BeatmapDifficulty difficulty;
        this.difficulty = serializedData.difficulty.BeatmapDifficultyFromSerializedName(out difficulty) ? difficulty : BeatmapDifficulty.Easy;
        this.difficultyRank = serializedData.difficultyRank;
        this.beatmapFilename = serializedData.beatmapFilename;
        this.noteJumpMovementSpeed = serializedData.noteJumpMovementSpeed;
        this.noteJumpStartBeatOffset = serializedData.noteJumpStartBeatOffset;
      }

      public DifficultyBeatmapData(BeatmapDifficulty beatmapDifficulty, string filename)
      {
        this.difficulty = beatmapDifficulty;
        this.difficultyRank = beatmapDifficulty.DefaultRating();
        this.beatmapFilename = filename;
        this.noteJumpMovementSpeed = 0.0f;
        this.noteJumpStartBeatOffset = 0.0f;
      }

      public void Update(float njs, float offset)
      {
        this.noteJumpMovementSpeed = njs;
        this.noteJumpStartBeatOffset = offset;
      }
    }
  }
}
