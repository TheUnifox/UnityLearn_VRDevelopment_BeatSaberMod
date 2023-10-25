using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B7 RID: 183
    public class HandshakeMessageReceivedAcknowledge : BaseAcknowledgeMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x1700011A RID: 282
        // (get) Token: 0x060006A8 RID: 1704 RVA: 0x00012028 File Offset: 0x00010228
        public static PacketPool<HandshakeMessageReceivedAcknowledge> pool
        {
            get
            {
                return ThreadStaticPacketPool<HandshakeMessageReceivedAcknowledge>.pool;
            }
        }

        // Token: 0x060006A9 RID: 1705 RVA: 0x0001202F File Offset: 0x0001022F
        public override void Release()
        {
            HandshakeMessageReceivedAcknowledge.pool.Release(this);
        }
    }
}
