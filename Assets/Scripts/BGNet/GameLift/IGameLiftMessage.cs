using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x02000097 RID: 151
    public interface IGameLiftMessage : IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
    }
}
