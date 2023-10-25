using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x02000098 RID: 152
    public interface IGameLiftClientToServerMessage : IGameLiftMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x170000F2 RID: 242
        // (get) Token: 0x060005F1 RID: 1521
        string userId { get; }

        // Token: 0x170000F3 RID: 243
        // (get) Token: 0x060005F2 RID: 1522
        string userName { get; }
    }
}
