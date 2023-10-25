// Decompiled with JetBrains decompiler
// Type: BeatmapData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class BeatmapData : IReadonlyBeatmapData, IBeatmapDataBasicInfo
{
  [CompilerGenerated]
  protected bool m_CupdateAllBeatmapDataOnInsert;
  [CompilerGenerated]
  protected int m_CcuttableNotesCount;
  [CompilerGenerated]
  protected int m_CobstaclesCount;
  [CompilerGenerated]
  protected int m_CbombsCount;
  protected readonly ISortedList<BeatmapDataItem> _allBeatmapData;
  protected readonly Dictionary<BeatmapDataItem, LinkedListNode<BeatmapDataItem>> _allBeatmapDataItemToNodeMap;
  protected readonly BeatmapDataSortedListForTypeAndIds<BeatmapDataItem> _beatmapDataItemsPerTypeAndId = new BeatmapDataSortedListForTypeAndIds<BeatmapDataItem>();
  protected readonly int _numberOfLines;
  protected readonly HashSet<string> _specialBasicBeatmapEventKeywords = new HashSet<string>();
  protected readonly BeatmapObjectsInTimeRowProcessor _beatmapObjectsInTimeRowProcessor;
  protected float _prevAddedBeatmapObjectDataTime;
  protected bool _isCreatingFilteredCopy;

  public bool updateAllBeatmapDataOnInsert
  {
    get => this.m_CupdateAllBeatmapDataOnInsert;
    set => this.m_CupdateAllBeatmapDataOnInsert = value;
  }

  public LinkedList<BeatmapDataItem> allBeatmapDataItems => this._allBeatmapData.items;

  public int numberOfLines => this._numberOfLines;

  public int cuttableNotesCount
  {
    get => this.m_CcuttableNotesCount;
    private set => this.m_CcuttableNotesCount = value;
  }

  public int obstaclesCount
  {
    get => this.m_CobstaclesCount;
    private set => this.m_CobstaclesCount = value;
  }

  public int bombsCount
  {
    get => this.m_CbombsCount;
    private set => this.m_CbombsCount = value;
  }

  public int spawnRotationEventsCount => this.GetBeatmapDataItemsCount<SpawnRotationBeatmapEventData>(0);

  public IEnumerable<string> specialBasicBeatmapEventKeywords => (IEnumerable<string>) this._specialBasicBeatmapEventKeywords;

  public event System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> beatmapEventDataWasInsertedEvent;

  public event System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> beatmapEventDataWillBeRemovedEvent;

  public event System.Action<BeatmapEventData> beatmapEventDataWasRemovedEvent;

  public BeatmapData(int numberOfLines)
  {
    this._numberOfLines = numberOfLines;
    this._allBeatmapData = (ISortedList<BeatmapDataItem>) new SortedList<BeatmapDataItem>((ISortedListItemProcessor<BeatmapDataItem>) null);
    this._allBeatmapDataItemToNodeMap = new Dictionary<BeatmapDataItem, LinkedListNode<BeatmapDataItem>>();
    this._beatmapObjectsInTimeRowProcessor = new BeatmapObjectsInTimeRowProcessor(numberOfLines);
  }

  public virtual IEnumerable<T> GetBeatmapDataItems<T>(int subtypeGroupIdentifier) where T : BeatmapDataItem => this._beatmapDataItemsPerTypeAndId.GetItems<T>(subtypeGroupIdentifier);

  public virtual int GetBeatmapDataItemsCount<T>(int subtypeGroupIdentifier) where T : BeatmapDataItem => this._beatmapDataItemsPerTypeAndId.GetCount<T>(subtypeGroupIdentifier);

  public virtual IEnumerable<T> GetBeatmapDataItemsMerged<T>(params int[] subtypeGroupIdentifiers) where T : BeatmapDataItem
  {
    List<BeatmapDataItem> source = new List<BeatmapDataItem>();
    LinkedListNode<BeatmapDataItem>[] array = ((IEnumerable<LinkedListNode<BeatmapDataItem>>) this._beatmapDataItemsPerTypeAndId.sortedListHeads).Where<LinkedListNode<BeatmapDataItem>>((Func<LinkedListNode<BeatmapDataItem>, bool>) (head => head.Value.GetType() == typeof (T) && ((IEnumerable<int>) subtypeGroupIdentifiers).Contains<int>(head.Value.subtypeGroupIdentifier))).ToArray<LinkedListNode<BeatmapDataItem>>();
    BinaryHeap<BeatmapData.BeatmapDataBinaryHeapItem> binaryHeap = new BinaryHeap<BeatmapData.BeatmapDataBinaryHeapItem>(array.Length);
    foreach (LinkedListNode<BeatmapDataItem> node in array)
    {
      if (node != null)
        binaryHeap.Insert(new BeatmapData.BeatmapDataBinaryHeapItem(node));
    }
    BeatmapData.BeatmapDataBinaryHeapItem output;
    while (binaryHeap.RemoveMin(out output))
    {
      source.Add(output.node.Value);
      if (output.node.Next != null)
      {
        output.node = output.node.Next;
        binaryHeap.Insert(output);
      }
    }
    return source.Cast<T>();
  }

  public virtual int GetBeatmapDataItemsMergedCount<T>(params int[] subtypeGroupIdentifiers) where T : BeatmapDataItem => this.GetBeatmapDataItemsMerged<T>(subtypeGroupIdentifiers).Count<T>();

  public virtual void AddBeatmapObjectData(BeatmapObjectData beatmapObjectData)
  {
    this._prevAddedBeatmapObjectDataTime = beatmapObjectData.time;
    switch (beatmapObjectData)
    {
      case ObstacleData _:
        ++this.obstaclesCount;
        break;
      case NoteData noteData:
        this._beatmapObjectsInTimeRowProcessor.ProcessNote(noteData);
        if (noteData.cutDirection != NoteCutDirection.None)
        {
          ++this.cuttableNotesCount;
          break;
        }
        ++this.bombsCount;
        break;
      case SliderData sliderData:
        this._beatmapObjectsInTimeRowProcessor.ProcessSlider(sliderData);
        break;
    }
    LinkedListNode<BeatmapDataItem> node = this._beatmapDataItemsPerTypeAndId.InsertItem((BeatmapDataItem) beatmapObjectData);
    if (!this.updateAllBeatmapDataOnInsert)
      return;
    this.InsertToAllBeatmapData((BeatmapDataItem) beatmapObjectData, node);
  }

  public virtual void AddBeatmapObjectDataInOrder(BeatmapObjectData beatmapObjectData)
  {
    this.AddBeatmapObjectData(beatmapObjectData);
    this.InsertToAllBeatmapData((BeatmapDataItem) beatmapObjectData);
  }

  public virtual void InsertBeatmapEventData(BeatmapEventData beatmapEventData)
  {
    LinkedListNode<BeatmapDataItem> node = this._beatmapDataItemsPerTypeAndId.InsertItem((BeatmapDataItem) beatmapEventData);
    if (!this.updateAllBeatmapDataOnInsert)
      return;
    LinkedListNode<BeatmapDataItem> allBeatmapData = this.InsertToAllBeatmapData((BeatmapDataItem) beatmapEventData, node);
    System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> wasInsertedEvent = this.beatmapEventDataWasInsertedEvent;
    if (wasInsertedEvent == null)
      return;
    wasInsertedEvent(beatmapEventData, allBeatmapData);
  }

  public virtual void InsertBeatmapEventDataInOrder(BeatmapEventData beatmapEventData)
  {
    this.InsertBeatmapEventData(beatmapEventData);
    LinkedListNode<BeatmapDataItem> allBeatmapData = this.InsertToAllBeatmapData((BeatmapDataItem) beatmapEventData);
    System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> wasInsertedEvent = this.beatmapEventDataWasInsertedEvent;
    if (wasInsertedEvent == null)
      return;
    wasInsertedEvent(beatmapEventData, allBeatmapData);
  }

  public virtual void RemoveBeatmapEventData(BeatmapEventData beatmapEventData)
  {
    LinkedListNode<BeatmapDataItem> beatmapDataItemToNode = this._allBeatmapDataItemToNodeMap[(BeatmapDataItem) beatmapEventData];
    System.Action<BeatmapEventData, LinkedListNode<BeatmapDataItem>> willBeRemovedEvent = this.beatmapEventDataWillBeRemovedEvent;
    if (willBeRemovedEvent != null)
      willBeRemovedEvent(beatmapEventData, beatmapDataItemToNode);
    this._beatmapDataItemsPerTypeAndId.RemoveItem((BeatmapDataItem) beatmapEventData);
    this._allBeatmapData.Remove(beatmapDataItemToNode);
    System.Action<BeatmapEventData> dataWasRemovedEvent = this.beatmapEventDataWasRemovedEvent;
    if (dataWasRemovedEvent == null)
      return;
    dataWasRemovedEvent(beatmapEventData);
  }

  public virtual void AddSpecialBasicBeatmapEventKeyword(string specialBasicBeatmapEventKeyword) => this._specialBasicBeatmapEventKeywords.Add(specialBasicBeatmapEventKeyword);

  public virtual void ProcessRemainingData() => this._beatmapObjectsInTimeRowProcessor.ProcessAllRemainingData();

  public virtual void ProcessAndSortBeatmapData()
  {
    this._allBeatmapData.items.Clear();
    LinkedListNode<BeatmapDataItem>[] sortedListHeads = this._beatmapDataItemsPerTypeAndId.sortedListHeads;
    BinaryHeap<BeatmapData.BeatmapDataBinaryHeapItem> binaryHeap = new BinaryHeap<BeatmapData.BeatmapDataBinaryHeapItem>(sortedListHeads.Length);
    foreach (LinkedListNode<BeatmapDataItem> node in sortedListHeads)
    {
      if (node != null)
        binaryHeap.Insert(new BeatmapData.BeatmapDataBinaryHeapItem(node));
    }
    BeatmapData.BeatmapDataBinaryHeapItem output;
    while (binaryHeap.RemoveMin(out output))
    {
      LinkedListNode<BeatmapDataItem> linkedListNode = this._allBeatmapData.Insert(output.node.Value);
      this._allBeatmapDataItemToNodeMap[output.node.Value] = linkedListNode;
      if (output.node.Next != null)
      {
        output.node = output.node.Next;
        binaryHeap.Insert(output);
      }
    }
  }

  public virtual BeatmapData GetCopy()
  {
    BeatmapData copy1 = new BeatmapData(this._numberOfLines);
    foreach (BeatmapDataItem allBeatmapDataItem in this.allBeatmapDataItems)
    {
      BeatmapDataItem copy2 = allBeatmapDataItem.GetCopy();
      if (copy2 is BeatmapEventData beatmapEventData)
        copy1.InsertBeatmapEventDataInOrder(beatmapEventData);
      else if (copy2 is BeatmapObjectData beatmapObjectData)
        copy1.AddBeatmapObjectDataInOrder(beatmapObjectData);
    }
    return copy1;
  }

  public virtual BeatmapData GetFilteredCopy(
    Func<BeatmapDataItem, BeatmapDataItem> processDataItem)
  {
    this._isCreatingFilteredCopy = true;
    BeatmapData filteredCopy = new BeatmapData(this._numberOfLines);
    foreach (BeatmapDataItem allBeatmapDataItem in this.allBeatmapDataItems)
    {
      BeatmapDataItem beatmapDataItem = processDataItem(allBeatmapDataItem.GetCopy());
      if (beatmapDataItem != null)
      {
        if (beatmapDataItem is BeatmapEventData beatmapEventData)
          filteredCopy.InsertBeatmapEventDataInOrder(beatmapEventData);
        else if (beatmapDataItem is BeatmapObjectData beatmapObjectData)
          filteredCopy.AddBeatmapObjectDataInOrder(beatmapObjectData);
      }
    }
    this._isCreatingFilteredCopy = false;
    return filteredCopy;
  }

  public virtual LinkedListNode<BeatmapDataItem> InsertToAllBeatmapData(
    BeatmapDataItem beatmapDataItem,
    LinkedListNode<BeatmapDataItem> node = null)
  {
    if (node?.Previous != null)
      this._allBeatmapData.TouchLastUsedNode(this._allBeatmapDataItemToNodeMap[node.Previous.Value]);
    LinkedListNode<BeatmapDataItem> allBeatmapData = this._allBeatmapData.Insert(beatmapDataItem);
    this._allBeatmapDataItemToNodeMap[beatmapDataItem] = allBeatmapData;
    return allBeatmapData;
  }

  public class BeatmapDataBinaryHeapItem : IComparable<BeatmapData.BeatmapDataBinaryHeapItem>
  {
    public LinkedListNode<BeatmapDataItem> node;

    public BeatmapDataBinaryHeapItem(LinkedListNode<BeatmapDataItem> node) => this.node = node;

    public virtual int CompareTo(BeatmapData.BeatmapDataBinaryHeapItem other)
    {
      int num = this.node.Value.CompareTo(other.node.Value);
      return num != 0 ? num : this.node.Value.subtypeIdentifier.CompareTo(other.node.Value.subtypeIdentifier);
    }
  }
}
