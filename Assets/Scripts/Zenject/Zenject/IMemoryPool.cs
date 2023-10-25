using System;

namespace Zenject
{
    // Token: 0x020001AA RID: 426
    public interface IMemoryPool
    {
        // Token: 0x17000078 RID: 120
        // (get) Token: 0x060008E5 RID: 2277
        int NumTotal { get; }

        // Token: 0x17000079 RID: 121
        // (get) Token: 0x060008E6 RID: 2278
        int NumActive { get; }

        // Token: 0x1700007A RID: 122
        // (get) Token: 0x060008E7 RID: 2279
        int NumInactive { get; }

        // Token: 0x1700007B RID: 123
        // (get) Token: 0x060008E8 RID: 2280
        Type ItemType { get; }

        // Token: 0x060008E9 RID: 2281
        void Resize(int desiredPoolSize);

        // Token: 0x060008EA RID: 2282
        void Clear();

        // Token: 0x060008EB RID: 2283
        void ExpandBy(int numToAdd);

        // Token: 0x060008EC RID: 2284
        void ShrinkBy(int numToRemove);

        // Token: 0x060008ED RID: 2285
        void Despawn(object obj);
    }

    public interface IMemoryPool<TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008EF RID: 2287
        TValue Spawn();
    }

    public interface IMemoryPool<in TParam1, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F0 RID: 2288
        TValue Spawn(TParam1 param);
    }

    public interface IMemoryPool<in TParam1, in TParam2, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F1 RID: 2289
        TValue Spawn(TParam1 param1, TParam2 param2);
    }

    public interface IMemoryPool<in TParam1, in TParam2, in TParam3, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F2 RID: 2290
        TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface IMemoryPool<in TParam1, in TParam2, in TParam3, in TParam4, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F3 RID: 2291
        TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }

    public interface IMemoryPool<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F4 RID: 2292
        TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
    }

    public interface IMemoryPool<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F5 RID: 2293
        TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6);
    }

    public interface IMemoryPool<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F6 RID: 2294
        TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7);
    }

    public interface IMemoryPool<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, TValue> : IDespawnableMemoryPool<TValue>, IMemoryPool
    {
        // Token: 0x060008F7 RID: 2295
        TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8);
    }
}
