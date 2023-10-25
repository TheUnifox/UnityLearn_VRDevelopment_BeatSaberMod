using System;
using LiteNetLib.Utils;

// Token: 0x02000038 RID: 56
public interface IConnectionRequestHandler
{
    // Token: 0x060001CF RID: 463
    void GetConnectionMessage(NetDataWriter writer, string userId, string userName, bool isConnectionOwner);

    // Token: 0x060001D0 RID: 464
    bool ValidateConnectionMessage(NetDataReader reader, out string userId, out string userName, out bool isConnectionOwner);
}
