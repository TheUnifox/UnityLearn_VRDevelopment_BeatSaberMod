using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x0200000D RID: 13
public struct ColorSchemeNetSerializable : INetSerializable
{
    // Token: 0x06000038 RID: 56 RVA: 0x000029AC File Offset: 0x00000BAC
    public ColorSchemeNetSerializable(Color saberAColor, Color saberBColor, Color obstaclesColor, Color environmentColor0, Color environmentColor1, Color environmentColor0Boost, Color environmentColor1Boost)
    {
        this.saberAColor = saberAColor;
        this.saberBColor = saberBColor;
        this.obstaclesColor = obstaclesColor;
        this.environmentColor0 = environmentColor0;
        this.environmentColor1 = environmentColor1;
        this.environmentColor0Boost = environmentColor0Boost;
        this.environmentColor1Boost = environmentColor1Boost;
    }

    // Token: 0x06000039 RID: 57 RVA: 0x00002A14 File Offset: 0x00000C14
    public void Serialize(NetDataWriter writer)
    {
        this.saberAColor.Serialize(writer);
        this.saberBColor.Serialize(writer);
        this.obstaclesColor.Serialize(writer);
        this.environmentColor0.Serialize(writer);
        this.environmentColor1.Serialize(writer);
        this.environmentColor0Boost.Serialize(writer);
        this.environmentColor1Boost.Serialize(writer);
    }

    // Token: 0x0600003A RID: 58 RVA: 0x00002A78 File Offset: 0x00000C78
    public void Deserialize(NetDataReader reader)
    {
        this.saberAColor.Deserialize(reader);
        this.saberBColor.Deserialize(reader);
        this.obstaclesColor.Deserialize(reader);
        this.environmentColor0.Deserialize(reader);
        this.environmentColor1.Deserialize(reader);
        this.environmentColor0Boost.Deserialize(reader);
        this.environmentColor1Boost.Deserialize(reader);
    }

    // Token: 0x0400001F RID: 31
    public ColorNoAlphaSerializable saberAColor;

    // Token: 0x04000020 RID: 32
    public ColorNoAlphaSerializable saberBColor;

    // Token: 0x04000021 RID: 33
    public ColorNoAlphaSerializable obstaclesColor;

    // Token: 0x04000022 RID: 34
    public ColorNoAlphaSerializable environmentColor0;

    // Token: 0x04000023 RID: 35
    public ColorNoAlphaSerializable environmentColor1;

    // Token: 0x04000024 RID: 36
    public ColorNoAlphaSerializable environmentColor0Boost;

    // Token: 0x04000025 RID: 37
    public ColorNoAlphaSerializable environmentColor1Boost;
}
