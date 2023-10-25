using System;
using System.Collections.Generic;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200020E RID: 526
    public class SceneDecoratorContext : Context
    {
        // Token: 0x170000C6 RID: 198
        // (get) Token: 0x06000B5F RID: 2911 RVA: 0x0001E930 File Offset: 0x0001CB30
        // (set) Token: 0x06000B60 RID: 2912 RVA: 0x0001E938 File Offset: 0x0001CB38
        public IEnumerable<MonoInstaller> LateInstallers
        {
            get
            {
                return this._lateInstallers;
            }
            set
            {
                this._lateInstallers.Clear();
                this._lateInstallers.AddRange(value);
            }
        }

        // Token: 0x170000C7 RID: 199
        // (get) Token: 0x06000B61 RID: 2913 RVA: 0x0001E954 File Offset: 0x0001CB54
        // (set) Token: 0x06000B62 RID: 2914 RVA: 0x0001E95C File Offset: 0x0001CB5C
        public IEnumerable<MonoInstaller> LateInstallerPrefabs
        {
            get
            {
                return this._lateInstallerPrefabs;
            }
            set
            {
                this._lateInstallerPrefabs.Clear();
                this._lateInstallerPrefabs.AddRange(value);
            }
        }

        // Token: 0x170000C8 RID: 200
        // (get) Token: 0x06000B63 RID: 2915 RVA: 0x0001E978 File Offset: 0x0001CB78
        // (set) Token: 0x06000B64 RID: 2916 RVA: 0x0001E980 File Offset: 0x0001CB80
        public IEnumerable<ScriptableObjectInstaller> LateScriptableObjectInstallers
        {
            get
            {
                return this._lateScriptableObjectInstallers;
            }
            set
            {
                this._lateScriptableObjectInstallers.Clear();
                this._lateScriptableObjectInstallers.AddRange(value);
            }
        }

        // Token: 0x170000C9 RID: 201
        // (get) Token: 0x06000B65 RID: 2917 RVA: 0x0001E99C File Offset: 0x0001CB9C
        public string DecoratedContractName
        {
            get
            {
                return this._decoratedContractName;
            }
        }

        // Token: 0x170000CA RID: 202
        // (get) Token: 0x06000B66 RID: 2918 RVA: 0x0001E9A4 File Offset: 0x0001CBA4
        public override DiContainer Container
        {
            get
            {
                ModestTree.Assert.IsNotNull(this._container);
                return this._container;
            }
        }

        // Token: 0x06000B67 RID: 2919 RVA: 0x0001E9B8 File Offset: 0x0001CBB8
        public override IEnumerable<GameObject> GetRootGameObjects()
        {
            throw ModestTree.Assert.CreateException();
        }

        // Token: 0x06000B68 RID: 2920 RVA: 0x0001E9C0 File Offset: 0x0001CBC0
        public void Initialize(DiContainer container)
        {
            ModestTree.Assert.IsNull(this._container);
            ModestTree.Assert.That(this._injectableMonoBehaviours.IsEmpty<MonoBehaviour>());
            this._container = container;
            this.GetInjectableMonoBehaviours(this._injectableMonoBehaviours);
            foreach (MonoBehaviour instance in this._injectableMonoBehaviours)
            {
                container.QueueForInject(instance);
            }
        }

        // Token: 0x06000B69 RID: 2921 RVA: 0x0001EA44 File Offset: 0x0001CC44
        public void InstallDecoratorSceneBindings()
        {
            this._container.Bind<SceneDecoratorContext>().FromInstance(this);
            base.InstallSceneBindings(this._injectableMonoBehaviours);
        }

        // Token: 0x06000B6A RID: 2922 RVA: 0x0001EA64 File Offset: 0x0001CC64
        public void InstallDecoratorInstallers()
        {
            base.InstallInstallers();
        }

        // Token: 0x06000B6B RID: 2923 RVA: 0x0001EA6C File Offset: 0x0001CC6C
        protected override void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            Scene scene = base.gameObject.scene;
            Zenject.Internal.ZenUtilInternal.AddStateMachineBehaviourAutoInjectersInScene(scene);
            Zenject.Internal.ZenUtilInternal.GetInjectableMonoBehavioursInScene(scene, monoBehaviours);
        }

        // Token: 0x06000B6C RID: 2924 RVA: 0x0001EA88 File Offset: 0x0001CC88
        public void InstallLateDecoratorInstallers()
        {
            base.InstallInstallers(new List<InstallerBase>(), new List<Type>(), this._lateScriptableObjectInstallers, this._lateInstallers, this._lateInstallerPrefabs);
        }

        // Token: 0x06000B6E RID: 2926 RVA: 0x0001EAE0 File Offset: 0x0001CCE0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SceneDecoratorContext), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000372 RID: 882
        [SerializeField]
        private List<MonoInstaller> _lateInstallers = new List<MonoInstaller>();

        // Token: 0x04000373 RID: 883
        [SerializeField]
        private List<MonoInstaller> _lateInstallerPrefabs = new List<MonoInstaller>();

        // Token: 0x04000374 RID: 884
        [SerializeField]
        private List<ScriptableObjectInstaller> _lateScriptableObjectInstallers = new List<ScriptableObjectInstaller>();

        // Token: 0x04000375 RID: 885
        [SerializeField]
        [FormerlySerializedAs("SceneName")]
        private string _decoratedContractName;

        // Token: 0x04000376 RID: 886
        private DiContainer _container;

        // Token: 0x04000377 RID: 887
        private readonly List<MonoBehaviour> _injectableMonoBehaviours = new List<MonoBehaviour>();
    }
}
