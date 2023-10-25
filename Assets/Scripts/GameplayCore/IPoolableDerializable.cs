using System;
using LiteNetLib.Utils;

// Token: 0x02000019 RID: 25
public interface IPoolableSerializable : INetSerializable
{
    // Token: 0x0600008C RID: 140
    void Retain();

    // Token: 0x0600008D RID: 141
    void Release();
}
