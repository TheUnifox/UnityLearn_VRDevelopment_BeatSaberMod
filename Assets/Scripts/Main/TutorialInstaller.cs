// Decompiled with JetBrains decompiler
// Type: TutorialInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
  [SerializeField]
  protected AudioClip _audioClip;
  [SerializeField]
  protected float _songBPM = 1f;
  [Space]
  [SerializeField]
  protected PlayerHeightDetector _playerHeightDetectorPrefab;
  [SerializeField]
  protected EffectPoolsManualInstaller _effectPoolsManualInstaller;
  [Inject]
  protected readonly TutorialSceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    BeatmapData beatmapData = new BeatmapData(4);
    PlayerSpecificSettings specificSettings = this._sceneSetupData.playerSpecificSettings;
    this.Container.Bind<EnvironmentContext>().FromInstance(EnvironmentContext.Gameplay).AsSingle();
    this.Container.Bind<PauseController.InitData>().FromInstance(new PauseController.InitData(false));
    this.Container.Bind<PauseMenuManager.InitData>().FromInstance(new PauseMenuManager.InitData(Localization.Get("BUTTON_MENU"), (IPreviewBeatmapLevel) null, BeatmapDifficulty.Easy, (BeatmapCharacteristicSO) null, true, false)).AsSingle();
    this.Container.Bind<EnvironmentKeywords>().FromInstance(new EnvironmentKeywords((IReadOnlyList<string>) null)).AsSingle();
    this.Container.Bind<AudioTimeSyncController.InitData>().FromInstance(new AudioTimeSyncController.InitData(this._audioClip, 0.0f, 0.0f, 1f)).AsSingle();
    this.Container.Bind<TutorialSongController.InitData>().FromInstance(new TutorialSongController.InitData(this._songBPM, beatmapData)).AsSingle();
    this.Container.BindInterfacesAndSelfTo<BeatmapCallbacksController>().AsSingle();
    this.Container.Bind<BeatmapCallbacksController.InitData>().FromInstance(new BeatmapCallbacksController.InitData((IReadonlyBeatmapData) beatmapData, 0.0f, false)).AsSingle();
    this.Container.Bind<ColorScheme>().FromInstance(this._sceneSetupData.colorScheme).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
    this.Container.Bind<BeatmapObjectSpawnController.InitData>().FromInstance(new BeatmapObjectSpawnController.InitData(this._songBPM, 4, 10f, BeatmapObjectSpawnMovementData.NoteJumpValueType.BeatOffset, 2f)).AsSingle();
    this.Container.Bind<TutorialBeatmapObjectManager.InitData>().FromInstance(new TutorialBeatmapObjectManager.InitData(60f));
    this.Container.Bind(typeof (BeatmapObjectManager), typeof (IBeatmapObjectSpawner)).To<TutorialBeatmapObjectManager>().AsSingle();
    this.Container.Bind<AutomaticSFXVolume.InitData>().FromInstance(new AutomaticSFXVolume.InitData(0.0f, true)).AsSingle();
    if (specificSettings.automaticPlayerHeight)
    {
      this.Container.Bind<PlayerHeightDetector.InitData>().FromInstance(new PlayerHeightDetector.InitData(0.1f, specificSettings.playerHeight)).AsSingle();
      this.Container.Bind<PlayerHeightDetector>().FromComponentInNewPrefab((Object) this._playerHeightDetectorPrefab).AsSingle().NonLazy();
      this.Container.BindInterfacesAndSelfTo<PlayerHeightToJumpOffsetYProvider>().AsSingle();
    }
    else
    {
      this.Container.Bind<StaticJumpOffsetYProvider.InitData>().FromInstance(new StaticJumpOffsetYProvider.InitData(0.0f)).AsSingle();
      this.Container.BindInterfacesAndSelfTo<StaticJumpOffsetYProvider>().AsSingle();
    }
    this.Container.Bind<SaberManager.InitData>().FromInstance(new SaberManager.InitData(false)).AsSingle();
    this.Container.Bind<SaberModelController.InitData>().FromInstance(new SaberModelController.InitData(new Color(1f, 1f, 1f, 0.5f))).AsSingle();
    this.Container.Bind<IGamePause>().To<TutorialPause>().AsSingle();
    this._effectPoolsManualInstaller.ManualInstallBindings(this.Container, false);
    this.Container.Bind<BeatEffectSpawner.InitData>().FromInstance(new BeatEffectSpawner.InitData(false));
    this.Container.Bind<CoreGameHUDController.InitData>().FromInstance(new CoreGameHUDController.InitData(false, false, false)).AsSingle().IfNotBound();
    this.Container.Bind<SaberClashChecker>().AsSingle();
  }
}
