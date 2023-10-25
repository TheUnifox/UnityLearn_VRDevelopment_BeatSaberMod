using System;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000150 RID: 336
    [NoReflectionBaking]
    public class SubContainerBinder
    {
        // Token: 0x06000731 RID: 1841 RVA: 0x0001311C File Offset: 0x0001131C
        public SubContainerBinder(BindInfo bindInfo, BindStatement bindStatement, object subIdentifier, bool resolveAll)
        {
            this._bindInfo = bindInfo;
            this._bindStatement = bindStatement;
            this._subIdentifier = subIdentifier;
            this._resolveAll = resolveAll;
            bindStatement.SetFinalizer(null);
        }

        // Token: 0x17000051 RID: 81
        // (set) Token: 0x06000732 RID: 1842 RVA: 0x00013148 File Offset: 0x00011348
        protected IBindingFinalizer SubFinalizer
        {
            set
            {
                this._bindStatement.SetFinalizer(value);
            }
        }

        // Token: 0x06000733 RID: 1843 RVA: 0x00013158 File Offset: 0x00011358
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByInstance(DiContainer subContainer)
        {
            this.SubFinalizer = new SubContainerBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer _) => new SubContainerCreatorByInstance(subContainer));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo);
        }

        // Token: 0x06000734 RID: 1844 RVA: 0x000131A8 File Offset: 0x000113A8
        public ScopeConcreteIdArgConditionCopyNonLazyBinder ByInstanceGetter(Func<InjectContext, DiContainer> subContainerGetter)
        {
            this.SubFinalizer = new SubContainerBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer _) => new SubContainerCreatorByInstanceGetter(subContainerGetter));
            return new ScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo);
        }

        // Token: 0x06000735 RID: 1845 RVA: 0x000131F8 File Offset: 0x000113F8
        public WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder ByInstaller<TInstaller>() where TInstaller : InstallerBase
        {
            return this.ByInstaller(typeof(TInstaller));
        }

        // Token: 0x06000736 RID: 1846 RVA: 0x0001320C File Offset: 0x0001140C
        public WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder ByInstaller(Type installerType)
        {
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            SubContainerCreatorBindInfo subContainerBindInfo = new SubContainerCreatorBindInfo();
            this.SubFinalizer = new SubContainerBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByInstaller(container, subContainerBindInfo, installerType));
            return new WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(subContainerBindInfo, this._bindInfo);
        }

        // Token: 0x06000737 RID: 1847 RVA: 0x00013288 File Offset: 0x00011488
        public WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder ByMethod(Action<DiContainer> installerMethod)
        {
            SubContainerCreatorBindInfo subContainerBindInfo = new SubContainerCreatorBindInfo();
            this.SubFinalizer = new SubContainerBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByMethod(container, subContainerBindInfo, installerMethod));
            return new WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(subContainerBindInfo, this._bindInfo);
        }

        // Token: 0x06000738 RID: 1848 RVA: 0x000132E8 File Offset: 0x000114E8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectMethod(Action<DiContainer> installerMethod)
        {
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewGameObjectMethod(container, gameObjectInfo, installerMethod));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x06000739 RID: 1849 RVA: 0x00013348 File Offset: 0x00011548
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabMethod(UnityEngine.Object prefab, Action<DiContainer> installerMethod)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewPrefabMethod(container, new PrefabProvider(prefab), gameObjectInfo, installerMethod));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x0600073A RID: 1850 RVA: 0x000133BC File Offset: 0x000115BC
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectInstaller<TInstaller>() where TInstaller : InstallerBase
        {
            return this.ByNewGameObjectInstaller(typeof(TInstaller));
        }

        // Token: 0x0600073B RID: 1851 RVA: 0x000133D0 File Offset: 0x000115D0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewGameObjectInstaller(Type installerType)
        {
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewGameObjectInstaller(container, gameObjectInfo, installerType, this._bindInfo.Arguments));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x0600073C RID: 1852 RVA: 0x00013454 File Offset: 0x00011654
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabInstaller<TInstaller>(UnityEngine.Object prefab) where TInstaller : InstallerBase
        {
            return this.ByNewPrefabInstaller(prefab, typeof(TInstaller));
        }

        // Token: 0x0600073D RID: 1853 RVA: 0x00013468 File Offset: 0x00011668
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabInstaller(UnityEngine.Object prefab, Type installerType)
        {
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewPrefabInstaller(container, new PrefabProvider(prefab), gameObjectInfo, installerType, this._bindInfo.Arguments));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x0600073E RID: 1854 RVA: 0x000134F0 File Offset: 0x000116F0
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceMethod(string resourcePath, Action<DiContainer> installerMethod)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewPrefabMethod(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerMethod));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x0600073F RID: 1855 RVA: 0x00013564 File Offset: 0x00011764
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceInstaller<TInstaller>(string resourcePath) where TInstaller : InstallerBase
        {
            return this.ByNewPrefabResourceInstaller(resourcePath, typeof(TInstaller));
        }

        // Token: 0x06000740 RID: 1856 RVA: 0x00013578 File Offset: 0x00011778
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResourceInstaller(string resourcePath, Type installerType)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>(), "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewPrefabInstaller(container, new PrefabProviderResource(resourcePath), gameObjectInfo, installerType, this._bindInfo.Arguments));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x06000741 RID: 1857 RVA: 0x0001360C File Offset: 0x0001180C
        [Obsolete("ByNewPrefab has been renamed to ByNewContextPrefab to avoid confusion with ByNewPrefabInstaller and ByNewPrefabMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefab(UnityEngine.Object prefab)
        {
            return this.ByNewContextPrefab(prefab);
        }

        // Token: 0x06000742 RID: 1858 RVA: 0x00013618 File Offset: 0x00011818
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefab(UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewPrefab(container, new PrefabProvider(prefab), gameObjectInfo));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x06000743 RID: 1859 RVA: 0x00013684 File Offset: 0x00011884
        [Obsolete("ByNewPrefabResource has been renamed to ByNewContextPrefabResource to avoid confusion with ByNewPrefabResourceInstaller and ByNewPrefabResourceMethod")]
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewPrefabResource(string resourcePath)
        {
            return this.ByNewContextPrefabResource(resourcePath);
        }

        // Token: 0x06000744 RID: 1860 RVA: 0x00013690 File Offset: 0x00011890
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder ByNewContextPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);
            GameObjectCreationParameters gameObjectInfo = new GameObjectCreationParameters();
            this.SubFinalizer = new SubContainerPrefabBindingFinalizer(this._bindInfo, this._subIdentifier, this._resolveAll, (DiContainer container) => new SubContainerCreatorByNewPrefab(container, new PrefabProviderResource(resourcePath), gameObjectInfo));
            return new NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(this._bindInfo, gameObjectInfo);
        }

        // Token: 0x04000261 RID: 609
        private readonly BindInfo _bindInfo;

        // Token: 0x04000262 RID: 610
        private readonly BindStatement _bindStatement;

        // Token: 0x04000263 RID: 611
        private readonly object _subIdentifier;

        // Token: 0x04000264 RID: 612
        private readonly bool _resolveAll;
    }
}
