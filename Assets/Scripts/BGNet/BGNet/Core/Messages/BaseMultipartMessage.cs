using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace BGNet.Core.Messages
{
    // Token: 0x020000A7 RID: 167
    public abstract class BaseMultipartMessage : BaseReliableRequest, IUnconnectedMultipartMessage, IUnconnectedReliableRequest, IUnconnectedMessage, INetSerializable, IPoolablePacket
    {
        // Token: 0x17000105 RID: 261
        // (get) Token: 0x06000646 RID: 1606 RVA: 0x00011159 File Offset: 0x0000F359
        // (set) Token: 0x06000647 RID: 1607 RVA: 0x00011161 File Offset: 0x0000F361
        public uint multipartMessageId { get; private set; }

        // Token: 0x17000106 RID: 262
        // (get) Token: 0x06000648 RID: 1608 RVA: 0x0001116A File Offset: 0x0000F36A
        // (set) Token: 0x06000649 RID: 1609 RVA: 0x00011172 File Offset: 0x0000F372
        public int offset { get; private set; }

        // Token: 0x17000107 RID: 263
        // (get) Token: 0x0600064A RID: 1610 RVA: 0x0001117B File Offset: 0x0000F37B
        // (set) Token: 0x0600064B RID: 1611 RVA: 0x00011183 File Offset: 0x0000F383
        public int length { get; private set; }

        // Token: 0x17000108 RID: 264
        // (get) Token: 0x0600064C RID: 1612 RVA: 0x0001118C File Offset: 0x0000F38C
        // (set) Token: 0x0600064D RID: 1613 RVA: 0x00011194 File Offset: 0x0000F394
        public int totalLength { get; private set; }

        // Token: 0x17000109 RID: 265
        // (get) Token: 0x0600064E RID: 1614 RVA: 0x0001119D File Offset: 0x0000F39D
        public byte[] data
        {
            get
            {
                return this._data;
            }
        }

        // Token: 0x0600064F RID: 1615 RVA: 0x000111A5 File Offset: 0x0000F3A5
        public BaseMultipartMessage Init(uint multipartMessageId, byte[] data, int offset, int length, int totalLength)
        {
            this.multipartMessageId = multipartMessageId;
            Buffer.BlockCopy(data, offset, this._data, 0, length);
            this.offset = offset;
            this.length = length;
            this.totalLength = totalLength;
            return this;
        }

        // Token: 0x06000650 RID: 1616 RVA: 0x000111D8 File Offset: 0x0000F3D8
        public override void Serialize(NetDataWriter writer)
        {
            base.Serialize(writer);
            writer.Put(this.multipartMessageId);
            writer.PutVarUInt((uint)this.offset);
            writer.PutVarUInt((uint)this.length);
            writer.PutVarUInt((uint)this.totalLength);
            writer.Put(this._data, 0, this.length);
        }

        // Token: 0x06000651 RID: 1617 RVA: 0x00011230 File Offset: 0x0000F430
        public override void Deserialize(NetDataReader reader)
        {
            base.Deserialize(reader);
            this.multipartMessageId = reader.GetUInt();
            this.offset = (int)reader.GetVarUInt();
            this.length = (int)reader.GetVarUInt();
            this.totalLength = (int)reader.GetVarUInt();
            if (this.length > this.data.Length)
            {
                throw new InvalidPacketException("Length is too long to be valid! Length: " + this.length);
            }
            if (this.totalLength > 32767)
            {
                throw new InvalidPacketException("Total Length is too long to be valid: " + this.totalLength);
            }
            reader.GetBytes(this._data, 0, this.length);
        }

        // Token: 0x0400027A RID: 634
        public const int kDataChunkSize = 384;

        // Token: 0x0400027B RID: 635
        public const int kMaximumDataSize = 32767;

        // Token: 0x04000280 RID: 640
        private readonly byte[] _data = new byte[384];
    }
}
