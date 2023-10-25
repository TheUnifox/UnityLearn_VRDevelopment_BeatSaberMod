// Decompiled with JetBrains decompiler
// Type: GameplayCoreSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading.Tasks;
using Zenject;

[ZenjectAllowDuringValidation]
public class GameplayCoreSceneSetupData : SceneSetupData
{
  public readonly IDifficultyBeatmap difficultyBeatmap;
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;
  public readonly GameplayModifiers gameplayModifiers;
  public readonly PlayerSpecificSettings playerSpecificSettings;
  public readonly PracticeSettings practiceSettings;
  public readonly bool useTestNoteCutSoundEffects;
  public readonly EnvironmentInfoSO environmentInfo;
  public readonly ColorScheme colorScheme;
  public readonly MainSettingsModelSO mainSettingsModel;
  public readonly BeatmapDataCache beatmapDataCache;
  protected IReadonlyBeatmapData _transformedBeatmapData;

  public IReadonlyBeatmapData transformedBeatmapData => this._transformedBeatmapData;

  public GameplayCoreSceneSetupData(
    IDifficultyBeatmap difficultyBeatmap,
    IPreviewBeatmapLevel previewBeatmapLevel,
    GameplayModifiers gameplayModifiers,
    PlayerSpecificSettings playerSpecificSettings,
    PracticeSettings practiceSettings,
    bool useTestNoteCutSoundEffects,
    EnvironmentInfoSO environmentInfo,
    ColorScheme colorScheme,
    MainSettingsModelSO mainSettingsModel,
    BeatmapDataCache beatmapDataCache = null)
  {
    this.difficultyBeatmap = difficultyBeatmap;
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.gameplayModifiers = gameplayModifiers;
    this.playerSpecificSettings = playerSpecificSettings;
    this.practiceSettings = practiceSettings;
    this.useTestNoteCutSoundEffects = useTestNoteCutSoundEffects;
    this.environmentInfo = environmentInfo;
    this.colorScheme = colorScheme;
    this.mainSettingsModel = mainSettingsModel;
    this.beatmapDataCache = beatmapDataCache;
  }

  public virtual async Task LoadTransformedBeatmapDataAsync() => this._transformedBeatmapData = await this.GetTransformedBeatmapDataAsync();

  public virtual async Task<IReadonlyBeatmapData> GetTransformedBeatmapDataAsync()
  {
    if (this.difficultyBeatmap == null)
      return (IReadonlyBeatmapData) null;
    IReadonlyBeatmapData beatmapData;
    if (this.beatmapDataCache != null)
      beatmapData = await this.beatmapDataCache.GetBeatmapData(this.difficultyBeatmap, this.environmentInfo, this.playerSpecificSettings);
    else
      beatmapData = await this.difficultyBeatmap.GetBeatmapDataAsync(this.environmentInfo, this.playerSpecificSettings);
    EnvironmentEffectsFilterPreset environmentEffectsFilterPreset = this.difficultyBeatmap.difficulty == BeatmapDifficulty.ExpertPlus ? this.playerSpecificSettings.environmentEffectsFilterExpertPlusPreset : this.playerSpecificSettings.environmentEffectsFilterDefaultPreset;
    return BeatmapDataTransformHelper.CreateTransformedBeatmapData(beatmapData, this.previewBeatmapLevel, this.gameplayModifiers, this.playerSpecificSettings.leftHanded, environmentEffectsFilterPreset, this.environmentInfo.environmentIntensityReductionOptions, this.mainSettingsModel);
  }
}
