using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002E9 RID: 745
    public class ActionInstaller : Installer<ActionInstaller>
    {
        // Token: 0x0600100C RID: 4108 RVA: 0x0002D600 File Offset: 0x0002B800
        public ActionInstaller(Action<DiContainer> installMethod)
        {
            this._installMethod = installMethod;
        }

        // Token: 0x0600100D RID: 4109 RVA: 0x0002D610 File Offset: 0x0002B810
        public override void InstallBindings()
        {
            this._installMethod(base.Container);
        }

        // Token: 0x0600100E RID: 4110 RVA: 0x0002D624 File Offset: 0x0002B824
        private static object __zenCreate(object[] P_0)
        {
            return new ActionInstaller((Action<DiContainer>)P_0[0]);
        }

        // Token: 0x0600100F RID: 4111 RVA: 0x0002D648 File Offset: 0x0002B848
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ActionInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ActionInstaller.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "installMethod", typeof(Action<DiContainer>), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000510 RID: 1296
        private readonly Action<DiContainer> _installMethod;
    }
}
