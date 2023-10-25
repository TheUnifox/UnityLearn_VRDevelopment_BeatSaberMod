using System;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000028 RID: 40
    public class BindSignalToBinder<TSignal>
    {
        // Token: 0x0600011E RID: 286 RVA: 0x00004AD4 File Offset: 0x00002CD4
        public BindSignalToBinder(DiContainer container, SignalBindingBindInfo signalBindInfo)
        {
            this._container = container;
            this._signalBindInfo = signalBindInfo;
            this._bindStatement = container.StartBinding(true);
        }

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x0600011F RID: 287 RVA: 0x00004AF8 File Offset: 0x00002CF8
        protected SignalBindingBindInfo SignalBindInfo
        {
            get
            {
                return this._signalBindInfo;
            }
        }

        // Token: 0x06000120 RID: 288 RVA: 0x00004B00 File Offset: 0x00002D00
        public SignalCopyBinder ToMethod(Action<TSignal> callback)
        {
            ModestTree.Assert.That(!this._bindStatement.HasFinalizer);
            this._bindStatement.SetFinalizer(new NullBindingFinalizer());
            return new SignalCopyBinder(this._container.Bind<IDisposable>().To<SignalCallbackWrapper>().AsCached().WithArguments<SignalBindingBindInfo, Action<object>>(this._signalBindInfo, delegate (object o)
            {
                callback((TSignal)((object)o));
            }).NonLazy().BindInfo);
        }

        // Token: 0x06000121 RID: 289 RVA: 0x00004B78 File Offset: 0x00002D78
        public SignalCopyBinder ToMethod(Action callback)
        {
            return this.ToMethod(delegate (TSignal signal)
            {
                callback();
            });
        }

        // Token: 0x06000122 RID: 290 RVA: 0x00004BA4 File Offset: 0x00002DA4
        public BindSignalFromBinder<TObject, TSignal> ToMethod<TObject>(Action<TObject, TSignal> handler)
        {
            return this.ToMethod<TObject>((TObject x) => delegate (TSignal s)
            {
                handler(x, s);
            });
        }

        // Token: 0x06000123 RID: 291 RVA: 0x00004BD0 File Offset: 0x00002DD0
        public BindSignalFromBinder<TObject, TSignal> ToMethod<TObject>(Func<TObject, Action> handlerGetter)
        {
            return this.ToMethod<TObject>((TObject x) => delegate (TSignal s)
            {
                handlerGetter(x)();
            });
        }

        // Token: 0x06000124 RID: 292 RVA: 0x00004BFC File Offset: 0x00002DFC
        public BindSignalFromBinder<TObject, TSignal> ToMethod<TObject>(Func<TObject, Action<TSignal>> handlerGetter)
        {
            return new BindSignalFromBinder<TObject, TSignal>(this._signalBindInfo, this._bindStatement, handlerGetter, this._container);
        }

        // Token: 0x06000125 RID: 293 RVA: 0x00004C18 File Offset: 0x00002E18
        private static object __zenCreate(object[] P_0)
        {
            return new BindSignalToBinder<TSignal>((DiContainer)P_0[0], (SignalBindingBindInfo)P_0[1]);
        }

        // Token: 0x06000126 RID: 294 RVA: 0x00004C48 File Offset: 0x00002E48
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(BindSignalToBinder<TSignal>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(BindSignalToBinder<TSignal>.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any),
                new InjectableInfo(false, null, "signalBindInfo", typeof(SignalBindingBindInfo), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x0400005B RID: 91
        private DiContainer _container;

        // Token: 0x0400005C RID: 92
        private BindStatement _bindStatement;

        // Token: 0x0400005D RID: 93
        private SignalBindingBindInfo _signalBindInfo;
    }
}
