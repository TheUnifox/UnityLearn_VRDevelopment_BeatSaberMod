using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000BA RID: 186
    public interface IUnconnectedReliableRequest : IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x1700011C RID: 284
        // (get) Token: 0x060006AE RID: 1710
        uint requestId { get; }

        // Token: 0x060006AF RID: 1711
        IUnconnectedReliableRequest WithRequestId(uint requestId);
    }
}
