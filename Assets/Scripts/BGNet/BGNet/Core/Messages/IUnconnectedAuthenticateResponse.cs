using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000C0 RID: 192
    public interface IUnconnectedAuthenticateResponse : IUnconnectedReliableResponse, IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket, IUnconnectedResponse
    {
        // Token: 0x17000121 RID: 289
        // (get) Token: 0x060006B6 RID: 1718
        bool isAuthenticated { get; }
    }
}
