using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000AD RID: 173
    public interface IHandshakeMessage : IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
    }
}
