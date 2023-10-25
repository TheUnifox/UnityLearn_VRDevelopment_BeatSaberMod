using System;

// Token: 0x02000048 RID: 72
public class PacketPool<T> : IPacketPool<T>, IPacketPool where T : IPoolablePacket, new()
{
    // Token: 0x060002E4 RID: 740 RVA: 0x00006CE8 File Offset: 0x00004EE8
    public T Obtain()
    {
        if (this._size > 0)
        {
            T[] pool = this._pool;
            int num = this._size - 1;
            this._size = num;
            return pool[num];
        }
        return Activator.CreateInstance<T>();
    }

    // Token: 0x060002E5 RID: 741 RVA: 0x00006D20 File Offset: 0x00004F20
    public void Release(T t)
    {
        if (this._size < 16)
        {
            T[] pool = this._pool;
            int size = this._size;
            this._size = size + 1;
            pool[size] = t;
        }
    }

    // Token: 0x060002E6 RID: 742 RVA: 0x00006D54 File Offset: 0x00004F54
    public void Fill()
    {
        while (this._size < 16)
        {
            T[] pool = this._pool;
            int size = this._size;
            this._size = size + 1;
            pool[size] = Activator.CreateInstance<T>();
        }
    }

    // Token: 0x060002E7 RID: 743 RVA: 0x00006D90 File Offset: 0x00004F90
    public void Clear()
    {
        for (int i = 0; i < this._size; i++)
        {
            this._pool[i] = default(T);
        }
        this._size = 0;
        this._clearCount++;
    }

    // Token: 0x060002E8 RID: 744 RVA: 0x00006DD8 File Offset: 0x00004FD8
    void IPacketPool.Release(object o)
    {
        if (o is T)
        {
            T t = (T)((object)o);
            this.Release(t);
        }
    }

    // Token: 0x060002E9 RID: 745 RVA: 0x00006DFD File Offset: 0x00004FFD
    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this._clearCount;
    }

    // Token: 0x040000FF RID: 255
    private int _clearCount;

    // Token: 0x04000100 RID: 256
    private const int kMaxPoolSize = 16;

    // Token: 0x04000101 RID: 257
    private int _size;

    // Token: 0x04000102 RID: 258
    private readonly T[] _pool = new T[16];
}
