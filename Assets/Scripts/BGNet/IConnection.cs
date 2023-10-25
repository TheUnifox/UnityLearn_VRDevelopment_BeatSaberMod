using System;
using BGNet.Core;
using LiteNetLib.Utils;

// Token: 0x02000036 RID: 54
public interface IConnection
{
    // Token: 0x1700005B RID: 91
    // (get) Token: 0x060001CA RID: 458
    string userId { get; }

    // Token: 0x1700005C RID: 92
    // (get) Token: 0x060001CB RID: 459
    string userName { get; }

    // Token: 0x1700005D RID: 93
    // (get) Token: 0x060001CC RID: 460
    bool isConnectionOwner { get; }

    // Token: 0x060001CD RID: 461
    void Send(NetDataWriter writer, DeliveryMethod deliveryMethod);

    // Token: 0x060001CE RID: 462
    void Disconnect();
}
