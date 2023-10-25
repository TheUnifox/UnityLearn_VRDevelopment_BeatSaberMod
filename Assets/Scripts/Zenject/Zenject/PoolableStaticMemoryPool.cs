using System;

namespace Zenject
{
    // Token: 0x020001DB RID: 475
    public class PoolableStaticMemoryPool<TValue> : StaticMemoryPool<TValue> where TValue : class, IPoolable, new()
    {
        // Token: 0x060009EC RID: 2540 RVA: 0x0001A768 File Offset: 0x00018968
        public PoolableStaticMemoryPool() : base(new Action<TValue>(PoolableStaticMemoryPool<TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TValue>.OnDespawned), 0)
        {
        }

        // Token: 0x060009ED RID: 2541 RVA: 0x0001A78C File Offset: 0x0001898C
        private static void OnSpawned(TValue value)
        {
            value.OnSpawned();
        }

        // Token: 0x060009EE RID: 2542 RVA: 0x0001A79C File Offset: 0x0001899C
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TValue> : StaticMemoryPool<TParam1, TValue> where TValue : class, IPoolable<TParam1>, new()
    {
        // Token: 0x060009EF RID: 2543 RVA: 0x0001A7AC File Offset: 0x000189AC
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TValue>(PoolableStaticMemoryPool<TParam1, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TValue>.OnDespawned))
        {
        }

        // Token: 0x060009F0 RID: 2544 RVA: 0x0001A7CC File Offset: 0x000189CC
        private static void OnSpawned(TParam1 p1, TValue value)
        {
            value.OnSpawned(p1);
        }

        // Token: 0x060009F1 RID: 2545 RVA: 0x0001A7DC File Offset: 0x000189DC
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TParam2, TValue> : StaticMemoryPool<TParam1, TParam2, TValue> where TValue : class, IPoolable<TParam1, TParam2>, new()
    {
        // Token: 0x060009F2 RID: 2546 RVA: 0x0001A7EC File Offset: 0x000189EC
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TParam2, TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TValue>.OnDespawned))
        {
        }

        // Token: 0x060009F3 RID: 2547 RVA: 0x0001A80C File Offset: 0x00018A0C
        private static void OnSpawned(TParam1 p1, TParam2 p2, TValue value)
        {
            value.OnSpawned(p1, p2);
        }

        // Token: 0x060009F4 RID: 2548 RVA: 0x0001A81C File Offset: 0x00018A1C
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TValue> : StaticMemoryPool<TParam1, TParam2, TParam3, TValue> where TValue : class, IPoolable<TParam1, TParam2, TParam3>, new()
    {
        // Token: 0x060009F5 RID: 2549 RVA: 0x0001A82C File Offset: 0x00018A2C
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TParam2, TParam3, TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TValue>.OnDespawned))
        {
        }

        // Token: 0x060009F6 RID: 2550 RVA: 0x0001A84C File Offset: 0x00018A4C
        private static void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TValue value)
        {
            value.OnSpawned(p1, p2, p3);
        }

        // Token: 0x060009F7 RID: 2551 RVA: 0x0001A85C File Offset: 0x00018A5C
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> where TValue : class, IPoolable<TParam1, TParam2, TParam3, TParam4>, new()
    {
        // Token: 0x060009F8 RID: 2552 RVA: 0x0001A86C File Offset: 0x00018A6C
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TParam2, TParam3, TParam4, TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>.OnDespawned))
        {
        }

        // Token: 0x060009F9 RID: 2553 RVA: 0x0001A88C File Offset: 0x00018A8C
        private static void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TValue value)
        {
            value.OnSpawned(p1, p2, p3, p4);
        }

        // Token: 0x060009FA RID: 2554 RVA: 0x0001A8A0 File Offset: 0x00018AA0
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> where TValue : class, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>, new()
    {
        // Token: 0x060009FB RID: 2555 RVA: 0x0001A8B0 File Offset: 0x00018AB0
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.OnDespawned))
        {
        }

        // Token: 0x060009FC RID: 2556 RVA: 0x0001A8D0 File Offset: 0x00018AD0
        private static void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TValue value)
        {
            value.OnSpawned(p1, p2, p3, p4, p5);
        }

        // Token: 0x060009FD RID: 2557 RVA: 0x0001A8E4 File Offset: 0x00018AE4
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> where TValue : class, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>, new()
    {
        // Token: 0x060009FE RID: 2558 RVA: 0x0001A8F4 File Offset: 0x00018AF4
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.OnDespawned))
        {
        }

        // Token: 0x060009FF RID: 2559 RVA: 0x0001A914 File Offset: 0x00018B14
        private static void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TValue value)
        {
            value.OnSpawned(p1, p2, p3, p4, p5, p6);
        }

        // Token: 0x06000A00 RID: 2560 RVA: 0x0001A92C File Offset: 0x00018B2C
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }

    public class PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> where TValue : class, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>, new()
    {
        // Token: 0x06000A01 RID: 2561 RVA: 0x0001A93C File Offset: 0x00018B3C
        public PoolableStaticMemoryPool() : base(new Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>.OnSpawned), new Action<TValue>(PoolableStaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>.OnDespawned))
        {
        }

        // Token: 0x06000A02 RID: 2562 RVA: 0x0001A95C File Offset: 0x00018B5C
        private static void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TValue value)
        {
            value.OnSpawned(p1, p2, p3, p4, p5, p6, p7);
        }

        // Token: 0x06000A03 RID: 2563 RVA: 0x0001A974 File Offset: 0x00018B74
        private static void OnDespawned(TValue value)
        {
            value.OnDespawned();
        }
    }
}
