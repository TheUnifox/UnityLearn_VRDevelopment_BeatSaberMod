// Decompiled with JetBrains decompiler
// Type: IReadonlyBeatmapData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public interface IReadonlyBeatmapData : IBeatmapDataBasicInfo
{
  LinkedList<BeatmapDataItem> allBeatmapDataItems { get; }

  IEnumerable<T> GetBeatmapDataItems<T>(int subtypeGroupIdentifier) where T : BeatmapDataItem;

  int spawnRotationEventsCount { get; }

  event System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> beatmapEventDataWasInsertedEvent;

  event System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> beatmapEventDataWillBeRemovedEvent;

  event System.Action<BeatmapEventData> beatmapEventDataWasRemovedEvent;

  BeatmapData GetCopy();

  BeatmapData GetFilteredCopy(
    Func<BeatmapDataItem, BeatmapDataItem> processDataItem);
}
