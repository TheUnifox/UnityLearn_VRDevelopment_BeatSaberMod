using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000190 RID: 400
    public class PlaceholderFactory<TValue> : PlaceholderFactoryBase<TValue>, IFactory<TValue>, IFactory
    {
        // Token: 0x0600084E RID: 2126 RVA: 0x00016974 File Offset: 0x00014B74
        [NotNull]
        public virtual TValue Create()
        {
            return base.CreateInternal(new List<TypeValuePair>());
        }

        // Token: 0x1700005F RID: 95
        // (get) Token: 0x0600084F RID: 2127 RVA: 0x00016984 File Offset: 0x00014B84
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield break;
            }
        }

        // Token: 0x06000851 RID: 2129 RVA: 0x00016998 File Offset: 0x00014B98
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TValue>();
        }

        // Token: 0x06000852 RID: 2130 RVA: 0x000169B0 File Offset: 0x00014BB0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TValue>, IFactory
    {
        // Token: 0x06000860 RID: 2144 RVA: 0x00016BA4 File Offset: 0x00014DA4
        [NotNull]
        public virtual TValue Create(TParam1 param)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param)
            });
        }

        // Token: 0x17000062 RID: 98
        // (get) Token: 0x06000861 RID: 2145 RVA: 0x00016BC0 File Offset: 0x00014DC0
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield break;
            }
        }

        // Token: 0x06000863 RID: 2147 RVA: 0x00016BD4 File Offset: 0x00014DD4
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TValue>();
        }

        // Token: 0x06000864 RID: 2148 RVA: 0x00016BEC File Offset: 0x00014DEC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TParam2, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TValue>, IFactory
    {
        // Token: 0x06000872 RID: 2162 RVA: 0x00016E04 File Offset: 0x00015004
        [NotNull]
        public virtual TValue Create(TParam1 param1, TParam2 param2)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2)
            });
        }

        // Token: 0x17000065 RID: 101
        // (get) Token: 0x06000873 RID: 2163 RVA: 0x00016E2C File Offset: 0x0001502C
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield break;
            }
        }

        // Token: 0x06000875 RID: 2165 RVA: 0x00016E40 File Offset: 0x00015040
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TParam2, TValue>();
        }

        // Token: 0x06000876 RID: 2166 RVA: 0x00016E58 File Offset: 0x00015058
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TParam2, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TParam2, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TParam2, TParam3, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TValue>, IFactory
    {
        // Token: 0x06000884 RID: 2180 RVA: 0x00017098 File Offset: 0x00015298
        [NotNull]
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3)
            });
        }

        // Token: 0x17000068 RID: 104
        // (get) Token: 0x06000885 RID: 2181 RVA: 0x000170CC File Offset: 0x000152CC
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield break;
            }
        }

        // Token: 0x06000887 RID: 2183 RVA: 0x000170E0 File Offset: 0x000152E0
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TParam2, TParam3, TValue>();
        }

        // Token: 0x06000888 RID: 2184 RVA: 0x000170F8 File Offset: 0x000152F8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TParam2, TParam3, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TParam2, TParam3, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>, IFactory
    {
        // Token: 0x06000896 RID: 2198 RVA: 0x0001735C File Offset: 0x0001555C
        [NotNull]
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4)
            });
        }

        // Token: 0x1700006B RID: 107
        // (get) Token: 0x06000897 RID: 2199 RVA: 0x0001739C File Offset: 0x0001559C
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield break;
            }
        }

        // Token: 0x06000899 RID: 2201 RVA: 0x000173B0 File Offset: 0x000155B0
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>();
        }

        // Token: 0x0600089A RID: 2202 RVA: 0x000173C8 File Offset: 0x000155C8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>, IFactory
    {
        // Token: 0x060008A8 RID: 2216 RVA: 0x00017650 File Offset: 0x00015850
        [NotNull]
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4),
                InjectUtil.CreateTypePair<TParam5>(param5)
            });
        }

        // Token: 0x1700006E RID: 110
        // (get) Token: 0x060008A9 RID: 2217 RVA: 0x000176A8 File Offset: 0x000158A8
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield break;
            }
        }

        // Token: 0x060008AB RID: 2219 RVA: 0x000176BC File Offset: 0x000158BC
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>();
        }

        // Token: 0x060008AC RID: 2220 RVA: 0x000176D4 File Offset: 0x000158D4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>, IFactory
    {
        // Token: 0x060008BA RID: 2234 RVA: 0x00017980 File Offset: 0x00015B80
        [NotNull]
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4),
                InjectUtil.CreateTypePair<TParam5>(param5),
                InjectUtil.CreateTypePair<TParam6>(param6)
            });
        }

        // Token: 0x17000071 RID: 113
        // (get) Token: 0x060008BB RID: 2235 RVA: 0x000179E4 File Offset: 0x00015BE4
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield return typeof(TParam6);
                yield break;
            }
        }

        // Token: 0x060008BD RID: 2237 RVA: 0x000179F8 File Offset: 0x00015BF8
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>();
        }

        // Token: 0x060008BE RID: 2238 RVA: 0x00017A10 File Offset: 0x00015C10
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue> : PlaceholderFactoryBase<TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>, IFactory
    {
        // Token: 0x060008CC RID: 2252 RVA: 0x00017CE0 File Offset: 0x00015EE0
        public virtual TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10)
        {
            return base.CreateInternal(new List<TypeValuePair>
            {
                InjectUtil.CreateTypePair<TParam1>(param1),
                InjectUtil.CreateTypePair<TParam2>(param2),
                InjectUtil.CreateTypePair<TParam3>(param3),
                InjectUtil.CreateTypePair<TParam4>(param4),
                InjectUtil.CreateTypePair<TParam5>(param5),
                InjectUtil.CreateTypePair<TParam6>(param6),
                InjectUtil.CreateTypePair<TParam7>(param7),
                InjectUtil.CreateTypePair<TParam8>(param8),
                InjectUtil.CreateTypePair<TParam9>(param9),
                InjectUtil.CreateTypePair<TParam10>(param10)
            });
        }

        // Token: 0x17000074 RID: 116
        // (get) Token: 0x060008CD RID: 2253 RVA: 0x00017D78 File Offset: 0x00015F78
        protected sealed override IEnumerable<Type> ParamTypes
        {
            get
            {
                yield return typeof(TParam1);
                yield return typeof(TParam2);
                yield return typeof(TParam3);
                yield return typeof(TParam4);
                yield return typeof(TParam5);
                yield return typeof(TParam6);
                yield return typeof(TParam7);
                yield return typeof(TParam8);
                yield return typeof(TParam9);
                yield return typeof(TParam10);
                yield break;
            }
        }

        // Token: 0x060008CF RID: 2255 RVA: 0x00017D8C File Offset: 0x00015F8C
        private static object __zenCreate(object[] P_0)
        {
            return new PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>();
        }

        // Token: 0x060008D0 RID: 2256 RVA: 0x00017DA4 File Offset: 0x00015FA4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
