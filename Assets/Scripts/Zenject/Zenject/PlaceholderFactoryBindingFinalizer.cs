using System;
using System.Linq;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x02000111 RID: 273
    [NoReflectionBaking]
    public class PlaceholderFactoryBindingFinalizer<TContract> : ProviderBindingFinalizer
    {
        // Token: 0x060005DD RID: 1501 RVA: 0x0000FB4C File Offset: 0x0000DD4C
        public PlaceholderFactoryBindingFinalizer(BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindInfo)
        {
            ModestTree.Assert.That(factoryBindInfo.FactoryType.DerivesFrom<IPlaceholderFactory>());
            this._factoryBindInfo = factoryBindInfo;
        }

        // Token: 0x060005DE RID: 1502 RVA: 0x0000FB6C File Offset: 0x0000DD6C
        protected override void OnFinalizeBinding(DiContainer container)
        {
            IProvider param = this._factoryBindInfo.ProviderFunc(container);
            TransientProvider transientProvider = new TransientProvider(this._factoryBindInfo.FactoryType, container, this._factoryBindInfo.Arguments.Concat(InjectUtil.CreateArgListExplicit<IProvider, InjectContext>(param, new InjectContext(container, typeof(TContract)))).ToList<TypeValuePair>(), base.BindInfo.ContextInfo, base.BindInfo.ConcreteIdentifier, null);
            IProvider provider;
            if (base.BindInfo.Scope == ScopeTypes.Unset || base.BindInfo.Scope == ScopeTypes.Singleton)
            {
                provider = BindingUtil.CreateCachedProvider(transientProvider);
            }
            else
            {
                ModestTree.Assert.IsEqual(base.BindInfo.Scope, ScopeTypes.Transient);
                provider = transientProvider;
            }
            base.RegisterProviderForAllContracts(container, provider);
        }

        // Token: 0x04000201 RID: 513
        private readonly FactoryBindInfo _factoryBindInfo;
    }
}
