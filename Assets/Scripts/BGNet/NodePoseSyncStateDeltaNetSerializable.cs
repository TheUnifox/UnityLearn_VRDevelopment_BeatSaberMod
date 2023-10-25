using System;
using LiteNetLib.Utils;

// Token: 0x02000086 RID: 134
public class NodePoseSyncStateDeltaNetSerializable : INetSerializable, IPoolablePacket, ISyncStateDeltaSerializable<NodePoseSyncState>
{
    // Token: 0x170000E2 RID: 226
    // (get) Token: 0x0600057C RID: 1404 RVA: 0x0000EED6 File Offset: 0x0000D0D6
    public static IPacketPool<NodePoseSyncStateDeltaNetSerializable> pool
    {
        get
        {
            return ThreadStaticPacketPool<NodePoseSyncStateDeltaNetSerializable>.pool;
        }
    }

    // Token: 0x170000E3 RID: 227
    // (get) Token: 0x0600057D RID: 1405 RVA: 0x0000EEDD File Offset: 0x0000D0DD
    // (set) Token: 0x0600057E RID: 1406 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
    public SyncStateId baseId { get; set; }

    // Token: 0x170000E4 RID: 228
    // (get) Token: 0x0600057F RID: 1407 RVA: 0x0000EEEE File Offset: 0x0000D0EE
    // (set) Token: 0x06000580 RID: 1408 RVA: 0x0000EEF6 File Offset: 0x0000D0F6
    public int timeOffsetMs { get; set; }

    // Token: 0x170000E5 RID: 229
    // (get) Token: 0x06000581 RID: 1409 RVA: 0x0000EEFF File Offset: 0x0000D0FF
    // (set) Token: 0x06000582 RID: 1410 RVA: 0x0000EF07 File Offset: 0x0000D107
    public NodePoseSyncState delta
    {
        get
        {
            return this._delta;
        }
        set
        {
            this._delta = value;
        }
    }

    // Token: 0x06000583 RID: 1411 RVA: 0x0000EF10 File Offset: 0x0000D110
    public void Serialize(NetDataWriter writer)
    {
        bool flag = default(NodePoseSyncState).Equals(this._delta);
        this.baseId.SerializeWithFlag(writer, flag);
        writer.PutVarInt(this.timeOffsetMs);
        if (!flag)
        {
            this._delta.Serialize(writer);
        }
    }

    // Token: 0x06000584 RID: 1412 RVA: 0x0000EF60 File Offset: 0x0000D160
    public void Deserialize(NetDataReader reader)
    {
        bool flag;
        this.baseId = SyncStateId.DeserializeWithFlag(reader, out flag);
        this.timeOffsetMs = reader.GetVarInt();
        if (!flag)
        {
            this._delta.Deserialize(reader);
            return;
        }
        this._delta = default(NodePoseSyncState);
    }

    // Token: 0x06000585 RID: 1413 RVA: 0x0000EFA3 File Offset: 0x0000D1A3
    public void Release()
    {
        NodePoseSyncStateDeltaNetSerializable.pool.Release(this);
    }

    // Token: 0x04000223 RID: 547
    private NodePoseSyncState _delta;
}
