using System;
using System.Collections;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200002F RID: 47
    public class SignalCallbackWithLookupWrapper : IDisposable
    {
        // Token: 0x0600013F RID: 319 RVA: 0x0000501C File Offset: 0x0000321C
        public SignalCallbackWithLookupWrapper(SignalBindingBindInfo signalBindInfo, Type objectType, Guid lookupId, Func<object, Action<object>> methodGetter, SignalBus signalBus, DiContainer container)
        {
            this._objectType = objectType;
            this._signalType = signalBindInfo.SignalType;
            this._identifier = signalBindInfo.Identifier;
            this._container = container;
            this._methodGetter = methodGetter;
            this._signalBus = signalBus;
            this._lookupId = lookupId;
            signalBus.SubscribeId(signalBindInfo.SignalType, this._identifier, new Action<object>(this.OnSignalFired));
        }

        // Token: 0x06000140 RID: 320 RVA: 0x0000508C File Offset: 0x0000328C
        private void OnSignalFired(object signal)
        {
            IList list = this._container.ResolveIdAll(this._objectType, this._lookupId);
            for (int i = 0; i < list.Count; i++)
            {
                this._methodGetter(list[i])(signal);
            }
        }

        // Token: 0x06000141 RID: 321 RVA: 0x000050E0 File Offset: 0x000032E0
        public void Dispose()
        {
            this._signalBus.UnsubscribeId(this._signalType, this._identifier, new Action<object>(this.OnSignalFired));
        }

        // Token: 0x06000142 RID: 322 RVA: 0x00005108 File Offset: 0x00003308
        private static object __zenCreate(object[] P_0)
        {
            return new SignalCallbackWithLookupWrapper((SignalBindingBindInfo)P_0[0], (Type)P_0[1], (Guid)P_0[2], (Func<object, Action<object>>)P_0[3], (SignalBus)P_0[4], (DiContainer)P_0[5]);
        }

        // Token: 0x06000143 RID: 323 RVA: 0x00005168 File Offset: 0x00003368
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalCallbackWithLookupWrapper), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalCallbackWithLookupWrapper.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "signalBindInfo", typeof(SignalBindingBindInfo), null, InjectSources.Any),
                new InjectableInfo(false, null, "objectType", typeof(Type), null, InjectSources.Any),
                new InjectableInfo(false, null, "lookupId", typeof(Guid), null, InjectSources.Any),
                new InjectableInfo(false, null, "methodGetter", typeof(Func<object, Action<object>>), null, InjectSources.Any),
                new InjectableInfo(false, null, "signalBus", typeof(SignalBus), null, InjectSources.Any),
                new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000066 RID: 102
        private readonly DiContainer _container;

        // Token: 0x04000067 RID: 103
        private readonly SignalBus _signalBus;

        // Token: 0x04000068 RID: 104
        private readonly Guid _lookupId;

        // Token: 0x04000069 RID: 105
        private readonly Func<object, Action<object>> _methodGetter;

        // Token: 0x0400006A RID: 106
        private readonly Type _objectType;

        // Token: 0x0400006B RID: 107
        private readonly Type _signalType;

        // Token: 0x0400006C RID: 108
        private readonly object _identifier;
    }
}
