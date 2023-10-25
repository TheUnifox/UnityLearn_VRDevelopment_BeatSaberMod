using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002D0 RID: 720
    public class DefaultGameObjectKernel : MonoKernel
    {
        // Token: 0x06000F66 RID: 3942 RVA: 0x0002B7E4 File Offset: 0x000299E4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(DefaultGameObjectKernel), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
