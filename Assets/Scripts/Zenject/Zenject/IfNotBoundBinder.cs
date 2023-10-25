using System;

namespace Zenject
{
    // Token: 0x0200014B RID: 331
    [NoReflectionBaking]
    public class IfNotBoundBinder
    {
        // Token: 0x0600071F RID: 1823 RVA: 0x00012F74 File Offset: 0x00011174
        public IfNotBoundBinder(BindInfo bindInfo)
        {
            this.BindInfo = bindInfo;
        }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x06000720 RID: 1824 RVA: 0x00012F84 File Offset: 0x00011184
        // (set) Token: 0x06000721 RID: 1825 RVA: 0x00012F8C File Offset: 0x0001118C
        public BindInfo BindInfo { get; private set; }

        // Token: 0x06000722 RID: 1826 RVA: 0x00012F98 File Offset: 0x00011198
        public void IfNotBound()
        {
            this.BindInfo.OnlyBindIfNotBound = true;
        }
    }
}
