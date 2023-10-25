// Decompiled with JetBrains decompiler
// Type: StandardLevelNoTransitionInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class StandardLevelNoTransitionInstaller : NoTransitionInstaller
{
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _scenesTransitionSetupData;
  [SerializeField]
  protected StandardLevelNoTransitionInstallerData _standardLevelNoTransitionInstallerData;

  public StandardLevelNoTransitionInstallerData standardLevelNoTransitionInstallerData => this._standardLevelNoTransitionInstallerData;

  public override void InstallBindings(DiContainer container)
  {
    IDifficultyBeatmap difficultyBeatmap = this._standardLevelNoTransitionInstallerData.beatmapLevel.GetDifficultyBeatmap(this._standardLevelNoTransitionInstallerData.beatmapCharacteristic, this._standardLevelNoTransitionInstallerData.beatmapDifficulty);
    OverrideEnvironmentSettings overrideEnvironmentSettings = new OverrideEnvironmentSettings();
    overrideEnvironmentSettings.SetEnvironmentInfoForType(this._standardLevelNoTransitionInstallerData.environmentInfo.environmentType, this._standardLevelNoTransitionInstallerData.environmentInfo);
    overrideEnvironmentSettings.overrideEnvironments = true;
    this._scenesTransitionSetupData.Init("Editor", difficultyBeatmap, (IPreviewBeatmapLevel) this._standardLevelNoTransitionInstallerData.beatmapLevel, overrideEnvironmentSettings, this._standardLevelNoTransitionInstallerData.colorScheme.colorScheme, this._standardLevelNoTransitionInstallerData.gameplayModifiers, this._standardLevelNoTransitionInstallerData.playerSpecificSettings, this._standardLevelNoTransitionInstallerData.practiceSettings, this._standardLevelNoTransitionInstallerData.backButtonText, this._standardLevelNoTransitionInstallerData.useTestNoteCutSoundEffects);
    this._scenesTransitionSetupData.BeforeScenesWillBeActivated(false);
    this._scenesTransitionSetupData.InstallBindings(container);
  }
}
