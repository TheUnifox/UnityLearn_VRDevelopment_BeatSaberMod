// Decompiled with JetBrains decompiler
// Type: PatternFightSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PatternFightSceneSetup : MonoInstaller
{
  [SerializeField]
  protected AudioClip _testAudioClip;
  [SerializeField]
  protected float _testAudioClipBPM = 120f;
  [SerializeField]
  protected PlayerHeightDetector _playerHeightDetectorPrefab;
  [Space]
  [SerializeField]
  protected AudioManagerSO _audioMixer;
  [Inject]
  protected readonly PatternFightSceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    PlayerSpecificSettings specificSettings = this._sceneSetupData.playerSpecificSettings;
    BeatmapData beatmapData = new BeatmapData(4);
    this._audioMixer.musicPitch = 1f;
    this.Container.Bind<EnvironmentContext>().FromInstance(EnvironmentContext.Gameplay).AsSingle();
    this.Container.Bind<ColorScheme>().FromInstance(this._sceneSetupData.colorScheme).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
    float db = AudioHelpers.NormalizedVolumeToDB(this._sceneSetupData.playerSpecificSettings.sfxVolume);
    this.Container.Bind<AutomaticSFXVolume.InitData>().FromInstance(new AutomaticSFXVolume.InitData(db, specificSettings.adaptiveSfx)).AsSingle();
    this.Container.Bind<EnvironmentKeywords>().FromInstance(new EnvironmentKeywords((IReadOnlyList<string>) null)).AsSingle();
    this.Container.Bind<BeatmapCallbacksController.InitData>().FromInstance(new BeatmapCallbacksController.InitData((IReadonlyBeatmapData) beatmapData, 0.0f, false)).AsSingle();
    this.Container.Bind<PatternFightSongController.InitData>().FromInstance(new PatternFightSongController.InitData(beatmapData)).AsSingle();
    BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType;
    float noteJumpValue;
    BeatmapObjectSpawnControllerHelpers.GetNoteJumpValues(this._sceneSetupData.playerSpecificSettings, 0.0f, out noteJumpValueType, out noteJumpValue);
    this.Container.Bind<BeatmapObjectSpawnController.InitData>().FromInstance(new BeatmapObjectSpawnController.InitData(this._testAudioClipBPM, 4, 10f, noteJumpValueType, noteJumpValue)).AsSingle();
    this.Container.Bind(typeof (BeatmapObjectManager), typeof (IBeatmapObjectSpawner)).To<BasicBeatmapObjectManager>().AsSingle();
    this.Container.Bind<BasicBeatmapObjectManager.InitData>().FromInstance(new BasicBeatmapObjectManager.InitData(false, false, 60f, 1f)).AsSingle();
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
    this.Container.Bind<AudioTimeSyncController.InitData>().FromInstance(new AudioTimeSyncController.InitData(this._testAudioClip, 0.0f, 0.0f, 1f)).AsSingle();
    this.Container.Bind<MainCameraCullingMask.InitData>().FromInstance(new MainCameraCullingMask.InitData(!specificSettings.reduceDebris)).AsSingle();
    this.Container.Bind<NoteCutSoundEffectManager.InitData>().FromInstance(new NoteCutSoundEffectManager.InitData(false, specificSettings.noFailEffects)).AsSingle();
    this.Container.Bind<SaberModelController.InitData>().FromInstance(new SaberModelController.InitData(new Color(1f, 1f, 1f, this._sceneSetupData.playerSpecificSettings.saberTrailIntensity))).AsSingle();
    this.Container.Bind<BeatEffectSpawner.InitData>().FromInstance(new BeatEffectSpawner.InitData(specificSettings.hideNoteSpawnEffect));
  }
}
