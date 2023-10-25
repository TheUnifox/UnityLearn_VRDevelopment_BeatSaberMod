using System;

namespace Zenject
{
    // Token: 0x02000034 RID: 52
    [NoReflectionBaking]
    public class SignalBindingBindInfo
    {
        // Token: 0x06000152 RID: 338 RVA: 0x00005490 File Offset: 0x00003690
        public SignalBindingBindInfo(Type signalType)
        {
            this.SignalType = signalType;
        }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000153 RID: 339 RVA: 0x000054A0 File Offset: 0x000036A0
        // (set) Token: 0x06000154 RID: 340 RVA: 0x000054A8 File Offset: 0x000036A8
        public object Identifier { get; set; }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000155 RID: 341 RVA: 0x000054B4 File Offset: 0x000036B4
        // (set) Token: 0x06000156 RID: 342 RVA: 0x000054BC File Offset: 0x000036BC
        public Type SignalType { get; private set; }
    }
}
