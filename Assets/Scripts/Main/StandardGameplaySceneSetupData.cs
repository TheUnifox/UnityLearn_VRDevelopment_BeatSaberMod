// Decompiled with JetBrains decompiler
// Type: StandardGameplaySceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

[ZenjectAllowDuringValidation]
public class StandardGameplaySceneSetupData : SceneSetupData
{
  public readonly bool autoRestart;
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;
  public readonly BeatmapDifficulty beatmapDifficulty;
  public readonly BeatmapCharacteristicSO beatmapCharacteristic;
  public readonly string backButtonText;
  public readonly GameplayModifiers gameplayModifiers;
  public readonly bool startPaused;

  public StandardGameplaySceneSetupData(
    bool autoRestart,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    string backButtonText,
    GameplayModifiers gameplayModifiers,
    bool startPaused)
  {
    this.autoRestart = autoRestart;
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.beatmapDifficulty = beatmapDifficulty;
    this.beatmapCharacteristic = beatmapCharacteristic;
    this.backButtonText = backButtonText;
    this.gameplayModifiers = gameplayModifiers;
    this.startPaused = startPaused;
  }
}
