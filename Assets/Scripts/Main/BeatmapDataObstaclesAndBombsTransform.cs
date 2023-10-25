// Decompiled with JetBrains decompiler
// Type: BeatmapDataObstaclesAndBombsTransform
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

public abstract class BeatmapDataObstaclesAndBombsTransform
{
  public static IReadonlyBeatmapData CreateTransformedData(
    IReadonlyBeatmapData beatmapData,
    GameplayModifiers.EnabledObstacleType enabledObstaclesType,
    bool noBombs)
  {
    return (IReadonlyBeatmapData) beatmapData.GetFilteredCopy(new Func<BeatmapDataItem, BeatmapDataItem>(ProcessData));

    BeatmapDataItem ProcessData(BeatmapDataItem beatmapDataItem) => !BeatmapDataObstaclesAndBombsTransform.ShouldUseBeatmapDataItem(beatmapDataItem, enabledObstaclesType, noBombs) ? (BeatmapDataItem) null : beatmapDataItem;
  }

  private static bool ShouldUseBeatmapDataItem(
    BeatmapDataItem beatmapDataItem,
    GameplayModifiers.EnabledObstacleType enabledObstaclesType,
    bool noBombs)
  {
    switch (beatmapDataItem)
    {
      case ObstacleData _:
        switch (enabledObstaclesType)
        {
          case GameplayModifiers.EnabledObstacleType.FullHeightOnly:
            return beatmapDataItem is ObstacleData obstacleData && obstacleData.height == 3;
          case GameplayModifiers.EnabledObstacleType.NoObstacles:
            return false;
        }
        break;
      case NoteData noteData:
        if (noteData.colorType == ColorType.None)
          return !noBombs;
        break;
    }
    return true;
  }
}
