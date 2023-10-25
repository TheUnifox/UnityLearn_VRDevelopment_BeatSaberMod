using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000AE RID: 174
    public interface IHandshakeClientToServerMessage : IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
    }
}
