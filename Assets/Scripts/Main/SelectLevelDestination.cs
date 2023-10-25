// Decompiled with JetBrains decompiler
// Type: SelectLevelDestination
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class SelectLevelDestination : MenuDestination
{
  public readonly IBeatmapLevelPack beatmapLevelPack;
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;
  public readonly BeatmapDifficulty beatmapDifficulty;
  public readonly BeatmapCharacteristicSO beatmapCharacteristic;

  public SelectLevelDestination(
    IBeatmapLevelPack beatmapLevelPack,
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic)
  {
    this.beatmapLevelPack = beatmapLevelPack;
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.beatmapDifficulty = beatmapDifficulty;
    this.beatmapCharacteristic = beatmapCharacteristic;
  }
}
