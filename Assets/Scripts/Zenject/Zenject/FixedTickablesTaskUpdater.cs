using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002E3 RID: 739
    public class FixedTickablesTaskUpdater : TaskUpdater<IFixedTickable>
    {
        // Token: 0x06000FD5 RID: 4053 RVA: 0x0002CBAC File Offset: 0x0002ADAC
        protected override void UpdateItem(IFixedTickable task)
        {
            task.FixedTick();
        }

        // Token: 0x06000FD7 RID: 4055 RVA: 0x0002CBBC File Offset: 0x0002ADBC
        private static object __zenCreate(object[] P_0)
        {
            return new FixedTickablesTaskUpdater();
        }

        // Token: 0x06000FD8 RID: 4056 RVA: 0x0002CBD4 File Offset: 0x0002ADD4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(FixedTickablesTaskUpdater), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(FixedTickablesTaskUpdater.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
