using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200022A RID: 554
    public class ZenjectBinding : MonoBehaviour
    {
        // Token: 0x170000D4 RID: 212
        // (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0001F914 File Offset: 0x0001DB14
        public bool UseSceneContext
        {
            get
            {
                return this._useSceneContext;
            }
        }

        // Token: 0x170000D5 RID: 213
        // (get) Token: 0x06000BDA RID: 3034 RVA: 0x0001F91C File Offset: 0x0001DB1C
        public bool IfNotBound
        {
            get
            {
                return this._ifNotBound;
            }
        }

        // Token: 0x170000D6 RID: 214
        // (get) Token: 0x06000BDB RID: 3035 RVA: 0x0001F924 File Offset: 0x0001DB24
        // (set) Token: 0x06000BDC RID: 3036 RVA: 0x0001F92C File Offset: 0x0001DB2C
        public Context Context
        {
            get
            {
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        // Token: 0x170000D7 RID: 215
        // (get) Token: 0x06000BDD RID: 3037 RVA: 0x0001F938 File Offset: 0x0001DB38
        public Component[] Components
        {
            get
            {
                return this._components;
            }
        }

        // Token: 0x170000D8 RID: 216
        // (get) Token: 0x06000BDE RID: 3038 RVA: 0x0001F940 File Offset: 0x0001DB40
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
        }

        // Token: 0x170000D9 RID: 217
        // (get) Token: 0x06000BDF RID: 3039 RVA: 0x0001F948 File Offset: 0x0001DB48
        public ZenjectBinding.BindTypes BindType
        {
            get
            {
                return this._bindType;
            }
        }

        // Token: 0x06000BE0 RID: 3040 RVA: 0x0001F950 File Offset: 0x0001DB50
        public void Start()
        {
        }

        // Token: 0x06000BE2 RID: 3042 RVA: 0x0001F968 File Offset: 0x0001DB68
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ZenjectBinding), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x0400037C RID: 892
        [Tooltip("The component to add to the Zenject container")]
        [SerializeField]
        private Component[] _components;

        // Token: 0x0400037D RID: 893
        [Tooltip("Note: This value is optional and can be ignored in most cases.  This can be useful to differentiate multiple bindings of the same type.  For example, if you have multiple cameras in your scene, you can 'name' them by giving each one a different identifier.  For your main camera you might call it 'Main' then any class can refer to it by using an attribute like [Inject(Id = 'Main')]")]
        [SerializeField]
        private string _identifier = string.Empty;

        // Token: 0x0400037E RID: 894
        [Tooltip("When set, this will bind the given components to the SceneContext.  It can be used as a shortcut to explicitly dragging the SceneContext into the Context field.  This is useful when using ZenjectBinding inside GameObjectContext.  If your ZenjectBinding is for a component that is not underneath GameObjectContext then it is not necessary to check this")]
        [SerializeField]
        private bool _useSceneContext;

        // Token: 0x0400037F RID: 895
        [SerializeField]
        private bool _ifNotBound;

        // Token: 0x04000380 RID: 896
        [FormerlySerializedAs("_compositionRoot")]
        [Tooltip("Note: This value is optional and can be ignored in most cases.  This value will determine what container the component gets added to.  If unset, the component will be bound on the most 'local' context.  In most cases this will be the SceneContext, unless this component is underneath a GameObjectContext, or ProjectContext, in which case it will bind to that instead by default.  You can also override this default by providing the Context directly.  This can be useful if you want to bind something that is inside a GameObjectContext to the SceneContext container.")]
        [SerializeField]
        private Context _context;

        // Token: 0x04000381 RID: 897
        [Tooltip("This value is used to determine how to bind this component.  When set to 'Self' is equivalent to calling Container.FromInstance inside an installer. When set to 'AllInterfaces' this is equivalent to calling 'Container.BindInterfaces<MyMonoBehaviour>().ToInstance', and similarly for InterfacesAndSelf")]
        [SerializeField]
        private ZenjectBinding.BindTypes _bindType;

        // Token: 0x0200022B RID: 555
        public enum BindTypes
        {
            // Token: 0x04000383 RID: 899
            Self,
            // Token: 0x04000384 RID: 900
            AllInterfaces,
            // Token: 0x04000385 RID: 901
            AllInterfacesAndSelf,
            // Token: 0x04000386 RID: 902
            BaseType
        }
    }
}
