using System;
using LiteNetLib.Utils;

// Token: 0x0200004C RID: 76
public interface IRemoteProcedureCall : INetSerializable, IPoolablePacket
{
    // Token: 0x17000097 RID: 151
    // (get) Token: 0x060002F3 RID: 755
    float syncTime { get; }
}
