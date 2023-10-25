using System;

namespace Zenject
{
    // Token: 0x0200014A RID: 330
    [NoReflectionBaking]
    public class IdScopeConcreteIdArgConditionCopyNonLazyBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x0600071D RID: 1821 RVA: 0x00012F58 File Offset: 0x00011158
        public IdScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x0600071E RID: 1822 RVA: 0x00012F64 File Offset: 0x00011164
        public ScopeConcreteIdArgConditionCopyNonLazyBinder WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }
}
