using System;
using System.Collections.Generic;

// Token: 0x02000003 RID: 3
public class LazyCopyHashSet<T> : global::ILazyCopyHashSet<T>
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
    public List<T> items
    {
        get
        {
            if (this._dirty)
            {
                this._itemsCopy.Clear();
                foreach (T item in this._items)
                {
                    this._itemsCopy.Add(item);
                }
                this._dirty = false;
            }
            return this._itemsCopy;
        }
    }

    // Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
    public LazyCopyHashSet() : this(10)
    {
    }

    // Token: 0x06000005 RID: 5 RVA: 0x000020D4 File Offset: 0x000002D4
    public LazyCopyHashSet(int capacity)
    {
        this._itemsCopy = new List<T>(capacity);
        this._items = global::HashSetExtensions.GetHashSet<T>(capacity);
        this._dirty = false;
    }

    // Token: 0x06000006 RID: 6 RVA: 0x000020FC File Offset: 0x000002FC
    public void Add(T item)
    {
        this._dirty = true;
        this._items.Add(item);
    }

    // Token: 0x06000007 RID: 7 RVA: 0x00002114 File Offset: 0x00000314
    public void Remove(T item)
    {
        this._dirty = true;
        this._items.Remove(item);
    }

    // Token: 0x04000001 RID: 1
    private readonly List<T> _itemsCopy;

    // Token: 0x04000002 RID: 2
    private readonly HashSet<T> _items;

    // Token: 0x04000003 RID: 3
    private bool _dirty;
}
