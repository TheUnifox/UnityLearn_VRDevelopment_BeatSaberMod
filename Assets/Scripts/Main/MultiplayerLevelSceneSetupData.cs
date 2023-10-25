// Decompiled with JetBrains decompiler
// Type: MultiplayerLevelSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

[ZenjectAllowDuringValidation]
public class MultiplayerLevelSceneSetupData : SceneSetupData
{
  public readonly IPreviewBeatmapLevel previewBeatmapLevel;
  public readonly BeatmapDifficulty beatmapDifficulty;
  public readonly BeatmapCharacteristicSO beatmapCharacteristic;
  public readonly bool hasSong;

  public MultiplayerLevelSceneSetupData(
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapDifficulty beatmapDifficulty,
    BeatmapCharacteristicSO beatmapCharacteristic,
    bool hasSong)
  {
    this.previewBeatmapLevel = previewBeatmapLevel;
    this.beatmapDifficulty = beatmapDifficulty;
    this.beatmapCharacteristic = beatmapCharacteristic;
    this.hasSong = hasSong;
  }
}
