using System;
using LiteNetLib.Utils;

// Token: 0x0200008A RID: 138
public class StandardScoreSyncStateDeltaNetSerializable : INetSerializable, IPoolablePacket, ISyncStateDeltaSerializable<StandardScoreSyncState>
{
    // Token: 0x170000EA RID: 234
    // (get) Token: 0x060005A2 RID: 1442 RVA: 0x0000F4BD File Offset: 0x0000D6BD
    public static IPacketPool<StandardScoreSyncStateDeltaNetSerializable> pool
    {
        get
        {
            return ThreadStaticPacketPool<StandardScoreSyncStateDeltaNetSerializable>.pool;
        }
    }

    // Token: 0x170000EB RID: 235
    // (get) Token: 0x060005A3 RID: 1443 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
    // (set) Token: 0x060005A4 RID: 1444 RVA: 0x0000F4CC File Offset: 0x0000D6CC
    public SyncStateId baseId { get; set; }

    // Token: 0x170000EC RID: 236
    // (get) Token: 0x060005A5 RID: 1445 RVA: 0x0000F4D5 File Offset: 0x0000D6D5
    // (set) Token: 0x060005A6 RID: 1446 RVA: 0x0000F4DD File Offset: 0x0000D6DD
    public int timeOffsetMs { get; set; }

    // Token: 0x170000ED RID: 237
    // (get) Token: 0x060005A7 RID: 1447 RVA: 0x0000F4E6 File Offset: 0x0000D6E6
    // (set) Token: 0x060005A8 RID: 1448 RVA: 0x0000F4EE File Offset: 0x0000D6EE
    public StandardScoreSyncState delta
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

    // Token: 0x060005A9 RID: 1449 RVA: 0x0000F4F8 File Offset: 0x0000D6F8
    public void Serialize(NetDataWriter writer)
    {
        bool flag = default(StandardScoreSyncState).Equals(this._delta);
        this.baseId.SerializeWithFlag(writer, flag);
        writer.PutVarInt(this.timeOffsetMs);
        if (!flag)
        {
            this._delta.Serialize(writer);
        }
    }

    // Token: 0x060005AA RID: 1450 RVA: 0x0000F548 File Offset: 0x0000D748
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
        this._delta = default(StandardScoreSyncState);
    }

    // Token: 0x060005AB RID: 1451 RVA: 0x0000F58B File Offset: 0x0000D78B
    public void Release()
    {
        StandardScoreSyncStateDeltaNetSerializable.pool.Release(this);
    }

    // Token: 0x0400022E RID: 558
    private StandardScoreSyncState _delta;
}
