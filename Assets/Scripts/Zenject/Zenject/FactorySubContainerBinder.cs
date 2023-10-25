using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000BE RID: 190
    [NoReflectionBaking]
    public class FactorySubContainerBinder<TContract> : FactorySubContainerBinderBase<TContract>
    {
        // Token: 0x0600046C RID: 1132 RVA: 0x0000B9D8 File Offset: 0x00009BD8
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x0600046D RID: 1133 RVA: 0x0000B9E8 File Offset: 0x00009BE8
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600046E RID: 1134 RVA: 0x0000BA34 File Offset: 0x00009C34
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600046F RID: 1135 RVA: 0x0000BA84 File Offset: 0x00009C84
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000470 RID: 1136 RVA: 0x0000BAE8 File Offset: 0x00009CE8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000471 RID: 1137 RVA: 0x0000BB4C File Offset: 0x00009D4C
        [Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab(UnityEngine.Object prefab)
        {
            return this.ByNewContextPrefab(prefab);
        }

        // Token: 0x06000472 RID: 1138 RVA: 0x0000BB58 File Offset: 0x00009D58
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab(UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefab(container, new PrefabProvider(prefab), gameObjectInfo), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000473 RID: 1139 RVA: 0x0000BBB4 File Offset: 0x00009DB4
        [Obsolete("ByNewPrefabResource has been renamed to ByNewContextPrefabResource to avoid confusion with ByNewPrefabResourceInstaller and ByNewPrefabResourceMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource(string resourcePath)
        {
            return this.ByNewContextPrefabResource(resourcePath);
        }

        // Token: 0x06000474 RID: 1140 RVA: 0x0000BBC0 File Offset: 0x00009DC0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefab(container, new PrefabProviderResource(resourcePath), gameObjectInfo), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x0600048D RID: 1165 RVA: 0x0000C004 File Offset: 0x0000A204
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x0600048E RID: 1166 RVA: 0x0000C014 File Offset: 0x0000A214
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600048F RID: 1167 RVA: 0x0000C060 File Offset: 0x0000A260
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000490 RID: 1168 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000491 RID: 1169 RVA: 0x0000C114 File Offset: 0x0000A314
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TParam2, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x060004B7 RID: 1207 RVA: 0x0000C81C File Offset: 0x0000AA1C
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x060004B8 RID: 1208 RVA: 0x0000C82C File Offset: 0x0000AA2C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1, TParam2>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x060004B9 RID: 1209 RVA: 0x0000C878 File Offset: 0x0000AA78
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1, TParam2> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004BA RID: 1210 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1, TParam2> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004BB RID: 1211 RVA: 0x0000C92C File Offset: 0x0000AB2C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1, TParam2> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x060004CC RID: 1228 RVA: 0x0000CC28 File Offset: 0x0000AE28
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x060004CD RID: 1229 RVA: 0x0000CC38 File Offset: 0x0000AE38
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2, TParam3> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1, TParam2, TParam3>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x060004CE RID: 1230 RVA: 0x0000CC84 File Offset: 0x0000AE84
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1, TParam2, TParam3> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004CF RID: 1231 RVA: 0x0000CCD4 File Offset: 0x0000AED4
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1, TParam2, TParam3> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004D0 RID: 1232 RVA: 0x0000CD38 File Offset: 0x0000AF38
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1, TParam2, TParam3> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x060004E1 RID: 1249 RVA: 0x0000D034 File Offset: 0x0000B234
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x060004E2 RID: 1250 RVA: 0x0000D044 File Offset: 0x0000B244
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x060004E3 RID: 1251 RVA: 0x0000D090 File Offset: 0x0000B290
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004E4 RID: 1252 RVA: 0x0000D0E0 File Offset: 0x0000B2E0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004E5 RID: 1253 RVA: 0x0000D144 File Offset: 0x0000B344
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1, TParam2, TParam3, TParam4> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x060004F6 RID: 1270 RVA: 0x0000D440 File Offset: 0x0000B640
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x060004F7 RID: 1271 RVA: 0x0000D450 File Offset: 0x0000B650
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x060004F8 RID: 1272 RVA: 0x0000D49C File Offset: 0x0000B69C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4, TParam5>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004F9 RID: 1273 RVA: 0x0000D4EC File Offset: 0x0000B6EC
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004FA RID: 1274 RVA: 0x0000D550 File Offset: 0x0000B750
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x0600050B RID: 1291 RVA: 0x0000D84C File Offset: 0x0000BA4C
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x0600050C RID: 1292 RVA: 0x0000D85C File Offset: 0x0000BA5C
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x0600050D RID: 1293 RVA: 0x0000D8A8 File Offset: 0x0000BAA8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600050E RID: 1294 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x0600050F RID: 1295 RVA: 0x0000D95C File Offset: 0x0000BB5C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }

    [NoReflectionBaking]
    public class FactorySubContainerBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> : FactorySubContainerBinderWithParams<TContract>
    {
        // Token: 0x060004A2 RID: 1186 RVA: 0x0000C410 File Offset: 0x0000A610
        public FactorySubContainerBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x060004A3 RID: 1187 RVA: 0x0000C420 File Offset: 0x0000A620
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>(container, subcontainerBindInfo, installerMethod), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo);
        }

        // Token: 0x060004A4 RID: 1188 RVA: 0x0000C46C File Offset: 0x0000A66C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>(container, gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004A5 RID: 1189 RVA: 0x0000C4BC File Offset: 0x0000A6BC
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x060004A6 RID: 1190 RVA: 0x0000C520 File Offset: 0x0000A720
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabMethod<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }
}
