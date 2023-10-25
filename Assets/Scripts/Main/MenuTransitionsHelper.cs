// Decompiled with JetBrains decompiler
// Type: MenuTransitionsHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatmapEditor3D;
using Polyglot;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class MenuTransitionsHelper : MonoBehaviour
{
  [SerializeField]
  protected AppInitScenesTransitionSetupDataContainerSO _appInitScenesTransitionSetupDataContainer;
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;
  [SerializeField]
  protected MultiplayerLevelScenesTransitionSetupDataSO _multiplayerLevelScenesTransitionSetupData;
  [SerializeField]
  protected MissionLevelScenesTransitionSetupDataSO _missionLevelScenesTransitionSetupData;
  [SerializeField]
  protected TutorialScenesTransitionSetupDataSO _tutorialScenesTransitionSetupData;
  [SerializeField]
  protected CreditsScenesTransitionSetupDataSO _creditsScenesTransitionSetupData;
  [SerializeField]
  protected BeatmapEditorScenesTransitionSetupDataSO _beatmapEditorScenesTransitionSetupData;
  [SerializeField]
  protected BeatmapEditorStandardLevelScenesTransitionSetupDataSO _beatmapEditorStandardLevelScenesTransitionSetupData;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly BeatmapDataCache _beatmapDataCache;
  protected System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> _standardLevelFinishedCallback;
  protected System.Action<LevelScenesTransitionSetupDataSO, LevelCompletionResults> _standardLevelRestartedCallback;
  protected System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> _multiplayerLevelFinishedCallback;
  protected System.Action<DisconnectedReason> _multiplayerDidDisconnectCallback;
  protected System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> _missionLevelFinishedCallback;
  protected System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> _missionLevelRestartedCallback;
  protected System.Action _beatmapEditorFinishedCallback;
  protected System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> _beatmapEditorGameplayLevelFinishedCallback;

  public virtual void StartStandardLevel(
    string gameMode,
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    OverrideEnvironmentSettings overrideEnvironmentSettings,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    string backButtonText,
    bool useTestNoteCutSoundEffects,
    bool startPaused,
    System.Action beforeSceneSwitchCallback,
    System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFinishedCallback,
    System.Action<LevelScenesTransitionSetupDataSO, LevelCompletionResults> levelRestartedCallback)
  {
    this.StartStandardLevel(gameMode, difficultyBeatmap, previewBeatmapLevel, overrideEnvironmentSettings, overrideColorScheme, gameplayModifiers, playerSpecificSettings, practiceSettings, backButtonText, useTestNoteCutSoundEffects, startPaused, beforeSceneSwitchCallback, (System.Action<DiContainer>) null, levelFinishedCallback, levelRestartedCallback);
  }

  public virtual void StartStandardLevel(
    string gameMode,
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    OverrideEnvironmentSettings overrideEnvironmentSettings,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    string backButtonText,
    bool useTestNoteCutSoundEffects,
    bool startPaused,
    System.Action beforeSceneSwitchCallback,
    System.Action<DiContainer> afterSceneSwitchCallback,
    System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFinishedCallback,
    System.Action<LevelScenesTransitionSetupDataSO, LevelCompletionResults> levelRestartedCallback)
  {
    this._standardLevelFinishedCallback = levelFinishedCallback;
    this._standardLevelRestartedCallback = levelRestartedCallback;
    this._standardLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleMainGameSceneDidFinish);
    this._standardLevelScenesTransitionSetupData.didFinishEvent += new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleMainGameSceneDidFinish);
    this._standardLevelScenesTransitionSetupData.Init(gameMode, difficultyBeatmap, previewBeatmapLevel, overrideEnvironmentSettings, overrideColorScheme, gameplayModifiers, playerSpecificSettings, practiceSettings, backButtonText, useTestNoteCutSoundEffects, startPaused, this._beatmapDataCache);
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._standardLevelScenesTransitionSetupData, 0.7f, beforeSceneSwitchCallback, afterSceneSwitchCallback);
  }

  public virtual void StartMissionLevel(
    string missionId,
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    MissionObjective[] missionObjectives,
    PlayerSpecificSettings playerSpecificSettings,
    System.Action beforeSceneSwitchCallback,
    System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> levelFinishedCallback,
    System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> levelRestartedCallback = null)
  {
    this._missionLevelFinishedCallback = levelFinishedCallback;
    this._missionLevelRestartedCallback = levelRestartedCallback;
    this._missionLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelSceneDidFinish);
    this._missionLevelScenesTransitionSetupData.didFinishEvent += new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelSceneDidFinish);
    this._missionLevelScenesTransitionSetupData.Init(missionId, difficultyBeatmap, previewBeatmapLevel, missionObjectives, overrideColorScheme, gameplayModifiers, playerSpecificSettings, Localization.Get("BUTTON_MENU"));
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._missionLevelScenesTransitionSetupData, 0.7f, beforeSceneSwitchCallback);
  }

  public virtual void StartMultiplayerLevel(
    string gameMode,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    IDifficultyBeatmap difficultyBeatmap,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    string backButtonText,
    bool useTestNoteCutSoundEffects,
    System.Action beforeSceneSwitchCallback,
    System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> levelFinishedCallback,
    System.Action<DisconnectedReason> didDisconnectCallback)
  {
    this.StartMultiplayerLevel(gameMode, previewBeatmapLevel, beatmapDifficulty, beatmapCharacteristic, difficultyBeatmap, overrideColorScheme, gameplayModifiers, playerSpecificSettings, practiceSettings, backButtonText, useTestNoteCutSoundEffects, beforeSceneSwitchCallback, (System.Action<DiContainer>) null, levelFinishedCallback, didDisconnectCallback);
  }

  public virtual void StartMultiplayerLevel(
    string gameMode,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    IDifficultyBeatmap difficultyBeatmap,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    string backButtonText,
    bool useTestNoteCutSoundEffects,
    System.Action beforeSceneSwitchCallback,
    System.Action<DiContainer> afterSceneSwitchCallback,
    System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> levelFinishedCallback,
    System.Action<DisconnectedReason> didDisconnectCallback)
  {
    this._multiplayerLevelFinishedCallback = levelFinishedCallback;
    this._multiplayerDidDisconnectCallback = didDisconnectCallback;
    this._multiplayerLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);
    this._multiplayerLevelScenesTransitionSetupData.didFinishEvent += new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);
    this._multiplayerLevelScenesTransitionSetupData.didDisconnectEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, DisconnectedReason>(this.HandleMultiplayerLevelDidDisconnect);
    this._multiplayerLevelScenesTransitionSetupData.didDisconnectEvent += new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, DisconnectedReason>(this.HandleMultiplayerLevelDidDisconnect);
    this._multiplayerLevelScenesTransitionSetupData.Init(gameMode, previewBeatmapLevel, beatmapDifficulty, beatmapCharacteristic, difficultyBeatmap, overrideColorScheme, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects);
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._multiplayerLevelScenesTransitionSetupData, 0.7f, beforeSceneSwitchCallback, afterSceneSwitchCallback);
  }

  public virtual void StartTutorial(
    PlayerSpecificSettings playerSpecificSettings,
    System.Action beforeSceneSwitchCallback = null)
  {
    this._tutorialScenesTransitionSetupData.didFinishEvent -= new System.Action<TutorialScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType>(this.HandleTutorialSceneDidFinish);
    this._tutorialScenesTransitionSetupData.didFinishEvent += new System.Action<TutorialScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType>(this.HandleTutorialSceneDidFinish);
    this._tutorialScenesTransitionSetupData.Init(playerSpecificSettings);
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._tutorialScenesTransitionSetupData, 0.7f, beforeSceneSwitchCallback);
  }

  public virtual void ShowCredits()
  {
    this._creditsScenesTransitionSetupData.didFinishEvent -= new System.Action<CreditsScenesTransitionSetupDataSO>(this.HandleCreditsSceneDidFinish);
    this._creditsScenesTransitionSetupData.didFinishEvent += new System.Action<CreditsScenesTransitionSetupDataSO>(this.HandleCreditsSceneDidFinish);
    this._creditsScenesTransitionSetupData.Init();
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._creditsScenesTransitionSetupData, 0.7f);
  }

  public virtual void StartBeatmapEditor(System.Action beatmapEditorFinishedCallback)
  {
    this._beatmapEditorFinishedCallback = beatmapEditorFinishedCallback;
    this._beatmapEditorScenesTransitionSetupData.didFinishEvent -= new System.Action<BeatmapEditorScenesTransitionSetupDataSO>(this.HandleBeatmapEditorSceneDidFinish);
    this._beatmapEditorScenesTransitionSetupData.didFinishEvent += new System.Action<BeatmapEditorScenesTransitionSetupDataSO>(this.HandleBeatmapEditorSceneDidFinish);
    this._beatmapEditorScenesTransitionSetupData.Init(false);
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._beatmapEditorScenesTransitionSetupData, 0.7f, (System.Action) (() => Localization.Instance.SelectLanguage(Language.English)));
  }

  public virtual void StartBeatmapEditorStandardLevel(
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    bool useFirstPersonFlyingController,
    System.Action beforeSceneSwitchCallback,
    System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFinishedCallback)
  {
    this._beatmapEditorGameplayLevelFinishedCallback = levelFinishedCallback;
    this._beatmapEditorStandardLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleBeatmapEditorGameSceneDidFinish);
    this._beatmapEditorStandardLevelScenesTransitionSetupData.didFinishEvent += new System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleBeatmapEditorGameSceneDidFinish);
    this._beatmapEditorStandardLevelScenesTransitionSetupData.Init(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, practiceSettings, useFirstPersonFlyingController, beforeSceneSwitchCallback, levelFinishedCallback);
    this._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) this._beatmapEditorStandardLevelScenesTransitionSetupData, 0.7f, beforeSceneSwitchCallback);
  }

  public virtual void RestartGame(System.Action<DiContainer> finishCallback = null)
  {
    this._appInitScenesTransitionSetupDataContainer.appInitScenesTransitionSetupData.Init();
    this._gameScenesManager.ClearAndOpenScenes((ScenesTransitionSetupDataSO) this._appInitScenesTransitionSetupDataContainer.appInitScenesTransitionSetupData, 0.35f, finishCallback: finishCallback);
  }

  public virtual void HandleMainGameSceneDidFinish(
    StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData,
    LevelCompletionResults levelCompletionResults)
  {
    if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.Restart)
    {
      System.Action<LevelScenesTransitionSetupDataSO, LevelCompletionResults> restartedCallback = this._standardLevelRestartedCallback;
      if (restartedCallback != null)
        restartedCallback((LevelScenesTransitionSetupDataSO) standardLevelScenesTransitionSetupData, levelCompletionResults);
      this._gameScenesManager.ReplaceScenes((ScenesTransitionSetupDataSO) this._standardLevelScenesTransitionSetupData, minDuration: 0.7f);
    }
    else
    {
      standardLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleMainGameSceneDidFinish);
      this._gameScenesManager.PopScenes(levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed || levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared ? 1.3f : 0.35f, finishCallback: (System.Action<DiContainer>) (container =>
      {
        System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> finishedCallback = this._standardLevelFinishedCallback;
        if (finishedCallback == null)
          return;
        finishedCallback(standardLevelScenesTransitionSetupData, levelCompletionResults);
      }));
    }
  }

  public virtual void HandleMultiplayerLevelDidFinish(
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData,
    MultiplayerResultsData multiplayerResultsData)
  {
    multiplayerLevelScenesTransitionSetupData.didDisconnectEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, DisconnectedReason>(this.HandleMultiplayerLevelDidDisconnect);
    multiplayerLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);
    this._gameScenesManager.PopScenes(multiplayerResultsData.localPlayerResultData.multiplayerLevelCompletionResults.hasAnyResults ? 1.3f : 0.35f, finishCallback: (System.Action<DiContainer>) (container =>
    {
      System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> finishedCallback = this._multiplayerLevelFinishedCallback;
      if (finishedCallback == null)
        return;
      finishedCallback(multiplayerLevelScenesTransitionSetupData, multiplayerResultsData);
    }));
  }

  public virtual void HandleMultiplayerLevelDidDisconnect(
    MultiplayerLevelScenesTransitionSetupDataSO multiplayerLevelScenesTransitionSetupData,
    DisconnectedReason disconnectedReason)
  {
    multiplayerLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData>(this.HandleMultiplayerLevelDidFinish);
    multiplayerLevelScenesTransitionSetupData.didDisconnectEvent -= new System.Action<MultiplayerLevelScenesTransitionSetupDataSO, DisconnectedReason>(this.HandleMultiplayerLevelDidDisconnect);
    this._gameScenesManager.PopScenes(0.35f, finishCallback: (System.Action<DiContainer>) (container =>
    {
      System.Action<DisconnectedReason> disconnectCallback = this._multiplayerDidDisconnectCallback;
      if (disconnectCallback == null)
        return;
      disconnectCallback(disconnectedReason);
    }));
  }

  public virtual void HandleMissionLevelSceneDidFinish(
    MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupData,
    MissionCompletionResults missionCompletionResults)
  {
    if (missionCompletionResults.levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.Restart)
    {
      System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> restartedCallback = this._missionLevelRestartedCallback;
      if (restartedCallback != null)
        restartedCallback(missionLevelScenesTransitionSetupData, missionCompletionResults);
      this._gameScenesManager.ReplaceScenes((ScenesTransitionSetupDataSO) missionLevelScenesTransitionSetupData, minDuration: 0.7f);
    }
    else
    {
      missionLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>(this.HandleMissionLevelSceneDidFinish);
      this._gameScenesManager.PopScenes(missionCompletionResults.levelCompletionResults.levelEndAction != LevelCompletionResults.LevelEndAction.Quit ? 1.3f : 0.35f, finishCallback: (System.Action<DiContainer>) (container =>
      {
        System.Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> finishedCallback = this._missionLevelFinishedCallback;
        if (finishedCallback == null)
          return;
        finishedCallback(missionLevelScenesTransitionSetupData, missionCompletionResults);
      }));
    }
  }

  public virtual void HandleTutorialSceneDidFinish(
    TutorialScenesTransitionSetupDataSO tutorialSceneTransitionSetupData,
    TutorialScenesTransitionSetupDataSO.TutorialEndStateType endState)
  {
    tutorialSceneTransitionSetupData.didFinishEvent -= new System.Action<TutorialScenesTransitionSetupDataSO, TutorialScenesTransitionSetupDataSO.TutorialEndStateType>(this.HandleTutorialSceneDidFinish);
    this._gameScenesManager.PopScenes(endState == TutorialScenesTransitionSetupDataSO.TutorialEndStateType.Completed ? 1.3f : 0.35f, finishCallback: (System.Action<DiContainer>) (container =>
    {
      if (endState != TutorialScenesTransitionSetupDataSO.TutorialEndStateType.Restart)
        return;
      this.StartTutorial(tutorialSceneTransitionSetupData.playerSpecificSettings);
    }));
  }

  public virtual void HandleCreditsSceneDidFinish(
    CreditsScenesTransitionSetupDataSO creditsSceneTransitionSetupData)
  {
    creditsSceneTransitionSetupData.didFinishEvent -= new System.Action<CreditsScenesTransitionSetupDataSO>(this.HandleCreditsSceneDidFinish);
    this._gameScenesManager.PopScenes(1.3f);
  }

  public virtual void HandleBeatmapEditorSceneDidFinish(
    BeatmapEditorScenesTransitionSetupDataSO beatmapEditorScenesTransitionSetupData)
  {
    Localization.Instance.SelectLanguage(this._mainSettingsModel.language.value);
    beatmapEditorScenesTransitionSetupData.didFinishEvent -= new System.Action<BeatmapEditorScenesTransitionSetupDataSO>(this.HandleBeatmapEditorSceneDidFinish);
    this._gameScenesManager.PopScenes(0.35f, finishCallback: (System.Action<DiContainer>) (container =>
    {
      System.Action finishedCallback = this._beatmapEditorFinishedCallback;
      if (finishedCallback == null)
        return;
      finishedCallback();
    }));
  }

  public virtual void HandleBeatmapEditorGameSceneDidFinish(
    BeatmapEditorStandardLevelScenesTransitionSetupDataSO beatmapEditorStandardLevelScenesTransitionSetupData,
    LevelCompletionResults levelCompletionResults)
  {
    beatmapEditorStandardLevelScenesTransitionSetupData.didFinishEvent -= new System.Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleBeatmapEditorGameSceneDidFinish);
    this._gameScenesManager.PopScenes(0.35f, finishCallback: (System.Action<DiContainer>) (container => this._beatmapEditorGameplayLevelFinishedCallback(beatmapEditorStandardLevelScenesTransitionSetupData, levelCompletionResults)));
  }

  [CompilerGenerated]
  public virtual void m_CHandleBeatmapEditorSceneDidFinishm_Eb__35_0(DiContainer container)
  {
    System.Action finishedCallback = this._beatmapEditorFinishedCallback;
    if (finishedCallback == null)
      return;
    finishedCallback();
  }
}
