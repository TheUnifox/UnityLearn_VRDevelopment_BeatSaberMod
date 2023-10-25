using System;

namespace Zenject
{
    public interface IFactory
    {
    }

    // Token: 0x0200017E RID: 382
    public interface IFactory<out TValue> : IFactory
    {
        // Token: 0x06000815 RID: 2069
        TValue Create();
    }

    public interface IFactory<in TParam1, out TValue> : IFactory
    {
        // Token: 0x06000816 RID: 2070
        TValue Create(TParam1 param);
    }

    public interface IFactory<in TParam1, in TParam2, out TValue> : IFactory
    {
        // Token: 0x06000817 RID: 2071
        TValue Create(TParam1 param1, TParam2 param2);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, out TValue> : IFactory
    {
        // Token: 0x06000818 RID: 2072
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, out TValue> : IFactory
    {
        // Token: 0x06000819 RID: 2073
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, out TValue> : IFactory
    {
        // Token: 0x0600081A RID: 2074
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, out TValue> : IFactory
    {
        // Token: 0x0600081B RID: 2075
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, out TValue> : IFactory
    {
        // Token: 0x0600081C RID: 2076
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, out TValue> : IFactory
    {
        // Token: 0x0600081D RID: 2077
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, in TParam9, out TValue> : IFactory
    {
        // Token: 0x0600081E RID: 2078
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, in TParam9, in TParam10, out TValue> : IFactory
    {
        // Token: 0x0600081F RID: 2079
        TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10);
    }
}
