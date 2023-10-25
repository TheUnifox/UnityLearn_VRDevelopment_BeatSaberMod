using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000A6 RID: 166
    public static class FactoryFromBinder4Extensions
    {
        // Token: 0x060003F6 RID: 1014 RVA: 0x0000AB44 File Offset: 0x00008D44
        public static ArgConditionCopyNonLazyBinder FromIFactory<TParam1, TParam2, TParam3, TParam4, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TParam1, TParam2, TParam3, TParam4, TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x060003F7 RID: 1015 RVA: 0x0000AB88 File Offset: 0x00008D88
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003F8 RID: 1016 RVA: 0x0000ABB0 File Offset: 0x00008DB0
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x060003F9 RID: 1017 RVA: 0x0000ABBC File Offset: 0x00008DBC
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003FA RID: 1018 RVA: 0x0000ABE4 File Offset: 0x00008DE4
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x060003FB RID: 1019 RVA: 0x0000ABF0 File Offset: 0x00008DF0
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003FC RID: 1020 RVA: 0x0000AC18 File Offset: 0x00008E18
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, IMemoryPool, TContract>
        {
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
