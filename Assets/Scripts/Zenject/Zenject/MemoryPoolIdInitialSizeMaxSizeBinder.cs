using System;

namespace Zenject
{
    // Token: 0x02000116 RID: 278
    [NoReflectionBaking]
    public class MemoryPoolIdInitialSizeMaxSizeBinder<TContract> : MemoryPoolInitialSizeMaxSizeBinder<TContract>
    {
        // Token: 0x060005EB RID: 1515 RVA: 0x0000FE14 File Offset: 0x0000E014
        public MemoryPoolIdInitialSizeMaxSizeBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo) : base(bindContainer, bindInfo, factoryBindInfo, poolBindInfo)
        {
        }

        // Token: 0x060005EC RID: 1516 RVA: 0x0000FE24 File Offset: 0x0000E024
        public MemoryPoolInitialSizeMaxSizeBinder<TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }
}
