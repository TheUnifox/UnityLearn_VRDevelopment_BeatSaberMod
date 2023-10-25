using System;
using System.Diagnostics;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000221 RID: 545
    [DebuggerStepThrough]
    public class MonoInstallerBase : MonoBehaviour, IInstaller
    {
        // Token: 0x170000D0 RID: 208
        // (get) Token: 0x06000BAC RID: 2988 RVA: 0x0001F308 File Offset: 0x0001D508
        // (set) Token: 0x06000BAD RID: 2989 RVA: 0x0001F310 File Offset: 0x0001D510
        [Inject]
        protected DiContainer Container { get; set; }

        // Token: 0x170000D1 RID: 209
        // (get) Token: 0x06000BAE RID: 2990 RVA: 0x0001F31C File Offset: 0x0001D51C
        public virtual bool IsEnabled
        {
            get
            {
                return base.enabled;
            }
        }

        // Token: 0x06000BAF RID: 2991 RVA: 0x0001F324 File Offset: 0x0001D524
        public virtual void Start()
        {
        }

        // Token: 0x06000BB0 RID: 2992 RVA: 0x0001F328 File Offset: 0x0001D528
        public virtual void InstallBindings()
        {
            throw new NotImplementedException();
        }

        // Token: 0x06000BB2 RID: 2994 RVA: 0x0001F338 File Offset: 0x0001D538
        private static void __zenPropertySetter0(object P_0, object P_1)
        {
            ((MonoInstallerBase)P_0).Container = (DiContainer)P_1;
        }

        // Token: 0x06000BB3 RID: 2995 RVA: 0x0001F358 File Offset: 0x0001D558
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoInstallerBase), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(MonoInstallerBase.__zenPropertySetter0), new InjectableInfo(false, null, "Container", typeof(DiContainer), null, InjectSources.Any))
            });
        }
    }
}
