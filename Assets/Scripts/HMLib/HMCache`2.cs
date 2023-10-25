// Decompiled with JetBrains decompiler
// Type: HMCache`2
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;

public class HMCache<K, V>
{
  protected Dictionary<K, V> _cache;
  protected Queue<K> _addedElements;
  protected int _maxNumberElements;

  public event System.Action<V> itemWillBeRemovedFromCacheEvent;

  public HMCache(int maxNumberElements)
  {
    this._maxNumberElements = maxNumberElements;
    this._cache = new Dictionary<K, V>(this._maxNumberElements);
    this._addedElements = new Queue<K>(this._maxNumberElements);
  }

  public virtual bool IsInCache(K key) => this._cache.ContainsKey(key);

  public virtual void UpdateOrderInCache(K key)
  {
    int count = this._addedElements.Count;
    for (int index = 0; index < count; ++index)
    {
      K k = this._addedElements.Dequeue();
      if (!k.Equals((object) key))
        this._addedElements.Enqueue(k);
    }
    this._addedElements.Enqueue(key);
  }

  public virtual V GetFromCache(K key) => this._cache.ContainsKey(key) ? this._cache[key] : default (V);

  public virtual void PutToCache(K key, V value)
  {
    if (this._cache.Count >= this._maxNumberElements)
    {
      K key1 = this._addedElements.Dequeue();
      V v = this._cache[key1];
      System.Action<V> removedFromCacheEvent = this.itemWillBeRemovedFromCacheEvent;
      if (removedFromCacheEvent != null)
        removedFromCacheEvent(v);
      this._cache.Remove(key1);
    }
    this._cache[key] = value;
    this._addedElements.Enqueue(key);
  }

  public virtual void Clear()
  {
    foreach (K key in this._cache.Keys)
    {
      V v = this._cache[key];
      System.Action<V> removedFromCacheEvent = this.itemWillBeRemovedFromCacheEvent;
      if (removedFromCacheEvent != null)
        removedFromCacheEvent(v);
    }
    this._cache.Clear();
    this._addedElements.Clear();
  }
}
