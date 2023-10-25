// Decompiled with JetBrains decompiler
// Type: SimpleLevelStarter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SimpleLevelStarter : MonoBehaviour
{
  [SerializeField]
  protected BeatmapLevelSO _level;
  [SerializeField]
  protected BeatmapCharacteristicSO _beatmapCharacteristic;
  [SerializeField]
  protected BeatmapDifficulty _beatmapDifficulty;
  [SerializeField]
  protected bool _useTestNoteCutSoundEffects;
  [SerializeField]
  protected bool _overrideStrobeFilterSettingsToAllEffects;
  [SerializeField]
  [NullAllowed]
  protected TextAsset _recordingTextAsset;
  [SerializeField]
  [NullAllowed]
  protected Component[] _prefabBindings;
  [Space]
  [SerializeField]
  protected Button _button;
  [Inject]
  protected readonly MenuTransitionsHelper _menuTransitionsHelper;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  protected ButtonBinder _buttonBinder;
  protected readonly GameplayModifiers _gameplayModifiers = GameplayModifiers.noModifiers.CopyWith(noFailOn0Energy: new bool?(true));

  public virtual void Awake()
  {
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._button, new System.Action(this.ButtonPressed));
  }

  public virtual void OnDestroy() => this._buttonBinder.ClearBindings();

  public virtual void StartLevel()
  {
    IDifficultyBeatmap difficultyBeatmap = this._level.beatmapLevelData.GetDifficultyBeatmap(this._beatmapCharacteristic, this._beatmapDifficulty);
    this._gameScenesManager.installEarlyBindingsEvent += new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.InstallEarlyBindings);
    PlayerSpecificSettings playerSpecificSettings = this._playerDataModel.playerData.playerSpecificSettings;
    if (this._overrideStrobeFilterSettingsToAllEffects)
      playerSpecificSettings = playerSpecificSettings.CopyWith(environmentEffectsFilterDefaultPreset: new EnvironmentEffectsFilterPreset?(EnvironmentEffectsFilterPreset.AllEffects), environmentEffectsFilterExpertPlusPreset: new EnvironmentEffectsFilterPreset?(EnvironmentEffectsFilterPreset.AllEffects));
    this._menuTransitionsHelper.StartStandardLevel("Simple", difficultyBeatmap, (IPreviewBeatmapLevel) this._level, this._playerDataModel.playerData.overrideEnvironmentSettings, (ColorScheme) null, this._gameplayModifiers, playerSpecificSettings, (PracticeSettings) null, Localization.Get("BUTTON_MENU"), this._useTestNoteCutSoundEffects, false, (System.Action) null, (System.Action<DiContainer>) (container =>
    {
      if ((UnityEngine.Object) this._recordingTextAsset != (UnityEngine.Object) null)
      {
        VRControllersRecorder controllersRecorder = container.Resolve<VRControllersRecorder>();
        controllersRecorder.SetInGamePlaybackDefaultSettings();
        controllersRecorder.recordingTextAsset = this._recordingTextAsset;
        controllersRecorder.recordingFileName = "";
        controllersRecorder.enabled = true;
      }
      this._gameScenesManager.installEarlyBindingsEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.InstallEarlyBindings);
    }), new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleLevelDidFinish), (System.Action<LevelScenesTransitionSetupDataSO, LevelCompletionResults>) null);
  }

  public virtual void InstallEarlyBindings(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    DiContainer container)
  {
    foreach (Component prefabBinding in this._prefabBindings)
      container.Bind(((object) prefabBinding).GetType()).FromComponentInNewPrefab((UnityEngine.Object) prefabBinding).AsSingle().NonLazy();
  }

  public virtual void ButtonPressed() => this.StartLevel();

  public virtual void HandleLevelDidFinish(
    StandardLevelScenesTransitionSetupDataSO standardLevelSceneSetupData,
    LevelCompletionResults levelCompletionResults)
  {
    if (levelCompletionResults.levelEndAction != LevelCompletionResults.LevelEndAction.Restart)
      return;
    this.StartLevel();
  }

  [CompilerGenerated]
  public virtual void m_CStartLevelm_Eg__AfterSceneSwitchCallback(
    DiContainer container)
  {
    if ((UnityEngine.Object) this._recordingTextAsset != (UnityEngine.Object) null)
    {
      VRControllersRecorder controllersRecorder = container.Resolve<VRControllersRecorder>();
      controllersRecorder.SetInGamePlaybackDefaultSettings();
      controllersRecorder.recordingTextAsset = this._recordingTextAsset;
      controllersRecorder.recordingFileName = "";
      controllersRecorder.enabled = true;
    }
    this._gameScenesManager.installEarlyBindingsEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.InstallEarlyBindings);
  }
}
