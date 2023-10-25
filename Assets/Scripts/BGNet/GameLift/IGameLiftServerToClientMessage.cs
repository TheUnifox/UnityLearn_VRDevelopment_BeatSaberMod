using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x02000099 RID: 153
    public interface IGameLiftServerToClientMessage : IGameLiftMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
    }
}
