using System;
using LiteNetLib.Utils;

// Token: 0x0200001C RID: 28
public class GameLiftClientConnectionRequestHandler : IConnectionRequestHandler
{
    // Token: 0x1700002D RID: 45
    // (get) Token: 0x060000EB RID: 235 RVA: 0x00005267 File Offset: 0x00003467
    // (set) Token: 0x060000EC RID: 236 RVA: 0x0000526F File Offset: 0x0000346F
    public string playerSessionId { get; set; }

    // Token: 0x060000ED RID: 237 RVA: 0x00005278 File Offset: 0x00003478
    public void GetConnectionMessage(NetDataWriter writer, string userId, string userName, bool isConnectionOwner)
    {
        writer.Put(userId);
        writer.Put(userName);
        writer.Put(isConnectionOwner);
        writer.Put(this.playerSessionId);
    }

    // Token: 0x060000EE RID: 238 RVA: 0x0000529C File Offset: 0x0000349C
    public bool ValidateConnectionMessage(NetDataReader reader, out string userId, out string userName, out bool isConnectionOwner)
    {
        userId = null;
        userName = null;
        isConnectionOwner = false;
        return false;
    }
}
