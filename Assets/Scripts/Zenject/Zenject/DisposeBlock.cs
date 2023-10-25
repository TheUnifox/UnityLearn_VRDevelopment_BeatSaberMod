using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x020002EC RID: 748
    [NoReflectionBaking]
    public class DisposeBlock : IDisposable
    {
        // Token: 0x06001018 RID: 4120 RVA: 0x0002D88C File Offset: 0x0002BA8C
        private static void OnSpawned(DisposeBlock that)
        {
            ModestTree.Assert.IsNull(that._disposables);
            ModestTree.Assert.IsNull(that._objectPoolPairs);
        }

        // Token: 0x06001019 RID: 4121 RVA: 0x0002D8A4 File Offset: 0x0002BAA4
        private static void OnDespawned(DisposeBlock that)
        {
            if (that._disposables != null)
            {
                for (int i = that._disposables.Count - 1; i >= 0; i--)
                {
                    that._disposables[i].Dispose();
                }
                ListPool<IDisposable>.Instance.Despawn(that._disposables);
                that._disposables = null;
            }
            if (that._objectPoolPairs != null)
            {
                for (int j = that._objectPoolPairs.Count - 1; j >= 0; j--)
                {
                    DisposeBlock.SpawnedObjectPoolPair spawnedObjectPoolPair = that._objectPoolPairs[j];
                    spawnedObjectPoolPair.Pool.Despawn(spawnedObjectPoolPair.Object);
                }
                ListPool<DisposeBlock.SpawnedObjectPoolPair>.Instance.Despawn(that._objectPoolPairs);
                that._objectPoolPairs = null;
            }
        }

        // Token: 0x0600101A RID: 4122 RVA: 0x0002D950 File Offset: 0x0002BB50
        private void LazyInitializeDisposableList()
        {
            if (this._disposables == null)
            {
                this._disposables = ListPool<IDisposable>.Instance.Spawn();
            }
        }

        // Token: 0x0600101B RID: 4123 RVA: 0x0002D96C File Offset: 0x0002BB6C
        public void AddRange<T>(IList<T> disposables) where T : IDisposable
        {
            this.LazyInitializeDisposableList();
            for (int i = 0; i < disposables.Count; i++)
            {
                this._disposables.Add(disposables[i]);
            }
        }

        // Token: 0x0600101C RID: 4124 RVA: 0x0002D9A8 File Offset: 0x0002BBA8
        public void Add(IDisposable disposable)
        {
            this.LazyInitializeDisposableList();
            ModestTree.Assert.That(!this._disposables.Contains(disposable));
            this._disposables.Add(disposable);
        }

        // Token: 0x0600101D RID: 4125 RVA: 0x0002D9D0 File Offset: 0x0002BBD0
        public void Remove(IDisposable disposable)
        {
            ModestTree.Assert.IsNotNull(this._disposables);
            this._disposables.RemoveWithConfirm(disposable);
        }

        // Token: 0x0600101E RID: 4126 RVA: 0x0002D9EC File Offset: 0x0002BBEC
        private void StoreSpawnedObject<T>(T obj, IDespawnableMemoryPool<T> pool)
        {
            if (typeof(T).DerivesFrom<IDisposable>())
            {
                this.Add((IDisposable)((object)obj));
                return;
            }
            DisposeBlock.SpawnedObjectPoolPair item = new DisposeBlock.SpawnedObjectPoolPair
            {
                Pool = pool,
                Object = obj
            };
            if (this._objectPoolPairs == null)
            {
                this._objectPoolPairs = ListPool<DisposeBlock.SpawnedObjectPoolPair>.Instance.Spawn();
            }
            this._objectPoolPairs.Add(item);
        }

        // Token: 0x0600101F RID: 4127 RVA: 0x0002DA60 File Offset: 0x0002BC60
        public T Spawn<T>(IMemoryPool<T> pool)
        {
            T t = pool.Spawn();
            this.StoreSpawnedObject<T>(t, pool);
            return t;
        }

        // Token: 0x06001020 RID: 4128 RVA: 0x0002DA80 File Offset: 0x0002BC80
        public TValue Spawn<TValue, TParam1>(IMemoryPool<TParam1, TValue> pool, TParam1 p1)
        {
            TValue tvalue = pool.Spawn(p1);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001021 RID: 4129 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
        public TValue Spawn<TValue, TParam1, TParam2>(IMemoryPool<TParam1, TParam2, TValue> pool, TParam1 p1, TParam2 p2)
        {
            TValue tvalue = pool.Spawn(p1, p2);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001022 RID: 4130 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
        public TValue Spawn<TValue, TParam1, TParam2, TParam3>(IMemoryPool<TParam1, TParam2, TParam3, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            TValue tvalue = pool.Spawn(p1, p2, p3);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001023 RID: 4131 RVA: 0x0002DAE4 File Offset: 0x0002BCE4
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            TValue tvalue = pool.Spawn(p1, p2, p3, p4);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001024 RID: 4132 RVA: 0x0002DB08 File Offset: 0x0002BD08
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4, TParam5>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            TValue tvalue = pool.Spawn(p1, p2, p3, p4, p5);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001025 RID: 4133 RVA: 0x0002DB30 File Offset: 0x0002BD30
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)
        {
            TValue tvalue = pool.Spawn(p1, p2, p3, p4, p5, p6);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001026 RID: 4134 RVA: 0x0002DB58 File Offset: 0x0002BD58
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
            TValue tvalue = pool.Spawn(p1, p2, p3, p4, p5, p6, p7);
            this.StoreSpawnedObject<TValue>(tvalue, pool);
            return tvalue;
        }

        // Token: 0x06001027 RID: 4135 RVA: 0x0002DB84 File Offset: 0x0002BD84
        public List<T> SpawnList<T>(IEnumerable<T> elements)
        {
            List<T> list = this.SpawnList<T>();
            list.AddRange(elements);
            return list;
        }

        // Token: 0x06001028 RID: 4136 RVA: 0x0002DB94 File Offset: 0x0002BD94
        public List<T> SpawnList<T>()
        {
            return this.Spawn<List<T>>(ListPool<T>.Instance);
        }

        // Token: 0x06001029 RID: 4137 RVA: 0x0002DBA4 File Offset: 0x0002BDA4
        public static DisposeBlock Spawn()
        {
            return DisposeBlock._pool.Spawn();
        }

        // Token: 0x0600102A RID: 4138 RVA: 0x0002DBB0 File Offset: 0x0002BDB0
        public void Dispose()
        {
            DisposeBlock._pool.Despawn(this);
        }

        // Token: 0x04000513 RID: 1299
        private static readonly StaticMemoryPool<DisposeBlock> _pool = new StaticMemoryPool<DisposeBlock>(new Action<DisposeBlock>(DisposeBlock.OnSpawned), new Action<DisposeBlock>(DisposeBlock.OnDespawned), 0);

        // Token: 0x04000514 RID: 1300
        private List<IDisposable> _disposables;

        // Token: 0x04000515 RID: 1301
        private List<DisposeBlock.SpawnedObjectPoolPair> _objectPoolPairs;

        // Token: 0x020002ED RID: 749
        private struct SpawnedObjectPoolPair
        {
            // Token: 0x04000516 RID: 1302
            public IMemoryPool Pool;

            // Token: 0x04000517 RID: 1303
            public object Object;
        }
    }
}
