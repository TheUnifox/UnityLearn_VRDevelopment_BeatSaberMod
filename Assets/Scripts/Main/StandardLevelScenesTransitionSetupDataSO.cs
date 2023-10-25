// Decompiled with JetBrains decompiler
// Type: StandardLevelScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class StandardLevelScenesTransitionSetupDataSO : LevelScenesTransitionSetupDataSO
{
  [SerializeField]
  protected SceneInfo _standardGameplaySceneInfo;
  [SerializeField]
  protected SceneInfo _gameCoreSceneInfo;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [CompilerGenerated]
  protected string m_CgameMode;
  [CompilerGenerated]
  protected IDifficultyBeatmap m_CdifficultyBeatmap;
  [CompilerGenerated]
  protected PracticeSettings m_CpracticeSettings;
  [CompilerGenerated]
  protected bool m_CusingOverrideColorScheme;
  [CompilerGenerated]
  protected ColorScheme m_CcolorScheme;
  [CompilerGenerated]
  protected bool m_CusingOverrideEnvironment;
  [CompilerGenerated]
  protected EnvironmentInfoSO m_CenvironmentInfo;
  [CompilerGenerated]
  protected GameplayModifiers m_CgameplayModifiers;

  public event System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> didFinishEvent;

  public string gameMode
  {
    get => this.m_CgameMode;
    private set => this.m_CgameMode = value;
  }

  public IDifficultyBeatmap difficultyBeatmap
  {
    get => this.m_CdifficultyBeatmap;
    private set => this.m_CdifficultyBeatmap = value;
  }

  public PracticeSettings practiceSettings
  {
    get => this.m_CpracticeSettings;
    private set => this.m_CpracticeSettings = value;
  }

  public bool usingOverrideColorScheme
  {
    get => this.m_CusingOverrideColorScheme;
    private set => this.m_CusingOverrideColorScheme = value;
  }

  public ColorScheme colorScheme
  {
    get => this.m_CcolorScheme;
    private set => this.m_CcolorScheme = value;
  }

  public bool usingOverrideEnvironment
  {
    get => this.m_CusingOverrideEnvironment;
    private set => this.m_CusingOverrideEnvironment = value;
  }

  public EnvironmentInfoSO environmentInfo
  {
    get => this.m_CenvironmentInfo;
    private set => this.m_CenvironmentInfo = value;
  }

  public GameplayModifiers gameplayModifiers
  {
    get => this.m_CgameplayModifiers;
    private set => this.m_CgameplayModifiers = value;
  }

  public virtual void Init(
    string gameMode,
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    OverrideEnvironmentSettings overrideEnvironmentSettings,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    string backButtonText,
    bool useTestNoteCutSoundEffects = false,
    bool startPaused = false,
    BeatmapDataCache beatmapDataCache = null)
  {
    this.gameMode = gameMode;
    this.difficultyBeatmap = difficultyBeatmap;
    this.practiceSettings = practiceSettings;
    this.gameplayModifiers = gameplayModifiers;
    this.environmentInfo = difficultyBeatmap.GetEnvironmentInfo();
    this.usingOverrideEnvironment = overrideEnvironmentSettings != null && overrideEnvironmentSettings.overrideEnvironments;
    if (this.usingOverrideEnvironment)
    {
      EnvironmentInfoSO environmentInfoForType = overrideEnvironmentSettings?.GetOverrideEnvironmentInfoForType(this.environmentInfo.environmentType);
      if ((UnityEngine.Object) environmentInfoForType != (UnityEngine.Object) null)
      {
        if (this.environmentInfo.environmentName == environmentInfoForType.environmentName)
          this.usingOverrideEnvironment = false;
        else
          this.environmentInfo = environmentInfoForType;
      }
    }
    this.usingOverrideColorScheme = overrideColorScheme != null;
    this.colorScheme = overrideColorScheme ?? new ColorScheme(this.environmentInfo.colorScheme);
    IBeatmapLevel level = difficultyBeatmap.level;
    this.gameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects, this.environmentInfo, this.colorScheme, this._mainSettingsModel, beatmapDataCache);
    this.Init(new SceneInfo[3]
    {
      this.environmentInfo.sceneInfo,
      this._standardGameplaySceneInfo,
      this._gameCoreSceneInfo
    }, new SceneSetupData[4]
    {
      (SceneSetupData) new EnvironmentSceneSetupData(this.environmentInfo, previewBeatmapLevel, this.usingOverrideEnvironment),
      (SceneSetupData) new StandardGameplaySceneSetupData(playerSpecificSettings.autoRestart, (IPreviewBeatmapLevel) level, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic, backButtonText, gameplayModifiers, startPaused),
      (SceneSetupData) this.gameplayCoreSceneSetupData,
      (SceneSetupData) new GameCoreSceneSetupData()
    });
  }

  public virtual void Finish(LevelCompletionResults levelCompletionResults)
  {
    System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, levelCompletionResults);
  }
}
