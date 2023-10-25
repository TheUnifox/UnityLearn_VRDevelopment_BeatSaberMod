// Decompiled with JetBrains decompiler
// Type: BeatmapIdentifierNetSerializableHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class BeatmapIdentifierNetSerializableHelper
{
  public static BeatmapIdentifierNetSerializable GetIdentifier(
    this IDifficultyBeatmap difficultyBeatmap)
  {
    return new BeatmapIdentifierNetSerializable(difficultyBeatmap.level.levelID, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName, difficultyBeatmap.difficulty);
  }

  public static bool HasIdentifier(
    this IDifficultyBeatmap difficultyBeatmap,
    BeatmapIdentifierNetSerializable beatmapId)
  {
    return difficultyBeatmap.level.levelID == beatmapId.levelID && difficultyBeatmap.difficulty == beatmapId.difficulty && difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName == beatmapId.beatmapCharacteristicSerializedName;
  }

  public static PreviewDifficultyBeatmap ToPreviewDifficultyBeatmap(
    this BeatmapIdentifierNetSerializable beatmapId,
    BeatmapLevelsModel beatmapLevelsModel,
    BeatmapCharacteristicCollectionSO beatmapCharacteristicCollection)
  {
    return string.IsNullOrEmpty(beatmapId?.levelID) ? (PreviewDifficultyBeatmap) null : new PreviewDifficultyBeatmap(beatmapLevelsModel.GetLevelPreviewForLevelId(beatmapId.levelID), beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(beatmapId.beatmapCharacteristicSerializedName), beatmapId.difficulty);
  }

  public static BeatmapIdentifierNetSerializable ToIdentifier(
    this PreviewDifficultyBeatmap previewDifficultyBeatmapLevel)
  {
    return previewDifficultyBeatmapLevel == (PreviewDifficultyBeatmap) null ? (BeatmapIdentifierNetSerializable) null : new BeatmapIdentifierNetSerializable(previewDifficultyBeatmapLevel.beatmapLevel.levelID, previewDifficultyBeatmapLevel.beatmapCharacteristic.serializedName, previewDifficultyBeatmapLevel.beatmapDifficulty);
  }
}
