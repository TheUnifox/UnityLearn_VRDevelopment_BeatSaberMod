using System;

namespace Zenject
{
    // Token: 0x02000115 RID: 277
    [NoReflectionBaking]
    public class MemoryPoolInitialSizeMaxSizeBinder<TContract> : MemoryPoolMaxSizeBinder<TContract>
    {
        // Token: 0x060005E8 RID: 1512 RVA: 0x0000FDCC File Offset: 0x0000DFCC
        public MemoryPoolInitialSizeMaxSizeBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo) : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        // Token: 0x060005E9 RID: 1513 RVA: 0x0000FDDC File Offset: 0x0000DFDC
        public MemoryPoolMaxSizeBinder<TContract> WithInitialSize(int size)
        {
            base.MemoryPoolBindInfo.InitialSize = size;
            return this;
        }

        // Token: 0x060005EA RID: 1514 RVA: 0x0000FDEC File Offset: 0x0000DFEC
        public FactoryArgumentsToChoiceBinder<TContract> WithFixedSize(int size)
        {
            base.MemoryPoolBindInfo.InitialSize = size;
            base.MemoryPoolBindInfo.MaxSize = size;
            base.MemoryPoolBindInfo.ExpandMethod = PoolExpandMethods.Disabled;
            return this;
        }
    }
}
