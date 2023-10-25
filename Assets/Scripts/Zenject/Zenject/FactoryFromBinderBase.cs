using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000F1 RID: 241
    [NoReflectionBaking]
    public class FactoryFromBinderBase : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x06000556 RID: 1366 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
        public FactoryFromBinderBase(DiContainer bindContainer, Type contractType, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindInfo)
        {
            this.FactoryBindInfo = factoryBindInfo;
            this.BindContainer = bindContainer;
            this.ContractType = contractType;
            factoryBindInfo.ProviderFunc = ((DiContainer container) => new TransientProvider(this.ContractType, container, base.BindInfo.Arguments, base.BindInfo.ContextInfo, base.BindInfo.ConcreteIdentifier, base.BindInfo.InstantiatedCallback));
        }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x06000557 RID: 1367 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
        // (set) Token: 0x06000558 RID: 1368 RVA: 0x0000E4EC File Offset: 0x0000C6EC
        internal DiContainer BindContainer { get; private set; }

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x06000559 RID: 1369 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
        // (set) Token: 0x0600055A RID: 1370 RVA: 0x0000E500 File Offset: 0x0000C700
        protected FactoryBindInfo FactoryBindInfo { get; private set; }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x0600055B RID: 1371 RVA: 0x0000E50C File Offset: 0x0000C70C
        // (set) Token: 0x0600055C RID: 1372 RVA: 0x0000E51C File Offset: 0x0000C71C
        internal Func<DiContainer, IProvider> ProviderFunc
        {
            get
            {
                return this.FactoryBindInfo.ProviderFunc;
            }
            set
            {
                this.FactoryBindInfo.ProviderFunc = value;
            }
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x0600055D RID: 1373 RVA: 0x0000E52C File Offset: 0x0000C72C
        // (set) Token: 0x0600055E RID: 1374 RVA: 0x0000E534 File Offset: 0x0000C734
        protected Type ContractType { get; private set; }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x0600055F RID: 1375 RVA: 0x0000E540 File Offset: 0x0000C740
        public IEnumerable<Type> AllParentTypes
        {
            get
            {
                yield return this.ContractType;
                foreach (Type type in base.BindInfo.ToTypes)
                {
                    yield return type;
                }
                List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
                yield break;
                yield break;
            }
        }

        // Token: 0x06000560 RID: 1376 RVA: 0x0000E550 File Offset: 0x0000C750
        public ConditionCopyNonLazyBinder FromNew()
        {
            BindingUtil.AssertIsNotComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            return this;
        }

        // Token: 0x06000561 RID: 1377 RVA: 0x0000E56C File Offset: 0x0000C76C
        public ConditionCopyNonLazyBinder FromResolve()
        {
            return this.FromResolve(null);
        }

        // Token: 0x06000562 RID: 1378 RVA: 0x0000E578 File Offset: 0x0000C778
        public ConditionCopyNonLazyBinder FromInstance(object instance)
        {
            BindingUtil.AssertInstanceDerivesFromOrEqual(instance, this.AllParentTypes);
            this.ProviderFunc = ((DiContainer container) => new InstanceProvider(this.ContractType, instance, container));
            return this;
        }

        // Token: 0x06000563 RID: 1379 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
        public ConditionCopyNonLazyBinder FromResolve(object subIdentifier)
        {
            this.ProviderFunc = ((DiContainer container) => new ResolveProvider(this.ContractType, container, subIdentifier, false, InjectSources.Any, false));
            return this;
        }

        // Token: 0x06000564 RID: 1380 RVA: 0x0000E5F4 File Offset: 0x0000C7F4
        internal ConcreteBinderGeneric<T> CreateIFactoryBinder<T>(out Guid factoryId)
        {
            factoryId = Guid.NewGuid();
            return this.BindContainer.BindNoFlush<T>().WithId(factoryId);
        }

        // Token: 0x06000565 RID: 1381 RVA: 0x0000E61C File Offset: 0x0000C81C
        public ConditionCopyNonLazyBinder FromComponentOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new GetFromGameObjectComponentProvider(this.ContractType, gameObject, true));
            return this;
        }

        // Token: 0x06000566 RID: 1382 RVA: 0x0000E674 File Offset: 0x0000C874
        public ConditionCopyNonLazyBinder FromComponentOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new GetFromGameObjectGetterComponentProvider(this.ContractType, gameObjectGetter, true));
            return this;
        }

        // Token: 0x06000567 RID: 1383 RVA: 0x0000E6C0 File Offset: 0x0000C8C0
        public ConditionCopyNonLazyBinder FromComponentOnRoot()
        {
            return this.FromComponentOn((InjectContext ctx) => this.BindContainer.Resolve<Context>().gameObject);
        }

        // Token: 0x06000568 RID: 1384 RVA: 0x0000E6D4 File Offset: 0x0000C8D4
        public ConditionCopyNonLazyBinder FromNewComponentOn(GameObject gameObject)
        {
            BindingUtil.AssertIsValidGameObject(gameObject);
            BindingUtil.AssertIsComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new AddToExistingGameObjectComponentProvider(gameObject, container, this.ContractType, new List<TypeValuePair>(), this.BindInfo.ConcreteIdentifier, this.BindInfo.InstantiatedCallback));
            return this;
        }

        // Token: 0x06000569 RID: 1385 RVA: 0x0000E72C File Offset: 0x0000C92C
        public ConditionCopyNonLazyBinder FromNewComponentOn(Func<InjectContext, GameObject> gameObjectGetter)
        {
            BindingUtil.AssertIsComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new AddToExistingGameObjectComponentProviderGetter(gameObjectGetter, container, this.ContractType, new List<TypeValuePair>(), this.BindInfo.ConcreteIdentifier, this.BindInfo.InstantiatedCallback));
            return this;
        }

        // Token: 0x0600056A RID: 1386 RVA: 0x0000E778 File Offset: 0x0000C978
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefab(UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new InstantiateOnPrefabComponentProvider(this.ContractType, new PrefabInstantiator(container, gameObjectInfo, this.ContractType, new Type[]
            {
                this.ContractType
            }, new List<TypeValuePair>(), new PrefabProvider(prefab), this.BindInfo.InstantiatedCallback)));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600056B RID: 1387 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefab(UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            BindingUtil.AssertIsInterfaceOrComponent(this.ContractType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new GetFromPrefabComponentProvider(this.ContractType, new PrefabInstantiator(container, gameObjectInfo, this.ContractType, new Type[]
            {
                this.ContractType
            }, new List<TypeValuePair>(), new PrefabProvider(prefab), this.BindInfo.InstantiatedCallback), true));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600056C RID: 1388 RVA: 0x0000E850 File Offset: 0x0000CA50
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentInNewPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrComponent(this.ContractType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new GetFromPrefabComponentProvider(this.ContractType, new PrefabInstantiator(container, gameObjectInfo, this.ContractType, new Type[]
            {
                this.ContractType
            }, new List<TypeValuePair>(), new PrefabProviderResource(resourcePath), this.BindInfo.InstantiatedCallback), true));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600056D RID: 1389 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder FromNewComponentOnNewPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsComponent(this.ContractType);
            BindingUtil.AssertIsNotAbstract(this.ContractType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new InstantiateOnPrefabComponentProvider(this.ContractType, new PrefabInstantiator(container, gameObjectInfo, this.ContractType, new Type[]
            {
                this.ContractType
            }, new List<TypeValuePair>(), new PrefabProviderResource(resourcePath), this.BindInfo.InstantiatedCallback)));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600056E RID: 1390 RVA: 0x0000E928 File Offset: 0x0000CB28
        public ConditionCopyNonLazyBinder FromNewScriptableObjectResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrScriptableObject(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new ScriptableObjectResourceProvider(resourcePath, this.ContractType, container, new List<TypeValuePair>(), true, null, this.BindInfo.InstantiatedCallback));
            return this;
        }

        // Token: 0x0600056F RID: 1391 RVA: 0x0000E974 File Offset: 0x0000CB74
        public ConditionCopyNonLazyBinder FromScriptableObjectResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            BindingUtil.AssertIsInterfaceOrScriptableObject(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new ScriptableObjectResourceProvider(resourcePath, this.ContractType, container, new List<TypeValuePair>(), false, null, this.BindInfo.InstantiatedCallback));
            return this;
        }

        // Token: 0x06000570 RID: 1392 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
        public ConditionCopyNonLazyBinder FromResource(string resourcePath)
        {
            BindingUtil.AssertDerivesFromUnityObject(this.ContractType);
            this.ProviderFunc = ((DiContainer container) => new ResourceProvider(resourcePath, this.ContractType, true));
            return this;
        }
    }
}
