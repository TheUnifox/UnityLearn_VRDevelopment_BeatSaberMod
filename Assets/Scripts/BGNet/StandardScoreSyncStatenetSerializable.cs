using System;
using LiteNetLib.Utils;

// Token: 0x0200008B RID: 139
public class StandardScoreSyncStateNetSerializable : INetSerializable, IPoolablePacket, ISyncStateSerializable<StandardScoreSyncState>
{
    // Token: 0x170000EE RID: 238
    // (get) Token: 0x060005AD RID: 1453 RVA: 0x0000F598 File Offset: 0x0000D798
    public static PacketPool<StandardScoreSyncStateNetSerializable> pool
    {
        get
        {
            return ThreadStaticPacketPool<StandardScoreSyncStateNetSerializable>.pool;
        }
    }

    // Token: 0x170000EF RID: 239
    // (get) Token: 0x060005AE RID: 1454 RVA: 0x0000F59F File Offset: 0x0000D79F
    // (set) Token: 0x060005AF RID: 1455 RVA: 0x0000F5A7 File Offset: 0x0000D7A7
    public SyncStateId id { get; set; }

    // Token: 0x170000F0 RID: 240
    // (get) Token: 0x060005B0 RID: 1456 RVA: 0x0000F5B0 File Offset: 0x0000D7B0
    // (set) Token: 0x060005B1 RID: 1457 RVA: 0x0000F5B8 File Offset: 0x0000D7B8
    public float time { get; set; }

    // Token: 0x170000F1 RID: 241
    // (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000F5C1 File Offset: 0x0000D7C1
    // (set) Token: 0x060005B3 RID: 1459 RVA: 0x0000F5C9 File Offset: 0x0000D7C9
    public StandardScoreSyncState state
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

    // Token: 0x060005B4 RID: 1460 RVA: 0x0000F5D4 File Offset: 0x0000D7D4
    public void Serialize(NetDataWriter writer)
    {
        this.id.Serialize(writer);
        writer.Put(this.time);
        this._state.Serialize(writer);
    }

    // Token: 0x060005B5 RID: 1461 RVA: 0x0000F608 File Offset: 0x0000D808
    public void Deserialize(NetDataReader reader)
    {
        this.id = SyncStateId.Deserialize(reader);
        this.time = reader.GetFloat();
        this._state.Deserialize(reader);
    }

    // Token: 0x060005B6 RID: 1462 RVA: 0x0000F62E File Offset: 0x0000D82E
    public void Release()
    {
        StandardScoreSyncStateNetSerializable.pool.Release(this);
    }

    // Token: 0x04000231 RID: 561
    private StandardScoreSyncState _state;
}
