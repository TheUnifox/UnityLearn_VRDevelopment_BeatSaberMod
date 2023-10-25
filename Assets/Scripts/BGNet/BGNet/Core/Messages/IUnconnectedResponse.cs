using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000BB RID: 187
    public interface IUnconnectedResponse : IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x1700011D RID: 285
        // (get) Token: 0x060006B0 RID: 1712
        uint responseId { get; }

        // Token: 0x1700011E RID: 286
        // (get) Token: 0x060006B1 RID: 1713
        byte resultCode { get; }

        // Token: 0x1700011F RID: 287
        // (get) Token: 0x060006B2 RID: 1714
        string resultCodeString { get; }

        // Token: 0x060006B3 RID: 1715
        IUnconnectedResponse WithResponseId(uint responseId);
    }
}
