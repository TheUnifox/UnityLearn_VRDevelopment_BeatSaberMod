using System;
using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002FA RID: 762
    public class ZenjectSceneLoader
    {
        // Token: 0x06001064 RID: 4196 RVA: 0x0002E2D4 File Offset: 0x0002C4D4
        public ZenjectSceneLoader([InjectOptional] SceneContext sceneRoot, ProjectKernel projectKernel)
        {
            this._projectKernel = projectKernel;
            this._sceneContainer = ((sceneRoot == null) ? null : sceneRoot.Container);
        }

        // Token: 0x06001065 RID: 4197 RVA: 0x0002E2FC File Offset: 0x0002C4FC
        public void LoadScene(string sceneName)
        {
            this.LoadScene(sceneName, LoadSceneMode.Single);
        }

        // Token: 0x06001066 RID: 4198 RVA: 0x0002E308 File Offset: 0x0002C508
        public void LoadScene(string sceneName, LoadSceneMode loadMode)
        {
            this.LoadScene(sceneName, loadMode, null);
        }

        // Token: 0x06001067 RID: 4199 RVA: 0x0002E314 File Offset: 0x0002C514
        public void LoadScene(string sceneName, LoadSceneMode loadMode, Action<DiContainer> extraBindings)
        {
            this.LoadScene(sceneName, loadMode, extraBindings, LoadSceneRelationship.None);
        }

        // Token: 0x06001068 RID: 4200 RVA: 0x0002E320 File Offset: 0x0002C520
        public void LoadScene(string sceneName, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode)
        {
            this.LoadScene(sceneName, loadMode, extraBindings, containerMode, null);
        }

        // Token: 0x06001069 RID: 4201 RVA: 0x0002E330 File Offset: 0x0002C530
        public void LoadScene(string sceneName, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode, Action<DiContainer> extraBindingsLate)
        {
            this.PrepareForLoadScene(loadMode, null, extraBindings, extraBindingsLate, containerMode);
            ModestTree.Assert.That(Application.CanStreamedLevelBeLoaded(sceneName), "Unable to load scene '{0}'", sceneName);
            SceneManager.LoadScene(sceneName, loadMode);
        }

        // Token: 0x0600106A RID: 4202 RVA: 0x0002E358 File Offset: 0x0002C558
        public AsyncOperation LoadSceneAsync(string sceneName)
        {
            return this.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        // Token: 0x0600106B RID: 4203 RVA: 0x0002E364 File Offset: 0x0002C564
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadMode)
        {
            return this.LoadSceneAsync(sceneName, loadMode, null);
        }

        // Token: 0x0600106C RID: 4204 RVA: 0x0002E370 File Offset: 0x0002C570
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Action<DiContainer> extraBindings)
        {
            return this.LoadSceneAsync(sceneName, loadMode, extraBindings, LoadSceneRelationship.None);
        }

        // Token: 0x0600106D RID: 4205 RVA: 0x0002E37C File Offset: 0x0002C57C
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode)
        {
            return this.LoadSceneAsync(sceneName, loadMode, null, extraBindings, containerMode, null);
        }

        // Token: 0x0600106E RID: 4206 RVA: 0x0002E38C File Offset: 0x0002C58C
        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Action<DiContainer> extraBindingsEarly, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode, Action<DiContainer> extraBindingsLate)
        {
            this.PrepareForLoadScene(loadMode, extraBindingsEarly, extraBindings, extraBindingsLate, containerMode);
            ModestTree.Assert.That(Application.CanStreamedLevelBeLoaded(sceneName), "Unable to load scene '{0}'", sceneName);
            return SceneManager.LoadSceneAsync(sceneName, loadMode);
        }

        // Token: 0x0600106F RID: 4207 RVA: 0x0002E3B4 File Offset: 0x0002C5B4
        private void PrepareForLoadScene(LoadSceneMode loadMode, Action<DiContainer> extraBindingsEarly, Action<DiContainer> extraBindings, Action<DiContainer> extraBindingsLate, LoadSceneRelationship containerMode)
        {
            if (loadMode == LoadSceneMode.Single)
            {
                ModestTree.Assert.IsEqual(containerMode, LoadSceneRelationship.None);
                this._projectKernel.ForceUnloadAllScenes(false);
            }
            if (containerMode == LoadSceneRelationship.None)
            {
                SceneContext.ParentContainers = null;
            }
            else if (containerMode == LoadSceneRelationship.Child)
            {
                if (this._sceneContainer == null)
                {
                    SceneContext.ParentContainers = null;
                }
                else
                {
                    SceneContext.ParentContainers = new DiContainer[]
                    {
                        this._sceneContainer
                    };
                }
            }
            else
            {
                ModestTree.Assert.IsNotNull(this._sceneContainer, "Cannot use LoadSceneRelationship.Sibling when loading scenes from ProjectContext");
                ModestTree.Assert.IsEqual(containerMode, LoadSceneRelationship.Sibling);
                SceneContext.ParentContainers = this._sceneContainer.ParentContainers;
            }
            SceneContext.ExtraBindingsEarlyInstallMethod = extraBindingsEarly;
            SceneContext.ExtraBindingsInstallMethod = extraBindings;
            SceneContext.ExtraBindingsLateInstallMethod = extraBindingsLate;
        }

        // Token: 0x06001070 RID: 4208 RVA: 0x0002E460 File Offset: 0x0002C660
        public void LoadScene(int sceneIndex)
        {
            this.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        // Token: 0x06001071 RID: 4209 RVA: 0x0002E46C File Offset: 0x0002C66C
        public void LoadScene(int sceneIndex, LoadSceneMode loadMode)
        {
            this.LoadScene(sceneIndex, loadMode, null);
        }

        // Token: 0x06001072 RID: 4210 RVA: 0x0002E478 File Offset: 0x0002C678
        public void LoadScene(int sceneIndex, LoadSceneMode loadMode, Action<DiContainer> extraBindings)
        {
            this.LoadScene(sceneIndex, loadMode, extraBindings, LoadSceneRelationship.None);
        }

        // Token: 0x06001073 RID: 4211 RVA: 0x0002E484 File Offset: 0x0002C684
        public void LoadScene(int sceneIndex, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode)
        {
            this.LoadScene(sceneIndex, loadMode, extraBindings, containerMode, null);
        }

        // Token: 0x06001074 RID: 4212 RVA: 0x0002E494 File Offset: 0x0002C694
        public void LoadScene(int sceneIndex, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode, Action<DiContainer> extraBindingsLate)
        {
            this.PrepareForLoadScene(loadMode, null, extraBindings, extraBindingsLate, containerMode);
            ModestTree.Assert.That(Application.CanStreamedLevelBeLoaded(sceneIndex), "Unable to load scene '{0}'", sceneIndex);
            SceneManager.LoadScene(sceneIndex, loadMode);
        }

        // Token: 0x06001075 RID: 4213 RVA: 0x0002E4C0 File Offset: 0x0002C6C0
        public AsyncOperation LoadSceneAsync(int sceneIndex)
        {
            return this.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        }

        // Token: 0x06001076 RID: 4214 RVA: 0x0002E4CC File Offset: 0x0002C6CC
        public AsyncOperation LoadSceneAsync(int sceneIndex, LoadSceneMode loadMode)
        {
            return this.LoadSceneAsync(sceneIndex, loadMode, null);
        }

        // Token: 0x06001077 RID: 4215 RVA: 0x0002E4D8 File Offset: 0x0002C6D8
        public AsyncOperation LoadSceneAsync(int sceneIndex, LoadSceneMode loadMode, Action<DiContainer> extraBindings)
        {
            return this.LoadSceneAsync(sceneIndex, loadMode, extraBindings, LoadSceneRelationship.None);
        }

        // Token: 0x06001078 RID: 4216 RVA: 0x0002E4E4 File Offset: 0x0002C6E4
        public AsyncOperation LoadSceneAsync(int sceneIndex, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode)
        {
            return this.LoadSceneAsync(sceneIndex, loadMode, extraBindings, containerMode, null);
        }

        // Token: 0x06001079 RID: 4217 RVA: 0x0002E4F4 File Offset: 0x0002C6F4
        public AsyncOperation LoadSceneAsync(int sceneIndex, LoadSceneMode loadMode, Action<DiContainer> extraBindings, LoadSceneRelationship containerMode, Action<DiContainer> extraBindingsLate)
        {
            this.PrepareForLoadScene(loadMode, null, extraBindings, extraBindingsLate, containerMode);
            ModestTree.Assert.That(Application.CanStreamedLevelBeLoaded(sceneIndex), "Unable to load scene '{0}'", sceneIndex);
            return SceneManager.LoadSceneAsync(sceneIndex, loadMode);
        }

        // Token: 0x0600107A RID: 4218 RVA: 0x0002E520 File Offset: 0x0002C720
        private static object __zenCreate(object[] P_0)
        {
            return new ZenjectSceneLoader((SceneContext)P_0[0], (ProjectKernel)P_0[1]);
        }

        // Token: 0x0600107B RID: 4219 RVA: 0x0002E550 File Offset: 0x0002C750
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ZenjectSceneLoader), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ZenjectSceneLoader.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(true, null, "sceneRoot", typeof(SceneContext), null, InjectSources.Any),
                new InjectableInfo(false, null, "projectKernel", typeof(ProjectKernel), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000533 RID: 1331
        private readonly ProjectKernel _projectKernel;

        // Token: 0x04000534 RID: 1332
        private readonly DiContainer _sceneContainer;
    }
}
