using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x0200009D RID: 157
    public class GameLiftMultipartMessage : BaseMultipartMessage, IGameLiftMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x170000FC RID: 252
        // (get) Token: 0x06000609 RID: 1545 RVA: 0x00010912 File Offset: 0x0000EB12
        public static PacketPool<GameLiftMultipartMessage> pool
        {
            get
            {
                return ThreadStaticPacketPool<GameLiftMultipartMessage>.pool;
            }
        }

        // Token: 0x0600060A RID: 1546 RVA: 0x00010919 File Offset: 0x0000EB19
        public override void Release()
        {
            GameLiftMultipartMessage.pool.Release(this);
        }
    }
}
