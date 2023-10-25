using System;
using System.Collections.Generic;

namespace Zenject.Internal
{
    // Token: 0x0200030F RID: 783
    public static class ZenPools
    {
        // Token: 0x060010C4 RID: 4292 RVA: 0x0002F36C File Offset: 0x0002D56C
        public static HashSet<T> SpawnHashSet<T>()
        {
            return HashSetPool<T>.Instance.Spawn();
        }

        // Token: 0x060010C5 RID: 4293 RVA: 0x0002F378 File Offset: 0x0002D578
        public static Dictionary<TKey, TValue> SpawnDictionary<TKey, TValue>()
        {
            return DictionaryPool<TKey, TValue>.Instance.Spawn();
        }

        // Token: 0x060010C6 RID: 4294 RVA: 0x0002F384 File Offset: 0x0002D584
        public static BindStatement SpawnStatement()
        {
            return ZenPools._bindStatementPool.Spawn();
        }

        // Token: 0x060010C7 RID: 4295 RVA: 0x0002F390 File Offset: 0x0002D590
        public static void DespawnStatement(BindStatement statement)
        {
            statement.Reset();
            ZenPools._bindStatementPool.Despawn(statement);
        }

        // Token: 0x060010C8 RID: 4296 RVA: 0x0002F3A4 File Offset: 0x0002D5A4
        public static BindInfo SpawnBindInfo()
        {
            return ZenPools._bindInfoPool.Spawn();
        }

        // Token: 0x060010C9 RID: 4297 RVA: 0x0002F3B0 File Offset: 0x0002D5B0
        public static void DespawnBindInfo(BindInfo bindInfo)
        {
            bindInfo.Reset();
            ZenPools._bindInfoPool.Despawn(bindInfo);
        }

        // Token: 0x060010CA RID: 4298 RVA: 0x0002F3C4 File Offset: 0x0002D5C4
        public static void DespawnDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            DictionaryPool<TKey, TValue>.Instance.Despawn(dictionary);
        }

        // Token: 0x060010CB RID: 4299 RVA: 0x0002F3D4 File Offset: 0x0002D5D4
        public static void DespawnHashSet<T>(HashSet<T> set)
        {
            HashSetPool<T>.Instance.Despawn(set);
        }

        // Token: 0x060010CC RID: 4300 RVA: 0x0002F3E4 File Offset: 0x0002D5E4
        public static LookupId SpawnLookupId(IProvider provider, BindingId bindingId)
        {
            LookupId lookupId = ZenPools._lookupIdPool.Spawn();
            lookupId.Provider = provider;
            lookupId.BindingId = bindingId;
            return lookupId;
        }

        // Token: 0x060010CD RID: 4301 RVA: 0x0002F400 File Offset: 0x0002D600
        public static void DespawnLookupId(LookupId lookupId)
        {
            ZenPools._lookupIdPool.Despawn(lookupId);
        }

        // Token: 0x060010CE RID: 4302 RVA: 0x0002F410 File Offset: 0x0002D610
        public static List<T> SpawnList<T>()
        {
            return ListPool<T>.Instance.Spawn();
        }

        // Token: 0x060010CF RID: 4303 RVA: 0x0002F41C File Offset: 0x0002D61C
        public static void DespawnList<T>(List<T> list)
        {
            ListPool<T>.Instance.Despawn(list);
        }

        // Token: 0x060010D0 RID: 4304 RVA: 0x0002F42C File Offset: 0x0002D62C
        public static void DespawnArray<T>(T[] arr)
        {
            ArrayPool<T>.GetPool(arr.Length).Despawn(arr);
        }

        // Token: 0x060010D1 RID: 4305 RVA: 0x0002F43C File Offset: 0x0002D63C
        public static T[] SpawnArray<T>(int length)
        {
            return ArrayPool<T>.GetPool(length).Spawn();
        }

        // Token: 0x060010D2 RID: 4306 RVA: 0x0002F44C File Offset: 0x0002D64C
        public static InjectContext SpawnInjectContext(DiContainer container, Type memberType)
        {
            InjectContext injectContext = ZenPools._contextPool.Spawn();
            injectContext.Container = container;
            injectContext.MemberType = memberType;
            return injectContext;
        }

        // Token: 0x060010D3 RID: 4307 RVA: 0x0002F468 File Offset: 0x0002D668
        public static void DespawnInjectContext(InjectContext context)
        {
            context.Reset();
            ZenPools._contextPool.Despawn(context);
        }

        // Token: 0x060010D4 RID: 4308 RVA: 0x0002F47C File Offset: 0x0002D67C
        public static InjectContext SpawnInjectContext(DiContainer container, InjectableInfo injectableInfo, InjectContext currentContext, object targetInstance, Type targetType, object concreteIdentifier)
        {
            InjectContext injectContext = ZenPools.SpawnInjectContext(container, injectableInfo.MemberType);
            injectContext.ObjectType = targetType;
            injectContext.ParentContext = currentContext;
            injectContext.ObjectInstance = targetInstance;
            injectContext.Identifier = injectableInfo.Identifier;
            injectContext.MemberName = injectableInfo.MemberName;
            injectContext.Optional = injectableInfo.Optional;
            injectContext.SourceType = injectableInfo.SourceType;
            injectContext.FallBackValue = injectableInfo.DefaultValue;
            injectContext.ConcreteIdentifier = concreteIdentifier;
            return injectContext;
        }

        // Token: 0x0400055E RID: 1374
        private static readonly StaticMemoryPool<InjectContext> _contextPool = new StaticMemoryPool<InjectContext>(null, null, 0);

        // Token: 0x0400055F RID: 1375
        private static readonly StaticMemoryPool<LookupId> _lookupIdPool = new StaticMemoryPool<LookupId>(null, null, 0);

        // Token: 0x04000560 RID: 1376
        private static readonly StaticMemoryPool<BindInfo> _bindInfoPool = new StaticMemoryPool<BindInfo>(null, null, 0);

        // Token: 0x04000561 RID: 1377
        private static readonly StaticMemoryPool<BindStatement> _bindStatementPool = new StaticMemoryPool<BindStatement>(null, null, 0);
    }
}
