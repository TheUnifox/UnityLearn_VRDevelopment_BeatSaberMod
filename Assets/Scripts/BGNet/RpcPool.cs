using System;
using System.Collections.Generic;

// Token: 0x0200007A RID: 122
public static class RpcPool
{
    // Token: 0x06000529 RID: 1321 RVA: 0x0000DDA8 File Offset: 0x0000BFA8
    public static T Obtain<T>() where T : IRemoteProcedureCall, new()
    {
        if (RpcPool._pools == null)
        {
            RpcPool._pools = new Dictionary<Type, IPacketPool>();
        }
        IPacketPool packetPool;
        if (!RpcPool._pools.TryGetValue(typeof(T), out packetPool))
        {
            packetPool = new PacketPool<T>();
            RpcPool._pools[typeof(T)] = packetPool;
        }
        return ((IPacketPool<T>)packetPool).Obtain();
    }

    // Token: 0x0600052A RID: 1322 RVA: 0x0000DE04 File Offset: 0x0000C004
    public static void Fill<T>() where T : IRemoteProcedureCall, new()
    {
        if (RpcPool._pools == null)
        {
            RpcPool._pools = new Dictionary<Type, IPacketPool>();
        }
        IPacketPool packetPool;
        if (!RpcPool._pools.TryGetValue(typeof(T), out packetPool))
        {
            packetPool = new PacketPool<T>();
            RpcPool._pools[typeof(T)] = packetPool;
        }
        packetPool.Fill();
    }

    // Token: 0x0600052B RID: 1323 RVA: 0x0000DE5C File Offset: 0x0000C05C
    public static void Release(IRemoteProcedureCall t)
    {
        if (RpcPool._pools == null)
        {
            return;
        }
        IPacketPool packetPool;
        if (!RpcPool._pools.TryGetValue(t.GetType(), out packetPool))
        {
            return;
        }
        packetPool.Release(t);
    }

    // Token: 0x040001F6 RID: 502
    [ThreadStatic]
    private static Dictionary<Type, IPacketPool> _pools;
}
