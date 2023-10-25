using System;
using System.Net;
using LiteNetLib.Utils;

// Token: 0x02000059 RID: 89
public interface IUnconnectedMessageSender
{
    // Token: 0x170000AB RID: 171
    // (get) Token: 0x060003FD RID: 1021
    byte[] unconnectedPacketHeader { get; }

    // Token: 0x170000AC RID: 172
    // (get) Token: 0x060003FE RID: 1022
    PacketEncryptionLayer encryptionLayer { get; }

    // Token: 0x060003FF RID: 1023
    void SendUnconnectedMessage(IPEndPoint remoteEndPoint, NetDataWriter writer);

    // Token: 0x06000400 RID: 1024
    void RegisterReceiver(IUnconnectedMessageReceiver receiver);

    // Token: 0x06000401 RID: 1025
    void UnregisterReceiver(IUnconnectedMessageReceiver receiver);
}
