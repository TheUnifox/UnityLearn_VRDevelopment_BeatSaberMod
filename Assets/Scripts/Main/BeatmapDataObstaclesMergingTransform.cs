// Decompiled with JetBrains decompiler
// Type: BeatmapDataObstaclesMergingTransform
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

public abstract class BeatmapDataObstaclesMergingTransform
{
  public static IReadonlyBeatmapData CreateTransformedData(IReadonlyBeatmapData beatmapData)
  {
    ObstacleData[] prevObstacleDataInLines = new ObstacleData[beatmapData.numberOfLines];
    return (IReadonlyBeatmapData) beatmapData.GetFilteredCopy(new Func<BeatmapDataItem, BeatmapDataItem>(ProcessData));

    BeatmapDataItem ProcessData(BeatmapDataItem beatmapDataItem)
    {
      switch (beatmapDataItem)
      {
        case ObstacleData secondObstacle:
          ObstacleData firstObstacle = prevObstacleDataInLines[secondObstacle.lineIndex];
          if (firstObstacle == null || !BeatmapDataObstaclesMergingTransform.CanBeMerged(firstObstacle, secondObstacle))
            return (BeatmapDataItem) (prevObstacleDataInLines[secondObstacle.lineIndex] = secondObstacle);
          firstObstacle.UpdateDuration(secondObstacle.time - firstObstacle.time + secondObstacle.duration);
          return (BeatmapDataItem) null;
        case NoteData noteData:
          prevObstacleDataInLines[noteData.lineIndex] = (ObstacleData) null;
          break;
        case SliderData sliderData:
          prevObstacleDataInLines[sliderData.headLineIndex] = (ObstacleData) null;
          break;
        case SpawnRotationBeatmapEventData _:
          for (int index = 0; index < prevObstacleDataInLines.Length; ++index)
            prevObstacleDataInLines[index] = (ObstacleData) null;
          break;
      }
      return beatmapDataItem;
    }
  }

  private static bool CanBeMerged(ObstacleData firstObstacle, ObstacleData secondObstacle) => secondObstacle.height == firstObstacle.height && secondObstacle.width == firstObstacle.width && secondObstacle.lineIndex == firstObstacle.lineIndex && secondObstacle.lineLayer == firstObstacle.lineLayer && (double) secondObstacle.time - (double) firstObstacle.time - (double) firstObstacle.duration < 0.5;
}
