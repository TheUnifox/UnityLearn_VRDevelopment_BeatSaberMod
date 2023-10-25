using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000BD RID: 189
    public interface IUnconnectedUnreliableMessage : IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
    }
}
