using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200020A RID: 522
    public class SceneContext : RunnableContext
    {
        // Token: 0x14000009 RID: 9
        // (add) Token: 0x06000B2D RID: 2861 RVA: 0x0001DDA4 File Offset: 0x0001BFA4
        // (remove) Token: 0x06000B2E RID: 2862 RVA: 0x0001DDDC File Offset: 0x0001BFDC
        public event Action PreInstall;

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x06000B2F RID: 2863 RVA: 0x0001DE14 File Offset: 0x0001C014
        // (remove) Token: 0x06000B30 RID: 2864 RVA: 0x0001DE4C File Offset: 0x0001C04C
        public event Action PostInstall;

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x06000B31 RID: 2865 RVA: 0x0001DE84 File Offset: 0x0001C084
        // (remove) Token: 0x06000B32 RID: 2866 RVA: 0x0001DEBC File Offset: 0x0001C0BC
        public event Action PreResolve;

        // Token: 0x1400000C RID: 12
        // (add) Token: 0x06000B33 RID: 2867 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
        // (remove) Token: 0x06000B34 RID: 2868 RVA: 0x0001DF2C File Offset: 0x0001C12C
        public event Action PostResolve;

        // Token: 0x170000BF RID: 191
        // (get) Token: 0x06000B35 RID: 2869 RVA: 0x0001DF64 File Offset: 0x0001C164
        public override DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x170000C0 RID: 192
        // (get) Token: 0x06000B36 RID: 2870 RVA: 0x0001DF6C File Offset: 0x0001C16C
        public bool HasResolved
        {
            get
            {
                return this._hasResolved;
            }
        }

        // Token: 0x170000C1 RID: 193
        // (get) Token: 0x06000B37 RID: 2871 RVA: 0x0001DF74 File Offset: 0x0001C174
        public bool HasInstalled
        {
            get
            {
                return this._hasInstalled;
            }
        }

        // Token: 0x170000C2 RID: 194
        // (get) Token: 0x06000B38 RID: 2872 RVA: 0x0001DF7C File Offset: 0x0001C17C
        public bool IsValidating
        {
            get
            {
                return ProjectContext.Instance.Container.IsValidating;
            }
        }

        // Token: 0x170000C3 RID: 195
        // (get) Token: 0x06000B39 RID: 2873 RVA: 0x0001DF90 File Offset: 0x0001C190
        // (set) Token: 0x06000B3A RID: 2874 RVA: 0x0001DF98 File Offset: 0x0001C198
        public IEnumerable<string> ContractNames
        {
            get
            {
                return this._contractNames;
            }
            set
            {
                this._contractNames.Clear();
                this._contractNames.AddRange(value);
            }
        }

        // Token: 0x170000C4 RID: 196
        // (get) Token: 0x06000B3B RID: 2875 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
        // (set) Token: 0x06000B3C RID: 2876 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
        public IEnumerable<string> ParentContractNames
        {
            get
            {
                List<string> list = new List<string>();
                list.AddRange(this._parentContractNames);
                return list;
            }
            set
            {
                this._parentContractNames = value.ToList<string>();
            }
        }

        // Token: 0x170000C5 RID: 197
        // (get) Token: 0x06000B3D RID: 2877 RVA: 0x0001DFD8 File Offset: 0x0001C1D8
        // (set) Token: 0x06000B3E RID: 2878 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
        public bool ParentNewObjectsUnderSceneContext
        {
            get
            {
                return this._parentNewObjectsUnderSceneContext;
            }
            set
            {
                this._parentNewObjectsUnderSceneContext = value;
            }
        }

        // Token: 0x06000B3F RID: 2879 RVA: 0x0001DFEC File Offset: 0x0001C1EC
        public void Awake()
        {
            base.Initialize();
        }

        // Token: 0x06000B40 RID: 2880 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
        public void Validate()
        {
            ModestTree.Assert.That(this.IsValidating);
            this.Install();
            this.Resolve();
        }

        // Token: 0x06000B41 RID: 2881 RVA: 0x0001E010 File Offset: 0x0001C210
        protected override void RunInternal()
        {
            ProjectContext.Instance.EnsureIsInitialized();
            this.Install();
            this.Resolve();
        }

        // Token: 0x06000B42 RID: 2882 RVA: 0x0001E028 File Offset: 0x0001C228
        public override IEnumerable<GameObject> GetRootGameObjects()
        {
            return Zenject.Internal.ZenUtilInternal.GetRootGameObjects(base.gameObject.scene);
        }

        // Token: 0x06000B43 RID: 2883 RVA: 0x0001E03C File Offset: 0x0001C23C
        private IEnumerable<DiContainer> GetParentContainers()
        {
            IEnumerable<string> parentContractNames = this.ParentContractNames;
            if (parentContractNames.IsEmpty<string>())
            {
                if (SceneContext.ParentContainers != null)
                {
                    IEnumerable<DiContainer> parentContainers = SceneContext.ParentContainers;
                    SceneContext.ParentContainers = null;
                    return parentContainers;
                }
                return new DiContainer[]
                {
                    ProjectContext.Instance.Container
                };
            }
            else
            {
                ModestTree.Assert.IsNull(SceneContext.ParentContainers, "Scene cannot have both a parent scene context name set and also an explicit parent container given");
                List<DiContainer> list = UnityUtil.AllLoadedScenes
                .Except(gameObject.scene)
                .SelectMany(scene => scene.GetRootGameObjects())
                .SelectMany(root => root.GetComponentsInChildren<SceneContext>())
                .Where(sceneContext => sceneContext.ContractNames.Where(x => parentContractNames.Contains(x)).Any())
                .Select(x => x.Container)
                .ToList();
                if (!list.Any<DiContainer>())
                {
                    throw ModestTree.Assert.CreateException("SceneContext on object {0} of scene {1} requires at least one of contracts '{2}', but none of the loaded SceneContexts implements that contract.", new object[]
                    {
                        base.gameObject.name,
                        base.gameObject.scene.name,
                        parentContractNames.Join(", ")
                    });
                }
                return list;
            }
        }

        // Token: 0x06000B44 RID: 2884 RVA: 0x0001E188 File Offset: 0x0001C388
        private List<SceneDecoratorContext> LookupDecoratorContexts()
        {
            if (this._contractNames.IsEmpty<string>())
            {
                return new List<SceneDecoratorContext>();
            }
            return (from decoratorContext in ModestTree.Util.UnityUtil.AllLoadedScenes.Except(base.gameObject.scene).SelectMany((Scene scene) => scene.GetRootGameObjects()).SelectMany((GameObject root) => root.GetComponentsInChildren<SceneDecoratorContext>())
                    where this._contractNames.Contains(decoratorContext.DecoratedContractName)
                    select decoratorContext).ToList<SceneDecoratorContext>();
        }

        // Token: 0x06000B45 RID: 2885 RVA: 0x0001E21C File Offset: 0x0001C41C
        public void Install()
        {
            ModestTree.Assert.That(!this._hasInstalled);
            this._hasInstalled = true;
            ModestTree.Assert.IsNull(this._container);
            IEnumerable<DiContainer> parents = this.GetParentContainers();
            ModestTree.Assert.That(!parents.IsEmpty<DiContainer>());
            ModestTree.Assert.That(parents.All((DiContainer x) => x.IsValidating == parents.First<DiContainer>().IsValidating));
            this._container = new DiContainer(parents, parents.First<DiContainer>().IsValidating);
            if (this.PreInstall != null)
            {
                this.PreInstall();
            }
            if (this.OnPreInstall != null)
            {
                this.OnPreInstall.Invoke();
            }
            ModestTree.Assert.That(this._decoratorContexts.IsEmpty<SceneDecoratorContext>());
            this._decoratorContexts.AddRange(this.LookupDecoratorContexts());
            if (this._parentNewObjectsUnderSceneContext)
            {
                this._container.DefaultParent = base.transform;
            }
            else
            {
                this._container.DefaultParent = null;
            }
            List<MonoBehaviour> list = new List<MonoBehaviour>();
            this.GetInjectableMonoBehaviours(list);
            foreach (MonoBehaviour instance in list)
            {
                this._container.QueueForInject(instance);
            }
            foreach (SceneDecoratorContext sceneDecoratorContext in this._decoratorContexts)
            {
                sceneDecoratorContext.Initialize(this._container);
            }
            this._container.IsInstalling = true;
            try
            {
                this.InstallBindings(list);
            }
            finally
            {
                this._container.IsInstalling = false;
            }
            if (this.PostInstall != null)
            {
                this.PostInstall();
            }
            if (this.OnPostInstall != null)
            {
                this.OnPostInstall.Invoke();
            }
            if (SceneContext.ExtraPostInstallMethod != null)
            {
                SceneContext.ExtraPostInstallMethod(this._container);
                SceneContext.ExtraPostInstallMethod = null;
            }
        }

        // Token: 0x06000B46 RID: 2886 RVA: 0x0001E424 File Offset: 0x0001C624
        public void Resolve()
        {
            if (this.PreResolve != null)
            {
                this.PreResolve();
            }
            if (this.OnPreResolve != null)
            {
                this.OnPreResolve.Invoke();
            }
            ModestTree.Assert.That(this._hasInstalled);
            ModestTree.Assert.That(!this._hasResolved);
            this._hasResolved = true;
            this._container.ResolveRoots();
            if (this.PostResolve != null)
            {
                this.PostResolve();
            }
            if (this.OnPostResolve != null)
            {
                this.OnPostResolve.Invoke();
            }
        }

        // Token: 0x06000B47 RID: 2887 RVA: 0x0001E4A8 File Offset: 0x0001C6A8
        private void InstallBindings(List<MonoBehaviour> injectableMonoBehaviours)
        {
            this._container.Bind(new Type[]
            {
                typeof(Context),
                typeof(SceneContext)
            }).To<SceneContext>().FromInstance(this);
            this._container.BindInterfacesTo<SceneContextRegistryAdderAndRemover>().AsSingle();
            this._container.BindExecutionOrder<SceneContextRegistryAdderAndRemover>(-1);
            if (SceneContext.ExtraBindingsEarlyInstallMethod != null)
            {
                SceneContext.ExtraBindingsEarlyInstallMethod(this._container);
                SceneContext.ExtraBindingsEarlyInstallMethod = null;
            }
            foreach (SceneDecoratorContext sceneDecoratorContext in this._decoratorContexts)
            {
                sceneDecoratorContext.InstallDecoratorSceneBindings();
            }
            base.InstallSceneBindings(injectableMonoBehaviours);
            this._container.Bind(new Type[]
            {
                typeof(SceneKernel),
                typeof(MonoKernel)
            }).To<SceneKernel>().FromNewComponentOn(base.gameObject).AsSingle().NonLazy();
            this._container.Bind<ZenjectSceneLoader>().AsSingle();
            if (SceneContext.ExtraBindingsInstallMethod != null)
            {
                SceneContext.ExtraBindingsInstallMethod(this._container);
                SceneContext.ExtraBindingsInstallMethod = null;
            }
            foreach (SceneDecoratorContext sceneDecoratorContext2 in this._decoratorContexts)
            {
                sceneDecoratorContext2.InstallDecoratorInstallers();
            }
            base.InstallInstallers();
            foreach (SceneDecoratorContext sceneDecoratorContext3 in this._decoratorContexts)
            {
                sceneDecoratorContext3.InstallLateDecoratorInstallers();
            }
            if (SceneContext.ExtraBindingsLateInstallMethod != null)
            {
                SceneContext.ExtraBindingsLateInstallMethod(this._container);
                SceneContext.ExtraBindingsLateInstallMethod = null;
            }
        }

        // Token: 0x06000B48 RID: 2888 RVA: 0x0001E688 File Offset: 0x0001C888
        protected override void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            Scene scene = base.gameObject.scene;
            Zenject.Internal.ZenUtilInternal.AddStateMachineBehaviourAutoInjectersInScene(scene);
            Zenject.Internal.ZenUtilInternal.GetInjectableMonoBehavioursInScene(scene, monoBehaviours);
        }

        // Token: 0x06000B49 RID: 2889 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
        public static SceneContext Create()
        {
            return RunnableContext.CreateComponent<SceneContext>(new GameObject("SceneContext"));
        }

        // Token: 0x06000B4C RID: 2892 RVA: 0x0001E6F8 File Offset: 0x0001C8F8
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SceneContext), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000359 RID: 857
        public UnityEvent OnPreInstall;

        // Token: 0x0400035A RID: 858
        public UnityEvent OnPostInstall;

        // Token: 0x0400035B RID: 859
        public UnityEvent OnPreResolve;

        // Token: 0x0400035C RID: 860
        public UnityEvent OnPostResolve;

        // Token: 0x0400035D RID: 861
        public static Action<DiContainer> ExtraBindingsEarlyInstallMethod;

        // Token: 0x0400035E RID: 862
        public static Action<DiContainer> ExtraBindingsInstallMethod;

        // Token: 0x0400035F RID: 863
        public static Action<DiContainer> ExtraBindingsLateInstallMethod;

        // Token: 0x04000360 RID: 864
        public static Action<DiContainer> ExtraPostInstallMethod;

        // Token: 0x04000361 RID: 865
        public static IEnumerable<DiContainer> ParentContainers;

        // Token: 0x04000362 RID: 866
        [FormerlySerializedAs("ParentNewObjectsUnderRoot")]
        [FormerlySerializedAs("_parentNewObjectsUnderRoot")]
        [Tooltip("When true, objects that are created at runtime will be parented to the SceneContext")]
        [SerializeField]
        private bool _parentNewObjectsUnderSceneContext;

        // Token: 0x04000363 RID: 867
        [Tooltip("Optional contract names for this SceneContext, allowing contexts in subsequently loaded scenes to depend on it and be parented to it, and also for previously loaded decorators to be included")]
        [SerializeField]
        private List<string> _contractNames = new List<string>();

        // Token: 0x04000364 RID: 868
        [SerializeField]
        [Tooltip("Optional contract names of SceneContexts in previously loaded scenes that this context depends on and to which it should be parented")]
        private List<string> _parentContractNames = new List<string>();

        // Token: 0x04000365 RID: 869
        private DiContainer _container;

        // Token: 0x04000366 RID: 870
        private readonly List<SceneDecoratorContext> _decoratorContexts = new List<SceneDecoratorContext>();

        // Token: 0x04000367 RID: 871
        private bool _hasInstalled;

        // Token: 0x04000368 RID: 872
        private bool _hasResolved;
    }
}
