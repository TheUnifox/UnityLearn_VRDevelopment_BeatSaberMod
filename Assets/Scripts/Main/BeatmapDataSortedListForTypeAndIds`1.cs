// Decompiled with JetBrains decompiler
// Type: BeatmapDataSortedListForTypeAndIds`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;

public class BeatmapDataSortedListForTypeAndIds<TBase> where TBase : BeatmapDataItem
{
  protected readonly Dictionary<(System.Type, int), ISortedList<TBase>> _items = new Dictionary<(System.Type, int), ISortedList<TBase>>();
  protected readonly Dictionary<System.Type, ISortedListItemProcessor<TBase>> _sortedListsDataProcessors = new Dictionary<System.Type, ISortedListItemProcessor<TBase>>()
  {
    {
      typeof (NoteData),
      (ISortedListItemProcessor<TBase>) null
    },
    {
      typeof (ObstacleData),
      (ISortedListItemProcessor<TBase>) null
    },
    {
      typeof (SliderData),
      (ISortedListItemProcessor<TBase>) null
    },
    {
      typeof (WaypointData),
      (ISortedListItemProcessor<TBase>) null
    },
    {
      typeof (BasicBeatmapEventData),
      (ISortedListItemProcessor<TBase>) new BasicBeatmapEventDataProcessor()
    },
    {
      typeof (SpawnRotationBeatmapEventData),
      (ISortedListItemProcessor<TBase>) new SpawnRotationBeatmapEventDataProcessor()
    },
    {
      typeof (BPMChangeBeatmapEventData),
      (ISortedListItemProcessor<TBase>) null
    },
    {
      typeof (LightColorBeatmapEventData),
      (ISortedListItemProcessor<TBase>) new LightColorBeatmapEventDataProcessor()
    },
    {
      typeof (LightRotationBeatmapEventData),
      (ISortedListItemProcessor<TBase>) new LightRotationBeatmapEventDataProcessor()
    },
    {
      typeof (LightTranslationBeatmapEventData),
      (ISortedListItemProcessor<TBase>) new LightTranslationBeatmapEventDataProcessor()
    },
    {
      typeof (ColorBoostBeatmapEventData),
      (ISortedListItemProcessor<TBase>) new ColorBoostBeatmapEventDataProcessor()
    }
  };
  protected readonly Dictionary<TBase, LinkedListNode<TBase>> _itemToNodeMap = new Dictionary<TBase, LinkedListNode<TBase>>();

  public LinkedListNode<TBase>[] sortedListHeads => this._items.Values.Select<ISortedList<TBase>, LinkedListNode<TBase>>((Func<ISortedList<TBase>, LinkedListNode<TBase>>) (listItem => listItem.items.First)).Where<LinkedListNode<TBase>>((Func<LinkedListNode<TBase>, bool>) (head => head != null)).ToArray<LinkedListNode<TBase>>();

  public virtual LinkedListNode<TBase> InsertItem(TBase item)
  {
    LinkedListNode<TBase> linkedListNode = this.GetList(item.GetType(), item.subtypeGroupIdentifier).Insert(item);
    this._itemToNodeMap[item] = linkedListNode;
    return linkedListNode;
  }

  public virtual void RemoveItem(TBase item)
  {
    ISortedList<TBase> list = this.GetList(item.GetType(), item.subtypeGroupIdentifier);
    LinkedListNode<TBase> node;
    if (!this._itemToNodeMap.TryGetValue(item, out node))
      return;
    list.Remove(node);
  }

  public virtual int GetCount<T>(int typeIdentifier) where T : TBase
  {
    ISortedList<TBase> sortedList;
    return this._items.TryGetValue((typeof (T), typeIdentifier), out sortedList) ? sortedList.items.Count : 0;
  }

  public virtual IEnumerable<T> GetItems<T>(int typeIdentifier) where T : TBase
  {
    ISortedList<TBase> sortedList;
    return this._items.TryGetValue((typeof (T), typeIdentifier), out sortedList) ? sortedList.items.Cast<T>() : (IEnumerable<T>) Array.Empty<T>();
  }

  public virtual ISortedList<TBase> GetList(System.Type type, int typeIdentifier)
  {
    (System.Type, int) key = (type, typeIdentifier);
    ISortedList<TBase> list;
    if (this._items.TryGetValue(key, out list))
      return list;
    this._items[key] = (ISortedList<TBase>) new SortedList<TBase>(this._sortedListsDataProcessors[type]);
    return this._items[key];
  }
}
