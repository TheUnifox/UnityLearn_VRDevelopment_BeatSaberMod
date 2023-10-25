using System;
using LiteNetLib.Utils;
using UnityEngine;

// Token: 0x02000022 RID: 34
public readonly struct MultiplayerAvatarData : INetImmutableSerializable<MultiplayerAvatarData>, IEquatable<MultiplayerAvatarData>
{
    // Token: 0x060000A3 RID: 163 RVA: 0x00003E04 File Offset: 0x00002004
    public MultiplayerAvatarData(string headTopId, Color32Serializable headTopPrimaryColor, Color32Serializable headTopSecondaryColor, string glassesId, Color32Serializable glassesColor, string facialHairId, Color32Serializable facialHairColor, string handsId, Color32Serializable handsColor, string clothesId, Color32Serializable clothesPrimaryColor, Color32 clothesSecondaryColor, Color32Serializable clothesDetailColor, string skinColorId, string eyesId, string mouthId)
    {
        this.headTopId = headTopId;
        this.headTopPrimaryColor = headTopPrimaryColor;
        this.headTopSecondaryColor = headTopSecondaryColor;
        this.glassesId = glassesId;
        this.glassesColor = glassesColor;
        this.facialHairId = facialHairId;
        this.facialHairColor = facialHairColor;
        this.handsId = handsId;
        this.handsColor = handsColor;
        this.clothesId = clothesId;
        this.clothesPrimaryColor = clothesPrimaryColor;
        this.clothesSecondaryColor = clothesSecondaryColor;
        this.clothesDetailColor = clothesDetailColor;
        this.skinColorId = skinColorId;
        this.eyesId = eyesId;
        this.mouthId = mouthId;
        this.glassesId = skinColorId;
    }

    // Token: 0x060000A4 RID: 164 RVA: 0x00003E98 File Offset: 0x00002098
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(this.headTopId);
        this.headTopPrimaryColor.Serialize(writer);
        this.handsColor.Serialize(writer);
        writer.Put(this.clothesId);
        this.clothesPrimaryColor.Serialize(writer);
        this.clothesSecondaryColor.Serialize(writer);
        this.clothesDetailColor.Serialize(writer);
        default(Color32Serializable).Serialize(writer);
        default(Color32Serializable).Serialize(writer);
        writer.Put(this.eyesId);
        writer.Put(this.mouthId);
        this.glassesColor.Serialize(writer);
        this.facialHairColor.Serialize(writer);
        this.headTopSecondaryColor.Serialize(writer);
        writer.Put(this.glassesId);
        writer.Put(this.facialHairId);
        writer.Put(this.handsId);
    }

    // Token: 0x060000A5 RID: 165 RVA: 0x00003FC7 File Offset: 0x000021C7
    public MultiplayerAvatarData CreateFromSerializedData(NetDataReader reader)
    {
        return MultiplayerAvatarData.Deserialize(reader);
    }

    // Token: 0x060000A6 RID: 166 RVA: 0x00003FD0 File Offset: 0x000021D0
    public static MultiplayerAvatarData Deserialize(NetDataReader reader)
    {
        Color32Serializable c = default(Color32Serializable);
        Color32Serializable c2 = default(Color32Serializable);
        Color32Serializable c3 = default(Color32Serializable);
        Color32Serializable c4 = default(Color32Serializable);
        Color32Serializable c5 = default(Color32Serializable);
        Color32Serializable color32Serializable = default(Color32Serializable);
        Color32Serializable color32Serializable2 = default(Color32Serializable);
        Color32Serializable c6 = default(Color32Serializable);
        Color32Serializable c7 = default(Color32Serializable);
        Color32Serializable c8 = default(Color32Serializable);
        string @string = reader.GetString();
        c.Deserialize(reader);
        c2.Deserialize(reader);
        string string2 = reader.GetString();
        c3.Deserialize(reader);
        c4.Deserialize(reader);
        c5.Deserialize(reader);
        color32Serializable.Deserialize(reader);
        color32Serializable2.Deserialize(reader);
        string string3 = reader.GetString();
        string string4 = reader.GetString();
        c6.Deserialize(reader);
        c7.Deserialize(reader);
        c8.Deserialize(reader);
        string string5 = reader.GetString();
        string string6 = reader.GetString();
        string string7 = reader.GetString();
        string text = string5;
        return new MultiplayerAvatarData(@string, c, c8, string5, c6, string6, c7, string7, c2, string2, c3, c4, c5, text, string3, string4);
    }

    // Token: 0x060000A7 RID: 167 RVA: 0x00004100 File Offset: 0x00002300
    public bool Equals(MultiplayerAvatarData other)
    {
        return this.headTopId == other.headTopId && this.headTopPrimaryColor.Equals(other.headTopPrimaryColor) && this.headTopSecondaryColor.Equals(other.headTopSecondaryColor) && this.glassesId == other.glassesId && this.glassesColor.Equals(other.glassesColor) && this.facialHairId == other.facialHairId && this.facialHairColor.Equals(other.facialHairColor) && this.handsId == other.handsId && this.handsColor.Equals(other.handsColor) && this.clothesId == other.clothesId && this.clothesPrimaryColor.Equals(other.clothesPrimaryColor) && this.clothesSecondaryColor.Equals(other.clothesSecondaryColor) && this.clothesDetailColor.Equals(other.clothesDetailColor) && this.skinColorId == other.skinColorId && this.eyesId == other.eyesId && this.mouthId == other.mouthId;
    }

    // Token: 0x060000A8 RID: 168 RVA: 0x000042B4 File Offset: 0x000024B4
    public override bool Equals(object obj)
    {
        if (obj is MultiplayerAvatarData)
        {
            MultiplayerAvatarData other = (MultiplayerAvatarData)obj;
            return this.Equals(other);
        }
        return false;
    }

    // Token: 0x060000A9 RID: 169 RVA: 0x000042DC File Offset: 0x000024DC
    public override int GetHashCode()
    {
        return ((((((((((((((((this.headTopId != null) ? this.headTopId.GetHashCode() : 0) * 397 ^ this.headTopPrimaryColor.GetHashCode()) * 397 ^ this.headTopSecondaryColor.GetHashCode()) * 397 ^ ((this.glassesId != null) ? this.glassesId.GetHashCode() : 0)) * 397 ^ this.glassesColor.GetHashCode()) * 397 ^ ((this.facialHairId != null) ? this.facialHairId.GetHashCode() : 0)) * 397 ^ this.facialHairColor.GetHashCode()) * 397 ^ ((this.handsId != null) ? this.handsId.GetHashCode() : 0)) * 397 ^ this.handsColor.GetHashCode()) * 397 ^ ((this.clothesId != null) ? this.clothesId.GetHashCode() : 0)) * 397 ^ this.clothesPrimaryColor.GetHashCode()) * 397 ^ this.clothesSecondaryColor.GetHashCode()) * 397 ^ this.clothesDetailColor.GetHashCode()) * 397 ^ ((this.skinColorId != null) ? this.skinColorId.GetHashCode() : 0)) * 397 ^ ((this.eyesId != null) ? this.eyesId.GetHashCode() : 0)) * 397 ^ ((this.mouthId != null) ? this.mouthId.GetHashCode() : 0);
    }

    // Token: 0x04000074 RID: 116
    public readonly string headTopId;

    // Token: 0x04000075 RID: 117
    public readonly Color32Serializable headTopPrimaryColor;

    // Token: 0x04000076 RID: 118
    public readonly Color32Serializable headTopSecondaryColor;

    // Token: 0x04000077 RID: 119
    public readonly string glassesId;

    // Token: 0x04000078 RID: 120
    public readonly Color32Serializable glassesColor;

    // Token: 0x04000079 RID: 121
    public readonly string facialHairId;

    // Token: 0x0400007A RID: 122
    public readonly Color32Serializable facialHairColor;

    // Token: 0x0400007B RID: 123
    public readonly string handsId;

    // Token: 0x0400007C RID: 124
    public readonly Color32Serializable handsColor;

    // Token: 0x0400007D RID: 125
    public readonly string clothesId;

    // Token: 0x0400007E RID: 126
    public readonly Color32Serializable clothesPrimaryColor;

    // Token: 0x0400007F RID: 127
    public readonly Color32Serializable clothesSecondaryColor;

    // Token: 0x04000080 RID: 128
    public readonly Color32Serializable clothesDetailColor;

    // Token: 0x04000081 RID: 129
    public readonly string skinColorId;

    // Token: 0x04000082 RID: 130
    public readonly string eyesId;

    // Token: 0x04000083 RID: 131
    public readonly string mouthId;
}
