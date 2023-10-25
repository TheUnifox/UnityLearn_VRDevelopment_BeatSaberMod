// Decompiled with JetBrains decompiler
// Type: PS4AppInit
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class PS4AppInit : AppInit
{
  [SerializeField]
  protected MainSystemInit _mainSystemInit;
  [SerializeField]
  protected DefaultScenesTransitionsFromInit _defaultScenesTransitionsFromInit;
  [SerializeField]
  protected AppInitScenesTransitionSetupDataContainerSO _appInitScenesTransitionSetupDataContainer;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [SerializeField]
  protected PS4ActivePublisherSKUSettingsSO _activePublisherSKUSettingsSO;
  [Inject]
  protected GameScenesManager _gameScenesManager;

  protected override void AppStartAndMultiSceneEditorSetup() => Debug.LogError((object) "Trying to run Platform Init Scene on a different platform.");

  protected override void RepeatableSetup()
  {
    this._mainSettingsModel.Load(false);
    MainSettingsDefaultValues.SetFixedDefaultValues(this._mainSettingsModel);
    this._mainSystemInit.Init();
  }

  protected override void TransitionToNextScene() => this._defaultScenesTransitionsFromInit.TransitionToNextScene(this.GetAppStartType() == AppInit.AppStartType.AppRestart, false, false);

  public override void InstallBindings()
  {
    this._mainSystemInit.PreInstall(this.GetMockPlayersModel());
    this.Container.Bind<MenuScenesTransitionSetupDataSO>().FromInstance(this._defaultScenesTransitionsFromInit.mainMenuScenesTransitionSetupData).AsSingle();
    this.Container.Bind<IAnalyticsModel>().To<NoAnalyticsModel>().AsSingle();
    this.Container.Bind<IExperimentModel>().To<NoExperimentModel>().AsSingle();
    this._mainSystemInit.InstallBindings(this.Container);
  }
}
