using System;

public interface IPacketPool
{
    // Token: 0x060002DD RID: 733
    void Release(object t);

    // Token: 0x060002DE RID: 734
    void Fill();

    // Token: 0x060002DF RID: 735
    void Clear();
}

// Token: 0x02000046 RID: 70
public interface IPacketPool<T> : IPacketPool
{
    // Token: 0x060002E0 RID: 736
    T Obtain();

    // Token: 0x060002E1 RID: 737
    void Release(T t);
}
