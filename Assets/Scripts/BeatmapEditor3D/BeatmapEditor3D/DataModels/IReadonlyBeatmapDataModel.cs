// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IReadonlyBeatmapDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public interface IReadonlyBeatmapDataModel
  {
    bool beatmapDataLoaded { get; }

    string version { get; }

    string songName { get; }

    string songSubName { get; }

    string songAuthorName { get; }

    string levelAuthorName { get; }

    float beatsPerMinute { get; }

    float songTimeOffset { get; }

    float shuffle { get; }

    float shufflePeriod { get; }

    float previewStartTime { get; }

    float previewDuration { get; }

    string songFilename { get; }

    string songFilePath { get; }

    AudioClip audioClip { get; }

    BpmData bpmData { get; }

    string coverImageFilename { get; }

    string coverImageFilePath { get; }

    Texture2D coverImage { get; }

    string environmentName { get; }

    EnvironmentInfoSO environment { get; }

    EnvironmentTracksDefinitionSO environmentTrackDefinition { get; }

    string allDirectionsEnvironmentName { get; }

    EnvironmentInfoSO allDirectionsEnvironment { get; }

    EnvironmentTracksDefinitionSO allDirectionsEnvironmentTrackDefinition { get; }

    IDictionary<BeatmapCharacteristicSO, IDifficultyBeatmapSetData> difficultyBeatmapSets { get; }
  }
}
