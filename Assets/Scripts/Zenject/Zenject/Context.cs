using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000205 RID: 517
    public abstract class Context : MonoBehaviour
    {
        // Token: 0x170000B2 RID: 178
        // (get) Token: 0x06000ADF RID: 2783 RVA: 0x0001C9E8 File Offset: 0x0001ABE8
        // (set) Token: 0x06000AE0 RID: 2784 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
        public IEnumerable<MonoInstaller> Installers
        {
            get
            {
                return this._monoInstallers;
            }
            set
            {
                this._monoInstallers.Clear();
                this._monoInstallers.AddRange(value);
            }
        }

        // Token: 0x170000B3 RID: 179
        // (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0001CA0C File Offset: 0x0001AC0C
        // (set) Token: 0x06000AE2 RID: 2786 RVA: 0x0001CA14 File Offset: 0x0001AC14
        public IEnumerable<MonoInstaller> InstallerPrefabs
        {
            get
            {
                return this._installerPrefabs;
            }
            set
            {
                this._installerPrefabs.Clear();
                this._installerPrefabs.AddRange(value);
            }
        }

        // Token: 0x170000B4 RID: 180
        // (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0001CA30 File Offset: 0x0001AC30
        // (set) Token: 0x06000AE4 RID: 2788 RVA: 0x0001CA38 File Offset: 0x0001AC38
        public IEnumerable<ScriptableObjectInstaller> ScriptableObjectInstallers
        {
            get
            {
                return this._scriptableObjectInstallers;
            }
            set
            {
                this._scriptableObjectInstallers.Clear();
                this._scriptableObjectInstallers.AddRange(value);
            }
        }

        // Token: 0x170000B5 RID: 181
        // (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0001CA54 File Offset: 0x0001AC54
        // (set) Token: 0x06000AE6 RID: 2790 RVA: 0x0001CA5C File Offset: 0x0001AC5C
        public IEnumerable<Type> NormalInstallerTypes
        {
            get
            {
                return this._normalInstallerTypes;
            }
            set
            {
                ModestTree.Assert.That(value.All((Type x) => x != null && x.DerivesFrom<InstallerBase>()));
                this._normalInstallerTypes.Clear();
                this._normalInstallerTypes.AddRange(value);
            }
        }

        // Token: 0x170000B6 RID: 182
        // (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0001CAAC File Offset: 0x0001ACAC
        // (set) Token: 0x06000AE8 RID: 2792 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
        public IEnumerable<InstallerBase> NormalInstallers
        {
            get
            {
                return this._normalInstallers;
            }
            set
            {
                this._normalInstallers.Clear();
                this._normalInstallers.AddRange(value);
            }
        }

        // Token: 0x170000B7 RID: 183
        // (get) Token: 0x06000AE9 RID: 2793
        public abstract DiContainer Container { get; }

        // Token: 0x06000AEA RID: 2794
        public abstract IEnumerable<GameObject> GetRootGameObjects();

        // Token: 0x06000AEB RID: 2795 RVA: 0x0001CAD0 File Offset: 0x0001ACD0
        public void AddNormalInstallerType(Type installerType)
        {
            ModestTree.Assert.IsNotNull(installerType);
            ModestTree.Assert.That(installerType.DerivesFrom<InstallerBase>());
            this._normalInstallerTypes.Add(installerType);
        }

        // Token: 0x06000AEC RID: 2796 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
        public void AddNormalInstaller(InstallerBase installer)
        {
            this._normalInstallers.Add(installer);
        }

        // Token: 0x06000AED RID: 2797 RVA: 0x0001CB00 File Offset: 0x0001AD00
        private void CheckInstallerPrefabTypes(List<MonoInstaller> installers, List<MonoInstaller> installerPrefabs)
        {
            foreach (MonoInstaller val in installers)
            {
                ModestTree.Assert.IsNotNull(val, "Found null installer in Context '{0}'", base.name);
            }
            foreach (MonoInstaller monoInstaller in installerPrefabs)
            {
                ModestTree.Assert.IsNotNull(monoInstaller, "Found null prefab in Context");
                ModestTree.Assert.That(monoInstaller.GetComponent<MonoInstaller>() != null, "Expected to find component with type 'MonoInstaller' on given installer prefab '{0}'", monoInstaller.name);
            }
        }

        // Token: 0x06000AEE RID: 2798 RVA: 0x0001CBB4 File Offset: 0x0001ADB4
        protected void InstallInstallers()
        {
            this.InstallInstallers(this._normalInstallers, this._normalInstallerTypes, this._scriptableObjectInstallers, this._monoInstallers, this._installerPrefabs);
        }

        // Token: 0x06000AEF RID: 2799 RVA: 0x0001CBDC File Offset: 0x0001ADDC
        protected void InstallInstallers(List<InstallerBase> normalInstallers, List<Type> normalInstallerTypes, List<ScriptableObjectInstaller> scriptableObjectInstallers, List<MonoInstaller> installers, List<MonoInstaller> installerPrefabs)
        {
            this.CheckInstallerPrefabTypes(installers, installerPrefabs);
            List<IInstaller> list = normalInstallers.Cast<IInstaller>().Concat(scriptableObjectInstallers.Cast<IInstaller>()).Concat(installers.Cast<IInstaller>()).ToList<IInstaller>();
            foreach (MonoInstaller monoInstaller in installerPrefabs)
            {
                ModestTree.Assert.IsNotNull(monoInstaller, "Found null installer prefab in '{0}'", base.GetType());
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(monoInstaller.gameObject);
                gameObject.transform.SetParent(base.transform, false);
                MonoInstaller component = gameObject.GetComponent<MonoInstaller>();
                ModestTree.Assert.IsNotNull(component, "Could not find installer component on prefab '{0}'", monoInstaller.name);
                list.Add(component);
            }
            foreach (Type concreteType in normalInstallerTypes)
            {
                ((InstallerBase)this.Container.Instantiate(concreteType)).InstallBindings();
            }
            foreach (IInstaller installer in list)
            {
                ModestTree.Assert.IsNotNull(installer, "Found null installer in '{0}'", base.GetType());
                this.Container.Inject(installer);
                installer.InstallBindings();
            }
        }

        // Token: 0x06000AF0 RID: 2800 RVA: 0x0001CD4C File Offset: 0x0001AF4C
        protected void InstallSceneBindings(List<MonoBehaviour> injectableMonoBehaviours)
        {
            foreach (ZenjectBinding zenjectBinding in injectableMonoBehaviours.OfType<ZenjectBinding>())
            {
                if (!(zenjectBinding == null) && (zenjectBinding.Context == null || (zenjectBinding.UseSceneContext && this is SceneContext)))
                {
                    zenjectBinding.Context = this;
                }
            }
            foreach (ZenjectBinding zenjectBinding2 in Resources.FindObjectsOfTypeAll<ZenjectBinding>())
            {
                if (!(zenjectBinding2 == null))
                {
                    if (this is SceneContext && zenjectBinding2.Context == null && zenjectBinding2.UseSceneContext && zenjectBinding2.gameObject.scene == base.gameObject.scene)
                    {
                        zenjectBinding2.Context = this;
                    }
                    if (zenjectBinding2.Context == this)
                    {
                        this.InstallZenjectBinding(zenjectBinding2);
                    }
                }
            }
        }

        // Token: 0x06000AF1 RID: 2801 RVA: 0x0001CE40 File Offset: 0x0001B040
        private void InstallZenjectBinding(ZenjectBinding binding)
        {
            if (!binding.enabled)
            {
                return;
            }
            if (binding.Components == null || binding.Components.IsEmpty<Component>())
            {
                ModestTree.Log.Warn("Found empty list of components on ZenjectBinding on object '{0}'", new object[]
                {
                    binding.name
                });
                return;
            }
            string identifier = null;
            if (binding.Identifier.Trim().Length > 0)
            {
                identifier = binding.Identifier;
            }
            foreach (Component component in binding.Components)
            {
                ZenjectBinding.BindTypes bindType = binding.BindType;
                if (component == null)
                {
                    ModestTree.Log.Warn("Found null component in ZenjectBinding on object '{0}'", new object[]
                    {
                        binding.name
                    });
                }
                else
                {
                    Type type = component.GetType();
                    bool ifNotBound = binding.IfNotBound;
                    switch (bindType)
                    {
                        case ZenjectBinding.BindTypes.Self:
                            if (ifNotBound)
                            {
                                this.Container.Bind(new Type[]
                                {
                                type
                                }).WithId(identifier).FromInstance(component).IfNotBound();
                            }
                            else
                            {
                                this.Container.Bind(new Type[]
                                {
                                type
                                }).WithId(identifier).FromInstance(component);
                            }
                            break;
                        case ZenjectBinding.BindTypes.AllInterfaces:
                            if (ifNotBound)
                            {
                                this.Container.Bind(type.Interfaces()).WithId(identifier).FromInstance(component).IfNotBound();
                            }
                            else
                            {
                                this.Container.Bind(type.Interfaces()).WithId(identifier).FromInstance(component);
                            }
                            break;
                        case ZenjectBinding.BindTypes.AllInterfacesAndSelf:
                            if (ifNotBound)
                            {
                                this.Container.Bind(type.Interfaces().Concat(new Type[]
                                {
                                type
                                }).ToArray<Type>()).WithId(identifier).FromInstance(component).IfNotBound();
                            }
                            else
                            {
                                this.Container.Bind(type.Interfaces().Concat(new Type[]
                                {
                                type
                                }).ToArray<Type>()).WithId(identifier).FromInstance(component);
                            }
                            break;
                        case ZenjectBinding.BindTypes.BaseType:
                            if (ifNotBound)
                            {
                                this.Container.Bind(new Type[]
                                {
                                type.BaseType()
                                }).WithId(identifier).FromInstance(component).IfNotBound();
                            }
                            else
                            {
                                this.Container.Bind(new Type[]
                                {
                                type.BaseType()
                                }).WithId(identifier).FromInstance(component);
                            }
                            break;
                        default:
                            throw ModestTree.Assert.CreateException();
                    }
                }
            }
        }

        // Token: 0x06000AF2 RID: 2802
        protected abstract void GetInjectableMonoBehaviours(List<MonoBehaviour> components);

        // Token: 0x06000AF4 RID: 2804 RVA: 0x0001D0DC File Offset: 0x0001B2DC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Context), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000338 RID: 824
        [SerializeField]
        private List<ScriptableObjectInstaller> _scriptableObjectInstallers = new List<ScriptableObjectInstaller>();

        // Token: 0x04000339 RID: 825
        [SerializeField]
        [FormerlySerializedAs("Installers")]
        [FormerlySerializedAs("_installers")]
        private List<MonoInstaller> _monoInstallers = new List<MonoInstaller>();

        // Token: 0x0400033A RID: 826
        [SerializeField]
        private List<MonoInstaller> _installerPrefabs = new List<MonoInstaller>();

        // Token: 0x0400033B RID: 827
        private List<InstallerBase> _normalInstallers = new List<InstallerBase>();

        // Token: 0x0400033C RID: 828
        private List<Type> _normalInstallerTypes = new List<Type>();
    }
}
