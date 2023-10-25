using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject.Internal
{
    // Token: 0x020002FF RID: 767
    [NoReflectionBaking]
    public class SingletonMarkRegistry
    {
        // Token: 0x0600108B RID: 4235 RVA: 0x0002E7A8 File Offset: 0x0002C9A8
        public void MarkNonSingleton(Type type)
        {
            ModestTree.Assert.That(!this._boundSingletons.Contains(type), "Found multiple creation bindings for type '{0}' in addition to AsSingle.  The AsSingle binding must be the definitive creation binding.  If this is intentional, use AsCached instead of AsSingle.", type);
            this._boundNonSingletons.Add(type);
        }

        // Token: 0x0600108C RID: 4236 RVA: 0x0002E7D4 File Offset: 0x0002C9D4
        public void MarkSingleton(Type type)
        {
            ModestTree.Assert.That(this._boundSingletons.Add(type), "Attempted to use AsSingle multiple times for type '{0}'.  As of Zenject 6+, AsSingle as can no longer be used for the same type across different bindings.  See the upgrade guide for details.", type);
            ModestTree.Assert.That(!this._boundNonSingletons.Contains(type), "Found multiple creation bindings for type '{0}' in addition to AsSingle.  The AsSingle binding must be the definitive creation binding.  If this is intentional, use AsCached instead of AsSingle.", type);
        }

        // Token: 0x0400053B RID: 1339
        private readonly HashSet<Type> _boundSingletons = new HashSet<Type>();

        // Token: 0x0400053C RID: 1340
        private readonly HashSet<Type> _boundNonSingletons = new HashSet<Type>();
    }
}
