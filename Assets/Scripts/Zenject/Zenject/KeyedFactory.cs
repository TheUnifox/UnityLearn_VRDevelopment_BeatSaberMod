using System;
using System.Collections.Generic;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200018B RID: 395
    public class KeyedFactory<TBase, TKey> : KeyedFactoryBase<TBase, TKey>
    {
        // Token: 0x1700005A RID: 90
        // (get) Token: 0x06000835 RID: 2101 RVA: 0x0001655C File Offset: 0x0001475C
        protected override IEnumerable<Type> ProvidedTypes
        {
            get
            {
                return new Type[0];
            }
        }

        // Token: 0x06000836 RID: 2102 RVA: 0x00016564 File Offset: 0x00014764
        public virtual TBase Create(TKey key)
        {
            Type typeForKey = base.GetTypeForKey(key);
            return (TBase)((object)base.Container.Instantiate(typeForKey));
        }

        // Token: 0x06000838 RID: 2104 RVA: 0x00016594 File Offset: 0x00014794
        private static object __zenCreate(object[] P_0)
        {
            return new KeyedFactory<TBase, TKey>();
        }

        // Token: 0x06000839 RID: 2105 RVA: 0x000165AC File Offset: 0x000147AC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(KeyedFactory<TBase, TKey>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(KeyedFactory<TBase, TKey>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class KeyedFactory<TBase, TKey, TParam1> : KeyedFactoryBase<TBase, TKey>
    {
        // Token: 0x1700005B RID: 91
        // (get) Token: 0x0600083A RID: 2106 RVA: 0x000165FC File Offset: 0x000147FC
        protected override IEnumerable<Type> ProvidedTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(TParam1)
                };
            }
        }

        // Token: 0x0600083B RID: 2107 RVA: 0x00016614 File Offset: 0x00014814
        public virtual TBase Create(TKey key, TParam1 param1)
        {
            return (TBase)((object)base.Container.InstantiateExplicit(base.GetTypeForKey(key), new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1)
            }));
        }

        // Token: 0x0600083D RID: 2109 RVA: 0x00016648 File Offset: 0x00014848
        private static object __zenCreate(object[] P_0)
        {
            return new KeyedFactory<TBase, TKey, TParam1>();
        }

        // Token: 0x0600083E RID: 2110 RVA: 0x00016660 File Offset: 0x00014860
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(KeyedFactory<TBase, TKey, TParam1>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(KeyedFactory<TBase, TKey, TParam1>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class KeyedFactory<TBase, TKey, TParam1, TParam2> : KeyedFactoryBase<TBase, TKey>
    {
        // Token: 0x1700005C RID: 92
        // (get) Token: 0x0600083F RID: 2111 RVA: 0x000166B0 File Offset: 0x000148B0
        protected override IEnumerable<Type> ProvidedTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(TParam1),
                    typeof(TParam2)
                };
            }
        }

        // Token: 0x06000840 RID: 2112 RVA: 0x000166D4 File Offset: 0x000148D4
        public virtual TBase Create(TKey key, TParam1 param1, TParam2 param2)
        {
            return (TBase)((object)base.Container.InstantiateExplicit(base.GetTypeForKey(key), new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2)
            }));
        }

        // Token: 0x06000842 RID: 2114 RVA: 0x00016714 File Offset: 0x00014914
        private static object __zenCreate(object[] P_0)
        {
            return new KeyedFactory<TBase, TKey, TParam1, TParam2>();
        }

        // Token: 0x06000843 RID: 2115 RVA: 0x0001672C File Offset: 0x0001492C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(KeyedFactory<TBase, TKey, TParam1, TParam2>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(KeyedFactory<TBase, TKey, TParam1, TParam2>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3> : KeyedFactoryBase<TBase, TKey>
    {
        // Token: 0x1700005D RID: 93
        // (get) Token: 0x06000844 RID: 2116 RVA: 0x0001677C File Offset: 0x0001497C
        protected override IEnumerable<Type> ProvidedTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(TParam1),
                    typeof(TParam2),
                    typeof(TParam3)
                };
            }
        }

        // Token: 0x06000845 RID: 2117 RVA: 0x000167AC File Offset: 0x000149AC
        public virtual TBase Create(TKey key, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return (TBase)((object)base.Container.InstantiateExplicit(base.GetTypeForKey(key), new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3)
            }));
        }

        // Token: 0x06000847 RID: 2119 RVA: 0x00016804 File Offset: 0x00014A04
        private static object __zenCreate(object[] P_0)
        {
            return new KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3>();
        }

        // Token: 0x06000848 RID: 2120 RVA: 0x0001681C File Offset: 0x00014A1C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3, TParam4> : KeyedFactoryBase<TBase, TKey>
    {
        // Token: 0x1700005E RID: 94
        // (get) Token: 0x06000849 RID: 2121 RVA: 0x0001686C File Offset: 0x00014A6C
        protected override IEnumerable<Type> ProvidedTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(TParam1),
                    typeof(TParam2),
                    typeof(TParam3),
                    typeof(TParam4)
                };
            }
        }

        // Token: 0x0600084A RID: 2122 RVA: 0x000168A8 File Offset: 0x00014AA8
        public virtual TBase Create(TKey key, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return (TBase)((object)base.Container.InstantiateExplicit(base.GetTypeForKey(key), new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4)
            }));
        }

        // Token: 0x0600084C RID: 2124 RVA: 0x0001690C File Offset: 0x00014B0C
        private static object __zenCreate(object[] P_0)
        {
            return new KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3, TParam4>();
        }

        // Token: 0x0600084D RID: 2125 RVA: 0x00016924 File Offset: 0x00014B24
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3, TParam4>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(KeyedFactory<TBase, TKey, TParam1, TParam2, TParam3, TParam4>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
