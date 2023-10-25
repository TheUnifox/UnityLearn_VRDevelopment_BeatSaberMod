using System;

namespace Zenject
{
    // Token: 0x02000114 RID: 276
    [NoReflectionBaking]
    public class MemoryPoolMaxSizeBinder<TContract> : MemoryPoolExpandBinder<TContract>
    {
        // Token: 0x060005E6 RID: 1510 RVA: 0x0000FDAC File Offset: 0x0000DFAC
        public MemoryPoolMaxSizeBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo) : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        // Token: 0x060005E7 RID: 1511 RVA: 0x0000FDBC File Offset: 0x0000DFBC
        public MemoryPoolExpandBinder<TContract> WithMaxSize(int size)
        {
            base.MemoryPoolBindInfo.MaxSize = size;
            return this;
        }
    }
}
