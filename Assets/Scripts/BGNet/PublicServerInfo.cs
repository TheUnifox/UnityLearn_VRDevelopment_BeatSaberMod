using System;
using LiteNetLib.Utils;

// Token: 0x02000072 RID: 114
public readonly struct PublicServerInfo
{
    // Token: 0x060004E8 RID: 1256 RVA: 0x0000D20F File Offset: 0x0000B40F
    public PublicServerInfo(string code, int currentPlayerCount)
    {
        this.code = code;
        this.currentPlayerCount = currentPlayerCount;
    }

    // Token: 0x060004E9 RID: 1257 RVA: 0x0000D21F File Offset: 0x0000B41F
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this.code);
        writer.PutVarInt(this.currentPlayerCount);
    }

    // Token: 0x060004EA RID: 1258 RVA: 0x0000D23C File Offset: 0x0000B43C
    public static PublicServerInfo Deserialize(NetDataReader reader)
    {
        string @string = reader.GetString();
        int varInt = reader.GetVarInt();
        return new PublicServerInfo(@string, varInt);
    }

    // Token: 0x040001E0 RID: 480
    public readonly string code;

    // Token: 0x040001E1 RID: 481
    public readonly int currentPlayerCount;
}
