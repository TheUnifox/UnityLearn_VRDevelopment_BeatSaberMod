// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorStandardLevelScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorStandardLevelScenesTransitionSetupDataSO : 
    LevelScenesTransitionSetupDataSO
  {
    [SerializeField]
    protected SceneInfo _standardGameplaySceneInfo;
    [SerializeField]
    protected SceneInfo _beatmapEditorGameplaySceneInfo;
    [SerializeField]
    protected SceneInfo _gameCoreSceneInfo;
    [SerializeField]
    protected MainSettingsModelSO _mainSettingsModel;

    public event System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> didFinishEvent;

    public virtual void Init(
      IDifficultyBeatmap difficultyBeatmap,
      IPreviewBeatmapLevel previewBeatmapLevel,
      GameplayModifiers gameplayModifiers,
      PlayerSpecificSettings playerSpecificSettings,
      PracticeSettings practiceSettings,
      bool useFirstPersonFlyingController,
      System.Action beforeSceneSwitchCallback,
      System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFinishedCallback)
    {
      EnvironmentInfoSO environmentInfo = difficultyBeatmap.GetEnvironmentInfo();
      ColorScheme colorScheme = new ColorScheme(environmentInfo.colorScheme);
      IBeatmapLevel level = difficultyBeatmap.level;
      this.gameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, practiceSettings, false, environmentInfo, colorScheme, this._mainSettingsModel);
      SceneSetupData[] sceneSetupData = new SceneSetupData[5]
      {
        (SceneSetupData) new EnvironmentSceneSetupData(environmentInfo, previewBeatmapLevel, false),
        (SceneSetupData) new StandardGameplaySceneSetupData(playerSpecificSettings.autoRestart, (IPreviewBeatmapLevel) level, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, "Back to editor", gameplayModifiers, true),
        (SceneSetupData) new BeatmapEditorGameplaySceneSetupData(useFirstPersonFlyingController, false, false),
        (SceneSetupData) this.gameplayCoreSceneSetupData,
        (SceneSetupData) new GameCoreSceneSetupData()
      };
      this.Init(new SceneInfo[4]
      {
        environmentInfo.sceneInfo,
        this._standardGameplaySceneInfo,
        this._beatmapEditorGameplaySceneInfo,
        this._gameCoreSceneInfo
      }, sceneSetupData);
    }

    public virtual void Finish(LevelCompletionResults levelCompletionResults)
    {
      System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(this, levelCompletionResults);
    }
  }
}
