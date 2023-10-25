using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000BC RID: 188
    public interface IUnconnectedReliableResponse : IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket, IUnconnectedResponse
    {
        // Token: 0x060006B4 RID: 1716
        IUnconnectedReliableResponse WithRequestAndResponseId(uint requestId, uint responseId);
    }
}
