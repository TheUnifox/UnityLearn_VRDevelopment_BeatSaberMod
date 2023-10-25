using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000AF RID: 175
    public interface IHandshakeServerToClientMessage : IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
    }
}
