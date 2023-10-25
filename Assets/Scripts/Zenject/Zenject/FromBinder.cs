using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000117 RID: 279
    public abstract class FromBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x060005ED RID: 1517 RVA: 0x0000FE34 File Offset: 0x0000E034
        public FromBinder(DiContainer bindContainer, BindInfo bindInfo, BindStatement bindStatement) : base(bindInfo)
        {
            this.BindStatement = bindStatement;
            this.BindContainer = bindContainer;
        }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060005EE RID: 1518 RVA: 0x0000FE4C File Offset: 0x0000E04C
        // (set) Token: 0x060005EF RID: 1519 RVA: 0x0000FE54 File Offset: 0x0000E054
        protected DiContainer BindContainer { get; private set; }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x060005F0 RID: 1520 RVA: 0x0000FE60 File Offset: 0x0000E060
        // (set) Token: 0x060005F1 RID: 1521 RVA: 0x0000FE68 File Offset: 0x0000E068
        protected BindStatement BindStatement { get; private set; }

        // Token: 0x1700004C RID: 76
        // (set) Token: 0x060005F2 RID: 1522 RVA: 0x0000FE74 File Offset: 0x0000E074
        protected IBindingFinalizer SubFinalizer
        {
            set
            {
                this.BindStatement.SetFinalizer(value);
            }
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x060005F3 RID: 1523 RVA: 0x0000FE84 File Offset: 0x0000E084
        protected IEnumerable<Type> AllParentTypes
        {
            get
            {
                return base.BindInfo.ContractTypes.Concat(base.BindInfo.ToTypes);
            }
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x060005F4 RID: 1524 RVA: 0x0000FEA4 File Offset: 0x0000E0A4
        protected IEnumerable<Type> ConcreteTypes
        {
            get
            {
                if (base.BindInfo.ToChoice == ToChoices.Self)
                {
                    return base.BindInfo.ContractTypes;
                }
                ModestTree.Assert.IsNotEmpty<Type>(base.BindInfo.ToTypes, "");
                return base.BindInfo.ToTypes;
            }
        }

        // Token: 0x060005F5 RID: 1525 RVA: 0x0000FEE0 File Offset: 0x0000E0E0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNew()
        {
            BindingUtil.AssertTypesAreNotComponents(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            return this;
        }

        // Token: 0x060005F6 RID: 1526 RVA: 0x0000FEFC File Offset: 0x0000E0FC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolve()
        {
            return this.FromResolve(null);
        }

        // Token: 0x060005F7 RID: 1527 RVA: 0x0000FF08 File Offset: 0x0000E108
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolve(object subIdentifier)
        {
            return this.FromResolve(subIdentifier, InjectSources.Any);
        }

        // Token: 0x060005F8 RID: 1528 RVA: 0x0000FF14 File Offset: 0x0000E114
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolve(object subIdentifier, InjectSources source)
        {
            return this.FromResolveInternal(subIdentifier, false, source);
        }

        // Token: 0x060005F9 RID: 1529 RVA: 0x0000FF20 File Offset: 0x0000E120
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAll()
        {
            return this.FromResolveAll(null);
        }

        // Token: 0x060005FA RID: 1530 RVA: 0x0000FF2C File Offset: 0x0000E12C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAll(object subIdentifier)
        {
            return this.FromResolveAll(subIdentifier, InjectSources.Any);
        }

        // Token: 0x060005FB RID: 1531 RVA: 0x0000FF38 File Offset: 0x0000E138
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveAll(object subIdentifier, InjectSources source)
        {
            return this.FromResolveInternal(subIdentifier, true, source);
        }

        // Token: 0x060005FC RID: 1532 RVA: 0x0000FF44 File Offset: 0x0000E144
        private ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveInternal(object subIdentifier, bool matchAll, InjectSources source)
        {
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new ResolveProvider(type, container, subIdentifier, false, source, matchAll));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x060005FD RID: 1533 RVA: 0x0000FFAC File Offset: 0x0000E1AC
        public SubContainerBinder FromSubContainerResolveAll()
        {
            return this.FromSubContainerResolveAll(null);
        }

        // Token: 0x060005FE RID: 1534 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
        public SubContainerBinder FromSubContainerResolveAll(object subIdentifier)
        {
            return this.FromSubContainerResolveInternal(subIdentifier, true);
        }

        // Token: 0x060005FF RID: 1535 RVA: 0x0000FFC4 File Offset: 0x0000E1C4
        public SubContainerBinder FromSubContainerResolve()
        {
            return this.FromSubContainerResolve(null);
        }

        // Token: 0x06000600 RID: 1536 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
        public SubContainerBinder FromSubContainerResolve(object subIdentifier)
        {
            return this.FromSubContainerResolveInternal(subIdentifier, false);
        }

        // Token: 0x06000601 RID: 1537 RVA: 0x0000FFDC File Offset: 0x0000E1DC
        private SubContainerBinder FromSubContainerResolveInternal(object subIdentifier, bool resolveAll)
        {
            base.BindInfo.RequireExplicitScope = true;
            base.BindInfo.MarkAsCreationBinding = false;
            return new SubContainerBinder(base.BindInfo, this.BindStatement, subIdentifier, resolveAll);
        }

        // Token: 0x06000602 RID: 1538 RVA: 0x0001000C File Offset: 0x0000E20C
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromIFactoryBase<TContract>(Action<ConcreteBinderGeneric<IFactory<TContract>>> factoryBindGenerator)
        {
            Guid factoryId = Guid.NewGuid();
            ConcreteBinderGeneric<IFactory<TContract>> concreteBinderGeneric = this.BindContainer.BindNoFlush<IFactory<TContract>>().WithId(factoryId);
            factoryBindGenerator(concreteBinderGeneric);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new IFactoryProvider<TContract>(container, factoryId));
            ScopeConcreteIdArgConditionCopyNonLazyBinder scopeConcreteIdArgConditionCopyNonLazyBinder = new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
            scopeConcreteIdArgConditionCopyNonLazyBinder.AddSecondaryCopyBindInfo(concreteBinderGeneric.BindInfo);
            return scopeConcreteIdArgConditionCopyNonLazyBinder;
        }

        // Token: 0x06000603 RID: 1539 RVA: 0x0001009C File Offset: 0x0000E29C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new GetFromGameObjectComponentProvider(type, gameObject, false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000604 RID: 1540 RVA: 0x0001010C File Offset: 0x0000E30C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new GetFromGameObjectComponentProvider(type, gameObject, true));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000605 RID: 1541 RVA: 0x0001017C File Offset: 0x0000E37C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new GetFromGameObjectGetterComponentProvider(type, gameObjectGetter, false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000606 RID: 1542 RVA: 0x000101E0 File Offset: 0x0000E3E0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new GetFromGameObjectGetterComponentProvider(type, gameObjectGetter, true));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000607 RID: 1543 RVA: 0x00010244 File Offset: 0x0000E444
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsOnRoot()
        {
            return this.FromComponentsOn((InjectContext ctx) => ctx.Container.Resolve<Context>().gameObject);
        }

        // Token: 0x06000608 RID: 1544 RVA: 0x0001026C File Offset: 0x0000E46C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentOnRoot()
        {
            return this.FromComponentOn((InjectContext ctx) => ctx.Container.Resolve<Context>().gameObject);
        }

        // Token: 0x06000609 RID: 1545 RVA: 0x00010294 File Offset: 0x0000E494
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new AddToExistingGameObjectComponentProvider(gameObject, container, type, this.BindInfo.Arguments, this.BindInfo.ConcreteIdentifier, this.BindInfo.InstantiatedCallback));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600060A RID: 1546 RVA: 0x0001030C File Offset: 0x0000E50C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new AddToExistingGameObjectComponentProviderGetter(gameObjectGetter, container, type, this.BindInfo.Arguments, this.BindInfo.ConcreteIdentifier, this.BindInfo.InstantiatedCallback));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600060B RID: 1547 RVA: 0x00010378 File Offset: 0x0000E578
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentSibling()
        {
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new SingleProviderBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new AddToCurrentGameObjectComponentProvider(container, type, base.BindInfo.Arguments, base.BindInfo.ConcreteIdentifier, base.BindInfo.InstantiatedCallback));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600060C RID: 1548 RVA: 0x000103D0 File Offset: 0x0000E5D0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnRoot()
        {
            return this.FromNewComponentOn((InjectContext ctx) => ctx.Container.Resolve<Context>().gameObject);
        }

        // Token: 0x0600060D RID: 1549 RVA: 0x000103F8 File Offset: 0x0000E5F8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefabResource(string resourcePath)
        {
            return this.FromNewComponentOnNewPrefabResource(resourcePath, new GameObjectCreationParameters());
        }

        // Token: 0x0600060E RID: 1550 RVA: 0x00010408 File Offset: 0x0000E608
        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefabResource(string resourcePath, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new PrefabResourceBindingFinalizer(base.BindInfo, gameObjectInfo, resourcePath, (Type contractType, IPrefabInstantiator instantiator) => new InstantiateOnPrefabComponentProvider(contractType, instantiator));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600060F RID: 1551 RVA: 0x0001047C File Offset: 0x0000E67C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefab(UnityEngine.Object prefab)
        {
            return this.FromNewComponentOnNewPrefab(prefab, new GameObjectCreationParameters());
        }

        // Token: 0x06000610 RID: 1552 RVA: 0x0001048C File Offset: 0x0000E68C
        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefab(UnityEngine.Object prefab, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsComponent(this.ConcreteTypes);
            BindingUtil.AssertTypesAreNotAbstract(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new PrefabBindingFinalizer(base.BindInfo, gameObjectInfo, prefab, (Type contractType, IPrefabInstantiator instantiator) => new InstantiateOnPrefabComponentProvider(contractType, instantiator));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000611 RID: 1553 RVA: 0x00010500 File Offset: 0x0000E700
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefab(UnityEngine.Object prefab)
        {
            return this.FromComponentInNewPrefab(prefab, new GameObjectCreationParameters());
        }

        // Token: 0x06000612 RID: 1554 RVA: 0x00010510 File Offset: 0x0000E710
        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefab(UnityEngine.Object prefab, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new PrefabBindingFinalizer(base.BindInfo, gameObjectInfo, prefab, (Type contractType, IPrefabInstantiator instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, true));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000613 RID: 1555 RVA: 0x00010578 File Offset: 0x0000E778
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefab(UnityEngine.Object prefab)
        {
            return this.FromComponentsInNewPrefab(prefab, new GameObjectCreationParameters());
        }

        // Token: 0x06000614 RID: 1556 RVA: 0x00010588 File Offset: 0x0000E788
        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefab(UnityEngine.Object prefab, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new PrefabBindingFinalizer(base.BindInfo, gameObjectInfo, prefab, (Type contractType, IPrefabInstantiator instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000615 RID: 1557 RVA: 0x000105F0 File Offset: 0x0000E7F0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefabResource(string resourcePath)
        {
            return this.FromComponentInNewPrefabResource(resourcePath, new GameObjectCreationParameters());
        }

        // Token: 0x06000616 RID: 1558 RVA: 0x00010600 File Offset: 0x0000E800
        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefabResource(string resourcePath, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new PrefabResourceBindingFinalizer(base.BindInfo, gameObjectInfo, resourcePath, (Type contractType, IPrefabInstantiator instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, true));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000617 RID: 1559 RVA: 0x00010668 File Offset: 0x0000E868
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefabResource(string resourcePath)
        {
            return this.FromComponentsInNewPrefabResource(resourcePath, new GameObjectCreationParameters());
        }

        // Token: 0x06000618 RID: 1560 RVA: 0x00010678 File Offset: 0x0000E878
        internal NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInNewPrefabResource(string resourcePath, GameObjectCreationParameters gameObjectInfo)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new PrefabResourceBindingFinalizer(base.BindInfo, gameObjectInfo, resourcePath, (Type contractType, IPrefabInstantiator instantiator) => new GetFromPrefabComponentProvider(contractType, instantiator, false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000619 RID: 1561 RVA: 0x000106E0 File Offset: 0x0000E8E0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewScriptableObject(ScriptableObject resource)
        {
            return this.FromScriptableObjectInternal(resource, true);
        }

        // Token: 0x0600061A RID: 1562 RVA: 0x000106EC File Offset: 0x0000E8EC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObject(ScriptableObject resource)
        {
            return this.FromScriptableObjectInternal(resource, false);
        }

        // Token: 0x0600061B RID: 1563 RVA: 0x000106F8 File Offset: 0x0000E8F8
        private ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObjectInternal(ScriptableObject resource, bool createNew)
        {
            BindingUtil.AssertIsInterfaceOrScriptableObject(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new ScriptableObjectInstanceProvider(resource, type, container, this.BindInfo.Arguments, createNew, this.BindInfo.ConcreteIdentifier, this.BindInfo.InstantiatedCallback));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600061C RID: 1564 RVA: 0x00010760 File Offset: 0x0000E960
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromNewScriptableObjectResource(string resourcePath)
        {
            return this.FromScriptableObjectResourceInternal(resourcePath, true);
        }

        // Token: 0x0600061D RID: 1565 RVA: 0x0001076C File Offset: 0x0000E96C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObjectResource(string resourcePath)
        {
            return this.FromScriptableObjectResourceInternal(resourcePath, false);
        }

        // Token: 0x0600061E RID: 1566 RVA: 0x00010778 File Offset: 0x0000E978
        private ScopeConcreteIdArgConditionCopyNonLazyBinder FromScriptableObjectResourceInternal(string resourcePath, bool createNew)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrScriptableObject(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new ScriptableObjectResourceProvider(resourcePath, type, container, this.BindInfo.Arguments, createNew, this.BindInfo.ConcreteIdentifier, this.BindInfo.InstantiatedCallback));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600061F RID: 1567 RVA: 0x000107EC File Offset: 0x0000E9EC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResource(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer _, Type type) => new ResourceProvider(resourcePath, type, true));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000620 RID: 1568 RVA: 0x00010848 File Offset: 0x0000EA48
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromResources(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(this.ConcreteTypes);
            base.BindInfo.RequireExplicitScope = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer _, Type type) => new ResourceProvider(resourcePath, type, false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x06000621 RID: 1569 RVA: 0x000108A4 File Offset: 0x0000EAA4
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInChildren(bool includeInactive = true)
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type concreteType) => new MethodMultipleProviderUntyped(delegate (InjectContext ctx)
            {
                ModestTree.Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>(), "Cannot use FromComponentInChildren to inject data into non monobehaviours!");
                ModestTree.Assert.IsNotNull(ctx.ObjectInstance);
                Component componentInChildren = ((MonoBehaviour)ctx.ObjectInstance).GetComponentInChildren(concreteType, includeInactive);
                if (componentInChildren == null)
                {
                    ModestTree.Assert.That(ctx.Optional, "Could not find any component with type '{0}' through FromComponentInChildren binding", concreteType);
                    return Enumerable.Empty<object>();
                }
                return new object[]
                {
                    componentInChildren
                };
            }, container));
            return this;
        }

        // Token: 0x06000622 RID: 1570 RVA: 0x00010900 File Offset: 0x0000EB00
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInChildrenBase(bool excludeSelf, Func<Component, bool> predicate, bool includeInactive)
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type concreteType) => new MethodMultipleProviderUntyped(delegate (InjectContext ctx)
            {
                ModestTree.Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>(), "Cannot use FromComponentsInChildren to inject data into non monobehaviours!");
                ModestTree.Assert.IsNotNull(ctx.ObjectInstance);
                MonoBehaviour monoBehaviour = (MonoBehaviour)ctx.ObjectInstance;
                IEnumerable<Component> source = from x in monoBehaviour.GetComponentsInChildren(concreteType, includeInactive)
                                                where x != ctx.ObjectInstance
                                                select x;
                if (excludeSelf)
                {
                    source = from x in source
                             where x.gameObject != monoBehaviour.gameObject
                             select x;
                }
                if (predicate != null)
                {
                    source = source.Where(predicate);
                }
                return source.Cast<object>();
            }, container));
            return this;
        }

        // Token: 0x06000623 RID: 1571 RVA: 0x0001096C File Offset: 0x0000EB6C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInParents(bool excludeSelf = false, bool includeInactive = true)
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type concreteType) => new MethodMultipleProviderUntyped(delegate (InjectContext ctx)
            {
                ModestTree.Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>(), "Cannot use FromComponentSibling to inject data into non monobehaviours!");
                ModestTree.Assert.IsNotNull(ctx.ObjectInstance);
                MonoBehaviour monoBehaviour = (MonoBehaviour)ctx.ObjectInstance;
                IEnumerable<Component> source = from x in monoBehaviour.GetComponentsInParent(concreteType, includeInactive)
                                                where x != ctx.ObjectInstance
                                                select x;
                if (excludeSelf)
                {
                    source = from x in source
                             where x.gameObject != monoBehaviour.gameObject
                             select x;
                }
                Component component = source.FirstOrDefault<Component>();
                if (component == null)
                {
                    ModestTree.Assert.That(ctx.Optional, "Could not find any component with type '{0}' through FromComponentInParents binding", concreteType);
                    return Enumerable.Empty<object>();
                }
                return new object[]
                {
                    component
                };
            }, container));
            return this;
        }

        // Token: 0x06000624 RID: 1572 RVA: 0x000109D0 File Offset: 0x0000EBD0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInParents(bool excludeSelf = false, bool includeInactive = true)
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type concreteType) => new MethodMultipleProviderUntyped(delegate (InjectContext ctx)
            {
                ModestTree.Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>(), "Cannot use FromComponentSibling to inject data into non monobehaviours!");
                ModestTree.Assert.IsNotNull(ctx.ObjectInstance);
                MonoBehaviour monoBehaviour = (MonoBehaviour)ctx.ObjectInstance;
                IEnumerable<Component> source = from x in monoBehaviour.GetComponentsInParent(concreteType, includeInactive)
                                                where x != ctx.ObjectInstance
                                                select x;
                if (excludeSelf)
                {
                    source = from x in source
                             where x.gameObject != monoBehaviour.gameObject
                             select x;
                }
                return source.Cast<object>();
            }, container));
            return this;
        }

        // Token: 0x06000625 RID: 1573 RVA: 0x00010A34 File Offset: 0x0000EC34
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentSibling()
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type concreteType) => new MethodMultipleProviderUntyped(delegate (InjectContext ctx)
            {
                ModestTree.Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>(), "Cannot use FromComponentSibling to inject data into non monobehaviours!");
                ModestTree.Assert.IsNotNull(ctx.ObjectInstance);
                Component component = ((MonoBehaviour)ctx.ObjectInstance).GetComponent(concreteType);
                if (component == null)
                {
                    ModestTree.Assert.That(ctx.Optional, "Could not find any component with type '{0}' through FromComponentSibling binding", concreteType);
                    return Enumerable.Empty<object>();
                }
                return new object[]
                {
                    component
                };
            }, container));
            return this;
        }

        // Token: 0x06000626 RID: 1574 RVA: 0x00010A98 File Offset: 0x0000EC98
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsSibling()
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type concreteType) => new MethodMultipleProviderUntyped(delegate (InjectContext ctx)
            {
                ModestTree.Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>(), "Cannot use FromComponentSibling to inject data into non monobehaviours!");
                ModestTree.Assert.IsNotNull(ctx.ObjectInstance);
                MonoBehaviour monoBehaviour = (MonoBehaviour)ctx.ObjectInstance;
                return (from x in monoBehaviour.GetComponents(concreteType)
                        where x != monoBehaviour
                        select x).Cast<object>();
            }, container));
            return this;
        }

        // Token: 0x06000627 RID: 1575 RVA: 0x00010AFC File Offset: 0x0000ECFC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInHierarchy(bool includeInactive = true)
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, concreteType) => new MethodMultipleProviderUntyped(ctx =>
                {
                    var match = container.Resolve<Context>().GetRootGameObjects()
                        .Select(x => x.GetComponentInChildren(concreteType, includeInactive))
                        .Where(x => x != null && !ReferenceEquals(x, ctx.ObjectInstance)).FirstOrDefault();

                    if (match == null)
                    {
                        Assert.That(ctx.Optional,
                            "Could not find any component with type '{0}' through FromComponentInHierarchy binding", concreteType);
                        return Enumerable.Empty<object>();
                    }

                    return new object[] { match };
                },
                    container));
            return this;
        }

        // Token: 0x06000628 RID: 1576 RVA: 0x00010B58 File Offset: 0x0000ED58
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsInHierarchyBase(Func<Component, bool> predicate = null, bool includeInactive = true)
        {
            BindingUtil.AssertIsInterfaceOrComponent(this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = true;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(
                BindInfo,
                (container, concreteType) => new MethodMultipleProviderUntyped(ctx =>
                {
                    var res = container.Resolve<Context>().GetRootGameObjects()
                        .SelectMany(x => x.GetComponentsInChildren(concreteType, includeInactive))
                        .Where(x => !ReferenceEquals(x, ctx.ObjectInstance));

                    if (predicate != null)
                    {
                        res = res.Where(predicate);
                    }

                    return res.Cast<object>();
                },
                    container));
            return this;
        }

        // Token: 0x06000629 RID: 1577 RVA: 0x00010BBC File Offset: 0x0000EDBC
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodUntyped(Func<InjectContext, object> method)
        {
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new MethodProviderUntyped(method, container));
            return this;
        }

        // Token: 0x0600062A RID: 1578 RVA: 0x00010C0C File Offset: 0x0000EE0C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultipleUntyped(Func<InjectContext, IEnumerable<object>> method)
        {
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new MethodMultipleProviderUntyped(method, container));
            return this;
        }

        // Token: 0x0600062B RID: 1579 RVA: 0x00010C5C File Offset: 0x0000EE5C
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodBase<TConcrete>(Func<InjectContext, TConcrete> method)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TConcrete), this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new MethodProvider<TConcrete>(method, container));
            return this;
        }

        // Token: 0x0600062C RID: 1580 RVA: 0x00010CC4 File Offset: 0x0000EEC4
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromMethodMultipleBase<TConcrete>(Func<InjectContext, IEnumerable<TConcrete>> method)
        {
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new MethodProviderMultiple<TConcrete>(method, container));
            return this;
        }

        // Token: 0x0600062D RID: 1581 RVA: 0x00010D14 File Offset: 0x0000EF14
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromResolveGetterBase<TObj, TResult>(object identifier, Func<TObj, TResult> method, InjectSources source, bool matchMultiple)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TResult), this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new GetterProvider<TObj, TResult>(identifier, method, container, source, matchMultiple));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600062E RID: 1582 RVA: 0x00010D9C File Offset: 0x0000EF9C
        protected ScopeConcreteIdArgConditionCopyNonLazyBinder FromInstanceBase(object instance)
        {
            BindingUtil.AssertInstanceDerivesFromOrEqual(instance, this.AllParentTypes);
            base.BindInfo.RequireExplicitScope = false;
            base.BindInfo.MarkAsCreationBinding = false;
            this.SubFinalizer = new ScopableBindingFinalizer(base.BindInfo, (DiContainer container, Type type) => new InstanceProvider(type, instance, container));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }
    }
}
