// Decompiled with JetBrains decompiler
// Type: AsyncCache`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class AsyncCache<TKey, TValue>
{
  protected readonly Func<TKey, Task<TValue>> _valueFactory;
  protected readonly ConcurrentDictionary<TKey, Lazy<Task<TValue>>> _map;

  public AsyncCache(Func<TKey, Task<TValue>> valueFactory)
  {
    this._valueFactory = valueFactory != null ? valueFactory : throw new ArgumentNullException("loader");
    this._map = new ConcurrentDictionary<TKey, Lazy<Task<TValue>>>();
  }

  public Task<TValue> this[TKey key]
  {
    get
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      return this._map.GetOrAdd(key, (Func<TKey, Lazy<Task<TValue>>>) (toAdd => new Lazy<Task<TValue>>((Func<Task<TValue>>) (() => this._valueFactory(toAdd))))).Value;
    }
  }

  public virtual void RemoveKey(TKey key)
  {
    if (!this._map.ContainsKey(key))
      return;
    this._map.TryRemove(key, out Lazy<Task<TValue>> _);
  }

  [CompilerGenerated]
  public virtual Lazy<Task<TValue>> m_Cget_Itemm_Eb__4_0(TKey toAdd) => new Lazy<Task<TValue>>((Func<Task<TValue>>) (() => this._valueFactory(toAdd)));
}
