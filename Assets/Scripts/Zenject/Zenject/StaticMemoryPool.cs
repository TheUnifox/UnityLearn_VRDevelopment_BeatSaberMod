using System;
using System.Buffers;

namespace Zenject
{
    // Token: 0x020001E5 RID: 485
    [NoReflectionBaking]
    public class StaticMemoryPool<TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A17 RID: 2583 RVA: 0x0001AB68 File Offset: 0x00018D68
        public StaticMemoryPool(Action<TValue> onSpawnMethod = null, Action<TValue> onDespawnedMethod = null, int initialSize = 0) : base(onDespawnedMethod)
        {
            this._onSpawnMethod = onSpawnMethod;
            if (initialSize > 0)
            {
                base.Resize(initialSize);
            }
        }

        // Token: 0x17000087 RID: 135
        // (set) Token: 0x06000A18 RID: 2584 RVA: 0x0001AB84 File Offset: 0x00018D84
        public Action<TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A19 RID: 2585 RVA: 0x0001AB90 File Offset: 0x00018D90
        public TValue Spawn()
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(tvalue);
            }
            return tvalue;
        }

        // Token: 0x040002FC RID: 764
        private Action<TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A1A RID: 2586 RVA: 0x0001ABBC File Offset: 0x00018DBC
        public StaticMemoryPool(Action<TParam1, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x17000088 RID: 136
        // (set) Token: 0x06000A1B RID: 2587 RVA: 0x0001ABD4 File Offset: 0x00018DD4
        public Action<TParam1, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A1C RID: 2588 RVA: 0x0001ABE0 File Offset: 0x00018DE0
        public TValue Spawn(TParam1 param)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(param, tvalue);
            }
            return tvalue;
        }

        // Token: 0x040002FD RID: 765
        private Action<TParam1, TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TParam2, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A1D RID: 2589 RVA: 0x0001AC0C File Offset: 0x00018E0C
        public StaticMemoryPool(Action<TParam1, TParam2, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x17000089 RID: 137
        // (set) Token: 0x06000A1E RID: 2590 RVA: 0x0001AC24 File Offset: 0x00018E24
        public Action<TParam1, TParam2, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A1F RID: 2591 RVA: 0x0001AC30 File Offset: 0x00018E30
        public TValue Spawn(TParam1 p1, TParam2 p2)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(p1, p2, tvalue);
            }
            return tvalue;
        }

        // Token: 0x040002FE RID: 766
        private Action<TParam1, TParam2, TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A20 RID: 2592 RVA: 0x0001AC5C File Offset: 0x00018E5C
        public StaticMemoryPool(Action<TParam1, TParam2, TParam3, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x1700008A RID: 138
        // (set) Token: 0x06000A21 RID: 2593 RVA: 0x0001AC74 File Offset: 0x00018E74
        public Action<TParam1, TParam2, TParam3, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A22 RID: 2594 RVA: 0x0001AC80 File Offset: 0x00018E80
        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(p1, p2, p3, tvalue);
            }
            return tvalue;
        }

        // Token: 0x040002FF RID: 767
        private Action<TParam1, TParam2, TParam3, TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A23 RID: 2595 RVA: 0x0001ACAC File Offset: 0x00018EAC
        public StaticMemoryPool(Action<TParam1, TParam2, TParam3, TParam4, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x1700008B RID: 139
        // (set) Token: 0x06000A24 RID: 2596 RVA: 0x0001ACC4 File Offset: 0x00018EC4
        public Action<TParam1, TParam2, TParam3, TParam4, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A25 RID: 2597 RVA: 0x0001ACD0 File Offset: 0x00018ED0
        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(p1, p2, p3, p4, tvalue);
            }
            return tvalue;
        }

        // Token: 0x04000300 RID: 768
        private Action<TParam1, TParam2, TParam3, TParam4, TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A26 RID: 2598 RVA: 0x0001AD00 File Offset: 0x00018F00
        public StaticMemoryPool(Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x1700008C RID: 140
        // (set) Token: 0x06000A27 RID: 2599 RVA: 0x0001AD18 File Offset: 0x00018F18
        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A28 RID: 2600 RVA: 0x0001AD24 File Offset: 0x00018F24
        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(p1, p2, p3, p4, p5, tvalue);
            }
            return tvalue;
        }

        // Token: 0x04000301 RID: 769
        private Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A29 RID: 2601 RVA: 0x0001AD54 File Offset: 0x00018F54
        public StaticMemoryPool(Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x1700008D RID: 141
        // (set) Token: 0x06000A2A RID: 2602 RVA: 0x0001AD6C File Offset: 0x00018F6C
        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A2B RID: 2603 RVA: 0x0001AD78 File Offset: 0x00018F78
        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(p1, p2, p3, p4, p5, p6, tvalue);
            }
            return tvalue;
        }

        // Token: 0x04000302 RID: 770
        private Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> _onSpawnMethod;
    }

    [NoReflectionBaking]
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>, IDespawnableMemoryPool<TValue>, IMemoryPool where TValue : class, new()
    {
        // Token: 0x06000A2C RID: 2604 RVA: 0x0001ADAC File Offset: 0x00018FAC
        public StaticMemoryPool(Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null) : base(onDespawnedMethod)
        {
            ModestTree.Assert.IsNotNull(onSpawnMethod);
            this._onSpawnMethod = onSpawnMethod;
        }

        // Token: 0x1700008E RID: 142
        // (set) Token: 0x06000A2D RID: 2605 RVA: 0x0001ADC4 File Offset: 0x00018FC4
        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> OnSpawnMethod
        {
            set
            {
                this._onSpawnMethod = value;
            }
        }

        // Token: 0x06000A2E RID: 2606 RVA: 0x0001ADD0 File Offset: 0x00018FD0
        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
            TValue tvalue = base.SpawnInternal();
            if (this._onSpawnMethod != null)
            {
                this._onSpawnMethod(p1, p2, p3, p4, p5, p6, p7, tvalue);
            }
            return tvalue;
        }

        // Token: 0x04000303 RID: 771
        private Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> _onSpawnMethod;
    }
}