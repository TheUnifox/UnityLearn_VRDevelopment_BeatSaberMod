using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x0200002D RID: 45
public abstract class PoolableSerializable : IPoolableSerializable, INetSerializable
{
    // Token: 0x060000E6 RID: 230 RVA: 0x00005264 File Offset: 0x00003464
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void NoDomainReloadInit()
    {
        PoolableSerializable._pools = null;
    }

    // Token: 0x060000E7 RID: 231 RVA: 0x0000526C File Offset: 0x0000346C
    public virtual void Retain()
    {
        this._referenceCount++;
    }

    // Token: 0x060000E8 RID: 232 RVA: 0x0000527C File Offset: 0x0000347C
    public virtual void Release()
    {
        this._referenceCount--;
        if (this._referenceCount == 0)
        {
            PoolableSerializable.Release(this);
        }
    }

    // Token: 0x060000E9 RID: 233
    public abstract void Serialize(NetDataWriter writer);

    // Token: 0x060000EA RID: 234
    public abstract void Deserialize(NetDataReader reader);

    // Token: 0x060000EB RID: 235 RVA: 0x0000529C File Offset: 0x0000349C
    public static T Obtain<T>()
    {
        if (PoolableSerializable._pools == null)
        {
            PoolableSerializable._pools = new Dictionary<Type, PoolableSerializable.Pool>();
        }
        PoolableSerializable.Pool pool;
        if (!PoolableSerializable._pools.TryGetValue(typeof(T), out pool))
        {
            if (!typeof(IPoolableSerializable).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidTypeException(string.Format("Type {0} must implement IPoolableSerializable", typeof(T)));
            }
            pool = new PoolableSerializable.Pool();
            PoolableSerializable._pools[typeof(T)] = pool;
        }
        object obj;
        if (pool.count <= 0)
        {
            obj = Activator.CreateInstance<T>();
        }
        else
        {
            IPoolableSerializable[] pool2 = pool.pool;
            PoolableSerializable.Pool pool3 = pool;
            int num = pool3.count - 1;
            pool3.count = num;
            obj = (T)((object)pool2[num]);
        }
        object obj2 = obj;
        ((IPoolableSerializable)((object)obj2)).Retain();
        return (T)obj2;
    }

    // Token: 0x060000EC RID: 236 RVA: 0x00005360 File Offset: 0x00003560
    private static void Release(IPoolableSerializable t)
    {
        if (PoolableSerializable._pools == null)
        {
            PoolableSerializable._pools = new Dictionary<Type, PoolableSerializable.Pool>();
        }
        PoolableSerializable.Pool pool;
        if (!PoolableSerializable._pools.TryGetValue(t.GetType(), out pool))
        {
            pool = new PoolableSerializable.Pool();
            PoolableSerializable._pools[t.GetType()] = pool;
        }
        if (pool.count < 32)
        {
            IPoolableSerializable[] pool2 = pool.pool;
            PoolableSerializable.Pool pool3 = pool;
            int count = pool3.count;
            pool3.count = count + 1;
            pool2[count] = t;
        }
    }

    // Token: 0x040000DC RID: 220
    private const int kPoolSize = 32;

    // Token: 0x040000DD RID: 221
    [ThreadStatic]
    private static Dictionary<Type, PoolableSerializable.Pool> _pools;

    // Token: 0x040000DE RID: 222
    private int _referenceCount;

    // Token: 0x0200002E RID: 46
    private class Pool
    {
        // Token: 0x040000DF RID: 223
        public readonly IPoolableSerializable[] pool = new IPoolableSerializable[32];

        // Token: 0x040000E0 RID: 224
        public int count;
    }
}
