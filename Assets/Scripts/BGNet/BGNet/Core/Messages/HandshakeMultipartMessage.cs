using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B8 RID: 184
    public class HandshakeMultipartMessage : BaseMultipartMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x1700011B RID: 283
        // (get) Token: 0x060006AB RID: 1707 RVA: 0x0001203C File Offset: 0x0001023C
        public static PacketPool<HandshakeMultipartMessage> pool
        {
            get
            {
                return ThreadStaticPacketPool<HandshakeMultipartMessage>.pool;
            }
        }

        // Token: 0x060006AC RID: 1708 RVA: 0x00012043 File Offset: 0x00010243
        public override void Release()
        {
            HandshakeMultipartMessage.pool.Release(this);
        }
    }
}
