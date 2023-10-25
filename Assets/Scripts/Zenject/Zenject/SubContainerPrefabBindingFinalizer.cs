using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000177 RID: 375
    [NoReflectionBaking]
    public class SubContainerPrefabBindingFinalizer : ProviderBindingFinalizer
    {
        // Token: 0x060007FD RID: 2045 RVA: 0x00015C44 File Offset: 0x00013E44
        public SubContainerPrefabBindingFinalizer(BindInfo bindInfo, object subIdentifier, bool resolveAll, Func<DiContainer, ISubContainerCreator> subContainerCreatorFactory) : base(bindInfo)
        {
            this._subIdentifier = subIdentifier;
            this._resolveAll = resolveAll;
            this._subContainerCreatorFactory = subContainerCreatorFactory;
        }

        // Token: 0x060007FE RID: 2046 RVA: 0x00015C64 File Offset: 0x00013E64
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

        // Token: 0x060007FF RID: 2047 RVA: 0x00015CA0 File Offset: 0x00013EA0
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
                            (_, concreteType) => new SubContainerDependencyProvider(
                                concreteType, _subIdentifier,
                                _subContainerCreatorFactory(container), _resolveAll));
                        break;
                    }
                case ScopeTypes.Singleton:
                    {
                        var containerCreator = new SubContainerCreatorCached(
                            _subContainerCreatorFactory(container));

                        RegisterProvidersForAllContractsPerConcreteType(
                            container,
                            concreteTypes,
                            (_, concreteType) =>
                            new SubContainerDependencyProvider(
                                concreteType, _subIdentifier, containerCreator, _resolveAll));
                        break;
                    }
                default:
                    {
                        throw Assert.CreateException();
                    }
            }
        }

        void FinalizeBindingSelf(DiContainer container)
        {
            var scope = GetScope();

            switch (scope)
            {
                case ScopeTypes.Transient:
                    {
                        RegisterProviderPerContract(
                            container,
                            (_, contractType) => new SubContainerDependencyProvider(
                                contractType, _subIdentifier,
                                _subContainerCreatorFactory(container), _resolveAll));
                        break;
                    }
                case ScopeTypes.Singleton:
                    {
                        var containerCreator = new SubContainerCreatorCached(
                            _subContainerCreatorFactory(container));

                        RegisterProviderPerContract(
                            container,
                            (_, contractType) =>
                            new SubContainerDependencyProvider(
                                contractType, _subIdentifier, containerCreator, _resolveAll));
                        break;
                    }
                default:
                    {
                        throw Assert.CreateException();
                    }
            }
        }

        // Token: 0x040002B0 RID: 688
        private readonly object _subIdentifier;

        // Token: 0x040002B1 RID: 689
        private readonly bool _resolveAll;

        // Token: 0x040002B2 RID: 690
        private readonly Func<DiContainer, ISubContainerCreator> _subContainerCreatorFactory;
    }
}
