using System;
using LiteNetLib.Utils;

// Token: 0x02000087 RID: 135
public class NodePoseSyncStateNetSerializable : INetSerializable, IPoolablePacket, ISyncStateSerializable<NodePoseSyncState>
{
    // Token: 0x170000E6 RID: 230
    // (get) Token: 0x06000587 RID: 1415 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
    public static PacketPool<NodePoseSyncStateNetSerializable> pool
    {
        get
        {
            return ThreadStaticPacketPool<NodePoseSyncStateNetSerializable>.pool;
        }
    }

    // Token: 0x170000E7 RID: 231
    // (get) Token: 0x06000588 RID: 1416 RVA: 0x0000EFB7 File Offset: 0x0000D1B7
    // (set) Token: 0x06000589 RID: 1417 RVA: 0x0000EFBF File Offset: 0x0000D1BF
    public SyncStateId id { get; set; }

    // Token: 0x170000E8 RID: 232
    // (get) Token: 0x0600058A RID: 1418 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
    // (set) Token: 0x0600058B RID: 1419 RVA: 0x0000EFD0 File Offset: 0x0000D1D0
    public float time { get; set; }

    // Token: 0x170000E9 RID: 233
    // (get) Token: 0x0600058C RID: 1420 RVA: 0x0000EFD9 File Offset: 0x0000D1D9
    // (set) Token: 0x0600058D RID: 1421 RVA: 0x0000EFE1 File Offset: 0x0000D1E1
    public NodePoseSyncState state
    {
        get
        {
            return this._state;
        }
        set
        {
            this._state = value;
        }
    }

    // Token: 0x0600058E RID: 1422 RVA: 0x0000EFEC File Offset: 0x0000D1EC
    public void Serialize(NetDataWriter writer)
    {
        this.id.Serialize(writer);
        writer.Put(this.time);
        this._state.Serialize(writer);
    }

    // Token: 0x0600058F RID: 1423 RVA: 0x0000F020 File Offset: 0x0000D220
    public void Deserialize(NetDataReader reader)
    {
        this.id = SyncStateId.Deserialize(reader);
        this.time = reader.GetFloat();
        this._state.Deserialize(reader);
    }

    // Token: 0x06000590 RID: 1424 RVA: 0x0000F046 File Offset: 0x0000D246
    public void Release()
    {
        NodePoseSyncStateNetSerializable.pool.Release(this);
    }

    // Token: 0x04000226 RID: 550
    private NodePoseSyncState _state;
}
