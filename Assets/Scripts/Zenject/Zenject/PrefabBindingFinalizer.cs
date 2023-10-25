using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000162 RID: 354
    [NoReflectionBaking]
    public class PrefabBindingFinalizer : ProviderBindingFinalizer
    {
        // Token: 0x0600079F RID: 1951 RVA: 0x000143A0 File Offset: 0x000125A0
        public PrefabBindingFinalizer(BindInfo bindInfo, GameObjectCreationParameters gameObjectBindInfo, UnityEngine.Object prefab, Func<Type, IPrefabInstantiator, IProvider> providerFactory) : base(bindInfo)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
            this._prefab = prefab;
            this._providerFactory = providerFactory;
        }

        // Token: 0x060007A0 RID: 1952 RVA: 0x000143C0 File Offset: 0x000125C0
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

        // Token: 0x060007A1 RID: 1953 RVA: 0x000143FC File Offset: 0x000125FC
        void FinalizeBindingConcrete(DiContainer container, List<Type> concreteTypes)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                    {
                        RegisterProvidersForAllContractsPerConcreteType(
                            container,
                            concreteTypes,
                            (_, concreteType) =>
                                _providerFactory(
                                    concreteType,
                                    new PrefabInstantiator(
                                        container,
                                        _gameObjectBindInfo,
                                        concreteType,
                                        concreteTypes,
                                        BindInfo.Arguments,
                                        new PrefabProvider(_prefab),
                                        BindInfo.InstantiatedCallback)));
                        break;
                    }
                case ScopeTypes.Singleton:
                    {
                        var argumentTarget = concreteTypes.OnlyOrDefault();

                        if (argumentTarget == null)
                        {
                            Assert.That(BindInfo.Arguments.IsEmpty(),
                                "Cannot provide arguments to prefab instantiator when using more than one concrete type");
                        }

                        var prefabCreator = new PrefabInstantiatorCached(
                            new PrefabInstantiator(
                                container,
                                _gameObjectBindInfo,
                                argumentTarget,
                                concreteTypes,
                                BindInfo.Arguments,
                                new PrefabProvider(_prefab),
                                BindInfo.InstantiatedCallback));

                        RegisterProvidersForAllContractsPerConcreteType(
                            container,
                            concreteTypes,
                            (_, concreteType) => BindingUtil.CreateCachedProvider(
                                _providerFactory(concreteType, prefabCreator)));
                        break;
                    }
                default:
                    {
                        throw Assert.CreateException();
                    }
            }
        }

        // Token: 0x060007A2 RID: 1954 RVA: 0x00014514 File Offset: 0x00012714
        void FinalizeBindingSelf(DiContainer container)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                    {
                        RegisterProviderPerContract(
                            container,
                            (_, contractType) =>
                                _providerFactory(
                                    contractType,
                                    new PrefabInstantiator(
                                        container,
                                        _gameObjectBindInfo,
                                        contractType,
                                        BindInfo.ContractTypes,
                                        BindInfo.Arguments,
                                        new PrefabProvider(_prefab),
                                        BindInfo.InstantiatedCallback)));
                        break;
                    }
                case ScopeTypes.Singleton:
                    {
                        var argumentTarget = BindInfo.ContractTypes.OnlyOrDefault();

                        if (argumentTarget == null)
                        {
                            Assert.That(BindInfo.Arguments.IsEmpty(),
                                "Cannot provide arguments to prefab instantiator when using more than one concrete type");
                        }

                        var prefabCreator = new PrefabInstantiatorCached(
                            new PrefabInstantiator(
                                container,
                                _gameObjectBindInfo,
                                argumentTarget,
                                BindInfo.ContractTypes,
                                BindInfo.Arguments,
                                new PrefabProvider(_prefab),
                                BindInfo.InstantiatedCallback));

                        RegisterProviderPerContract(
                            container,
                            (_, contractType) =>
                                BindingUtil.CreateCachedProvider(
                                    _providerFactory(contractType, prefabCreator)));
                        break;
                    }
                default:
                    {
                        throw Assert.CreateException();
                    }
            }
        }

        // Token: 0x04000283 RID: 643
        private readonly GameObjectCreationParameters _gameObjectBindInfo;

        // Token: 0x04000284 RID: 644
        private readonly UnityEngine.Object _prefab;

        // Token: 0x04000285 RID: 645
        private readonly Func<Type, IPrefabInstantiator, IProvider> _providerFactory;
    }
}
