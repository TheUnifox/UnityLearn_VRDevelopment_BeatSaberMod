using System;

namespace Zenject
{
    // Token: 0x02000035 RID: 53
    [NoReflectionBaking]
    public class SignalDeclarationBindInfo
    {
        // Token: 0x06000157 RID: 343 RVA: 0x000054C8 File Offset: 0x000036C8
        public SignalDeclarationBindInfo(Type signalType)
        {
            this.SignalType = signalType;
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000158 RID: 344 RVA: 0x000054D8 File Offset: 0x000036D8
        // (set) Token: 0x06000159 RID: 345 RVA: 0x000054E0 File Offset: 0x000036E0
        public object Identifier { get; set; }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x0600015A RID: 346 RVA: 0x000054EC File Offset: 0x000036EC
        // (set) Token: 0x0600015B RID: 347 RVA: 0x000054F4 File Offset: 0x000036F4
        public Type SignalType { get; private set; }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x0600015C RID: 348 RVA: 0x00005500 File Offset: 0x00003700
        // (set) Token: 0x0600015D RID: 349 RVA: 0x00005508 File Offset: 0x00003708
        public bool RunAsync { get; set; }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x0600015E RID: 350 RVA: 0x00005514 File Offset: 0x00003714
        // (set) Token: 0x0600015F RID: 351 RVA: 0x0000551C File Offset: 0x0000371C
        public int TickPriority { get; set; }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x06000160 RID: 352 RVA: 0x00005528 File Offset: 0x00003728
        // (set) Token: 0x06000161 RID: 353 RVA: 0x00005530 File Offset: 0x00003730
        public SignalMissingHandlerResponses MissingHandlerResponse { get; set; }
    }
}
