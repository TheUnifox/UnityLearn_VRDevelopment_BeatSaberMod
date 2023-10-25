using System;

namespace Zenject
{
    // Token: 0x02000037 RID: 55
    public static class SignalExtensions
    {
        // Token: 0x0600016A RID: 362 RVA: 0x000055E4 File Offset: 0x000037E4
        public static SignalDeclarationBindInfo CreateDefaultSignalDeclarationBindInfo(DiContainer container, Type signalType)
        {
            return new SignalDeclarationBindInfo(signalType)
            {
                RunAsync = (container.Settings.Signals.DefaultSyncMode == SignalDefaultSyncModes.Asynchronous),
                MissingHandlerResponse = container.Settings.Signals.MissingHandlerDefaultResponse,
                TickPriority = container.Settings.Signals.DefaultAsyncTickPriority
            };
        }

        // Token: 0x0600016B RID: 363 RVA: 0x0000563C File Offset: 0x0000383C
        public static DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal>(this DiContainer container)
        {
            SignalDeclarationBindInfo signalDeclarationBindInfo = SignalExtensions.CreateDefaultSignalDeclarationBindInfo(container, typeof(TSignal));
            BindInfo bindInfo = container.Bind<SignalDeclaration>().AsCached().WithArguments<SignalDeclarationBindInfo>(signalDeclarationBindInfo).WhenInjectedInto(new Type[]
            {
                typeof(SignalBus),
                typeof(SignalDeclarationAsyncInitializer)
            }).BindInfo;
            DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder declareSignalIdRequireHandlerAsyncTickPriorityCopyBinder = new DeclareSignalIdRequireHandlerAsyncTickPriorityCopyBinder(signalDeclarationBindInfo);
            declareSignalIdRequireHandlerAsyncTickPriorityCopyBinder.AddCopyBindInfo(bindInfo);
            return declareSignalIdRequireHandlerAsyncTickPriorityCopyBinder;
        }

        // Token: 0x0600016C RID: 364 RVA: 0x000056A4 File Offset: 0x000038A4
        public static BindSignalIdToBinder<TSignal> BindSignal<TSignal>(this DiContainer container)
        {
            SignalBindingBindInfo signalBindInfo = new SignalBindingBindInfo(typeof(TSignal));
            return new BindSignalIdToBinder<TSignal>(container, signalBindInfo);
        }
    }
}
