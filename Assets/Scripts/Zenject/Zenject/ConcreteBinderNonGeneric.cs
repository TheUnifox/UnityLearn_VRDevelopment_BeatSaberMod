using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000051 RID: 81
    [NoReflectionBaking]
    public class ConcreteBinderNonGeneric : FromBinderNonGeneric
    {
        // Token: 0x06000225 RID: 549 RVA: 0x000073B4 File Offset: 0x000055B4
        public ConcreteBinderNonGeneric(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindContainer, bindInfo, bindStatement)
        {
            this.ToSelf();
        }

        // Token: 0x06000226 RID: 550 RVA: 0x000073C8 File Offset: 0x000055C8
        public FromBinderNonGeneric ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            base.BindInfo.RequireExplicitScope = true;
            base.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new TransientProvider(type, container, base.BindInfo.Arguments, base.BindInfo.ContextInfo, base.BindInfo.ConcreteIdentifier, base.BindInfo.InstantiatedCallback));
            return this;
        }

        // Token: 0x06000227 RID: 551 RVA: 0x0000741C File Offset: 0x0000561C
        public FromBinderNonGeneric To<TConcrete>()
        {
            return this.To(new Type[]
            {
                typeof(TConcrete)
            });
        }

        // Token: 0x06000228 RID: 552 RVA: 0x00007438 File Offset: 0x00005638
        public FromBinderNonGeneric To(params Type[] concreteTypes)
        {
            return this.To(concreteTypes);
        }

        // Token: 0x06000229 RID: 553 RVA: 0x00007444 File Offset: 0x00005644
        public FromBinderNonGeneric To(IEnumerable<Type> concreteTypes)
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.AddRange(concreteTypes);
            if (base.BindInfo.ToTypes.Count > 1 && base.BindInfo.ContractTypes.Count > 1)
            {
                base.BindInfo.InvalidBindResponse = InvalidBindResponses.Skip;
            }
            else
            {
                BindingUtil.AssertIsDerivedFromTypes(concreteTypes, base.BindInfo.ContractTypes, base.BindInfo.InvalidBindResponse);
            }
            return this;
        }

        // Token: 0x0600022A RID: 554 RVA: 0x000074D0 File Offset: 0x000056D0
        public FromBinderNonGeneric To(Action<ConventionSelectTypesBinder> generator)
        {
            ConventionBindInfo conventionBindInfo = new ConventionBindInfo();
            base.BindInfo.InvalidBindResponse = InvalidBindResponses.Skip;
            generator(new ConventionSelectTypesBinder(conventionBindInfo));
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.AddRange(conventionBindInfo.ResolveTypes());
            return this;
        }
    }
}
