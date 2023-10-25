using System;

// Token: 0x02000047 RID: 71
public interface IVersionedPacketPool<T> : IPacketPool
{
    // Token: 0x060002E2 RID: 738
    T Obtain(uint version);

    // Token: 0x060002E3 RID: 739
    void Release(T t);
}
