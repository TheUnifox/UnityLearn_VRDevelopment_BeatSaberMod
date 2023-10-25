using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002D6 RID: 726
    public class PoolableManager
    {
        // Token: 0x06000F8C RID: 3980 RVA: 0x0002BED0 File Offset: 0x0002A0D0
        public PoolableManager([InjectLocal] List<IPoolable> poolables, [Inject(Optional = true, Source = InjectSources.Local)] List<ModestTree.Util.ValuePair<Type, int>> priorities)
        {
            this._poolables = poolables.Select(x => CreatePoolableInfo(x, priorities))
                .OrderBy(x => x.Priority).Select(x => x.Poolable).ToList();
        }

        // Token: 0x06000F8D RID: 3981 RVA: 0x0002BF5C File Offset: 0x0002A15C
        private PoolableManager.PoolableInfo CreatePoolableInfo(IPoolable poolable, List<ModestTree.Util.ValuePair<Type, int>> priorities)
        {
            int? num = (from x in priorities
                        where poolable.GetType().DerivesFromOrEqual(x.First)
                        select new int?(x.Second)).SingleOrDefault<int?>();
            int priority = (num != null) ? num.Value : 0;
            return new PoolableManager.PoolableInfo(poolable, priority);
        }

        // Token: 0x06000F8E RID: 3982 RVA: 0x0002BFD4 File Offset: 0x0002A1D4
        public void TriggerOnSpawned()
        {
            ModestTree.Assert.That(!this._isSpawned);
            this._isSpawned = true;
            for (int i = 0; i < this._poolables.Count; i++)
            {
                this._poolables[i].OnSpawned();
            }
        }

        // Token: 0x06000F8F RID: 3983 RVA: 0x0002C020 File Offset: 0x0002A220
        public void TriggerOnDespawned()
        {
            ModestTree.Assert.That(this._isSpawned);
            this._isSpawned = false;
            for (int i = this._poolables.Count - 1; i >= 0; i--)
            {
                this._poolables[i].OnDespawned();
            }
        }

        // Token: 0x06000F90 RID: 3984 RVA: 0x0002C068 File Offset: 0x0002A268
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableManager((List<IPoolable>)P_0[0], (List<ModestTree.Util.ValuePair<Type, int>>)P_0[1]);
        }

        // Token: 0x06000F91 RID: 3985 RVA: 0x0002C098 File Offset: 0x0002A298
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableManager), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableManager.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "poolables", typeof(List<IPoolable>), null, InjectSources.Local),
                new InjectableInfo(true, null, "priorities", typeof(List<ModestTree.Util.ValuePair<Type, int>>), null, InjectSources.Local)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004E6 RID: 1254
        private readonly List<IPoolable> _poolables;

        // Token: 0x040004E7 RID: 1255
        private bool _isSpawned;

        // Token: 0x020002D7 RID: 727
        private struct PoolableInfo
        {
            // Token: 0x06000F92 RID: 3986 RVA: 0x0002C12C File Offset: 0x0002A32C
            public PoolableInfo(IPoolable poolable, int priority)
            {
                this.Poolable = poolable;
                this.Priority = priority;
            }

            // Token: 0x040004E8 RID: 1256
            public IPoolable Poolable;

            // Token: 0x040004E9 RID: 1257
            public int Priority;
        }
    }
}
