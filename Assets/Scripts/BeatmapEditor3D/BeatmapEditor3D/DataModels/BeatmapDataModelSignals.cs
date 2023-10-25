// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapDataModelSignals
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public static class BeatmapDataModelSignals
  {
    public class UpdateBeatmapDataSignal
    {
      public string songName { get; }

      public string songSubName { get; }

      public string songAuthorName { get; }

      public string levelAuthorName { get; }

      public float? beatsPerMinute { get; }

      public float? songTimeOffset { get; }

      public float? shuffle { get; }

      public float? shufflePeriod { get; }

      public float? previewStartTime { get; }

      public float? previewDuration { get; }

      public string songFilename { get; }

      public string coverImageFilename { get; }

      public string environmentName { get; }

      public string allDirectionsEnvironmentName { get; }

      public Dictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData> difficultyBeatmapSets { get; }

      public UpdateBeatmapDataSignal(
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
        string coverImageFilename = null,
        string environmentName = null,
        string allDirectionsEnvironmentName = null,
        Dictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData> difficultyBeatmapSets = null)
      {
        this.songName = songName;
        this.songSubName = songSubName;
        this.songAuthorName = songAuthorName;
        this.levelAuthorName = levelAuthorName;
        this.beatsPerMinute = beatsPerMinute;
        this.songTimeOffset = songTimeOffset;
        this.shuffle = shuffle;
        this.shufflePeriod = shufflePeriod;
        this.previewStartTime = previewStartTime;
        this.previewDuration = previewDuration;
        this.songFilename = songFilename;
        this.coverImageFilename = coverImageFilename;
        this.environmentName = environmentName;
        this.allDirectionsEnvironmentName = allDirectionsEnvironmentName;
        this.difficultyBeatmapSets = difficultyBeatmapSets;
      }
    }

    public class UpdateBeatmapSongSignal
    {
      public string songFilePath { get; }

      public AudioClip audioClip { get; }

      public UpdateBeatmapSongSignal(string songFilePath, AudioClip audioClip)
      {
        this.songFilePath = songFilePath;
        this.audioClip = audioClip;
      }
    }

    public class UpdateBeatmapCoverImageSignal
    {
      public readonly string coverImageFileName;
      public readonly Texture2D coverImage;

      public UpdateBeatmapCoverImageSignal(string coverImageFileName, Texture2D coverImage)
      {
        this.coverImageFileName = coverImageFileName;
        this.coverImage = coverImage;
      }
    }

    public class AddDifficultyBeatmapSignal
    {
      public BeatmapCharacteristicSO beatmapCharacteristic { get; }

      public BeatmapDifficulty difficulty { get; }

      public AddDifficultyBeatmapSignal(
        BeatmapCharacteristicSO beatmapCharacteristic,
        BeatmapDifficulty difficulty)
      {
        this.beatmapCharacteristic = beatmapCharacteristic;
        this.difficulty = difficulty;
      }
    }

    public class DifficultyBeatmapAddedSignal
    {
      public readonly BeatmapCharacteristicSO beatmapCharacteristic;
      public readonly BeatmapDifficulty difficulty;

      public DifficultyBeatmapAddedSignal(
        BeatmapCharacteristicSO beatmapCharacteristic,
        BeatmapDifficulty difficulty)
      {
        this.beatmapCharacteristic = beatmapCharacteristic;
        this.difficulty = difficulty;
      }
    }

    public class RemoveDifficultyBeatmapSignal
    {
      public readonly BeatmapCharacteristicSO beatmapCharacteristic;
      public readonly BeatmapDifficulty beatmapDifficulty;

      public RemoveDifficultyBeatmapSignal(
        BeatmapCharacteristicSO beatmapCharacteristic,
        BeatmapDifficulty beatmapDifficulty)
      {
        this.beatmapCharacteristic = beatmapCharacteristic;
        this.beatmapDifficulty = beatmapDifficulty;
      }
    }

    public class UpdateDifficultyBeatmapSignal
    {
      public readonly BeatmapCharacteristicSO beatmapCharacteristic;
      public readonly BeatmapDifficulty beatmapDifficulty;
      public readonly float njs;
      public readonly float offset;

      public UpdateDifficultyBeatmapSignal(
        BeatmapCharacteristicSO beatmapCharacteristic,
        BeatmapDifficulty beatmapDifficulty,
        float njs,
        float offset)
      {
        this.beatmapCharacteristic = beatmapCharacteristic;
        this.beatmapDifficulty = beatmapDifficulty;
        this.njs = njs;
        this.offset = offset;
      }
    }

    public class BeatmapLoadedSignal
    {
    }

    public class WaveformDataProcessedSignal
    {
    }

    public class BeatmapLoadErrorSignal
    {
      public readonly BeatmapDataModelSignals.BeatmapLoadErrorSignal.ErrorType errorType;

      public BeatmapLoadErrorSignal(
        BeatmapDataModelSignals.BeatmapLoadErrorSignal.ErrorType errorType)
      {
        this.errorType = errorType;
      }

      public enum ErrorType
      {
        UnableToLoadAudio,
      }
    }

    public class BeatmapUpdatedSignal
    {
    }

    public class UpdateBeatmapDataCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapDataModelSignals.UpdateBeatmapDataSignal _signal;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._beatmapDataModel.UpdateWith(this._signal.songName, this._signal.songSubName, this._signal.songAuthorName, this._signal.levelAuthorName, this._signal.beatsPerMinute, this._signal.songTimeOffset, this._signal.shuffle, this._signal.shufflePeriod, this._signal.previewStartTime, this._signal.previewDuration, (string) null, this._signal.songFilename, (string) null, this._signal.coverImageFilename, this._signal.environmentName, this._signal.allDirectionsEnvironmentName, this._signal.difficultyBeatmapSets, false);
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapUpdatedSignal>();
      }
    }

    public class UpdateBeatmapSongCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapDataModelSignals.UpdateBeatmapSongSignal _signal;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._beatmapProjectManager.SaveBeatmapSong(this._signal.songFilePath, this._signal.audioClip);
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapUpdatedSignal>();
      }
    }

    public class UpdateBeatmapCoverImageCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapDataModelSignals.UpdateBeatmapCoverImageSignal _signal;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._beatmapProjectManager.SaveBeatmapCoverImage(this._signal.coverImageFileName, this._signal.coverImage);
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapUpdatedSignal>();
      }
    }

    public class AddDifficultyBeatmapCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapDataModelSignals.AddDifficultyBeatmapSignal _signal;
      [Inject]
      private readonly SignalBus _signalBus;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute()
      {
        this._beatmapDataModel.AddDifficultyBeatmap(this._signal.beatmapCharacteristic, this._signal.difficulty);
        this._beatmapProjectManager.SaveEmptyBeatmapLevel(this._signal.beatmapCharacteristic, this._signal.difficulty);
        this._signalBus.Fire<BeatmapDataModelSignals.DifficultyBeatmapAddedSignal>(new BeatmapDataModelSignals.DifficultyBeatmapAddedSignal(this._signal.beatmapCharacteristic, this._signal.difficulty));
      }
    }

    public class RemoveDifficultyBeatmapCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapDataModelSignals.RemoveDifficultyBeatmapSignal _signal;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._beatmapProjectManager.RemoveBeatmapLevel(this._signal.beatmapCharacteristic, this._signal.beatmapDifficulty);
        this._beatmapDataModel.RemoveDifficultyBeatmap(this._signal.beatmapCharacteristic, this._signal.beatmapDifficulty);
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapLoadedSignal>();
      }
    }

    public class UpdateDifficultyBeatmapCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapDataModelSignals.UpdateDifficultyBeatmapSignal _signal;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._beatmapDataModel.UpdateBeatmapDifficulty(this._signal.beatmapCharacteristic, this._signal.beatmapDifficulty, this._signal.njs, this._signal.offset);
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapUpdatedSignal>();
      }
    }
  }
}
