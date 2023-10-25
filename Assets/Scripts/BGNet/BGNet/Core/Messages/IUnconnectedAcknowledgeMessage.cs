using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000BE RID: 190
    public interface IUnconnectedAcknowledgeMessage : IUnconnectedResponse, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000120 RID: 288
        // (get) Token: 0x060006B5 RID: 1717
        bool messageHandled { get; }
    }
}
