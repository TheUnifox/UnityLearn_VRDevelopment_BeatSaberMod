using System;
using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000029 RID: 41
public class ObstacleSpawnInfoNetSerializable : PoolableSerializable
{
    // Token: 0x060000C8 RID: 200 RVA: 0x00004CFE File Offset: 0x00002EFE
    public static ObstacleSpawnInfoNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<ObstacleSpawnInfoNetSerializable>();
    }

    // Token: 0x060000C9 RID: 201 RVA: 0x00004D08 File Offset: 0x00002F08
    public virtual ObstacleSpawnInfoNetSerializable Init(float time, int lineIndex, NoteLineLayer lineLayer, float duration, int width, int height, Vector3 moveStartPos, Vector3 moveEndPos, Vector3 jumpEndPos, float obstacleHeight, float moveDuration, float jumpDuration, float noteLinesDistance, float rotation)
    {
        this.time = time;
        this.lineIndex = lineIndex;
        this.lineLayer = lineLayer;
        this.duration = duration;
        this.width = width;
        this.height = height;
        this.moveStartPos = moveStartPos;
        this.moveEndPos = moveEndPos;
        this.jumpEndPos = jumpEndPos;
        this.obstacleHeight = obstacleHeight;
        this.moveDuration = moveDuration;
        this.jumpDuration = jumpDuration;
        this.noteLinesDistance = noteLinesDistance;
        this.rotation = rotation;
        return this;
    }

    // Token: 0x060000CA RID: 202 RVA: 0x00004823 File Offset: 0x00002A23
    [Preserve]
    public ObstacleSpawnInfoNetSerializable()
    {
    }

    // Token: 0x060000CB RID: 203 RVA: 0x00004D94 File Offset: 0x00002F94
    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(this.time);
        writer.PutVarInt(this.lineIndex);
        writer.PutVarInt((int)this.lineLayer);
        writer.Put(this.duration);
        writer.PutVarInt(this.width);
        writer.PutVarInt(this.height);
        this.moveStartPos.Serialize(writer);
        this.moveEndPos.Serialize(writer);
        this.jumpEndPos.Serialize(writer);
        writer.Put(this.obstacleHeight);
        writer.Put(this.moveDuration);
        writer.Put(this.jumpDuration);
        writer.Put(this.noteLinesDistance);
        writer.Put(this.rotation);
    }

    // Token: 0x060000CC RID: 204 RVA: 0x00004E4C File Offset: 0x0000304C
    public override void Deserialize(NetDataReader reader)
    {
        this.time = reader.GetFloat();
        this.lineIndex = reader.GetVarInt();
        this.lineLayer = (NoteLineLayer)reader.GetVarInt();
        this.duration = reader.GetFloat();
        this.width = reader.GetVarInt();
        this.height = reader.GetVarInt();
        this.moveStartPos.Deserialize(reader);
        this.moveEndPos.Deserialize(reader);
        this.jumpEndPos.Deserialize(reader);
        this.obstacleHeight = reader.GetFloat();
        this.moveDuration = reader.GetFloat();
        this.jumpDuration = reader.GetFloat();
        this.noteLinesDistance = reader.GetFloat();
        this.rotation = reader.GetFloat();
    }

    // Token: 0x040000C6 RID: 198
    public float time;

    // Token: 0x040000C7 RID: 199
    public int lineIndex;

    // Token: 0x040000C8 RID: 200
    public NoteLineLayer lineLayer;

    // Token: 0x040000C9 RID: 201
    public float duration;

    // Token: 0x040000CA RID: 202
    public int width;

    // Token: 0x040000CB RID: 203
    public int height;

    // Token: 0x040000CC RID: 204
    public Vector3Serializable moveStartPos;

    // Token: 0x040000CD RID: 205
    public Vector3Serializable moveEndPos;

    // Token: 0x040000CE RID: 206
    public Vector3Serializable jumpEndPos;

    // Token: 0x040000CF RID: 207
    public float obstacleHeight;

    // Token: 0x040000D0 RID: 208
    public float moveDuration;

    // Token: 0x040000D1 RID: 209
    public float jumpDuration;

    // Token: 0x040000D2 RID: 210
    public float noteLinesDistance;

    // Token: 0x040000D3 RID: 211
    public float rotation;
}
