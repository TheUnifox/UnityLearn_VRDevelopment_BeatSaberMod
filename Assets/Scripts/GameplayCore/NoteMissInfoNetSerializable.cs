using System;
using LiteNetLib.Utils;
using UnityEngine.Scripting;

// Token: 0x02000027 RID: 39
[Preserve]
public class NoteMissInfoNetSerializable : PoolableSerializable
{
    // Token: 0x17000026 RID: 38
    // (get) Token: 0x060000B5 RID: 181 RVA: 0x0000492E File Offset: 0x00002B2E
    // (set) Token: 0x060000B6 RID: 182 RVA: 0x00004936 File Offset: 0x00002B36
    public ColorType colorType { get; private set; }

    // Token: 0x17000027 RID: 39
    // (get) Token: 0x060000B7 RID: 183 RVA: 0x0000493F File Offset: 0x00002B3F
    // (set) Token: 0x060000B8 RID: 184 RVA: 0x00004947 File Offset: 0x00002B47
    public float noteTime { get; private set; }

    // Token: 0x17000028 RID: 40
    // (get) Token: 0x060000B9 RID: 185 RVA: 0x00004950 File Offset: 0x00002B50
    // (set) Token: 0x060000BA RID: 186 RVA: 0x00004958 File Offset: 0x00002B58
    public int noteLineIndex { get; private set; }

    // Token: 0x17000029 RID: 41
    // (get) Token: 0x060000BB RID: 187 RVA: 0x00004961 File Offset: 0x00002B61
    // (set) Token: 0x060000BC RID: 188 RVA: 0x00004969 File Offset: 0x00002B69
    public NoteLineLayer noteLineLayer { get; private set; }

    // Token: 0x060000BD RID: 189 RVA: 0x00004972 File Offset: 0x00002B72
    public static NoteMissInfoNetSerializable Obtain()
    {
        return PoolableSerializable.Obtain<NoteMissInfoNetSerializable>();
    }

    // Token: 0x060000BE RID: 190 RVA: 0x00004979 File Offset: 0x00002B79
    [Preserve]
    public override void Deserialize(NetDataReader reader)
    {
        this.colorType = (ColorType)reader.GetVarInt();
        this.noteLineLayer = (NoteLineLayer)reader.GetVarInt();
        this.noteLineIndex = reader.GetVarInt();
        this.noteTime = reader.GetFloat();
    }

    // Token: 0x060000BF RID: 191 RVA: 0x000049AB File Offset: 0x00002BAB
    [Preserve]
    public override void Serialize(NetDataWriter writer)
    {
        writer.PutVarInt((int)this.colorType);
        writer.PutVarInt((int)this.noteLineLayer);
        writer.PutVarInt(this.noteLineIndex);
        writer.Put(this.noteTime);
    }

    // Token: 0x060000C0 RID: 192 RVA: 0x00004823 File Offset: 0x00002A23
    [Preserve]
    public NoteMissInfoNetSerializable()
    {
    }

    // Token: 0x060000C1 RID: 193 RVA: 0x000049DD File Offset: 0x00002BDD
    public virtual NoteMissInfoNetSerializable Init(NoteData noteData)
    {
        return this.Init(noteData.colorType, noteData.noteLineLayer, noteData.lineIndex, noteData.time);
    }

    // Token: 0x060000C2 RID: 194 RVA: 0x000049FD File Offset: 0x00002BFD
    public virtual NoteMissInfoNetSerializable Init(ColorType colorType, NoteLineLayer lineLayer, int noteLineIndex, float noteTime)
    {
        this.colorType = colorType;
        this.noteLineLayer = lineLayer;
        this.noteLineIndex = noteLineIndex;
        this.noteTime = noteTime;
        return this;
    }
}
