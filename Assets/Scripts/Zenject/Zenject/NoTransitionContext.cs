using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200023E RID: 574
    public class NoTransitionContext : MonoBehaviour
    {
        // Token: 0x170000F5 RID: 245
        // (get) Token: 0x06000D38 RID: 3384 RVA: 0x00023998 File Offset: 0x00021B98
        public Action<DiContainer> installMethod
        {
            get
            {
                return new Action<DiContainer>(this._noScenesTransitionInstaller.InstallBindings);
            }
        }

        // Token: 0x170000F6 RID: 246
        // (get) Token: 0x06000D39 RID: 3385 RVA: 0x000239AC File Offset: 0x00021BAC
        public Action<DiContainer> postInstallMethod
        {
            get
            {
                return new Action<DiContainer>(this._noScenesTransitionInstaller.PostInstall);
            }
        }

        // Token: 0x06000D3A RID: 3386 RVA: 0x000239C0 File Offset: 0x00021BC0
        protected void Awake()
        {
            if (SceneContext.ExtraBindingsEarlyInstallMethod != null)
            {
                return;
            }
            SceneContext.ExtraBindingsEarlyInstallMethod = this.installMethod;
            SceneContext.ExtraPostInstallMethod = this.postInstallMethod;
        }

        // Token: 0x06000D3C RID: 3388 RVA: 0x000239E8 File Offset: 0x00021BE8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(NoTransitionContext), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040003D2 RID: 978
        [SerializeField]
        private NoTransitionInstaller _noScenesTransitionInstaller;
    }
}
