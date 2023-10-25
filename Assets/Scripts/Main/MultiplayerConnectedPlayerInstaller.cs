// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerInstaller : MonoInstaller
{
  [SerializeField]
  protected MultiplayerConnectedPlayerSongTimeSyncController _connectedPlayerAudioTimeSyncControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerBeatmapObjectEventManager _connectedPlayerBeatmapObjectEventManagerPrefab;
  [Space]
  [SerializeField]
  protected MultiplayerConnectedPlayerGameNoteController _multiplayerGameNoteControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerGameNoteController _multiplayerBurstSliderHeadGameNoteControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerGameNoteController _multiplayerBurstSliderGameNoteControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerGameNoteController _multiplayerBurstSliderFillControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerBombNoteController _multiplayerBombNoteControllerPrefab;
  [SerializeField]
  protected MultiplayerConnectedPlayerObstacleController _multiplayerObstacleControllerPrefab;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [Inject]
  protected readonly MultiplayerPlayerStartState _localPlayerStartState;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;
  [Inject]
  protected readonly PlayersSpecificSettingsAtGameStartModel _playersSpecificSettingsAtGameStartModel;

  public override void InstallBindings()
  {
    PlayerSpecificSettingsNetSerializable settingsForUserId = this._playersSpecificSettingsAtGameStartModel.GetPlayerSpecificSettingsForUserId(this._connectedPlayer.userId);
    IDifficultyBeatmap difficultyBeatmap = this._sceneSetupData.difficultyBeatmap ?? (IDifficultyBeatmap) new EmptyDifficultyBeatmap();
    IBeatmapLevel level = difficultyBeatmap.level;
    PracticeSettings practiceSettings = this._sceneSetupData.practiceSettings;
    GameplayModifiers gameplayModifiers = this._sceneSetupData.gameplayModifiers;
    float songSpeedMul = gameplayModifiers.songSpeedMul;
    float startSongTime = 0.0f;
    if (practiceSettings != null)
    {
      startSongTime = practiceSettings.startInAdvanceAndClearNotes ? Mathf.Max(0.0f, practiceSettings.startSongTime - 1f) : practiceSettings.startSongTime;
      songSpeedMul = practiceSettings.songSpeedMul;
    }
    this.Container.Bind<ColorScheme>().FromInstance(ColorSchemeConverter.FromNetSerializable(settingsForUserId.colorScheme)).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
    this.Container.Bind<MultiplayerPlayerStartState>().FromInstance(this._localPlayerStartState).AsSingle();
    this.Container.BindInstance<IConnectedPlayer>(this._connectedPlayer);
    this.Container.Bind<IConnectedPlayerBeatmapObjectEventManager>().FromComponentInNewPrefab((UnityEngine.Object) this._connectedPlayerBeatmapObjectEventManagerPrefab).AsSingle();
    this.Container.Bind(typeof (BeatmapObjectManager), typeof (IBeatmapObjectSpawner), typeof (IDisposable)).To<MultiplayerConnectedPlayerBeatmapObjectManager>().AsSingle();
    this.Container.Bind<MultiplayerConnectedPlayerBeatmapObjectManager.InitData>().FromInstance(new MultiplayerConnectedPlayerBeatmapObjectManager.InitData(gameplayModifiers.disappearingArrows, gameplayModifiers.ghostNotes, gameplayModifiers.notesUniformScale)).AsSingle();
    this.Container.Bind(typeof (IAudioTimeSource), typeof (MultiplayerConnectedPlayerSongTimeSyncController)).FromComponentInNewPrefab((UnityEngine.Object) this._connectedPlayerAudioTimeSyncControllerPrefab).AsSingle();
    this.Container.Bind<MultiplayerConnectedPlayerSongTimeSyncController.InitData>().FromInstance(new MultiplayerConnectedPlayerSongTimeSyncController.InitData(startSongTime, level.songTimeOffset, songSpeedMul)).AsSingle();
    bool oneSaberMode = difficultyBeatmap.parentDifficultyBeatmapSet != null && difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.numberOfColors == 1;
    this.Container.Bind<SaberManager.InitData>().FromInstance(new SaberManager.InitData(oneSaberMode, SaberTypeExtensions.MainSaber(settingsForUserId.leftHanded))).AsSingle();
    this.Container.Bind<SaberModelController.InitData>().FromInstance(new SaberModelController.InitData(new Color(1f, 1f, 1f, this._sceneSetupData.playerSpecificSettings.saberTrailIntensity))).AsSingle();
    this.Container.BindMemoryPool<MultiplayerConnectedPlayerGameNoteController, MultiplayerConnectedPlayerGameNoteController.Pool>().WithId((object) NoteData.GameplayType.Normal).WithInitialSize(50).FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerGameNoteControllerPrefab);
    this.Container.BindMemoryPool<MultiplayerConnectedPlayerGameNoteController, MultiplayerConnectedPlayerGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderHead).WithInitialSize(10).FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerBurstSliderHeadGameNoteControllerPrefab);
    this.Container.BindMemoryPool<MultiplayerConnectedPlayerGameNoteController, MultiplayerConnectedPlayerGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElement).WithInitialSize(40).FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerBurstSliderGameNoteControllerPrefab);
    this.Container.BindMemoryPool<MultiplayerConnectedPlayerGameNoteController, MultiplayerConnectedPlayerGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElementFill).WithInitialSize(20).FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerBurstSliderFillControllerPrefab);
    this.Container.BindMemoryPool<MultiplayerConnectedPlayerBombNoteController, MultiplayerConnectedPlayerBombNoteController.Pool>().WithInitialSize(6).FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerBombNoteControllerPrefab);
    this.Container.BindMemoryPool<MultiplayerConnectedPlayerObstacleController, MultiplayerConnectedPlayerObstacleController.Pool>().WithInitialSize(4).FromComponentInNewPrefab((UnityEngine.Object) this._multiplayerObstacleControllerPrefab);
  }
}
