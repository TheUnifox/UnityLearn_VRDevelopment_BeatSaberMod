using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002BC RID: 700
    public class AnimatorInstaller : Installer<Animator, AnimatorInstaller>
    {
        // Token: 0x06000F0F RID: 3855 RVA: 0x0002A474 File Offset: 0x00028674
        public AnimatorInstaller(Animator animator)
        {
            this._animator = animator;
        }

        // Token: 0x06000F10 RID: 3856 RVA: 0x0002A484 File Offset: 0x00028684
        public override void InstallBindings()
        {
            base.Container.Bind<AnimatorIkHandlerManager>().FromNewComponentOn(this._animator.gameObject);
            base.Container.Bind<AnimatorIkHandlerManager>().FromNewComponentOn(this._animator.gameObject);
        }

        // Token: 0x06000F11 RID: 3857 RVA: 0x0002A4C0 File Offset: 0x000286C0
        private static object __zenCreate(object[] P_0)
        {
            return new AnimatorInstaller((Animator)P_0[0]);
        }

        // Token: 0x06000F12 RID: 3858 RVA: 0x0002A4E4 File Offset: 0x000286E4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(AnimatorInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(AnimatorInstaller.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "animator", typeof(Animator), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004B8 RID: 1208
        private readonly Animator _animator;
    }
}
