// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditorGameplayCoreSceneSetupData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public class EditorGameplayCoreSceneSetupData : SceneSetupData
  {
    public readonly ColorSchemeSO colorScheme;
    public readonly BasicSpectrogramData basicSpectrogramData;
    public readonly IReadonlyBeatmapState beatmapState;
    public readonly BeatmapDataModel beatmapDataModel;
    public readonly BeatmapData beatmapData;
    public readonly BeatmapEditorSettingsDataModel beatmapEditorSettingsDataModel;

    public EditorGameplayCoreSceneSetupData(
      ColorSchemeSO colorScheme,
      BasicSpectrogramData basicSpectrogramData,
      BeatmapDataModel beatmapDataModel,
      IReadonlyBeatmapState beatmapState,
      BeatmapData beatmapData,
      BeatmapEditorSettingsDataModel beatmapEditorSettingsDataModel)
    {
      this.colorScheme = colorScheme;
      this.basicSpectrogramData = basicSpectrogramData;
      this.beatmapState = beatmapState;
      this.beatmapDataModel = beatmapDataModel;
      this.beatmapData = beatmapData;
      this.beatmapEditorSettingsDataModel = beatmapEditorSettingsDataModel;
    }
  }
}
