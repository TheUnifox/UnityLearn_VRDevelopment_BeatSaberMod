using System;
using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000026 RID: 38
[Preserve]
public class NoteCutInfoNetSerializable : PoolableSerializable
{
    // Token: 0x060000AF RID: 175 RVA: 0x00004683 File Offset: 0x00002883
    public static NoteCutInfoNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<NoteCutInfoNetSerializable>();
    }

    // Token: 0x060000B0 RID: 176 RVA: 0x0000468C File Offset: 0x0000288C
    [Preserve]
    public override void Deserialize(NetDataReader reader)
    {
        byte @byte = reader.GetByte();
        this.cutWasOk = ((@byte & 1) > 0);
        this.saberSpeed = reader.GetFloat();
        this.saberDir.Deserialize(reader);
        this.cutPoint.Deserialize(reader);
        this.cutNormal.Deserialize(reader);
        this.notePosition.Deserialize(reader);
        this.noteScale.Deserialize(reader);
        this.noteRotation.Deserialize(reader);
        this.gameplayType = (NoteData.GameplayType)reader.GetVarInt();
        this.colorType = (ColorType)reader.GetVarInt();
        this.lineLayer = (NoteLineLayer)reader.GetVarInt();
        this.noteLineIndex = reader.GetVarInt();
        this.noteTime = reader.GetFloat();
        this.timeToNextColorNote = reader.GetFloat();
        this.moveVec.Deserialize(reader);
    }

    // Token: 0x060000B1 RID: 177 RVA: 0x00004754 File Offset: 0x00002954
    [Preserve]
    public override void Serialize(NetDataWriter writer)
    {
        byte b = 0;
        b |= (byte)(this.cutWasOk ? 1 : 0);
        writer.Put(b);
        writer.Put(this.saberSpeed);
        this.saberDir.Serialize(writer);
        this.cutPoint.Serialize(writer);
        this.cutNormal.Serialize(writer);
        this.notePosition.Serialize(writer);
        this.noteScale.Serialize(writer);
        this.noteRotation.Serialize(writer);
        writer.PutVarInt((int)this.gameplayType);
        writer.PutVarInt((int)this.colorType);
        writer.PutVarInt((int)this.lineLayer);
        writer.PutVarInt(this.noteLineIndex);
        writer.Put(this.noteTime);
        writer.Put(this.timeToNextColorNote);
        this.moveVec.Serialize(writer);
    }

    // Token: 0x060000B2 RID: 178 RVA: 0x00004823 File Offset: 0x00002A23
    [Preserve]
    public NoteCutInfoNetSerializable()
    {
    }

    // Token: 0x060000B3 RID: 179 RVA: 0x0000482C File Offset: 0x00002A2C
    public virtual NoteCutInfoNetSerializable Init(in NoteCutInfo noteCutInfo, NoteData noteData, Vector3 notePosition, Quaternion noteRotation, Vector3 noteScale, Vector3 moveVec)
    {
        return this.Init(noteCutInfo.saberSpeed, noteCutInfo.allIsOK, noteCutInfo.saberDir, noteCutInfo.cutPoint, noteCutInfo.cutNormal, noteData.gameplayType, noteData.colorType, noteData.noteLineLayer, noteData.lineIndex, noteData.time, noteData.timeToNextColorNote, notePosition, noteRotation, noteScale, moveVec);
    }

    // Token: 0x060000B4 RID: 180 RVA: 0x00004888 File Offset: 0x00002A88
    public virtual NoteCutInfoNetSerializable Init(float saberSpeed, bool cutWasOk, Vector3 saberDir, Vector3 cutPoint, Vector3 cutNormal, NoteData.GameplayType gameplayType, ColorType colorType, NoteLineLayer lineLayer, int noteLineIndex, float noteTime, float timeToNextColorNote, Vector3 notePosition, Quaternion noteRotation, Vector3 noteScale, Vector3 moveVec)
    {
        this.saberSpeed = saberSpeed;
        this.cutWasOk = cutWasOk;
        this.saberDir = saberDir;
        this.cutPoint = cutPoint;
        this.cutNormal = cutNormal;
        this.notePosition = notePosition;
        this.noteScale = noteScale;
        this.noteRotation = noteRotation;
        this.gameplayType = gameplayType;
        this.colorType = colorType;
        this.lineLayer = lineLayer;
        this.noteLineIndex = noteLineIndex;
        this.noteTime = noteTime;
        this.timeToNextColorNote = timeToNextColorNote;
        this.moveVec = moveVec;
        return this;
    }

    // Token: 0x0400009E RID: 158
    public float saberSpeed;

    // Token: 0x0400009F RID: 159
    public bool cutWasOk;

    // Token: 0x040000A0 RID: 160
    public Vector3Serializable saberDir;

    // Token: 0x040000A1 RID: 161
    public Vector3Serializable cutPoint;

    // Token: 0x040000A2 RID: 162
    public Vector3Serializable cutNormal;

    // Token: 0x040000A3 RID: 163
    public Vector3Serializable notePosition;

    // Token: 0x040000A4 RID: 164
    public Vector3Serializable noteScale;

    // Token: 0x040000A5 RID: 165
    public QuaternionSerializable noteRotation;

    // Token: 0x040000A6 RID: 166
    public NoteData.GameplayType gameplayType;

    // Token: 0x040000A7 RID: 167
    public ColorType colorType;

    // Token: 0x040000A8 RID: 168
    public float noteTime;

    // Token: 0x040000A9 RID: 169
    public int noteLineIndex;

    // Token: 0x040000AA RID: 170
    public NoteLineLayer lineLayer;

    // Token: 0x040000AB RID: 171
    public float timeToNextColorNote;

    // Token: 0x040000AC RID: 172
    public Vector3Serializable moveVec;
}
