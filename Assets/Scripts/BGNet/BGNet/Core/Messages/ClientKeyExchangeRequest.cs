using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B5 RID: 181
    public class ClientKeyExchangeRequest : BaseReliableResponse, IHandshakeClientToServerMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000118 RID: 280
        // (get) Token: 0x0600069F RID: 1695 RVA: 0x00011F9D File Offset: 0x0001019D
        public static PacketPool<ClientKeyExchangeRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<ClientKeyExchangeRequest>.pool;
            }
        }

        // Token: 0x060006A0 RID: 1696 RVA: 0x00011FA4 File Offset: 0x000101A4
        public ClientKeyExchangeRequest Init(byte[] clientPublicKey)
        {
            this.clientPublicKey.data = clientPublicKey;
            return this;
        }

        // Token: 0x060006A1 RID: 1697 RVA: 0x00011FB3 File Offset: 0x000101B3
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            this.clientPublicKey.Serialize(writer);
        }

        // Token: 0x060006A2 RID: 1698 RVA: 0x00011FC8 File Offset: 0x000101C8
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.clientPublicKey.Deserialize(reader);
        }

        // Token: 0x060006A3 RID: 1699 RVA: 0x00011FDD File Offset: 0x000101DD
        public override void Release()
        {
            this.clientPublicKey.Clear();
            ClientKeyExchangeRequest.pool.Release(this);
        }

        // Token: 0x040002A3 RID: 675
        public readonly ByteArrayNetSerializable clientPublicKey = new ByteArrayNetSerializable("clientPublicKey", 0, 256, false);
    }
}
