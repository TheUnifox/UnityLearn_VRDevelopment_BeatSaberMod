using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002E2 RID: 738
    public class LateTickablesTaskUpdater : TaskUpdater<ILateTickable>
    {
        // Token: 0x06000FD1 RID: 4049 RVA: 0x0002CB34 File Offset: 0x0002AD34
        protected override void UpdateItem(ILateTickable task)
        {
            task.LateTick();
        }

        // Token: 0x06000FD3 RID: 4051 RVA: 0x0002CB44 File Offset: 0x0002AD44
        private static object __zenCreate(object[] P_0)
        {
            return new LateTickablesTaskUpdater();
        }

        // Token: 0x06000FD4 RID: 4052 RVA: 0x0002CB5C File Offset: 0x0002AD5C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(LateTickablesTaskUpdater), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(LateTickablesTaskUpdater.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
