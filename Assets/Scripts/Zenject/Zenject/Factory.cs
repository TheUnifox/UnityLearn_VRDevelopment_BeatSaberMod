using System;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000192 RID: 402
    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TValue> : PlaceholderFactory<TValue>
    {
        // Token: 0x0600085E RID: 2142 RVA: 0x00016B3C File Offset: 0x00014D3C
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TValue>();
        }

        // Token: 0x0600085F RID: 2143 RVA: 0x00016B54 File Offset: 0x00014D54
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TValue> : PlaceholderFactory<TParam1, TValue>
    {
        // Token: 0x06000870 RID: 2160 RVA: 0x00016D9C File Offset: 0x00014F9C
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TValue>();
        }

        // Token: 0x06000871 RID: 2161 RVA: 0x00016DB4 File Offset: 0x00014FB4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TValue> : PlaceholderFactory<TParam1, TParam2, TValue>
    {
        // Token: 0x06000882 RID: 2178 RVA: 0x00017030 File Offset: 0x00015230
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TParam2, TValue>();
        }

        // Token: 0x06000883 RID: 2179 RVA: 0x00017048 File Offset: 0x00015248
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TParam2, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TParam2, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TValue> : PlaceholderFactory<TParam1, TParam2, TParam3, TValue>
    {
        // Token: 0x06000894 RID: 2196 RVA: 0x000172F4 File Offset: 0x000154F4
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TParam2, TParam3, TValue>();
        }

        // Token: 0x06000895 RID: 2197 RVA: 0x0001730C File Offset: 0x0001550C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TParam2, TParam3, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TParam2, TParam3, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TValue> : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TValue>
    {
        // Token: 0x060008A6 RID: 2214 RVA: 0x000175E8 File Offset: 0x000157E8
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TParam2, TParam3, TParam4, TValue>();
        }

        // Token: 0x060008A7 RID: 2215 RVA: 0x00017600 File Offset: 0x00015800
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TParam2, TParam3, TParam4, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TParam2, TParam3, TParam4, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
        // Token: 0x060008B8 RID: 2232 RVA: 0x00017918 File Offset: 0x00015B18
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>();
        }

        // Token: 0x060008B9 RID: 2233 RVA: 0x00017930 File Offset: 0x00015B30
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
    {
        // Token: 0x060008CA RID: 2250 RVA: 0x00017C78 File Offset: 0x00015E78
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>();
        }

        // Token: 0x060008CB RID: 2251 RVA: 0x00017C90 File Offset: 0x00015E90
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    [Obsolete("Zenject.Factory has been renamed to PlaceholderFactory.  Zenject.Factory will be removed in future versions")]
    public class Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue> : PlaceholderFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>
    {
        // Token: 0x060008DC RID: 2268 RVA: 0x000180A0 File Offset: 0x000162A0
        private static object __zenCreate(object[] P_0)
        {
            return new Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>();
        }

        // Token: 0x060008DD RID: 2269 RVA: 0x000180B8 File Offset: 0x000162B8
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(Factory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
