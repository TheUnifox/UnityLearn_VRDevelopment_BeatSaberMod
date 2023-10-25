using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002E1 RID: 737
    public class TickablesTaskUpdater : TaskUpdater<ITickable>
    {
        // Token: 0x06000FCD RID: 4045 RVA: 0x0002CABC File Offset: 0x0002ACBC
        protected override void UpdateItem(ITickable task)
        {
            task.Tick();
        }

        // Token: 0x06000FCF RID: 4047 RVA: 0x0002CACC File Offset: 0x0002ACCC
        private static object __zenCreate(object[] P_0)
        {
            return new TickablesTaskUpdater();
        }

        // Token: 0x06000FD0 RID: 4048 RVA: 0x0002CAE4 File Offset: 0x0002ACE4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(TickablesTaskUpdater), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(TickablesTaskUpdater.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
