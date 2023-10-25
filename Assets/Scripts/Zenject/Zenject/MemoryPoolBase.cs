using System;
using System.Buffers;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001C0 RID: 448
    [ZenjectAllowDuringValidation]
    public class MemoryPoolBase<TContract> : IValidatable, IMemoryPool, IDisposable
    {
        // Token: 0x06000934 RID: 2356 RVA: 0x000189F4 File Offset: 0x00016BF4
        [Inject]
        private void Construct(IFactory<TContract> factory, DiContainer container, [InjectOptional] MemoryPoolSettings settings)
        {
            this._settings = (settings ?? MemoryPoolSettings.Default);
            this._factory = factory;
            this._container = container;
            this._inactiveItems = new Stack<TContract>(this._settings.InitialSize);
            if (!container.IsValidating)
            {
                for (int i = 0; i < this._settings.InitialSize; i++)
                {
                    this._inactiveItems.Push(this.AllocNew());
                }
            }
        }

        // Token: 0x1700007C RID: 124
        // (get) Token: 0x06000935 RID: 2357 RVA: 0x00018A64 File Offset: 0x00016C64
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x1700007D RID: 125
        // (get) Token: 0x06000936 RID: 2358 RVA: 0x00018A6C File Offset: 0x00016C6C
        public IEnumerable<TContract> InactiveItems
        {
            get
            {
                return this._inactiveItems;
            }
        }

        // Token: 0x1700007E RID: 126
        // (get) Token: 0x06000937 RID: 2359 RVA: 0x00018A74 File Offset: 0x00016C74
        public int NumTotal
        {
            get
            {
                return this.NumInactive + this.NumActive;
            }
        }

        // Token: 0x1700007F RID: 127
        // (get) Token: 0x06000938 RID: 2360 RVA: 0x00018A84 File Offset: 0x00016C84
        public int NumInactive
        {
            get
            {
                return this._inactiveItems.Count;
            }
        }

        // Token: 0x17000080 RID: 128
        // (get) Token: 0x06000939 RID: 2361 RVA: 0x00018A94 File Offset: 0x00016C94
        public int NumActive
        {
            get
            {
                return this._activeCount;
            }
        }

        // Token: 0x17000081 RID: 129
        // (get) Token: 0x0600093A RID: 2362 RVA: 0x00018A9C File Offset: 0x00016C9C
        public Type ItemType
        {
            get
            {
                return typeof(TContract);
            }
        }

        // Token: 0x0600093B RID: 2363 RVA: 0x00018AA8 File Offset: 0x00016CA8
        public void Dispose()
        {
        }

        // Token: 0x0600093C RID: 2364 RVA: 0x00018AAC File Offset: 0x00016CAC
        void IMemoryPool.Despawn(object item)
        {
            this.Despawn((TContract)((object)item));
        }

        // Token: 0x0600093D RID: 2365 RVA: 0x00018ABC File Offset: 0x00016CBC
        public void Despawn(TContract item)
        {
            this._activeCount--;
            this._inactiveItems.Push(item);
            this.OnDespawned(item);
            if (this._inactiveItems.Count > this._settings.MaxSize)
            {
                this.Resize(this._settings.MaxSize);
            }
        }

        // Token: 0x0600093E RID: 2366 RVA: 0x00018B14 File Offset: 0x00016D14
        private TContract AllocNew()
        {
            TContract result;
            try
            {
                TContract tcontract = this._factory.Create();
                if (!this._container.IsValidating)
                {
                    ModestTree.Assert.IsNotNull(tcontract, "Factory '{0}' returned null value when creating via {1}!", this._factory.GetType(), base.GetType());
                    this.OnCreated(tcontract);
                }
                result = tcontract;
            }
            catch (Exception innerException)
            {
                throw new ZenjectException("Error during construction of type '{0}' via {1}.Create method!".Fmt(new object[]
                {
                    typeof(TContract),
                    base.GetType()
                }), innerException);
            }
            return result;
        }

        // Token: 0x0600093F RID: 2367 RVA: 0x00018BA8 File Offset: 0x00016DA8
        void IValidatable.Validate()
        {
            try
            {
                this._factory.Create();
            }
            catch (Exception innerException)
            {
                throw new ZenjectException("Validation for factory '{0}' failed".Fmt(new object[]
                {
                    base.GetType()
                }), innerException);
            }
        }

        // Token: 0x06000940 RID: 2368 RVA: 0x00018BF4 File Offset: 0x00016DF4
        public void Clear()
        {
            this.Resize(0);
        }

        // Token: 0x06000941 RID: 2369 RVA: 0x00018C00 File Offset: 0x00016E00
        public void ShrinkBy(int numToRemove)
        {
            this.Resize(this._inactiveItems.Count - numToRemove);
        }

        // Token: 0x06000942 RID: 2370 RVA: 0x00018C18 File Offset: 0x00016E18
        public void ExpandBy(int numToAdd)
        {
            this.Resize(this._inactiveItems.Count + numToAdd);
        }

        // Token: 0x06000943 RID: 2371 RVA: 0x00018C30 File Offset: 0x00016E30
        protected TContract GetInternal()
        {
            if (this._inactiveItems.Count == 0)
            {
                this.ExpandPool();
                ModestTree.Assert.That(!this._inactiveItems.IsEmpty<TContract>());
            }
            TContract tcontract = this._inactiveItems.Pop();
            this._activeCount++;
            this.OnSpawned(tcontract);
            return tcontract;
        }

        // Token: 0x06000944 RID: 2372 RVA: 0x00018C88 File Offset: 0x00016E88
        public void Resize(int desiredPoolSize)
        {
            if (this._inactiveItems.Count == desiredPoolSize)
            {
                return;
            }
            if (this._settings.ExpandMethod == PoolExpandMethods.Disabled)
            {
                throw new PoolExceededFixedSizeException("Pool factory '{0}' attempted resize but pool set to fixed size of '{1}'!".Fmt(new object[]
                {
                    base.GetType(),
                    this._inactiveItems.Count
                }));
            }
            ModestTree.Assert.That(desiredPoolSize >= 0, "Attempted to resize the pool to a negative amount");
            while (this._inactiveItems.Count > desiredPoolSize)
            {
                this.OnDestroyed(this._inactiveItems.Pop());
            }
            while (desiredPoolSize > this._inactiveItems.Count)
            {
                this._inactiveItems.Push(this.AllocNew());
            }
            ModestTree.Assert.IsEqual(this._inactiveItems.Count, desiredPoolSize);
        }

        // Token: 0x06000945 RID: 2373 RVA: 0x00018D54 File Offset: 0x00016F54
        private void ExpandPool()
        {
            switch (this._settings.ExpandMethod)
            {
                case PoolExpandMethods.OneAtATime:
                    this.ExpandBy(1);
                    return;
                case PoolExpandMethods.Double:
                    if (this.NumTotal == 0)
                    {
                        this.ExpandBy(1);
                        return;
                    }
                    this.ExpandBy(this.NumTotal);
                    return;
                case PoolExpandMethods.Disabled:
                    throw new PoolExceededFixedSizeException("Pool factory '{0}' exceeded its fixed size of '{1}'!".Fmt(new object[]
                    {
                    base.GetType(),
                    this._inactiveItems.Count
                    }));
                default:
                    throw ModestTree.Assert.CreateException();
            }
        }

        // Token: 0x06000946 RID: 2374 RVA: 0x00018DE0 File Offset: 0x00016FE0
        protected virtual void OnDespawned(TContract item)
        {
        }

        // Token: 0x06000947 RID: 2375 RVA: 0x00018DE4 File Offset: 0x00016FE4
        protected virtual void OnSpawned(TContract item)
        {
        }

        // Token: 0x06000948 RID: 2376 RVA: 0x00018DE8 File Offset: 0x00016FE8
        protected virtual void OnCreated(TContract item)
        {
        }

        // Token: 0x06000949 RID: 2377 RVA: 0x00018DEC File Offset: 0x00016FEC
        protected virtual void OnDestroyed(TContract item)
        {
        }

        // Token: 0x0600094B RID: 2379 RVA: 0x00018DF8 File Offset: 0x00016FF8
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPoolBase<TContract>();
        }

        // Token: 0x0600094C RID: 2380 RVA: 0x00018E10 File Offset: 0x00017010
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((MemoryPoolBase<TContract>)P_0).Construct((IFactory<TContract>)P_1[0], (DiContainer)P_1[1], (MemoryPoolSettings)P_1[2]);
        }

        // Token: 0x0600094D RID: 2381 RVA: 0x00018E44 File Offset: 0x00017044
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPoolBase<TContract>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPoolBase<TContract>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(MemoryPoolBase<TContract>.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "factory", typeof(IFactory<TContract>), null, InjectSources.Any),
                    new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any),
                    new InjectableInfo(true, null, "settings", typeof(MemoryPoolSettings), null, InjectSources.Any)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002E3 RID: 739
        private Stack<TContract> _inactiveItems;

        // Token: 0x040002E4 RID: 740
        private IFactory<TContract> _factory;

        // Token: 0x040002E5 RID: 741
        private MemoryPoolSettings _settings;

        // Token: 0x040002E6 RID: 742
        private DiContainer _container;

        // Token: 0x040002E7 RID: 743
        private int _activeCount;
    }
}
