using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000148 RID: 328
    [NoReflectionBaking]
    public class TransformScopeConcreteIdArgConditionCopyNonLazyBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x06000715 RID: 1813 RVA: 0x00012EE4 File Offset: 0x000110E4
        public TransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo bindInfo, GameObjectCreationParameters gameObjectInfo) : base(bindInfo)
        {
            this.GameObjectInfo = gameObjectInfo;
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x06000716 RID: 1814 RVA: 0x00012EF4 File Offset: 0x000110F4
        // (set) Token: 0x06000717 RID: 1815 RVA: 0x00012EFC File Offset: 0x000110FC
        protected GameObjectCreationParameters GameObjectInfo { get; private set; }

        // Token: 0x06000718 RID: 1816 RVA: 0x00012F08 File Offset: 0x00011108
        public ScopeConcreteIdArgConditionCopyNonLazyBinder UnderTransform(Transform parent)
        {
            this.GameObjectInfo.ParentTransform = parent;
            return this;
        }

        // Token: 0x06000719 RID: 1817 RVA: 0x00012F18 File Offset: 0x00011118
        public ScopeConcreteIdArgConditionCopyNonLazyBinder UnderTransform(Func<InjectContext, Transform> parentGetter)
        {
            this.GameObjectInfo.ParentTransformGetter = parentGetter;
            return this;
        }

        // Token: 0x0600071A RID: 1818 RVA: 0x00012F28 File Offset: 0x00011128
        public ScopeConcreteIdArgConditionCopyNonLazyBinder UnderTransformGroup(string transformGroupname)
        {
            this.GameObjectInfo.GroupName = transformGroupname;
            return this;
        }
    }
}
