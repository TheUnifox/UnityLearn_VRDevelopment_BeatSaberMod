using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000B8 RID: 184
    public static class FactoryFromBinder6Extensions
    {
        // Token: 0x0600044E RID: 1102 RVA: 0x0000B604 File Offset: 0x00009804
        public static ArgConditionCopyNonLazyBinder FromIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder, Action<ConcreteBinderGeneric<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>>> factoryBindGenerator)
        {
            Guid factoryId;
            factoryBindGenerator(fromBinder.CreateIFactoryBinder<IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>>(out factoryId));
            fromBinder.ProviderFunc = ((DiContainer container) => new IFactoryProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(container, factoryId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }

        // Token: 0x0600044F RID: 1103 RVA: 0x0000B648 File Offset: 0x00009848
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000450 RID: 1104 RVA: 0x0000B670 File Offset: 0x00009870
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000451 RID: 1105 RVA: 0x0000B67C File Offset: 0x0000987C
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool>
        {
            return fromBinder.FromMonoPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000452 RID: 1106 RVA: 0x0000B6A4 File Offset: 0x000098A4
        public static ArgConditionCopyNonLazyBinder FromMonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool>
        {
            return fromBinder.FromPoolableMemoryPool(poolBindGenerator);
        }

        // Token: 0x06000453 RID: 1107 RVA: 0x0000B6B0 File Offset: 0x000098B0
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool, TContract>
        {
            return fromBinder.FromPoolableMemoryPool(delegate (MemoryPoolInitialSizeMaxSizeBinder<TContract> x)
            {
            });
        }

        // Token: 0x06000454 RID: 1108 RVA: 0x0000B6D8 File Offset: 0x000098D8
        public static ArgConditionCopyNonLazyBinder FromPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TMemoryPool>(this FactoryFromBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> fromBinder, Action<MemoryPoolInitialSizeMaxSizeBinder<TContract>> poolBindGenerator) where TContract : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool> where TMemoryPool : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, IMemoryPool, TContract>
        {
            ModestTree.Assert.IsEqual(typeof(TContract), typeof(TContract));
            Guid poolId = Guid.NewGuid();
            MemoryPoolInitialSizeMaxSizeBinder<TContract> memoryPoolInitialSizeMaxSizeBinder = fromBinder.BindContainer.BindMemoryPoolCustomInterfaceNoFlush<TContract, TMemoryPool, TMemoryPool>(false).WithId(poolId);
            memoryPoolInitialSizeMaxSizeBinder.NonLazy();
            poolBindGenerator(memoryPoolInitialSizeMaxSizeBinder);
            fromBinder.ProviderFunc = ((DiContainer container) => new PoolableMemoryPoolProvider<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TMemoryPool>(container, poolId));
            return new ArgConditionCopyNonLazyBinder(fromBinder.BindInfo);
        }
    }
}
