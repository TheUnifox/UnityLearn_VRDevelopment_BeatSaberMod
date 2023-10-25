using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002BD RID: 701
    public class AnimatorMoveHandlerManager : MonoBehaviour
    {
        // Token: 0x06000F13 RID: 3859 RVA: 0x0002A558 File Offset: 0x00028758
        [Inject]
        public void Construct([Inject(Source = InjectSources.Local)] List<IAnimatorMoveHandler> handlers)
        {
            this._handlers = handlers;
        }

        // Token: 0x06000F14 RID: 3860 RVA: 0x0002A564 File Offset: 0x00028764
        public void OnAnimatorMove()
        {
            foreach (IAnimatorMoveHandler animatorMoveHandler in this._handlers)
            {
                animatorMoveHandler.OnAnimatorMove();
            }
        }

        // Token: 0x06000F16 RID: 3862 RVA: 0x0002A5BC File Offset: 0x000287BC
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((AnimatorMoveHandlerManager)P_0).Construct((List<IAnimatorMoveHandler>)P_1[0]);
        }

        // Token: 0x06000F17 RID: 3863 RVA: 0x0002A5D8 File Offset: 0x000287D8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(AnimatorMoveHandlerManager), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(AnimatorMoveHandlerManager.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "handlers", typeof(List<IAnimatorMoveHandler>), null, InjectSources.Local)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004B9 RID: 1209
        private List<IAnimatorMoveHandler> _handlers;
    }
}
