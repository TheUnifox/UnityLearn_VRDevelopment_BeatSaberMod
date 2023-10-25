using System;

namespace Zenject
{
    // Token: 0x02000281 RID: 641
    [NoReflectionBaking]
    public class SubContainerCreatorBindInfo
    {
        // Token: 0x1700014E RID: 334
        // (get) Token: 0x06000E5E RID: 3678 RVA: 0x0002769C File Offset: 0x0002589C
        // (set) Token: 0x06000E5F RID: 3679 RVA: 0x000276A4 File Offset: 0x000258A4
        public string DefaultParentName { get; set; }

        // Token: 0x1700014F RID: 335
        // (get) Token: 0x06000E60 RID: 3680 RVA: 0x000276B0 File Offset: 0x000258B0
        // (set) Token: 0x06000E61 RID: 3681 RVA: 0x000276B8 File Offset: 0x000258B8
        public bool CreateKernel { get; set; }

        // Token: 0x17000150 RID: 336
        // (get) Token: 0x06000E62 RID: 3682 RVA: 0x000276C4 File Offset: 0x000258C4
        // (set) Token: 0x06000E63 RID: 3683 RVA: 0x000276CC File Offset: 0x000258CC
        public Type KernelType { get; set; }
    }
}
