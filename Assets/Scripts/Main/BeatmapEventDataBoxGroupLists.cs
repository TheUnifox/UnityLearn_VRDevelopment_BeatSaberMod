// Decompiled with JetBrains decompiler
// Type: BeatmapEventDataBoxGroupLists
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public class BeatmapEventDataBoxGroupLists
{
  protected readonly Dictionary<int, BeatmapEventDataBoxGroupList> _beatmapEventDataBoxGroupListDict = new Dictionary<int, BeatmapEventDataBoxGroupList>();
  protected readonly BeatmapData _beatmapData;
  protected readonly IBeatToTimeConvertor _beatToTimeConvertor;
  protected bool _updateBeatmapDataOnInsert;

  public BeatmapEventDataBoxGroupLists(
    BeatmapData beatmapData,
    IBeatToTimeConvertor beatToTimeConvertor,
    bool updateBeatmapDataOnInsert)
  {
    this._beatmapData = beatmapData;
    this._beatToTimeConvertor = beatToTimeConvertor;
    this._updateBeatmapDataOnInsert = updateBeatmapDataOnInsert;
  }

  public virtual LinkedListNode<BeatmapEventDataBoxGroup> Insert(
    int groupId,
    BeatmapEventDataBoxGroup beatmapEventDataBoxGroup)
  {
    BeatmapEventDataBoxGroupList dataBoxGroupList;
    if (!this._beatmapEventDataBoxGroupListDict.TryGetValue(groupId, out dataBoxGroupList))
    {
      dataBoxGroupList = new BeatmapEventDataBoxGroupList(groupId, this._beatmapData, this._beatToTimeConvertor)
      {
        updateBeatmapDataOnInsert = this._updateBeatmapDataOnInsert
      };
      this._beatmapEventDataBoxGroupListDict[groupId] = dataBoxGroupList;
    }
    return dataBoxGroupList.Insert(beatmapEventDataBoxGroup);
  }

  public virtual void Remove(
    int groupId,
    LinkedListNode<BeatmapEventDataBoxGroup> nodeToDelete)
  {
    BeatmapEventDataBoxGroupList dataBoxGroupList;
    if (!this._beatmapEventDataBoxGroupListDict.TryGetValue(groupId, out dataBoxGroupList))
      throw new ArgumentOutOfRangeException();
    dataBoxGroupList.Remove(nodeToDelete);
  }

  public virtual void ToggleUpdateBeatmapDataOnInsert(bool enableUpdateOnInsert)
  {
    this._updateBeatmapDataOnInsert = enableUpdateOnInsert;
    foreach (KeyValuePair<int, BeatmapEventDataBoxGroupList> keyValuePair in this._beatmapEventDataBoxGroupListDict)
      keyValuePair.Value.updateBeatmapDataOnInsert = enableUpdateOnInsert;
  }

  public virtual void SyncWithBeatmapData()
  {
    foreach (KeyValuePair<int, BeatmapEventDataBoxGroupList> keyValuePair in this._beatmapEventDataBoxGroupListDict)
      keyValuePair.Value.SyncWithBeatmapData();
  }
}
