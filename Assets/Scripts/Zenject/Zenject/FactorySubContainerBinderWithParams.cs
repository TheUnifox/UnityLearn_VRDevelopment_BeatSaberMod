using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000ED RID: 237
    [NoReflectionBaking]
    public class FactorySubContainerBinderWithParams<TContract> : FactorySubContainerBinderBase<TContract>
    {
        // Token: 0x06000544 RID: 1348 RVA: 0x0000E200 File Offset: 0x0000C400
        public FactorySubContainerBinderWithParams(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier) : base(bindContainer, bindInfo, factoryBindInfo, subIdentifier)
        {
        }

        // Token: 0x06000545 RID: 1349 RVA: 0x0000E210 File Offset: 0x0000C410
        [Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab(Type installerType, UnityEngine.Object prefab)
        {
            return this.ByNewContextPrefab(installerType, prefab);
        }

        // Token: 0x06000546 RID: 1350 RVA: 0x0000E21C File Offset: 0x0000C41C
        [Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab<TInstaller>(UnityEngine.Object prefab) where TInstaller : IInstaller
        {
            return this.ByNewContextPrefab<TInstaller>(prefab);
        }

        // Token: 0x06000547 RID: 1351 RVA: 0x0000E228 File Offset: 0x0000C428
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab<TInstaller>(UnityEngine.Object prefab) where TInstaller : IInstaller
        {
            return this.ByNewContextPrefab(typeof(TInstaller), prefab);
        }

        // Token: 0x06000548 RID: 1352 RVA: 0x0000E23C File Offset: 0x0000C43C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab(Type installerType, UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            ModestTree.Assert.That(installerType.DerivesFrom<MonoInstaller>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'MonoInstaller'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabWithParams(installerType, container, new PrefabProvider(prefab), gameObjectInfo), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000549 RID: 1353 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
        [Obsolete("ByNewPrefabResource has been renamed to ByNewContextPrefabResource to avoid confusion with ByNewPrefabResourceInstaller and ByNewPrefabResourceMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource<TInstaller>(string resourcePath) where TInstaller : IInstaller
        {
            return this.ByNewContextPrefabResource<TInstaller>(resourcePath);
        }

        // Token: 0x0600054A RID: 1354 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource(Type installerType, string resourcePath)
        {
            return this.ByNewContextPrefabResource(installerType, resourcePath);
        }

        // Token: 0x0600054B RID: 1355 RVA: 0x0000E2D0 File Offset: 0x0000C4D0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefabResource<TInstaller>(string resourcePath) where TInstaller : IInstaller
        {
            return this.ByNewContextPrefabResource(typeof(TInstaller), resourcePath);
        }

        // Token: 0x0600054C RID: 1356 RVA: 0x0000E2E4 File Offset: 0x0000C4E4
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefabResource(Type installerType, string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            base.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabWithParams(installerType, container, new PrefabProviderResource(resourcePath), gameObjectInfo), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(base.BindInfo, gameObjectInfo);
        }
    }
}
