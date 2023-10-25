// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SerializedData.BeatmapEditorSettingsLoader
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using Zenject;

namespace BeatmapEditor3D.SerializedData
{
  public class BeatmapEditorSettingsLoader
  {
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [DoesNotRequireDomainReloadInit]
    private static readonly Version version1 = new Version("0.1.0");

    public void Load(string path)
    {
      BeatmapEditorSettingsSerializedData serializedData = FileHelpers.LoadFromJSONFile<BeatmapEditorSettingsSerializedData>(path) ?? new BeatmapEditorSettingsSerializedData();
      this.ProvideDefaultData(serializedData);
      this._beatmapEditorSettingsDataModel.UpdateWith(serializedData.customLevelsFolder, serializedData.recentlyOpenedBeatmaps, serializedData.editorWindowResolution, serializedData.invertSubdivisionScroll, serializedData.invertTimelineScroll, serializedData.staticLights, serializedData.autoExposure, serializedData.enableFpfc, serializedData.preserveTime, serializedData.playbackSpeed, (BeatmapEditor3D.Types.WaveformType) serializedData.waveformType, (BeatmapEditor3D.Types.GameplayUIState) serializedData.gameplayUIState);
    }

    private void ProvideDefaultData(BeatmapEditorSettingsSerializedData serializedData)
    {
      if (new Version(serializedData.version) >= BeatmapEditorSettingsLoader.version1)
        return;
      serializedData.staticLights = true;
      serializedData.enableFpfc = true;
      serializedData.preserveTime = true;
      serializedData.playbackSpeed = 1f;
      serializedData.autoExposure = true;
      serializedData.waveformType = BeatmapEditorSettingsSerializedData.WaveformType.Expanded;
      serializedData.gameplayUIState = BeatmapEditorSettingsSerializedData.GameplayUIState.Hidden;
    }
  }
}
