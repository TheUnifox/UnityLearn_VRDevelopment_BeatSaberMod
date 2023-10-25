// Decompiled with JetBrains decompiler
// Type: BinaryHeap`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;

public class BinaryHeap<T> where T : IComparable<T>
{
  protected T[] _data;
  protected int _tail;

  public BinaryHeap()
    : this(10)
  {
  }

  public BinaryHeap(int capacity)
  {
    this._data = new T[capacity];
    this._tail = 0;
  }

  public virtual void Insert(T item)
  {
    ++this._tail;
    if (this._tail >= this._data.Length)
    {
      T[] destinationArray = new T[this._data.Length * 2];
      Array.Copy((Array) this._data, (Array) destinationArray, this._data.Length);
      this._data = destinationArray;
    }
    this._data[this._tail] = item;
    int index1;
    for (int index2 = this._tail; index2 > 1; index2 = index1)
    {
      index1 = index2 / 2;
      if (this._data[index1].CompareTo(this._data[index2]) <= 0)
        break;
      T[] data1 = this._data;
      int index3 = index1;
      T[] data2 = this._data;
      int num = index2;
      T obj1 = this._data[index2];
      T obj2 = this._data[index1];
      data1[index3] = obj1;
      int index4 = num;
      T obj3 = obj2;
      data2[index4] = obj3;
    }
  }

  public virtual bool RemoveMin(out T output)
  {
    if (this._tail < 1)
    {
      output = default (T);
      return false;
    }
    output = this._data[1];
    this._data[1] = this._data[this._tail];
    --this._tail;
    int index1 = 1;
    int index2;
    while (true)
    {
      index2 = index1 * 2;
      int index3 = index1 * 2 + 1;
      if (index2 <= this._tail)
      {
        if (index2 != this._tail)
        {
          int index4 = this._data[index2].CompareTo(this._data[index3]) < 0 ? index2 : index3;
          if (this._data[index4].CompareTo(this._data[index1]) < 0)
          {
            T[] data1 = this._data;
            int index5 = index1;
            T[] data2 = this._data;
            int num = index4;
            T obj1 = this._data[index4];
            T obj2 = this._data[index1];
            data1[index5] = obj1;
            int index6 = num;
            T obj3 = obj2;
            data2[index6] = obj3;
          }
          index1 = index4;
        }
        else
          goto label_6;
      }
      else
        break;
    }
    return true;
label_6:
    if (this._data[index2].CompareTo(this._data[index1]) < 0)
    {
      T[] data3 = this._data;
      int index7 = index1;
      T[] data4 = this._data;
      int num = index2;
      T obj4 = this._data[index2];
      T obj5 = this._data[index1];
      data3[index7] = obj4;
      int index8 = num;
      T obj6 = obj5;
      data4[index8] = obj6;
    }
    return true;
  }
}
