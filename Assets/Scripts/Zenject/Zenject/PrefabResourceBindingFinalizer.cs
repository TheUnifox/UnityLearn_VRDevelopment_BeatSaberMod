using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000167 RID: 359
    [NoReflectionBaking]
    public class PrefabResourceBindingFinalizer : ProviderBindingFinalizer
    {
        // Token: 0x060007B3 RID: 1971 RVA: 0x00014900 File Offset: 0x00012B00
        public PrefabResourceBindingFinalizer(BindInfo bindInfo, GameObjectCreationParameters gameObjectBindInfo, string resourcePath, Func<Type, IPrefabInstantiator, IProvider> providerFactory) : base(bindInfo)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
            this._resourcePath = resourcePath;
            this._providerFactory = providerFactory;
        }

        // Token: 0x060007B4 RID: 1972 RVA: 0x00014920 File Offset: 0x00012B20
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

        // Token: 0x060007B5 RID: 1973 RVA: 0x0001495C File Offset: 0x00012B5C
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
                                        new PrefabProviderResource(_resourcePath),
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
                                new PrefabProviderResource(_resourcePath),
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

        // Token: 0x060007B6 RID: 1974 RVA: 0x00014A74 File Offset: 0x00012C74
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
                                        new PrefabProviderResource(_resourcePath),
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
                                new PrefabProviderResource(_resourcePath),
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

        // Token: 0x0400028F RID: 655
        private readonly GameObjectCreationParameters _gameObjectBindInfo;

        // Token: 0x04000290 RID: 656
        private readonly string _resourcePath;

        // Token: 0x04000291 RID: 657
        private readonly Func<Type, IPrefabInstantiator, IProvider> _providerFactory;
    }
}
