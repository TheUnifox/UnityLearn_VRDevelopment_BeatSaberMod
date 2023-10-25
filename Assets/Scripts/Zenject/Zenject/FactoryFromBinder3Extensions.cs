using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200009D RID: 157
    public static class FactoryFromBinder3Extensions
    {
        // Token: 0x060003CA RID: 970 RVA: 0x0000A5E4 File Offset: 0x000087E4
        public static ArgConditionCopyNonLazyBinder FromIFactory<TParam1, TParam2, TParam3, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TParam1, TParam2, TParam3, TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TParam2, TParam3, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x060003CB RID: 971 RVA: 0x0000A628 File Offset: 0x00008828
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003CC RID: 972 RVA: 0x0000A650 File Offset: 0x00008850
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x060003CD RID: 973 RVA: 0x0000A65C File Offset: 0x0000885C
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003CE RID: 974 RVA: 0x0000A684 File Offset: 0x00008884
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x060003CF RID: 975 RVA: 0x0000A690 File Offset: 0x00008890
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x060003D0 RID: 976 RVA: 0x0000A6B8 File Offset: 0x000088B8
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, IMemoryPool, TContract>
        {
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
