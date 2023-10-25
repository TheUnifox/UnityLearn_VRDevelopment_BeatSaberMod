using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000A9 RID: 169
    public abstract class BaseReliableResponse : IUnconnectedReliableResponse, IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket, IUnconnectedResponse
    {
        // Token: 0x1700010B RID: 267
        // (get) Token: 0x0600065A RID: 1626 RVA: 0x00011329 File Offset: 0x0000F529
        // (set) Token: 0x0600065B RID: 1627 RVA: 0x00011331 File Offset: 0x0000F531
        public uint requestId { get; private set; }

        // Token: 0x1700010C RID: 268
        // (get) Token: 0x0600065C RID: 1628 RVA: 0x0001133A File Offset: 0x0000F53A
        // (set) Token: 0x0600065D RID: 1629 RVA: 0x00011342 File Offset: 0x0000F542
        public uint responseId { get; private set; }

        // Token: 0x1700010D RID: 269
        // (get) Token: 0x0600065E RID: 1630 RVA: 0x00004777 File Offset: 0x00002977
        public virtual byte resultCode
        {
            get
            {
                return 0;
            }
        }

        // Token: 0x1700010E RID: 270
        // (get) Token: 0x0600065F RID: 1631 RVA: 0x0001134B File Offset: 0x0000F54B
        public virtual string resultCodeString
        {
            get
            {
                return "Success";
            }
        }

        // Token: 0x06000660 RID: 1632 RVA: 0x00011352 File Offset: 0x0000F552
        public virtual void Serialize(NetDataWriter writer)
        {
            writer.Put(this.requestId);
            writer.Put(this.responseId);
        }

        // Token: 0x06000661 RID: 1633 RVA: 0x0001136C File Offset: 0x0000F56C
        public virtual void Deserialize(NetDataReader reader)
        {
            this.requestId = reader.GetUInt();
            this.responseId = reader.GetUInt();
        }

        // Token: 0x06000662 RID: 1634
        public abstract void Release();

        // Token: 0x06000663 RID: 1635 RVA: 0x00011386 File Offset: 0x0000F586
        IUnconnectedReliableRequest IUnconnectedReliableRequest.WithRequestId(uint requestId)
        {
            this.requestId = requestId;
            return this;
        }

        // Token: 0x06000664 RID: 1636 RVA: 0x00011390 File Offset: 0x0000F590
        IUnconnectedResponse IUnconnectedResponse.WithResponseId(uint responseId)
        {
            this.responseId = responseId;
            return this;
        }

        // Token: 0x06000665 RID: 1637 RVA: 0x0001139A File Offset: 0x0000F59A
        IUnconnectedReliableResponse IUnconnectedReliableResponse.WithRequestAndResponseId(uint requestId, uint responseId)
        {
            this.requestId = requestId;
            this.responseId = responseId;
            return this;
        }
    }
}
