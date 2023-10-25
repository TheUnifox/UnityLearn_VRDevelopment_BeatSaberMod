// Decompiled with JetBrains decompiler
// Type: UndoRedoBuffer`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;

public class UndoRedoBuffer<T> where T : class
{
  protected List<T> _data;
  protected int _capacity;
  protected int _cursor;

  public UndoRedoBuffer(int capacity)
  {
    this._capacity = capacity;
    this._data = new List<T>(capacity + 1);
  }

  public virtual void Add(T item)
  {
    if (this._data.Count >= this._capacity)
      this._data.RemoveAt(this._data.Count - 1);
    if (this._cursor > 0)
    {
      for (int index = 0; index < this._cursor; ++index)
        this._data.RemoveAt(0);
    }
    this._data.Insert(0, item);
    this._cursor = 0;
  }

  public virtual T Undo()
  {
    if (this._cursor + 1 >= this._data.Count)
      return default (T);
    ++this._cursor;
    return this._data[this._cursor];
  }

  public virtual T Redo()
  {
    if (this._cursor == 0)
      return default (T);
    --this._cursor;
    return this._data[this._cursor];
  }

  public virtual void Clear()
  {
    this._data.Clear();
    this._cursor = 0;
  }
}
