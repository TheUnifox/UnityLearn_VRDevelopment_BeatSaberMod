// Decompiled with JetBrains decompiler
// Type: PCAppInit
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PCAppInit : AppInit
{
  [SerializeField]
  protected MainSystemInit _mainSystemInit;
  [SerializeField]
  protected OculusInit _oculusInit;
  [SerializeField]
  protected SteamInit _steamInit;
  [SerializeField]
  protected DefaultScenesTransitionsFromInit _defaultScenesTransitionsFromInit;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [Space]
  [SerializeField]
  protected string _goStraightToMenuCommandArgument;
  [SerializeField]
  protected string _goStraightToEditorCommandArgument;

  protected override void AppStartAndMultiSceneEditorSetup() => this._oculusInit.Init();

  protected override void RepeatableSetup()
  {
    this._mainSettingsModel.Load(false);
    MainSettingsDefaultValues.SetFixedDefaultValues(this._mainSettingsModel);
    this._mainSystemInit.Init();
  }

  protected override void TransitionToNextScene()
  {
    RecordingToolManager recordingToolManager = this.Container.TryResolve<RecordingToolManager>();
    bool goToRecordingToolScene = recordingToolManager != null && recordingToolManager.showRecordingToolScene;
    this._defaultScenesTransitionsFromInit.TransitionToNextScene(this.GetAppStartType() == AppInit.AppStartType.AppRestart || CommandLineArguments.Contains(this._goStraightToMenuCommandArgument), CommandLineArguments.Contains(this._goStraightToEditorCommandArgument), goToRecordingToolScene);
  }

  public override void InstallBindings()
  {
    this._mainSystemInit.PreInstall(this.GetMockPlayersModel());
    this.Container.Bind<MenuScenesTransitionSetupDataSO>().FromInstance(this._defaultScenesTransitionsFromInit.mainMenuScenesTransitionSetupData).AsSingle();
    if (OculusInit.__enabled)
      this.Container.Bind<IAnalyticsModel>().To<OculusAnalyticsModel>().AsSingle();
    else
      this.Container.Bind<IAnalyticsModel>().To<NoAnalyticsModel>().AsSingle();
    this.Container.Bind<IExperimentModel>().To<NoExperimentModel>().AsSingle();
    this._mainSystemInit.InstallBindings(this.Container);
  }
}
