// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorLevelSceneTransitionSetupDataSO
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorLevelSceneTransitionSetupDataSO : ScenesTransitionSetupDataSO
  {
    [SerializeField]
    private SceneInfo _gameCoreSceneInfo;
    [SerializeField]
    private SceneInfo _beatmapLevelEditorSceneInfo;
    [SerializeField]
    private SceneInfo _beatmapLevelEditorWorldUISceneInfo;

    public void Init(
      EnvironmentInfoSO environmentInfo,
      BasicSpectrogramData basicSpectrogramData,
      BeatmapDataModel beatmapDataModel,
      IReadonlyBeatmapState beatmapState,
      BeatmapData beatmapData,
      BeatmapEditorSettingsDataModel beatmapEditorSettingsDataModel)
    {
      SceneSetupData[] sceneSetupData = new SceneSetupData[2]
      {
        (SceneSetupData) new EditorGameplayCoreSceneSetupData(environmentInfo.colorScheme, basicSpectrogramData, beatmapDataModel, beatmapState, beatmapData, beatmapEditorSettingsDataModel),
        (SceneSetupData) new EnvironmentSceneSetupData(environmentInfo, (IPreviewBeatmapLevel) new BeatmapEditorPreviewBeatmapLevel(beatmapDataModel.coverImage), false)
      };
      this.Init(new SceneInfo[4]
      {
        environmentInfo.sceneInfo,
        this._beatmapLevelEditorSceneInfo,
        this._gameCoreSceneInfo,
        this._beatmapLevelEditorWorldUISceneInfo
      }, sceneSetupData);
    }
  }
}
