using System;

// Token: 0x02000007 RID: 7
[Serializable]
public class MockNoteData
{
    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000016 RID: 22 RVA: 0x000021E5 File Offset: 0x000003E5
    // (set) Token: 0x06000017 RID: 23 RVA: 0x000021ED File Offset: 0x000003ED
    public float time { get; set; }

    // Token: 0x17000008 RID: 8
    // (get) Token: 0x06000018 RID: 24 RVA: 0x000021F6 File Offset: 0x000003F6
    // (set) Token: 0x06000019 RID: 25 RVA: 0x000021FE File Offset: 0x000003FE
    public int lineIndex { get; set; }

    // Token: 0x17000009 RID: 9
    // (get) Token: 0x0600001A RID: 26 RVA: 0x00002207 File Offset: 0x00000407
    // (set) Token: 0x0600001B RID: 27 RVA: 0x0000220F File Offset: 0x0000040F
    public NoteData.GameplayType gameplayType { get; set; }

    // Token: 0x1700000A RID: 10
    // (get) Token: 0x0600001C RID: 28 RVA: 0x00002218 File Offset: 0x00000418
    // (set) Token: 0x0600001D RID: 29 RVA: 0x00002220 File Offset: 0x00000420
    public ColorType colorType { get; set; }

    // Token: 0x1700000B RID: 11
    // (get) Token: 0x0600001E RID: 30 RVA: 0x00002229 File Offset: 0x00000429
    // (set) Token: 0x0600001F RID: 31 RVA: 0x00002231 File Offset: 0x00000431
    public NoteCutDirection cutDirection { get; set; }

    // Token: 0x1700000C RID: 12
    // (get) Token: 0x06000020 RID: 32 RVA: 0x0000223A File Offset: 0x0000043A
    // (set) Token: 0x06000021 RID: 33 RVA: 0x00002242 File Offset: 0x00000442
    public NoteLineLayer noteLineLayer { get; set; }

    // Token: 0x1700000D RID: 13
    // (get) Token: 0x06000022 RID: 34 RVA: 0x0000224B File Offset: 0x0000044B
    // (set) Token: 0x06000023 RID: 35 RVA: 0x00002253 File Offset: 0x00000453
    public float duration { get; set; }

    // Token: 0x06000024 RID: 36 RVA: 0x0000225C File Offset: 0x0000045C
    public virtual void Mirror(int lineCount)
    {
        this.lineIndex = lineCount - 1 - this.lineIndex;
        if (this.cutDirection == NoteCutDirection.Left)
        {
            this.cutDirection = NoteCutDirection.Right;
        }
        else if (this.cutDirection == NoteCutDirection.Right)
        {
            this.cutDirection = NoteCutDirection.Left;
        }
        if (this.colorType == ColorType.ColorA)
        {
            this.colorType = ColorType.ColorB;
            return;
        }
        if (this.colorType == ColorType.ColorB)
        {
            this.colorType = ColorType.ColorA;
        }
    }
}
