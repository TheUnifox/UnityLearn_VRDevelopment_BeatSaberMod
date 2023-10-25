// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IBeatmapDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public interface IBeatmapDataModel : IReadonlyBeatmapDataModel
  {
    new string version { get; set; }

    new string songName { get; set; }

    new string songSubName { get; set; }

    new string songAuthorName { get; set; }

    new string levelAuthorName { get; set; }

    new float beatsPerMinute { get; set; }

    new float songTimeOffset { get; set; }

    new float shuffle { get; set; }

    new float shufflePeriod { get; set; }

    new float previewStartTime { get; set; }

    new float previewDuration { get; set; }

    new string songFilename { get; set; }

    new string songFilePath { get; set; }

    new AudioClip audioClip { get; set; }

    new BpmData bpmData { get; set; }

    new string coverImageFilename { get; set; }

    new string coverImageFilePath { get; set; }

    new Texture2D coverImage { get; set; }

    new string environmentName { get; set; }

    new EnvironmentInfoSO environment { get; set; }

    new EnvironmentTracksDefinitionSO environmentTrackDefinition { get; set; }

    new string allDirectionsEnvironmentName { get; set; }

    new EnvironmentInfoSO allDirectionsEnvironment { get; set; }

    new EnvironmentTracksDefinitionSO allDirectionsEnvironmentTrackDefinition { get; set; }

    new IDictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData> difficultyBeatmapSets { get; set; }

    void UpdateWith(
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
      bool clearDirty = false);

    void LoadBpmData(BpmData bpmData, bool triggerUpdate);

    void Close();
  }
}
