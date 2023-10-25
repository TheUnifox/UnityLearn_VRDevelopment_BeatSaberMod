using System;

namespace Zenject
{
    // Token: 0x02000033 RID: 51
    [NoReflectionBaking]
    public class DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder : DeclareSignalAsyncTickPriorityCopyBinder
    {
        // Token: 0x0600014E RID: 334 RVA: 0x00005454 File Offset: 0x00003654
        public DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder(SignalDeclarationBindInfo signalBindInfo) : base(signalBindInfo)
        {
        }

        // Token: 0x0600014F RID: 335 RVA: 0x00005460 File Offset: 0x00003660
        public DeclareSignalAsyncTickPriorityCopyBinder RequireSubscriber()
        {
            base.SignalBindInfo.MissingHandlerResponse = SignalMissingHandlerResponses.Throw;
            return this;
        }

        // Token: 0x06000150 RID: 336 RVA: 0x00005470 File Offset: 0x00003670
        public DeclareSignalAsyncTickPriorityCopyBinder OptionalSubscriber()
        {
            base.SignalBindInfo.MissingHandlerResponse = SignalMissingHandlerResponses.Ignore;
            return this;
        }

        // Token: 0x06000151 RID: 337 RVA: 0x00005480 File Offset: 0x00003680
        public DeclareSignalAsyncTickPriorityCopyBinder OptionalSubscriberWithWarning()
        {
            base.SignalBindInfo.MissingHandlerResponse = SignalMissingHandlerResponses.Warn;
            return this;
        }
    }
}
