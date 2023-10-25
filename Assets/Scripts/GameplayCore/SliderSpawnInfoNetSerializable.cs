using System;
using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200003B RID: 59
public class SliderSpawnInfoNetSerializable : PoolableSerializable
{
    // Token: 0x06000150 RID: 336 RVA: 0x0000697D File Offset: 0x00004B7D
    public static SliderSpawnInfoNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<SliderSpawnInfoNetSerializable>();
    }

    // Token: 0x06000151 RID: 337 RVA: 0x00006984 File Offset: 0x00004B84
    public virtual SliderSpawnInfoNetSerializable Init(ColorType colorType, SliderData.Type sliderType, bool hasHeadNote, float headTime, int headLineIndex, NoteLineLayer headLineLayer, NoteLineLayer headBeforeJumpLineLayer, float headControlPointLengthMultiplier, NoteCutDirection headCutDirection, float headCutDirectionAngleOffset, bool hasTailNote, float tailTime, int tailLineIndex, NoteLineLayer tailLineLayer, NoteLineLayer tailBeforeJumpLineLayer, float tailControlPointLengthMultiplier, NoteCutDirection tailCutDirection, float tailCutDirectionAngleOffset, SliderMidAnchorMode midAnchorMode, int sliceCount, float squishAmount, Vector3 headMoveStartPos, Vector3 headJumpStartPos, Vector3 headJumpEndPos, float headJumpGravity, Vector3 tailMoveStartPos, Vector3 tailJumpStartPos, Vector3 tailJumpEndPos, float tailJumpGravity, float moveDuration, float jumpDuration, float rotation)
    {
        this.colorType = colorType;
        this.sliderType = sliderType;
        this.hasHeadNote = hasHeadNote;
        this.headTime = headTime;
        this.headLineIndex = headLineIndex;
        this.headBeforeJumpLineLayer = headBeforeJumpLineLayer;
        this.headLineLayer = headLineLayer;
        this.headControlPointLengthMultiplier = headControlPointLengthMultiplier;
        this.headCutDirection = headCutDirection;
        this.headCutDirectionAngleOffset = headCutDirectionAngleOffset;
        this.hasTailNote = hasTailNote;
        this.tailTime = tailTime;
        this.tailLineIndex = tailLineIndex;
        this.tailBeforeJumpLineLayer = tailBeforeJumpLineLayer;
        this.tailLineLayer = tailLineLayer;
        this.tailControlPointLengthMultiplier = tailControlPointLengthMultiplier;
        this.tailCutDirection = tailCutDirection;
        this.tailCutDirectionAngleOffset = tailCutDirectionAngleOffset;
        this.midAnchorMode = midAnchorMode;
        this.sliceCount = sliceCount;
        this.squishAmount = squishAmount;
        this.headMoveStartPos = headMoveStartPos;
        this.headJumpStartPos = headJumpStartPos;
        this.headJumpEndPos = headJumpEndPos;
        this.headJumpGravity = headJumpGravity;
        this.tailMoveStartPos = tailMoveStartPos;
        this.tailJumpStartPos = tailJumpStartPos;
        this.tailJumpEndPos = tailJumpEndPos;
        this.tailJumpGravity = tailJumpGravity;
        this.moveDuration = moveDuration;
        this.jumpDuration = jumpDuration;
        this.rotation = rotation;
        return this;
    }

    // Token: 0x06000152 RID: 338 RVA: 0x00004823 File Offset: 0x00002A23
    [Preserve]
    public SliderSpawnInfoNetSerializable()
    {
    }

    // Token: 0x06000153 RID: 339 RVA: 0x00006AB0 File Offset: 0x00004CB0
    public override void Serialize(NetDataWriter writer)
    {
        writer.PutVarInt((int)this.colorType);
        writer.PutVarInt((int)this.sliderType);
        writer.Put(this.hasHeadNote);
        writer.Put(this.headTime);
        writer.PutVarInt(this.headLineIndex);
        writer.PutVarInt((int)this.headLineLayer);
        writer.PutVarInt((int)this.headBeforeJumpLineLayer);
        writer.Put(this.headControlPointLengthMultiplier);
        writer.PutVarInt((int)this.headCutDirection);
        writer.Put(this.headCutDirectionAngleOffset);
        writer.Put(this.hasTailNote);
        writer.Put(this.tailTime);
        writer.PutVarInt(this.tailLineIndex);
        writer.PutVarInt((int)this.tailLineLayer);
        writer.PutVarInt((int)this.tailBeforeJumpLineLayer);
        writer.Put(this.tailControlPointLengthMultiplier);
        writer.PutVarInt((int)this.tailCutDirection);
        writer.Put(this.tailCutDirectionAngleOffset);
        writer.PutVarInt((int)this.midAnchorMode);
        writer.PutVarInt(this.sliceCount);
        writer.Put(this.squishAmount);
        this.headMoveStartPos.Serialize(writer);
        this.headJumpStartPos.Serialize(writer);
        this.headJumpEndPos.Serialize(writer);
        writer.Put(this.headJumpGravity);
        this.tailMoveStartPos.Serialize(writer);
        this.tailJumpStartPos.Serialize(writer);
        this.tailJumpEndPos.Serialize(writer);
        writer.Put(this.tailJumpGravity);
        writer.Put(this.moveDuration);
        writer.Put(this.jumpDuration);
        writer.Put(this.rotation);
    }

    // Token: 0x06000154 RID: 340 RVA: 0x00006C40 File Offset: 0x00004E40
    public override void Deserialize(NetDataReader reader)
    {
        this.colorType = (ColorType)reader.GetVarInt();
        this.sliderType = (SliderData.Type)reader.GetVarInt();
        this.hasHeadNote = reader.GetBool();
        this.headTime = reader.GetFloat();
        this.headLineIndex = reader.GetVarInt();
        this.headLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.headBeforeJumpLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.headControlPointLengthMultiplier = reader.GetFloat();
        this.headCutDirection = (NoteCutDirection)reader.GetVarInt();
        this.headCutDirectionAngleOffset = reader.GetFloat();
        this.hasTailNote = reader.GetBool();
        this.tailTime = reader.GetFloat();
        this.tailLineIndex = reader.GetVarInt();
        this.tailLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.tailBeforeJumpLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.tailControlPointLengthMultiplier = reader.GetFloat();
        this.tailCutDirection = (NoteCutDirection)reader.GetVarInt();
        this.tailCutDirectionAngleOffset = reader.GetFloat();
        this.midAnchorMode = (SliderMidAnchorMode)reader.GetVarInt();
        this.sliceCount = reader.GetVarInt();
        this.squishAmount = reader.GetFloat();
        this.headMoveStartPos.Deserialize(reader);
        this.headJumpStartPos.Deserialize(reader);
        this.headJumpEndPos.Deserialize(reader);
        this.headJumpGravity = reader.GetFloat();
        this.tailMoveStartPos.Deserialize(reader);
        this.tailJumpStartPos.Deserialize(reader);
        this.tailJumpEndPos.Deserialize(reader);
        this.tailJumpGravity = reader.GetFloat();
        this.moveDuration = reader.GetFloat();
        this.jumpDuration = reader.GetFloat();
        this.rotation = reader.GetFloat();
    }

    // Token: 0x04000105 RID: 261
    public ColorType colorType;

    // Token: 0x04000106 RID: 262
    public SliderData.Type sliderType;

    // Token: 0x04000107 RID: 263
    public bool hasHeadNote;

    // Token: 0x04000108 RID: 264
    public float headTime;

    // Token: 0x04000109 RID: 265
    public int headLineIndex;

    // Token: 0x0400010A RID: 266
    public NoteLineLayer headLineLayer;

    // Token: 0x0400010B RID: 267
    public NoteLineLayer headBeforeJumpLineLayer;

    // Token: 0x0400010C RID: 268
    public float headControlPointLengthMultiplier;

    // Token: 0x0400010D RID: 269
    public NoteCutDirection headCutDirection;

    // Token: 0x0400010E RID: 270
    public float headCutDirectionAngleOffset;

    // Token: 0x0400010F RID: 271
    public bool hasTailNote;

    // Token: 0x04000110 RID: 272
    public float tailTime;

    // Token: 0x04000111 RID: 273
    public int tailLineIndex;

    // Token: 0x04000112 RID: 274
    public NoteLineLayer tailLineLayer;

    // Token: 0x04000113 RID: 275
    public NoteLineLayer tailBeforeJumpLineLayer;

    // Token: 0x04000114 RID: 276
    public float tailControlPointLengthMultiplier;

    // Token: 0x04000115 RID: 277
    public NoteCutDirection tailCutDirection;

    // Token: 0x04000116 RID: 278
    public float tailCutDirectionAngleOffset;

    // Token: 0x04000117 RID: 279
    public SliderMidAnchorMode midAnchorMode;

    // Token: 0x04000118 RID: 280
    public int sliceCount;

    // Token: 0x04000119 RID: 281
    public float squishAmount;

    // Token: 0x0400011A RID: 282
    public Vector3Serializable headMoveStartPos;

    // Token: 0x0400011B RID: 283
    public Vector3Serializable headJumpStartPos;

    // Token: 0x0400011C RID: 284
    public Vector3Serializable headJumpEndPos;

    // Token: 0x0400011D RID: 285
    public float headJumpGravity;

    // Token: 0x0400011E RID: 286
    public Vector3Serializable tailMoveStartPos;

    // Token: 0x0400011F RID: 287
    public Vector3Serializable tailJumpStartPos;

    // Token: 0x04000120 RID: 288
    public Vector3Serializable tailJumpEndPos;

    // Token: 0x04000121 RID: 289
    public float tailJumpGravity;

    // Token: 0x04000122 RID: 290
    public float moveDuration;

    // Token: 0x04000123 RID: 291
    public float jumpDuration;

    // Token: 0x04000124 RID: 292
    public float rotation;
}
