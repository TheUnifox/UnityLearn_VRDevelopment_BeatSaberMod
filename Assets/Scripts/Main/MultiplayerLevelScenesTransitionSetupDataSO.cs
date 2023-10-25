// Decompiled with JetBrains decompiler
// Type: MultiplayerLevelScenesTransitionSetupDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class MultiplayerLevelScenesTransitionSetupDataSO : LevelScenesTransitionSetupDataSO
{
  [SerializeField]
  protected SceneInfo _multiplayerLevelSceneInfo;
  [SerializeField]
  protected SceneInfo _gameCoreSceneInfo;
  [SerializeField]
  protected EnvironmentInfoSO _multiplayerEnvironmentInfo;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [CompilerGenerated]
  protected string m_CgameMode;
  [CompilerGenerated]
  protected IPreviewBeatmapLevel m_CpreviewBeatmapLevel;
  [CompilerGenerated]
  protected BeatmapDifficulty m_CbeatmapDifficulty;
  [CompilerGenerated]
  protected IDifficultyBeatmap m_CdifficultyBeatmap;
  [CompilerGenerated]
  protected BeatmapCharacteristicSO m_CbeatmapCharacteristic;
  [CompilerGenerated]
  protected bool m_CusingOverrideColorScheme;
  [CompilerGenerated]
  protected ColorScheme m_CcolorScheme;

  public event System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> didFinishEvent;

  public event System.Action<MultiplayerLevelScenesTransitionSetupDataSO, DisconnectedReason> didDisconnectEvent;

  public string gameMode
  {
    get => this.m_CgameMode;
    private set => this.m_CgameMode = value;
  }

  public IPreviewBeatmapLevel previewBeatmapLevel
  {
    get => this.m_CpreviewBeatmapLevel;
    private set => this.m_CpreviewBeatmapLevel = value;
  }

  public BeatmapDifficulty beatmapDifficulty
  {
    get => this.m_CbeatmapDifficulty;
    private set => this.m_CbeatmapDifficulty = value;
  }

  public IDifficultyBeatmap difficultyBeatmap
  {
    get => this.m_CdifficultyBeatmap;
    private set => this.m_CdifficultyBeatmap = value;
  }

  public BeatmapCharacteristicSO beatmapCharacteristic
  {
    get => this.m_CbeatmapCharacteristic;
    private set => this.m_CbeatmapCharacteristic = value;
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

  public virtual void Init(
    string gameMode,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    IDifficultyBeatmap difficultyBeatmap,
    ColorScheme overrideColorScheme,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    bool useTestNoteCutSoundEffects = false)
  {
    this.usingOverrideColorScheme = overrideColorScheme != null;
    this.colorScheme = overrideColorScheme ?? new ColorScheme(this._multiplayerEnvironmentInfo.colorScheme);
    this.gameMode = gameMode;
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.beatmapDifficulty = beatmapDifficulty;
    this.difficultyBeatmap = difficultyBeatmap;
    this.beatmapCharacteristic = beatmapCharacteristic;
    this.gameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, previewBeatmapLevel, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects, this._multiplayerEnvironmentInfo, this.colorScheme, this._mainSettingsModel);
    this.Init(new SceneInfo[3]
    {
      this._multiplayerEnvironmentInfo.sceneInfo,
      this._multiplayerLevelSceneInfo,
      this._gameCoreSceneInfo
    }, new SceneSetupData[4]
    {
      (SceneSetupData) new EnvironmentSceneSetupData(this._multiplayerEnvironmentInfo, previewBeatmapLevel, false),
      (SceneSetupData) new MultiplayerLevelSceneSetupData(previewBeatmapLevel, beatmapDifficulty, beatmapCharacteristic, difficultyBeatmap != null),
      (SceneSetupData) this.gameplayCoreSceneSetupData,
      (SceneSetupData) new GameCoreSceneSetupData()
    });
  }

  public virtual void Finish(MultiplayerResultsData resultsData)
  {
    System.Action<MultiplayerLevelScenesTransitionSetupDataSO, MultiplayerResultsData> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this, resultsData);
  }

  public virtual void FinishWithDisconnect(DisconnectedReason disconnectedReason)
  {
    System.Action<MultiplayerLevelScenesTransitionSetupDataSO, DisconnectedReason> didDisconnectEvent = this.didDisconnectEvent;
    if (didDisconnectEvent == null)
      return;
    didDisconnectEvent(this, disconnectedReason);
  }
}
