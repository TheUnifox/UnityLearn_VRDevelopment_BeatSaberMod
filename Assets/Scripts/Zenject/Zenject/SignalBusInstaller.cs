using System;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000042 RID: 66
    public class SignalBusInstaller : Installer<SignalBusInstaller>
    {
        // Token: 0x060001D7 RID: 471 RVA: 0x00006770 File Offset: 0x00004970
        public override void InstallBindings()
        {
            ModestTree.Assert.That(!base.Container.HasBinding<SignalBus>(), "Detected multiple SignalBus bindings.  SignalBusInstaller should only be installed once");
            base.Container.BindInterfacesAndSelfTo<SignalBus>().AsSingle().CopyIntoAllSubContainers();
            base.Container.BindInterfacesTo<SignalDeclarationAsyncInitializer>().AsSingle().CopyIntoAllSubContainers();
            base.Container.BindMemoryPool<SignalSubscription, SignalSubscription.Pool>().ExpandByOneAtATime(false);
            base.Container.BindLateDisposableExecutionOrder<SignalBus>(-999);
            base.Container.BindFactory<SignalDeclarationBindInfo, SignalDeclaration, SignalDeclaration.Factory>();
        }

        // Token: 0x060001D9 RID: 473 RVA: 0x000067F8 File Offset: 0x000049F8
        private static object __zenCreate(object[] P_0)
        {
            return new SignalBusInstaller();
        }

        // Token: 0x060001DA RID: 474 RVA: 0x00006810 File Offset: 0x00004A10
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalBusInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalBusInstaller.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}