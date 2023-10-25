using System;
using System.Collections.Generic;
using Zenject;

// Token: 0x02000006 RID: 6
public class MemoryPoolContainer<T>
{
    // Token: 0x17000003 RID: 3
    // (get) Token: 0x0600000C RID: 12 RVA: 0x00002184 File Offset: 0x00000384
    public List<T> activeItems
    {
        get
        {
            return this._activeItems.items;
        }
    }

    // Token: 0x0600000D RID: 13 RVA: 0x00002194 File Offset: 0x00000394
    public MemoryPoolContainer(Zenject.IMemoryPool<T> memoryPool)
    {
        this._memoryPool = memoryPool;
    }

    // Token: 0x0600000E RID: 14 RVA: 0x000021B0 File Offset: 0x000003B0
    public T Spawn()
    {
        T t = this._memoryPool.Spawn();
        this._activeItems.Add(t);
        return t;
    }

    // Token: 0x0600000F RID: 15 RVA: 0x000021D8 File Offset: 0x000003D8
    public void Despawn(T item)
    {
        this._activeItems.Remove(item);
        this._memoryPool.Despawn(item);
    }

    // Token: 0x04000006 RID: 6
    private readonly global::LazyCopyHashSet<T> _activeItems = new global::LazyCopyHashSet<T>();

    // Token: 0x04000007 RID: 7
    private readonly Zenject.IMemoryPool<T> _memoryPool;
}

public class MemoryPoolContainer<T0, T1> where T0 : T1
{
    // Token: 0x17000004 RID: 4
    // (get) Token: 0x06000010 RID: 16 RVA: 0x000021F4 File Offset: 0x000003F4
    public List<T1> activeItems
    {
        get
        {
            return this._activeItems.items;
        }
    }

    // Token: 0x06000011 RID: 17 RVA: 0x00002204 File Offset: 0x00000404
    public MemoryPoolContainer(Zenject.IMemoryPool<T0> memoryPool)
    {
        this._memoryPool = memoryPool;
    }

    // Token: 0x06000012 RID: 18 RVA: 0x00002220 File Offset: 0x00000420
    public T0 Spawn()
    {
        T0 t = this._memoryPool.Spawn();
        this._activeItems.Add((T1)((object)t));
        return t;
    }

    // Token: 0x06000013 RID: 19 RVA: 0x00002250 File Offset: 0x00000450
    public void Despawn(T0 item)
    {
        this._activeItems.Remove((T1)((object)item));
        this._memoryPool.Despawn(item);
    }

    // Token: 0x04000008 RID: 8
    private readonly global::LazyCopyHashSet<T1> _activeItems = new global::LazyCopyHashSet<T1>();

    // Token: 0x04000009 RID: 9
    private readonly Zenject.IMemoryPool<T0> _memoryPool;
}
