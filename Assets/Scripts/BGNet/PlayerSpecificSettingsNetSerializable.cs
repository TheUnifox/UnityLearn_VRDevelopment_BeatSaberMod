using System;
using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200006E RID: 110
[Preserve]
public class PlayerSpecificSettingsNetSerializable : INetSerializable
{
    // Token: 0x060004C7 RID: 1223 RVA: 0x000024B7 File Offset: 0x000006B7
    public PlayerSpecificSettingsNetSerializable()
    {
    }

    // Token: 0x060004C8 RID: 1224 RVA: 0x0000CCB8 File Offset: 0x0000AEB8
    public PlayerSpecificSettingsNetSerializable(string userId, string userName, bool leftHanded, bool automaticPlayerHeight, float playerHeight, float headPosToPlayerHeightOffset, Color saberAColor, Color saberBColor, Color obstaclesColor, Color environmentColor0, Color environmentColor1, Color environmentColor0Boost, Color environmentColor1Boost)
    {
        this.userId = userId;
        this.userName = userName;
        this.leftHanded = leftHanded;
        this.automaticPlayerHeight = automaticPlayerHeight;
        this.playerHeight = playerHeight;
        this.headPosToPlayerHeightOffset = headPosToPlayerHeightOffset;
        this.colorScheme = new ColorSchemeNetSerializable(saberAColor, saberBColor, obstaclesColor, environmentColor0, environmentColor1, environmentColor0Boost, environmentColor1Boost);
    }

    // Token: 0x060004C9 RID: 1225 RVA: 0x0000CD14 File Offset: 0x0000AF14
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this.userId);
        writer.Put(this.userName);
        writer.Put(this.leftHanded);
        writer.Put(this.automaticPlayerHeight);
        writer.Put(this.playerHeight);
        writer.Put(this.headPosToPlayerHeightOffset);
        this.colorScheme.Serialize(writer);
    }

    // Token: 0x060004CA RID: 1226 RVA: 0x0000CD78 File Offset: 0x0000AF78
    public void Deserialize(NetDataReader reader)
    {
        this.userId = reader.GetString();
        this.userName = reader.GetString();
        this.leftHanded = reader.GetBool();
        this.automaticPlayerHeight = reader.GetBool();
        this.playerHeight = reader.GetFloat();
        this.headPosToPlayerHeightOffset = reader.GetFloat();
        this.colorScheme.Deserialize(reader);
    }

    // Token: 0x040001D2 RID: 466
    public string userId;

    // Token: 0x040001D3 RID: 467
    public string userName;

    // Token: 0x040001D4 RID: 468
    public bool leftHanded;

    // Token: 0x040001D5 RID: 469
    public bool automaticPlayerHeight;

    // Token: 0x040001D6 RID: 470
    public float playerHeight;

    // Token: 0x040001D7 RID: 471
    public float headPosToPlayerHeightOffset;

    // Token: 0x040001D8 RID: 472
    public ColorSchemeNetSerializable colorScheme;
}
