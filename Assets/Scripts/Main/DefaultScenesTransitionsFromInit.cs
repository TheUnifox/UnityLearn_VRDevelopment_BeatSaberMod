// Decompiled with JetBrains decompiler
// Type: DefaultScenesTransitionsFromInit
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class DefaultScenesTransitionsFromInit : MonoBehaviour
{
  [SerializeField]
  protected HealthWarningScenesTransitionSetupDataSO _healthWarningScenesTransitionSetupData;
  [SerializeField]
  protected RecordingToolScenesTransitionSetupDataSO _recordingToolScenesTransitionSetupData;
  [SerializeField]
  protected MenuScenesTransitionSetupDataSO _mainMenuScenesTransitionSetupData;
  [SerializeField]
  protected BeatmapEditorScenesTransitionSetupDataSO _beatmapEditorScenesTransitionSetupData;
  [SerializeField]
  protected ShaderWarmupScenesTransitionSetupDataSO _shaderWarmupScenesTransitionSetupData;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;

  public MenuScenesTransitionSetupDataSO mainMenuScenesTransitionSetupData => this._mainMenuScenesTransitionSetupData;

  public virtual void TransitionToNextScene(
    bool goStraightToMenu,
    bool goStraightToEditor,
    bool goToRecordingToolScene)
  {
    this._mainMenuScenesTransitionSetupData.Init();
    if (goStraightToMenu)
    {
      this._shaderWarmupScenesTransitionSetupData.Init(new ShaderWarmupSceneSetupData((ScenesTransitionSetupDataSO) this._mainMenuScenesTransitionSetupData));
      this._gameScenesManager.ReplaceScenes((ScenesTransitionSetupDataSO) this._shaderWarmupScenesTransitionSetupData);
    }
    else if (goStraightToEditor)
    {
      this._beatmapEditorScenesTransitionSetupData.Init(true);
      this._gameScenesManager.ReplaceScenes((ScenesTransitionSetupDataSO) this._beatmapEditorScenesTransitionSetupData);
    }
    else if (goToRecordingToolScene)
    {
      this._shaderWarmupScenesTransitionSetupData.Init(new ShaderWarmupSceneSetupData((ScenesTransitionSetupDataSO) this._mainMenuScenesTransitionSetupData));
      this._recordingToolScenesTransitionSetupData.Init(new RecordingToolSceneSetupData((ScenesTransitionSetupDataSO) this._shaderWarmupScenesTransitionSetupData));
      this._gameScenesManager.ReplaceScenes((ScenesTransitionSetupDataSO) this._recordingToolScenesTransitionSetupData);
    }
    else
    {
      this._shaderWarmupScenesTransitionSetupData.Init(new ShaderWarmupSceneSetupData((ScenesTransitionSetupDataSO) this._mainMenuScenesTransitionSetupData));
      this._healthWarningScenesTransitionSetupData.Init(new HealthWarningSceneSetupData((ScenesTransitionSetupDataSO) this._shaderWarmupScenesTransitionSetupData));
      this._gameScenesManager.ReplaceScenes((ScenesTransitionSetupDataSO) this._healthWarningScenesTransitionSetupData);
    }
  }
}
