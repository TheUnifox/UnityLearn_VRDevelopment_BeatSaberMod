using System;
using System.Buffers;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001C7 RID: 455
    public class MonoPoolableMemoryPool<TValue> : MemoryPool<TValue> where TValue : Component, IPoolable
    {
        // Token: 0x06000978 RID: 2424 RVA: 0x000195B4 File Offset: 0x000177B4
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x06000979 RID: 2425 RVA: 0x000195BC File Offset: 0x000177BC
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x0600097A RID: 2426 RVA: 0x000195E8 File Offset: 0x000177E8
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x0600097B RID: 2427 RVA: 0x000195FC File Offset: 0x000177FC
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x0600097C RID: 2428 RVA: 0x0001965C File Offset: 0x0001785C
        protected override void Reinitialize(TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned();
        }

        // Token: 0x0600097D RID: 2429 RVA: 0x0001967C File Offset: 0x0001787C
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TValue>();
        }

        // Token: 0x0600097E RID: 2430 RVA: 0x00019694 File Offset: 0x00017894
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002EE RID: 750
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue> where TValue : Component, IPoolable<TParam1>
    {
        // Token: 0x0600097F RID: 2431 RVA: 0x000196E4 File Offset: 0x000178E4
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x06000980 RID: 2432 RVA: 0x000196EC File Offset: 0x000178EC
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000981 RID: 2433 RVA: 0x00019718 File Offset: 0x00017918
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000982 RID: 2434 RVA: 0x0001972C File Offset: 0x0001792C
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000983 RID: 2435 RVA: 0x0001978C File Offset: 0x0001798C
        protected override void Reinitialize(TParam1 p1, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1);
        }

        // Token: 0x06000984 RID: 2436 RVA: 0x000197AC File Offset: 0x000179AC
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TValue>();
        }

        // Token: 0x06000985 RID: 2437 RVA: 0x000197C4 File Offset: 0x000179C4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002EF RID: 751
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TValue> : MemoryPool<TParam1, TParam2, TValue> where TValue : Component, IPoolable<TParam1, TParam2>
    {
        // Token: 0x06000986 RID: 2438 RVA: 0x00019814 File Offset: 0x00017A14
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x06000987 RID: 2439 RVA: 0x0001981C File Offset: 0x00017A1C
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000988 RID: 2440 RVA: 0x00019848 File Offset: 0x00017A48
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000989 RID: 2441 RVA: 0x0001985C File Offset: 0x00017A5C
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x0600098A RID: 2442 RVA: 0x000198BC File Offset: 0x00017ABC
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2);
        }

        // Token: 0x0600098B RID: 2443 RVA: 0x000198DC File Offset: 0x00017ADC
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TValue>();
        }

        // Token: 0x0600098C RID: 2444 RVA: 0x000198F4 File Offset: 0x00017AF4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F0 RID: 752
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TValue> : MemoryPool<TParam1, TParam2, TParam3, TValue> where TValue : Component, IPoolable<TParam1, TParam2, TParam3>
    {
        // Token: 0x0600098D RID: 2445 RVA: 0x00019944 File Offset: 0x00017B44
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x0600098E RID: 2446 RVA: 0x0001994C File Offset: 0x00017B4C
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x0600098F RID: 2447 RVA: 0x00019978 File Offset: 0x00017B78
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000990 RID: 2448 RVA: 0x0001998C File Offset: 0x00017B8C
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000991 RID: 2449 RVA: 0x000199EC File Offset: 0x00017BEC
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2, p3);
        }

        // Token: 0x06000992 RID: 2450 RVA: 0x00019A10 File Offset: 0x00017C10
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TValue>();
        }

        // Token: 0x06000993 RID: 2451 RVA: 0x00019A28 File Offset: 0x00017C28
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F1 RID: 753
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        // Token: 0x06000994 RID: 2452 RVA: 0x00019A78 File Offset: 0x00017C78
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x06000995 RID: 2453 RVA: 0x00019A80 File Offset: 0x00017C80
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000996 RID: 2454 RVA: 0x00019AAC File Offset: 0x00017CAC
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000997 RID: 2455 RVA: 0x00019AC0 File Offset: 0x00017CC0
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000998 RID: 2456 RVA: 0x00019B20 File Offset: 0x00017D20
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2, p3, p4);
        }

        // Token: 0x06000999 RID: 2457 RVA: 0x00019B48 File Offset: 0x00017D48
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>();
        }

        // Token: 0x0600099A RID: 2458 RVA: 0x00019B60 File Offset: 0x00017D60
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F2 RID: 754
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        // Token: 0x0600099B RID: 2459 RVA: 0x00019BB0 File Offset: 0x00017DB0
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x0600099C RID: 2460 RVA: 0x00019BB8 File Offset: 0x00017DB8
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x0600099D RID: 2461 RVA: 0x00019BE4 File Offset: 0x00017DE4
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x0600099E RID: 2462 RVA: 0x00019BF8 File Offset: 0x00017DF8
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x0600099F RID: 2463 RVA: 0x00019C58 File Offset: 0x00017E58
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2, p3, p4, p5);
        }

        // Token: 0x060009A0 RID: 2464 RVA: 0x00019C80 File Offset: 0x00017E80
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>();
        }

        // Token: 0x060009A1 RID: 2465 RVA: 0x00019C98 File Offset: 0x00017E98
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F3 RID: 755
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        // Token: 0x060009A2 RID: 2466 RVA: 0x00019CE8 File Offset: 0x00017EE8
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x060009A3 RID: 2467 RVA: 0x00019CF0 File Offset: 0x00017EF0
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x060009A4 RID: 2468 RVA: 0x00019D1C File Offset: 0x00017F1C
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x060009A5 RID: 2469 RVA: 0x00019D30 File Offset: 0x00017F30
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x060009A6 RID: 2470 RVA: 0x00019D90 File Offset: 0x00017F90
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2, p3, p4, p5, p6);
        }

        // Token: 0x060009A7 RID: 2471 RVA: 0x00019DBC File Offset: 0x00017FBC
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>();
        }

        // Token: 0x060009A8 RID: 2472 RVA: 0x00019DD4 File Offset: 0x00017FD4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F4 RID: 756
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        // Token: 0x060009A9 RID: 2473 RVA: 0x00019E24 File Offset: 0x00018024
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x060009AA RID: 2474 RVA: 0x00019E2C File Offset: 0x0001802C
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x060009AB RID: 2475 RVA: 0x00019E58 File Offset: 0x00018058
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x060009AC RID: 2476 RVA: 0x00019E6C File Offset: 0x0001806C
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x060009AD RID: 2477 RVA: 0x00019ECC File Offset: 0x000180CC
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2, p3, p4, p5, p6, p7);
        }

        // Token: 0x060009AE RID: 2478 RVA: 0x00019EF8 File Offset: 0x000180F8
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>();
        }

        // Token: 0x060009AF RID: 2479 RVA: 0x00019F10 File Offset: 0x00018110
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F5 RID: 757
        private Transform _originalParent;
    }

    public class MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> where TValue : Component, IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        // Token: 0x060009B0 RID: 2480 RVA: 0x00019F60 File Offset: 0x00018160
        [Inject]
        public MonoPoolableMemoryPool()
        {
        }

        // Token: 0x060009B1 RID: 2481 RVA: 0x00019F68 File Offset: 0x00018168
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x060009B2 RID: 2482 RVA: 0x00019F94 File Offset: 0x00018194
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x060009B3 RID: 2483 RVA: 0x00019FA8 File Offset: 0x000181A8
        protected override void OnDespawned(TValue item)
        {
            item.OnDespawned();
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x060009B4 RID: 2484 RVA: 0x0001A008 File Offset: 0x00018208
        protected override void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TValue item)
        {
            item.gameObject.SetActive(true);
            item.OnSpawned(p1, p2, p3, p4, p5, p6, p7, p8);
        }

        // Token: 0x060009B5 RID: 2485 RVA: 0x0001A040 File Offset: 0x00018240
        private static object __zenCreate(object[] P_0)
        {
            return new MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>();
        }

        // Token: 0x060009B6 RID: 2486 RVA: 0x0001A058 File Offset: 0x00018258
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoPoolableMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002F6 RID: 758
        private Transform _originalParent;
    }
}
