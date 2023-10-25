using System;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000A5 RID: 165
    public abstract class BaseAcknowledgeMessage : BaseResponse, IUnconnectedAcknowledgeMessage, IUnconnectedResponse, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x170000FE RID: 254
        // (get) Token: 0x06000624 RID: 1572 RVA: 0x00010990 File Offset: 0x0000EB90
        public override byte resultCode
        {
            get
            {
                if (!this.messageHandled)
                {
                    return 0;
                }
                return 1;
            }
        }

        // Token: 0x170000FF RID: 255
        // (get) Token: 0x06000625 RID: 1573 RVA: 0x0001099D File Offset: 0x0000EB9D
        public override string resultCodeString
        {
            get
            {
                if (!this.messageHandled)
                {
                    return "Unhandled";
                }
                return "Handled";
            }
        }

        // Token: 0x17000100 RID: 256
        // (get) Token: 0x06000626 RID: 1574 RVA: 0x000109B2 File Offset: 0x0000EBB2
        // (set) Token: 0x06000627 RID: 1575 RVA: 0x000109BA File Offset: 0x0000EBBA
        public bool messageHandled { get; private set; }

        // Token: 0x06000628 RID: 1576 RVA: 0x000109C3 File Offset: 0x0000EBC3
        public BaseAcknowledgeMessage Init(bool messageHandled)
        {
            this.messageHandled = messageHandled;
            return this;
        }

        // Token: 0x06000629 RID: 1577 RVA: 0x000109CD File Offset: 0x0000EBCD
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.Put(this.messageHandled);
        }

        // Token: 0x0600062A RID: 1578 RVA: 0x000109E2 File Offset: 0x0000EBE2
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.messageHandled = reader.GetBool();
        }
    }
}
