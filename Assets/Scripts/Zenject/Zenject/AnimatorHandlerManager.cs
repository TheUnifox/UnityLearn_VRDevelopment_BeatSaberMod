using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002BB RID: 699
    public class AnimatorIkHandlerManager : MonoBehaviour
    {
        // Token: 0x06000F0A RID: 3850 RVA: 0x0002A364 File Offset: 0x00028564
        [Inject]
        public void Construct([Inject(Source = InjectSources.Local)] List<IAnimatorIkHandler> handlers)
        {
            this._handlers = handlers;
        }

        // Token: 0x06000F0B RID: 3851 RVA: 0x0002A370 File Offset: 0x00028570
        public void OnAnimatorIk()
        {
            foreach (IAnimatorIkHandler animatorIkHandler in this._handlers)
            {
                animatorIkHandler.OnAnimatorIk();
            }
        }

        // Token: 0x06000F0D RID: 3853 RVA: 0x0002A3C8 File Offset: 0x000285C8
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((AnimatorIkHandlerManager)P_0).Construct((List<IAnimatorIkHandler>)P_1[0]);
        }

        // Token: 0x06000F0E RID: 3854 RVA: 0x0002A3E4 File Offset: 0x000285E4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(AnimatorIkHandlerManager), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(AnimatorIkHandlerManager.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "handlers", typeof(List<IAnimatorIkHandler>), null, InjectSources.Local)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004B7 RID: 1207
        private List<IAnimatorIkHandler> _handlers;
    }
}
