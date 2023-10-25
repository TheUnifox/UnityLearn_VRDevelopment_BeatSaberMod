using System;

namespace Zenject
{
    // Token: 0x02000052 RID: 82
    [NoReflectionBaking]
    public class ConcreteIdBinderGeneric<TContract> : ConcreteBinderGeneric<TContract>
    {
        // Token: 0x0600022C RID: 556 RVA: 0x00007568 File Offset: 0x00005768
        public ConcreteIdBinderGeneric(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindContainer, bindInfo, bindStatement)
        {
        }

        // Token: 0x0600022D RID: 557 RVA: 0x00007574 File Offset: 0x00005774
        public ConcreteBinderGeneric<TContract> WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }
}
