// Decompiled with JetBrains decompiler
// Type: MissionGameplayInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class MissionGameplayInstaller : MonoInstaller
{
  [Inject]
  protected readonly MissionGameplaySceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    this.Container.Bind<EnvironmentContext>().FromInstance(EnvironmentContext.Gameplay).AsSingle();
    this.Container.Bind<MissionObjectiveCheckersManager.InitData>().FromInstance(new MissionObjectiveCheckersManager.InitData(this._sceneSetupData.missionObjectives)).AsSingle();
    this.Container.Bind<ScoreUIController.InitData>().FromInstance(new ScoreUIController.InitData(ScoreUIController.ScoreDisplayType.MultipliedScore)).AsSingle();
    this.Container.Bind<PauseMenuManager.InitData>().FromInstance(new PauseMenuManager.InitData(this._sceneSetupData.backButtonText, this._sceneSetupData.previewBeatmapLevel, this._sceneSetupData.beatmapDifficulty, this._sceneSetupData.beatmapCharacteristic, true, true)).AsSingle();
    this.Container.Bind<MissionLevelFailedController.InitData>().FromInstance(new MissionLevelFailedController.InitData(this._sceneSetupData.autoRestart)).AsSingle();
    this.Container.Bind<MissionLevelGameplayManager.InitData>().FromInstance(new MissionLevelGameplayManager.InitData(!this._sceneSetupData.gameplayModifiers.noFailOn0Energy)).AsSingle();
    this.Container.Bind<PauseController.InitData>().FromInstance(new PauseController.InitData(false)).AsSingle();
    this.Container.Bind<IGamePause>().To<GamePause>().AsSingle();
  }
}
