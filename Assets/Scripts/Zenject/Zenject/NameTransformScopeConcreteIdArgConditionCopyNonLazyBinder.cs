using System;

namespace Zenject
{
    // Token: 0x02000147 RID: 327
    [NoReflectionBaking]
    public class NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder : TransformScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x06000713 RID: 1811 RVA: 0x00012EC8 File Offset: 0x000110C8
        public NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder(BindInfo bindInfo, GameObjectCreationParameters gameObjectInfo) : base(bindInfo, gameObjectInfo)
        {
        }

        // Token: 0x06000714 RID: 1812 RVA: 0x00012ED4 File Offset: 0x000110D4
        public TransformScopeConcreteIdArgConditionCopyNonLazyBinder WithGameObjectName(string gameObjectName)
        {
            base.GameObjectInfo.Name = gameObjectName;
            return this;
        }
    }
}
