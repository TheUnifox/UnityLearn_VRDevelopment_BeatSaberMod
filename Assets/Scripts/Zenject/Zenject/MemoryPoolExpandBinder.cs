using System;

namespace Zenject
{
    // Token: 0x02000113 RID: 275
    [NoReflectionBaking]
    public class MemoryPoolExpandBinder<TContract> : FactoryArgumentsToChoiceBinder<TContract>
    {
        // Token: 0x060005E1 RID: 1505 RVA: 0x0000FD44 File Offset: 0x0000DF44
        public MemoryPoolExpandBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
            this.MemoryPoolBindInfo = poolBindInfo;
            this.ExpandByOneAtATime(true);
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x060005E2 RID: 1506 RVA: 0x0000FD60 File Offset: 0x0000DF60
        // (set) Token: 0x060005E3 RID: 1507 RVA: 0x0000FD68 File Offset: 0x0000DF68
        protected MemoryPoolBindInfo MemoryPoolBindInfo { get; private set; }

        // Token: 0x060005E4 RID: 1508 RVA: 0x0000FD74 File Offset: 0x0000DF74
        public FactoryArgumentsToChoiceBinder<TContract> ExpandByOneAtATime(bool showExpandWarning = true)
        {
            this.MemoryPoolBindInfo.ExpandMethod = PoolExpandMethods.OneAtATime;
            this.MemoryPoolBindInfo.ShowExpandWarning = showExpandWarning;
            return this;
        }

        // Token: 0x060005E5 RID: 1509 RVA: 0x0000FD90 File Offset: 0x0000DF90
        public FactoryArgumentsToChoiceBinder<TContract> ExpandByDoubling(bool showExpandWarning = true)
        {
            this.MemoryPoolBindInfo.ExpandMethod = PoolExpandMethods.Double;
            this.MemoryPoolBindInfo.ShowExpandWarning = showExpandWarning;
            return this;
        }
    }
}
