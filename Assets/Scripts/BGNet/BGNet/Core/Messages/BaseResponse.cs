using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000AA RID: 170
    public abstract class BaseResponse : IUnconnectedResponse, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x1700010F RID: 271
        // (get) Token: 0x06000667 RID: 1639 RVA: 0x000113AB File Offset: 0x0000F5AB
        // (set) Token: 0x06000668 RID: 1640 RVA: 0x000113B3 File Offset: 0x0000F5B3
        public uint responseId { get; private set; }

        // Token: 0x17000110 RID: 272
        // (get) Token: 0x06000669 RID: 1641 RVA: 0x00004777 File Offset: 0x00002977
        public virtual byte resultCode
        {
            get
            {
                return 0;
            }
        }

        // Token: 0x17000111 RID: 273
        // (get) Token: 0x0600066A RID: 1642 RVA: 0x000113BC File Offset: 0x0000F5BC
        public virtual string resultCodeString
        {
            get
            {
                return "Success";
            }
        }

        // Token: 0x0600066B RID: 1643 RVA: 0x000113C3 File Offset: 0x0000F5C3
        public virtual void Serialize(NetDataWriter writer)
        {
            writer.Put(this.responseId);
        }

        // Token: 0x0600066C RID: 1644 RVA: 0x000113D1 File Offset: 0x0000F5D1
        public virtual void Deserialize(NetDataReader reader)
        {
            this.responseId = reader.GetUInt();
        }

        // Token: 0x0600066D RID: 1645
        public abstract void Release();

        // Token: 0x0600066E RID: 1646 RVA: 0x000113DF File Offset: 0x0000F5DF
        IUnconnectedResponse IUnconnectedResponse.WithResponseId(uint responseId)
        {
            this.responseId = responseId;
            return this;
        }
    }
}
