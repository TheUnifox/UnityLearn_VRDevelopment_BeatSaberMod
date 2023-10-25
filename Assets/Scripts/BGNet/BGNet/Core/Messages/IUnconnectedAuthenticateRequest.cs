using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000BF RID: 191
    public interface IUnconnectedAuthenticateRequest : IUnconnectedReliableResponse, IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket, IUnconnectedResponse
    {
    }
}