// Decompiled with JetBrains decompiler
// Type: StandardGameplayInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class StandardGameplayInstaller : MonoInstaller
{
  [Inject]
  protected readonly StandardGameplaySceneSetupData _standardSceneSetupData;

  public override void InstallBindings()
  {
    this.Container.Bind<EnvironmentContext>().FromInstance(EnvironmentContext.Gameplay).AsSingle();
    this.Container.Bind<PauseMenuManager.InitData>().FromInstance(new PauseMenuManager.InitData(this._standardSceneSetupData.backButtonText, this._standardSceneSetupData.previewBeatmapLevel, this._standardSceneSetupData.beatmapDifficulty, this._standardSceneSetupData.beatmapCharacteristic, true, true)).AsSingle();
    this.Container.Bind<StandardLevelFailedController.InitData>().FromInstance(new StandardLevelFailedController.InitData(this._standardSceneSetupData.autoRestart)).AsSingle();
    this.Container.Bind<PauseController.InitData>().FromInstance(new PauseController.InitData(this._standardSceneSetupData.startPaused)).AsSingle();
    this.Container.Bind<IGamePause>().To<GamePause>().AsSingle();
    this.Container.Bind<StandardLevelGameplayManager.InitData>().FromInstance(new StandardLevelGameplayManager.InitData(!this._standardSceneSetupData.gameplayModifiers.noFailOn0Energy)).AsSingle();
  }
}
