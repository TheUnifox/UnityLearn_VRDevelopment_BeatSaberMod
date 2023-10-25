using System;

// Token: 0x0200004A RID: 74
public static class ThreadStaticPacketPool<T> where T : IPoolablePacket, new()
{
    // Token: 0x17000095 RID: 149
    // (get) Token: 0x060002F1 RID: 753 RVA: 0x00006F0D File Offset: 0x0000510D
    public static PacketPool<T> pool
    {
        get
        {
            if (ThreadStaticPacketPool<T>._pool == null)
            {
                ThreadStaticPacketPool<T>._pool = new PacketPool<T>();
            }
            return ThreadStaticPacketPool<T>._pool;
        }
    }

    // Token: 0x04000106 RID: 262
    [ThreadStatic]
    private static PacketPool<T> _pool;
}
