using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001F1 RID: 497
    public class PoolCleanupChecker : ILateDisposable
    {
        // Token: 0x06000A43 RID: 2627 RVA: 0x0001AF94 File Offset: 0x00019194
        public PoolCleanupChecker([Inject(Optional = true, Source = InjectSources.Local)] List<IMemoryPool> poolFactories, [Inject(Source = InjectSources.Local)] List<Type> ignoredPools)
        {
            this._poolFactories = poolFactories;
            this._ignoredPools = ignoredPools;
            ModestTree.Assert.That(ignoredPools.All((Type x) => x.DerivesFrom<IMemoryPool>()));
        }

        // Token: 0x06000A44 RID: 2628 RVA: 0x0001AFD4 File Offset: 0x000191D4
        public void LateDispose()
        {
            foreach (IMemoryPool memoryPool in this._poolFactories)
            {
                if (!this._ignoredPools.Contains(memoryPool.GetType()))
                {
                    ModestTree.Assert.IsEqual(memoryPool.NumActive, 0, "Found active objects in pool '{0}' during dispose.  Did you forget to despawn an object of type '{1}'?".Fmt(new object[]
                    {
                        memoryPool.GetType(),
                        memoryPool.ItemType
                    }));
                }
            }
        }

        // Token: 0x06000A45 RID: 2629 RVA: 0x0001B06C File Offset: 0x0001926C
        private static object __zenCreate(object[] P_0)
        {
            return new PoolCleanupChecker((List<IMemoryPool>)P_0[0], (List<Type>)P_0[1]);
        }

        // Token: 0x06000A46 RID: 2630 RVA: 0x0001B09C File Offset: 0x0001929C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolCleanupChecker), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolCleanupChecker.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(true, null, "poolFactories", typeof(List<IMemoryPool>), null, InjectSources.Local),
                new InjectableInfo(false, null, "ignoredPools", typeof(List<Type>), null, InjectSources.Local)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000309 RID: 777
        private readonly List<IMemoryPool> _poolFactories;

        // Token: 0x0400030A RID: 778
        private readonly List<Type> _ignoredPools;
    }
}
