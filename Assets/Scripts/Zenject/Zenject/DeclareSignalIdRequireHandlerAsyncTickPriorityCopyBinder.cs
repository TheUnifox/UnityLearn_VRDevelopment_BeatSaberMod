using System;

namespace Zenject
{
    // Token: 0x02000032 RID: 50
    [NoReflectionBaking]
    public class DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder : DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder
    {
        // Token: 0x0600014C RID: 332 RVA: 0x00005438 File Offset: 0x00003638
        public DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder(SignalDeclarationBindInfo signalBindInfo) : base(signalBindInfo)
        {
        }

        // Token: 0x0600014D RID: 333 RVA: 0x00005444 File Offset: 0x00003644
        public DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder WithId(object identifier)
        {
            base.SignalBindInfo.Identifier = identifier;
            return this;
        }
    }
}
