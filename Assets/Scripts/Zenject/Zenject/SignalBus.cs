using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200003E RID: 62
    public class SignalBus : ILateDisposable
    {
        // Token: 0x06000196 RID: 406 RVA: 0x00005DB4 File Offset: 0x00003FB4
        public SignalBus([Inject(Source = InjectSources.Local)] List<SignalDeclaration> signalDeclarations, [Inject(Source = InjectSources.Parent, Optional = true)] SignalBus parentBus, [InjectOptional] ZenjectSettings zenjectSettings, SignalSubscription.Pool subscriptionPool, SignalDeclaration.Factory signalDeclarationFactory, DiContainer container)
        {
            this._subscriptionPool = subscriptionPool;
            zenjectSettings = (zenjectSettings ?? ZenjectSettings.Default);
            this._settings = (zenjectSettings.Signals ?? ZenjectSettings.SignalSettings.Default);
            this._signalDeclarationFactory = signalDeclarationFactory;
            this._container = container;
            this._localDeclarationMap = signalDeclarations.ToDictionary((SignalDeclaration x) => x.BindingId, (SignalDeclaration x) => x);
            this._parentBus = parentBus;
        }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x06000197 RID: 407 RVA: 0x00005E5C File Offset: 0x0000405C
        public SignalBus ParentBus
        {
            get
            {
                return this._parentBus;
            }
        }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x06000198 RID: 408 RVA: 0x00005E64 File Offset: 0x00004064
        public int NumSubscribers
        {
            get
            {
                return this._subscriptionMap.Count;
            }
        }

        // Token: 0x06000199 RID: 409 RVA: 0x00005E74 File Offset: 0x00004074
        public void LateDispose()
        {
            if (this._settings.RequireStrictUnsubscribe)
            {
                if (!this._subscriptionMap.IsEmpty<KeyValuePair<SignalSubscriptionId, SignalSubscription>>())
                {
                    string message = "Found subscriptions for signals '{0}' in SignalBus.LateDispose!  Either add the explicit Unsubscribe or set SignalSettings.AutoUnsubscribeInDispose to true";
                    object[] array = new object[1];
                    array[0] = (from x in this._subscriptionMap.Values
                                select x.SignalId.ToString()).Join(", ");
                    throw ModestTree.Assert.CreateException(message, array);
                }
            }
            else
            {
                foreach (SignalSubscription signalSubscription in this._subscriptionMap.Values)
                {
                    signalSubscription.Dispose();
                }
            }
            foreach (SignalDeclaration signalDeclaration in this._localDeclarationMap.Values)
            {
                signalDeclaration.Dispose();
            }
        }

        // Token: 0x0600019A RID: 410 RVA: 0x00005F78 File Offset: 0x00004178
        public void FireId<TSignal>(object identifier, TSignal signal)
        {
            this.GetDeclaration(typeof(TSignal), identifier, true).Fire(signal);
        }

        // Token: 0x0600019B RID: 411 RVA: 0x00005F98 File Offset: 0x00004198
        public void Fire<TSignal>(TSignal signal)
        {
            this.FireId<TSignal>(null, signal);
        }

        // Token: 0x0600019C RID: 412 RVA: 0x00005FA4 File Offset: 0x000041A4
        public void FireId<TSignal>(object identifier)
        {
            this.GetDeclaration(typeof(TSignal), identifier, true).Fire((TSignal)((object)Activator.CreateInstance(typeof(TSignal))));
        }

        // Token: 0x0600019D RID: 413 RVA: 0x00005FD8 File Offset: 0x000041D8
        public void Fire<TSignal>()
        {
            this.FireId<TSignal>(null);
        }

        // Token: 0x0600019E RID: 414 RVA: 0x00005FE4 File Offset: 0x000041E4
        public void FireId(object identifier, object signal)
        {
            this.GetDeclaration(signal.GetType(), identifier, true).Fire(signal);
        }

        // Token: 0x0600019F RID: 415 RVA: 0x00005FFC File Offset: 0x000041FC
        public void Fire(object signal)
        {
            this.FireId(null, signal);
        }

        // Token: 0x060001A0 RID: 416 RVA: 0x00006008 File Offset: 0x00004208
        public void TryFireId<TSignal>(object identifier, TSignal signal)
        {
            SignalDeclaration declaration = this.GetDeclaration(typeof(TSignal), identifier, false);
            if (declaration != null)
            {
                declaration.Fire(signal);
            }
        }

        // Token: 0x060001A1 RID: 417 RVA: 0x00006038 File Offset: 0x00004238
        public void TryFire<TSignal>(TSignal signal)
        {
            this.TryFireId<TSignal>(null, signal);
        }

        // Token: 0x060001A2 RID: 418 RVA: 0x00006044 File Offset: 0x00004244
        public void TryFireId<TSignal>(object identifier)
        {
            SignalDeclaration declaration = this.GetDeclaration(typeof(TSignal), identifier, false);
            if (declaration != null)
            {
                declaration.Fire((TSignal)((object)Activator.CreateInstance(typeof(TSignal))));
            }
        }

        // Token: 0x060001A3 RID: 419 RVA: 0x00006088 File Offset: 0x00004288
        public void TryFire<TSignal>()
        {
            this.TryFireId<TSignal>(null);
        }

        // Token: 0x060001A4 RID: 420 RVA: 0x00006094 File Offset: 0x00004294
        public void TryFireId(object identifier, object signal)
        {
            SignalDeclaration declaration = this.GetDeclaration(signal.GetType(), identifier, false);
            if (declaration != null)
            {
                declaration.Fire(signal);
            }
        }

        // Token: 0x060001A5 RID: 421 RVA: 0x000060BC File Offset: 0x000042BC
        public void TryFire(object signal)
        {
            this.TryFireId(null, signal);
        }

        // Token: 0x060001A6 RID: 422 RVA: 0x000060C8 File Offset: 0x000042C8
        public void SubscribeId<TSignal>(object identifier, Action callback)
        {
            Action<object> callback2 = delegate (object args)
            {
                callback();
            };
            this.SubscribeInternal(typeof(TSignal), identifier, callback, callback2);
        }

        // Token: 0x060001A7 RID: 423 RVA: 0x00006108 File Offset: 0x00004308
        public void Subscribe<TSignal>(Action callback)
        {
            this.SubscribeId<TSignal>(null, callback);
        }

        // Token: 0x060001A8 RID: 424 RVA: 0x00006114 File Offset: 0x00004314
        public void SubscribeId<TSignal>(object identifier, Action<TSignal> callback)
        {
            Action<object> callback2 = delegate (object args)
            {
                callback((TSignal)((object)args));
            };
            this.SubscribeInternal(typeof(TSignal), identifier, callback, callback2);
        }

        // Token: 0x060001A9 RID: 425 RVA: 0x00006154 File Offset: 0x00004354
        public void Subscribe<TSignal>(Action<TSignal> callback)
        {
            this.SubscribeId<TSignal>(null, callback);
        }

        // Token: 0x060001AA RID: 426 RVA: 0x00006160 File Offset: 0x00004360
        public void SubscribeId(Type signalType, object identifier, Action<object> callback)
        {
            this.SubscribeInternal(signalType, identifier, callback, callback);
        }

        // Token: 0x060001AB RID: 427 RVA: 0x0000616C File Offset: 0x0000436C
        public void Subscribe(Type signalType, Action<object> callback)
        {
            this.SubscribeId(signalType, null, callback);
        }

        // Token: 0x060001AC RID: 428 RVA: 0x00006178 File Offset: 0x00004378
        public void UnsubscribeId<TSignal>(object identifier, Action callback)
        {
            this.UnsubscribeId(typeof(TSignal), identifier, callback);
        }

        // Token: 0x060001AD RID: 429 RVA: 0x0000618C File Offset: 0x0000438C
        public void Unsubscribe<TSignal>(Action callback)
        {
            this.UnsubscribeId<TSignal>(null, callback);
        }

        // Token: 0x060001AE RID: 430 RVA: 0x00006198 File Offset: 0x00004398
        public void UnsubscribeId(Type signalType, object identifier, Action callback)
        {
            this.UnsubscribeInternal(signalType, identifier, callback, true);
        }

        // Token: 0x060001AF RID: 431 RVA: 0x000061A4 File Offset: 0x000043A4
        public void Unsubscribe(Type signalType, Action callback)
        {
            this.UnsubscribeId(signalType, null, callback);
        }

        // Token: 0x060001B0 RID: 432 RVA: 0x000061B0 File Offset: 0x000043B0
        public void UnsubscribeId(Type signalType, object identifier, Action<object> callback)
        {
            this.UnsubscribeInternal(signalType, identifier, callback, true);
        }

        // Token: 0x060001B1 RID: 433 RVA: 0x000061BC File Offset: 0x000043BC
        public void Unsubscribe(Type signalType, Action<object> callback)
        {
            this.UnsubscribeId(signalType, null, callback);
        }

        // Token: 0x060001B2 RID: 434 RVA: 0x000061C8 File Offset: 0x000043C8
        public void UnsubscribeId<TSignal>(object identifier, Action<TSignal> callback)
        {
            this.UnsubscribeInternal(typeof(TSignal), identifier, callback, true);
        }

        // Token: 0x060001B3 RID: 435 RVA: 0x000061E0 File Offset: 0x000043E0
        public void Unsubscribe<TSignal>(Action<TSignal> callback)
        {
            this.UnsubscribeId<TSignal>(null, callback);
        }

        // Token: 0x060001B4 RID: 436 RVA: 0x000061EC File Offset: 0x000043EC
        public void TryUnsubscribeId<TSignal>(object identifier, Action callback)
        {
            this.UnsubscribeInternal(typeof(TSignal), identifier, callback, false);
        }

        // Token: 0x060001B5 RID: 437 RVA: 0x00006204 File Offset: 0x00004404
        public void TryUnsubscribe<TSignal>(Action callback)
        {
            this.TryUnsubscribeId<TSignal>(null, callback);
        }

        // Token: 0x060001B6 RID: 438 RVA: 0x00006210 File Offset: 0x00004410
        public void TryUnsubscribeId(Type signalType, object identifier, Action callback)
        {
            this.UnsubscribeInternal(signalType, identifier, callback, false);
        }

        // Token: 0x060001B7 RID: 439 RVA: 0x0000621C File Offset: 0x0000441C
        public void TryUnsubscribe(Type signalType, Action callback)
        {
            this.TryUnsubscribeId(signalType, null, callback);
        }

        // Token: 0x060001B8 RID: 440 RVA: 0x00006228 File Offset: 0x00004428
        public void TryUnsubscribeId(Type signalType, object identifier, Action<object> callback)
        {
            this.UnsubscribeInternal(signalType, identifier, callback, false);
        }

        // Token: 0x060001B9 RID: 441 RVA: 0x00006234 File Offset: 0x00004434
        public void TryUnsubscribe(Type signalType, Action<object> callback)
        {
            this.TryUnsubscribeId(signalType, null, callback);
        }

        // Token: 0x060001BA RID: 442 RVA: 0x00006240 File Offset: 0x00004440
        public void TryUnsubscribeId<TSignal>(object identifier, Action<TSignal> callback)
        {
            this.UnsubscribeInternal(typeof(TSignal), identifier, callback, false);
        }

        // Token: 0x060001BB RID: 443 RVA: 0x00006258 File Offset: 0x00004458
        public void TryUnsubscribe<TSignal>(Action<TSignal> callback)
        {
            this.TryUnsubscribeId<TSignal>(null, callback);
        }

        // Token: 0x060001BC RID: 444 RVA: 0x00006264 File Offset: 0x00004464
        private void UnsubscribeInternal(Type signalType, object identifier, object token, bool throwIfMissing)
        {
            this.UnsubscribeInternal(new BindingId(signalType, identifier), token, throwIfMissing);
        }

        // Token: 0x060001BD RID: 445 RVA: 0x00006278 File Offset: 0x00004478
        private void UnsubscribeInternal(BindingId signalId, object token, bool throwIfMissing)
        {
            this.UnsubscribeInternal(new SignalSubscriptionId(signalId, token), throwIfMissing);
        }

        // Token: 0x060001BE RID: 446 RVA: 0x00006288 File Offset: 0x00004488
        private void UnsubscribeInternal(SignalSubscriptionId id, bool throwIfMissing)
        {
            SignalSubscription signalSubscription;
            if (this._subscriptionMap.TryGetValue(id, out signalSubscription))
            {
                this._subscriptionMap.RemoveWithConfirm(id);
                signalSubscription.Dispose();
                return;
            }
            if (throwIfMissing)
            {
                throw ModestTree.Assert.CreateException("Called unsubscribe for signal '{0}' but could not find corresponding subscribe.  If this is intentional, call TryUnsubscribe instead.");
            }
        }

        // Token: 0x060001BF RID: 447 RVA: 0x000062C8 File Offset: 0x000044C8
        private void SubscribeInternal(Type signalType, object identifier, object token, Action<object> callback)
        {
            this.SubscribeInternal(new BindingId(signalType, identifier), token, callback);
        }

        // Token: 0x060001C0 RID: 448 RVA: 0x000062DC File Offset: 0x000044DC
        private void SubscribeInternal(BindingId signalId, object token, Action<object> callback)
        {
            this.SubscribeInternal(new SignalSubscriptionId(signalId, token), callback);
        }

        // Token: 0x060001C1 RID: 449 RVA: 0x000062EC File Offset: 0x000044EC
        private void SubscribeInternal(SignalSubscriptionId id, Action<object> callback)
        {
            ModestTree.Assert.That(!this._subscriptionMap.ContainsKey(id), "Tried subscribing to the same signal with the same callback on Zenject.SignalBus");
            SignalDeclaration declaration = this.GetDeclaration(id.SignalId, true);
            SignalSubscription value = this._subscriptionPool.Spawn(callback, declaration);
            this._subscriptionMap.Add(id, value);
        }

        // Token: 0x060001C2 RID: 450 RVA: 0x0000633C File Offset: 0x0000453C
        public void DeclareSignal<T>(object identifier = null, SignalMissingHandlerResponses? missingHandlerResponse = null, bool? forceAsync = null, int? asyncTickPriority = null)
        {
            this.DeclareSignal(typeof(T), identifier, missingHandlerResponse, forceAsync, asyncTickPriority);
        }

        // Token: 0x060001C3 RID: 451 RVA: 0x00006354 File Offset: 0x00004554
        public void DeclareSignal(Type signalType, object identifier = null, SignalMissingHandlerResponses? missingHandlerResponse = null, bool? forceAsync = null, int? asyncTickPriority = null)
        {
            SignalDeclarationBindInfo signalDeclarationBindInfo = SignalExtensions.CreateDefaultSignalDeclarationBindInfo(this._container, signalType);
            signalDeclarationBindInfo.Identifier = identifier;
            if (missingHandlerResponse != null)
            {
                signalDeclarationBindInfo.Identifier = missingHandlerResponse.Value;
            }
            if (forceAsync != null)
            {
                signalDeclarationBindInfo.RunAsync = forceAsync.Value;
            }
            if (asyncTickPriority != null)
            {
                signalDeclarationBindInfo.TickPriority = asyncTickPriority.Value;
            }
            SignalDeclaration signalDeclaration = this._signalDeclarationFactory.Create(signalDeclarationBindInfo);
            this._localDeclarationMap.Add(signalDeclaration.BindingId, signalDeclaration);
        }

        // Token: 0x060001C4 RID: 452 RVA: 0x000063DC File Offset: 0x000045DC
        private SignalDeclaration GetDeclaration(Type signalType, object identifier, bool requireDeclaration)
        {
            return this.GetDeclaration(new BindingId(signalType, identifier), requireDeclaration);
        }

        // Token: 0x060001C5 RID: 453 RVA: 0x000063EC File Offset: 0x000045EC
        private SignalDeclaration GetDeclaration(BindingId signalId, bool requireDeclaration)
        {
            SignalDeclaration result;
            if (this._localDeclarationMap.TryGetValue(signalId, out result))
            {
                return result;
            }
            if (this._parentBus != null)
            {
                return this._parentBus.GetDeclaration(signalId, requireDeclaration);
            }
            if (requireDeclaration)
            {
                throw ModestTree.Assert.CreateException("Fired undeclared signal '{0}'!", new object[]
                {
                    signalId
                });
            }
            return null;
        }

        // Token: 0x060001C6 RID: 454 RVA: 0x00006440 File Offset: 0x00004640
        private static object __zenCreate(object[] P_0)
        {
            return new SignalBus((List<SignalDeclaration>)P_0[0], (SignalBus)P_0[1], (ZenjectSettings)P_0[2], (SignalSubscription.Pool)P_0[3], (SignalDeclaration.Factory)P_0[4], (DiContainer)P_0[5]);
        }

        // Token: 0x060001C7 RID: 455 RVA: 0x000064A0 File Offset: 0x000046A0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalBus), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalBus.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "signalDeclarations", typeof(List<SignalDeclaration>), null, InjectSources.Local),
                new InjectableInfo(true, null, "parentBus", typeof(SignalBus), null, InjectSources.Parent),
                new InjectableInfo(true, null, "zenjectSettings", typeof(ZenjectSettings), null, InjectSources.Any),
                new InjectableInfo(false, null, "subscriptionPool", typeof(SignalSubscription.Pool), null, InjectSources.Any),
                new InjectableInfo(false, null, "signalDeclarationFactory", typeof(SignalDeclaration.Factory), null, InjectSources.Any),
                new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000087 RID: 135
        private readonly SignalSubscription.Pool _subscriptionPool;

        // Token: 0x04000088 RID: 136
        private readonly Dictionary<BindingId, SignalDeclaration> _localDeclarationMap;

        // Token: 0x04000089 RID: 137
        private readonly SignalBus _parentBus;

        // Token: 0x0400008A RID: 138
        private readonly Dictionary<SignalSubscriptionId, SignalSubscription> _subscriptionMap = new Dictionary<SignalSubscriptionId, SignalSubscription>();

        // Token: 0x0400008B RID: 139
        private readonly ZenjectSettings.SignalSettings _settings;

        // Token: 0x0400008C RID: 140
        private readonly SignalDeclaration.Factory _signalDeclarationFactory;

        // Token: 0x0400008D RID: 141
        private readonly DiContainer _container;
    }
}
