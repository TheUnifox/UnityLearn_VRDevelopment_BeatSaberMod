using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000189 RID: 393
    public abstract class KeyedFactoryBase<TBase, TKey> : IValidatable
    {
        // Token: 0x17000056 RID: 86
        // (get) Token: 0x06000820 RID: 2080 RVA: 0x000161B4 File Offset: 0x000143B4
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x06000821 RID: 2081
        protected abstract IEnumerable<Type> ProvidedTypes { get; }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x06000822 RID: 2082 RVA: 0x000161BC File Offset: 0x000143BC
        public ICollection<TKey> Keys
        {
            get
            {
                return this._typeMap.Keys;
            }
        }

        // Token: 0x17000059 RID: 89
        // (get) Token: 0x06000823 RID: 2083 RVA: 0x000161CC File Offset: 0x000143CC
        protected Dictionary<TKey, Type> TypeMap
        {
            get
            {
                return this._typeMap;
            }
        }

        // Token: 0x06000824 RID: 2084 RVA: 0x000161D4 File Offset: 0x000143D4
        [Inject]
        public void Initialize()
        {
            ModestTree.Assert.That(this._fallbackType == null || this._fallbackType.DerivesFromOrEqual<TBase>(), "Expected fallback type '{0}' to derive from '{1}'", this._fallbackType, typeof(TBase));
            this._typeMap = this._typePairs.ToDictionary((ModestTree.Util.ValuePair<TKey, Type> x) => x.First, (ModestTree.Util.ValuePair<TKey, Type> x) => x.Second);
            this._typePairs.Clear();
        }

        // Token: 0x06000825 RID: 2085 RVA: 0x00016274 File Offset: 0x00014474
        public bool HasKey(TKey key)
        {
            return this._typeMap.ContainsKey(key);
        }

        // Token: 0x06000826 RID: 2086 RVA: 0x00016284 File Offset: 0x00014484
        protected Type GetTypeForKey(TKey key)
        {
            Type result;
            if (!this._typeMap.TryGetValue(key, out result))
            {
                ModestTree.Assert.IsNotNull(this._fallbackType, "Could not find instance for key '{0}'", key);
                return this._fallbackType;
            }
            return result;
        }

        // Token: 0x06000827 RID: 2087 RVA: 0x000162C0 File Offset: 0x000144C0
        public virtual void Validate()
        {
            foreach (Type concreteType in this._typeMap.Values)
            {
                this.Container.InstantiateExplicit(concreteType, ValidationUtil.CreateDefaultArgs(this.ProvidedTypes.ToArray<Type>()));
            }
        }

        // Token: 0x06000828 RID: 2088 RVA: 0x00016330 File Offset: 0x00014530
        protected static ConditionCopyNonLazyBinder AddBindingInternal<TDerived>(DiContainer container, TKey key) where TDerived : TBase
        {
            return container.Bind<ModestTree.Util.ValuePair<TKey, Type>>().FromInstance(ModestTree.Util.ValuePair.New<TKey, Type>(key, typeof(TDerived)));
        }

        // Token: 0x0600082A RID: 2090 RVA: 0x00016358 File Offset: 0x00014558
        private static void __zenFieldSetter0(object P_0, object P_1)
        {
            ((KeyedFactoryBase<TBase, TKey>)P_0)._container = (DiContainer)P_1;
        }

        // Token: 0x0600082B RID: 2091 RVA: 0x00016378 File Offset: 0x00014578
        private static void __zenFieldSetter1(object P_0, object P_1)
        {
            ((KeyedFactoryBase<TBase, TKey>)P_0)._typePairs = (List<ModestTree.Util.ValuePair<TKey, Type>>)P_1;
        }

        // Token: 0x0600082C RID: 2092 RVA: 0x00016398 File Offset: 0x00014598
        private static void __zenFieldSetter2(object P_0, object P_1)
        {
            ((KeyedFactoryBase<TBase, TKey>)P_0)._fallbackType = (Type)P_1;
        }

        // Token: 0x0600082D RID: 2093 RVA: 0x000163B8 File Offset: 0x000145B8
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((KeyedFactoryBase<TBase, TKey>)P_0).Initialize();
        }

        // Token: 0x0600082E RID: 2094 RVA: 0x000163C8 File Offset: 0x000145C8
        [Zenject.Internal.Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(KeyedFactoryBase<TBase, TKey>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(KeyedFactoryBase<TBase, TKey>.__zenInjectMethod0), new InjectableInfo[0], "Initialize")
            }, new InjectTypeInfo.InjectMemberInfo[]
            {
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(KeyedFactoryBase<TBase, TKey>.__zenFieldSetter0), new InjectableInfo(false, null, "_container", typeof(DiContainer), null, InjectSources.Any)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(KeyedFactoryBase<TBase, TKey>.__zenFieldSetter1), new InjectableInfo(true, null, "_typePairs", typeof(List<ModestTree.Util.ValuePair<TKey, Type>>), null, InjectSources.Any)),
                new InjectTypeInfo.InjectMemberInfo(new ZenMemberSetterMethod(KeyedFactoryBase<TBase, TKey>.__zenFieldSetter2), new InjectableInfo(true, null, "_fallbackType", typeof(Type), null, InjectSources.Any))
            });
        }

        // Token: 0x040002BD RID: 701
        [Inject]
        private DiContainer _container;

        // Token: 0x040002BE RID: 702
        [InjectOptional]
        private List<ModestTree.Util.ValuePair<TKey, Type>> _typePairs;

        // Token: 0x040002BF RID: 703
        private Dictionary<TKey, Type> _typeMap;

        // Token: 0x040002C0 RID: 704
        [InjectOptional]
        private Type _fallbackType;
    }
}
