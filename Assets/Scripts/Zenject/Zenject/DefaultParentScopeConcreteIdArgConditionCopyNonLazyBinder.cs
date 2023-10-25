using System;

namespace Zenject
{
    // Token: 0x0200006F RID: 111
    [NoReflectionBaking]
    public class DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x060002C7 RID: 711 RVA: 0x0000888C File Offset: 0x00006A8C
        public DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(SubContainerCreatorBindInfo subContainerBindInfo, BindInfo bindInfo) : base(bindInfo)
        {
            this.SubContainerCreatorBindInfo = subContainerBindInfo;
        }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x060002C8 RID: 712 RVA: 0x0000889C File Offset: 0x00006A9C
        // (set) Token: 0x060002C9 RID: 713 RVA: 0x000088A4 File Offset: 0x00006AA4
        protected SubContainerCreatorBindInfo SubContainerCreatorBindInfo { get; private set; }

        // Token: 0x060002CA RID: 714 RVA: 0x000088B0 File Offset: 0x00006AB0
        public ScopeConcreteIdArgConditionCopyNonLazyBinder WithDefaultGameObjectParent(string defaultParentName)
        {
            this.SubContainerCreatorBindInfo.DefaultParentName = defaultParentName;
            return this;
        }
    }
}
