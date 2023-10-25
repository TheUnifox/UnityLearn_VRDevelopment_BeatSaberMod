using System;

namespace Zenject
{
    // Token: 0x02000012 RID: 18
    public interface IPoolable
    {
        // Token: 0x06000024 RID: 36
        void OnDespawned();

        // Token: 0x06000025 RID: 37
        void OnSpawned();
    }

    public interface IPoolable<TParam1>
    {
        // Token: 0x06000026 RID: 38
        void OnDespawned();

        // Token: 0x06000027 RID: 39
        void OnSpawned(TParam1 p1);
    }

    public interface IPoolable<TParam1, TParam2>
    {
        // Token: 0x06000028 RID: 40
        void OnDespawned();

        // Token: 0x06000029 RID: 41
        void OnSpawned(TParam1 p1, TParam2 p2);
    }

    public interface IPoolable<TParam1, TParam2, TParam3>
    {
        // Token: 0x0600002A RID: 42
        void OnDespawned();

        // Token: 0x0600002B RID: 43
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4>
    {
        // Token: 0x0600002C RID: 44
        void OnDespawned();

        // Token: 0x0600002D RID: 45
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>
    {
        // Token: 0x0600002E RID: 46
        void OnDespawned();

        // Token: 0x0600002F RID: 47
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>
    {
        // Token: 0x06000030 RID: 48
        void OnDespawned();

        // Token: 0x06000031 RID: 49
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>
    {
        // Token: 0x06000032 RID: 50
        void OnDespawned();

        // Token: 0x06000033 RID: 51
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>
    {
        // Token: 0x06000034 RID: 52
        void OnDespawned();

        // Token: 0x06000035 RID: 53
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9>
    {
        // Token: 0x06000036 RID: 54
        void OnDespawned();

        // Token: 0x06000037 RID: 55
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TParam9 p9);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>
    {
        // Token: 0x06000038 RID: 56
        void OnDespawned();

        // Token: 0x06000039 RID: 57
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TParam9 p9, TParam10 p10);
    }

    public interface IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11>
    {
        // Token: 0x0600003A RID: 58
        void OnDespawned();

        // Token: 0x0600003B RID: 59
        void OnSpawned(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TParam9 p9, TParam10 p10, TParam11 p11);
    }
}
