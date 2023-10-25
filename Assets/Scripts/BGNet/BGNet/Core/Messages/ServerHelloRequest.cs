using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B3 RID: 179
    public class ServerHelloRequest : BaseReliableResponse, IHandshakeServerToClientMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000115 RID: 277
        // (get) Token: 0x06000692 RID: 1682 RVA: 0x00011C02 File Offset: 0x0000FE02
        public static PacketPool<ServerHelloRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<ServerHelloRequest>.pool;
            }
        }

        // Token: 0x06000693 RID: 1683 RVA: 0x00011C09 File Offset: 0x0000FE09
        public ServerHelloRequest Init(byte[] random, byte[] publicKey, byte[] signature)
        {
            this.random.data = random;
            this.publicKey.data = publicKey;
            this.signature.data = signature;
            return this;
        }

        // Token: 0x06000694 RID: 1684 RVA: 0x00011C30 File Offset: 0x0000FE30
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            this.random.Serialize(writer);
            this.publicKey.Serialize(writer);
            this.signature.Serialize(writer);
        }

        // Token: 0x06000695 RID: 1685 RVA: 0x00011C5D File Offset: 0x0000FE5D
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.random.Deserialize(reader);
            this.publicKey.Deserialize(reader);
            this.signature.Deserialize(reader);
        }

        // Token: 0x06000696 RID: 1686 RVA: 0x00011C8A File Offset: 0x0000FE8A
        public override void Release()
        {
            this.random.Clear();
            this.publicKey.Clear();
            this.signature.Clear();
            ServerHelloRequest.pool.Release(this);
        }

        // Token: 0x0400029E RID: 670
        public readonly ByteArrayNetSerializable random = new ByteArrayNetSerializable("random", 32, false);

        // Token: 0x0400029F RID: 671
        public readonly ByteArrayNetSerializable publicKey = new ByteArrayNetSerializable("publicKey", 0, 256, false);

        // Token: 0x040002A0 RID: 672
        public readonly ByteArrayNetSerializable signature = new ByteArrayNetSerializable("signature", 128, 512, false);
    }
}
