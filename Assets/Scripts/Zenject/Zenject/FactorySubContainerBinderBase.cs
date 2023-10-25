using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x020000E8 RID: 232
    [NoReflectionBaking]
    public class FactorySubContainerBinderBase<TContract>
    {
        // Token: 0x06000520 RID: 1312 RVA: 0x0000DC58 File Offset: 0x0000BE58
        public FactorySubContainerBinderBase(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo, object subIdentifier)
        {
            this.FactoryBindInfo = factoryBindInfo;
            this.SubIdentifier = subIdentifier;
            this.BindInfo = bindInfo;
            this.BindContainer = bindContainer;
            factoryBindInfo.ProviderFunc = null;
        }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x06000521 RID: 1313 RVA: 0x0000DC84 File Offset: 0x0000BE84
        // (set) Token: 0x06000522 RID: 1314 RVA: 0x0000DC8C File Offset: 0x0000BE8C
        protected DiContainer BindContainer { get; private set; }

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x06000523 RID: 1315 RVA: 0x0000DC98 File Offset: 0x0000BE98
        // (set) Token: 0x06000524 RID: 1316 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
        protected FactoryBindInfo FactoryBindInfo { get; private set; }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x06000525 RID: 1317 RVA: 0x0000DCAC File Offset: 0x0000BEAC
        // (set) Token: 0x06000526 RID: 1318 RVA: 0x0000DCBC File Offset: 0x0000BEBC
        protected Func<DiContainer, IProvider> ProviderFunc
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

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x06000527 RID: 1319 RVA: 0x0000DCCC File Offset: 0x0000BECC
        // (set) Token: 0x06000528 RID: 1320 RVA: 0x0000DCD4 File Offset: 0x0000BED4
        protected BindInfo BindInfo { get; private set; }

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x06000529 RID: 1321 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
        // (set) Token: 0x0600052A RID: 1322 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
        protected object SubIdentifier { get; private set; }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x0600052B RID: 1323 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
        protected Type ContractType
        {
            get
            {
                return typeof(TContract);
            }
        }

        // Token: 0x0600052C RID: 1324 RVA: 0x0000DD00 File Offset: 0x0000BF00
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByInstaller<TInstaller>() where TInstaller : InstallerBase
        {
            return this.ByInstaller(typeof(TInstaller));
        }

        // Token: 0x0600052D RID: 1325 RVA: 0x0000DD14 File Offset: 0x0000BF14
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByInstaller(Type installerType)
        {
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            SubContainerCreatorBindInfo subcontainerBindInfo = new SubContainerCreatorBindInfo();
            this.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByInstaller(container, subcontainerBindInfo, installerType, this.BindInfo.Arguments), false));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(this.BindInfo);
        }

        // Token: 0x0600052E RID: 1326 RVA: 0x0000DD78 File Offset: 0x0000BF78
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectInstaller<TInstaller>() where TInstaller : InstallerBase
        {
            return this.ByNewGameObjectInstaller(typeof(TInstaller));
        }

        // Token: 0x0600052F RID: 1327 RVA: 0x0000DD8C File Offset: 0x0000BF8C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectInstaller(Type installerType)
        {
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewGameObjectInstaller(container, gameObjectInfo, installerType, this.BindInfo.Arguments), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000530 RID: 1328 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabInstaller<TInstaller>(UnityEngine.Object prefab) where TInstaller : InstallerBase
        {
            return this.ByNewPrefabInstaller(prefab, typeof(TInstaller));
        }

        // Token: 0x06000531 RID: 1329 RVA: 0x0000DE0C File Offset: 0x0000C00C
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabInstaller(UnityEngine.Object prefab, Type installerType)
        {
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabInstaller(container, new PrefabProvider(prefab), gameObjectInfo, installerType, this.BindInfo.Arguments), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this.BindInfo, gameObjectInfo);
        }

        // Token: 0x06000532 RID: 1330 RVA: 0x0000DE80 File Offset: 0x0000C080
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceInstaller<TInstaller>(string resourcePath) where TInstaller : InstallerBase
        {
            return this.ByNewPrefabResourceInstaller(resourcePath, typeof(TInstaller));
        }

        // Token: 0x06000533 RID: 1331 RVA: 0x0000DE94 File Offset: 0x0000C094
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceInstaller(string resourcePath, Type installerType)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.ProviderFunc = ((DiContainer container) => new SubContainerDependencyProvider(this.ContractType, this.SubIdentifier, new SubContainerCreatorByNewPrefabInstaller(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerType, this.BindInfo.Arguments), false));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this.BindInfo, gameObjectInfo);
        }
    }
}
