using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x020001EF RID: 495
    public class HashSetPool<T> : StaticMemoryPool<HashSet<T>>
    {
        // Token: 0x06000A3A RID: 2618 RVA: 0x0001AEFC File Offset: 0x000190FC
        public HashSetPool() : base(null, null, 0)
        {
            base.OnSpawnMethod = new Action<HashSet<T>>(HashSetPool<T>.OnSpawned);
            base.OnDespawnedMethod = new Action<HashSet<T>>(HashSetPool<T>.OnDespawned);
        }

        // Token: 0x17000090 RID: 144
        // (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001AF2C File Offset: 0x0001912C
        public static HashSetPool<T> Instance
        {
            get
            {
                return HashSetPool<T>._instance;
            }
        }

        // Token: 0x06000A3C RID: 2620 RVA: 0x0001AF34 File Offset: 0x00019134
        private static void OnSpawned(HashSet<T> items)
        {
            ModestTree.Assert.That(items.IsEmpty<T>());
        }

        // Token: 0x06000A3D RID: 2621 RVA: 0x0001AF44 File Offset: 0x00019144
        private static void OnDespawned(HashSet<T> items)
        {
            items.Clear();
        }

        // Token: 0x04000307 RID: 775
        private static readonly HashSetPool<T> _instance = new HashSetPool<T>();
    }
}
