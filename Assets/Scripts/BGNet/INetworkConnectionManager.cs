using System;
using System.Net;

// Token: 0x0200003F RID: 63
public interface INetworkConnectionManager : IConnectionManager, IPollable, IDisposable, IUnconnectedConnectionManager, IUnconnectedMessageSender
{
    // Token: 0x14000069 RID: 105
    // (add) Token: 0x060002A3 RID: 675
    // (remove) Token: 0x060002A4 RID: 676
    event NetworkStatisticsState.NetworkStatisticsUpdateDelegate onStatisticsUpdatedEvent;

    // Token: 0x17000078 RID: 120
    // (get) Token: 0x060002A5 RID: 677
    int port { get; }

    // Token: 0x17000079 RID: 121
    // (get) Token: 0x060002A6 RID: 678
    bool isClient { get; }

    // Token: 0x1700007A RID: 122
    // (get) Token: 0x060002A7 RID: 679
    bool isServer { get; }

    // Token: 0x060002A8 RID: 680
    void ConnectToEndPoint(string userId, string userName, IPEndPoint remoteEndPoint, string remoteUserId, string remoteUserName, bool remoteUserIsConnectionOwner);
}
