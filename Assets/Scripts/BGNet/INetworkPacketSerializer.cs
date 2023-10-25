using System;
using LiteNetLib.Utils;

// Token: 0x02000063 RID: 99
public interface INetworkPacketSerializer<TData>
{
    // Token: 0x0600046D RID: 1133
    void ProcessAllPackets(NetDataReader reader, TData data);

    // Token: 0x0600046E RID: 1134
    void SerializePacket(NetDataWriter writer, INetSerializable packet);
}
