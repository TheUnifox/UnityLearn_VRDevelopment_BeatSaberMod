// Decompiled with JetBrains decompiler
// Type: BeatmapDataZenModeTransform
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

public abstract class BeatmapDataZenModeTransform
{
  public static IReadonlyBeatmapData CreateTransformedData(IReadonlyBeatmapData beatmapData)
  {
    return (IReadonlyBeatmapData) beatmapData.GetFilteredCopy(new Func<BeatmapDataItem, BeatmapDataItem>(ProcessData));

    BeatmapDataItem ProcessData(BeatmapDataItem beatmapDataItem) => !(beatmapDataItem is BeatmapObjectData) || beatmapDataItem is WaypointData ? beatmapDataItem : (BeatmapDataItem) null;
  }
}
