using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000039 RID: 57
    public class SignalDeclaration : ITickable, IDisposable
    {
        // Token: 0x06000171 RID: 369 RVA: 0x00005708 File Offset: 0x00003908
        public SignalDeclaration(SignalDeclarationBindInfo bindInfo, [InjectOptional] ZenjectSettings zenjectSettings)
        {
            zenjectSettings = (zenjectSettings ?? ZenjectSettings.Default);
            this._settings = (zenjectSettings.Signals ?? ZenjectSettings.SignalSettings.Default);
            this._bindingId = new BindingId(bindInfo.SignalType, bindInfo.Identifier);
            this._missingHandlerResponses = bindInfo.MissingHandlerResponse;
            this._isAsync = bindInfo.RunAsync;
            this.TickPriority = bindInfo.TickPriority;
        }

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x06000172 RID: 370 RVA: 0x00005790 File Offset: 0x00003990
        // (set) Token: 0x06000173 RID: 371 RVA: 0x00005798 File Offset: 0x00003998
        public int TickPriority { get; private set; }

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x06000174 RID: 372 RVA: 0x000057A4 File Offset: 0x000039A4
        public bool IsAsync
        {
            get
            {
                return this._isAsync;
            }
        }

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x06000175 RID: 373 RVA: 0x000057AC File Offset: 0x000039AC
        public BindingId BindingId
        {
            get
            {
                return this._bindingId;
            }
        }

        // Token: 0x06000176 RID: 374 RVA: 0x000057B4 File Offset: 0x000039B4
        public void Dispose()
        {
            if (this._settings.RequireStrictUnsubscribe)
            {
                ModestTree.Assert.That(this._subscriptions.IsEmpty<SignalSubscription>(), "Found {0} signal handlers still added to declaration {1}", this._subscriptions.Count, this._bindingId);
                return;
            }
            for (int i = 0; i < this._subscriptions.Count; i++)
            {
                this._subscriptions[i].OnDeclarationDespawned();
            }
        }

        // Token: 0x06000177 RID: 375 RVA: 0x00005828 File Offset: 0x00003A28
        public void Fire(object signal)
        {
            ModestTree.Assert.That(signal.GetType().DerivesFromOrEqual(this._bindingId.Type));
            if (this._isAsync)
            {
                this._asyncQueue.Add(signal);
                return;
            }
            using (DisposeBlock disposeBlock = DisposeBlock.Spawn())
            {
                List<SignalSubscription> list = disposeBlock.SpawnList<SignalSubscription>();
                list.AddRange(this._subscriptions);
                this.FireInternal(list, signal);
            }
        }

        // Token: 0x06000178 RID: 376 RVA: 0x000058A8 File Offset: 0x00003AA8
        private void FireInternal(List<SignalSubscription> subscriptions, object signal)
        {
            if (subscriptions.IsEmpty<SignalSubscription>())
            {
                if (this._missingHandlerResponses == SignalMissingHandlerResponses.Warn)
                {
                    ModestTree.Log.Warn("Fired signal '{0}' but no subscriptions found!  If this is intentional then either add OptionalSubscriber() to the binding or change the default in ZenjectSettings", new object[]
                    {
                        signal.GetType()
                    });
                }
                else if (this._missingHandlerResponses == SignalMissingHandlerResponses.Throw)
                {
                    throw ModestTree.Assert.CreateException("Fired signal '{0}' but no subscriptions found!  If this is intentional then either add OptionalSubscriber() to the binding or change the default in ZenjectSettings", new object[]
                    {
                        signal.GetType()
                    });
                }
            }
            for (int i = 0; i < subscriptions.Count; i++)
            {
                SignalSubscription signalSubscription = subscriptions[i];
                if (this._subscriptions.Contains(signalSubscription))
                {
                    signalSubscription.Invoke(signal);
                }
            }
        }

        // Token: 0x06000179 RID: 377 RVA: 0x00005934 File Offset: 0x00003B34
        public void Tick()
        {
            ModestTree.Assert.That(this._isAsync);
            if (!this._asyncQueue.IsEmpty<object>())
            {
                using (DisposeBlock disposeBlock = DisposeBlock.Spawn())
                {
                    List<SignalSubscription> list = disposeBlock.SpawnList<SignalSubscription>();
                    list.AddRange(this._subscriptions);
                    List<object> list2 = disposeBlock.SpawnList<object>();
                    list2.AddRange(this._asyncQueue);
                    this._asyncQueue.Clear();
                    for (int i = 0; i < list2.Count; i++)
                    {
                        this.FireInternal(list, list2[i]);
                    }
                }
            }
        }

        // Token: 0x0600017A RID: 378 RVA: 0x000059CC File Offset: 0x00003BCC
        public void Add(SignalSubscription subscription)
        {
            ModestTree.Assert.That(!this._subscriptions.Contains(subscription));
            this._subscriptions.Add(subscription);
        }

        // Token: 0x0600017B RID: 379 RVA: 0x000059F0 File Offset: 0x00003BF0
        public void Remove(SignalSubscription subscription)
        {
            this._subscriptions.RemoveWithConfirm(subscription);
        }

        // Token: 0x0600017C RID: 380 RVA: 0x00005A00 File Offset: 0x00003C00
        private static object __zenCreate(object[] P_0)
        {
            return new SignalDeclaration((SignalDeclarationBindInfo)P_0[0], (ZenjectSettings)P_0[1]);
        }

        // Token: 0x0600017D RID: 381 RVA: 0x00005A30 File Offset: 0x00003C30
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalDeclaration), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalDeclaration.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "bindInfo", typeof(SignalDeclarationBindInfo), null, InjectSources.Any),
                new InjectableInfo(true, null, "zenjectSettings", typeof(ZenjectSettings), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x0400007A RID: 122
        private readonly List<SignalSubscription> _subscriptions = new List<SignalSubscription>();

        // Token: 0x0400007B RID: 123
        private readonly List<object> _asyncQueue = new List<object>();

        // Token: 0x0400007C RID: 124
        private readonly BindingId _bindingId;

        // Token: 0x0400007D RID: 125
        private readonly SignalMissingHandlerResponses _missingHandlerResponses;

        // Token: 0x0400007E RID: 126
        private readonly bool _isAsync;

        // Token: 0x0400007F RID: 127
        private readonly ZenjectSettings.SignalSettings _settings;

        // Token: 0x0200003A RID: 58
        public class Factory : PlaceholderFactory<SignalDeclarationBindInfo, SignalDeclaration>
        {
            // Token: 0x0600017F RID: 383 RVA: 0x00005ACC File Offset: 0x00003CCC
            private static object __zenCreate(object[] P_0)
            {
                return new SignalDeclaration.Factory();
            }

            // Token: 0x06000180 RID: 384 RVA: 0x00005AE4 File Offset: 0x00003CE4
            [Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(SignalDeclaration.Factory), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalDeclaration.Factory.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }
        }
    }
}
