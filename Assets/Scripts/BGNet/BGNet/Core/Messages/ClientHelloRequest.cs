using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B0 RID: 176
    public class ClientHelloRequest : BaseReliableRequest, IHandshakeClientToServerMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000112 RID: 274
        // (get) Token: 0x06000680 RID: 1664 RVA: 0x00011A47 File Offset: 0x0000FC47
        public static PacketPool<ClientHelloRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<ClientHelloRequest>.pool;
            }
        }

        // Token: 0x06000681 RID: 1665 RVA: 0x00011A4E File Offset: 0x0000FC4E
        public ClientHelloRequest Init(byte[] random)
        {
            Buffer.BlockCopy(random, 0, this.random, 0, this.random.Length);
            return this;
        }

        // Token: 0x06000682 RID: 1666 RVA: 0x00011A67 File Offset: 0x0000FC67
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.Put(this.random);
        }

        // Token: 0x06000683 RID: 1667 RVA: 0x00011A7C File Offset: 0x0000FC7C
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            reader.GetBytes(this.random, this.random.Length);
        }

        // Token: 0x06000684 RID: 1668 RVA: 0x00011A99 File Offset: 0x0000FC99
        public override void Release()
        {
            ClientHelloRequest.pool.Release(this);
        }

        // Token: 0x04000299 RID: 665
        public readonly byte[] random = new byte[32];
    }
}
