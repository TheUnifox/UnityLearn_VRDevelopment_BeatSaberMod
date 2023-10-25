// Decompiled with JetBrains decompiler
// Type: RunLevelMenuDestination
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class RunLevelMenuDestination : MenuDestination
{
  public readonly IBeatmapLevelPack beatmapLevelPack;
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;
  public readonly BeatmapDifficulty beatmapDifficulty;
  public readonly BeatmapCharacteristicSO beatmapCharacteristic;
  public readonly bool practice;
  public readonly float startSongTime;
  public readonly float songSpeedMultiplier;
  public readonly bool overrideEnvironments;
  public readonly string environmentType;
  public readonly string environmentName;

  public RunLevelMenuDestination(
    IBeatmapLevelPack beatmapLevelPack,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    bool practice,
    float startSongTime,
    float songSpeedMultiplier,
    bool overrideEnvironments,
    string environmentType,
    string environmentName)
  {
    this.beatmapLevelPack = beatmapLevelPack;
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.beatmapDifficulty = beatmapDifficulty;
    this.beatmapCharacteristic = beatmapCharacteristic;
    this.practice = practice;
    this.startSongTime = startSongTime;
    this.songSpeedMultiplier = songSpeedMultiplier;
    this.overrideEnvironments = overrideEnvironments;
    this.environmentType = environmentType;
    this.environmentName = environmentName;
  }
}
