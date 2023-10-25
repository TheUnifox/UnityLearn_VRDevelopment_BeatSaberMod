using System;

namespace Zenject
{
    // Token: 0x0200015D RID: 349
    [NoReflectionBaking]
    public class WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder : DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x06000775 RID: 1909 RVA: 0x00013D94 File Offset: 0x00011F94
        public WithKernelDefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder(SubContainerCreatorBindInfo subContainerBindInfo, BindInfo bindInfo) : base(subContainerBindInfo, bindInfo)
        {
        }

        // Token: 0x06000776 RID: 1910 RVA: 0x00013DA0 File Offset: 0x00011FA0
        public DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder WithKernel()
        {
            base.SubContainerCreatorBindInfo.CreateKernel = true;
            return this;
        }

        // Token: 0x06000777 RID: 1911 RVA: 0x00013DB0 File Offset: 0x00011FB0
        public DefaultParentScopeConcreteIdArgConditionCopyNonLazyBinder WithKernel<TKernel>() where TKernel : Kernel
        {
            base.SubContainerCreatorBindInfo.CreateKernel = true;
            base.SubContainerCreatorBindInfo.KernelType = typeof(TKernel);
            return this;
        }
    }
}
