using System;

// Token: 0x0200002C RID: 44
public class ObstacleData : BeatmapObjectData
{
    // Token: 0x17000021 RID: 33
    // (get) Token: 0x060000B3 RID: 179 RVA: 0x000038DE File Offset: 0x00001ADE
    // (set) Token: 0x060000B4 RID: 180 RVA: 0x000038E6 File Offset: 0x00001AE6
    public int lineIndex { get; private set; }

    // Token: 0x17000022 RID: 34
    // (get) Token: 0x060000B5 RID: 181 RVA: 0x000038EF File Offset: 0x00001AEF
    // (set) Token: 0x060000B6 RID: 182 RVA: 0x000038F7 File Offset: 0x00001AF7
    public NoteLineLayer lineLayer { get; protected set; }

    // Token: 0x17000023 RID: 35
    // (get) Token: 0x060000B7 RID: 183 RVA: 0x00003900 File Offset: 0x00001B00
    // (set) Token: 0x060000B8 RID: 184 RVA: 0x00003908 File Offset: 0x00001B08
    public float duration { get; private set; }

    // Token: 0x17000024 RID: 36
    // (get) Token: 0x060000B9 RID: 185 RVA: 0x00003911 File Offset: 0x00001B11
    // (set) Token: 0x060000BA RID: 186 RVA: 0x00003919 File Offset: 0x00001B19
    public int width { get; private set; }

    // Token: 0x17000025 RID: 37
    // (get) Token: 0x060000BB RID: 187 RVA: 0x00003922 File Offset: 0x00001B22
    // (set) Token: 0x060000BC RID: 188 RVA: 0x0000392A File Offset: 0x00001B2A
    public int height { get; private set; }

    // Token: 0x060000BD RID: 189 RVA: 0x00003933 File Offset: 0x00001B33
    public ObstacleData(float time, int lineIndex, NoteLineLayer lineLayer, float duration, int width, int height) : base(time, 0)
    {
        this.duration = duration;
        this.lineIndex = lineIndex;
        this.lineLayer = lineLayer;
        this.width = width;
        this.height = height;
    }

    // Token: 0x060000BE RID: 190 RVA: 0x00003963 File Offset: 0x00001B63
    public virtual void UpdateDuration(float duration)
    {
        this.duration = duration;
    }

    // Token: 0x060000BF RID: 191 RVA: 0x0000396C File Offset: 0x00001B6C
    public override BeatmapDataItem GetCopy()
    {
        return new ObstacleData(base.time, this.lineIndex, this.lineLayer, this.duration, this.width, this.height);
    }

    // Token: 0x060000C0 RID: 192 RVA: 0x00003997 File Offset: 0x00001B97
    public override void Mirror(int lineCount)
    {
        this.lineIndex = lineCount - this.width - this.lineIndex;
    }
}