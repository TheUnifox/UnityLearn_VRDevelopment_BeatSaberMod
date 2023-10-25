using System;
using System.Buffers;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001D0 RID: 464
    public class PoolWrapperFactory<T> : IFactory<T>, IFactory where T : IDisposable
    {
        // Token: 0x060009B7 RID: 2487 RVA: 0x0001A0A8 File Offset: 0x000182A8
        public PoolWrapperFactory(IMemoryPool<T> pool)
        {
            this._pool = pool;
        }

        // Token: 0x060009B8 RID: 2488 RVA: 0x0001A0B8 File Offset: 0x000182B8
        public T Create()
        {
            return this._pool.Spawn();
        }

        // Token: 0x060009B9 RID: 2489 RVA: 0x0001A0C8 File Offset: 0x000182C8
        private static object __zenCreate(object[] P_0)
        {
            return new PoolWrapperFactory<T>((IMemoryPool<T>)P_0[0]);
        }

        // Token: 0x060009BA RID: 2490 RVA: 0x0001A0EC File Offset: 0x000182EC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolWrapperFactory<T>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolWrapperFactory<T>.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "pool", typeof(IMemoryPool<T>), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F7 RID: 759
        private readonly IMemoryPool<T> _pool;
    }

    public class PoolWrapperFactory<TParam1, TValue> : IFactory<TParam1, TValue>, IFactory where TValue : IDisposable
    {
        // Token: 0x060009BB RID: 2491 RVA: 0x0001A160 File Offset: 0x00018360
        public PoolWrapperFactory(IMemoryPool<TParam1, TValue> pool)
        {
            this._pool = pool;
        }

        // Token: 0x060009BC RID: 2492 RVA: 0x0001A170 File Offset: 0x00018370
        public TValue Create(TParam1 arg)
        {
            return this._pool.Spawn(arg);
        }

        // Token: 0x060009BD RID: 2493 RVA: 0x0001A180 File Offset: 0x00018380
        private static object __zenCreate(object[] P_0)
        {
            return new PoolWrapperFactory<TParam1, TValue>((IMemoryPool<TParam1, TValue>)P_0[0]);
        }

        // Token: 0x060009BE RID: 2494 RVA: 0x0001A1A4 File Offset: 0x000183A4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolWrapperFactory<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolWrapperFactory<TParam1, TValue>.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "pool", typeof(IMemoryPool<TParam1, TValue>), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F8 RID: 760
        private readonly IMemoryPool<TParam1, TValue> _pool;
    }
}
