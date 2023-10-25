using System;
using System.Buffers;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001B5 RID: 437
    public class MemoryPool<TValue> : MemoryPoolBase<TValue>, IMemoryPool<TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TValue>, IFactory
    {
        // Token: 0x060008F8 RID: 2296 RVA: 0x000182CC File Offset: 0x000164CC
        public TValue Spawn()
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(@internal);
            }
            return @internal;
        }

        // Token: 0x060008F9 RID: 2297 RVA: 0x000182F8 File Offset: 0x000164F8
        protected virtual void Reinitialize(TValue item)
        {
        }

        // Token: 0x060008FA RID: 2298 RVA: 0x000182FC File Offset: 0x000164FC
        TValue IFactory<TValue>.Create()

        {
            return this.Spawn();
        }

        // Token: 0x060008FC RID: 2300 RVA: 0x0001830C File Offset: 0x0001650C
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TValue>();
        }

        // Token: 0x060008FD RID: 2301 RVA: 0x00018324 File Offset: 0x00016524
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TValue>, IFactory
    {
        // Token: 0x060008FE RID: 2302 RVA: 0x00018374 File Offset: 0x00016574
        public TValue Spawn(TParam1 param)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param, @internal);
            }
            return @internal;
        }

        // Token: 0x060008FF RID: 2303 RVA: 0x000183A0 File Offset: 0x000165A0
        protected virtual void Reinitialize(TParam1 p1, TValue item)
        {
        }

        // Token: 0x06000900 RID: 2304 RVA: 0x000183A4 File Offset: 0x000165A4
        TValue IFactory<TParam1, TValue>.Create(TParam1 p1)

        {
            return this.Spawn(p1);
        }

        // Token: 0x06000902 RID: 2306 RVA: 0x000183B8 File Offset: 0x000165B8
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TValue>();
        }

        // Token: 0x06000903 RID: 2307 RVA: 0x000183D0 File Offset: 0x000165D0
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TValue>, IFactory
    {
        // Token: 0x06000904 RID: 2308 RVA: 0x00018420 File Offset: 0x00016620
        public TValue Spawn(TParam1 param1, TParam2 param2)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, @internal);
            }
            return @internal;
        }

        // Token: 0x06000905 RID: 2309 RVA: 0x0001844C File Offset: 0x0001664C
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TValue item)
        {
        }

        // Token: 0x06000906 RID: 2310 RVA: 0x00018450 File Offset: 0x00016650
        TValue IFactory<TParam1, TParam2, TValue>.Create(TParam1 p1, TParam2 p2)

        {
            return this.Spawn(p1, p2);
        }

        // Token: 0x06000908 RID: 2312 RVA: 0x00018464 File Offset: 0x00016664
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TValue>();
        }

        // Token: 0x06000909 RID: 2313 RVA: 0x0001847C File Offset: 0x0001667C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TParam3, TValue>, IFactory
    {
        // Token: 0x0600090A RID: 2314 RVA: 0x000184CC File Offset: 0x000166CC
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, param3, @internal);
            }
            return @internal;
        }

        // Token: 0x0600090B RID: 2315 RVA: 0x000184F8 File Offset: 0x000166F8
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TValue item)
        {
        }

        // Token: 0x0600090C RID: 2316 RVA: 0x000184FC File Offset: 0x000166FC
        TValue IFactory<TParam1, TParam2, TParam3, TValue>.Create(TParam1 p1, TParam2 p2, TParam3 p3)

        {
            return this.Spawn(p1, p2, p3);
        }

        // Token: 0x0600090E RID: 2318 RVA: 0x00018510 File Offset: 0x00016710
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TParam3, TValue>();
        }

        // Token: 0x0600090F RID: 2319 RVA: 0x00018528 File Offset: 0x00016728
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TParam3, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TParam3, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TParam3, TParam4, TValue>, IFactory
    {
        // Token: 0x06000910 RID: 2320 RVA: 0x00018578 File Offset: 0x00016778
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, param3, param4, @internal);
            }
            return @internal;
        }

        // Token: 0x06000911 RID: 2321 RVA: 0x000185A8 File Offset: 0x000167A8
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TValue item)
        {
        }

        // Token: 0x06000912 RID: 2322 RVA: 0x000185AC File Offset: 0x000167AC
        TValue IFactory<TParam1, TParam2, TParam3, TParam4, TValue>.Create(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)

        {
            return this.Spawn(p1, p2, p3, p4);
        }

        // Token: 0x06000914 RID: 2324 RVA: 0x000185C4 File Offset: 0x000167C4
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>();
        }

        // Token: 0x06000915 RID: 2325 RVA: 0x000185DC File Offset: 0x000167DC
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>, IFactory
    {
        // Token: 0x06000916 RID: 2326 RVA: 0x0001862C File Offset: 0x0001682C
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, param3, param4, param5, @internal);
            }
            return @internal;
        }

        // Token: 0x06000917 RID: 2327 RVA: 0x0001865C File Offset: 0x0001685C
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TValue item)
        {
        }

        // Token: 0x06000918 RID: 2328 RVA: 0x00018660 File Offset: 0x00016860
        TValue IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.Create(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)

        {
            return this.Spawn(p1, p2, p3, p4, p5);
        }

        // Token: 0x0600091A RID: 2330 RVA: 0x00018678 File Offset: 0x00016878
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>();
        }

        // Token: 0x0600091B RID: 2331 RVA: 0x00018690 File Offset: 0x00016890
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>, IFactory
    {
        // Token: 0x0600091C RID: 2332 RVA: 0x000186E0 File Offset: 0x000168E0
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, param3, param4, param5, param6, @internal);
            }
            return @internal;
        }

        // Token: 0x0600091D RID: 2333 RVA: 0x00018714 File Offset: 0x00016914
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TValue item)
        {
        }

        // Token: 0x0600091E RID: 2334 RVA: 0x00018718 File Offset: 0x00016918
        TValue IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.Create(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)

        {
            return this.Spawn(p1, p2, p3, p4, p5, p6);
        }

        // Token: 0x06000920 RID: 2336 RVA: 0x00018734 File Offset: 0x00016934
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>();
        }

        // Token: 0x06000921 RID: 2337 RVA: 0x0001874C File Offset: 0x0001694C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>, IFactory
    {
        // Token: 0x06000922 RID: 2338 RVA: 0x0001879C File Offset: 0x0001699C
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, param3, param4, param5, param6, param7, @internal);
            }
            return @internal;
        }

        // Token: 0x06000923 RID: 2339 RVA: 0x000187D0 File Offset: 0x000169D0
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TValue item)
        {
        }

        // Token: 0x06000924 RID: 2340 RVA: 0x000187D4 File Offset: 0x000169D4
        TValue IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>.Create(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)

        {
            return this.Spawn(p1, p2, p3, p4, p5, p6, p7);
        }

        // Token: 0x06000926 RID: 2342 RVA: 0x000187F0 File Offset: 0x000169F0
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>();
        }

        // Token: 0x06000927 RID: 2343 RVA: 0x00018808 File Offset: 0x00016A08
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }

    public class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue> : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>, IFactory
    {
        // Token: 0x06000928 RID: 2344 RVA: 0x00018858 File Offset: 0x00016A58
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
        {
            TValue @internal = base.GetInternal();
            if (!base.Container.IsValidating)
            {
                this.Reinitialize(param1, param2, param3, param4, param5, param6, param7, param8, @internal);
            }
            return @internal;
        }

        // Token: 0x06000929 RID: 2345 RVA: 0x00018890 File Offset: 0x00016A90
        protected virtual void Reinitialize(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8, TValue item)
        {
        }

        // Token: 0x0600092A RID: 2346 RVA: 0x00018894 File Offset: 0x00016A94
        TValue IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>.Create(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7, TParam8 p8)

        {
            return this.Spawn(p1, p2, p3, p4, p5, p6, p7, p8);
        }

        // Token: 0x0600092C RID: 2348 RVA: 0x000188BC File Offset: 0x00016ABC
        private static object __zenCreate(object[] P_0)
        {
            return new MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>();
        }

        // Token: 0x0600092D RID: 2349 RVA: 0x000188D4 File Offset: 0x00016AD4
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TValue>.__zenCreate), new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }
    }
}
