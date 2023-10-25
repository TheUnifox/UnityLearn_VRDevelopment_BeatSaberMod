using System;

namespace Zenject
{
    // Token: 0x02000053 RID: 83
    [NoReflectionBaking]
    public class ConcreteIdBinderNonGeneric : ConcreteBinderNonGeneric
    {
        // Token: 0x0600022E RID: 558 RVA: 0x00007584 File Offset: 0x00005784
        public ConcreteIdBinderNonGeneric(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindContainer, bindInfo, bindStatement)
        {
        }

        // Token: 0x0600022F RID: 559 RVA: 0x00007590 File Offset: 0x00005790
        public ConcreteBinderNonGeneric WithId(object identifier)
        {
            base.BindInfo.Identifier = identifier;
            return this;
        }
    }
}
