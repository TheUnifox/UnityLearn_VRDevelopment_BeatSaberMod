// Decompiled with JetBrains decompiler
// Type: MissionLevelNoTransitionInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MissionLevelNoTransitionInstaller : NoTransitionInstaller
{
  [SerializeField]
  protected BeatmapLevelSO _beatmapLevel;
  [SerializeField]
  protected BeatmapCharacteristicSO _beatmapCharacteristic;
  [SerializeField]
  protected BeatmapDifficulty _beatmapDifficulty;
  [SerializeField]
  protected ColorSchemeSO _colorScheme;
  [SerializeField]
  protected MissionObjective[] _missionObjectives;
  [SerializeField]
  protected GameplayModifiers _gameplayModifiers;
  [SerializeField]
  protected PlayerSpecificSettings _playerSpecificSettings;
  [SerializeField]
  protected string _backButtonText;
  [Space]
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _scenesTransitionSetupData;

  public override void InstallBindings(DiContainer container)
  {
    IDifficultyBeatmap difficultyBeatmap = this._beatmapLevel.GetDifficultyBeatmap(this._beatmapCharacteristic, this._beatmapDifficulty);
    this._scenesTransitionSetupData.Init(string.Empty, difficultyBeatmap, (IPreviewBeatmapLevel) this._beatmapLevel, this._missionObjectives, this._colorScheme.colorScheme, this._gameplayModifiers, this._playerSpecificSettings, this._backButtonText);
    this._scenesTransitionSetupData.BeforeScenesWillBeActivated(false);
    this._scenesTransitionSetupData.InstallBindings(container);
  }
}
