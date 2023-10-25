using System;
using LiteNetLib.Utils;

// Token: 0x02000064 RID: 100
public interface INetworkPacketSubSerializer<TData>
{
    // Token: 0x0600046F RID: 1135
    void Deserialize(NetDataReader reader, int length, TData data);

    // Token: 0x06000470 RID: 1136
    void Serialize(NetDataWriter writer, INetSerializable packet);

    // Token: 0x06000471 RID: 1137
    bool HandlesType(Type type);
}
