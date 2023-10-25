// Decompiled with JetBrains decompiler
// Type: QueueSet`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;

public class QueueSet<T>
{
  protected readonly LinkedList<T> _linkedList = new LinkedList<T>();
  protected readonly HashSet<T> _set = new HashSet<T>();

  public int Count => this._set.Count;

  public virtual void Enqueue(T item)
  {
    if (this._set.Contains(item))
      return;
    this._linkedList.AddLast(item);
    this._set.Add(item);
  }

  public virtual T Dequeue()
  {
    LinkedListNode<T> first = this._linkedList.First;
    this._linkedList.RemoveFirst();
    this._set.Remove(first.Value);
    return first.Value;
  }

  public virtual void Clear()
  {
    this._linkedList.Clear();
    this._set.Clear();
  }
}
