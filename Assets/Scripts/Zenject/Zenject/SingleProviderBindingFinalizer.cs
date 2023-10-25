using System;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x02000171 RID: 369
    [NoReflectionBaking]
    public class SingleProviderBindingFinalizer : ProviderBindingFinalizer
    {
        // Token: 0x060007E7 RID: 2023 RVA: 0x000157B0 File Offset: 0x000139B0
        public SingleProviderBindingFinalizer(BindInfo bindInfo, Func<DiContainer, Type, IProvider> providerFactory) : base(bindInfo)
        {
            this._providerFactory = providerFactory;
        }

        // Token: 0x060007E8 RID: 2024 RVA: 0x000157C0 File Offset: 0x000139C0
        protected override void OnFinalizeBinding(DiContainer container)
        {
            if (base.BindInfo.ToChoice == ToChoices.Self)
            {
                ModestTree.Assert.IsEmpty<Type>(base.BindInfo.ToTypes);
                base.RegisterProviderPerContract(container, this._providerFactory);
                return;
            }
            if (!base.BindInfo.ToTypes.IsEmpty<Type>())
            {
                base.RegisterProvidersForAllContractsPerConcreteType(container, base.BindInfo.ToTypes, this._providerFactory);
            }
        }

        // Token: 0x040002A4 RID: 676
        private readonly Func<DiContainer, Type, IProvider> _providerFactory;
    }
}
