using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000055 RID: 85
    [NoReflectionBaking]
    public class ConditionCopyNonLazyBinder : CopyNonLazyBinder
    {
        // Token: 0x06000232 RID: 562 RVA: 0x000075BC File Offset: 0x000057BC
        public ConditionCopyNonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x06000233 RID: 563 RVA: 0x000075C8 File Offset: 0x000057C8
        public CopyNonLazyBinder When(BindingCondition condition)
        {
            base.BindInfo.Condition = condition;
            return this;
        }

        // Token: 0x06000234 RID: 564 RVA: 0x000075D8 File Offset: 0x000057D8
        public CopyNonLazyBinder WhenInjectedIntoInstance(object instance)
        {
            return this.When((InjectContext r) => r.ObjectInstance == instance);
        }

        // Token: 0x06000235 RID: 565 RVA: 0x00007604 File Offset: 0x00005804
        public CopyNonLazyBinder WhenInjectedInto(params Type[] targets)
        {
            return this.When((InjectContext r) => (from x in targets
                                                   where r.ObjectType != null && r.ObjectType.DerivesFromOrEqual(x)
                                                   select x).Any<Type>());
        }

        // Token: 0x06000236 RID: 566 RVA: 0x00007630 File Offset: 0x00005830
        public CopyNonLazyBinder WhenInjectedInto<T>()
        {
            return this.When((InjectContext r) => r.ObjectType != null && r.ObjectType.DerivesFromOrEqual(typeof(T)));
        }

        // Token: 0x06000237 RID: 567 RVA: 0x00007658 File Offset: 0x00005858
        public CopyNonLazyBinder WhenNotInjectedInto<T>()
        {
            return this.When((InjectContext r) => r.ObjectType == null || !r.ObjectType.DerivesFromOrEqual(typeof(T)));
        }
    }
}
