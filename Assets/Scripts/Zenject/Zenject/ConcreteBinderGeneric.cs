using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200004F RID: 79
    [NoReflectionBaking]
    public class ConcreteBinderGeneric<TContract> : FromBinderGeneric<TContract>
    {
        // Token: 0x06000219 RID: 537 RVA: 0x00007144 File Offset: 0x00005344
        public ConcreteBinderGeneric(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindContainer, bindInfo, bindStatement)
        {
            this.ToSelf();
        }

        // Token: 0x0600021A RID: 538 RVA: 0x00007158 File Offset: 0x00005358
        public FromBinderGeneric<TContract> ToSelf()
        {
            ModestTree.Assert.IsEqual(base.BindInfo.ToChoice, ToChoices.Self);
            base.BindInfo.RequireExplicitScope = true;
            base.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new TransientProvider(type, container, base.BindInfo.Arguments, base.BindInfo.ContextInfo, base.BindInfo.ConcreteIdentifier, base.BindInfo.InstantiatedCallback));
            return this;
        }

        // Token: 0x0600021B RID: 539 RVA: 0x000071AC File Offset: 0x000053AC
        public FromBinderGeneric<TConcrete> To<TConcrete>() where TConcrete : TContract
        {
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.Add(typeof(TConcrete));
            return new FromBinderGeneric<TConcrete>(base.BindContainer, base.BindInfo, base.BindStatement);
        }

        // Token: 0x0600021C RID: 540 RVA: 0x00007208 File Offset: 0x00005408
        public FromBinderNonGeneric To(params Type[] concreteTypes)
        {
            return this.To(concreteTypes);
        }

        // Token: 0x0600021D RID: 541 RVA: 0x00007214 File Offset: 0x00005414
        public FromBinderNonGeneric To(IEnumerable<Type> concreteTypes)
        {
            BindingUtil.AssertIsDerivedFromTypes(concreteTypes, base.BindInfo.ContractTypes, base.BindInfo.InvalidBindResponse);
            base.BindInfo.ToChoice = ToChoices.Concrete;
            base.BindInfo.ToTypes.Clear();
            base.BindInfo.ToTypes.AddRange(concreteTypes);
            return new FromBinderNonGeneric(base.BindContainer, base.BindInfo, base.BindStatement);
        }

        // Token: 0x0600021E RID: 542 RVA: 0x00007284 File Offset: 0x00005484
        public FromBinderNonGeneric To(Action<ConventionSelectTypesBinder> generator)
        {
            ConventionBindInfo conventionBindInfo = new ConventionBindInfo();
            conventionBindInfo.AddTypeFilter((Type concreteType) => base.BindInfo.ContractTypes.All((Type contractType) => concreteType.DerivesFromOrEqual(contractType)));
            generator(new ConventionSelectTypesBinder(conventionBindInfo));
            return this.To(conventionBindInfo.ResolveTypes());
        }
    }
}
