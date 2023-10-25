using System;
using System.Collections.Generic;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002EE RID: 750
    public class ExecutionOrderInstaller : Installer<List<Type>, ExecutionOrderInstaller>
    {
        // Token: 0x0600102D RID: 4141 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
        public ExecutionOrderInstaller(List<Type> typeOrder)
        {
            this._typeOrder = typeOrder;
        }

        // Token: 0x0600102E RID: 4142 RVA: 0x0002DC00 File Offset: 0x0002BE00
        public override void InstallBindings()
        {
            int num = -1 * this._typeOrder.Count;
            foreach (Type type in this._typeOrder)
            {
                base.Container.BindExecutionOrder(type, num);
                num++;
            }
        }

        // Token: 0x0600102F RID: 4143 RVA: 0x0002DC6C File Offset: 0x0002BE6C
        private static object __zenCreate(object[] P_0)
        {
            return new ExecutionOrderInstaller((List<Type>)P_0[0]);
        }

        // Token: 0x06001030 RID: 4144 RVA: 0x0002DC90 File Offset: 0x0002BE90
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ExecutionOrderInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(ExecutionOrderInstaller.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "typeOrder", typeof(List<Type>), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000518 RID: 1304
        private List<Type> _typeOrder;
    }
}
