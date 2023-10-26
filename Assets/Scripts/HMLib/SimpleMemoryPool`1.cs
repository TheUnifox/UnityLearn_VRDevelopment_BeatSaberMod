// Decompiled with JetBrains decompiler
// Type: SimpleMemoryPool`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections.Generic;

public class SimpleMemoryPool<T> where T : class
{
  protected readonly LazyCopyHashSet<T> _activeElements;
  protected readonly List<T> _inactiveElements;
  protected readonly Func<T> _createNewItemFunc;

  public List<T> items => this._activeElements.items;

  public SimpleMemoryPool(int startCapacity, Func<T> createNewItemFunc)
  {
    this._createNewItemFunc = createNewItemFunc;
    this._activeElements = new LazyCopyHashSet<T>(startCapacity);
    this._inactiveElements = new List<T>(startCapacity);
    for (int index = 0; index < startCapacity; ++index)
      this._inactiveElements.Add(this._createNewItemFunc());
  }

  public virtual T Spawn()
  {
    T obj2;
    if (this._inactiveElements.Count > 0)
    {
      obj2 = this._inactiveElements[0];
      this._inactiveElements.RemoveAt(0);
    }
    else
      obj2 = this._createNewItemFunc();
    this._activeElements.Add(obj2);
    return obj2;
  }

  public virtual void Despawn(T item)
  {
    this._activeElements.Remove(item);
    this._inactiveElements.Add(item);
  }
}
