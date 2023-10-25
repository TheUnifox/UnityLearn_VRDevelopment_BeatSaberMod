// Decompiled with JetBrains decompiler
// Type: AppInit
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Zenject;

public abstract class AppInit : MonoInstaller
{
  [SerializeField]
  private GameObject _cameraGO;
  [SerializeField]
  private MultiplayerMockSettings _multiplayerMockSettings;
  [InjectOptional]
  private AppInitScenesTransitionSetupDataSO.AppInitSceneSetupData _sceneSetupData = new AppInitScenesTransitionSetupDataSO.AppInitSceneSetupData(AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.DoNotOverride, (MockPlayersModel) null);
  [Inject]
  private GameScenesManager _gameScenesManager;

  protected GameScenesManager gameScenesManager => this._gameScenesManager;

  public override void Start()
  {
    base.Start();
    switch (this.GetAppStartType())
    {
      case AppInit.AppStartType.AppStart:
      case AppInit.AppStartType.AppRestart:
        this._cameraGO.SetActive(true);
        break;
    }
    this._gameScenesManager.beforeDismissingScenesEvent += new System.Action(this.HandleBeforeDismissingScenes);
    this.StartCoroutine(this.StartCoroutine());
  }

  private IEnumerator StartCoroutine()
  {
    AppInit.AppStartType startType = this.GetAppStartType();
    yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    if (startType == AppInit.AppStartType.AppStart || startType == AppInit.AppStartType.MultiSceneEditor)
      this.AppStartAndMultiSceneEditorSetup();
    this.RepeatableSetup();
    yield return (object) null;
    if (startType != AppInit.AppStartType.MultiSceneEditor)
    {
      yield return (object) new WaitUntil((Func<bool>) (() => SplashScreen.isFinished));
      this.TransitionToNextScene();
    }
  }

  protected void OnDestroy()
  {
    if (!((UnityEngine.Object) this._gameScenesManager != (UnityEngine.Object) null))
      return;
    this._gameScenesManager.beforeDismissingScenesEvent -= new System.Action(this.HandleBeforeDismissingScenes);
  }

  private void HandleBeforeDismissingScenes()
  {
    this._gameScenesManager.beforeDismissingScenesEvent -= new System.Action(this.HandleBeforeDismissingScenes);
    this._cameraGO.SetActive(false);
  }

  protected MockPlayersModel GetMockPlayersModel()
  {
    if (this._sceneSetupData.overrideMockPlayersModel != null)
      return this._sceneSetupData.overrideMockPlayersModel;
    int num = this._multiplayerMockSettings.isEnabled ? 1 : 0;
    return (MockPlayersModel) null;
  }

  protected AppInit.AppStartType GetAppStartType()
  {
    if (this._sceneSetupData.appInitOverrideStartType != AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.DoNotOverride)
    {
      switch (this._sceneSetupData.appInitOverrideStartType)
      {
        case AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.AppStart:
          return AppInit.AppStartType.AppStart;
        case AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.AppRestart:
          return AppInit.AppStartType.AppRestart;
        case AppInitScenesTransitionSetupDataSO.AppInitOverrideStartType.MultiSceneEditor:
          return AppInit.AppStartType.MultiSceneEditor;
        default:
          return AppInit.AppStartType.AppRestart;
      }
    }
    else
      return SceneManager.sceneCount != 1 ? AppInit.AppStartType.MultiSceneEditor : AppInit.AppStartType.AppStart;
  }

  protected abstract void AppStartAndMultiSceneEditorSetup();

  protected abstract void RepeatableSetup();

  protected abstract void TransitionToNextScene();

  public enum AppStartType
  {
    AppStart,
    AppRestart,
    MultiSceneEditor,
  }
}
