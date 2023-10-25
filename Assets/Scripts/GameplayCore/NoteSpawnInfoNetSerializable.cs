using System;
using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000028 RID: 40
public class NoteSpawnInfoNetSerializable : PoolableSerializable
{
    // Token: 0x060000C3 RID: 195 RVA: 0x00004A1D File Offset: 0x00002C1D
    public static NoteSpawnInfoNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<NoteSpawnInfoNetSerializable>();
    }

    // Token: 0x060000C4 RID: 196 RVA: 0x00004A24 File Offset: 0x00002C24
    public virtual NoteSpawnInfoNetSerializable Init(float time, int lineIndex, NoteLineLayer noteLineLayer, NoteLineLayer beforeJumpNoteLineLayer, NoteData.GameplayType gameplayType, NoteData.ScoringType scoringType, ColorType colorType, NoteCutDirection cutDirection, float timeToNextColorNote, float timeToPrevColorNote, int flipLineIndex, float flipYSide, Vector3 moveStartPos, Vector3 moveEndPos, Vector3 jumpEndPos, float jumpGravity, float moveDuration, float jumpDuration, float rotation, float cutDirectionAngleOffset, float cutSfxVolumeMultiplier)
    {
        this.time = time;
        this.lineIndex = lineIndex;
        this.noteLineLayer = noteLineLayer;
        this.beforeJumpNoteLineLayer = beforeJumpNoteLineLayer;
        this.gameplayType = gameplayType;
        this.scoringType = scoringType;
        this.colorType = colorType;
        this.cutDirection = cutDirection;
        this.timeToNextColorNote = timeToNextColorNote;
        this.timeToPrevColorNote = timeToPrevColorNote;
        this.flipLineIndex = flipLineIndex;
        this.flipYSide = flipYSide;
        this.moveStartPos = moveStartPos;
        this.moveEndPos = moveEndPos;
        this.jumpEndPos = jumpEndPos;
        this.jumpGravity = jumpGravity;
        this.moveDuration = moveDuration;
        this.jumpDuration = jumpDuration;
        this.rotation = rotation;
        this.cutDirectionAngleOffset = cutDirectionAngleOffset;
        this.cutSfxVolumeMultiplier = cutSfxVolumeMultiplier;
        return this;
    }

    // Token: 0x060000C5 RID: 197 RVA: 0x00004823 File Offset: 0x00002A23
    [Preserve]
    public NoteSpawnInfoNetSerializable()
    {
    }

    // Token: 0x060000C6 RID: 198 RVA: 0x00004AE8 File Offset: 0x00002CE8
    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(this.time);
        writer.PutVarInt(this.lineIndex);
        writer.PutVarInt((int)this.noteLineLayer);
        writer.PutVarInt((int)this.beforeJumpNoteLineLayer);
        writer.PutVarInt((int)this.gameplayType);
        writer.PutVarInt((int)this.scoringType);
        writer.PutVarInt((int)this.colorType);
        writer.PutVarInt((int)this.cutDirection);
        writer.Put(this.timeToNextColorNote);
        writer.Put(this.timeToPrevColorNote);
        writer.PutVarInt(this.flipLineIndex);
        writer.PutVarInt((int)this.flipYSide);
        this.moveStartPos.Serialize(writer);
        this.moveEndPos.Serialize(writer);
        this.jumpEndPos.Serialize(writer);
        writer.Put(this.jumpGravity);
        writer.Put(this.moveDuration);
        writer.Put(this.jumpDuration);
        writer.Put(this.rotation);
        writer.Put(this.cutDirectionAngleOffset);
        writer.Put(this.cutSfxVolumeMultiplier);
    }

    // Token: 0x060000C7 RID: 199 RVA: 0x00004BF4 File Offset: 0x00002DF4
    public override void Deserialize(NetDataReader reader)
    {
        this.time = reader.GetFloat();
        this.lineIndex = reader.GetVarInt();
        this.noteLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.beforeJumpNoteLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.gameplayType = (NoteData.GameplayType)reader.GetVarInt();
        this.scoringType = (NoteData.ScoringType)reader.GetVarInt();
        this.colorType = (ColorType)reader.GetVarInt();
        this.cutDirection = (NoteCutDirection)reader.GetVarInt();
        this.timeToNextColorNote = reader.GetFloat();
        this.timeToPrevColorNote = reader.GetFloat();
        this.flipLineIndex = reader.GetVarInt();
        this.flipYSide = (float)reader.GetVarInt();
        this.moveStartPos.Deserialize(reader);
        this.moveEndPos.Deserialize(reader);
        this.jumpEndPos.Deserialize(reader);
        this.jumpGravity = reader.GetFloat();
        this.moveDuration = reader.GetFloat();
        this.jumpDuration = reader.GetFloat();
        this.rotation = reader.GetFloat();
        this.cutDirectionAngleOffset = reader.GetFloat();
        this.cutSfxVolumeMultiplier = reader.GetFloat();
    }

    // Token: 0x040000B1 RID: 177
    public float time;

    // Token: 0x040000B2 RID: 178
    public int lineIndex;

    // Token: 0x040000B3 RID: 179
    public NoteLineLayer noteLineLayer;

    // Token: 0x040000B4 RID: 180
    public NoteLineLayer beforeJumpNoteLineLayer;

    // Token: 0x040000B5 RID: 181
    public NoteData.GameplayType gameplayType;

    // Token: 0x040000B6 RID: 182
    public NoteData.ScoringType scoringType;

    // Token: 0x040000B7 RID: 183
    public ColorType colorType;

    // Token: 0x040000B8 RID: 184
    public NoteCutDirection cutDirection;

    // Token: 0x040000B9 RID: 185
    public float timeToNextColorNote;

    // Token: 0x040000BA RID: 186
    public float timeToPrevColorNote;

    // Token: 0x040000BB RID: 187
    public int flipLineIndex;

    // Token: 0x040000BC RID: 188
    public float flipYSide;

    // Token: 0x040000BD RID: 189
    public Vector3Serializable moveStartPos;

    // Token: 0x040000BE RID: 190
    public Vector3Serializable moveEndPos;

    // Token: 0x040000BF RID: 191
    public Vector3Serializable jumpEndPos;

    // Token: 0x040000C0 RID: 192
    public float jumpGravity;

    // Token: 0x040000C1 RID: 193
    public float moveDuration;

    // Token: 0x040000C2 RID: 194
    public float jumpDuration;

    // Token: 0x040000C3 RID: 195
    public float rotation;

    // Token: 0x040000C4 RID: 196
    public float cutDirectionAngleOffset;

    // Token: 0x040000C5 RID: 197
    public float cutSfxVolumeMultiplier;
}
