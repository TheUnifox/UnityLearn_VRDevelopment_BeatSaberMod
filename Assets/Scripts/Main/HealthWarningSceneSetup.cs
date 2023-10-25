// Decompiled with JetBrains decompiler
// Type: HealthWarningSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class HealthWarningSceneSetup : MonoInstaller
{
  [Inject]
  protected readonly HealthWarningSceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    this.Container.Bind<HealthWarningFlowCoordinator.InitData>().FromInstance(new HealthWarningFlowCoordinator.InitData(this._sceneSetupData.nextScenesTransitionSetupData)).AsSingle();
    this.Container.Bind<SafeAreaRectChecker.InitData>().FromInstance(new SafeAreaRectChecker.InitData(false)).AsSingle();
    this.Container.Bind<EulaViewController.InitData>().FromInstance(new EulaViewController.InitData(true)).AsSingle();
  }
}
