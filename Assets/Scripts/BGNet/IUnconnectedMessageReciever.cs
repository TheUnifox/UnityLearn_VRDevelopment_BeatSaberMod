using System;
using System.Net;
using LiteNetLib.Utils;

// Token: 0x02000058 RID: 88
public interface IUnconnectedMessageReceiver : IPollable
{
    // Token: 0x060003FC RID: 1020
    void ReceiveUnconnectedMessage(IPEndPoint endPoint, NetDataReader reader);
}
