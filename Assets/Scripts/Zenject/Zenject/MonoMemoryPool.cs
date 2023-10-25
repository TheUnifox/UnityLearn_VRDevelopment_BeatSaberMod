using System;
using System.Buffers;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001C1 RID: 449
    public class MonoMemoryPool<TValue> : MemoryPool<TValue> where TValue : Component
    {
        // Token: 0x0600094E RID: 2382 RVA: 0x00018F24 File Offset: 0x00017124
        [Inject]
        public MonoMemoryPool()
        {
        }

        // Token: 0x0600094F RID: 2383 RVA: 0x00018F2C File Offset: 0x0001712C
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000950 RID: 2384 RVA: 0x00018F58 File Offset: 0x00017158
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000951 RID: 2385 RVA: 0x00018F6C File Offset: 0x0001716C
        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        // Token: 0x06000952 RID: 2386 RVA: 0x00018F80 File Offset: 0x00017180
        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000953 RID: 2387 RVA: 0x00018FD4 File Offset: 0x000171D4
        private static object __zenCreate(object[] P_0)
        {
            return new MonoMemoryPool<TValue>();
        }

        // Token: 0x06000954 RID: 2388 RVA: 0x00018FEC File Offset: 0x000171EC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoMemoryPool<TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoMemoryPool<TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002E8 RID: 744
        private Transform _originalParent;
    }

    public class MonoMemoryPool<TParam1, TValue> : MemoryPool<TParam1, TValue> where TValue : Component
    {
        // Token: 0x06000955 RID: 2389 RVA: 0x0001903C File Offset: 0x0001723C
        [Inject]
        public MonoMemoryPool()
        {
        }

        // Token: 0x06000956 RID: 2390 RVA: 0x00019044 File Offset: 0x00017244
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000957 RID: 2391 RVA: 0x00019070 File Offset: 0x00017270
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000958 RID: 2392 RVA: 0x00019084 File Offset: 0x00017284
        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        // Token: 0x06000959 RID: 2393 RVA: 0x00019098 File Offset: 0x00017298
        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x0600095A RID: 2394 RVA: 0x000190EC File Offset: 0x000172EC
        private static object __zenCreate(object[] P_0)
        {
            return new MonoMemoryPool<TParam1, TValue>();
        }

        // Token: 0x0600095B RID: 2395 RVA: 0x00019104 File Offset: 0x00017304
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoMemoryPool<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoMemoryPool<TParam1, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002E9 RID: 745
        private Transform _originalParent;
    }

    public class MonoMemoryPool<TParam1, TParam2, TValue> : MemoryPool<TParam1, TParam2, TValue> where TValue : Component
    {
        // Token: 0x0600095C RID: 2396 RVA: 0x00019154 File Offset: 0x00017354
        [Inject]
        public MonoMemoryPool()
        {
        }

        // Token: 0x0600095D RID: 2397 RVA: 0x0001915C File Offset: 0x0001735C
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x0600095E RID: 2398 RVA: 0x00019188 File Offset: 0x00017388
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x0600095F RID: 2399 RVA: 0x0001919C File Offset: 0x0001739C
        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        // Token: 0x06000960 RID: 2400 RVA: 0x000191B0 File Offset: 0x000173B0
        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000961 RID: 2401 RVA: 0x00019204 File Offset: 0x00017404
        private static object __zenCreate(object[] P_0)
        {
            return new MonoMemoryPool<TParam1, TParam2, TValue>();
        }

        // Token: 0x06000962 RID: 2402 RVA: 0x0001921C File Offset: 0x0001741C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoMemoryPool<TParam1, TParam2, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoMemoryPool<TParam1, TParam2, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002EA RID: 746
        private Transform _originalParent;
    }

    public class MonoMemoryPool<TParam1, TParam2, TParam3, TValue> : MemoryPool<TParam1, TParam2, TParam3, TValue> where TValue : Component
    {
        // Token: 0x06000963 RID: 2403 RVA: 0x0001926C File Offset: 0x0001746C
        [Inject]
        public MonoMemoryPool()
        {
        }

        // Token: 0x06000964 RID: 2404 RVA: 0x00019274 File Offset: 0x00017474
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000965 RID: 2405 RVA: 0x000192A0 File Offset: 0x000174A0
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000966 RID: 2406 RVA: 0x000192B4 File Offset: 0x000174B4
        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        // Token: 0x06000967 RID: 2407 RVA: 0x000192C8 File Offset: 0x000174C8
        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000968 RID: 2408 RVA: 0x0001931C File Offset: 0x0001751C
        private static object __zenCreate(object[] P_0)
        {
            return new MonoMemoryPool<TParam1, TParam2, TParam3, TValue>();
        }

        // Token: 0x06000969 RID: 2409 RVA: 0x00019334 File Offset: 0x00017534
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoMemoryPool<TParam1, TParam2, TParam3, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoMemoryPool<TParam1, TParam2, TParam3, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002EB RID: 747
        private Transform _originalParent;
    }

    public class MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> where TValue : Component
    {
        // Token: 0x0600096A RID: 2410 RVA: 0x00019384 File Offset: 0x00017584
        [Inject]
        public MonoMemoryPool()
        {
        }

        // Token: 0x0600096B RID: 2411 RVA: 0x0001938C File Offset: 0x0001758C
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x0600096C RID: 2412 RVA: 0x000193B8 File Offset: 0x000175B8
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x0600096D RID: 2413 RVA: 0x000193CC File Offset: 0x000175CC
        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        // Token: 0x0600096E RID: 2414 RVA: 0x000193E0 File Offset: 0x000175E0
        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x0600096F RID: 2415 RVA: 0x00019434 File Offset: 0x00017634
        private static object __zenCreate(object[] P_0)
        {
            return new MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>();
        }

        // Token: 0x06000970 RID: 2416 RVA: 0x0001944C File Offset: 0x0001764C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002EC RID: 748
        private Transform _originalParent;
    }

    public class MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> where TValue : Component
    {
        // Token: 0x06000971 RID: 2417 RVA: 0x0001949C File Offset: 0x0001769C
        [Inject]
        public MonoMemoryPool()
        {
        }

        // Token: 0x06000972 RID: 2418 RVA: 0x000194A4 File Offset: 0x000176A4
        protected override void OnCreated(TValue item)
        {
            item.gameObject.SetActive(false);
            this._originalParent = item.transform.parent;
        }

        // Token: 0x06000973 RID: 2419 RVA: 0x000194D0 File Offset: 0x000176D0
        protected override void OnDestroyed(TValue item)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }

        // Token: 0x06000974 RID: 2420 RVA: 0x000194E4 File Offset: 0x000176E4
        protected override void OnSpawned(TValue item)
        {
            item.gameObject.SetActive(true);
        }

        // Token: 0x06000975 RID: 2421 RVA: 0x000194F8 File Offset: 0x000176F8
        protected override void OnDespawned(TValue item)
        {
            item.gameObject.SetActive(false);
            if (item.transform.parent != this._originalParent)
            {
                item.transform.SetParent(this._originalParent, false);
            }
        }

        // Token: 0x06000976 RID: 2422 RVA: 0x0001954C File Offset: 0x0001774C
        private static object __zenCreate(object[] P_0)
        {
            return new MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>();
        }

        // Token: 0x06000977 RID: 2423 RVA: 0x00019564 File Offset: 0x00017764
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MonoMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040002ED RID: 749
        private Transform _originalParent;
    }
}
