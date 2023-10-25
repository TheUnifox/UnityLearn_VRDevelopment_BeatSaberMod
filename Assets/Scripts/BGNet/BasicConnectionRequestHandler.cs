using System;
using LiteNetLib.Utils;

// Token: 0x02000006 RID: 6
public class BasicConnectionRequestHandler : IConnectionRequestHandler
{
    // Token: 0x17000004 RID: 4
    // (get) Token: 0x0600001C RID: 28 RVA: 0x00002435 File Offset: 0x00000635
    // (set) Token: 0x0600001D RID: 29 RVA: 0x0000243D File Offset: 0x0000063D
    public string secret { get; set; }

    // Token: 0x0600001E RID: 30 RVA: 0x00002446 File Offset: 0x00000646
    public void GetConnectionMessage(NetDataWriter writer, string userId, string userName, bool isConnectionOwner)
    {
        writer.Put(this.secret);
        writer.Put(userId);
        writer.Put(userName);
        writer.Put(isConnectionOwner);
    }

    // Token: 0x0600001F RID: 31 RVA: 0x0000246C File Offset: 0x0000066C
    public bool ValidateConnectionMessage(NetDataReader reader, out string userId, out string userName, out bool isConnectionOwner)
    {
        userId = null;
        userName = null;
        isConnectionOwner = false;
        string a;
        return reader.TryGetString(out a) && reader.TryGetString(out userId) && reader.TryGetString(out userName) && reader.TryGetBool(out isConnectionOwner) && a == this.secret;
    }
}
