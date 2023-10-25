using System;

namespace Zenject
{
    // Token: 0x0200014F RID: 335
    [NoReflectionBaking]
    public class ScopeConcreteIdArgConditionCopyNonLazyBinder : ConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x0600072D RID: 1837 RVA: 0x000130D4 File Offset: 0x000112D4
        public ScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x0600072E RID: 1838 RVA: 0x000130E0 File Offset: 0x000112E0
        public ConcreteIdArgConditionCopyNonLazyBinder AsCached()
        {
            base.BindInfo.Scope = ScopeTypes.Singleton;
            return this;
        }

        // Token: 0x0600072F RID: 1839 RVA: 0x000130F0 File Offset: 0x000112F0
        public ConcreteIdArgConditionCopyNonLazyBinder AsSingle()
        {
            base.BindInfo.Scope = ScopeTypes.Singleton;
            base.BindInfo.MarkAsUniqueSingleton = true;
            return this;
        }

        // Token: 0x06000730 RID: 1840 RVA: 0x0001310C File Offset: 0x0001130C
        public ConcreteIdArgConditionCopyNonLazyBinder AsTransient()
        {
            base.BindInfo.Scope = ScopeTypes.Transient;
            return this;
        }
    }
}
