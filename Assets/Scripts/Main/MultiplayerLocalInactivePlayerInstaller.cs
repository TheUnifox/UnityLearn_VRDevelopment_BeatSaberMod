// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalInactivePlayerInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalInactivePlayerInstaller : MonoInstaller
{
  [SerializeField]
  protected AudioManagerSO _audioManager;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;
  [Inject]
  protected readonly PerceivedLoudnessPerLevelModel _beatmapLoudnessModel;
  [Inject]
  protected readonly RelativeSfxVolumePerLevelModel _relativeSfxVolumePerLevelModel;

  public override void InstallBindings()
  {
    IBeatmapLevel level = this._sceneSetupData.difficultyBeatmap?.level;
    PracticeSettings practiceSettings = this._sceneSetupData.practiceSettings;
    PlayerSpecificSettings specificSettings = this._sceneSetupData.playerSpecificSettings;
    float songSpeedMul = this._sceneSetupData.gameplayModifiers.songSpeedMul;
    float startSongTime = 0.0f;
    if (practiceSettings != null)
    {
      startSongTime = !practiceSettings.startInAdvanceAndClearNotes ? practiceSettings.startSongTime : Mathf.Max(0.0f, practiceSettings.startSongTime - 1f);
      songSpeedMul = practiceSettings.songSpeedMul;
    }
    this.Container.Bind<ColorScheme>().FromInstance(this._sceneSetupData.colorScheme).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
    this.Container.Bind<MultiplayerLocalInactivePlayerSongSyncController.InitData>().FromInstance(new MultiplayerLocalInactivePlayerSongSyncController.InitData(level?.beatmapLevelData?.audioClip, startSongTime, level != null ? level.songTimeOffset : 0.0f, songSpeedMul)).AsSingle();
    this._audioManager.musicPitch = 1f / songSpeedMul;
    double volumeOffset = (double) AudioHelpers.NormalizedVolumeToDB(this._sceneSetupData.playerSpecificSettings.sfxVolume) + (double) this._relativeSfxVolumePerLevelModel.GetRelativeSfxVolume(this._sceneSetupData.previewBeatmapLevel.levelID);
    float sfxVolumeByLevelId = this._beatmapLoudnessModel.GetMaxSfxVolumeByLevelId(this._sceneSetupData.previewBeatmapLevel.levelID);
    int num = specificSettings.adaptiveSfx ? 1 : 0;
    double maxVolume = (double) sfxVolumeByLevelId;
    AutomaticSFXVolume.InitData instance = new AutomaticSFXVolume.InitData((float) volumeOffset, num != 0, (float) maxVolume);
    this.Container.Bind<AutomaticSFXVolume.InitData>().FromInstance(instance).AsSingle();
  }
}
