using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x020001F0 RID: 496
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        // Token: 0x06000A3F RID: 2623 RVA: 0x0001AF58 File Offset: 0x00019158
        public ListPool() : base(null, null, 0)
        {
            base.OnDespawnedMethod = new Action<List<T>>(this.OnDespawned);
        }

        // Token: 0x17000091 RID: 145
        // (get) Token: 0x06000A40 RID: 2624 RVA: 0x0001AF78 File Offset: 0x00019178
        public static ListPool<T> Instance
        {
            get
            {
                return ListPool<T>._instance;
            }
        }

        // Token: 0x06000A41 RID: 2625 RVA: 0x0001AF80 File Offset: 0x00019180
        private void OnDespawned(List<T> list)
        {
            list.Clear();
        }

        // Token: 0x04000308 RID: 776
        private static readonly ListPool<T> _instance = new ListPool<T>();
    }
}
