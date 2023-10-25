using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Token: 0x020001EE RID: 494
    public class DictionaryPool<TKey, TValue> : StaticMemoryPool<Dictionary<TKey, TValue>>
    {
        // Token: 0x06000A35 RID: 2613 RVA: 0x0001AEA0 File Offset: 0x000190A0
        public DictionaryPool() : base(null, null, 0)
        {
            base.OnSpawnMethod = new Action<Dictionary<TKey, TValue>>(DictionaryPool<TKey, TValue>.OnSpawned);
            base.OnDespawnedMethod = new Action<Dictionary<TKey, TValue>>(DictionaryPool<TKey, TValue>.OnDespawned);
        }

        // Token: 0x1700008F RID: 143
        // (get) Token: 0x06000A36 RID: 2614 RVA: 0x0001AED0 File Offset: 0x000190D0
        public static DictionaryPool<TKey, TValue> Instance
        {
            get
            {
                return DictionaryPool<TKey, TValue>._instance;
            }
        }

        // Token: 0x06000A37 RID: 2615 RVA: 0x0001AED8 File Offset: 0x000190D8
        private static void OnSpawned(Dictionary<TKey, TValue> items)
        {
            ModestTree.Assert.That(items.IsEmpty<KeyValuePair<TKey, TValue>>());
        }

        // Token: 0x06000A38 RID: 2616 RVA: 0x0001AEE8 File Offset: 0x000190E8
        private static void OnDespawned(Dictionary<TKey, TValue> items)
        {
            items.Clear();
        }

        // Token: 0x04000306 RID: 774
        private static readonly DictionaryPool<TKey, TValue> _instance = new DictionaryPool<TKey, TValue>();
    }
}
