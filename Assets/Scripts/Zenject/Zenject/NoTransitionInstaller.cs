using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200023F RID: 575
    public class NoTransitionInstaller : MonoBehaviour
    {
        // Token: 0x06000D3D RID: 3389 RVA: 0x00023A2C File Offset: 0x00021C2C
        public virtual void InstallBindings(DiContainer container)
        {
        }

        // Token: 0x06000D3E RID: 3390 RVA: 0x00023A30 File Offset: 0x00021C30
        public virtual void PostInstall(DiContainer container)
        {
        }

        // Token: 0x06000D40 RID: 3392 RVA: 0x00023A3C File Offset: 0x00021C3C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(NoTransitionInstaller), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}