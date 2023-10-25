using System;

// Token: 0x02000079 RID: 121
public class RpcHandler<TType> where TType : struct, IConvertible
{
    // Token: 0x06000517 RID: 1303 RVA: 0x0000DA68 File Offset: 0x0000BC68
    public RpcHandler(IMultiplayerSessionManager multiplayerSessionManager, MultiplayerSessionManager.MessageType messageType)
    {
        this._multiplayerSessionManager = multiplayerSessionManager;
        this._messageType = messageType;
        this._multiplayerSessionManager.RegisterSerializer(messageType, this._rpcSerializer);
    }

    // Token: 0x06000518 RID: 1304 RVA: 0x0000DA9B File Offset: 0x0000BC9B
    public void Destroy()
    {
        this._multiplayerSessionManager.UnregisterSerializer(this._messageType, this._rpcSerializer);
    }

    // Token: 0x06000519 RID: 1305 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
    public void EnqueueRpc<T>() where T : RemoteProcedureCall, new()
    {
        this._multiplayerSessionManager.Send<IRemoteProcedureCall>(RpcPool.Obtain<T>().Init(this._multiplayerSessionManager.syncTime));
    }

    // Token: 0x0600051A RID: 1306 RVA: 0x0000DADB File Offset: 0x0000BCDB
    public void EnqueueRpc<T, T0>(T0 value0) where T : RemoteProcedureCall<T0>, new()
    {
        this._multiplayerSessionManager.Send<IRemoteProcedureCall>(RpcPool.Obtain<T>().Init(this._multiplayerSessionManager.syncTime, value0));
    }

    // Token: 0x0600051B RID: 1307 RVA: 0x0000DB03 File Offset: 0x0000BD03
    public void EnqueueRpc<T, T0, T1>(T0 value0, T1 value1) where T : RemoteProcedureCall<T0, T1>, new()
    {
        this._multiplayerSessionManager.Send<IRemoteProcedureCall>(RpcPool.Obtain<T>().Init(this._multiplayerSessionManager.syncTime, value0, value1));
    }

    // Token: 0x0600051C RID: 1308 RVA: 0x0000DB2C File Offset: 0x0000BD2C
    public void EnqueueRpc<T, T0, T1, T2>(T0 value0, T1 value1, T2 value2) where T : RemoteProcedureCall<T0, T1, T2>, new()
    {
        this._multiplayerSessionManager.Send<IRemoteProcedureCall>(RpcPool.Obtain<T>().Init(this._multiplayerSessionManager.syncTime, value0, value1, value2));
    }

    // Token: 0x0600051D RID: 1309 RVA: 0x0000DB56 File Offset: 0x0000BD56
    public void EnqueueRpc<T, T0, T1, T2, T3>(T0 value0, T1 value1, T2 value2, T3 value3) where T : RemoteProcedureCall<T0, T1, T2, T3>, new()
    {
        this._multiplayerSessionManager.Send<IRemoteProcedureCall>(RpcPool.Obtain<T>().Init(this._multiplayerSessionManager.syncTime, value0, value1, value2, value3));
    }

    // Token: 0x0600051E RID: 1310 RVA: 0x0000DB84 File Offset: 0x0000BD84
    public void RegisterCallback<T>(TType type, Action<string> callback) where T : RemoteProcedureCall, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId);
        });
    }

    // Token: 0x0600051F RID: 1311 RVA: 0x0000DBB4 File Offset: 0x0000BDB4
    public void RegisterCallback<T, T0>(TType type, Action<string, T0> callback) where T : RemoteProcedureCall<T0>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, T0> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.value0);
        });
    }

    // Token: 0x06000520 RID: 1312 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
    public void RegisterCallback<T, T0, T1>(TType type, Action<string, T0, T1> callback) where T : RemoteProcedureCall<T0, T1>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, T0, T1> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.value0, rpc.value1);
        });
    }

    // Token: 0x06000521 RID: 1313 RVA: 0x0000DC14 File Offset: 0x0000BE14
    public void RegisterCallback<T, T0, T1, T2>(TType type, Action<string, T0, T1, T2> callback) where T : RemoteProcedureCall<T0, T1, T2>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, T0, T1, T2> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.value0, rpc.value1, rpc.value2);
        });
    }

    // Token: 0x06000522 RID: 1314 RVA: 0x0000DC44 File Offset: 0x0000BE44
    public void RegisterCallback<T, T0, T1, T2, T3>(TType type, Action<string, T0, T1, T2, T3> callback) where T : RemoteProcedureCall<T0, T1, T2, T3>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, T0, T1, T2, T3> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.value0, rpc.value1, rpc.value2, rpc.value3);
        });
    }

    // Token: 0x06000523 RID: 1315 RVA: 0x0000DC74 File Offset: 0x0000BE74
    public void RegisterCallbackWithTime<T>(TType type, Action<string, float> callback) where T : RemoteProcedureCall, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, float> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.syncTime);
        });
    }

    // Token: 0x06000524 RID: 1316 RVA: 0x0000DCA4 File Offset: 0x0000BEA4
    public void RegisterCallbackWithTime<T, T0>(TType type, Action<string, float, T0> callback) where T : RemoteProcedureCall<T0>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, float, T0> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.syncTime, rpc.value0);
        });
    }

    // Token: 0x06000525 RID: 1317 RVA: 0x0000DCD4 File Offset: 0x0000BED4
    public void RegisterCallbackWithTime<T, T0, T1>(TType type, Action<string, float, T0, T1> callback) where T : RemoteProcedureCall<T0, T1>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, float, T0, T1> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.syncTime, rpc.value0, rpc.value1);
        });
    }

    // Token: 0x06000526 RID: 1318 RVA: 0x0000DD04 File Offset: 0x0000BF04
    public void RegisterCallbackWithTime<T, T0, T1, T2>(TType type, Action<string, float, T0, T1, T2> callback) where T : RemoteProcedureCall<T0, T1, T2>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, float, T0, T1, T2> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.syncTime, rpc.value0, rpc.value1, rpc.value2);
        });
    }

    // Token: 0x06000527 RID: 1319 RVA: 0x0000DD34 File Offset: 0x0000BF34
    public void RegisterCallbackWithTime<T, T0, T1, T2, T3>(TType type, Action<string, float, T0, T1, T2, T3> callback) where T : RemoteProcedureCall<T0, T1, T2, T3>, new()
    {
        this.RegisterCallback<T>(type, delegate (IConnectedPlayer player, T rpc)
        {
            Action<string, float, T0, T1, T2, T3> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(player.userId, rpc.syncTime, rpc.value0, rpc.value1, rpc.value2, rpc.value3);
        });
    }

    // Token: 0x06000528 RID: 1320 RVA: 0x0000DD64 File Offset: 0x0000BF64
    private void RegisterCallback<T>(TType type, Action<IConnectedPlayer, T> callback) where T : IRemoteProcedureCall, new()
    {
        RpcPool.Fill<T>();
        this._rpcSerializer.RegisterCallback<T>(type, delegate (T rpc, IConnectedPlayer player)
        {
            if (player.isConnected)
            {
                callback(player, rpc);
            }
            rpc.Release();
        }, new Func<T>(RpcPool.Obtain<T>));
    }

    // Token: 0x040001F3 RID: 499
    private readonly IMultiplayerSessionManager _multiplayerSessionManager;

    // Token: 0x040001F4 RID: 500
    private readonly MultiplayerSessionManager.MessageType _messageType;

    // Token: 0x040001F5 RID: 501
    private readonly NetworkPacketSerializer<TType, IConnectedPlayer> _rpcSerializer = new NetworkPacketSerializer<TType, IConnectedPlayer>();
}
