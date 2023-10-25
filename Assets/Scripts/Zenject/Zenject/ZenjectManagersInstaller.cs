using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200022C RID: 556
    public class ZenjectManagersInstaller : Installer<ZenjectManagersInstaller>
    {
        // Token: 0x06000BE3 RID: 3043 RVA: 0x0001F9AC File Offset: 0x0001DBAC
        public override void InstallBindings()
        {
            base.Container.Bind(new Type[]
            {
                typeof(TickableManager),
                typeof(InitializableManager),
                typeof(DisposableManager)
            }).ToSelf().AsSingle().CopyIntoAllSubContainers();
        }

        // Token: 0x06000BE5 RID: 3045 RVA: 0x0001FA0C File Offset: 0x0001DC0C
        private static object __zenCreate(object[] P_0)
        {
            return new ZenjectManagersInstaller();
        }

        // Token: 0x06000BE6 RID: 3046 RVA: 0x0001FA24 File Offset: 0x0001DC24
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ZenjectManagersInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ZenjectManagersInstaller.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
