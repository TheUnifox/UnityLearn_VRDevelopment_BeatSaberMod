using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000229 RID: 553
    public class ScriptableObjectInstallerBase : ScriptableObject, IInstaller
    {
        // Token: 0x170000D2 RID: 210
        // (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0001F83C File Offset: 0x0001DA3C
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x170000D3 RID: 211
        // (get) Token: 0x06000BD3 RID: 3027 RVA: 0x0001F844 File Offset: 0x0001DA44
        bool IInstaller.IsEnabled
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06000BD4 RID: 3028 RVA: 0x0001F848 File Offset: 0x0001DA48
        public virtual void InstallBindings()
        {
            throw new NotImplementedException();
        }

        // Token: 0x06000BD6 RID: 3030 RVA: 0x0001F858 File Offset: 0x0001DA58
        private static object __zenCreate(object[] P_0)
        {
            return new ScriptableObjectInstallerBase();
        }

        // Token: 0x06000BD7 RID: 3031 RVA: 0x0001F870 File Offset: 0x0001DA70
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((ScriptableObjectInstallerBase)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x06000BD8 RID: 3032 RVA: 0x0001F890 File Offset: 0x0001DA90
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ScriptableObjectInstallerBase), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ScriptableObjectInstallerBase.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(ScriptableObjectInstallerBase.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any))
            });
        }

        // Token: 0x0400037B RID: 891
        [Inject]
        private DiContainer _container;
    }
}
