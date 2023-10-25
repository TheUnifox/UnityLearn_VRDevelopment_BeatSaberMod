using System;
using BGNet.Core.Messages;
using LiteNetLib.Utils;

namespace GameLift
{
    // Token: 0x0200009C RID: 156
    public class GameLiftMessageReceivedAcknowledge : BaseAcknowledgeMessage, IGameLiftMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x170000FB RID: 251
        // (get) Token: 0x06000606 RID: 1542 RVA: 0x000108F6 File Offset: 0x0000EAF6
        public static PacketPool<GameLiftMessageReceivedAcknowledge> pool
        {
            get
            {
                return ThreadStaticPacketPool<GameLiftMessageReceivedAcknowledge>.pool;
            }
        }

        // Token: 0x06000607 RID: 1543 RVA: 0x000108FD File Offset: 0x0000EAFD
        public override void Release()
        {
            GameLiftMessageReceivedAcknowledge.pool.Release(this);
        }
    }
}
