// Decompiled with JetBrains decompiler
// Type: MissionLevelScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class MissionLevelScenesTransitionSetupDataSO : LevelScenesTransitionSetupDataSO
{
  [SerializeField]
  protected SceneInfo _missionGameplaySceneInfo;
  [SerializeField]
  protected SceneInfo _gameCoreSceneInfo;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [CompilerGenerated]
  protected string m_CmissionId;
  [CompilerGenerated]
  protected IDifficultyBeatmap m_CdifficultyBeatmap;

  public event System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> didFinishEvent;

  public string missionId
  {
    get => this.m_CmissionId;
    private set => this.m_CmissionId = value;
  }

  public IDifficultyBeatmap difficultyBeatmap
  {
    get => this.m_CdifficultyBeatmap;
    private set => this.m_CdifficultyBeatmap = value;
  }

  public virtual void Init(
    string missionId,
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    MissionObjective[] missionObjectives,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    string backButtonText)
  {
    this.missionId = missionId;
    this.difficultyBeatmap = difficultyBeatmap;
    EnvironmentInfoSO environmentInfo = difficultyBeatmap.GetEnvironmentInfo();
    ColorScheme colorScheme = overrideColorScheme ?? new ColorScheme(environmentInfo.colorScheme);
    IBeatmapLevel level = difficultyBeatmap.level;
    this.gameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, (PracticeSettings) null, false, environmentInfo, colorScheme, this._mainSettingsModel);
    this.Init(new SceneInfo[3]
    {
      environmentInfo.sceneInfo,
      this._missionGameplaySceneInfo,
      this._gameCoreSceneInfo
    }, new SceneSetupData[4]
    {
      (SceneSetupData) new EnvironmentSceneSetupData(environmentInfo, previewBeatmapLevel, false),
      (SceneSetupData) new MissionGameplaySceneSetupData(missionObjectives, playerSpecificSettings.autoRestart, (IPreviewBeatmapLevel) level, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, gameplayModifiers, backButtonText),
      (SceneSetupData) this.gameplayCoreSceneSetupData,
      (SceneSetupData) new GameCoreSceneSetupData()
    });
  }

  public virtual void Finish(MissionCompletionResults levelCompletionResults)
  {
    System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, levelCompletionResults);
  }
}
