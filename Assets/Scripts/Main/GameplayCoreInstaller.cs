// Decompiled with JetBrains decompiler
// Type: GameplayCoreInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Tweening;
using UnityEngine;
using Zenject;

public class GameplayCoreInstaller : MonoInstaller
{
  [SerializeField]
  protected BeatLineManager _beatLineManagerPrefab;
  [SerializeField]
  protected SongTimeTweeningManager _songTimeTweeningManager;
  [Space]
  [SerializeField]
  protected AudioManagerSO _audioManager;
  [SerializeField]
  protected PlayerHeightDetector _playerHeightDetectorPrefab;
  [SerializeField]
  protected NoteCutScoreSpawner _noteCutScoreSpawnerPrefab;
  [SerializeField]
  protected BadNoteCutEffectSpawner _badNoteCutEffectSpawnerPrefab;
  [SerializeField]
  protected MissedNoteEffectSpawner _missedNoteEffectSpawnerPrefab;
  [Space]
  [SerializeField]
  protected EffectPoolsManualInstaller _effectPoolsManualInstaller;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;
  [Inject]
  protected readonly PerceivedLoudnessPerLevelModel _perceivedLoudnessPerLevelModel;
  [Inject]
  protected readonly RelativeSfxVolumePerLevelModel _relativeSfxVolumePerLevelModel;

  public override void InstallBindings()
  {
    IDifficultyBeatmap difficultyBeatmap = this._sceneSetupData.difficultyBeatmap;
    IBeatmapLevel level = difficultyBeatmap.level;
    PracticeSettings practiceSettings = this._sceneSetupData.practiceSettings;
    PlayerSpecificSettings specificSettings = this._sceneSetupData.playerSpecificSettings;
    GameplayModifiers gameplayModifiers = this._sceneSetupData.gameplayModifiers;
    float songSpeedMul = gameplayModifiers.songSpeedMul;
    float startSongTime = 0.0f;
    float startFilterTime = 0.0f;
    int num1 = this._sceneSetupData.transformedBeatmapData.spawnRotationEventsCount > 0 ? 1 : 0;
    if (practiceSettings != null)
    {
      startFilterTime = practiceSettings.startSongTime;
      startSongTime = !practiceSettings.startInAdvanceAndClearNotes ? practiceSettings.startSongTime : Mathf.Max(0.0f, practiceSettings.startSongTime - 1f);
      songSpeedMul = practiceSettings.songSpeedMul;
    }
    this.Container.BindMemoryPool<GoodCutScoringElement, GoodCutScoringElement.Pool>().WithInitialSize(30);
    this.Container.BindMemoryPool<BadCutScoringElement, BadCutScoringElement.Pool>().WithInitialSize(30);
    this.Container.BindMemoryPool<MissScoringElement, MissScoringElement.Pool>().WithInitialSize(30);
    this.Container.Bind<ColorScheme>().FromInstance(this._sceneSetupData.colorScheme).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
    this.Container.Bind<SongSpeedData>().FromInstance(new SongSpeedData(songSpeedMul));
    float noteJumpMovementSpeed = difficultyBeatmap.noteJumpMovementSpeed;
    if ((double) noteJumpMovementSpeed <= 0.0)
      noteJumpMovementSpeed = difficultyBeatmap.difficulty.NoteJumpMovementSpeed();
    if (gameplayModifiers.fastNotes)
      noteJumpMovementSpeed = 20f;
    this.Container.Bind<AudioTimeSyncController.InitData>().FromInstance(new AudioTimeSyncController.InitData(level.beatmapLevelData.audioClip, startSongTime, level.songTimeOffset, songSpeedMul)).AsSingle();
    this.Container.Bind<CoreGameHUDController.InitData>().FromInstance(new CoreGameHUDController.InitData(specificSettings.noTextsAndHuds || gameplayModifiers.zenMode, true, specificSettings.advancedHud)).AsSingle().IfNotBound();
    this._effectPoolsManualInstaller.ManualInstallBindings(this.Container, this._sceneSetupData.environmentInfo.environmentSizeData.ceilingType == EnvironmentSizeData.CeilingType.LowCeiling);
    this._audioManager.musicPitch = 1f / songSpeedMul;
    this._audioManager.musicVolume = this._perceivedLoudnessPerLevelModel.GetLoudnessCorrectionByLevelId(level.levelID);
    double volumeOffset = (double) AudioHelpers.NormalizedVolumeToDB(this._sceneSetupData.playerSpecificSettings.sfxVolume) + (double) this._relativeSfxVolumePerLevelModel.GetRelativeSfxVolume(this._sceneSetupData.previewBeatmapLevel.levelID);
    float sfxVolumeByLevelId = this._perceivedLoudnessPerLevelModel.GetMaxSfxVolumeByLevelId(this._sceneSetupData.previewBeatmapLevel.levelID);
    int num2 = specificSettings.adaptiveSfx ? 1 : 0;
    double maxVolume = (double) sfxVolumeByLevelId;
    AutomaticSFXVolume.InitData instance = new AutomaticSFXVolume.InitData((float) volumeOffset, num2 != 0, (float) maxVolume);
    this.Container.Bind<AutomaticSFXVolume.InitData>().FromInstance(instance).AsSingle();
    IReadonlyBeatmapData transformedBeatmapData = this._sceneSetupData.transformedBeatmapData;
    this.Container.Bind<IReadonlyBeatmapData>().FromInstance(transformedBeatmapData).AsSingle();
    this.Container.Bind<IDifficultyBeatmap>().FromInstance(difficultyBeatmap).AsSingle();
    this.Container.BindInterfacesAndSelfTo<BeatmapCallbacksController>().AsSingle();
    this.Container.Bind<BeatmapCallbacksController.InitData>().FromInstance(new BeatmapCallbacksController.InitData(transformedBeatmapData, startFilterTime, false)).AsSingle();
    this.Container.Bind<BpmController.InitData>().FromInstance(new BpmController.InitData(level.beatsPerMinute)).AsSingle();
    this.Container.BindInterfacesAndSelfTo<BpmController>().AsSingle();
    BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType;
    float noteJumpValue;
    BeatmapObjectSpawnControllerHelpers.GetNoteJumpValues(this._sceneSetupData.playerSpecificSettings, difficultyBeatmap.noteJumpStartBeatOffset, out noteJumpValueType, out noteJumpValue);
    this.Container.Bind<BeatmapObjectSpawnController.InitData>().FromInstance(new BeatmapObjectSpawnController.InitData(level.beatsPerMinute, transformedBeatmapData.numberOfLines, noteJumpMovementSpeed, noteJumpValueType, noteJumpValue)).AsSingle();
    this.Container.Bind<GameplayModifiers>().FromInstance(this._sceneSetupData.gameplayModifiers).AsSingle();
    bool oneSaberMode = difficultyBeatmap.parentDifficultyBeatmapSet != null && difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.numberOfColors == 1;
    this.Container.Bind<SaberManager.InitData>().FromInstance(new SaberManager.InitData(oneSaberMode, SaberTypeExtensions.MainSaber(this._sceneSetupData.playerSpecificSettings.leftHanded))).AsSingle();
    this.Container.Bind<MainCameraCullingMask.InitData>().FromInstance(new MainCameraCullingMask.InitData(!specificSettings.reduceDebris)).AsSingle();
    FlyingScoreSpawner.SpawnPosition spawnPosition = this._sceneSetupData.environmentInfo.environmentSizeData.floorType == EnvironmentSizeData.FloorType.CloseTo0 ? FlyingScoreSpawner.SpawnPosition.AboveGround : FlyingScoreSpawner.SpawnPosition.Underground;
    this.Container.Bind<FlyingScoreSpawner.InitData>().FromInstance(new FlyingScoreSpawner.InitData(spawnPosition));
    this.Container.Bind<NoteCutSoundEffectManager.InitData>().FromInstance(new NoteCutSoundEffectManager.InitData(this._sceneSetupData.useTestNoteCutSoundEffects, specificSettings.noFailEffects)).AsSingle();
    if (num1 != 0)
      this.Container.Bind<BeatLineManager>().FromComponentInNewPrefab((Object) this._beatLineManagerPrefab).AsSingle().NonLazy();
    this.Container.Bind<SaberModelController.InitData>().FromInstance(new SaberModelController.InitData(new Color(1f, 1f, 1f, this._sceneSetupData.playerSpecificSettings.saberTrailIntensity))).AsSingle();
    this.Container.Bind<BeatEffectSpawner.InitData>().FromInstance(new BeatEffectSpawner.InitData(specificSettings.hideNoteSpawnEffect));
    this.Container.Bind<SliderIntensityEffect.InitData>().FromInstance(new SliderIntensityEffect.InitData(specificSettings.arcsVisible, specificSettings.arcsHapticFeedback));
    if (specificSettings.automaticPlayerHeight)
    {
      this.Container.Bind<PlayerHeightDetector.InitData>().FromInstance(new PlayerHeightDetector.InitData(0.1f, specificSettings.playerHeight)).AsSingle();
      this.Container.Bind<PlayerHeightDetector>().FromComponentInNewPrefab((Object) this._playerHeightDetectorPrefab).AsSingle().NonLazy();
      this.Container.BindInterfacesAndSelfTo<PlayerHeightToJumpOffsetYProvider>().AsSingle();
    }
    else
    {
      this.Container.Bind<StaticJumpOffsetYProvider.InitData>().FromInstance(new StaticJumpOffsetYProvider.InitData(PlayerHeightToJumpOffsetYProvider.JumpOffsetYForPlayerHeight(specificSettings.playerHeight))).AsSingle();
      this.Container.BindInterfacesAndSelfTo<StaticJumpOffsetYProvider>().AsSingle();
    }
    this.Container.Bind(typeof (BasicBeatmapObjectManager), typeof (BeatmapObjectManager), typeof (IBeatmapObjectSpawner)).To<BasicBeatmapObjectManager>().AsSingle();
    this.Container.Bind<BasicBeatmapObjectManager.InitData>().FromInstance(new BasicBeatmapObjectManager.InitData(gameplayModifiers.disappearingArrows, gameplayModifiers.ghostNotes, gameplayModifiers.cutAngleTolerance, gameplayModifiers.notesUniformScale)).AsSingle();
    if (!specificSettings.noTextsAndHuds && !gameplayModifiers.zenMode)
      this.Container.Bind<NoteCutScoreSpawner>().FromComponentInNewPrefab((Object) this._noteCutScoreSpawnerPrefab).AsSingle().NonLazy();
    if (!specificSettings.noFailEffects)
    {
      this.Container.Bind<BadNoteCutEffectSpawner>().FromComponentInNewPrefab((Object) this._badNoteCutEffectSpawnerPrefab).AsSingle().NonLazy();
      this.Container.Bind<MissedNoteEffectSpawner>().FromComponentInNewPrefab((Object) this._missedNoteEffectSpawnerPrefab).AsSingle().NonLazy();
    }
    this.Container.Bind<GameEnergyCounter.InitData>().FromInstance(new GameEnergyCounter.InitData(gameplayModifiers.energyType, false, gameplayModifiers.instaFail, gameplayModifiers.failOnSaberClash)).AsSingle();
    this.Container.Bind<SaberClashChecker>().AsSingle();
    this.Container.Bind<SongTimeTweeningManager>().FromComponentInNewPrefab((Object) this._songTimeTweeningManager).AsSingle();
  }
}
