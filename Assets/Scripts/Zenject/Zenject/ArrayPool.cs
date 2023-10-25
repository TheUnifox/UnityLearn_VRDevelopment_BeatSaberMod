using System;
using System.Collections.Generic;

namespace Zenject
{
    // Token: 0x020001ED RID: 493
    public class ArrayPool<T> : StaticMemoryPoolBaseBase<T[]>
    {
        // Token: 0x06000A2F RID: 2607 RVA: 0x0001AE04 File Offset: 0x00019004
        public ArrayPool(int length) : base(new Action<T[]>(ArrayPool<T>.OnDespawned))
        {
            this._length = length;
        }

        // Token: 0x06000A30 RID: 2608 RVA: 0x0001AE20 File Offset: 0x00019020
        private static void OnDespawned(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = default(T);
            }
        }

        // Token: 0x06000A31 RID: 2609 RVA: 0x0001AE4C File Offset: 0x0001904C
        public T[] Spawn()
        {
            return base.SpawnInternal();
        }

        // Token: 0x06000A32 RID: 2610 RVA: 0x0001AE54 File Offset: 0x00019054
        protected override T[] Alloc()
        {
            return new T[this._length];
        }

        // Token: 0x06000A33 RID: 2611 RVA: 0x0001AE64 File Offset: 0x00019064
        public static ArrayPool<T> GetPool(int length)
        {
            ArrayPool<T> arrayPool;
            if (!ArrayPool<T>._pools.TryGetValue(length, out arrayPool))
            {
                arrayPool = new ArrayPool<T>(length);
                ArrayPool<T>._pools.Add(length, arrayPool);
            }
            return arrayPool;
        }

        // Token: 0x04000304 RID: 772
        private readonly int _length;

        // Token: 0x04000305 RID: 773
        private static readonly Dictionary<int, ArrayPool<T>> _pools = new Dictionary<int, ArrayPool<T>>();
    }
}
