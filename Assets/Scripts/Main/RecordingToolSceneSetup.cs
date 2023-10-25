// Decompiled with JetBrains decompiler
// Type: RecordingToolSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class RecordingToolSceneSetup : MonoInstaller
{
  [Inject]
  protected readonly RecordingToolSceneSetupData _sceneSetupData;

  public override void InstallBindings() => this.Container.Bind<RecordingToolSettingsFlowCoordinator.InitData>().FromInstance(new RecordingToolSettingsFlowCoordinator.InitData(this._sceneSetupData.nextScenesTransitionSetupData)).AsSingle();
}
