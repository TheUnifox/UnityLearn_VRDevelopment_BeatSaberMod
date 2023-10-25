using System;
using System.Collections.Generic;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000207 RID: 519
    public class GameObjectContext : RunnableContext
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000AFA RID: 2810 RVA: 0x0001D1B0 File Offset: 0x0001B3B0
        // (remove) Token: 0x06000AFB RID: 2811 RVA: 0x0001D1E8 File Offset: 0x0001B3E8
        public event Action PreInstall;

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000AFC RID: 2812 RVA: 0x0001D220 File Offset: 0x0001B420
        // (remove) Token: 0x06000AFD RID: 2813 RVA: 0x0001D258 File Offset: 0x0001B458
        public event Action PostInstall;

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x06000AFE RID: 2814 RVA: 0x0001D290 File Offset: 0x0001B490
        // (remove) Token: 0x06000AFF RID: 2815 RVA: 0x0001D2C8 File Offset: 0x0001B4C8
        public event Action PreResolve;

        // Token: 0x14000004 RID: 4
        // (add) Token: 0x06000B00 RID: 2816 RVA: 0x0001D300 File Offset: 0x0001B500
        // (remove) Token: 0x06000B01 RID: 2817 RVA: 0x0001D338 File Offset: 0x0001B538
        public event Action PostResolve;

        // Token: 0x170000B8 RID: 184
        // (get) Token: 0x06000B02 RID: 2818 RVA: 0x0001D370 File Offset: 0x0001B570
        public override DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000B03 RID: 2819 RVA: 0x0001D378 File Offset: 0x0001B578
        public override IEnumerable<GameObject> GetRootGameObjects()
        {
            return new GameObject[]
            {
                base.gameObject
            };
        }

        // Token: 0x06000B04 RID: 2820 RVA: 0x0001D38C File Offset: 0x0001B58C
        [Inject]
        public void Construct(DiContainer parentContainer)
        {
            ModestTree.Assert.IsNull(this._container);
            this._container = parentContainer.CreateSubContainer();
            base.Initialize();
        }

        // Token: 0x06000B05 RID: 2821 RVA: 0x0001D3AC File Offset: 0x0001B5AC
        protected override void RunInternal()
        {
            if (this.PreInstall != null)
            {
                this.PreInstall();
            }
            List<MonoBehaviour> list = new List<MonoBehaviour>();
            this.GetInjectableMonoBehaviours(list);
            foreach (MonoBehaviour monoBehaviour in list)
            {
                if (monoBehaviour is MonoKernel)
                {
                    ModestTree.Assert.That(monoBehaviour == this._kernel, "Found MonoKernel derived class that is not hooked up to GameObjectContext.  If you use MonoKernel, you must indicate this to GameObjectContext by dragging and dropping it to the Kernel field in the inspector");
                }
                this._container.QueueForInject(monoBehaviour);
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
            if (base.gameObject.scene.isLoaded && !this._container.IsValidating)
            {
                this._kernel = this._container.Resolve<MonoKernel>();
                this._kernel.Initialize();
            }
        }

        // Token: 0x06000B06 RID: 2822 RVA: 0x0001D4EC File Offset: 0x0001B6EC
        protected override void GetInjectableMonoBehaviours(List<MonoBehaviour> monoBehaviours)
        {
            Zenject.Internal.ZenUtilInternal.AddStateMachineBehaviourAutoInjectersUnderGameObject(base.gameObject);
            foreach (MonoBehaviour monoBehaviour in base.GetComponents<MonoBehaviour>())
            {
                if (!(monoBehaviour == null) && Zenject.Internal.ZenUtilInternal.IsInjectableMonoBehaviourType(monoBehaviour.GetType()) && !(monoBehaviour == this))
                {
                    monoBehaviours.Add(monoBehaviour);
                }
            }
            for (int j = 0; j < base.transform.childCount; j++)
            {
                Transform child = base.transform.GetChild(j);
                if (child != null)
                {
                    Zenject.Internal.ZenUtilInternal.GetInjectableMonoBehavioursUnderGameObject(child.gameObject, monoBehaviours);
                }
            }
        }

        // Token: 0x06000B07 RID: 2823 RVA: 0x0001D580 File Offset: 0x0001B780
        private void InstallBindings(List<MonoBehaviour> injectableMonoBehaviours)
        {
            this._container.DefaultParent = base.transform;
            this._container.Bind<Context>().FromInstance(this);
            this._container.Bind<GameObjectContext>().FromInstance(this);
            if (this._kernel == null)
            {
                this._container.Bind<MonoKernel>().To<DefaultGameObjectKernel>().FromNewComponentOn(base.gameObject).AsSingle().NonLazy();
            }
            else
            {
                this._container.Bind<MonoKernel>().FromInstance(this._kernel).AsSingle().NonLazy();
            }
            base.InstallSceneBindings(injectableMonoBehaviours);
            base.InstallInstallers();
        }

        // Token: 0x06000B09 RID: 2825 RVA: 0x0001D630 File Offset: 0x0001B830
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((GameObjectContext)P_0).Construct((DiContainer)P_1[0]);
        }

        // Token: 0x06000B0A RID: 2826 RVA: 0x0001D64C File Offset: 0x0001B84C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(GameObjectContext), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(GameObjectContext.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "parentContainer", typeof(DiContainer), null, InjectSources.Any)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000343 RID: 835
        [FormerlySerializedAs("_facade")]
        [Tooltip("Note that this field is optional and can be ignored in most cases.  This is really only needed if you want to control the 'Script Execution Order' of your subcontainer.  In this case, define a new class that derives from MonoKernel, add it to this game object, then drag it into this field.  Then you can set a value for 'Script Execution Order' for this new class and this will control when all ITickable/IInitializable classes bound within this subcontainer get called.")]
        [SerializeField]
        private MonoKernel _kernel;

        // Token: 0x04000344 RID: 836
        private DiContainer _container;
    }
}
