using System;
using LiteNetLib.Utils;

// Token: 0x02000090 RID: 144
public readonly struct SyncStateId : INetImmutableSerializable<SyncStateId>, IEquatable<SyncStateId>
{
    // Token: 0x060005D1 RID: 1489 RVA: 0x000100AC File Offset: 0x0000E2AC
    private SyncStateId(byte id)
    {
        this._id = id;
    }

    // Token: 0x060005D2 RID: 1490 RVA: 0x000100B5 File Offset: 0x0000E2B5
    public bool Equals(SyncStateId other)
    {
        return this._id == other._id;
    }

    // Token: 0x060005D3 RID: 1491 RVA: 0x000100C8 File Offset: 0x0000E2C8
    public override bool Equals(object obj)
    {
        if (obj is SyncStateId)
        {
            SyncStateId other = (SyncStateId)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x060005D4 RID: 1492 RVA: 0x000100F0 File Offset: 0x0000E2F0
    public override int GetHashCode()
    {
        return this._id.GetHashCode();
    }

    // Token: 0x060005D5 RID: 1493 RVA: 0x0001010C File Offset: 0x0000E30C
    public override string ToString()
    {
        return this._id.ToString();
    }

    // Token: 0x060005D6 RID: 1494 RVA: 0x000100B5 File Offset: 0x0000E2B5
    public static bool operator ==(SyncStateId a, SyncStateId b)
    {
        return a._id == b._id;
    }

    // Token: 0x060005D7 RID: 1495 RVA: 0x00010127 File Offset: 0x0000E327
    public static bool operator !=(SyncStateId a, SyncStateId b)
    {
        return a._id != b._id;
    }

    // Token: 0x060005D8 RID: 1496 RVA: 0x0001013A File Offset: 0x0000E33A
    public SyncStateId Increment()
    {
        return new SyncStateId((byte)((this._id + 1) % 128));
    }

    // Token: 0x060005D9 RID: 1497 RVA: 0x00010150 File Offset: 0x0000E350
    public bool Before(SyncStateId other)
    {
        if (this._id <= other._id)
        {
            return other._id - this._id <= 64;
        }
        return this._id - other._id > 64;
    }

    // Token: 0x060005DA RID: 1498 RVA: 0x00010186 File Offset: 0x0000E386
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this._id);
    }

    // Token: 0x060005DB RID: 1499 RVA: 0x00010194 File Offset: 0x0000E394
    public void SerializeWithFlag(NetDataWriter writer, bool flag)
    {
        writer.Put(this._id | (flag ? 128 : 0));
    }

    // Token: 0x060005DC RID: 1500 RVA: 0x000101AF File Offset: 0x0000E3AF
    public static SyncStateId Deserialize(NetDataReader reader)
    {
        return new SyncStateId(reader.GetByte());
    }

    // Token: 0x060005DD RID: 1501 RVA: 0x000101BC File Offset: 0x0000E3BC
    public static SyncStateId DeserializeWithFlag(NetDataReader reader, out bool flag)
    {
        byte @byte = reader.GetByte();
        flag = ((@byte & 128) > 0);
        return new SyncStateId((byte)((int)@byte & -129));
    }

    // Token: 0x060005DE RID: 1502 RVA: 0x000101E9 File Offset: 0x0000E3E9
    public SyncStateId CreateFromSerializedData(NetDataReader reader)
    {
        return SyncStateId.Deserialize(reader);
    }

    // Token: 0x0400024A RID: 586
    private const byte kMaxValue = 128;

    // Token: 0x0400024B RID: 587
    private readonly byte _id;
}
