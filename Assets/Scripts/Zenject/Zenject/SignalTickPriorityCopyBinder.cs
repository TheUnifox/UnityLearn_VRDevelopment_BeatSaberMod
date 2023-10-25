using System;

namespace Zenject
{
    // Token: 0x02000038 RID: 56
    [NoReflectionBaking]
    public class SignalTickPriorityCopyBinder : SignalCopyBinder
    {
        // Token: 0x0600016D RID: 365 RVA: 0x000056C8 File Offset: 0x000038C8
        public SignalTickPriorityCopyBinder(SignalDeclarationBindInfo signalBindInfo)
        {
            this.SignalBindInfo = signalBindInfo;
        }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x0600016E RID: 366 RVA: 0x000056D8 File Offset: 0x000038D8
        // (set) Token: 0x0600016F RID: 367 RVA: 0x000056E0 File Offset: 0x000038E0
        protected SignalDeclarationBindInfo SignalBindInfo { get; private set; }

        // Token: 0x06000170 RID: 368 RVA: 0x000056EC File Offset: 0x000038EC
        public SignalCopyBinder WithTickPriority(int priority)
        {
            this.SignalBindInfo.TickPriority = priority;
            this.SignalBindInfo.RunAsync = true;
            return this;
        }
    }
}
