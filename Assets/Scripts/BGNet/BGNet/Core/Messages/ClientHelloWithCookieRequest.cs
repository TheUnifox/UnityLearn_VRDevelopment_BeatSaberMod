using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B1 RID: 177
    public class ClientHelloWithCookieRequest : BaseReliableRequest, IHandshakeClientToServerMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000113 RID: 275
        // (get) Token: 0x06000686 RID: 1670 RVA: 0x00011ABB File Offset: 0x0000FCBB
        public static PacketPool<ClientHelloWithCookieRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<ClientHelloWithCookieRequest>.pool;
            }
        }

        // Token: 0x06000687 RID: 1671 RVA: 0x00011AC2 File Offset: 0x0000FCC2
        public ClientHelloWithCookieRequest Init(uint certificateResponseId, byte[] random, byte[] cookie)
        {
            this.certificateResponseId = certificateResponseId;
            this.random.data = random;
            this.cookie.data = cookie;
            return this;
        }

        // Token: 0x06000688 RID: 1672 RVA: 0x00011AE4 File Offset: 0x0000FCE4
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.Put(this.certificateResponseId);
            this.random.Serialize(writer);
            this.cookie.Serialize(writer);
        }

        // Token: 0x06000689 RID: 1673 RVA: 0x00011B11 File Offset: 0x0000FD11
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.certificateResponseId = reader.GetUInt();
            this.random.Deserialize(reader);
            this.cookie.Deserialize(reader);
        }

        // Token: 0x0600068A RID: 1674 RVA: 0x00011B3E File Offset: 0x0000FD3E
        public override void Release()
        {
            this.random.Clear();
            this.cookie.Clear();
            ClientHelloWithCookieRequest.pool.Release(this);
        }

        // Token: 0x0400029A RID: 666
        public uint certificateResponseId;

        // Token: 0x0400029B RID: 667
        public readonly ByteArrayNetSerializable random = new ByteArrayNetSerializable("random", 32, false);

        // Token: 0x0400029C RID: 668
        public readonly ByteArrayNetSerializable cookie = new ByteArrayNetSerializable("cookie", 32, false);
    }
}
