using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200016C RID: 364
    [NoReflectionBaking]
    public abstract class ProviderBindingFinalizer : IBindingFinalizer
    {
        // Token: 0x060007C7 RID: 1991 RVA: 0x00014E60 File Offset: 0x00013060
        public ProviderBindingFinalizer(BindInfo bindInfo)
        {
            this.BindInfo = bindInfo;
        }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x060007C8 RID: 1992 RVA: 0x00014E70 File Offset: 0x00013070
        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                return this.BindInfo.BindingInheritanceMethod;
            }
        }

        // Token: 0x17000055 RID: 85
        // (get) Token: 0x060007C9 RID: 1993 RVA: 0x00014E80 File Offset: 0x00013080
        // (set) Token: 0x060007CA RID: 1994 RVA: 0x00014E88 File Offset: 0x00013088
        protected BindInfo BindInfo { get; private set; }

        // Token: 0x060007CB RID: 1995 RVA: 0x00014E94 File Offset: 0x00013094
        protected ScopeTypes GetScope()
        {
            if (this.BindInfo.Scope == ScopeTypes.Unset)
            {
                ModestTree.Assert.That(!this.BindInfo.RequireExplicitScope || this.BindInfo.Condition != null, "Scope must be set for the previous binding!  Please either specify AsTransient, AsCached, or AsSingle. Last binding: Contract: {0}, Identifier: {1} {2}", (from x in this.BindInfo.ContractTypes
                                                                                                                                                                                                                                                                           select x.PrettyName()).Join(", "), this.BindInfo.Identifier, (this.BindInfo.ContextInfo != null) ? "Context: '{0}'".Fmt(new object[]
                {
                    this.BindInfo.ContextInfo
                }) : "");
                return ScopeTypes.Transient;
            }
            return this.BindInfo.Scope;
        }

        // Token: 0x060007CC RID: 1996 RVA: 0x00014F5C File Offset: 0x0001315C
        public void FinalizeBinding(DiContainer container)
        {
            if (this.BindInfo.ContractTypes.Count == 0)
            {
                return;
            }
            try
            {
                this.OnFinalizeBinding(container);
            }
            catch (Exception innerException)
            {
                string message = "Error while finalizing previous binding! Contract: {0}, Identifier: {1} {2}";
                object[] array = new object[3];
                array[0] = (from x in this.BindInfo.ContractTypes
                            select x.PrettyName()).Join(", ");
                array[1] = this.BindInfo.Identifier;
                array[2] = ((this.BindInfo.ContextInfo != null) ? "Context: '{0}'".Fmt(new object[]
                {
                    this.BindInfo.ContextInfo
                }) : "");
                throw ModestTree.Assert.CreateException(innerException, message, array);
            }
        }

        // Token: 0x060007CD RID: 1997
        protected abstract void OnFinalizeBinding(DiContainer container);

        // Token: 0x060007CE RID: 1998 RVA: 0x0001502C File Offset: 0x0001322C
        protected void RegisterProvider<TContract>(DiContainer container, IProvider provider)
        {
            this.RegisterProvider(container, typeof(TContract), provider);
        }

        // Token: 0x060007CF RID: 1999 RVA: 0x00015040 File Offset: 0x00013240
        protected void RegisterProvider(DiContainer container, Type contractType, IProvider provider)
        {
            if (this.BindInfo.OnlyBindIfNotBound && container.HasBindingId(contractType, this.BindInfo.Identifier))
            {
                return;
            }
            container.RegisterProvider(new BindingId(contractType, this.BindInfo.Identifier), this.BindInfo.Condition, provider, this.BindInfo.NonLazy);
            if (contractType.IsValueType() && (!contractType.IsGenericType() || !(contractType.GetGenericTypeDefinition() == typeof(Nullable<>))))
            {
                Type type = typeof(Nullable<>).MakeGenericType(new Type[]
                {
                    contractType
                });
                container.RegisterProvider(new BindingId(type, this.BindInfo.Identifier), this.BindInfo.Condition, provider, this.BindInfo.NonLazy);
            }
        }

        // Token: 0x060007D0 RID: 2000 RVA: 0x0001510C File Offset: 0x0001330C
        protected void RegisterProviderPerContract(DiContainer container, Func<DiContainer, Type, IProvider> providerFunc)
        {
            foreach (Type type in this.BindInfo.ContractTypes)
            {
                IProvider provider = providerFunc(container, type);
                if (this.BindInfo.MarkAsUniqueSingleton)
                {
                    container.SingletonMarkRegistry.MarkSingleton(type);
                }
                else if (this.BindInfo.MarkAsCreationBinding)
                {
                    container.SingletonMarkRegistry.MarkNonSingleton(type);
                }
                this.RegisterProvider(container, type, provider);
            }
        }

        // Token: 0x060007D1 RID: 2001 RVA: 0x000151A4 File Offset: 0x000133A4
        protected void RegisterProviderForAllContracts(DiContainer container, IProvider provider)
        {
            foreach (Type type in this.BindInfo.ContractTypes)
            {
                if (this.BindInfo.MarkAsUniqueSingleton)
                {
                    container.SingletonMarkRegistry.MarkSingleton(type);
                }
                else if (this.BindInfo.MarkAsCreationBinding)
                {
                    container.SingletonMarkRegistry.MarkNonSingleton(type);
                }
                this.RegisterProvider(container, type, provider);
            }
        }

        // Token: 0x060007D2 RID: 2002 RVA: 0x00015234 File Offset: 0x00013434
        protected void RegisterProvidersPerContractAndConcreteType(DiContainer container, List<Type> concreteTypes, Func<Type, Type, IProvider> providerFunc)
        {
            ModestTree.Assert.That(!this.BindInfo.ContractTypes.IsEmpty<Type>());
            ModestTree.Assert.That(!concreteTypes.IsEmpty<Type>());
            foreach (Type type in this.BindInfo.ContractTypes)
            {
                foreach (Type type2 in concreteTypes)
                {
                    if (this.ValidateBindTypes(type2, type))
                    {
                        this.RegisterProvider(container, type, providerFunc(type, type2));
                    }
                }
            }
        }

        // Token: 0x060007D3 RID: 2003 RVA: 0x000152FC File Offset: 0x000134FC
        private bool ValidateBindTypes(Type concreteType, Type contractType)
        {
            bool flag = concreteType.IsOpenGenericType();
            bool flag2 = contractType.IsOpenGenericType();
            if (flag != flag2)
            {
                return false;
            }
            if (flag2)
            {
                ModestTree.Assert.That(flag);
                if (ModestTree.TypeExtensions.IsAssignableToGenericType(concreteType, contractType))
                {
                    return true;
                }
            }
            else if (concreteType.DerivesFromOrEqual(contractType))
            {
                return true;
            }
            if (this.BindInfo.InvalidBindResponse == InvalidBindResponses.Assert)
            {
                throw ModestTree.Assert.CreateException("Expected type '{0}' to derive from or be equal to '{1}'", new object[]
                {
                    concreteType,
                    contractType
                });
            }
            ModestTree.Assert.IsEqual(this.BindInfo.InvalidBindResponse, InvalidBindResponses.Skip);
            return false;
        }

        // Token: 0x060007D4 RID: 2004 RVA: 0x00015380 File Offset: 0x00013580
        protected void RegisterProvidersForAllContractsPerConcreteType(DiContainer container, List<Type> concreteTypes, Func<DiContainer, Type, IProvider> providerFunc)
        {
            ModestTree.Assert.That(!this.BindInfo.ContractTypes.IsEmpty<Type>());
            ModestTree.Assert.That(!concreteTypes.IsEmpty<Type>());
            Dictionary<Type, IProvider> dictionary = Zenject.Internal.ZenPools.SpawnDictionary<Type, IProvider>();
            try
            {
                foreach (Type type in concreteTypes)
                {
                    IProvider value = providerFunc(container, type);
                    dictionary[type] = value;
                    if (this.BindInfo.MarkAsUniqueSingleton)
                    {
                        container.SingletonMarkRegistry.MarkSingleton(type);
                    }
                    else if (this.BindInfo.MarkAsCreationBinding)
                    {
                        container.SingletonMarkRegistry.MarkNonSingleton(type);
                    }
                }
                foreach (Type contractType in this.BindInfo.ContractTypes)
                {
                    foreach (Type type2 in concreteTypes)
                    {
                        if (this.ValidateBindTypes(type2, contractType))
                        {
                            this.RegisterProvider(container, contractType, dictionary[type2]);
                        }
                    }
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnDictionary<Type, IProvider>(dictionary);
            }
        }
    }
}
