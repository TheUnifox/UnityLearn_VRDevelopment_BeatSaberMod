using System;

namespace Zenject
{
    // Token: 0x020001AB RID: 427
    public interface IDespawnableMemoryPool<TValue> : IMemoryPool
    {
        // Token: 0x060008EE RID: 2286
        void Despawn(TValue item);
    }
}
