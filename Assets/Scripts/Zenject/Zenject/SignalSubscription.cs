using System;
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200003B RID: 59
    public class SignalSubscription : IDisposable, IPoolable<Action<object>, SignalDeclaration>
    {
        // Token: 0x06000181 RID: 385 RVA: 0x00005B34 File Offset: 0x00003D34
        public SignalSubscription(SignalSubscription.Pool pool)
        {
            this._pool = pool;
            this.SetDefaults();
        }

        // Token: 0x17000026 RID: 38
        // (get) Token: 0x06000182 RID: 386 RVA: 0x00005B4C File Offset: 0x00003D4C
        public BindingId SignalId
        {
            get
            {
                return this._signalId;
            }
        }

        // Token: 0x06000183 RID: 387 RVA: 0x00005B54 File Offset: 0x00003D54
        public void OnSpawned(Action<object> callback, SignalDeclaration declaration)
        {
            ModestTree.Assert.IsNull(this._callback);
            this._callback = callback;
            this._declaration = declaration;
            this._signalId = declaration.BindingId;
            declaration.Add(this);
        }

        // Token: 0x06000184 RID: 388 RVA: 0x00005B84 File Offset: 0x00003D84
        public void OnDespawned()
        {
            if (this._declaration != null)
            {
                this._declaration.Remove(this);
            }
            this.SetDefaults();
        }

        // Token: 0x06000185 RID: 389 RVA: 0x00005BA0 File Offset: 0x00003DA0
        private void SetDefaults()
        {
            this._callback = null;
            this._declaration = null;
            this._signalId = default(BindingId);
        }

        // Token: 0x06000186 RID: 390 RVA: 0x00005BBC File Offset: 0x00003DBC
        public void Dispose()
        {
            if (!this._pool.InactiveItems.Contains(this))
            {
                this._pool.Despawn(this);
            }
        }

        // Token: 0x06000187 RID: 391 RVA: 0x00005BE0 File Offset: 0x00003DE0
        public void OnDeclarationDespawned()
        {
            this._declaration = null;
        }

        // Token: 0x06000188 RID: 392 RVA: 0x00005BEC File Offset: 0x00003DEC
        public void Invoke(object signal)
        {
            this._callback(signal);
        }

        // Token: 0x06000189 RID: 393 RVA: 0x00005BFC File Offset: 0x00003DFC
        private static object __zenCreate(object[] P_0)
        {
            return new SignalSubscription((SignalSubscription.Pool)P_0[0]);
        }

        // Token: 0x0600018A RID: 394 RVA: 0x00005C20 File Offset: 0x00003E20
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(SignalSubscription), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalSubscription.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "pool", typeof(SignalSubscription.Pool), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000081 RID: 129
        private readonly SignalSubscription.Pool _pool;

        // Token: 0x04000082 RID: 130
        private Action<object> _callback;

        // Token: 0x04000083 RID: 131
        private SignalDeclaration _declaration;

        // Token: 0x04000084 RID: 132
        private BindingId _signalId;

        // Token: 0x0200003C RID: 60
        public class Pool : PoolableMemoryPool<Action<object>, SignalDeclaration, SignalSubscription>
        {
            // Token: 0x0600018C RID: 396 RVA: 0x00005C9C File Offset: 0x00003E9C
            private static object __zenCreate(object[] P_0)
            {
                return new SignalSubscription.Pool();
            }

            // Token: 0x0600018D RID: 397 RVA: 0x00005CB4 File Offset: 0x00003EB4
            [Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(SignalSubscription.Pool), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(SignalSubscription.Pool.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }
        }
    }
}
