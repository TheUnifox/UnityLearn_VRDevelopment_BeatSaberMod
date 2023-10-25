using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002D5 RID: 725
    public class SceneKernel : MonoKernel
    {
        // Token: 0x06000F8B RID: 3979 RVA: 0x0002BE8C File Offset: 0x0002A08C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SceneKernel), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
