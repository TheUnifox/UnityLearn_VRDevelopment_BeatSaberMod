using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000030 RID: 48
    public class SignalCallbackWrapper : IDisposable
    {
        // Token: 0x06000144 RID: 324 RVA: 0x00005288 File Offset: 0x00003488
        public SignalCallbackWrapper(SignalBindingBindInfo bindInfo, Action<object> action, SignalBus signalBus)
        {
            this._signalType = bindInfo.SignalType;
            this._identifier = bindInfo.Identifier;
            this._signalBus = signalBus;
            this._action = action;
            signalBus.SubscribeId(bindInfo.SignalType, this._identifier, new Action<object>(this.OnSignalFired));
        }

        // Token: 0x06000145 RID: 325 RVA: 0x000052E0 File Offset: 0x000034E0
        private void OnSignalFired(object signal)
        {
            this._action(signal);
        }

        // Token: 0x06000146 RID: 326 RVA: 0x000052F0 File Offset: 0x000034F0
        public void Dispose()
        {
            this._signalBus.UnsubscribeId(this._signalType, this._identifier, new Action<object>(this.OnSignalFired));
        }

        // Token: 0x06000147 RID: 327 RVA: 0x00005318 File Offset: 0x00003518
        private static object __zenCreate(object[] P_0)
        {
            return new SignalCallbackWrapper((SignalBindingBindInfo)P_0[0], (Action<object>)P_0[1], (SignalBus)P_0[2]);
        }

        // Token: 0x06000148 RID: 328 RVA: 0x00005354 File Offset: 0x00003554
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalCallbackWrapper), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalCallbackWrapper.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "bindInfo", typeof(SignalBindingBindInfo), null, InjectSources.Any),
                new InjectableInfo(false, null, "action", typeof(Action<object>), null, InjectSources.Any),
                new InjectableInfo(false, null, "signalBus", typeof(SignalBus), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x0400006D RID: 109
        private readonly SignalBus _signalBus;

        // Token: 0x0400006E RID: 110
        private readonly Action<object> _action;

        // Token: 0x0400006F RID: 111
        private readonly Type _signalType;

        // Token: 0x04000070 RID: 112
        private readonly object _identifier;
    }
}
