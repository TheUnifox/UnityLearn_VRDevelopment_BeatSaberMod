using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B2 RID: 178
    public class HelloVerifyRequest : BaseReliableResponse, IHandshakeServerToClientMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000114 RID: 276
        // (get) Token: 0x0600068C RID: 1676 RVA: 0x00011B8F File Offset: 0x0000FD8F
        public static PacketPool<HelloVerifyRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<HelloVerifyRequest>.pool;
            }
        }

        // Token: 0x0600068D RID: 1677 RVA: 0x00011B96 File Offset: 0x0000FD96
        public HelloVerifyRequest Init(byte[] cookie)
        {
            this.cookie.data = cookie;
            return this;
        }

        // Token: 0x0600068E RID: 1678 RVA: 0x00011BA5 File Offset: 0x0000FDA5
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            this.cookie.Serialize(writer);
        }

        // Token: 0x0600068F RID: 1679 RVA: 0x00011BBA File Offset: 0x0000FDBA
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.cookie.Deserialize(reader);
        }

        // Token: 0x06000690 RID: 1680 RVA: 0x00011BCF File Offset: 0x0000FDCF
        public override void Release()
        {
            this.cookie.Clear();
            HelloVerifyRequest.pool.Release(this);
        }

        // Token: 0x0400029D RID: 669
        public readonly ByteArrayNetSerializable cookie = new ByteArrayNetSerializable("cookie", 32, false);
    }
}
