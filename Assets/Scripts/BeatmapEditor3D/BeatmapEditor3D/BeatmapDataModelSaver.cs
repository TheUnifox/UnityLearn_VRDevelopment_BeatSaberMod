// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapDataModelSaver
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapDataModelSaver
  {
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;

    public StandardLevelInfoSaveData Save() => new StandardLevelInfoSaveData(this._beatmapDataModel.songName, this._beatmapDataModel.songSubName, this._beatmapDataModel.songAuthorName, this._beatmapDataModel.levelAuthorName, this._beatmapDataModel.beatsPerMinute, this._beatmapDataModel.songTimeOffset, this._beatmapDataModel.shuffle, this._beatmapDataModel.shufflePeriod, this._beatmapDataModel.previewStartTime, this._beatmapDataModel.previewDuration, this._beatmapDataModel.songFilename, this._beatmapDataModel.coverImageFilename, this._beatmapDataModel.environmentName, this._beatmapDataModel.allDirectionsEnvironmentName, this._beatmapDataModel.difficultyBeatmapSets.Values.Select<IDifficultyBeatmapSetData, StandardLevelInfoSaveData.DifficultyBeatmapSet>((Func<IDifficultyBeatmapSetData, StandardLevelInfoSaveData.DifficultyBeatmapSet>) (difficultyBeatmapSet => new StandardLevelInfoSaveData.DifficultyBeatmapSet(difficultyBeatmapSet.beatmapCharacteristic.serializedName, difficultyBeatmapSet.difficultyBeatmaps.Values.OrderBy<IDifficultyBeatmapData, BeatmapDifficulty>((Func<IDifficultyBeatmapData, BeatmapDifficulty>) (a => a.difficulty)).Select<IDifficultyBeatmapData, StandardLevelInfoSaveData.DifficultyBeatmap>((Func<IDifficultyBeatmapData, StandardLevelInfoSaveData.DifficultyBeatmap>) (difficultyBeatmapData => new StandardLevelInfoSaveData.DifficultyBeatmap(difficultyBeatmapData.difficulty.SerializedName(), difficultyBeatmapData.difficultyRank, difficultyBeatmapData.beatmapFilename, difficultyBeatmapData.noteJumpMovementSpeed, difficultyBeatmapData.noteJumpStartBeatOffset))).ToArray<StandardLevelInfoSaveData.DifficultyBeatmap>()))).ToArray<StandardLevelInfoSaveData.DifficultyBeatmapSet>());
  }
}
