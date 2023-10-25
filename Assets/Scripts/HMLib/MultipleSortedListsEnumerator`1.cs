// Decompiled with JetBrains decompiler
// Type: MultipleSortedListsEnumerator`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class MultipleSortedListsEnumerator<T> : IEnumerable where T : IComparable<T>
{
  protected readonly BinaryHeap<MultipleSortedListsEnumerator<T>.HeapItem> _heap;

  public MultipleSortedListsEnumerator(params IReadOnlyList<T>[] dataList)
  {
    this._heap = new BinaryHeap<MultipleSortedListsEnumerator<T>.HeapItem>(dataList.Length);
    foreach (IReadOnlyList<T> data in dataList)
    {
      if (data != null && data.Count > 0)
        this._heap.Insert(new MultipleSortedListsEnumerator<T>.HeapItem(data));
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public virtual IEnumerator<T> GetEnumerator()
  {
    MultipleSortedListsEnumerator<T>.HeapItem heapItem;
    while (this._heap.RemoveMin(out heapItem))
    {
      yield return heapItem.currentValue;
      if (heapItem.MoveToNextItem())
        this._heap.Insert(heapItem);
      heapItem = (MultipleSortedListsEnumerator<T>.HeapItem) null;
    }
  }

  public class HeapItem : IComparable<MultipleSortedListsEnumerator<T>.HeapItem>
  {
    protected readonly IReadOnlyList<T> _dataList;
    protected int _idx;
    protected T _currentValue;

    public T currentValue => this._currentValue;

    public HeapItem(IReadOnlyList<T> dataList)
    {
      this._dataList = dataList;
      this._currentValue = this._dataList[0];
    }

    public virtual bool MoveToNextItem()
    {
      ++this._idx;
      if (this._idx >= this._dataList.Count)
        return false;
      this._currentValue = this._dataList[this._idx];
      return true;
    }

    public virtual int CompareTo(MultipleSortedListsEnumerator<T>.HeapItem heapItem) => this._currentValue.CompareTo(heapItem.currentValue);
  }
}
