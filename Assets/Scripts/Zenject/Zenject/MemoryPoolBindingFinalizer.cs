using System;
using System.Buffers;
using System.Linq;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x02000112 RID: 274
    [NoReflectionBaking]
    public class MemoryPoolBindingFinalizer<TContract> : ProviderBindingFinalizer
    {
        // Token: 0x060005DF RID: 1503 RVA: 0x0000FC28 File Offset: 0x0000DE28
        public MemoryPoolBindingFinalizer(BindInfo bindInfo, FactoryBindInfo factoryBindInfo, MemoryPoolBindInfo poolBindInfo) : base(bindInfo)
        {
            ModestTree.Assert.That(factoryBindInfo.FactoryType.DerivesFrom<IMemoryPool>());
            this._factoryBindInfo = factoryBindInfo;
            this._poolBindInfo = poolBindInfo;
        }

        // Token: 0x060005E0 RID: 1504 RVA: 0x0000FC50 File Offset: 0x0000DE50
        protected override void OnFinalizeBinding(DiContainer container)
        {
            FactoryProviderWrapper<TContract> param = new FactoryProviderWrapper<TContract>(this._factoryBindInfo.ProviderFunc(container), new InjectContext(container, typeof(TContract)));
            MemoryPoolSettings param2 = new MemoryPoolSettings(this._poolBindInfo.InitialSize, this._poolBindInfo.MaxSize, this._poolBindInfo.ExpandMethod, this._poolBindInfo.ShowExpandWarning);
            TransientProvider transientProvider = new TransientProvider(this._factoryBindInfo.FactoryType, container, this._factoryBindInfo.Arguments.Concat(InjectUtil.CreateArgListExplicit<FactoryProviderWrapper<TContract>, MemoryPoolSettings>(param, param2)).ToList<TypeValuePair>(), base.BindInfo.ContextInfo, base.BindInfo.ConcreteIdentifier, null);
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

        // Token: 0x04000202 RID: 514
        private readonly MemoryPoolBindInfo _poolBindInfo;

        // Token: 0x04000203 RID: 515
        private readonly FactoryBindInfo _factoryBindInfo;
    }
}
