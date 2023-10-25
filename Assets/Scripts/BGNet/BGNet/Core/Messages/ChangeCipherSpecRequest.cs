using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B6 RID: 182
    public class ChangeCipherSpecRequest : BaseReliableResponse, IHandshakeServerToClientMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000119 RID: 281
        // (get) Token: 0x060006A5 RID: 1701 RVA: 0x00012014 File Offset: 0x00010214
        public static PacketPool<ChangeCipherSpecRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<ChangeCipherSpecRequest>.pool;
            }
        }

        // Token: 0x060006A6 RID: 1702 RVA: 0x0001201B File Offset: 0x0001021B
        public override void Release()
        {
            ChangeCipherSpecRequest.pool.Release(this);
        }
    }
}
