// Decompiled with JetBrains decompiler
// Type: LevelGameplaySetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class LevelGameplaySetupData : ILevelGameplaySetupData
{
  protected PreviewDifficultyBeatmap _beatmapLevel;
  protected GameplayModifiers _gameplayModifiers = GameplayModifiers.noModifiers;

  public PreviewDifficultyBeatmap beatmapLevel => this._beatmapLevel;

  public GameplayModifiers gameplayModifiers => this._gameplayModifiers;

  public LevelGameplaySetupData()
  {
  }

  public LevelGameplaySetupData(
    PreviewDifficultyBeatmap beatmapLevel,
    GameplayModifiers gameplayModifiers)
  {
    this._beatmapLevel = beatmapLevel;
    this._gameplayModifiers = gameplayModifiers ?? GameplayModifiers.noModifiers;
  }

  public virtual void ClearGameplaySetupData()
  {
    this._beatmapLevel = (PreviewDifficultyBeatmap) null;
    this._gameplayModifiers = GameplayModifiers.noModifiers;
  }

  public virtual void SetBeatmapLevel(PreviewDifficultyBeatmap beatmapLevel) => this._beatmapLevel = beatmapLevel;

  public virtual void SetGameplayModifiers(GameplayModifiers gameplayModifiers) => this._gameplayModifiers = gameplayModifiers ?? GameplayModifiers.noModifiers;
}
