using System;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002F6 RID: 758
    public class ZenAutoInjecter : MonoBehaviour
    {
        // Token: 0x1700015F RID: 351
        // (get) Token: 0x06001059 RID: 4185 RVA: 0x0002E15C File Offset: 0x0002C35C
        // (set) Token: 0x0600105A RID: 4186 RVA: 0x0002E164 File Offset: 0x0002C364
        public ZenAutoInjecter.ContainerSources ContainerSource
        {
            get
            {
                return this._containerSource;
            }
            set
            {
                this._containerSource = value;
            }
        }

        // Token: 0x0600105B RID: 4187 RVA: 0x0002E170 File Offset: 0x0002C370
        [Inject]
        public void Construct()
        {
            if (!this._hasInjected)
            {
                throw ModestTree.Assert.CreateException("ZenAutoInjecter was injected!  Do not use ZenAutoInjecter for objects that are instantiated through zenject or which exist in the initial scene hierarchy");
            }
        }

        // Token: 0x0600105C RID: 4188 RVA: 0x0002E188 File Offset: 0x0002C388
        public void Awake()
        {
            this._hasInjected = true;
            this.LookupContainer().InjectGameObject(base.gameObject);
        }

        // Token: 0x0600105D RID: 4189 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
        private DiContainer LookupContainer()
        {
            if (this._containerSource == ZenAutoInjecter.ContainerSources.ProjectContext)
            {
                return ProjectContext.Instance.Container;
            }
            if (this._containerSource == ZenAutoInjecter.ContainerSources.SceneContext)
            {
                return this.GetContainerForCurrentScene();
            }
            ModestTree.Assert.IsEqual(this._containerSource, ZenAutoInjecter.ContainerSources.SearchHierarchy);
            Context componentInParent = base.transform.GetComponentInParent<Context>();
            if (componentInParent != null)
            {
                return componentInParent.Container;
            }
            return this.GetContainerForCurrentScene();
        }

        // Token: 0x0600105E RID: 4190 RVA: 0x0002E20C File Offset: 0x0002C40C
        private DiContainer GetContainerForCurrentScene()
        {
            return ProjectContext.Instance.Container.Resolve<SceneContextRegistry>().GetContainerForScene(base.gameObject.scene);
        }

        // Token: 0x06001060 RID: 4192 RVA: 0x0002E240 File Offset: 0x0002C440
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((ZenAutoInjecter)P_0).Construct();
        }

        // Token: 0x06001061 RID: 4193 RVA: 0x0002E250 File Offset: 0x0002C450
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ZenAutoInjecter), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(ZenAutoInjecter.__zenInjectMethod0), new InjectableInfo[0], "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000529 RID: 1321
        [SerializeField]
        private ZenAutoInjecter.ContainerSources _containerSource = ZenAutoInjecter.ContainerSources.SearchHierarchy;

        // Token: 0x0400052A RID: 1322
        private bool _hasInjected;

        // Token: 0x020002F7 RID: 759
        public enum ContainerSources
        {
            // Token: 0x0400052C RID: 1324
            SceneContext,
            // Token: 0x0400052D RID: 1325
            ProjectContext,
            // Token: 0x0400052E RID: 1326
            SearchHierarchy
        }
    }
}
