// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapEditorObjectsCollection`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using System.Linq;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapEditorObjectsCollection<T>
  {
    private readonly HashSet<T> _items;

    public IReadOnlyList<T> items => (IReadOnlyList<T>) this._items.ToList<T>();

    public int count => this._items.Count;

    public BeatmapEditorObjectsCollection() => this._items = new HashSet<T>();

    public void Add(T item) => this._items.Add(item);

    public void AddRange(IEnumerable<T> items)
    {
      foreach (T obj in items)
        this._items.Add(obj);
    }

    public void Remove(T item) => this._items.Remove(item);

    public void RemoveRange(IEnumerable<T> items)
    {
      foreach (T obj in items)
        this._items.Remove(obj);
    }

    public bool Contains(T item) => this._items.Contains(item);

    public void CopyFrom(BeatmapEditorObjectsCollection<T> other)
    {
      this._items.Clear();
      foreach (T obj in (IEnumerable<T>) other.items)
        this._items.Add(obj);
    }

    public void Clear() => this._items.Clear();
  }
}
