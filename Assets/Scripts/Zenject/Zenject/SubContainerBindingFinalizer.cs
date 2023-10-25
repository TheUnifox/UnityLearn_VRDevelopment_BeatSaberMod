using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000172 RID: 370
    [NoReflectionBaking]
    public class SubContainerBindingFinalizer : ProviderBindingFinalizer
    {
        // Token: 0x060007E9 RID: 2025 RVA: 0x00015824 File Offset: 0x00013A24
        public SubContainerBindingFinalizer(BindInfo bindInfo, object subIdentifier, bool resolveAll, Func<DiContainer, ISubContainerCreator> creatorFactory) : base(bindInfo)
        {
            this._subIdentifier = subIdentifier;
            this._resolveAll = resolveAll;
            this._creatorFactory = creatorFactory;
        }

        // Token: 0x060007EA RID: 2026 RVA: 0x00015844 File Offset: 0x00013A44
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

        // Token: 0x060007EB RID: 2027 RVA: 0x00015880 File Offset: 0x00013A80
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
                                new SubContainerDependencyProvider(
                                    concreteType, _subIdentifier, _creatorFactory(container), _resolveAll));
                        break;
                    }
                case ScopeTypes.Singleton:
                    {
                        var containerCreator = new SubContainerCreatorCached(_creatorFactory(container));

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

        // Token: 0x060007EC RID: 2028 RVA: 0x00015920 File Offset: 0x00013B20
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
                                contractType, _subIdentifier, _creatorFactory(container), _resolveAll));
                        break;
                    }
                case ScopeTypes.Singleton:
                    {
                        var containerCreator = new SubContainerCreatorCached(_creatorFactory(container));

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

        // Token: 0x040002A5 RID: 677
        private readonly object _subIdentifier;

        // Token: 0x040002A6 RID: 678
        private readonly bool _resolveAll;

        // Token: 0x040002A7 RID: 679
        private readonly Func<DiContainer, ISubContainerCreator> _creatorFactory;
    }
}
