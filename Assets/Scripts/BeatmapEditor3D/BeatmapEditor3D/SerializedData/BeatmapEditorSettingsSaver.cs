// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SerializedData.BeatmapEditorSettingsSaver
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.SerializedData
{
  public class BeatmapEditorSettingsSaver
  {
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    public BeatmapEditorSettingsSerializedData Save() => new BeatmapEditorSettingsSerializedData(this._beatmapEditorSettingsDataModel.customLevelsFolder, this._beatmapEditorSettingsDataModel.recentlyOpenedBeatmaps.ToList<string>(), this._beatmapEditorSettingsDataModel.editorWindowResolution, this._beatmapEditorSettingsDataModel.invertSubdivisionScroll, this._beatmapEditorSettingsDataModel.invertTimelineScroll, this._beatmapEditorSettingsDataModel.staticLights, this._beatmapEditorSettingsDataModel.autoExposure, this._beatmapEditorSettingsDataModel.enableFpfc, this._beatmapEditorSettingsDataModel.preserveTime, this._beatmapEditorSettingsDataModel.playbackSpeed, (BeatmapEditorSettingsSerializedData.WaveformType) this._beatmapEditorSettingsDataModel.waveformType, (BeatmapEditorSettingsSerializedData.GameplayUIState) this._beatmapEditorSettingsDataModel.gameplayUIState);
  }
}
