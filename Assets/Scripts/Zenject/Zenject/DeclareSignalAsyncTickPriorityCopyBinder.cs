using System;

namespace Zenject
{
    // Token: 0x02000031 RID: 49
    [NoReflectionBaking]
    public class DeclareSignalAsyncTickPriorityCopyBinder : SignalTickPriorityCopyBinder
    {
        // Token: 0x06000149 RID: 329 RVA: 0x0000540C File Offset: 0x0000360C
        public DeclareSignalAsyncTickPriorityCopyBinder(SignalDeclarationBindInfo signalBindInfo) : base(signalBindInfo)
        {
        }

        // Token: 0x0600014A RID: 330 RVA: 0x00005418 File Offset: 0x00003618
        public SignalTickPriorityCopyBinder RunAsync()
        {
            base.SignalBindInfo.RunAsync = true;
            return this;
        }

        // Token: 0x0600014B RID: 331 RVA: 0x00005428 File Offset: 0x00003628
        public SignalCopyBinder RunSync()
        {
            base.SignalBindInfo.RunAsync = false;
            return this;
        }
    }
}
