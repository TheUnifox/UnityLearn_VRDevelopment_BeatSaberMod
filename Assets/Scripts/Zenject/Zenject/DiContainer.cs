using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200022F RID: 559
    [NoReflectionBaking]
    public class DiContainer : IInstantiator
    {
        // Token: 0x06000BF6 RID: 3062 RVA: 0x0001FB80 File Offset: 0x0001DD80
        public DiContainer(IEnumerable<DiContainer> parentContainersEnumerable, bool isValidating)
        {
            this._isValidating = isValidating;
            this._lazyInjector = new LazyInstanceInjector(this);
            this.InstallDefaultBindings();
            this.FlushBindings();
            ModestTree.Assert.That(this._currentBindings.Count == 0);
            this._settings = ZenjectSettings.Default;
            DiContainer[] array = new DiContainer[]
            {
                this
            };
            this._containerLookups[1] = array;
            DiContainer[] array2 = parentContainersEnumerable.ToArray<DiContainer>();
            this._containerLookups[2] = array2;
            DiContainer[] array3 = this.FlattenInheritanceChain().ToArray();
            this._containerLookups[3] = array3;
            this._containerLookups[0] = array.Concat(array3).ToArray<DiContainer>();
            if (!array2.IsEmpty<DiContainer>())
            {
                for (int i = 0; i < array2.Length; i++)
                {
                    array2[i].FlushBindings();
                }
                this._inheritedDefaultParent = array2.First<DiContainer>().DefaultParent;
                foreach (DiContainer diContainer in array3.Distinct<DiContainer>())
                {
                    foreach (BindStatement binding in diContainer._childBindings)
                    {
                        if (this.ShouldInheritBinding(binding, diContainer))
                        {
                            this.FinalizeBinding(binding);
                        }
                    }
                }
                ModestTree.Assert.That(this._currentBindings.Count == 0);
                ModestTree.Assert.That(this._childBindings.Count == 0);
            }
            ZenjectSettings zenjectSettings = this.TryResolve<ZenjectSettings>();
            if (zenjectSettings != null)
            {
                this._settings = zenjectSettings;
            }
        }

        // Token: 0x06000BF7 RID: 3063 RVA: 0x0001FD8C File Offset: 0x0001DF8C
        public DiContainer(bool isValidating) : this(Enumerable.Empty<DiContainer>(), isValidating)
        {
        }

        // Token: 0x06000BF8 RID: 3064 RVA: 0x0001FD9C File Offset: 0x0001DF9C
        public DiContainer() : this(Enumerable.Empty<DiContainer>(), false)
        {
        }

        // Token: 0x06000BF9 RID: 3065 RVA: 0x0001FDAC File Offset: 0x0001DFAC
        public DiContainer(DiContainer parentContainer, bool isValidating) : this(new DiContainer[]
        {
            parentContainer
        }, isValidating)
        {
        }

        // Token: 0x06000BFA RID: 3066 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
        public DiContainer(DiContainer parentContainer) : this(new DiContainer[]
        {
            parentContainer
        }, false)
        {
        }

        // Token: 0x06000BFB RID: 3067 RVA: 0x0001FDD4 File Offset: 0x0001DFD4
        public DiContainer(IEnumerable<DiContainer> parentContainers) : this(parentContainers, false)
        {
        }

        // Token: 0x170000DC RID: 220
        // (get) Token: 0x06000BFC RID: 3068 RVA: 0x0001FDE0 File Offset: 0x0001DFE0
        // (set) Token: 0x06000BFD RID: 3069 RVA: 0x0001FDE8 File Offset: 0x0001DFE8
        public ZenjectSettings Settings
        {
            get
            {
                return this._settings;
            }
            set
            {
                this._settings = value;
                this.Rebind<ZenjectSettings>().FromInstance(value);
            }
        }

        // Token: 0x170000DD RID: 221
        // (get) Token: 0x06000BFE RID: 3070 RVA: 0x0001FE00 File Offset: 0x0001E000
        internal Zenject.Internal.SingletonMarkRegistry SingletonMarkRegistry
        {
            get
            {
                return this._singletonMarkRegistry;
            }
        }

        // Token: 0x170000DE RID: 222
        // (get) Token: 0x06000BFF RID: 3071 RVA: 0x0001FE08 File Offset: 0x0001E008
        public IEnumerable<IProvider> AllProviders
        {
            get
            {
                return (from x in this._providers.Values.SelectMany((List<DiContainer.ProviderInfo> x) => x)
                        select x.Provider).Distinct<IProvider>();
            }
        }

        // Token: 0x06000C00 RID: 3072 RVA: 0x0001FE70 File Offset: 0x0001E070
        private void InstallDefaultBindings()
        {
            this.Bind(new Type[]
            {
                typeof(DiContainer),
                typeof(IInstantiator)
            }).FromInstance(this);
            this.Bind(new Type[]
            {
                typeof(LazyInject<>)
            }).FromMethodUntyped(new Func<InjectContext, object>(this.CreateLazyBinding)).Lazy();
        }

        // Token: 0x06000C01 RID: 3073 RVA: 0x0001FEDC File Offset: 0x0001E0DC
        private object CreateLazyBinding(InjectContext context)
        {
            InjectContext injectContext = context.Clone();
            injectContext.MemberType = context.MemberType.GenericArguments().Single<Type>();
            object obj = Activator.CreateInstance(typeof(LazyInject<>).MakeGenericType(new Type[]
            {
                injectContext.MemberType
            }), new object[]
            {
                this,
                injectContext
            });
            if (this._isValidating)
            {
                this.QueueForValidate((IValidatable)obj);
            }
            return obj;
        }

        // Token: 0x06000C02 RID: 3074 RVA: 0x0001FF50 File Offset: 0x0001E150
        public void QueueForValidate(IValidatable validatable)
        {
            if (!this._hasResolvedRoots)
            {
                Type type = validatable.GetType();
                if (!this._validatedTypes.Contains(type))
                {
                    this._validatedTypes.Add(type);
                    this._validationQueue.Add(validatable);
                }
            }
        }

        // Token: 0x06000C03 RID: 3075 RVA: 0x0001FF94 File Offset: 0x0001E194
        private bool ShouldInheritBinding(BindStatement binding, DiContainer ancestorContainer)
        {
            return binding.BindingInheritanceMethod == BindingInheritanceMethods.CopyIntoAll || binding.BindingInheritanceMethod == BindingInheritanceMethods.MoveIntoAll || ((binding.BindingInheritanceMethod == BindingInheritanceMethods.CopyDirectOnly || binding.BindingInheritanceMethod == BindingInheritanceMethods.MoveDirectOnly) && this.ParentContainers.Contains(ancestorContainer));
        }

        // Token: 0x170000DF RID: 223
        // (get) Token: 0x06000C04 RID: 3076 RVA: 0x0001FFD0 File Offset: 0x0001E1D0
        private Transform ContextTransform
        {
            get
            {
                if (!this._hasLookedUpContextTransform)
                {
                    this._hasLookedUpContextTransform = true;
                    Context context = this.TryResolve<Context>();
                    if (context != null)
                    {
                        this._contextTransform = context.transform;
                    }
                }
                return this._contextTransform;
            }
        }

        // Token: 0x170000E0 RID: 224
        // (get) Token: 0x06000C05 RID: 3077 RVA: 0x00020010 File Offset: 0x0001E210
        // (set) Token: 0x06000C06 RID: 3078 RVA: 0x00020018 File Offset: 0x0001E218
        public bool AssertOnNewGameObjects { get; set; }

        // Token: 0x170000E1 RID: 225
        // (get) Token: 0x06000C07 RID: 3079 RVA: 0x00020024 File Offset: 0x0001E224
        public Transform InheritedDefaultParent
        {
            get
            {
                return this._inheritedDefaultParent;
            }
        }

        // Token: 0x170000E2 RID: 226
        // (get) Token: 0x06000C08 RID: 3080 RVA: 0x0002002C File Offset: 0x0001E22C
        // (set) Token: 0x06000C09 RID: 3081 RVA: 0x00020034 File Offset: 0x0001E234
        public Transform DefaultParent
        {
            get
            {
                return this._explicitDefaultParent;
            }
            set
            {
                this._explicitDefaultParent = value;
                this._hasExplicitDefaultParent = true;
            }
        }

        // Token: 0x170000E3 RID: 227
        // (get) Token: 0x06000C0A RID: 3082 RVA: 0x00020044 File Offset: 0x0001E244
        public DiContainer[] ParentContainers
        {
            get
            {
                return this._containerLookups[2];
            }
        }

        // Token: 0x170000E4 RID: 228
        // (get) Token: 0x06000C0B RID: 3083 RVA: 0x00020050 File Offset: 0x0001E250
        public DiContainer[] AncestorContainers
        {
            get
            {
                return this._containerLookups[3];
            }
        }

        // Token: 0x170000E5 RID: 229
        // (get) Token: 0x06000C0C RID: 3084 RVA: 0x0002005C File Offset: 0x0001E25C
        public bool ChecksForCircularDependencies
        {
            get
            {
                return true;
            }
        }

        // Token: 0x170000E6 RID: 230
        // (get) Token: 0x06000C0D RID: 3085 RVA: 0x00020060 File Offset: 0x0001E260
        public bool IsValidating
        {
            get
            {
                return this._isValidating;
            }
        }

        // Token: 0x170000E7 RID: 231
        // (get) Token: 0x06000C0E RID: 3086 RVA: 0x00020068 File Offset: 0x0001E268
        // (set) Token: 0x06000C0F RID: 3087 RVA: 0x00020070 File Offset: 0x0001E270
        public bool IsInstalling
        {
            get
            {
                return this._isInstalling;
            }
            set
            {
                this._isInstalling = value;
            }
        }

        // Token: 0x170000E8 RID: 232
        // (get) Token: 0x06000C10 RID: 3088 RVA: 0x0002007C File Offset: 0x0001E27C
        public IEnumerable<BindingId> AllContracts
        {
            get
            {
                this.FlushBindings();
                return this._providers.Keys;
            }
        }

        // Token: 0x06000C11 RID: 3089 RVA: 0x00020090 File Offset: 0x0001E290
        public void ResolveRoots()
        {
            ModestTree.Assert.That(!this._hasResolvedRoots);
            this.FlushBindings();
            this.ResolveDependencyRoots();
            this._lazyInjector.LazyInjectAll();
            if (this.IsValidating)
            {
                this.FlushValidationQueue();
            }
            ModestTree.Assert.That(!this._hasResolvedRoots);
            this._hasResolvedRoots = true;
        }

        // Token: 0x06000C12 RID: 3090 RVA: 0x000200E8 File Offset: 0x0001E2E8
        private void ResolveDependencyRoots()
        {
            List<BindingId> list = new List<BindingId>();
            List<DiContainer.ProviderInfo> list2 = new List<DiContainer.ProviderInfo>();
            foreach (KeyValuePair<BindingId, List<DiContainer.ProviderInfo>> keyValuePair in this._providers)
            {
                foreach (DiContainer.ProviderInfo providerInfo in keyValuePair.Value)
                {
                    if (providerInfo.NonLazy)
                    {
                        list.Add(keyValuePair.Key);
                        list2.Add(providerInfo);
                    }
                }
            }
            ModestTree.Assert.IsEqual(list2.Count, list.Count);
            List<object> list3 = Zenject.Internal.ZenPools.SpawnList<object>();
            try
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    BindingId bindingId = list[i];
                    DiContainer.ProviderInfo providerInfo2 = list2[i];
                    using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, bindingId.Type))
                    {
                        injectContext.Identifier = bindingId.Identifier;
                        injectContext.SourceType = InjectSources.Local;
                        injectContext.Optional = false;
                        list3.Clear();
                        this.SafeGetInstances(providerInfo2, injectContext, list3);
                    }
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<object>(list3);
            }
        }

        // Token: 0x06000C13 RID: 3091 RVA: 0x00020254 File Offset: 0x0001E454
        private void ValidateFullResolve()
        {
            ModestTree.Assert.That(!this._hasResolvedRoots);
            ModestTree.Assert.That(this.IsValidating);
            foreach (BindingId bindingId in this._providers.Keys.ToList<BindingId>())
            {
                if (!bindingId.Type.IsOpenGenericType())
                {
                    using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, bindingId.Type))
                    {
                        injectContext.Identifier = bindingId.Identifier;
                        injectContext.SourceType = InjectSources.Local;
                        injectContext.Optional = true;
                        this.ResolveAll(injectContext);
                    }
                }
            }
        }

        // Token: 0x06000C14 RID: 3092 RVA: 0x0002031C File Offset: 0x0001E51C
        private void FlushValidationQueue()
        {
            ModestTree.Assert.That(!this._hasResolvedRoots);
            ModestTree.Assert.That(this.IsValidating);
            List<IValidatable> list = new List<IValidatable>();
            while (this._validationQueue.Any<IValidatable>())
            {
                list.Clear();
                list.AllocFreeAddRange(this._validationQueue);
                this._validationQueue.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Validate();
                }
            }
        }

        // Token: 0x06000C15 RID: 3093 RVA: 0x00020394 File Offset: 0x0001E594
        public DiContainer CreateSubContainer()
        {
            return this.CreateSubContainer(this._isValidating);
        }

        // Token: 0x06000C16 RID: 3094 RVA: 0x000203A4 File Offset: 0x0001E5A4
        public void QueueForInject(object instance)
        {
            this._lazyInjector.AddInstance(instance);
        }

        // Token: 0x06000C17 RID: 3095 RVA: 0x000203B4 File Offset: 0x0001E5B4
        public T LazyInject<T>(T instance)
        {
            this._lazyInjector.LazyInject(instance);
            return instance;
        }

        // Token: 0x06000C18 RID: 3096 RVA: 0x000203C8 File Offset: 0x0001E5C8
        private DiContainer CreateSubContainer(bool isValidating)
        {
            return new DiContainer(new DiContainer[]
            {
                this
            }, isValidating);
        }

        // Token: 0x06000C19 RID: 3097 RVA: 0x000203DC File Offset: 0x0001E5DC
        public void RegisterProvider(BindingId bindingId, BindingCondition condition, IProvider provider, bool nonLazy)
        {
            DiContainer.ProviderInfo item = new DiContainer.ProviderInfo(provider, condition, nonLazy, this);
            List<DiContainer.ProviderInfo> list;
            if (!this._providers.TryGetValue(bindingId, out list))
            {
                list = new List<DiContainer.ProviderInfo>();
                this._providers.Add(bindingId, list);
            }
            list.Add(item);
        }

        // Token: 0x06000C1A RID: 3098 RVA: 0x00020420 File Offset: 0x0001E620
        private void GetProviderMatches(InjectContext context, List<DiContainer.ProviderInfo> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(buffer.Count == 0);
            List<DiContainer.ProviderInfo> list = Zenject.Internal.ZenPools.SpawnList<DiContainer.ProviderInfo>();
            try
            {
                this.GetProvidersForContract(context.BindingId, context.SourceType, list);
                for (int i = 0; i < list.Count; i++)
                {
                    DiContainer.ProviderInfo providerInfo = list[i];
                    if (providerInfo.Condition == null || providerInfo.Condition(context))
                    {
                        buffer.Add(providerInfo);
                    }
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<DiContainer.ProviderInfo>(list);
            }
        }

        // Token: 0x06000C1B RID: 3099 RVA: 0x000204AC File Offset: 0x0001E6AC
        private DiContainer.ProviderInfo TryGetUniqueProvider(InjectContext context)
        {
            ModestTree.Assert.IsNotNull(context);
            BindingId bindingId = context.BindingId;
            InjectSources sourceType = context.SourceType;
            DiContainer[] array = this._containerLookups[(int)sourceType];
            for (int i = 0; i < array.Length; i++)
            {
                array[i].FlushBindings();
            }
            List<DiContainer.ProviderInfo> list = Zenject.Internal.ZenPools.SpawnList<DiContainer.ProviderInfo>();
            DiContainer.ProviderInfo result;
            try
            {
                DiContainer.ProviderInfo providerInfo = null;
                int num = int.MaxValue;
                bool flag = false;
                bool flag2 = false;
                foreach (DiContainer diContainer in array)
                {
                    int containerHeirarchyDistance = this.GetContainerHeirarchyDistance(diContainer);
                    if (containerHeirarchyDistance <= num)
                    {
                        list.Clear();
                        diContainer.GetLocalProviders(bindingId, list);
                        for (int k = 0; k < list.Count; k++)
                        {
                            DiContainer.ProviderInfo providerInfo2 = list[k];
                            bool flag3 = providerInfo2.Condition != null;
                            if (!flag3 || providerInfo2.Condition(context))
                            {
                                ModestTree.Assert.That(providerInfo == null || num == containerHeirarchyDistance);
                                if (flag3)
                                {
                                    flag2 = flag;
                                }
                                else
                                {
                                    if (flag)
                                    {
                                        goto IL_EB;
                                    }
                                    if (providerInfo != null)
                                    {
                                        flag2 = true;
                                    }
                                }
                                if (!flag2)
                                {
                                    num = containerHeirarchyDistance;
                                    flag = flag3;
                                    providerInfo = providerInfo2;
                                }
                            }
                        IL_EB:;
                        }
                    }
                }
                if (flag2)
                {
                    throw ModestTree.Assert.CreateException("Found multiple matches when only one was expected for type '{0}'{1}. Object graph:\n {2}", new object[]
                    {
                        context.MemberType,
                        (context.ObjectType == null) ? "" : " while building object with type '{0}'".Fmt(new object[]
                        {
                            context.ObjectType
                        }),
                        context.GetObjectGraphString()
                    });
                }
                result = providerInfo;
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<DiContainer.ProviderInfo>(list);
            }
            return result;
        }

        // Token: 0x06000C1C RID: 3100 RVA: 0x00020648 File Offset: 0x0001E848
        private List<DiContainer> FlattenInheritanceChain()
        {
            List<DiContainer> list = new List<DiContainer>();
            Queue<DiContainer> queue = new Queue<DiContainer>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                foreach (DiContainer item in queue.Dequeue().ParentContainers)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                        queue.Enqueue(item);
                    }
                }
            }
            return list;
        }

        // Token: 0x06000C1D RID: 3101 RVA: 0x000206B0 File Offset: 0x0001E8B0
        private void GetLocalProviders(BindingId bindingId, List<DiContainer.ProviderInfo> buffer)
        {
            List<DiContainer.ProviderInfo> items;
            if (this._providers.TryGetValue(bindingId, out items))
            {
                buffer.AllocFreeAddRange(items);
                return;
            }
            if (bindingId.Type.IsGenericType() && this._providers.TryGetValue(new BindingId(bindingId.Type.GetGenericTypeDefinition(), bindingId.Identifier), out items))
            {
                buffer.AllocFreeAddRange(items);
            }
        }

        // Token: 0x06000C1E RID: 3102 RVA: 0x00020714 File Offset: 0x0001E914
        private void GetProvidersForContract(BindingId bindingId, InjectSources sourceType, List<DiContainer.ProviderInfo> buffer)
        {
            DiContainer[] array = this._containerLookups[(int)sourceType];
            for (int i = 0; i < array.Length; i++)
            {
                array[i].FlushBindings();
            }
            for (int j = 0; j < array.Length; j++)
            {
                array[j].GetLocalProviders(bindingId, buffer);
            }
        }

        // Token: 0x06000C1F RID: 3103 RVA: 0x00020758 File Offset: 0x0001E958
        public void Install<TInstaller>() where TInstaller : Installer
        {
            this.Instantiate<TInstaller>().InstallBindings();
        }

        // Token: 0x06000C20 RID: 3104 RVA: 0x0002076C File Offset: 0x0001E96C
        public void Install<TInstaller>(object[] extraArgs) where TInstaller : Installer
        {
            this.Instantiate<TInstaller>(extraArgs).InstallBindings();
        }

        // Token: 0x06000C21 RID: 3105 RVA: 0x00020780 File Offset: 0x0001E980
        public IList ResolveAll(InjectContext context)
        {
            List<object> list = Zenject.Internal.ZenPools.SpawnList<object>();
            IList result;
            try
            {
                this.ResolveAll(context, list);
                result = ModestTree.ReflectionUtil.CreateGenericList(context.MemberType, list);
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<object>(list);
            }
            return result;
        }

        // Token: 0x06000C22 RID: 3106 RVA: 0x000207C4 File Offset: 0x0001E9C4
        public void ResolveAll(InjectContext context, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            this.FlushBindings();
            this.CheckForInstallWarning(context);
            List<DiContainer.ProviderInfo> list = Zenject.Internal.ZenPools.SpawnList<DiContainer.ProviderInfo>();
            try
            {
                this.GetProviderMatches(context, list);
                if (list.Count == 0)
                {
                    if (!context.Optional)
                    {
                        throw ModestTree.Assert.CreateException("Could not find required dependency with type '{0}' Object graph:\n {1}", new object[]
                        {
                            context.MemberType,
                            context.GetObjectGraphString()
                        });
                    }
                }
                else
                {
                    List<object> list2 = Zenject.Internal.ZenPools.SpawnList<object>();
                    List<object> list3 = Zenject.Internal.ZenPools.SpawnList<object>();
                    try
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            DiContainer.ProviderInfo providerInfo = list[i];
                            list2.Clear();
                            this.SafeGetInstances(providerInfo, context, list2);
                            for (int j = 0; j < list2.Count; j++)
                            {
                                list3.Add(list2[j]);
                            }
                        }
                        if (list3.Count == 0 && !context.Optional)
                        {
                            throw ModestTree.Assert.CreateException("Could not find required dependency with type '{0}'.  Found providers but they returned zero results!", new object[]
                            {
                                context.MemberType
                            });
                        }
                        if (this.IsValidating)
                        {
                            for (int k = 0; k < list3.Count; k++)
                            {
                                if (list3[k] is ValidationMarker)
                                {
                                    list3[k] = context.MemberType.GetDefaultValue();
                                }
                            }
                        }
                        buffer.AllocFreeAddRange(list3);
                    }
                    finally
                    {
                        Zenject.Internal.ZenPools.DespawnList<object>(list2);
                        Zenject.Internal.ZenPools.DespawnList<object>(list3);
                    }
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<DiContainer.ProviderInfo>(list);
            }
        }

        // Token: 0x06000C23 RID: 3107 RVA: 0x00020940 File Offset: 0x0001EB40
        private void CheckForInstallWarning(InjectContext context)
        {
            if (!this._settings.DisplayWarningWhenResolvingDuringInstall)
            {
                return;
            }
            ModestTree.Assert.IsNotNull(context);
        }

        // Token: 0x06000C24 RID: 3108 RVA: 0x00020958 File Offset: 0x0001EB58
        public Type ResolveType<T>()
        {
            return this.ResolveType(typeof(T));
        }

        // Token: 0x06000C25 RID: 3109 RVA: 0x0002096C File Offset: 0x0001EB6C
        public Type ResolveType(Type type)
        {
            Type result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, type))
            {
                result = this.ResolveType(injectContext);
            }
            return result;
        }

        // Token: 0x06000C26 RID: 3110 RVA: 0x000209A8 File Offset: 0x0001EBA8
        public Type ResolveType(InjectContext context)
        {
            ModestTree.Assert.IsNotNull(context);
            this.FlushBindings();
            DiContainer.ProviderInfo providerInfo = this.TryGetUniqueProvider(context);
            if (providerInfo == null)
            {
                throw ModestTree.Assert.CreateException("Unable to resolve {0}{1}. Object graph:\n{2}", new object[]
                {
                    context.BindingId,
                    (context.ObjectType == null) ? "" : " while building object with type '{0}'".Fmt(new object[]
                    {
                        context.ObjectType
                    }),
                    context.GetObjectGraphString()
                });
            }
            return providerInfo.Provider.GetInstanceType(context);
        }

        // Token: 0x06000C27 RID: 3111 RVA: 0x00020A34 File Offset: 0x0001EC34
        public List<Type> ResolveTypeAll(Type type)
        {
            return this.ResolveTypeAll(type, null);
        }

        // Token: 0x06000C28 RID: 3112 RVA: 0x00020A40 File Offset: 0x0001EC40
        public List<Type> ResolveTypeAll(Type type, object identifier)
        {
            List<Type> result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, type))
            {
                injectContext.Identifier = identifier;
                result = this.ResolveTypeAll(injectContext);
            }
            return result;
        }

        // Token: 0x06000C29 RID: 3113 RVA: 0x00020A84 File Offset: 0x0001EC84
        public List<Type> ResolveTypeAll(InjectContext context)
        {
            ModestTree.Assert.IsNotNull(context);
            this.FlushBindings();
            List<DiContainer.ProviderInfo> list = Zenject.Internal.ZenPools.SpawnList<DiContainer.ProviderInfo>();
            List<Type> result;
            try
            {
                this.GetProviderMatches(context, list);
                if (list.Count > 0)
                {
                    result = (from x in list
                              select x.Provider.GetInstanceType(context) into x
                              where x != null
                              select x).ToList<Type>();
                }
                else
                {
                    result = new List<Type>();
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<DiContainer.ProviderInfo>(list);
            }
            return result;
        }

        // Token: 0x06000C2A RID: 3114 RVA: 0x00020B2C File Offset: 0x0001ED2C
        public object Resolve(BindingId id)
        {
            object result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, id.Type))
            {
                injectContext.Identifier = id.Identifier;
                result = this.Resolve(injectContext);
            }
            return result;
        }

        // Token: 0x06000C2B RID: 3115 RVA: 0x00020B7C File Offset: 0x0001ED7C
        public object Resolve(InjectContext context)
        {
            ModestTree.Assert.IsNotNull(context);
            Type memberType = context.MemberType;
            this.FlushBindings();
            this.CheckForInstallWarning(context);
            InjectContext injectContext = context;
            if (memberType.IsGenericType() && memberType.GetGenericTypeDefinition() == typeof(LazyInject<>))
            {
                injectContext = context.Clone();
                injectContext.Identifier = null;
                injectContext.SourceType = InjectSources.Local;
                injectContext.Optional = false;
            }
            DiContainer.ProviderInfo providerInfo = this.TryGetUniqueProvider(injectContext);
            object result;
            if (providerInfo == null)
            {
                if (memberType.IsArray && memberType.GetArrayRank() == 1)
                {
                    Type elementType = memberType.GetElementType();
                    InjectContext injectContext2 = context.Clone();
                    injectContext2.MemberType = elementType;
                    injectContext2.Optional = true;
                    List<object> list = Zenject.Internal.ZenPools.SpawnList<object>();
                    try
                    {
                        this.ResolveAll(injectContext2, list);
                        return ModestTree.ReflectionUtil.CreateArray(injectContext2.MemberType, list);
                    }
                    finally
                    {
                        Zenject.Internal.ZenPools.DespawnList<object>(list);
                    }
                }
                if (memberType.IsGenericType() && (memberType.GetGenericTypeDefinition() == typeof(List<>) || memberType.GetGenericTypeDefinition() == typeof(IList<>) || memberType.GetGenericTypeDefinition() == typeof(IReadOnlyList<>) || memberType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    Type memberType2 = memberType.GenericArguments().Single<Type>();
                    InjectContext injectContext3 = context.Clone();
                    injectContext3.MemberType = memberType2;
                    injectContext3.Optional = true;
                    return this.ResolveAll(injectContext3);
                }
                if (context.Optional)
                {
                    return context.FallBackValue;
                }
                throw ModestTree.Assert.CreateException("Unable to resolve '{0}'{1}. Object graph:\n{2}", new object[]
                {
                    context.BindingId,
                    (context.ObjectType == null) ? "" : " while building object with type '{0}'".Fmt(new object[]
                    {
                        context.ObjectType
                    }),
                    context.GetObjectGraphString()
                });
            }
            else
            {
                List<object> list2 = Zenject.Internal.ZenPools.SpawnList<object>();
                try
                {
                    this.SafeGetInstances(providerInfo, context, list2);
                    if (list2.Count == 0)
                    {
                        if (!context.Optional)
                        {
                            throw ModestTree.Assert.CreateException("Unable to resolve '{0}'{1}. Object graph:\n{2}", new object[]
                            {
                                context.BindingId,
                                (context.ObjectType == null) ? "" : " while building object with type '{0}'".Fmt(new object[]
                                {
                                    context.ObjectType
                                }),
                                context.GetObjectGraphString()
                            });
                        }
                        result = context.FallBackValue;
                    }
                    else
                    {
                        if (list2.Count<object>() > 1)
                        {
                            throw ModestTree.Assert.CreateException("Provider returned multiple instances when only one was expected!  While resolving '{0}'{1}. Object graph:\n{2}", new object[]
                            {
                                context.BindingId,
                                (context.ObjectType == null) ? "" : " while building object with type '{0}'".Fmt(new object[]
                                {
                                    context.ObjectType
                                }),
                                context.GetObjectGraphString()
                            });
                        }
                        result = list2.First<object>();
                    }
                }
                finally
                {
                    Zenject.Internal.ZenPools.DespawnList<object>(list2);
                }
            }
            return result;
        }

        // Token: 0x06000C2C RID: 3116 RVA: 0x00020E64 File Offset: 0x0001F064
        private void SafeGetInstances(DiContainer.ProviderInfo providerInfo, InjectContext context, List<object> instances)
        {
            ModestTree.Assert.IsNotNull(context);
            IProvider provider = providerInfo.Provider;
            if (this.ChecksForCircularDependencies)
            {
                Zenject.Internal.LookupId lookupId = Zenject.Internal.ZenPools.SpawnLookupId(provider, context.BindingId);
                try
                {
                    DiContainer container = providerInfo.Container;
                    if (container._resolvesTwiceInProgress.Contains(lookupId))
                    {
                        throw ModestTree.Assert.CreateException("Circular dependency detected! Object graph:\n {0}", new object[]
                        {
                            context.GetObjectGraphString()
                        });
                    }
                    bool flag = false;
                    if (!container._resolvesInProgress.Add(lookupId))
                    {
                        ModestTree.Assert.That(container._resolvesTwiceInProgress.Add(lookupId));
                        flag = true;
                    }
                    try
                    {
                        this.GetDecoratedInstances(provider, context, instances);
                        return;
                    }
                    finally
                    {
                        if (flag)
                        {
                            ModestTree.Assert.That(container._resolvesTwiceInProgress.Remove(lookupId));
                        }
                        else
                        {
                            ModestTree.Assert.That(container._resolvesInProgress.Remove(lookupId));
                        }
                    }
                }
                finally
                {
                    Zenject.Internal.ZenPools.DespawnLookupId(lookupId);
                }
            }
            this.GetDecoratedInstances(provider, context, instances);
        }

        // Token: 0x06000C2D RID: 3117 RVA: 0x00020F48 File Offset: 0x0001F148
        public DecoratorToChoiceFromBinder<TContract> Decorate<TContract>()
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(IFactory<TContract, TContract>));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(PlaceholderFactory<TContract, TContract>));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            Guid guid = Guid.NewGuid();
            bindInfo.Identifier = guid;
            Zenject.Internal.IDecoratorProvider decoratorProvider;
            if (!this._decorators.TryGetValue(typeof(TContract), out decoratorProvider))
            {
                decoratorProvider = new Zenject.Internal.DecoratorProvider<TContract>(this);
                this._decorators.Add(typeof(TContract), decoratorProvider);
            }
            ((Zenject.Internal.DecoratorProvider<TContract>)decoratorProvider).AddFactoryId(guid);
            return new DecoratorToChoiceFromBinder<TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000C2E RID: 3118 RVA: 0x00020FF0 File Offset: 0x0001F1F0
        private void GetDecoratedInstances(IProvider provider, InjectContext context, List<object> buffer)
        {
            Zenject.Internal.IDecoratorProvider decoratorProvider = this.TryGetDecoratorProvider(context.BindingId.Type);
            if (decoratorProvider != null)
            {
                decoratorProvider.GetAllInstances(provider, context, buffer);
                return;
            }
            provider.GetAllInstances(context, buffer);
        }

        // Token: 0x06000C2F RID: 3119 RVA: 0x00021028 File Offset: 0x0001F228
        private Zenject.Internal.IDecoratorProvider TryGetDecoratorProvider(Type contractType)
        {
            Zenject.Internal.IDecoratorProvider result;
            if (this._decorators.TryGetValue(contractType, out result))
            {
                return result;
            }
            DiContainer[] ancestorContainers = this.AncestorContainers;
            for (int i = 0; i < ancestorContainers.Length; i++)
            {
                if (ancestorContainers[i]._decorators.TryGetValue(contractType, out result))
                {
                    return result;
                }
            }
            return null;
        }

        // Token: 0x06000C30 RID: 3120 RVA: 0x00021074 File Offset: 0x0001F274
        private int GetContainerHeirarchyDistance(DiContainer container)
        {
            return this.GetContainerHeirarchyDistance(container, 0).Value;
        }

        // Token: 0x06000C31 RID: 3121 RVA: 0x00021094 File Offset: 0x0001F294
        private int? GetContainerHeirarchyDistance(DiContainer container, int depth)
        {
            if (container == this)
            {
                return new int?(depth);
            }
            int? result = null;
            DiContainer[] parentContainers = this.ParentContainers;
            for (int i = 0; i < parentContainers.Length; i++)
            {
                int? containerHeirarchyDistance = parentContainers[i].GetContainerHeirarchyDistance(container, depth + 1);
                if (containerHeirarchyDistance != null && (result == null || containerHeirarchyDistance.Value < result.Value))
                {
                    result = containerHeirarchyDistance;
                }
            }
            return result;
        }

        // Token: 0x06000C32 RID: 3122 RVA: 0x000210FC File Offset: 0x0001F2FC
        public IEnumerable<Type> GetDependencyContracts<TContract>()
        {
            return this.GetDependencyContracts(typeof(TContract));
        }

        // Token: 0x06000C33 RID: 3123 RVA: 0x00021110 File Offset: 0x0001F310
        public IEnumerable<Type> GetDependencyContracts(Type contract)
        {
            this.FlushBindings();
            InjectTypeInfo injectTypeInfo = TypeAnalyzer.TryGetInfo(contract);
            if (injectTypeInfo != null)
            {
                foreach (InjectableInfo injectableInfo in injectTypeInfo.AllInjectables)
                {
                    yield return injectableInfo.MemberType;
                }
            }
            yield break;
        }

        // Token: 0x06000C34 RID: 3124 RVA: 0x00021128 File Offset: 0x0001F328
        private object InstantiateInternal(Type concreteType, bool autoInject, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier)
        {
            ModestTree.Assert.That(!concreteType.DerivesFrom<Component>(), "Error occurred while instantiating object of type '{0}'. Instantiator should not be used to create new mono behaviours.  Must use InstantiatePrefabForComponent, InstantiatePrefab, or InstantiateComponent.", concreteType);
            ModestTree.Assert.That(!concreteType.IsAbstract(), "Expected type '{0}' to be non-abstract", concreteType);
            this.FlushBindings();
            this.CheckForInstallWarning(context);
            InjectTypeInfo injectTypeInfo = TypeAnalyzer.TryGetInfo(concreteType);
            ModestTree.Assert.IsNotNull(injectTypeInfo, "Tried to create type '{0}' but could not find type information", concreteType);
            bool flag = this.IsValidating && TypeAnalyzer.ShouldAllowDuringValidation(concreteType);
            object obj;
            if (concreteType.DerivesFrom<ScriptableObject>())
            {
                ModestTree.Assert.That(injectTypeInfo.InjectConstructor.Parameters.Length == 0, "Found constructor parameters on ScriptableObject type '{0}'.  This is not allowed.  Use an [Inject] method or fields instead.");
                if (!this.IsValidating || flag)
                {
                    obj = ScriptableObject.CreateInstance(concreteType);
                }
                else
                {
                    obj = new ValidationMarker(concreteType);
                }
            }
            else
            {
                ModestTree.Assert.IsNotNull(injectTypeInfo.InjectConstructor.Factory, "More than one (or zero) constructors found for type '{0}' when creating dependencies.  Use one [Inject] attribute to specify which to use.", concreteType);
                object[] array = Zenject.Internal.ZenPools.SpawnArray<object>(injectTypeInfo.InjectConstructor.Parameters.Length);
                try
                {
                    for (int i = 0; i < injectTypeInfo.InjectConstructor.Parameters.Length; i++)
                    {
                        InjectableInfo injectableInfo = injectTypeInfo.InjectConstructor.Parameters[i];
                        object obj2;
                        if (!InjectUtil.PopValueWithType(extraArgs, injectableInfo.MemberType, out obj2))
                        {
                            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, injectableInfo, context, null, concreteType, concreteIdentifier))
                            {
                                obj2 = this.Resolve(injectContext);
                            }
                        }
                        if (obj2 == null || obj2 is ValidationMarker)
                        {
                            array[i] = injectableInfo.MemberType.GetDefaultValue();
                        }
                        else
                        {
                            array[i] = obj2;
                        }
                    }
                    if (!this.IsValidating || flag)
                    {
                        try
                        {
                            obj = injectTypeInfo.InjectConstructor.Factory(array);
                            goto IL_19D;
                        }
                        catch (Exception innerException)
                        {
                            throw ModestTree.Assert.CreateException(innerException, "Error occurred while instantiating object with type '{0}'", new object[]
                            {
                                concreteType
                            });
                        }
                    }
                    obj = new ValidationMarker(concreteType);
                }
                finally
                {
                    Zenject.Internal.ZenPools.DespawnArray<object>(array);
                }
            }
        IL_19D:
            if (autoInject)
            {
                this.InjectExplicit(obj, concreteType, extraArgs, context, concreteIdentifier);
                if (extraArgs.Count > 0 && !(obj is ValidationMarker))
                {
                    string message = "Passed unnecessary parameters when injecting into type '{0}'. \nExtra Parameters: {1}\nObject graph:\n{2}";
                    object[] array2 = new object[3];
                    array2[0] = obj.GetType();
                    array2[1] = string.Join(",", (from x in extraArgs
                                                  select x.Type.PrettyName()).ToArray<string>());
                    array2[2] = context.GetObjectGraphString();
                    throw ModestTree.Assert.CreateException(message, array2);
                }
            }
            return obj;
        }

        // Token: 0x06000C35 RID: 3125 RVA: 0x00021378 File Offset: 0x0001F578
        public void InjectExplicit(object injectable, List<TypeValuePair> extraArgs)
        {
            Type type;
            if (injectable is ValidationMarker)
            {
                type = ((ValidationMarker)injectable).MarkedType;
            }
            else
            {
                type = injectable.GetType();
            }
            this.InjectExplicit(injectable, type, extraArgs, new InjectContext(this, type, null), null);
        }

        // Token: 0x06000C36 RID: 3126 RVA: 0x000213B4 File Offset: 0x0001F5B4
        public void InjectExplicit(object injectable, Type injectableType, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier)
        {
            if (this.IsValidating)
            {
                ValidationMarker validationMarker = injectable as ValidationMarker;
                if (validationMarker != null && validationMarker.InstantiateFailed)
                {
                    return;
                }
                if (this._settings.ValidationErrorResponse == ValidationErrorResponses.Throw)
                {
                    this.InjectExplicitInternal(injectable, injectableType, extraArgs, context, concreteIdentifier);
                    return;
                }
                try
                {
                    this.InjectExplicitInternal(injectable, injectableType, extraArgs, context, concreteIdentifier);
                    return;
                }
                catch (Exception e)
                {
                    ModestTree.Log.ErrorException(e);
                    return;
                }
            }
            this.InjectExplicitInternal(injectable, injectableType, extraArgs, context, concreteIdentifier);
        }

        // Token: 0x06000C37 RID: 3127 RVA: 0x0002142C File Offset: 0x0001F62C
        private void CallInjectMethodsTopDown(object injectable, Type injectableType, InjectTypeInfo typeInfo, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier, bool isDryRun)
        {
            if (typeInfo.BaseTypeInfo != null)
            {
                this.CallInjectMethodsTopDown(injectable, injectableType, typeInfo.BaseTypeInfo, extraArgs, context, concreteIdentifier, isDryRun);
            }
            for (int i = 0; i < typeInfo.InjectMethods.Length; i++)
            {
                InjectTypeInfo.InjectMethodInfo injectMethodInfo = typeInfo.InjectMethods[i];
                object[] array = Zenject.Internal.ZenPools.SpawnArray<object>(injectMethodInfo.Parameters.Length);
                try
                {
                    for (int j = 0; j < injectMethodInfo.Parameters.Length; j++)
                    {
                        InjectableInfo injectableInfo = injectMethodInfo.Parameters[j];
                        object obj;
                        if (!InjectUtil.PopValueWithType(extraArgs, injectableInfo.MemberType, out obj))
                        {
                            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, injectableInfo, context, injectable, injectableType, concreteIdentifier))
                            {
                                obj = this.Resolve(injectContext);
                            }
                        }
                        if (obj is ValidationMarker)
                        {
                            ModestTree.Assert.That(this.IsValidating);
                            array[j] = injectableInfo.MemberType.GetDefaultValue();
                        }
                        else
                        {
                            array[j] = obj;
                        }
                    }
                    if (!isDryRun)
                    {
                        injectMethodInfo.Action(injectable, array);
                    }
                }
                finally
                {
                    Zenject.Internal.ZenPools.DespawnArray<object>(array);
                }
            }
        }

        // Token: 0x06000C38 RID: 3128 RVA: 0x00021540 File Offset: 0x0001F740
        private void InjectMembersTopDown(object injectable, Type injectableType, InjectTypeInfo typeInfo, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier, bool isDryRun)
        {
            if (typeInfo.BaseTypeInfo != null)
            {
                this.InjectMembersTopDown(injectable, injectableType, typeInfo.BaseTypeInfo, extraArgs, context, concreteIdentifier, isDryRun);
            }
            for (int i = 0; i < typeInfo.InjectMembers.Length; i++)
            {
                InjectableInfo info = typeInfo.InjectMembers[i].Info;
                ZenMemberSetterMethod setter = typeInfo.InjectMembers[i].Setter;
                object obj;
                if (InjectUtil.PopValueWithType(extraArgs, info.MemberType, out obj))
                {
                    if (!isDryRun)
                    {
                        if (obj is ValidationMarker)
                        {
                            ModestTree.Assert.That(this.IsValidating);
                        }
                        else
                        {
                            setter(injectable, obj);
                        }
                    }
                }
                else
                {
                    using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, info, context, injectable, injectableType, concreteIdentifier))
                    {
                        obj = this.Resolve(injectContext);
                    }
                    if ((!info.Optional || obj != null) && !isDryRun)
                    {
                        if (obj is ValidationMarker)
                        {
                            ModestTree.Assert.That(this.IsValidating);
                        }
                        else
                        {
                            setter(injectable, obj);
                        }
                    }
                }
            }
        }

        // Token: 0x06000C39 RID: 3129 RVA: 0x00021638 File Offset: 0x0001F838
        private void InjectExplicitInternal(object injectable, Type injectableType, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier)
        {
            ModestTree.Assert.That(injectable != null);
            InjectTypeInfo injectTypeInfo = TypeAnalyzer.TryGetInfo(injectableType);
            if (injectTypeInfo == null)
            {
                ModestTree.Assert.That(extraArgs.IsEmpty<TypeValuePair>());
                return;
            }
            bool flag = this.IsValidating && TypeAnalyzer.ShouldAllowDuringValidation(injectableType);
            bool flag2 = this.IsValidating && !flag;
            if (!flag2)
            {
                ModestTree.Assert.IsEqual(injectable.GetType(), injectableType);
            }
            if (injectableType == typeof(GameObject))
            {
                ModestTree.Assert.CreateException("Use InjectGameObject to Inject game objects instead of Inject method. Object graph: {0}", new object[]
                {
                    context.GetObjectGraphString()
                });
            }
            this.FlushBindings();
            this.CheckForInstallWarning(context);
            this.InjectMembersTopDown(injectable, injectableType, injectTypeInfo, extraArgs, context, concreteIdentifier, flag2);
            this.CallInjectMethodsTopDown(injectable, injectableType, injectTypeInfo, extraArgs, context, concreteIdentifier, flag2);
            if (extraArgs.Count > 0)
            {
                string message = "Passed unnecessary parameters when injecting into type '{0}'. \nExtra Parameters: {1}\nObject graph:\n{2}";
                object[] array = new object[3];
                array[0] = injectableType;
                array[1] = string.Join(",", (from x in extraArgs
                                             select x.Type.PrettyName()).ToArray<string>());
                array[2] = context.GetObjectGraphString();
                throw ModestTree.Assert.CreateException(message, array);
            }
        }

        // Token: 0x06000C3A RID: 3130 RVA: 0x00021750 File Offset: 0x0001F950
        internal GameObject CreateAndParentPrefabResource(string resourcePath, GameObjectCreationParameters gameObjectBindInfo, InjectContext context, out bool shouldMakeActive)
        {
            GameObject gameObject = (GameObject)Resources.Load(resourcePath);
            ModestTree.Assert.IsNotNull(gameObject, "Could not find prefab at resource location '{0}'".Fmt(new object[]
            {
                resourcePath
            }));
            return this.CreateAndParentPrefab(gameObject, gameObjectBindInfo, context, out shouldMakeActive);
        }

        // Token: 0x06000C3B RID: 3131 RVA: 0x00021790 File Offset: 0x0001F990
        private GameObject GetPrefabAsGameObject(UnityEngine.Object prefab)
        {
            if (prefab is GameObject)
            {
                return (GameObject)prefab;
            }
            ModestTree.Assert.That(prefab is Component, "Invalid type given for prefab. Given object name: '{0}'", prefab.name);
            return ((Component)prefab).gameObject;
        }

        // Token: 0x06000C3C RID: 3132 RVA: 0x000217C8 File Offset: 0x0001F9C8
        internal GameObject CreateAndParentPrefab(UnityEngine.Object prefab, GameObjectCreationParameters gameObjectBindInfo, InjectContext context, out bool shouldMakeActive)
        {
            ModestTree.Assert.That(prefab != null, "Null prefab found when instantiating game object");
            ModestTree.Assert.That(!this.AssertOnNewGameObjects, "Given DiContainer does not support creating new game objects");
            this.FlushBindings();
            GameObject prefabAsGameObject = this.GetPrefabAsGameObject(prefab);
            bool activeSelf = prefabAsGameObject.activeSelf;
            shouldMakeActive = activeSelf;
            Transform transformGroup = this.GetTransformGroup(gameObjectBindInfo, context);
            if (activeSelf)
            {
                prefabAsGameObject.SetActive(false);
            }
            Transform parent;
            if (transformGroup != null)
            {
                parent = transformGroup;
            }
            else
            {
                parent = this.ContextTransform;
            }
            GameObject gameObject;
            bool worldPositionStays;
            if (gameObjectBindInfo.Position != null && gameObjectBindInfo.Rotation != null)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabAsGameObject, gameObjectBindInfo.Position.Value, gameObjectBindInfo.Rotation.Value, parent);
                worldPositionStays = true;
            }
            else if (gameObjectBindInfo.Position != null)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabAsGameObject, gameObjectBindInfo.Position.Value, prefabAsGameObject.transform.rotation, parent);
                worldPositionStays = true;
            }
            else if (gameObjectBindInfo.Rotation != null)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabAsGameObject, prefabAsGameObject.transform.position, gameObjectBindInfo.Rotation.Value, parent);
                worldPositionStays = true;
            }
            else
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabAsGameObject, parent);
                worldPositionStays = false;
            }
            if (activeSelf)
            {
                prefabAsGameObject.SetActive(true);
            }
            if (gameObject.transform.parent != transformGroup)
            {
                gameObject.transform.SetParent(transformGroup, worldPositionStays);
            }
            if (gameObjectBindInfo.Name != null)
            {
                gameObject.name = gameObjectBindInfo.Name;
            }
            return gameObject;
        }

        // Token: 0x06000C3D RID: 3133 RVA: 0x0002194C File Offset: 0x0001FB4C
        public GameObject CreateEmptyGameObject(string name)
        {
            return this.CreateEmptyGameObject(new GameObjectCreationParameters
            {
                Name = name
            }, null);
        }

        // Token: 0x06000C3E RID: 3134 RVA: 0x00021964 File Offset: 0x0001FB64
        public GameObject CreateEmptyGameObject(GameObjectCreationParameters gameObjectBindInfo, InjectContext context)
        {
            ModestTree.Assert.That(!this.AssertOnNewGameObjects, "Given DiContainer does not support creating new game objects");
            this.FlushBindings();
            GameObject gameObject = new GameObject(gameObjectBindInfo.Name ?? "GameObject");
            Transform transformGroup = this.GetTransformGroup(gameObjectBindInfo, context);
            if (transformGroup == null)
            {
                gameObject.transform.SetParent(this.ContextTransform, false);
                gameObject.transform.SetParent(null, false);
            }
            else
            {
                gameObject.transform.SetParent(transformGroup, false);
            }
            return gameObject;
        }

        // Token: 0x06000C3F RID: 3135 RVA: 0x000219E0 File Offset: 0x0001FBE0
        private Transform GetTransformGroup(GameObjectCreationParameters gameObjectBindInfo, InjectContext context)
        {
            ModestTree.Assert.That(!this.AssertOnNewGameObjects, "Given DiContainer does not support creating new game objects");
            if (gameObjectBindInfo.ParentTransform != null)
            {
                ModestTree.Assert.IsNull(gameObjectBindInfo.GroupName);
                ModestTree.Assert.IsNull(gameObjectBindInfo.ParentTransformGetter);
                return gameObjectBindInfo.ParentTransform;
            }
            if (gameObjectBindInfo.ParentTransformGetter != null && !this.IsValidating)
            {
                ModestTree.Assert.IsNull(gameObjectBindInfo.GroupName);
                if (context == null)
                {
                    context = new InjectContext
                    {
                        Container = this
                    };
                }
                return gameObjectBindInfo.ParentTransformGetter(context);
            }
            string groupName = gameObjectBindInfo.GroupName;
            Transform transform = this._hasExplicitDefaultParent ? this._explicitDefaultParent : this._inheritedDefaultParent;
            if (transform == null)
            {
                if (groupName == null)
                {
                    return null;
                }
                return (GameObject.Find("/" + groupName) ?? this.CreateTransformGroup(groupName)).transform;
            }
            else
            {
                if (groupName == null)
                {
                    return transform;
                }
                foreach (object obj in transform)
                {
                    Transform transform2 = (Transform)obj;
                    if (transform2.name == groupName)
                    {
                        return transform2;
                    }
                }
                Transform transform3 = new GameObject(groupName).transform;
                transform3.SetParent(transform, false);
                return transform3;
            }
        }

        // Token: 0x06000C40 RID: 3136 RVA: 0x00021B24 File Offset: 0x0001FD24
        private GameObject CreateTransformGroup(string groupName)
        {
            GameObject gameObject = new GameObject(groupName);
            gameObject.transform.SetParent(this.ContextTransform, false);
            gameObject.transform.SetParent(null, false);
            return gameObject;
        }

        // Token: 0x06000C41 RID: 3137 RVA: 0x00021B4C File Offset: 0x0001FD4C
        public T Instantiate<T>()
        {
            return this.Instantiate<T>(new object[0]);
        }

        // Token: 0x06000C42 RID: 3138 RVA: 0x00021B5C File Offset: 0x0001FD5C
        public T Instantiate<T>(IEnumerable<object> extraArgs)
        {
            object obj = this.Instantiate(typeof(T), extraArgs);
            if (this.IsValidating && !(obj is T))
            {
                ModestTree.Assert.That(obj is ValidationMarker);
                return default(T);
            }
            return (T)((object)obj);
        }

        // Token: 0x06000C43 RID: 3139 RVA: 0x00021BAC File Offset: 0x0001FDAC
        public object Instantiate(Type concreteType)
        {
            return this.Instantiate(concreteType, new object[0]);
        }

        // Token: 0x06000C44 RID: 3140 RVA: 0x00021BBC File Offset: 0x0001FDBC
        public object Instantiate(Type concreteType, IEnumerable<object> extraArgs)
        {
            ModestTree.Assert.That(!extraArgs.ContainsItem(null), "Null value given to factory constructor arguments when instantiating object with type '{0}'. In order to use null use InstantiateExplicit", concreteType);
            return this.InstantiateExplicit(concreteType, InjectUtil.CreateArgList(extraArgs));
        }

        // Token: 0x06000C45 RID: 3141 RVA: 0x00021BE0 File Offset: 0x0001FDE0
        public TContract InstantiateComponent<TContract>(GameObject gameObject) where TContract : Component
        {
            return this.InstantiateComponent<TContract>(gameObject, new object[0]);
        }

        // Token: 0x06000C46 RID: 3142 RVA: 0x00021BF0 File Offset: 0x0001FDF0
        public TContract InstantiateComponent<TContract>(GameObject gameObject, IEnumerable<object> extraArgs) where TContract : Component
        {
            return (TContract)((object)this.InstantiateComponent(typeof(TContract), gameObject, extraArgs));
        }

        // Token: 0x06000C47 RID: 3143 RVA: 0x00021C0C File Offset: 0x0001FE0C
        public Component InstantiateComponent(Type componentType, GameObject gameObject)
        {
            return this.InstantiateComponent(componentType, gameObject, new object[0]);
        }

        // Token: 0x06000C48 RID: 3144 RVA: 0x00021C1C File Offset: 0x0001FE1C
        public Component InstantiateComponent(Type componentType, GameObject gameObject, IEnumerable<object> extraArgs)
        {
            return this.InstantiateComponentExplicit(componentType, gameObject, InjectUtil.CreateArgList(extraArgs));
        }

        // Token: 0x06000C49 RID: 3145 RVA: 0x00021C2C File Offset: 0x0001FE2C
        public T InstantiateComponentOnNewGameObject<T>() where T : Component
        {
            return this.InstantiateComponentOnNewGameObject<T>(typeof(T).Name);
        }

        // Token: 0x06000C4A RID: 3146 RVA: 0x00021C44 File Offset: 0x0001FE44
        public T InstantiateComponentOnNewGameObject<T>(IEnumerable<object> extraArgs) where T : Component
        {
            return this.InstantiateComponentOnNewGameObject<T>(typeof(T).Name, extraArgs);
        }

        // Token: 0x06000C4B RID: 3147 RVA: 0x00021C5C File Offset: 0x0001FE5C
        public T InstantiateComponentOnNewGameObject<T>(string gameObjectName) where T : Component
        {
            return this.InstantiateComponentOnNewGameObject<T>(gameObjectName, new object[0]);
        }

        // Token: 0x06000C4C RID: 3148 RVA: 0x00021C6C File Offset: 0x0001FE6C
        public T InstantiateComponentOnNewGameObject<T>(string gameObjectName, IEnumerable<object> extraArgs) where T : Component
        {
            return this.InstantiateComponent<T>(this.CreateEmptyGameObject(gameObjectName), extraArgs);
        }

        // Token: 0x06000C4D RID: 3149 RVA: 0x00021C7C File Offset: 0x0001FE7C
        public GameObject InstantiatePrefab(UnityEngine.Object prefab)
        {
            return this.InstantiatePrefab(prefab, GameObjectCreationParameters.Default);
        }

        // Token: 0x06000C4E RID: 3150 RVA: 0x00021C8C File Offset: 0x0001FE8C
        public GameObject InstantiatePrefab(UnityEngine.Object prefab, Transform parentTransform)
        {
            return this.InstantiatePrefab(prefab, new GameObjectCreationParameters
            {
                ParentTransform = parentTransform
            });
        }

        // Token: 0x06000C4F RID: 3151 RVA: 0x00021CA4 File Offset: 0x0001FEA4
        public GameObject InstantiatePrefab(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            return this.InstantiatePrefab(prefab, new GameObjectCreationParameters
            {
                ParentTransform = parentTransform,
                Position = new Vector3?(position),
                Rotation = new Quaternion?(rotation)
            });
        }

        // Token: 0x06000C50 RID: 3152 RVA: 0x00021CD4 File Offset: 0x0001FED4
        public GameObject InstantiatePrefab(UnityEngine.Object prefab, GameObjectCreationParameters gameObjectBindInfo)
        {
            this.FlushBindings();
            bool flag;
            GameObject gameObject = this.CreateAndParentPrefab(prefab, gameObjectBindInfo, null, out flag);
            this.InjectGameObject(gameObject);
            if (flag && !this.IsValidating)
            {
                gameObject.SetActive(true);
            }
            return gameObject;
        }

        // Token: 0x06000C51 RID: 3153 RVA: 0x00021D10 File Offset: 0x0001FF10
        public GameObject InstantiatePrefabResource(string resourcePath)
        {
            return this.InstantiatePrefabResource(resourcePath, GameObjectCreationParameters.Default);
        }

        // Token: 0x06000C52 RID: 3154 RVA: 0x00021D20 File Offset: 0x0001FF20
        public GameObject InstantiatePrefabResource(string resourcePath, Transform parentTransform)
        {
            return this.InstantiatePrefabResource(resourcePath, new GameObjectCreationParameters
            {
                ParentTransform = parentTransform
            });
        }

        // Token: 0x06000C53 RID: 3155 RVA: 0x00021D38 File Offset: 0x0001FF38
        public GameObject InstantiatePrefabResource(string resourcePath, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            return this.InstantiatePrefabResource(resourcePath, new GameObjectCreationParameters
            {
                ParentTransform = parentTransform,
                Position = new Vector3?(position),
                Rotation = new Quaternion?(rotation)
            });
        }

        // Token: 0x06000C54 RID: 3156 RVA: 0x00021D68 File Offset: 0x0001FF68
        public GameObject InstantiatePrefabResource(string resourcePath, GameObjectCreationParameters creationInfo)
        {
            GameObject gameObject = (GameObject)Resources.Load(resourcePath);
            ModestTree.Assert.IsNotNull(gameObject, "Could not find prefab at resource location '{0}'".Fmt(new object[]
            {
                resourcePath
            }));
            return this.InstantiatePrefab(gameObject, creationInfo);
        }

        // Token: 0x06000C55 RID: 3157 RVA: 0x00021DA4 File Offset: 0x0001FFA4
        public T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab)
        {
            return (T)((object)this.InstantiatePrefabForComponent(typeof(T), prefab, null, new object[0]));
        }

        // Token: 0x06000C56 RID: 3158 RVA: 0x00021DC4 File Offset: 0x0001FFC4
        public T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, IEnumerable<object> extraArgs)
        {
            return (T)((object)this.InstantiatePrefabForComponent(typeof(T), prefab, null, extraArgs));
        }

        // Token: 0x06000C57 RID: 3159 RVA: 0x00021DE0 File Offset: 0x0001FFE0
        public T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Transform parentTransform)
        {
            return (T)((object)this.InstantiatePrefabForComponent(typeof(T), prefab, parentTransform, new object[0]));
        }

        // Token: 0x06000C58 RID: 3160 RVA: 0x00021E00 File Offset: 0x00020000
        public T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Transform parentTransform, IEnumerable<object> extraArgs)
        {
            return (T)((object)this.InstantiatePrefabForComponent(typeof(T), prefab, parentTransform, extraArgs));
        }

        // Token: 0x06000C59 RID: 3161 RVA: 0x00021E1C File Offset: 0x0002001C
        public T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            return (T)((object)this.InstantiatePrefabForComponent(typeof(T), prefab, new object[0], new GameObjectCreationParameters
            {
                ParentTransform = parentTransform,
                Position = new Vector3?(position),
                Rotation = new Quaternion?(rotation)
            }));
        }

        // Token: 0x06000C5A RID: 3162 RVA: 0x00021E6C File Offset: 0x0002006C
        public T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform, IEnumerable<object> extraArgs)
        {
            return (T)((object)this.InstantiatePrefabForComponent(typeof(T), prefab, extraArgs, new GameObjectCreationParameters
            {
                ParentTransform = parentTransform,
                Position = new Vector3?(position),
                Rotation = new Quaternion?(rotation)
            }));
        }

        // Token: 0x06000C5B RID: 3163 RVA: 0x00021EAC File Offset: 0x000200AC
        public object InstantiatePrefabForComponent(Type concreteType, UnityEngine.Object prefab, Transform parentTransform, IEnumerable<object> extraArgs)
        {
            return this.InstantiatePrefabForComponent(concreteType, prefab, extraArgs, new GameObjectCreationParameters
            {
                ParentTransform = parentTransform
            });
        }

        // Token: 0x06000C5C RID: 3164 RVA: 0x00021EC4 File Offset: 0x000200C4
        public object InstantiatePrefabForComponent(Type concreteType, UnityEngine.Object prefab, IEnumerable<object> extraArgs, GameObjectCreationParameters creationInfo)
        {
            return this.InstantiatePrefabForComponentExplicit(concreteType, prefab, InjectUtil.CreateArgList(extraArgs), creationInfo);
        }

        // Token: 0x06000C5D RID: 3165 RVA: 0x00021ED8 File Offset: 0x000200D8
        public T InstantiatePrefabResourceForComponent<T>(string resourcePath)
        {
            return (T)((object)this.InstantiatePrefabResourceForComponent(typeof(T), resourcePath, null, new object[0]));
        }

        // Token: 0x06000C5E RID: 3166 RVA: 0x00021EF8 File Offset: 0x000200F8
        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, IEnumerable<object> extraArgs)
        {
            return (T)((object)this.InstantiatePrefabResourceForComponent(typeof(T), resourcePath, null, extraArgs));
        }

        // Token: 0x06000C5F RID: 3167 RVA: 0x00021F14 File Offset: 0x00020114
        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform)
        {
            return (T)((object)this.InstantiatePrefabResourceForComponent(typeof(T), resourcePath, parentTransform, new object[0]));
        }

        // Token: 0x06000C60 RID: 3168 RVA: 0x00021F34 File Offset: 0x00020134
        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform, IEnumerable<object> extraArgs)
        {
            return (T)((object)this.InstantiatePrefabResourceForComponent(typeof(T), resourcePath, parentTransform, extraArgs));
        }

        // Token: 0x06000C61 RID: 3169 RVA: 0x00021F50 File Offset: 0x00020150
        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            return this.InstantiatePrefabResourceForComponent<T>(resourcePath, position, rotation, parentTransform, new object[0]);
        }

        // Token: 0x06000C62 RID: 3170 RVA: 0x00021F64 File Offset: 0x00020164
        public T InstantiatePrefabResourceForComponent<T>(string resourcePath, Vector3 position, Quaternion rotation, Transform parentTransform, IEnumerable<object> extraArgs)
        {
            List<TypeValuePair> extraArgs2 = InjectUtil.CreateArgList(extraArgs);
            GameObjectCreationParameters creationInfo = new GameObjectCreationParameters
            {
                ParentTransform = parentTransform,
                Position = new Vector3?(position),
                Rotation = new Quaternion?(rotation)
            };
            return (T)((object)this.InstantiatePrefabResourceForComponentExplicit(typeof(T), resourcePath, extraArgs2, creationInfo));
        }

        // Token: 0x06000C63 RID: 3171 RVA: 0x00021FB8 File Offset: 0x000201B8
        public object InstantiatePrefabResourceForComponent(Type concreteType, string resourcePath, Transform parentTransform, IEnumerable<object> extraArgs)
        {
            ModestTree.Assert.That(!extraArgs.ContainsItem(null), "Null value given to factory constructor arguments when instantiating object with type '{0}'. In order to use null use InstantiatePrefabForComponentExplicit", concreteType);
            return this.InstantiatePrefabResourceForComponentExplicit(concreteType, resourcePath, InjectUtil.CreateArgList(extraArgs), new GameObjectCreationParameters
            {
                ParentTransform = parentTransform
            });
        }

        // Token: 0x06000C64 RID: 3172 RVA: 0x00021FEC File Offset: 0x000201EC
        public T InstantiateScriptableObjectResource<T>(string resourcePath) where T : ScriptableObject
        {
            return this.InstantiateScriptableObjectResource<T>(resourcePath, new object[0]);
        }

        // Token: 0x06000C65 RID: 3173 RVA: 0x00021FFC File Offset: 0x000201FC
        public T InstantiateScriptableObjectResource<T>(string resourcePath, IEnumerable<object> extraArgs) where T : ScriptableObject
        {
            return (T)((object)this.InstantiateScriptableObjectResource(typeof(T), resourcePath, extraArgs));
        }

        // Token: 0x06000C66 RID: 3174 RVA: 0x00022018 File Offset: 0x00020218
        public object InstantiateScriptableObjectResource(Type scriptableObjectType, string resourcePath)
        {
            return this.InstantiateScriptableObjectResource(scriptableObjectType, resourcePath, new object[0]);
        }

        // Token: 0x06000C67 RID: 3175 RVA: 0x00022028 File Offset: 0x00020228
        public object InstantiateScriptableObjectResource(Type scriptableObjectType, string resourcePath, IEnumerable<object> extraArgs)
        {
            ModestTree.Assert.DerivesFromOrEqual<ScriptableObject>(scriptableObjectType);
            return this.InstantiateScriptableObjectResourceExplicit(scriptableObjectType, resourcePath, InjectUtil.CreateArgList(extraArgs));
        }

        // Token: 0x06000C68 RID: 3176 RVA: 0x00022040 File Offset: 0x00020240
        public void InjectGameObject(GameObject gameObject)
        {
            this.FlushBindings();
            Zenject.Internal.ZenUtilInternal.AddStateMachineBehaviourAutoInjectersUnderGameObject(gameObject);
            List<MonoBehaviour> list = Zenject.Internal.ZenPools.SpawnList<MonoBehaviour>();
            try
            {
                Zenject.Internal.ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(gameObject, list);
                for (int i = 0; i < list.Count; i++)
                {
                    this.Inject(list[i]);
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<MonoBehaviour>(list);
            }
        }

        // Token: 0x06000C69 RID: 3177 RVA: 0x000220A0 File Offset: 0x000202A0
        public T InjectGameObjectForComponent<T>(GameObject gameObject) where T : Component
        {
            return this.InjectGameObjectForComponent<T>(gameObject, new object[0]);
        }

        // Token: 0x06000C6A RID: 3178 RVA: 0x000220B0 File Offset: 0x000202B0
        public T InjectGameObjectForComponent<T>(GameObject gameObject, IEnumerable<object> extraArgs) where T : Component
        {
            return (T)((object)this.InjectGameObjectForComponent(gameObject, typeof(T), extraArgs));
        }

        // Token: 0x06000C6B RID: 3179 RVA: 0x000220CC File Offset: 0x000202CC
        public object InjectGameObjectForComponent(GameObject gameObject, Type componentType, IEnumerable<object> extraArgs)
        {
            return this.InjectGameObjectForComponentExplicit(gameObject, componentType, InjectUtil.CreateArgList(extraArgs), new InjectContext(this, componentType, null), null);
        }

        // Token: 0x06000C6C RID: 3180 RVA: 0x000220E8 File Offset: 0x000202E8
        public Component InjectGameObjectForComponentExplicit(GameObject gameObject, Type componentType, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier)
        {
            if (!componentType.DerivesFrom<MonoBehaviour>() && extraArgs.Count > 0)
            {
                throw ModestTree.Assert.CreateException("Cannot inject into non-monobehaviours!  Argument list must be zero length");
            }
            Zenject.Internal.ZenUtilInternal.AddStateMachineBehaviourAutoInjectersUnderGameObject(gameObject);
            List<MonoBehaviour> list = Zenject.Internal.ZenPools.SpawnList<MonoBehaviour>();
            try
            {
                Zenject.Internal.ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(gameObject, list);
                for (int i = 0; i < list.Count; i++)
                {
                    MonoBehaviour monoBehaviour = list[i];
                    if (monoBehaviour.GetType().DerivesFromOrEqual(componentType))
                    {
                        this.InjectExplicit(monoBehaviour, monoBehaviour.GetType(), extraArgs, context, concreteIdentifier);
                    }
                    else
                    {
                        this.Inject(monoBehaviour);
                    }
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<MonoBehaviour>(list);
            }
            Component[] componentsInChildren = gameObject.GetComponentsInChildren(componentType, true);
            ModestTree.Assert.That(componentsInChildren.Length != 0, "Expected to find component with type '{0}' when injecting into game object '{1}'", componentType, gameObject.name);
            ModestTree.Assert.That(componentsInChildren.Length == 1, "Found multiple component with type '{0}' when injecting into game object '{1}'", componentType, gameObject.name);
            return componentsInChildren[0];
        }

        // Token: 0x06000C6D RID: 3181 RVA: 0x000221B8 File Offset: 0x000203B8
        public void Inject(object injectable)
        {
            this.Inject(injectable, new object[0]);
        }

        // Token: 0x06000C6E RID: 3182 RVA: 0x000221C8 File Offset: 0x000203C8
        public void Inject(object injectable, IEnumerable<object> extraArgs)
        {
            this.InjectExplicit(injectable, InjectUtil.CreateArgList(extraArgs));
        }

        // Token: 0x06000C6F RID: 3183 RVA: 0x000221D8 File Offset: 0x000203D8
        public TContract Resolve<TContract>()
        {
            return (TContract)((object)this.Resolve(typeof(TContract)));
        }

        // Token: 0x06000C70 RID: 3184 RVA: 0x000221F0 File Offset: 0x000203F0
        public object Resolve(Type contractType)
        {
            return this.ResolveId(contractType, null);
        }

        // Token: 0x06000C71 RID: 3185 RVA: 0x000221FC File Offset: 0x000203FC
        public TContract ResolveId<TContract>(object identifier)
        {
            return (TContract)((object)this.ResolveId(typeof(TContract), identifier));
        }

        // Token: 0x06000C72 RID: 3186 RVA: 0x00022214 File Offset: 0x00020414
        public object ResolveId(Type contractType, object identifier)
        {
            object result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, contractType))
            {
                injectContext.Identifier = identifier;
                result = this.Resolve(injectContext);
            }
            return result;
        }

        // Token: 0x06000C73 RID: 3187 RVA: 0x00022258 File Offset: 0x00020458
        public TContract TryResolve<TContract>() where TContract : class
        {
            return (TContract)((object)this.TryResolve(typeof(TContract)));
        }

        // Token: 0x06000C74 RID: 3188 RVA: 0x00022270 File Offset: 0x00020470
        public object TryResolve(Type contractType)
        {
            return this.TryResolveId(contractType, null);
        }

        // Token: 0x06000C75 RID: 3189 RVA: 0x0002227C File Offset: 0x0002047C
        public TContract TryResolveId<TContract>(object identifier) where TContract : class
        {
            return (TContract)((object)this.TryResolveId(typeof(TContract), identifier));
        }

        // Token: 0x06000C76 RID: 3190 RVA: 0x00022294 File Offset: 0x00020494
        public object TryResolveId(Type contractType, object identifier)
        {
            object result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, contractType))
            {
                injectContext.Identifier = identifier;
                injectContext.Optional = true;
                result = this.Resolve(injectContext);
            }
            return result;
        }

        // Token: 0x06000C77 RID: 3191 RVA: 0x000222DC File Offset: 0x000204DC
        public List<TContract> ResolveAll<TContract>()
        {
            return (List<TContract>)this.ResolveAll(typeof(TContract));
        }

        // Token: 0x06000C78 RID: 3192 RVA: 0x000222F4 File Offset: 0x000204F4
        public IList ResolveAll(Type contractType)
        {
            return this.ResolveIdAll(contractType, null);
        }

        // Token: 0x06000C79 RID: 3193 RVA: 0x00022300 File Offset: 0x00020500
        public List<TContract> ResolveIdAll<TContract>(object identifier)
        {
            return (List<TContract>)this.ResolveIdAll(typeof(TContract), identifier);
        }

        // Token: 0x06000C7A RID: 3194 RVA: 0x00022318 File Offset: 0x00020518
        public IList ResolveIdAll(Type contractType, object identifier)
        {
            IList result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, contractType))
            {
                injectContext.Identifier = identifier;
                injectContext.Optional = true;
                result = this.ResolveAll(injectContext);
            }
            return result;
        }

        // Token: 0x06000C7B RID: 3195 RVA: 0x00022360 File Offset: 0x00020560
        public void UnbindAll()
        {
            this.FlushBindings();
            this._providers.Clear();
        }

        // Token: 0x06000C7C RID: 3196 RVA: 0x00022374 File Offset: 0x00020574
        public bool Unbind<TContract>()
        {
            return this.Unbind(typeof(TContract));
        }

        // Token: 0x06000C7D RID: 3197 RVA: 0x00022388 File Offset: 0x00020588
        public bool Unbind(Type contractType)
        {
            return this.UnbindId(contractType, null);
        }

        // Token: 0x06000C7E RID: 3198 RVA: 0x00022394 File Offset: 0x00020594
        public bool UnbindId<TContract>(object identifier)
        {
            return this.UnbindId(typeof(TContract), identifier);
        }

        // Token: 0x06000C7F RID: 3199 RVA: 0x000223A8 File Offset: 0x000205A8
        public bool UnbindId(Type contractType, object identifier)
        {
            this.FlushBindings();
            BindingId key = new BindingId(contractType, identifier);
            return this._providers.Remove(key);
        }

        // Token: 0x06000C80 RID: 3200 RVA: 0x000223D0 File Offset: 0x000205D0
        public void UnbindInterfacesTo<TConcrete>()
        {
            this.UnbindInterfacesTo(typeof(TConcrete));
        }

        // Token: 0x06000C81 RID: 3201 RVA: 0x000223E4 File Offset: 0x000205E4
        public void UnbindInterfacesTo(Type concreteType)
        {
            foreach (Type contractType in concreteType.Interfaces())
            {
                this.Unbind(contractType, concreteType);
            }
        }

        // Token: 0x06000C82 RID: 3202 RVA: 0x00022414 File Offset: 0x00020614
        public bool Unbind<TContract, TConcrete>()
        {
            return this.Unbind(typeof(TContract), typeof(TConcrete));
        }

        // Token: 0x06000C83 RID: 3203 RVA: 0x00022430 File Offset: 0x00020630
        public bool Unbind(Type contractType, Type concreteType)
        {
            return this.UnbindId(contractType, concreteType, null);
        }

        // Token: 0x06000C84 RID: 3204 RVA: 0x0002243C File Offset: 0x0002063C
        public bool UnbindId<TContract, TConcrete>(object identifier)
        {
            return this.UnbindId(typeof(TContract), typeof(TConcrete), identifier);
        }

        // Token: 0x06000C85 RID: 3205 RVA: 0x0002245C File Offset: 0x0002065C
        public bool UnbindId(Type contractType, Type concreteType, object identifier)
        {
            this.FlushBindings();
            BindingId key = new BindingId(contractType, identifier);
            List<DiContainer.ProviderInfo> list;
            if (!this._providers.TryGetValue(key, out list))
            {
                return false;
            }
            List<DiContainer.ProviderInfo> list2 = (from x in list
                                                    where x.Provider.GetInstanceType(new InjectContext(this, contractType, identifier)).DerivesFromOrEqual(concreteType)
                                                    select x).ToList<DiContainer.ProviderInfo>();
            if (list2.Count == 0)
            {
                return false;
            }
            foreach (DiContainer.ProviderInfo item in list2)
            {
                ModestTree.Assert.That(list.Remove(item));
            }
            return true;
        }

        // Token: 0x06000C86 RID: 3206 RVA: 0x00022524 File Offset: 0x00020724
        public bool HasBinding<TContract>()
        {
            return this.HasBinding(typeof(TContract));
        }

        // Token: 0x06000C87 RID: 3207 RVA: 0x00022538 File Offset: 0x00020738
        public bool HasBinding(Type contractType)
        {
            return this.HasBindingId(contractType, null);
        }

        // Token: 0x06000C88 RID: 3208 RVA: 0x00022544 File Offset: 0x00020744
        public bool HasBindingId<TContract>(object identifier)
        {
            return this.HasBindingId(typeof(TContract), identifier);
        }

        // Token: 0x06000C89 RID: 3209 RVA: 0x00022558 File Offset: 0x00020758
        public bool HasBindingId(Type contractType, object identifier)
        {
            return this.HasBindingId(contractType, identifier, InjectSources.Any);
        }

        // Token: 0x06000C8A RID: 3210 RVA: 0x00022564 File Offset: 0x00020764
        public bool HasBindingId(Type contractType, object identifier, InjectSources sourceType)
        {
            bool result;
            using (InjectContext injectContext = Zenject.Internal.ZenPools.SpawnInjectContext(this, contractType))
            {
                injectContext.Identifier = identifier;
                injectContext.SourceType = sourceType;
                result = this.HasBinding(injectContext);
            }
            return result;
        }

        // Token: 0x06000C8B RID: 3211 RVA: 0x000225AC File Offset: 0x000207AC
        public bool HasBinding(InjectContext context)
        {
            ModestTree.Assert.IsNotNull(context);
            this.FlushBindings();
            List<DiContainer.ProviderInfo> list = Zenject.Internal.ZenPools.SpawnList<DiContainer.ProviderInfo>();
            bool result;
            try
            {
                this.GetProviderMatches(context, list);
                result = (list.Count > 0);
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<DiContainer.ProviderInfo>(list);
            }
            return result;
        }

        // Token: 0x06000C8C RID: 3212 RVA: 0x000225F8 File Offset: 0x000207F8
        public void FlushBindings()
        {
            while (this._currentBindings.Count > 0)
            {
                BindStatement bindStatement = this._currentBindings.Dequeue();
                if (bindStatement.BindingInheritanceMethod != BindingInheritanceMethods.MoveDirectOnly && bindStatement.BindingInheritanceMethod != BindingInheritanceMethods.MoveIntoAll)
                {
                    this.FinalizeBinding(bindStatement);
                }
                if (bindStatement.BindingInheritanceMethod != BindingInheritanceMethods.None)
                {
                    this._childBindings.Add(bindStatement);
                }
                else
                {
                    bindStatement.Dispose();
                }
            }
        }

        // Token: 0x06000C8D RID: 3213 RVA: 0x00022658 File Offset: 0x00020858
        private void FinalizeBinding(BindStatement binding)
        {
            this._isFinalizingBinding = true;
            try
            {
                binding.FinalizeBinding(this);
            }
            finally
            {
                this._isFinalizingBinding = false;
            }
        }

        // Token: 0x06000C8E RID: 3214 RVA: 0x00022690 File Offset: 0x00020890
        public BindStatement StartBinding(bool flush = true)
        {
            ModestTree.Assert.That(!this._isFinalizingBinding, "Attempted to start a binding during a binding finalizer.  This is not allowed, since binding finalizers should directly use AddProvider instead, to allow for bindings to be inherited properly without duplicates");
            if (flush)
            {
                this.FlushBindings();
            }
            BindStatement bindStatement = Zenject.Internal.ZenPools.SpawnStatement();
            this._currentBindings.Enqueue(bindStatement);
            return bindStatement;
        }

        // Token: 0x06000C8F RID: 3215 RVA: 0x000226CC File Offset: 0x000208CC
        public ConcreteBinderGeneric<TContract> Rebind<TContract>()
        {
            return this.RebindId<TContract>(null);
        }

        // Token: 0x06000C90 RID: 3216 RVA: 0x000226D8 File Offset: 0x000208D8
        public ConcreteBinderGeneric<TContract> RebindId<TContract>(object identifier)
        {
            this.UnbindId<TContract>(identifier);
            return this.Bind<TContract>().WithId(identifier);
        }

        // Token: 0x06000C91 RID: 3217 RVA: 0x000226F0 File Offset: 0x000208F0
        public ConcreteBinderNonGeneric Rebind(Type contractType)
        {
            return this.RebindId(contractType, null);
        }

        // Token: 0x06000C92 RID: 3218 RVA: 0x000226FC File Offset: 0x000208FC
        public ConcreteBinderNonGeneric RebindId(Type contractType, object identifier)
        {
            this.UnbindId(contractType, identifier);
            return this.Bind(new Type[]
            {
                contractType
            }).WithId(identifier);
        }

        // Token: 0x06000C93 RID: 3219 RVA: 0x00022720 File Offset: 0x00020920
        public ConcreteIdBinderGeneric<TContract> Bind<TContract>()
        {
            return this.Bind<TContract>(this.StartBinding(true));
        }

        // Token: 0x06000C94 RID: 3220 RVA: 0x00022730 File Offset: 0x00020930
        public ConcreteIdBinderGeneric<TContract> BindNoFlush<TContract>()
        {
            return this.Bind<TContract>(this.StartBinding(false));
        }

        // Token: 0x06000C95 RID: 3221 RVA: 0x00022740 File Offset: 0x00020940
        private ConcreteIdBinderGeneric<TContract> Bind<TContract>(BindStatement bindStatement)
        {
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            ModestTree.Assert.That(!typeof(TContract).DerivesFrom<IPlaceholderFactory>(), "You should not use Container.Bind for factory classes.  Use Container.BindFactory instead.");
            ModestTree.Assert.That(!bindInfo.ContractTypes.Contains(typeof(TContract)));
            bindInfo.ContractTypes.Add(typeof(TContract));
            return new ConcreteIdBinderGeneric<TContract>(this, bindInfo, bindStatement);
        }

        // Token: 0x06000C96 RID: 3222 RVA: 0x000227AC File Offset: 0x000209AC
        public ConcreteIdBinderNonGeneric Bind(params Type[] contractTypes)
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.AllocFreeAddRange(contractTypes);
            return this.BindInternal(bindInfo, bindStatement);
        }

        // Token: 0x06000C97 RID: 3223 RVA: 0x000227DC File Offset: 0x000209DC
        public ConcreteIdBinderNonGeneric Bind(IEnumerable<Type> contractTypes)
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.AddRange(contractTypes);
            return this.BindInternal(bindInfo, bindStatement);
        }

        // Token: 0x06000C98 RID: 3224 RVA: 0x0002280C File Offset: 0x00020A0C
        private ConcreteIdBinderNonGeneric BindInternal(BindInfo bindInfo, BindStatement bindingFinalizer)
        {
            ModestTree.Assert.That(bindInfo.ContractTypes.All((Type x) => !x.DerivesFrom<IPlaceholderFactory>()), "You should not use Container.Bind for factory classes.  Use Container.BindFactory instead.");
            return new ConcreteIdBinderNonGeneric(this, bindInfo, bindingFinalizer);
        }

        // Token: 0x06000C99 RID: 3225 RVA: 0x0002284C File Offset: 0x00020A4C
        public ConcreteIdBinderNonGeneric Bind(Action<ConventionSelectTypesBinder> generator)
        {
            ConventionBindInfo conventionBindInfo = new ConventionBindInfo();
            generator(new ConventionSelectTypesBinder(conventionBindInfo));
            List<Type> list = conventionBindInfo.ResolveTypes();
            ModestTree.Assert.That(list.All((Type x) => !x.DerivesFrom<IPlaceholderFactory>()), "You should not use Container.Bind for factory classes.  Use Container.BindFactory instead.");
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.AllocFreeAddRange(list);
            bindInfo.InvalidBindResponse = InvalidBindResponses.Skip;
            return new ConcreteIdBinderNonGeneric(this, bindInfo, bindStatement);
        }

        // Token: 0x06000C9A RID: 3226 RVA: 0x000228CC File Offset: 0x00020ACC
        public FromBinderNonGeneric BindInterfacesTo<T>()
        {
            return this.BindInterfacesTo(typeof(T));
        }

        // Token: 0x06000C9B RID: 3227 RVA: 0x000228E0 File Offset: 0x00020AE0
        public FromBinderNonGeneric BindInterfacesTo(Type type)
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            Type[] array = type.Interfaces();
            if (array.Length == 0)
            {
                ModestTree.Log.Warn("Called BindInterfacesTo for type {0} but no interfaces were found", new object[]
                {
                    type
                });
            }
            bindInfo.ContractTypes.AllocFreeAddRange(array);
            bindInfo.RequireExplicitScope = true;
            return this.BindInternal(bindInfo, bindStatement).To(new Type[]
            {
                type
            });
        }

        // Token: 0x06000C9C RID: 3228 RVA: 0x00022948 File Offset: 0x00020B48
        public FromBinderNonGeneric BindInterfacesAndSelfTo<T>()
        {
            return this.BindInterfacesAndSelfTo(typeof(T));
        }

        // Token: 0x06000C9D RID: 3229 RVA: 0x0002295C File Offset: 0x00020B5C
        public FromBinderNonGeneric BindInterfacesAndSelfTo(Type type)
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.AllocFreeAddRange(type.Interfaces());
            bindInfo.ContractTypes.Add(type);
            bindInfo.RequireExplicitScope = true;
            return this.BindInternal(bindInfo, bindStatement).To(new Type[]
            {
                type
            });
        }

        // Token: 0x06000C9E RID: 3230 RVA: 0x000229B4 File Offset: 0x00020BB4
        public IdScopeConcreteIdArgConditionCopyNonLazyBinder BindInstance<TContract>(TContract instance)
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TContract));
            bindStatement.SetFinalizer(new ScopableBindingFinalizer(bindInfo, (DiContainer container, Type type) => new InstanceProvider(type, instance, container)));
            return new IdScopeConcreteIdArgConditionCopyNonLazyBinder(bindInfo);
        }

        // Token: 0x06000C9F RID: 3231 RVA: 0x00022A10 File Offset: 0x00020C10
        public void BindInstances(params object[] instances)
        {
            foreach (object obj in instances)
            {
                ModestTree.Assert.That(!Zenject.Internal.ZenUtilInternal.IsNull(obj), "Found null instance provided to BindInstances method");
                this.Bind(new Type[]
                {
                    obj.GetType()
                }).FromInstance(obj);
            }
        }

        // Token: 0x06000CA0 RID: 3232 RVA: 0x00022A60 File Offset: 0x00020C60
        private FactoryToChoiceIdBinder<TContract> BindFactoryInternal<TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CA1 RID: 3233 RVA: 0x00022AB4 File Offset: 0x00020CB4
        public FactoryToChoiceIdBinder<TContract> BindIFactory<TContract>()
        {
            return this.BindFactoryInternal<TContract, IFactory<TContract>, PlaceholderFactory<TContract>>();
        }

        // Token: 0x06000CA2 RID: 3234 RVA: 0x00022ABC File Offset: 0x00020CBC
        public FactoryToChoiceIdBinder<TContract> BindFactory<TContract, TFactory>() where TFactory : PlaceholderFactory<TContract>
        {
            return this.BindFactoryInternal<TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CA3 RID: 3235 RVA: 0x00022AC4 File Offset: 0x00020CC4
        public FactoryToChoiceIdBinder<TContract> BindFactoryCustomInterface<TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CA4 RID: 3236 RVA: 0x00022ACC File Offset: 0x00020CCC
        public MemoryPoolIdInitialSizeMaxSizeBinder<TItemContract> BindMemoryPool<TItemContract>()
        {
            return this.BindMemoryPool<TItemContract, MemoryPool<TItemContract>>();
        }

        // Token: 0x06000CA5 RID: 3237 RVA: 0x00022AD4 File Offset: 0x00020CD4
        public MemoryPoolIdInitialSizeMaxSizeBinder<TItemContract> BindMemoryPool<TItemContract, TPool>() where TPool : IMemoryPool
        {
            return this.BindMemoryPoolCustomInterface<TItemContract, TPool, TPool>(false);
        }

        // Token: 0x06000CA6 RID: 3238 RVA: 0x00022AE0 File Offset: 0x00020CE0
        public MemoryPoolIdInitialSizeMaxSizeBinder<TItemContract> BindMemoryPoolCustomInterface<TItemContract, TPoolConcrete, TPoolContract>(bool includeConcreteType = false) where TPoolConcrete : TPoolContract, IMemoryPool where TPoolContract : IMemoryPool
        {
            return this.BindMemoryPoolCustomInterfaceInternal<TItemContract, TPoolConcrete, TPoolContract>(includeConcreteType, this.StartBinding(true));
        }

        // Token: 0x06000CA7 RID: 3239 RVA: 0x00022AF0 File Offset: 0x00020CF0
        internal MemoryPoolIdInitialSizeMaxSizeBinder<TItemContract> BindMemoryPoolCustomInterfaceNoFlush<TItemContract, TPoolConcrete, TPoolContract>(bool includeConcreteType = false) where TPoolConcrete : TPoolContract, IMemoryPool where TPoolContract : IMemoryPool
        {
            return this.BindMemoryPoolCustomInterfaceInternal<TItemContract, TPoolConcrete, TPoolContract>(includeConcreteType, this.StartBinding(false));
        }

        // Token: 0x06000CA8 RID: 3240 RVA: 0x00022B00 File Offset: 0x00020D00
        private MemoryPoolIdInitialSizeMaxSizeBinder<TItemContract> BindMemoryPoolCustomInterfaceInternal<TItemContract, TPoolConcrete, TPoolContract>(bool includeConcreteType, BindStatement statement) where TPoolConcrete : TPoolContract, IMemoryPool where TPoolContract : IMemoryPool
        {
            List<Type> list = new List<Type>
            {
                typeof(IDisposable),
                typeof(TPoolContract)
            };
            if (includeConcreteType)
            {
                list.Add(typeof(TPoolConcrete));
            }
            BindInfo bindInfo = statement.SpawnBindInfo();
            bindInfo.ContractTypes.AllocFreeAddRange(list);
            bindInfo.ContractTypes.Add(typeof(IMemoryPool));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TPoolConcrete));
            MemoryPoolBindInfo poolBindInfo = new MemoryPoolBindInfo();
            statement.SetFinalizer(new MemoryPoolBindingFinalizer<TItemContract>(bindInfo, factoryBindInfo, poolBindInfo));
            return new MemoryPoolIdInitialSizeMaxSizeBinder<TItemContract>(this, bindInfo, factoryBindInfo, poolBindInfo);
        }

        // Token: 0x06000CA9 RID: 3241 RVA: 0x00022B9C File Offset: 0x00020D9C
        private FactoryToChoiceIdBinder<TParam1, TContract> BindFactoryInternal<TParam1, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CAA RID: 3242 RVA: 0x00022BF0 File Offset: 0x00020DF0
        public FactoryToChoiceIdBinder<TParam1, TContract> BindIFactory<TParam1, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TContract, IFactory<TParam1, TContract>, PlaceholderFactory<TParam1, TContract>>();
        }

        // Token: 0x06000CAB RID: 3243 RVA: 0x00022BF8 File Offset: 0x00020DF8
        public FactoryToChoiceIdBinder<TParam1, TContract> BindFactory<TParam1, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TContract>
        {
            return this.BindFactoryInternal<TParam1, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CAC RID: 3244 RVA: 0x00022C00 File Offset: 0x00020E00
        public FactoryToChoiceIdBinder<TParam1, TContract> BindFactoryCustomInterface<TParam1, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CAD RID: 3245 RVA: 0x00022C08 File Offset: 0x00020E08
        private FactoryToChoiceIdBinder<TParam1, TParam2, TContract> BindFactoryInternal<TParam1, TParam2, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TParam2, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CAE RID: 3246 RVA: 0x00022C5C File Offset: 0x00020E5C
        public FactoryToChoiceIdBinder<TParam1, TParam2, TContract> BindIFactory<TParam1, TParam2, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TParam2, TContract, IFactory<TParam1, TParam2, TContract>, PlaceholderFactory<TParam1, TParam2, TContract>>();
        }

        // Token: 0x06000CAF RID: 3247 RVA: 0x00022C64 File Offset: 0x00020E64
        public FactoryToChoiceIdBinder<TParam1, TParam2, TContract> BindFactory<TParam1, TParam2, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TParam2, TContract>
        {
            return this.BindFactoryInternal<TParam1, TParam2, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CB0 RID: 3248 RVA: 0x00022C6C File Offset: 0x00020E6C
        public FactoryToChoiceIdBinder<TParam1, TParam2, TContract> BindFactoryCustomInterface<TParam1, TParam2, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TParam2, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TParam2, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CB1 RID: 3249 RVA: 0x00022C74 File Offset: 0x00020E74
        private FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TContract> BindFactoryInternal<TParam1, TParam2, TParam3, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CB2 RID: 3250 RVA: 0x00022CC8 File Offset: 0x00020EC8
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TContract> BindIFactory<TParam1, TParam2, TParam3, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TContract, IFactory<TParam1, TParam2, TParam3, TContract>, PlaceholderFactory<TParam1, TParam2, TParam3, TContract>>();
        }

        // Token: 0x06000CB3 RID: 3251 RVA: 0x00022CD0 File Offset: 0x00020ED0
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TContract> BindFactory<TParam1, TParam2, TParam3, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TParam2, TParam3, TContract>
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CB4 RID: 3252 RVA: 0x00022CD8 File Offset: 0x00020ED8
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TContract> BindFactoryCustomInterface<TParam1, TParam2, TParam3, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TParam2, TParam3, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CB5 RID: 3253 RVA: 0x00022CE0 File Offset: 0x00020EE0
        private FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CB6 RID: 3254 RVA: 0x00022D34 File Offset: 0x00020F34
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TContract, IFactory<TParam1, TParam2, TParam3, TParam4, TContract>, PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TContract>>();
        }

        // Token: 0x06000CB7 RID: 3255 RVA: 0x00022D3C File Offset: 0x00020F3C
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindFactory<TParam1, TParam2, TParam3, TParam4, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TContract>
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CB8 RID: 3256 RVA: 0x00022D44 File Offset: 0x00020F44
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TContract> BindFactoryCustomInterface<TParam1, TParam2, TParam3, TParam4, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CB9 RID: 3257 RVA: 0x00022D4C File Offset: 0x00020F4C
        private FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CBA RID: 3258 RVA: 0x00022DA0 File Offset: 0x00020FA0
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>, PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>>();
        }

        // Token: 0x06000CBB RID: 3259 RVA: 0x00022DA8 File Offset: 0x00020FA8
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> BindFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CBC RID: 3260 RVA: 0x00022DB0 File Offset: 0x00020FB0
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> BindFactoryCustomInterface<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CBD RID: 3261 RVA: 0x00022DB8 File Offset: 0x00020FB8
        private FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CBE RID: 3262 RVA: 0x00022E0C File Offset: 0x0002100C
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>, PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>>();
        }

        // Token: 0x06000CBF RID: 3263 RVA: 0x00022E14 File Offset: 0x00021014
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> BindFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CC0 RID: 3264 RVA: 0x00022E1C File Offset: 0x0002101C
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> BindFactoryCustomInterface<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CC1 RID: 3265 RVA: 0x00022E24 File Offset: 0x00021024
        private FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract, TFactoryContract, TFactoryConcrete>() where TFactoryContract : IFactory where TFactoryConcrete : TFactoryContract, IFactory
        {
            BindStatement bindStatement = this.StartBinding(true);
            BindInfo bindInfo = bindStatement.SpawnBindInfo();
            bindInfo.ContractTypes.Add(typeof(TFactoryContract));
            FactoryBindInfo factoryBindInfo = new FactoryBindInfo(typeof(TFactoryConcrete));
            bindStatement.SetFinalizer(new PlaceholderFactoryBindingFinalizer<TContract>(bindInfo, factoryBindInfo));
            return new FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>(this, bindInfo, factoryBindInfo);
        }

        // Token: 0x06000CC2 RID: 3266 RVA: 0x00022E78 File Offset: 0x00021078
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> BindIFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>()
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>, PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>>();
        }

        // Token: 0x06000CC3 RID: 3267 RVA: 0x00022E80 File Offset: 0x00021080
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> BindFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract, TFactory>() where TFactory : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract, TFactory, TFactory>();
        }

        // Token: 0x06000CC4 RID: 3268 RVA: 0x00022E88 File Offset: 0x00021088
        public FactoryToChoiceIdBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> BindFactoryCustomInterface<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract, TFactoryConcrete, TFactoryContract>() where TFactoryConcrete : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>, TFactoryContract where TFactoryContract : IFactory
        {
            return this.BindFactoryInternal<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract, TFactoryContract, TFactoryConcrete>();
        }

        // Token: 0x06000CC5 RID: 3269 RVA: 0x00022E90 File Offset: 0x00021090
        public T InstantiateExplicit<T>(List<TypeValuePair> extraArgs)
        {
            return (T)((object)this.InstantiateExplicit(typeof(T), extraArgs));
        }

        // Token: 0x06000CC6 RID: 3270 RVA: 0x00022EA8 File Offset: 0x000210A8
        public object InstantiateExplicit(Type concreteType, List<TypeValuePair> extraArgs)
        {
            bool autoInject = true;
            return this.InstantiateExplicit(concreteType, autoInject, extraArgs, new InjectContext(this, concreteType, null), null);
        }

        // Token: 0x06000CC7 RID: 3271 RVA: 0x00022ECC File Offset: 0x000210CC
        public object InstantiateExplicit(Type concreteType, bool autoInject, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier)
        {
            if (this.IsValidating)
            {
                if (this._settings.ValidationErrorResponse == ValidationErrorResponses.Throw)
                {
                    return this.InstantiateInternal(concreteType, autoInject, extraArgs, context, concreteIdentifier);
                }
                try
                {
                    return this.InstantiateInternal(concreteType, autoInject, extraArgs, context, concreteIdentifier);
                }
                catch (Exception e)
                {
                    ModestTree.Log.ErrorException(e);
                    return new ValidationMarker(concreteType, true);
                }
            }
            return this.InstantiateInternal(concreteType, autoInject, extraArgs, context, concreteIdentifier);
        }

        // Token: 0x06000CC8 RID: 3272 RVA: 0x00022F3C File Offset: 0x0002113C
        public Component InstantiateComponentExplicit(Type componentType, GameObject gameObject, List<TypeValuePair> extraArgs)
        {
            ModestTree.Assert.That(componentType.DerivesFrom<Component>());
            this.FlushBindings();
            Component component = gameObject.AddComponent(componentType);
            this.InjectExplicit(component, extraArgs);
            return component;
        }

        // Token: 0x06000CC9 RID: 3273 RVA: 0x00022F6C File Offset: 0x0002116C
        public object InstantiateScriptableObjectResourceExplicit(Type scriptableObjectType, string resourcePath, List<TypeValuePair> extraArgs)
        {
            UnityEngine.Object[] array = Resources.LoadAll(resourcePath, scriptableObjectType);
            ModestTree.Assert.That(array.Length != 0, "Could not find resource at path '{0}' with type '{1}'", resourcePath, scriptableObjectType);
            ModestTree.Assert.That(array.Length == 1, "Found multiple scriptable objects at path '{0}' when only 1 was expected with type '{1}'", resourcePath, scriptableObjectType);
            UnityEngine.Object @object = UnityEngine.Object.Instantiate(array.Single<UnityEngine.Object>());
            this.InjectExplicit(@object, extraArgs);
            return @object;
        }

        // Token: 0x06000CCA RID: 3274 RVA: 0x00022FB8 File Offset: 0x000211B8
        public object InstantiatePrefabResourceForComponentExplicit(Type componentType, string resourcePath, List<TypeValuePair> extraArgs, GameObjectCreationParameters creationInfo)
        {
            return this.InstantiatePrefabResourceForComponentExplicit(componentType, resourcePath, extraArgs, new InjectContext(this, componentType, null), null, creationInfo);
        }

        // Token: 0x06000CCB RID: 3275 RVA: 0x00022FD0 File Offset: 0x000211D0
        public object InstantiatePrefabResourceForComponentExplicit(Type componentType, string resourcePath, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier, GameObjectCreationParameters creationInfo)
        {
            GameObject gameObject = (GameObject)Resources.Load(resourcePath);
            ModestTree.Assert.IsNotNull(gameObject, "Could not find prefab at resource location '{0}'".Fmt(new object[]
            {
                resourcePath
            }));
            return this.InstantiatePrefabForComponentExplicit(componentType, gameObject, extraArgs, context, concreteIdentifier, creationInfo);
        }

        // Token: 0x06000CCC RID: 3276 RVA: 0x00023014 File Offset: 0x00021214
        public object InstantiatePrefabForComponentExplicit(Type componentType, UnityEngine.Object prefab, List<TypeValuePair> extraArgs)
        {
            return this.InstantiatePrefabForComponentExplicit(componentType, prefab, extraArgs, GameObjectCreationParameters.Default);
        }

        // Token: 0x06000CCD RID: 3277 RVA: 0x00023024 File Offset: 0x00021224
        public object InstantiatePrefabForComponentExplicit(Type componentType, UnityEngine.Object prefab, List<TypeValuePair> extraArgs, GameObjectCreationParameters gameObjectBindInfo)
        {
            return this.InstantiatePrefabForComponentExplicit(componentType, prefab, extraArgs, new InjectContext(this, componentType, null), null, gameObjectBindInfo);
        }

        // Token: 0x06000CCE RID: 3278 RVA: 0x0002303C File Offset: 0x0002123C
        public object InstantiatePrefabForComponentExplicit(Type componentType, UnityEngine.Object prefab, List<TypeValuePair> extraArgs, InjectContext context, object concreteIdentifier, GameObjectCreationParameters gameObjectBindInfo)
        {
            ModestTree.Assert.That(!this.AssertOnNewGameObjects, "Given DiContainer does not support creating new game objects");
            this.FlushBindings();
            ModestTree.Assert.That(componentType.IsInterface() || componentType.DerivesFrom<Component>(), "Expected type '{0}' to derive from UnityEngine.Component", componentType);
            bool flag;
            GameObject gameObject = this.CreateAndParentPrefab(prefab, gameObjectBindInfo, context, out flag);
            object result = this.InjectGameObjectForComponentExplicit(gameObject, componentType, extraArgs, context, concreteIdentifier);
            if (flag && !this.IsValidating)
            {
                gameObject.SetActive(true);
            }
            return result;
        }

        // Token: 0x06000CCF RID: 3279 RVA: 0x000230AC File Offset: 0x000212AC
        public void BindExecutionOrder<T>(int order)
        {
            this.BindExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CD0 RID: 3280 RVA: 0x000230C0 File Offset: 0x000212C0
        public void BindExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<ITickable>() || type.DerivesFrom<IInitializable>() || type.DerivesFrom<IDisposable>() || type.DerivesFrom<ILateDisposable>() || type.DerivesFrom<IFixedTickable>() || type.DerivesFrom<ILateTickable>() || type.DerivesFrom<IPoolable>(), "Expected type '{0}' to derive from one or more of the following interfaces: ITickable, IInitializable, ILateTickable, IFixedTickable, IDisposable, ILateDisposable", type);
            if (type.DerivesFrom<ITickable>())
            {
                this.BindTickableExecutionOrder(type, order);
            }
            if (type.DerivesFrom<IInitializable>())
            {
                this.BindInitializableExecutionOrder(type, order);
            }
            if (type.DerivesFrom<IDisposable>())
            {
                this.BindDisposableExecutionOrder(type, order);
            }
            if (type.DerivesFrom<ILateDisposable>())
            {
                this.BindLateDisposableExecutionOrder(type, order);
            }
            if (type.DerivesFrom<IFixedTickable>())
            {
                this.BindFixedTickableExecutionOrder(type, order);
            }
            if (type.DerivesFrom<ILateTickable>())
            {
                this.BindLateTickableExecutionOrder(type, order);
            }
            if (type.DerivesFrom<IPoolable>())
            {
                this.BindPoolableExecutionOrder(type, order);
            }
        }

        // Token: 0x06000CD1 RID: 3281 RVA: 0x00023188 File Offset: 0x00021388
        public CopyNonLazyBinder BindTickableExecutionOrder<T>(int order) where T : ITickable
        {
            return this.BindTickableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CD2 RID: 3282 RVA: 0x0002319C File Offset: 0x0002139C
        public CopyNonLazyBinder BindTickableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<ITickable>(), "Expected type '{0}' to derive from ITickable", type);
            return this.BindInstance<ModestTree.Util.ValuePair<Type, int>>(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WhenInjectedInto<TickableManager>();
        }

        // Token: 0x06000CD3 RID: 3283 RVA: 0x000231C4 File Offset: 0x000213C4
        public CopyNonLazyBinder BindInitializableExecutionOrder<T>(int order) where T : IInitializable
        {
            return this.BindInitializableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CD4 RID: 3284 RVA: 0x000231D8 File Offset: 0x000213D8
        public CopyNonLazyBinder BindInitializableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<IInitializable>(), "Expected type '{0}' to derive from IInitializable", type);
            return this.BindInstance<ModestTree.Util.ValuePair<Type, int>>(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WhenInjectedInto<InitializableManager>();
        }

        // Token: 0x06000CD5 RID: 3285 RVA: 0x00023200 File Offset: 0x00021400
        public CopyNonLazyBinder BindDisposableExecutionOrder<T>(int order) where T : IDisposable
        {
            return this.BindDisposableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CD6 RID: 3286 RVA: 0x00023214 File Offset: 0x00021414
        public CopyNonLazyBinder BindLateDisposableExecutionOrder<T>(int order) where T : ILateDisposable
        {
            return this.BindLateDisposableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CD7 RID: 3287 RVA: 0x00023228 File Offset: 0x00021428
        public CopyNonLazyBinder BindDisposableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<IDisposable>(), "Expected type '{0}' to derive from IDisposable", type);
            return this.BindInstance<ModestTree.Util.ValuePair<Type, int>>(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WhenInjectedInto<DisposableManager>();
        }

        // Token: 0x06000CD8 RID: 3288 RVA: 0x00023250 File Offset: 0x00021450
        public CopyNonLazyBinder BindLateDisposableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<ILateDisposable>(), "Expected type '{0}' to derive from ILateDisposable", type);
            return this.BindInstance<ModestTree.Util.ValuePair<Type, int>>(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WithId("Late").WhenInjectedInto<DisposableManager>();
        }

        // Token: 0x06000CD9 RID: 3289 RVA: 0x00023280 File Offset: 0x00021480
        public CopyNonLazyBinder BindFixedTickableExecutionOrder<T>(int order) where T : IFixedTickable
        {
            return this.BindFixedTickableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CDA RID: 3290 RVA: 0x00023294 File Offset: 0x00021494
        public CopyNonLazyBinder BindFixedTickableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<IFixedTickable>(), "Expected type '{0}' to derive from IFixedTickable", type);
            return this.Bind<ModestTree.Util.ValuePair<Type, int>>().WithId("Fixed").FromInstance(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WhenInjectedInto<TickableManager>();
        }

        // Token: 0x06000CDB RID: 3291 RVA: 0x000232C8 File Offset: 0x000214C8
        public CopyNonLazyBinder BindLateTickableExecutionOrder<T>(int order) where T : ILateTickable
        {
            return this.BindLateTickableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CDC RID: 3292 RVA: 0x000232DC File Offset: 0x000214DC
        public CopyNonLazyBinder BindLateTickableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<ILateTickable>(), "Expected type '{0}' to derive from ILateTickable", type);
            return this.Bind<ModestTree.Util.ValuePair<Type, int>>().WithId("Late").FromInstance(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WhenInjectedInto<TickableManager>();
        }

        // Token: 0x06000CDD RID: 3293 RVA: 0x00023310 File Offset: 0x00021510
        public CopyNonLazyBinder BindPoolableExecutionOrder<T>(int order) where T : IPoolable
        {
            return this.BindPoolableExecutionOrder(typeof(T), order);
        }

        // Token: 0x06000CDE RID: 3294 RVA: 0x00023324 File Offset: 0x00021524
        public CopyNonLazyBinder BindPoolableExecutionOrder(Type type, int order)
        {
            ModestTree.Assert.That(type.DerivesFrom<IPoolable>(), "Expected type '{0}' to derive from IPoolable", type);
            return this.Bind<ModestTree.Util.ValuePair<Type, int>>().FromInstance(ModestTree.Util.ValuePair.New<Type, int>(type, order)).WhenInjectedInto<PoolableManager>();
        }

        // Token: 0x04000389 RID: 905
        private readonly Dictionary<Type, Zenject.Internal.IDecoratorProvider> _decorators = new Dictionary<Type, Zenject.Internal.IDecoratorProvider>();

        // Token: 0x0400038A RID: 906
        private readonly Dictionary<BindingId, List<DiContainer.ProviderInfo>> _providers = new Dictionary<BindingId, List<DiContainer.ProviderInfo>>();

        // Token: 0x0400038B RID: 907
        private readonly DiContainer[][] _containerLookups = new DiContainer[4][];

        // Token: 0x0400038C RID: 908
        private readonly HashSet<Zenject.Internal.LookupId> _resolvesInProgress = new HashSet<Zenject.Internal.LookupId>();

        // Token: 0x0400038D RID: 909
        private readonly HashSet<Zenject.Internal.LookupId> _resolvesTwiceInProgress = new HashSet<Zenject.Internal.LookupId>();

        // Token: 0x0400038E RID: 910
        private readonly LazyInstanceInjector _lazyInjector;

        // Token: 0x0400038F RID: 911
        private readonly Zenject.Internal.SingletonMarkRegistry _singletonMarkRegistry = new Zenject.Internal.SingletonMarkRegistry();

        // Token: 0x04000390 RID: 912
        private readonly Queue<BindStatement> _currentBindings = new Queue<BindStatement>();

        // Token: 0x04000391 RID: 913
        private readonly List<BindStatement> _childBindings = new List<BindStatement>();

        // Token: 0x04000392 RID: 914
        private readonly HashSet<Type> _validatedTypes = new HashSet<Type>();

        // Token: 0x04000393 RID: 915
        private readonly List<IValidatable> _validationQueue = new List<IValidatable>();

        // Token: 0x04000394 RID: 916
        private Transform _contextTransform;

        // Token: 0x04000395 RID: 917
        private bool _hasLookedUpContextTransform;

        // Token: 0x04000396 RID: 918
        private Transform _inheritedDefaultParent;

        // Token: 0x04000397 RID: 919
        private Transform _explicitDefaultParent;

        // Token: 0x04000398 RID: 920
        private bool _hasExplicitDefaultParent;

        // Token: 0x04000399 RID: 921
        private ZenjectSettings _settings;

        // Token: 0x0400039A RID: 922
        private bool _hasResolvedRoots;

        // Token: 0x0400039B RID: 923
        private bool _isFinalizingBinding;

        // Token: 0x0400039C RID: 924
        private bool _isValidating;

        // Token: 0x0400039D RID: 925
        private bool _isInstalling;

        // Token: 0x02000230 RID: 560
        private class ProviderInfo
        {
            // Token: 0x06000CDF RID: 3295 RVA: 0x00023350 File Offset: 0x00021550
            public ProviderInfo(IProvider provider, BindingCondition condition, bool nonLazy, DiContainer container)
            {
                this.Provider = provider;
                this.Condition = condition;
                this.NonLazy = nonLazy;
                this.Container = container;
            }

            // Token: 0x06000CE0 RID: 3296 RVA: 0x00023378 File Offset: 0x00021578
            private static object __zenCreate(object[] P_0)
            {
                return new DiContainer.ProviderInfo((IProvider)P_0[0], (BindingCondition)P_0[1], (bool)P_0[2], (DiContainer)P_0[3]);
            }

            // Token: 0x06000CE1 RID: 3297 RVA: 0x000233C0 File Offset: 0x000215C0
            [Zenject.Internal.Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(DiContainer.ProviderInfo), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(DiContainer.ProviderInfo.__zenCreate), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "provider", typeof(IProvider), null, InjectSources.Any),
                    new InjectableInfo(false, null, "condition", typeof(BindingCondition), null, InjectSources.Any),
                    new InjectableInfo(false, null, "nonLazy", typeof(bool), null, InjectSources.Any),
                    new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any)
                }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x0400039F RID: 927
            public readonly DiContainer Container;

            // Token: 0x040003A0 RID: 928
            public readonly bool NonLazy;

            // Token: 0x040003A1 RID: 929
            public readonly IProvider Provider;

            // Token: 0x040003A2 RID: 930
            public readonly BindingCondition Condition;
        }
    }
}
