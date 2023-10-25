// Decompiled with JetBrains decompiler
// Type: MissionGameplaySceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class MissionGameplaySceneSetupData : SceneSetupData
{
  public readonly MissionObjective[] missionObjectives;
  public readonly bool autoRestart;
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;
  public readonly BeatmapDifficulty beatmapDifficulty;
  public readonly BeatmapCharacteristicSO beatmapCharacteristic;
  public readonly GameplayModifiers gameplayModifiers;
  public readonly string backButtonText;

  public MissionGameplaySceneSetupData(
    MissionObjective[] missionObjectives,
    bool autoRestart,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    GameplayModifiers gameplayModifiers,
    string backButtonText)
  {
    this.missionObjectives = missionObjectives;
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.beatmapDifficulty = beatmapDifficulty;
    this.beatmapCharacteristic = beatmapCharacteristic;
    this.autoRestart = autoRestart;
    this.gameplayModifiers = gameplayModifiers;
    this.backButtonText = backButtonText;
  }
}
