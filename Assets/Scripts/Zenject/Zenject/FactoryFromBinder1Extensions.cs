using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000087 RID: 135
    public static class FactoryFromBinder1Extensions
    {
        // Token: 0x0600035F RID: 863 RVA: 0x000098BC File Offset: 0x00007ABC
        public static ArgConditionCopyNonLazyBinder FromIFactory<TParam1, TContract>(this FactoryFromBinder<TParam1, TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TParam1, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TParam1, TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x06000360 RID: 864 RVA: 0x00009900 File Offset: 0x00007B00
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TContract>(this FactoryFromBinder<TParam1, TContract> fromBinder) where TContract : IPoolable<TParam1, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000361 RID: 865 RVA: 0x00009928 File Offset: 0x00007B28
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TContract>(this FactoryFromBinder<TParam1, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000362 RID: 866 RVA: 0x00009934 File Offset: 0x00007B34
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TContract>(this FactoryFromBinder<TParam1, TContract> fromBinder) where TContract : Component, IPoolable<TParam1, IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000363 RID: 867 RVA: 0x0000995C File Offset: 0x00007B5C
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TContract>(this FactoryFromBinder<TParam1, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<TParam1, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000364 RID: 868 RVA: 0x00009968 File Offset: 0x00007B68
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TContract> fromBinder) where TContract : IPoolable<TParam1, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000365 RID: 869 RVA: 0x00009990 File Offset: 0x00007B90
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, IMemoryPool, TContract>
        {
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TParam1, TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
