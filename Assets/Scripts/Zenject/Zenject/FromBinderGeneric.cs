using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200013F RID: 319
    [NoReflectionBaking]
    public class FromBinderGeneric<TContract> : FromBinder
    {
        // Token: 0x060006DC RID: 1756 RVA: 0x00012938 File Offset: 0x00010B38
        public FromBinderGeneric(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindContainer, bindInfo, bindStatement)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TContract), base.BindInfo.ContractTypes);
        }

        // Token: 0x060006DD RID: 1757 RVA: 0x00012960 File Offset: 0x00010B60
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromFactory<TFactory>() where TFactory : IFactory<TContract>
        {
            return this.FromIFactory(delegate (ConcreteBinderGeneric<IFactory<TContract>> x)
            {
                x.To<TFactory>().AsCached();
            });
        }

        // Token: 0x060006DE RID: 1758 RVA: 0x00012988 File Offset: 0x00010B88
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactory(Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            return base.FromIFactoryBase<TContract>(factoryBindGenerator);
        }

        // Token: 0x060006DF RID: 1759 RVA: 0x00012994 File Offset: 0x00010B94
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod(Func<TContract> method)
        {
            return base.FromMethodBase<TContract>((InjectContext ctx) => method());
        }

        // Token: 0x060006E0 RID: 1760 RVA: 0x000129C0 File Offset: 0x00010BC0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod(Func<InjectContext, TContract> method)
        {
            return base.FromMethodBase<TContract>(method);
        }

        // Token: 0x060006E1 RID: 1761 RVA: 0x000129CC File Offset: 0x00010BCC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultiple(Func<InjectContext, IEnumerable<TContract>> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TContract), base.AllParentTypes);
            return base.FromMethodMultipleBase<TContract>(method);
        }

        // Token: 0x060006E2 RID: 1762 RVA: 0x000129EC File Offset: 0x00010BEC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj>(Func<TObj, TContract> method)
        {
            return this.FromResolveGetter<TObj>(null, method);
        }

        // Token: 0x060006E3 RID: 1763 RVA: 0x000129F8 File Offset: 0x00010BF8
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj>(object identifier, Func<TObj, TContract> method)
        {
            return this.FromResolveGetter<TObj>(identifier, method, InjectSources.Any);
        }

        // Token: 0x060006E4 RID: 1764 RVA: 0x00012A04 File Offset: 0x00010C04
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return base.FromResolveGetterBase<TObj, TContract>(identifier, method, source, false);
        }

        // Token: 0x060006E5 RID: 1765 RVA: 0x00012A10 File Offset: 0x00010C10
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj>(Func<TObj, TContract> method)
        {
            return this.FromResolveAllGetter<TObj>(null, method);
        }

        // Token: 0x060006E6 RID: 1766 RVA: 0x00012A1C File Offset: 0x00010C1C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj>(object identifier, Func<TObj, TContract> method)
        {
            return this.FromResolveAllGetter<TObj>(identifier, method, InjectSources.Any);
        }

        // Token: 0x060006E7 RID: 1767 RVA: 0x00012A28 File Offset: 0x00010C28
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return base.FromResolveGetterBase<TObj, TContract>(identifier, method, source, true);
        }

        // Token: 0x060006E8 RID: 1768 RVA: 0x00012A34 File Offset: 0x00010C34
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromInstance(TContract instance)
        {
            return base.FromInstanceBase(instance);
        }

        // Token: 0x060006E9 RID: 1769 RVA: 0x00012A44 File Offset: 0x00010C44
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(Func<TContract, bool> predicate, bool includeInactive = true)
        {
            return this.FromComponentsInChildren(false, predicate, includeInactive);
        }

        // Token: 0x060006EA RID: 1770 RVA: 0x00012A50 File Offset: 0x00010C50
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(bool excludeSelf = false, Func<TContract, bool> predicate = null, bool includeInactive = true)
        {
            Func<Component, bool> predicate2;
            if (predicate != null)
            {
                predicate2 = ((Component component) => predicate((TContract)((object)component)));
            }
            else
            {
                predicate2 = null;
            }
            return base.FromComponentsInChildrenBase(excludeSelf, predicate2, includeInactive);
        }

        // Token: 0x060006EB RID: 1771 RVA: 0x00012A8C File Offset: 0x00010C8C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInHierarchy(Func<TContract, bool> predicate = null, bool includeInactive = true)
        {
            Func<Component, bool> predicate2;
            if (predicate != null)
            {
                predicate2 = ((Component component) => predicate((TContract)((object)component)));
            }
            else
            {
                predicate2 = null;
            }
            return base.FromComponentsInHierarchyBase(predicate2, includeInactive);
        }
    }
}
