using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000027 RID: 39
    public class BindSignalIdToBinder<TSignal> : BindSignalToBinder<TSignal>
    {
        // Token: 0x0600011A RID: 282 RVA: 0x000049F4 File Offset: 0x00002BF4
        public BindSignalIdToBinder(DiContainer container, SignalBindingBindInfo signalBindInfo) : base(container, signalBindInfo)
        {
        }

        // Token: 0x0600011B RID: 283 RVA: 0x00004A00 File Offset: 0x00002C00
        public BindSignalToBinder<TSignal> WithId(object identifier)
        {
            base.SignalBindInfo.Identifier = identifier;
            return this;
        }

        // Token: 0x0600011C RID: 284 RVA: 0x00004A10 File Offset: 0x00002C10
        private static object __zenCreate(object[] P_0)
        {
            return new BindSignalIdToBinder<TSignal>((DiContainer)P_0[0], (SignalBindingBindInfo)P_0[1]);
        }

        // Token: 0x0600011D RID: 285 RVA: 0x00004A40 File Offset: 0x00002C40
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(BindSignalIdToBinder<TSignal>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(BindSignalIdToBinder<TSignal>.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any),
                new InjectableInfo(false, null, "signalBindInfo", typeof(SignalBindingBindInfo), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
