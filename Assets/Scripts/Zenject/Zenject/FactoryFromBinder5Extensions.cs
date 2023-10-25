using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000AF RID: 175
    public static class FactoryFromBinder5Extensions
    {
        // Token: 0x06000422 RID: 1058 RVA: 0x0000B0A4 File Offset: 0x000092A4
        public static ArgConditionCopyNonLazyBinder FromIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x06000423 RID: 1059 RVA: 0x0000B0E8 File Offset: 0x000092E8
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000424 RID: 1060 RVA: 0x0000B110 File Offset: 0x00009310
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000425 RID: 1061 RVA: 0x0000B11C File Offset: 0x0000931C
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000426 RID: 1062 RVA: 0x0000B144 File Offset: 0x00009344
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000427 RID: 1063 RVA: 0x0000B150 File Offset: 0x00009350
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000428 RID: 1064 RVA: 0x0000B178 File Offset: 0x00009378
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, IMemoryPool, TContract>
        {
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
