// Decompiled with JetBrains decompiler
// Type: ReferenceCountingCache`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Diagnostics;

public class ReferenceCountingCache<TKey, TValue> : IReferenceCountingCache<TKey, TValue>
{
  protected readonly Dictionary<TKey, TValue> _items = new Dictionary<TKey, TValue>();
  protected readonly Dictionary<TKey, int> _referencesCount = new Dictionary<TKey, int>();

  public virtual int Insert(TKey key, TValue item)
  {
    int num1;
    if (this._referencesCount.TryGetValue(key, out num1) && num1 > 0)
    {
      int num2 = num1 + 1;
      this._referencesCount[key] = num2;
      return num2;
    }
    this._items[key] = item;
    this._referencesCount[key] = 1;
    return 1;
  }

  public virtual int AddReference(TKey key)
  {
    int num1;
    if (!this._referencesCount.TryGetValue(key, out num1) || num1 <= 0)
      return 0;
    int num2 = num1 + 1;
    this._referencesCount[key] = num2;
    return num2;
  }

  public virtual int RemoveReference(TKey key)
  {
    int num1;
    if (!this._referencesCount.TryGetValue(key, out num1) || num1 <= 0)
      return 0;
    int num2 = num1 - 1;
    if (num2 > 0)
    {
      this._referencesCount[key] = num2;
    }
    else
    {
      this._referencesCount.Remove(key);
      this._items.Remove(key);
    }
    return num2;
  }

  public virtual int GetReferenceCount(TKey key)
  {
    int num;
    return this._referencesCount.TryGetValue(key, out num) ? num : 0;
  }

  public virtual bool TryGet(TKey key, out TValue result) => this._items.TryGetValue(key, out result);

  [Conditional("REFERENCE_COUNTING_CACHE_LOG_ENABLED")]
  private static void LogError(string message) => UnityEngine.Debug.LogError((object) message);
}
