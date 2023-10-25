using System;
using System.Threading.Tasks;
using BGNet.Core;
using LiteNetLib.Utils;

// Token: 0x02000035 RID: 53
public interface IConnectionManager : IPollable, IDisposable
{
    // Token: 0x14000027 RID: 39
    // (add) Token: 0x060001AE RID: 430
    // (remove) Token: 0x060001AF RID: 431
    event Action onInitializedEvent;

    // Token: 0x14000028 RID: 40
    // (add) Token: 0x060001B0 RID: 432
    // (remove) Token: 0x060001B1 RID: 433
    event Action onConnectedEvent;

    // Token: 0x14000029 RID: 41
    // (add) Token: 0x060001B2 RID: 434
    // (remove) Token: 0x060001B3 RID: 435
    event Action<DisconnectedReason> onDisconnectedEvent;

    // Token: 0x1400002A RID: 42
    // (add) Token: 0x060001B4 RID: 436
    // (remove) Token: 0x060001B5 RID: 437
    event Action<ConnectionFailedReason> onConnectionFailedEvent;

    // Token: 0x1400002B RID: 43
    // (add) Token: 0x060001B6 RID: 438
    // (remove) Token: 0x060001B7 RID: 439
    event Action<IConnection> onConnectionConnectedEvent;

    // Token: 0x1400002C RID: 44
    // (add) Token: 0x060001B8 RID: 440
    // (remove) Token: 0x060001B9 RID: 441
    event Action<IConnection, DisconnectedReason> onConnectionDisconnectedEvent;

    // Token: 0x1400002D RID: 45
    // (add) Token: 0x060001BA RID: 442
    // (remove) Token: 0x060001BB RID: 443
    event Action<IConnection, NetDataReader, DeliveryMethod> onReceivedDataEvent;

    // Token: 0x17000053 RID: 83
    // (get) Token: 0x060001BC RID: 444
    string userId { get; }

    // Token: 0x17000054 RID: 84
    // (get) Token: 0x060001BD RID: 445
    string userName { get; }

    // Token: 0x17000055 RID: 85
    // (get) Token: 0x060001BE RID: 446
    bool isConnected { get; }

    // Token: 0x17000056 RID: 86
    // (get) Token: 0x060001BF RID: 447
    bool isConnecting { get; }

    // Token: 0x17000057 RID: 87
    // (get) Token: 0x060001C0 RID: 448
    bool isDisconnecting { get; }

    // Token: 0x17000058 RID: 88
    // (get) Token: 0x060001C1 RID: 449
    int connectionCount { get; }

    // Token: 0x17000059 RID: 89
    // (get) Token: 0x060001C2 RID: 450
    bool isConnectionOwner { get; }

    // Token: 0x1700005A RID: 90
    // (get) Token: 0x060001C3 RID: 451
    bool isDisposed { get; }

    // Token: 0x060001C4 RID: 452
    void SendToAll(NetDataWriter writer, DeliveryMethod deliveryMethod);

    // Token: 0x060001C5 RID: 453
    void SendToAll(NetDataWriter writer, DeliveryMethod deliveryMethod, IConnection excludingConnection);

    // Token: 0x060001C6 RID: 454
    bool Init<T>(IConnectionInitParams<T> initParams) where T : IConnectionManager;

    // Token: 0x060001C7 RID: 455
    void Disconnect(DisconnectedReason disconnectedReason = DisconnectedReason.UserInitiated);

    // Token: 0x060001C8 RID: 456
    IConnection GetConnection(int index);

    // Token: 0x060001C9 RID: 457
    Task DisposeAsync();
}
