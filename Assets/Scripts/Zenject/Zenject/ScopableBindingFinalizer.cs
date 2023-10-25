using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x0200016E RID: 366
    [NoReflectionBaking]
    public class ScopableBindingFinalizer : ProviderBindingFinalizer
    {
        // Token: 0x060007DB RID: 2011 RVA: 0x00015570 File Offset: 0x00013770
        public ScopableBindingFinalizer(BindInfo bindInfo, Func<DiContainer, Type, IProvider> providerFactory) : base(bindInfo)
        {
            this._providerFactory = providerFactory;
        }

        // Token: 0x060007DC RID: 2012 RVA: 0x00015580 File Offset: 0x00013780
        protected override void OnFinalizeBinding(DiContainer container)
        {
            if (base.BindInfo.ToChoice == ToChoices.Self)
            {
                ModestTree.Assert.IsEmpty<Type>(base.BindInfo.ToTypes);
                this.FinalizeBindingSelf(container);
                return;
            }
            this.FinalizeBindingConcrete(container, base.BindInfo.ToTypes);
        }

        // Token: 0x060007DD RID: 2013 RVA: 0x000155BC File Offset: 0x000137BC
        private void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            if (concreteTypes.Count == 0)
            {
                return;
            }
            ScopeTypes scope = base.GetScope();
            if (scope == ScopeTypes.Transient)
            {
                base.RegisterProvidersForAllContractsPerConcreteType(container, concreteTypes, this._providerFactory);
                return;
            }
            if (scope != ScopeTypes.Singleton)
            {
                throw ModestTree.Assert.CreateException();
            }
            base.RegisterProvidersForAllContractsPerConcreteType(container, concreteTypes, (DiContainer _, Type concreteType) => BindingUtil.CreateCachedProvider(this._providerFactory(container, concreteType)));
        }

        // Token: 0x060007DE RID: 2014 RVA: 0x0001562C File Offset: 0x0001382C
        private void FinalizeBindingSelf(DiContainer container)
        {
            ScopeTypes scope = base.GetScope();
            if (scope == ScopeTypes.Transient)
            {
                base.RegisterProviderPerContract(container, this._providerFactory);
                return;
            }
            if (scope != ScopeTypes.Singleton)
            {
                throw ModestTree.Assert.CreateException();
            }
            base.RegisterProviderPerContract(container, (DiContainer _, Type contractType) => BindingUtil.CreateCachedProvider(this._providerFactory(container, contractType)));
        }

        // Token: 0x0400029F RID: 671
        private readonly Func<DiContainer, Type, IProvider> _providerFactory;
    }
}
