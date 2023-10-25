using System;

namespace Zenject
{
    // Token: 0x0200014E RID: 334
    [NoReflectionBaking]
    public class NonLazyBinder : IfNotBoundBinder
    {
        // Token: 0x0600072A RID: 1834 RVA: 0x000130A8 File Offset: 0x000112A8
        public NonLazyBinder(BindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x0600072B RID: 1835 RVA: 0x000130B4 File Offset: 0x000112B4
        public IfNotBoundBinder NonLazy()
        {
            base.BindInfo.NonLazy = true;
            return this;
        }

        // Token: 0x0600072C RID: 1836 RVA: 0x000130C4 File Offset: 0x000112C4
        public IfNotBoundBinder Lazy()
        {
            base.BindInfo.NonLazy = false;
            return this;
        }
    }
}
