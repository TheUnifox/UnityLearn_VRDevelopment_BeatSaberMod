using System;

// Token: 0x0200004B RID: 75
public static class VersionedThreadStaticPacketPool<T, T2> where T : IPoolablePacket, new() where T2 : T, new()
{
    // Token: 0x17000096 RID: 150
    // (get) Token: 0x060002F2 RID: 754 RVA: 0x00006F25 File Offset: 0x00005125
    public static VersionedPacketPool<T, T2> pool
    {
        get
        {
            if (VersionedThreadStaticPacketPool<T, T2>._pool == null)
            {
                VersionedThreadStaticPacketPool<T, T2>._pool = new VersionedPacketPool<T, T2>(8U);
            }
            return VersionedThreadStaticPacketPool<T, T2>._pool;
        }
    }

    // Token: 0x04000107 RID: 263
    [ThreadStatic]
    private static VersionedPacketPool<T, T2> _pool;
}
