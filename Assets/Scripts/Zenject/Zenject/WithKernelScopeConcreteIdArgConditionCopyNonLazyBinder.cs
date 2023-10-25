using System;

namespace Zenject
{
    // Token: 0x0200015E RID: 350
    [NoReflectionBaking]
    public class WithKernelScopeConcreteIdArgConditionCopyNonLazyBinder : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        // Token: 0x06000778 RID: 1912 RVA: 0x00013DD4 File Offset: 0x00011FD4
        public WithKernelScopeConcreteIdArgConditionCopyNonLazyBinder(SubContainerCreatorBindInfo subContainerBindInfo, BindInfo bindInfo) : base(bindInfo)
        {
            this._subContainerBindInfo = subContainerBindInfo;
        }

        // Token: 0x06000779 RID: 1913 RVA: 0x00013DE4 File Offset: 0x00011FE4
        public ScopeConcreteIdArgConditionCopyNonLazyBinder WithKernel()
        {
            this._subContainerBindInfo.CreateKernel = true;
            return this;
        }

        // Token: 0x0600077A RID: 1914 RVA: 0x00013DF4 File Offset: 0x00011FF4
        public ScopeConcreteIdArgConditionCopyNonLazyBinder WithKernel<TKernel>() where TKernel : Kernel
        {
            this._subContainerBindInfo.CreateKernel = true;
            this._subContainerBindInfo.KernelType = typeof(TKernel);
            return this;
        }

        // Token: 0x04000282 RID: 642
        private SubContainerCreatorBindInfo _subContainerBindInfo;
    }
}
