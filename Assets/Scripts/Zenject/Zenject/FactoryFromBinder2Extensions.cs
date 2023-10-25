using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000094 RID: 148
    public static class FactoryFromBinder2Extensions
    {
        // Token: 0x0600039E RID: 926 RVA: 0x0000A084 File Offset: 0x00008284
        public static ArgConditionCopyNonLazyBinder FromIFactory<TParam1, TParam2, TContract>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TParam1, TParam2, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TParam1, TParam2, TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TParam2, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x0600039F RID: 927 RVA: 0x0000A0C8 File Offset: 0x000082C8
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TContract>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003A0 RID: 928 RVA: 0x0000A0F0 File Offset: 0x000082F0
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TContract>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x060003A1 RID: 929 RVA: 0x0000A0FC File Offset: 0x000082FC
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TContract>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder) where TContract : Component, IPoolable<TParam1, TParam2, IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003A2 RID: 930 RVA: 0x0000A124 File Offset: 0x00008324
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TContract>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<TParam1, TParam2, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x060003A3 RID: 931 RVA: 0x0000A130 File Offset: 0x00008330
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003A4 RID: 932 RVA: 0x0000A158 File Offset: 0x00008358
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, IMemoryPool, TContract>
        {
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TParam1, TParam2, TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
