using System;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000024 RID: 36
    public class BindSignalFromBinder<TObject, TSignal>
    {
        // Token: 0x06000107 RID: 263 RVA: 0x00004624 File Offset: 0x00002824
        public BindSignalFromBinder(SignalBindingBindInfo signalBindInfo, BindStatement bindStatement, Func<TObject, Action<TSignal>> methodGetter, DiContainer container)
        {
            this._signalBindInfo = signalBindInfo;
            this._bindStatement = bindStatement;
            this._methodGetter = methodGetter;
            this._container = container;
        }

        // Token: 0x06000108 RID: 264 RVA: 0x0000464C File Offset: 0x0000284C
        public SignalCopyBinder FromResolve()
        {
            return this.From(delegate (ConcreteBinderGeneric<TObject> x)
            {
                x.FromResolve().AsCached();
            });
        }

        // Token: 0x06000109 RID: 265 RVA: 0x00004674 File Offset: 0x00002874
        public SignalCopyBinder FromResolveAll()
        {
            return this.From(delegate (ConcreteBinderGeneric<TObject> x)
            {
                x.FromResolveAll().AsCached();
            });
        }

        // Token: 0x0600010A RID: 266 RVA: 0x0000469C File Offset: 0x0000289C
        public SignalCopyBinder FromNew()
        {
            return this.From(delegate (ConcreteBinderGeneric<TObject> x)
            {
                x.FromNew().AsCached();
            });
        }

        // Token: 0x0600010B RID: 267 RVA: 0x000046C4 File Offset: 0x000028C4
        public SignalCopyBinder From(Action<ConcreteBinderGeneric<TObject>> objectBindCallback)
        {
            ModestTree.Assert.That(!this._bindStatement.HasFinalizer);
            this._bindStatement.SetFinalizer(new NullBindingFinalizer());
            Guid guid = Guid.NewGuid();
            ConcreteBinderGeneric<TObject> concreteBinderGeneric = this._container.BindNoFlush<TObject>().WithId(guid);
            objectBindCallback(concreteBinderGeneric);
            Func<object, Action<object>> param = (object obj) => delegate (object s)
            {
                this._methodGetter((TObject)((object)obj))((TSignal)((object)s));
            };
            SignalCopyBinder signalCopyBinder = new SignalCopyBinder(this._container.Bind<IDisposable>().To<SignalCallbackWithLookupWrapper>().AsCached().WithArguments<SignalBindingBindInfo, Type, Guid, Func<object, Action<object>>>(this._signalBindInfo, typeof(TObject), guid, param).NonLazy().BindInfo);
            signalCopyBinder.AddCopyBindInfo(concreteBinderGeneric.BindInfo);
            return signalCopyBinder;
        }

        // Token: 0x0600010D RID: 269 RVA: 0x0000478C File Offset: 0x0000298C
        private static object __zenCreate(object[] P_0)
        {
            return new BindSignalFromBinder<TObject, TSignal>((SignalBindingBindInfo)P_0[0], (BindStatement)P_0[1], (Func<TObject, Action<TSignal>>)P_0[2], (DiContainer)P_0[3]);
        }

        // Token: 0x0600010E RID: 270 RVA: 0x000047D4 File Offset: 0x000029D4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(BindSignalFromBinder<TObject, TSignal>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(BindSignalFromBinder<TObject, TSignal>.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "signalBindInfo", typeof(SignalBindingBindInfo), null, InjectSources.Any),
                new InjectableInfo(false, null, "bindStatement", typeof(BindStatement), null, InjectSources.Any),
                new InjectableInfo(false, null, "methodGetter", typeof(Func<TObject, Action<TSignal>>), null, InjectSources.Any),
                new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000051 RID: 81
        private readonly BindStatement _bindStatement;

        // Token: 0x04000052 RID: 82
        private readonly Func<TObject, Action<TSignal>> _methodGetter;

        // Token: 0x04000053 RID: 83
        private readonly DiContainer _container;

        // Token: 0x04000054 RID: 84
        private readonly SignalBindingBindInfo _signalBindInfo;
    }
}
