using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000C1 RID: 193
    public interface IUnconnectedMultipartMessage : IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000122 RID: 290
        // (get) Token: 0x060006B7 RID: 1719
        uint multipartMessageId { get; }

        // Token: 0x17000123 RID: 291
        // (get) Token: 0x060006B8 RID: 1720
        int offset { get; }

        // Token: 0x17000124 RID: 292
        // (get) Token: 0x060006B9 RID: 1721
        int length { get; }

        // Token: 0x17000125 RID: 293
        // (get) Token: 0x060006BA RID: 1722
        int totalLength { get; }

        // Token: 0x17000126 RID: 294
        // (get) Token: 0x060006BB RID: 1723
        byte[] data { get; }
    }
}
