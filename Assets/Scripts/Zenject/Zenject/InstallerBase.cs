using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000218 RID: 536
    public abstract class InstallerBase : IInstaller
    {
        // Token: 0x170000CE RID: 206
        // (get) Token: 0x06000B88 RID: 2952 RVA: 0x0001EE04 File Offset: 0x0001D004
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x170000CF RID: 207
        // (get) Token: 0x06000B89 RID: 2953 RVA: 0x0001EE0C File Offset: 0x0001D00C
        public virtual bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06000B8A RID: 2954
        public abstract void InstallBindings();

        // Token: 0x06000B8C RID: 2956 RVA: 0x0001EE18 File Offset: 0x0001D018
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((InstallerBase)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000B8D RID: 2957 RVA: 0x0001EE38 File Offset: 0x0001D038
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(InstallerBase), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(InstallerBase.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x04000379 RID: 889
        [Inject]
        private DiContainer _container;
    }
}
