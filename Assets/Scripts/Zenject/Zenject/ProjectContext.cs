using System;
using System.Collections.Generic;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000208 RID: 520
    public class ProjectContext : Context
    {
        // Token: 0x14000005 RID: 5
        // (add) Token: 0x06000B0B RID: 2827 RVA: 0x0001D6DC File Offset: 0x0001B8DC
        // (remove) Token: 0x06000B0C RID: 2828 RVA: 0x0001D714 File Offset: 0x0001B914
        public event Action PreInstall;

        // Token: 0x14000006 RID: 6
        // (add) Token: 0x06000B0D RID: 2829 RVA: 0x0001D74C File Offset: 0x0001B94C
        // (remove) Token: 0x06000B0E RID: 2830 RVA: 0x0001D784 File Offset: 0x0001B984
        public event Action PostInstall;

        // Token: 0x14000007 RID: 7
        // (add) Token: 0x06000B0F RID: 2831 RVA: 0x0001D7BC File Offset: 0x0001B9BC
        // (remove) Token: 0x06000B10 RID: 2832 RVA: 0x0001D7F4 File Offset: 0x0001B9F4
        public event Action PreResolve;

        // Token: 0x14000008 RID: 8
        // (add) Token: 0x06000B11 RID: 2833 RVA: 0x0001D82C File Offset: 0x0001BA2C
        // (remove) Token: 0x06000B12 RID: 2834 RVA: 0x0001D864 File Offset: 0x0001BA64
        public event Action PostResolve;

        // Token: 0x170000B9 RID: 185
        // (get) Token: 0x06000B13 RID: 2835 RVA: 0x0001D89C File Offset: 0x0001BA9C
        public override DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x170000BA RID: 186
        // (get) Token: 0x06000B14 RID: 2836 RVA: 0x0001D8A4 File Offset: 0x0001BAA4
        public static bool HasInstance
        {
            get
            {
                return ProjectContext._instance != null;
            }
        }

        // Token: 0x170000BB RID: 187
        // (get) Token: 0x06000B15 RID: 2837 RVA: 0x0001D8B4 File Offset: 0x0001BAB4
        public static ProjectContext Instance
        {
            get
            {
                if (ProjectContext._instance == null)
                {
                    ProjectContext.InstantiateAndInitialize();
                    ModestTree.Assert.IsNotNull(ProjectContext._instance);
                }
                return ProjectContext._instance;
            }
        }

        // Token: 0x170000BC RID: 188
        // (get) Token: 0x06000B16 RID: 2838 RVA: 0x0001D8D8 File Offset: 0x0001BAD8
        // (set) Token: 0x06000B17 RID: 2839 RVA: 0x0001D8E0 File Offset: 0x0001BAE0
        public static bool ValidateOnNextRun { get; set; }

        // Token: 0x06000B18 RID: 2840 RVA: 0x0001D8E8 File Offset: 0x0001BAE8
        public override IEnumerable<GameObject> GetRootGameObjects()
        {
            return new GameObject[]
            {
                base.gameObject
            };
        }

        // Token: 0x06000B19 RID: 2841 RVA: 0x0001D8FC File Offset: 0x0001BAFC
        public static GameObject TryGetPrefab()
        {
            UnityEngine.Object[] array = Resources.LoadAll("ProjectContext", typeof(GameObject));
            if (array.Length != 0)
            {
                ModestTree.Assert.That(array.Length == 1, "Found multiple project context prefabs at resource path '{0}'", "ProjectContext");
                return (GameObject)array[0];
            }
            array = Resources.LoadAll("ProjectCompositionRoot", typeof(GameObject));
            if (array.Length != 0)
            {
                ModestTree.Assert.That(array.Length == 1, "Found multiple project context prefabs at resource path '{0}'", "ProjectCompositionRoot");
                return (GameObject)array[0];
            }
            return null;
        }

        // Token: 0x06000B1A RID: 2842 RVA: 0x0001D978 File Offset: 0x0001BB78
        private static void InstantiateAndInitialize()
        {
            ModestTree.Assert.That(UnityEngine.Object.FindObjectsOfType<ProjectContext>().IsEmpty<ProjectContext>(), "Tried to create multiple instances of ProjectContext!");
            GameObject gameObject = ProjectContext.TryGetPrefab();
            bool flag = false;
            if (gameObject == null)
            {
                ProjectContext._instance = new GameObject("ProjectContext").AddComponent<ProjectContext>();
            }
            else
            {
                flag = gameObject.activeSelf;
                GameObject gameObject2;
                if (flag)
                {
                    gameObject.SetActive(false);
                    gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                }
                ProjectContext._instance = gameObject2.GetComponent<ProjectContext>();
                ModestTree.Assert.IsNotNull(ProjectContext._instance, "Could not find ProjectContext component on prefab 'Resources/{0}.prefab'", "ProjectContext");
            }
            ProjectContext._instance.Initialize();
            if (flag)
            {
                ProjectContext._instance.gameObject.SetActive(true);
            }
        }

        // Token: 0x170000BD RID: 189
        // (get) Token: 0x06000B1B RID: 2843 RVA: 0x0001DA24 File Offset: 0x0001BC24
        // (set) Token: 0x06000B1C RID: 2844 RVA: 0x0001DA2C File Offset: 0x0001BC2C
        public bool ParentNewObjectsUnderContext
        {
            get
            {
                return this._parentNewObjectsUnderContext;
            }
            set
            {
                this._parentNewObjectsUnderContext = value;
            }
        }

        // Token: 0x06000B1D RID: 2845 RVA: 0x0001DA38 File Offset: 0x0001BC38
        public void EnsureIsInitialized()
        {
        }

        // Token: 0x06000B1E RID: 2846 RVA: 0x0001DA3C File Offset: 0x0001BC3C
        public void Awake()
        {
            if (Application.isPlaying)
            {
                UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            }
        }

        // Token: 0x06000B1F RID: 2847 RVA: 0x0001DA50 File Offset: 0x0001BC50
        private void Initialize()
        {
            ModestTree.Assert.IsNull(this._container);
            if (Application.isEditor)
            {
                TypeAnalyzer.ReflectionBakingCoverageMode = this._editorReflectionBakingCoverageMode;
            }
            else
            {
                TypeAnalyzer.ReflectionBakingCoverageMode = this._buildsReflectionBakingCoverageMode;
            }
            bool validateOnNextRun = ProjectContext.ValidateOnNextRun;
            ProjectContext.ValidateOnNextRun = false;
            this._container = new DiContainer(new DiContainer[]
            {
                StaticContext.Container
            }, validateOnNextRun);
            if (this.PreInstall != null)
            {
                this.PreInstall();
            }
            List<MonoBehaviour> list = new List<MonoBehaviour>();
            this.GetInjectableMonoBehaviours(list);
            foreach (MonoBehaviour instance in list)
            {
                this._container.QueueForInject(instance);
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
            if (this.PreResolve != null)
            {
                this.PreResolve();
            }
            this._container.ResolveRoots();
            if (this.PostResolve != null)
            {
                this.PostResolve();
            }
        }

        // Token: 0x06000B20 RID: 2848 RVA: 0x0001DB88 File Offset: 0x0001BD88
        protected override void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            Zenject.Internal.ZenUtilInternal.AddStateMachineBehaviourAutoInjectersUnderGameObject(base.gameObject);
            Zenject.Internal.ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(base.gameObject, monoBehaviours);
        }

        // Token: 0x06000B21 RID: 2849 RVA: 0x0001DBA4 File Offset: 0x0001BDA4
        private void InstallBindings(List<MonoBehaviour> injectableMonoBehaviours)
        {
            if (this._parentNewObjectsUnderContext)
            {
                this._container.DefaultParent = base.transform;
            }
            else
            {
                this._container.DefaultParent = null;
            }
            this._container.Settings = (this._settings ?? ZenjectSettings.Default);
            this._container.Bind<ZenjectSceneLoader>().AsSingle();
            Installer<ZenjectManagersInstaller>.Install(this._container);
            this._container.Bind<Context>().FromInstance(this);
            this._container.Bind(new Type[]
            {
                typeof(ProjectKernel),
                typeof(MonoKernel)
            }).To<ProjectKernel>().FromNewComponentOn(base.gameObject).AsSingle().NonLazy();
            this._container.Bind<SceneContextRegistry>().AsSingle();
            base.InstallSceneBindings(injectableMonoBehaviours);
            base.InstallInstallers();
        }

        // Token: 0x06000B23 RID: 2851 RVA: 0x0001DC94 File Offset: 0x0001BE94
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ProjectContext), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000349 RID: 841
        public const string ProjectContextResourcePath = "ProjectContext";

        // Token: 0x0400034A RID: 842
        public const string ProjectContextResourcePathOld = "ProjectCompositionRoot";

        // Token: 0x0400034B RID: 843
        private static ProjectContext _instance;

        // Token: 0x0400034C RID: 844
        [SerializeField]
        [Tooltip("When true, objects that are created at runtime will be parented to the ProjectContext")]
        private bool _parentNewObjectsUnderContext = true;

        // Token: 0x0400034D RID: 845
        [SerializeField]
        private ReflectionBakingCoverageModes _editorReflectionBakingCoverageMode;

        // Token: 0x0400034E RID: 846
        [SerializeField]
        private ReflectionBakingCoverageModes _buildsReflectionBakingCoverageMode;

        // Token: 0x0400034F RID: 847
        [SerializeField]
        private ZenjectSettings _settings;

        // Token: 0x04000350 RID: 848
        private DiContainer _container;
    }
}
