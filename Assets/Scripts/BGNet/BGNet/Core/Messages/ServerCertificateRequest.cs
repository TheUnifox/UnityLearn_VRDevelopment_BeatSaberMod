using System;
using System.Collections.Generic;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000B4 RID: 180
    public class ServerCertificateRequest : BaseReliableResponse, IHandshakeServerToClientMessage, IHandshakeMessage, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000116 RID: 278
        // (get) Token: 0x06000698 RID: 1688 RVA: 0x00011D10 File Offset: 0x0000FF10
        public static PacketPool<ServerCertificateRequest> pool
        {
            get
            {
                return ThreadStaticPacketPool<ServerCertificateRequest>.pool;
            }
        }

        // Token: 0x17000117 RID: 279
        // (get) Token: 0x06000699 RID: 1689 RVA: 0x00011D17 File Offset: 0x0000FF17
        public IEnumerable<byte[]> certificateList
        {
            get
            {
                int num;
                for (int i = 0; i < this._certificateCount; i = num + 1)
                {
                    yield return this._certificateList[i].data;
                    num = i;
                }
                yield break;
            }
        }

        // Token: 0x0600069A RID: 1690 RVA: 0x00011D28 File Offset: 0x0000FF28
        public ServerCertificateRequest Init(IEnumerable<byte[]> certificateList)
        {
            this._certificateCount = 0;
            foreach (byte[] data in certificateList)
            {
                if (this._certificateCount == this._certificateList.Length)
                {
                    throw new ArgumentException(string.Format("Certificate Chain exceeds max length {0}", this._certificateList.Length));
                }
                ByteArrayNetSerializable[] certificateList2 = this._certificateList;
                int certificateCount = this._certificateCount;
                this._certificateCount = certificateCount + 1;
                certificateList2[certificateCount].data = data;
            }
            return this;
        }

        // Token: 0x0600069B RID: 1691 RVA: 0x00011DBC File Offset: 0x0000FFBC
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.PutVarUInt((uint)this._certificateCount);
            for (int i = 0; i < this._certificateCount; i++)
            {
                this._certificateList[i].Serialize(writer);
            }
        }

        // Token: 0x0600069C RID: 1692 RVA: 0x00011DFC File Offset: 0x0000FFFC
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this._certificateCount = (int)reader.GetVarUInt();
            if (this._certificateCount <= 0 || this._certificateCount > this._certificateList.Length)
            {
                throw new ArgumentException(string.Format("CertificateCount {0} exceeds maximum chain length {1}", this._certificateCount, this._certificateList.Length));
            }
            for (int i = 0; i < this._certificateCount; i++)
            {
                this._certificateList[i].Deserialize(reader);
            }
        }

        // Token: 0x0600069D RID: 1693 RVA: 0x00011E7C File Offset: 0x0001007C
        public override void Release()
        {
            for (int i = 0; i < this._certificateCount; i++)
            {
                this._certificateList[i].Clear();
            }
            ServerCertificateRequest.pool.Release(this);
        }

        // Token: 0x040002A1 RID: 673
        private readonly ByteArrayNetSerializable[] _certificateList = new ByteArrayNetSerializable[]
        {
            new ByteArrayNetSerializable("certificateList[0]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[1]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[2]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[3]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[4]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[5]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[6]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[7]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[8]", 0, 4096, false),
            new ByteArrayNetSerializable("certificateList[9]", 0, 4096, false)
        };

        // Token: 0x040002A2 RID: 674
        private int _certificateCount;
    }
}
