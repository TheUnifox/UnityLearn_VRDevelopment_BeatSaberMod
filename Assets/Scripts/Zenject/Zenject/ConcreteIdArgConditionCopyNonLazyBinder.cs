using System;

namespace Zenject
{
    // Token: 0x02000054 RID: 84
    [NoReflectionBaking]
    public class ConcreteIdArgConditionCopyNonLazyBinder : ArgConditionCopyNonLazyBinder
    {
        // Token: 0x06000230 RID: 560 RVA: 0x000075A0 File Offset: 0x000057A0
        public ConcreteIdArgConditionCopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x06000231 RID: 561 RVA: 0x000075AC File Offset: 0x000057AC
        public ArgConditionCopyNonLazyBinder WithConcreteId(object id)
        {
            base.BindInfo.ConcreteIdentifier = id;
            return this;
        }
    }
}
