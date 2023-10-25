// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapLevelEditorSceneSetup
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using System.Collections.Generic;
using Tweening;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapLevelEditorSceneSetup : MonoInstaller
  {
    [SerializeField]
    private SongTimeTweeningManager _songTimeTweeningManagerPrefab;
    [Space]
    [SerializeField]
    private AudioTimeSyncController _audioTimeSyncController;
    [SerializeField]
    private SongTimeFixedUpdateController _songTimeFixedUpdateController;
    [Inject]
    private readonly EditorGameplayCoreSceneSetupData _editorGameplayCoreSceneSetupData;

    public override void InstallBindings()
    {
      this.Container.Bind<EnvironmentContext>().FromInstance(EnvironmentContext.BeatmapEditor).AsSingle();
      this.Container.Bind<ColorScheme>().FromInstance(this._editorGameplayCoreSceneSetupData.colorScheme.colorScheme).AsSingle();
      this.Container.Bind<ColorManager>().AsSingle();
      this.Container.Bind<PauseMenuManager.InitData>().FromInstance(new PauseMenuManager.InitData("", (IPreviewBeatmapLevel) null, BeatmapDifficulty.Easy, (BeatmapCharacteristicSO) null, false, false)).AsSingle();
      this.Container.Bind<BeatmapCallbacksController.InitData>().FromInstance(new BeatmapCallbacksController.InitData((IReadonlyBeatmapData) this._editorGameplayCoreSceneSetupData.beatmapData, 0.0f, true)).AsSingle();
      this.Container.Bind<IReadonlyBeatmapData>().FromInstance((IReadonlyBeatmapData) this._editorGameplayCoreSceneSetupData.beatmapData).AsSingle();
      this.Container.Bind<EnvironmentKeywords>().FromInstance(new EnvironmentKeywords((IReadOnlyList<string>) null)).AsSingle();
      this.Container.Bind<AudioTimeSyncController.InitData>().FromInstance(new AudioTimeSyncController.InitData((AudioClip) null, 0.0f, 0.0f, 1f)).AsSingle();
      this.Container.Bind<SongSpeedData>().FromInstance(new SongSpeedData(1f));
      this.Container.Bind(typeof (IBeatmapObjectSpawnController)).To<BeatmapEditorBeatmapObjectSpawnController>().FromNewComponentOnRoot().AsSingle();
      this.Container.Bind<IGamePause>().To<MockPause>().AsSingle();
      this.Container.Bind<ILevelEndActions>().To<MockLevelEndActions>().AsSingle();
      this.Container.Bind<SaberModelController.InitData>().FromInstance(new SaberModelController.InitData(new Color(1f, 1f, 1f, 1f))).AsSingle();
      BeatmapEditor3D.Types.GameplayUIState gameplayUiState = this._editorGameplayCoreSceneSetupData.beatmapEditorSettingsDataModel.gameplayUIState;
      this.Container.Bind<CoreGameHUDController.InitData>().FromInstance(new CoreGameHUDController.InitData(gameplayUiState == BeatmapEditor3D.Types.GameplayUIState.Hidden, gameplayUiState != 0, gameplayUiState == BeatmapEditor3D.Types.GameplayUIState.Advanced)).AsSingle();
      this.Container.Bind<SaberManager.InitData>().FromInstance(new SaberManager.InitData(false)).AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorGameEnergyCounter>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorComboController>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorScoreController>().AsSingle();
      this.Container.Bind<RelativeScoreAndImmediateRankCounter>().FromMock<RelativeScoreAndImmediateRankCounter>();
      this.Container.Bind<SaberClashChecker>().AsSingle();
      this.Container.Bind<StaticJumpOffsetYProvider.InitData>().FromInstance(new StaticJumpOffsetYProvider.InitData(0.0f)).AsSingle();
      this.Container.BindInterfacesAndSelfTo<StaticJumpOffsetYProvider>().AsSingle();
      this.Container.Bind<GameplayModifiers>().FromInstance(new GameplayModifiers()).AsSingle();
      this.Container.Bind<GameEnergyCounter.InitData>().FromInstance(new GameEnergyCounter.InitData(GameplayModifiers.EnergyType.Bar, false, false, false));
      this.Container.Bind<BeatmapCallbacksController>().AsSingle();
      this.Container.Bind(typeof (BeatmapObjectManager), typeof (BeatmapEditorBeatmapObjectManager)).To<BeatmapEditorBeatmapObjectManager>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorBpmController>().FromInstance((object) new BeatmapEditorBpmController(this._editorGameplayCoreSceneSetupData.beatmapState, this._editorGameplayCoreSceneSetupData.beatmapDataModel)).AsSingle();
      this.Container.Rebind<BasicSpectrogramData>().FromInstance(this._editorGameplayCoreSceneSetupData.basicSpectrogramData).AsSingle();
      this.Container.Bind<AudioTimeSyncController>().FromInstance(this._audioTimeSyncController).AsSingle();
      this.Container.Bind<SongTimeFixedUpdateController>().FromInstance(this._songTimeFixedUpdateController).AsSingle();
      BeatmapEditorAudioTimeSyncController instance = new BeatmapEditorAudioTimeSyncController(this._editorGameplayCoreSceneSetupData.beatmapDataModel, this._editorGameplayCoreSceneSetupData.beatmapState);
      this.Container.BindInterfacesAndSelfTo<BeatmapEditorAudioTimeSyncController>().FromInstance((object) instance).AsSingle();
      this.Container.Bind<SongTimeTweeningManager>().FromComponentInNewPrefab((Object) this._songTimeTweeningManagerPrefab).AsSingle();
    }
  }
}
