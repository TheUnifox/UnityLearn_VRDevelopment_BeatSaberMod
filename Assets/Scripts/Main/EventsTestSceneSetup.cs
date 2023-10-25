// Decompiled with JetBrains decompiler
// Type: EventsTestSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EventsTestSceneSetup : MonoInstaller
{
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    BeatmapData beatmapData = new BeatmapData(4);
    this.Container.Bind<EnvironmentContext>().FromInstance(EnvironmentContext.Gameplay).AsSingle();
    this.Container.Bind<PauseMenuManager.InitData>().FromInstance(new PauseMenuManager.InitData(Localization.Get("BUTTON_MENU"), (IPreviewBeatmapLevel) null, BeatmapDifficulty.Easy, (BeatmapCharacteristicSO) null, true, true)).AsSingle();
    this.Container.Bind<ColorScheme>().FromInstance(this._sceneSetupData.colorScheme).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
    this.Container.Bind<StaticJumpOffsetYProvider.InitData>().FromInstance(new StaticJumpOffsetYProvider.InitData(0.0f)).AsSingle();
    this.Container.BindInterfacesAndSelfTo<StaticJumpOffsetYProvider>().AsSingle();
    this.Container.Bind<EnvironmentKeywords>().FromInstance(new EnvironmentKeywords((IReadOnlyList<string>) null)).AsSingle();
    this.Container.BindInterfacesAndSelfTo<MockComboController>().AsSingle();
    this.Container.BindInterfacesAndSelfTo<BeatmapCallbacksController>().AsSingle();
    this.Container.Bind<BeatmapCallbacksController.InitData>().FromInstance(new BeatmapCallbacksController.InitData((IReadonlyBeatmapData) beatmapData, 0.0f, false)).AsSingle();
    this.Container.Bind<BpmController.InitData>().FromInstance(new BpmController.InitData(60f)).AsSingle();
    this.Container.BindInterfacesAndSelfTo<BpmController>().AsSingle();
    this.Container.Bind<BeatmapData>().FromInstance(beatmapData).AsSingle();
    this.Container.Bind<IReadonlyBeatmapData>().FromInstance((IReadonlyBeatmapData) beatmapData).AsSingle();
    this.Container.Bind<SongSpeedData>().FromInstance(new SongSpeedData(1f));
    this.Container.BindInterfacesAndSelfTo<BeatmapObjectSpawnController>().FromNewComponentOnRoot().AsSingle();
    this.Container.Bind<IGamePause>().To<MockPause>().AsSingle();
    this.Container.Bind<ILevelEndActions>().To<MockLevelEndActions>().AsSingle();
    this.Container.Bind<SaberModelController.InitData>().FromInstance(new SaberModelController.InitData(new Color(1f, 1f, 1f, 1f))).AsSingle();
    this.Container.Bind<CoreGameHUDController.InitData>().FromInstance(new CoreGameHUDController.InitData(true, false, false)).AsSingle();
    this.Container.Bind<SaberManager.InitData>().FromInstance(new SaberManager.InitData(false)).AsSingle();
    this.Container.Bind<GameEnergyCounter>().FromMock<GameEnergyCounter>();
    this.Container.Bind<IGameEnergyCounter>().To<GameEnergyCounter>().FromResolve();
    this.Container.Bind<ScoreController>().FromMock<ScoreController>();
    this.Container.Bind<IScoreController>().To<ScoreController>().FromResolve();
    this.Container.Bind<RelativeScoreAndImmediateRankCounter>().FromMock<RelativeScoreAndImmediateRankCounter>();
    this.Container.Bind<BeatmapObjectSpawnController.InitData>().FromInstance(new BeatmapObjectSpawnController.InitData(0.0f, 4, 0.0f, BeatmapObjectSpawnMovementData.NoteJumpValueType.BeatOffset, 0.0f)).AsSingle();
    this.Container.Bind(typeof (IBeatmapObjectSpawner), typeof (BeatmapObjectManager)).To<MockBeatmapObjectManager>().AsSingle();
  }
}
