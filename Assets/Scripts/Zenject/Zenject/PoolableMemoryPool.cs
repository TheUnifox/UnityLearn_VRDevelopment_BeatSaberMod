using System;
using System.Buffers;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001D2 RID: 466
    public class PoolableMemoryPool<TValue> : MemoryPool<TValue> where TValue : IPoolable
    {
        // Token: 0x060009BF RID: 2495 RVA: 0x0001A218 File Offset: 0x00018418
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009C0 RID: 2496 RVA: 0x0001A228 File Offset: 0x00018428
        protected override void Reinitialize(TValue item)
        {
            item.OnSpawned();
        }

        // Token: 0x060009C2 RID: 2498 RVA: 0x0001A240 File Offset: 0x00018440
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TValue>();
        }

        // Token: 0x060009C3 RID: 2499 RVA: 0x0001A258 File Offset: 0x00018458
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue> where TValue : IPoolable<TParam1>
    {
        // Token: 0x060009C4 RID: 2500 RVA: 0x0001A2A8 File Offset: 0x000184A8
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009C5 RID: 2501 RVA: 0x0001A2B8 File Offset: 0x000184B8
        protected override void Reinitialize(TParam1 p1, TValue item)
        {
            item.OnSpawned(p1);
        }

        // Token: 0x060009C7 RID: 2503 RVA: 0x0001A2D0 File Offset: 0x000184D0
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TValue>();
        }

        // Token: 0x060009C8 RID: 2504 RVA: 0x0001A2E8 File Offset: 0x000184E8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TValue> : MemoryPool<TParam1, TParam2, TValue> where TValue : IPoolable<TParam1, TParam2>
    {
        // Token: 0x060009C9 RID: 2505 RVA: 0x0001A338 File Offset: 0x00018538
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009CA RID: 2506 RVA: 0x0001A348 File Offset: 0x00018548
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TValue item)
        {
            item.OnSpawned(p1, p2);
        }

        // Token: 0x060009CC RID: 2508 RVA: 0x0001A364 File Offset: 0x00018564
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TValue>();
        }

        // Token: 0x060009CD RID: 2509 RVA: 0x0001A37C File Offset: 0x0001857C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TValue> : MemoryPool<TParam1, TParam2, TParam3, TValue> where TValue : IPoolable<TParam1, TParam2, TParam3>
    {
        // Token: 0x060009CE RID: 2510 RVA: 0x0001A3CC File Offset: 0x000185CC
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009CF RID: 2511 RVA: 0x0001A3DC File Offset: 0x000185DC
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TValue item)
        {
            item.OnSpawned(p1, p2, p3);
        }

        // Token: 0x060009D1 RID: 2513 RVA: 0x0001A3F8 File Offset: 0x000185F8
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TParam3, TValue>();
        }

        // Token: 0x060009D2 RID: 2514 RVA: 0x0001A410 File Offset: 0x00018610
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TParam3, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TParam3, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        // Token: 0x060009D3 RID: 2515 RVA: 0x0001A460 File Offset: 0x00018660
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009D4 RID: 2516 RVA: 0x0001A470 File Offset: 0x00018670
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4);
        }

        // Token: 0x060009D6 RID: 2518 RVA: 0x0001A48C File Offset: 0x0001868C
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>();
        }

        // Token: 0x060009D7 RID: 2519 RVA: 0x0001A4A4 File Offset: 0x000186A4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        // Token: 0x060009D8 RID: 2520 RVA: 0x0001A4F4 File Offset: 0x000186F4
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009D9 RID: 2521 RVA: 0x0001A504 File Offset: 0x00018704
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5);
        }

        // Token: 0x060009DB RID: 2523 RVA: 0x0001A524 File Offset: 0x00018724
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>();
        }

        // Token: 0x060009DC RID: 2524 RVA: 0x0001A53C File Offset: 0x0001873C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        // Token: 0x060009DD RID: 2525 RVA: 0x0001A58C File Offset: 0x0001878C
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009DE RID: 2526 RVA: 0x0001A59C File Offset: 0x0001879C
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5, p6);
        }

        // Token: 0x060009E0 RID: 2528 RVA: 0x0001A5BC File Offset: 0x000187BC
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>();
        }

        // Token: 0x060009E1 RID: 2529 RVA: 0x0001A5D4 File Offset: 0x000187D4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        // Token: 0x060009E2 RID: 2530 RVA: 0x0001A624 File Offset: 0x00018824
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009E3 RID: 2531 RVA: 0x0001A634 File Offset: 0x00018834
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5, p6, p7);
        }

        // Token: 0x060009E5 RID: 2533 RVA: 0x0001A658 File Offset: 0x00018858
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>();
        }

        // Token: 0x060009E6 RID: 2534 RVA: 0x0001A670 File Offset: 0x00018870
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> where TValue : IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        // Token: 0x060009E7 RID: 2535 RVA: 0x0001A6C0 File Offset: 0x000188C0
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
        }

        // Token: 0x060009E8 RID: 2536 RVA: 0x0001A6D0 File Offset: 0x000188D0
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TValue item)
        {
            item.OnSpawned(p1, p2, p3, p4, p5, p6, p7, p8);
        }

        // Token: 0x060009EA RID: 2538 RVA: 0x0001A700 File Offset: 0x00018900
        private static object __zenCreate(object[] P_0)
        {
            return new PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>();
        }

        // Token: 0x060009EB RID: 2539 RVA: 0x0001A718 File Offset: 0x00018918
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}