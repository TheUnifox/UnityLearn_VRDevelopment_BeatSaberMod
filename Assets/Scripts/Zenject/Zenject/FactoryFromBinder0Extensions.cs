using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200007E RID: 126
    public static class FactoryFromBinder0Extensions
    {
        // Token: 0x06000333 RID: 819 RVA: 0x0000935C File Offset: 0x0000755C
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TContract, TMemoryPool>(this FactoryFromBinder<TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<IMemoryPool> where TMemoryPool : MemoryPool<IMemoryPool, TContract>
        {
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x06000334 RID: 820 RVA: 0x000093C4 File Offset: 0x000075C4
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TContract>(this FactoryFromBinder<TContract> fromBinder) where TContract : IPoolable<IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000335 RID: 821 RVA: 0x000093EC File Offset: 0x000075EC
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TContract>(this FactoryFromBinder<TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000336 RID: 822 RVA: 0x000093F8 File Offset: 0x000075F8
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TContract>(this FactoryFromBinder<TContract> fromBinder) where TContract : Component, IPoolable<IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000337 RID: 823 RVA: 0x00009420 File Offset: 0x00007620
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TContract>(this FactoryFromBinder<TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000338 RID: 824 RVA: 0x0000942C File Offset: 0x0000762C
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TContract, TMemoryPool>(this FactoryFromBinder<TContract> fromBinder) where TContract : IPoolable<IMemoryPool> where TMemoryPool : MemoryPool<IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000339 RID: 825 RVA: 0x00009454 File Offset: 0x00007654
        public static ArgConditionCopyNonLazyBinder FromIFactory<TContract>(this FactoryFromBinder<TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
