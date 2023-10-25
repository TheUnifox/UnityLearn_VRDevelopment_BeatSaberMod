// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapDataModelVersionedLoader
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeatmapEditor3D
{
  public class BeatmapDataModelVersionedLoader
  {
    private readonly IBeatmapDataModel _beatmapDataModel;
    private readonly BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
    private readonly Version _version200 = new Version(2, 0, 0);

    public BeatmapDataModelVersionedLoader(
      IBeatmapDataModel beatmapDataModel,
      BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection)
    {
      this._beatmapDataModel = beatmapDataModel;
      this._beatmapCharacteristicCollection = beatmapCharacteristicCollection;
    }

    public void Load(string projectPath)
    {
      if (BeatmapProjectFileHelper.GetVersionedJSONVersion(projectPath, "Info.dat") < this._version200)
        this.Load_v1();
      else
        this.LoadCurrent(projectPath);
    }

    private void LoadCurrent(string projectPath)
    {
      StandardLevelInfoSaveData levelInfoSaveData = BeatmapProjectFileHelper.LoadProjectInfo(projectPath);
      if (levelInfoSaveData == null)
        return;
      (string str1, string str2) = BeatmapProjectFileHelper.GetCorrectedPathAndFilename(projectPath, levelInfoSaveData.songFilename);
      (string str3, string str4) = BeatmapProjectFileHelper.GetCorrectedPathAndFilename(projectPath, levelInfoSaveData.coverImageFilename);
      this._beatmapDataModel.UpdateWith(levelInfoSaveData.songName, levelInfoSaveData.songSubName, levelInfoSaveData.songAuthorName, levelInfoSaveData.levelAuthorName, new float?(levelInfoSaveData.beatsPerMinute), new float?(levelInfoSaveData.songTimeOffset), new float?(levelInfoSaveData.shuffle), new float?(levelInfoSaveData.shufflePeriod), new float?(levelInfoSaveData.previewStartTime), new float?(levelInfoSaveData.previewDuration), str2, str1, str4, str3, levelInfoSaveData.environmentName, levelInfoSaveData.allDirectionsEnvironmentName, ((IEnumerable<IDifficultyBeatmapSetData>) ((IEnumerable<StandardLevelInfoSaveData.DifficultyBeatmapSet>) levelInfoSaveData.difficultyBeatmapSets).Select<StandardLevelInfoSaveData.DifficultyBeatmapSet, BeatmapDataModel.DifficultyBeatmapSetData>((Func<StandardLevelInfoSaveData.DifficultyBeatmapSet, BeatmapDataModel.DifficultyBeatmapSetData>) (beatmapSetData => new BeatmapDataModel.DifficultyBeatmapSetData(this._beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(beatmapSetData.beatmapCharacteristicName), (IDictionary<BeatmapDifficulty, IDifficultyBeatmapData>) ((IEnumerable<IDifficultyBeatmapData>) ((IEnumerable<StandardLevelInfoSaveData.DifficultyBeatmap>) beatmapSetData.difficultyBeatmaps).Select<StandardLevelInfoSaveData.DifficultyBeatmap, BeatmapDataModel.DifficultyBeatmapData>((Func<StandardLevelInfoSaveData.DifficultyBeatmap, BeatmapDataModel.DifficultyBeatmapData>) (data => new BeatmapDataModel.DifficultyBeatmapData(data)))).ToDictionary<IDifficultyBeatmapData, BeatmapDifficulty>((Func<IDifficultyBeatmapData, BeatmapDifficulty>) (data => data.difficulty)))))).ToDictionary<IDifficultyBeatmapSetData, BeatmapCharacteristicSO>((Func<IDifficultyBeatmapSetData, BeatmapCharacteristicSO>) (data => data.beatmapCharacteristic)), true);
    }

    private void Load_v1() => throw new NotImplementedException("Load v1 not yet implemented");
  }
}
