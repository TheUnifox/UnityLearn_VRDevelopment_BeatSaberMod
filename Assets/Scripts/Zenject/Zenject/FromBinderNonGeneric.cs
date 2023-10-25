using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000144 RID: 324
    [NoReflectionBaking]
    public class FromBinderNonGeneric : FromBinder
    {
        // Token: 0x060006FD RID: 1789 RVA: 0x00012CDC File Offset: 0x00010EDC
        public FromBinderNonGeneric(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindContainer, bindInfo, bindStatement)
        {
        }

        // Token: 0x060006FE RID: 1790 RVA: 0x00012CE8 File Offset: 0x00010EE8
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromFactory<TConcrete, TFactory>() where TFactory : IFactory<TConcrete>
        {
            return this.FromIFactory<TConcrete>(delegate (ConcreteBinderGeneric<IFactory<TConcrete>> x)
            {
                x.To<TFactory>().AsCached();
            });
        }

        // Token: 0x060006FF RID: 1791 RVA: 0x00012D10 File Offset: 0x00010F10
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactory<TContract>(Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            return base.FromIFactoryBase<TContract>(factoryBindGenerator);
        }

        // Token: 0x06000700 RID: 1792 RVA: 0x00012D1C File Offset: 0x00010F1C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethod<TConcrete>(Func<InjectContext, TConcrete> method)
        {
            return base.FromMethodBase<TConcrete>(method);
        }

        // Token: 0x06000701 RID: 1793 RVA: 0x00012D28 File Offset: 0x00010F28
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultiple<TConcrete>(Func<InjectContext, IEnumerable<TConcrete>> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), base.AllParentTypes);
            return base.FromMethodMultipleBase<TConcrete>(method);
        }

        // Token: 0x06000702 RID: 1794 RVA: 0x00012D48 File Offset: 0x00010F48
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(Func<TObj, TContract> method)
        {
            return this.FromResolveGetter<TObj, TContract>(null, method);
        }

        // Token: 0x06000703 RID: 1795 RVA: 0x00012D54 File Offset: 0x00010F54
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method)
        {
            return this.FromResolveGetter<TObj, TContract>(identifier, method, InjectSources.Any);
        }

        // Token: 0x06000704 RID: 1796 RVA: 0x00012D60 File Offset: 0x00010F60
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return base.FromResolveGetterBase<TObj, TContract>(identifier, method, source, false);
        }

        // Token: 0x06000705 RID: 1797 RVA: 0x00012D6C File Offset: 0x00010F6C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(Func<TObj, TContract> method)
        {
            return this.FromResolveAllGetter<TObj, TContract>(null, method);
        }

        // Token: 0x06000706 RID: 1798 RVA: 0x00012D78 File Offset: 0x00010F78
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method)
        {
            return this.FromResolveAllGetter<TObj, TContract>(identifier, method, InjectSources.Any);
        }

        // Token: 0x06000707 RID: 1799 RVA: 0x00012D84 File Offset: 0x00010F84
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAllGetter<TObj, TContract>(object identifier, Func<TObj, TContract> method, InjectSources source)
        {
            return base.FromResolveGetterBase<TObj, TContract>(identifier, method, source, true);
        }

        // Token: 0x06000708 RID: 1800 RVA: 0x00012D90 File Offset: 0x00010F90
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromInstance(object instance)
        {
            return base.FromInstanceBase(instance);
        }

        // Token: 0x06000709 RID: 1801 RVA: 0x00012D9C File Offset: 0x00010F9C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(Func<Component, bool> predicate, bool includeInactive = true)
        {
            return this.FromComponentsInChildren(false, predicate, includeInactive);
        }

        // Token: 0x0600070A RID: 1802 RVA: 0x00012DA8 File Offset: 0x00010FA8
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildren(bool excludeSelf = false, Func<Component, bool> predicate = null, bool includeInactive = true)
        {
            return base.FromComponentsInChildrenBase(excludeSelf, predicate, includeInactive);
        }

        // Token: 0x0600070B RID: 1803 RVA: 0x00012DB4 File Offset: 0x00010FB4
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInHierarchy(Func<Component, bool> predicate = null, bool includeInactive = true)
        {
            return base.FromComponentsInHierarchyBase(predicate, includeInactive);
        }
    }
}
