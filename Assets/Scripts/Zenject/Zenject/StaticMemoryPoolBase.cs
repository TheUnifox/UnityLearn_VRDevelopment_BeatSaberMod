using System;

namespace Zenject
{
    // Token: 0x020001E4 RID: 484
    [NoReflectionBaking]
    public abstract class StaticMemoryPoolBase<TValue> : StaticMemoryPoolBaseBase<TValue> where TValue : class, new()
    {
        // Token: 0x06000A15 RID: 2581 RVA: 0x0001AB54 File Offset: 0x00018D54
        public StaticMemoryPoolBase(Action<TValue> onDespawnedMethod) : base(onDespawnedMethod)
        {
        }

        // Token: 0x06000A16 RID: 2582 RVA: 0x0001AB60 File Offset: 0x00018D60
        protected override TValue Alloc()
        {
            return Activator.CreateInstance<TValue>();
        }
    }
}
