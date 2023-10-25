using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000A8 RID: 168
    public abstract class BaseReliableRequest : IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x1700010A RID: 266
        // (get) Token: 0x06000653 RID: 1619 RVA: 0x000112F2 File Offset: 0x0000F4F2
        // (set) Token: 0x06000654 RID: 1620 RVA: 0x000112FA File Offset: 0x0000F4FA
        public uint requestId { get; private set; }

        // Token: 0x06000655 RID: 1621 RVA: 0x00011303 File Offset: 0x0000F503
        public virtual void Serialize(NetDataWriter writer)
        {
            writer.Put(this.requestId);
        }

        // Token: 0x06000656 RID: 1622 RVA: 0x00011311 File Offset: 0x0000F511
        public virtual void Deserialize(NetDataReader reader)
        {
            this.requestId = reader.GetUInt();
        }

        // Token: 0x06000657 RID: 1623
        public abstract void Release();

        // Token: 0x06000658 RID: 1624 RVA: 0x0001131F File Offset: 0x0000F51F
        IUnconnectedReliableRequest IUnconnectedReliableRequest.WithRequestId(uint requestId)
        {
            this.requestId = requestId;
            return this;
        }
    }
}
