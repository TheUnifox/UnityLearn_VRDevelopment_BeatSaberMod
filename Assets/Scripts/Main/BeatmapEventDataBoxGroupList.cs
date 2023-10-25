// Decompiled with JetBrains decompiler
// Type: BeatmapEventDataBoxGroupList
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BeatmapEventDataBoxGroupList
{
  public bool updateBeatmapDataOnInsert;
  protected readonly BeatmapEventDataBoxGroupProcessor _beatmapEventDataBoxGroupProcessor = new BeatmapEventDataBoxGroupProcessor();
  protected readonly SortedList<BeatmapEventDataBoxGroup, BeatmapEventDataBoxGroup> _sortedList;
  protected readonly int _groupId;
  protected readonly BeatmapData _beatmapData;
  protected readonly IBeatToTimeConvertor _beatToTimeConvertor;
  protected bool _nonSyncedInsertsExist;
  protected static readonly HashSet<BeatmapEventDataBoxGroup> _usedBeatmapEventDataBoxes = new HashSet<BeatmapEventDataBoxGroup>();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  private static void NoDomainReloadInit() => BeatmapEventDataBoxGroupList._usedBeatmapEventDataBoxes.Clear();

  public BeatmapEventDataBoxGroupList(
    int groupId,
    BeatmapData beatmapData,
    IBeatToTimeConvertor beatToTimeConvertor)
  {
    this._groupId = groupId;
    this._beatmapData = beatmapData;
    this._beatToTimeConvertor = beatToTimeConvertor;
    this._sortedList = new SortedList<BeatmapEventDataBoxGroup, BeatmapEventDataBoxGroup>((ISortedListItemProcessor<BeatmapEventDataBoxGroup>) this._beatmapEventDataBoxGroupProcessor);
  }

  public virtual LinkedListNode<BeatmapEventDataBoxGroup> Insert(
    BeatmapEventDataBoxGroup beatmapEventDataBoxGroup)
  {
    Assert.IsFalse(BeatmapEventDataBoxGroupList._usedBeatmapEventDataBoxes.Contains(beatmapEventDataBoxGroup));
    BeatmapEventDataBoxGroupList._usedBeatmapEventDataBoxes.Add(beatmapEventDataBoxGroup);
    LinkedListNode<BeatmapEventDataBoxGroup> linkedListNode = this._sortedList.Insert(beatmapEventDataBoxGroup);
    if (this.updateBeatmapDataOnInsert)
    {
      foreach (BeatmapEventDataBoxGroup dirtyBoxGroup in (IEnumerable<BeatmapEventDataBoxGroup>) this._beatmapEventDataBoxGroupProcessor.dirtyBoxGroups)
        dirtyBoxGroup.SyncWithBeatmapData(this._groupId, this._beatmapData, this._beatToTimeConvertor);
      beatmapEventDataBoxGroup.SyncWithBeatmapData(this._groupId, this._beatmapData, this._beatToTimeConvertor);
      this._beatmapEventDataBoxGroupProcessor.ClearDirtyData();
    }
    else
      this._nonSyncedInsertsExist = true;
    return linkedListNode;
  }

  public virtual void Remove(
    LinkedListNode<BeatmapEventDataBoxGroup> nodeToDelete)
  {
    if (this._nonSyncedInsertsExist)
      this.SyncWithBeatmapData();
    BeatmapEventDataBoxGroup eventDataBoxGroup = nodeToDelete.Value;
    Assert.IsTrue(BeatmapEventDataBoxGroupList._usedBeatmapEventDataBoxes.Contains(eventDataBoxGroup));
    BeatmapEventDataBoxGroupList._usedBeatmapEventDataBoxes.Remove(eventDataBoxGroup);
    this._sortedList.Remove(nodeToDelete);
    eventDataBoxGroup.RemoveBeatmapEventDataFromBeatmapData(this._beatmapData);
    foreach (BeatmapEventDataBoxGroup dirtyBoxGroup in (IEnumerable<BeatmapEventDataBoxGroup>) this._beatmapEventDataBoxGroupProcessor.dirtyBoxGroups)
      dirtyBoxGroup.SyncWithBeatmapData(this._groupId, this._beatmapData, this._beatToTimeConvertor);
    this._beatmapEventDataBoxGroupProcessor.ClearDirtyData();
  }

  public virtual void SyncWithBeatmapData()
  {
    foreach (BeatmapEventDataBoxGroup eventDataBoxGroup in this._sortedList.items)
      eventDataBoxGroup.SyncWithBeatmapData(this._groupId, this._beatmapData, this._beatToTimeConvertor);
    this._beatmapEventDataBoxGroupProcessor.ClearDirtyData();
    this._nonSyncedInsertsExist = false;
  }
}
