// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorScenesManager
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorScenesManager : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorLevelSceneTransitionSetupDataSO _beatmapEditorLevelSceneTransitionSetupData;
    [Inject]
    private readonly GameScenesManager _gameScenesManager;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BasicSpectrogramData _basicSpectrogramData;
    [Inject]
    private readonly BeatmapData _beatmapData;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    private bool _environmentOpened;

    public void OpenEnvironment(EnvironmentInfoSO environmentInfo)
    {
      if (this._environmentOpened)
        return;
      this._environmentOpened = true;
      this._beatmapEditorLevelSceneTransitionSetupData.Init(environmentInfo, this._basicSpectrogramData, this._beatmapDataModel, this._beatmapState, this._beatmapData, this._beatmapEditorSettingsDataModel);
      this._gameScenesManager.AppendScenes((ScenesTransitionSetupDataSO) this._beatmapEditorLevelSceneTransitionSetupData);
    }

    public void CloseEnvironment(EnvironmentInfoSO environmentInfo)
    {
      if (!this._environmentOpened)
        return;
      this._environmentOpened = false;
      this._beatmapEditorLevelSceneTransitionSetupData.Init(environmentInfo, this._basicSpectrogramData, this._beatmapDataModel, this._beatmapState, this._beatmapData, this._beatmapEditorSettingsDataModel);
      this._gameScenesManager.RemoveScenes((ScenesTransitionSetupDataSO) this._beatmapEditorLevelSceneTransitionSetupData);
    }

    public IEnumerator OpenEnvironmentDelayed(EnvironmentInfoSO environmentInfo)
    {
      this.OpenEnvironment(environmentInfo);
      yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    }

    public IEnumerator CloseEnvironmentDelayed(EnvironmentInfoSO environmentInfo)
    {
      this.CloseEnvironment(environmentInfo);
      yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    }
  }
}
