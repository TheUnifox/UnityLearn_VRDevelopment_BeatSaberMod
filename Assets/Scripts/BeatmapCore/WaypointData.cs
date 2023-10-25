using System;

// Token: 0x02000033 RID: 51
public class WaypointData : BeatmapObjectData
{
    // Token: 0x1700003B RID: 59
    // (get) Token: 0x060000F7 RID: 247 RVA: 0x00003D96 File Offset: 0x00001F96
    // (set) Token: 0x060000F8 RID: 248 RVA: 0x00003D9E File Offset: 0x00001F9E
    public OffsetDirection offsetDirection { get; private set; }

    // Token: 0x1700003C RID: 60
    // (get) Token: 0x060000F9 RID: 249 RVA: 0x00003DA7 File Offset: 0x00001FA7
    // (set) Token: 0x060000FA RID: 250 RVA: 0x00003DAF File Offset: 0x00001FAF
    public int lineIndex { get; private set; }

    // Token: 0x1700003D RID: 61
    // (get) Token: 0x060000FB RID: 251 RVA: 0x00003DB8 File Offset: 0x00001FB8
    // (set) Token: 0x060000FC RID: 252 RVA: 0x00003DC0 File Offset: 0x00001FC0
    public NoteLineLayer lineLayer { get; protected set; }

    // Token: 0x060000FD RID: 253 RVA: 0x00003DC9 File Offset: 0x00001FC9
    public override BeatmapDataItem GetCopy()
    {
        return new WaypointData(base.time, this.lineIndex, this.lineLayer, this.offsetDirection);
    }

    // Token: 0x060000FE RID: 254 RVA: 0x00003DE8 File Offset: 0x00001FE8
    public WaypointData(float time, int lineIndex, NoteLineLayer lineLayer, OffsetDirection offsetDirection) : base(time, 0)
    {
        this.offsetDirection = offsetDirection;
        this.lineIndex = lineIndex;
        this.lineLayer = lineLayer;
    }

    // Token: 0x060000FF RID: 255 RVA: 0x00003E08 File Offset: 0x00002008
    public override void Mirror(int lineCount)
    {
        this.lineIndex = lineCount - 1 - this.lineIndex;
        this.MirrorTransformOffsetDirection();
    }

    // Token: 0x06000100 RID: 256 RVA: 0x00003E20 File Offset: 0x00002020
    public virtual void MirrorTransformOffsetDirection()
    {
        switch (this.offsetDirection)
        {
            case OffsetDirection.Left:
                this.offsetDirection = OffsetDirection.Right;
                return;
            case OffsetDirection.Right:
                this.offsetDirection = OffsetDirection.Left;
                return;
            case OffsetDirection.UpLeft:
                this.offsetDirection = OffsetDirection.UpRight;
                return;
            case OffsetDirection.UpRight:
                this.offsetDirection = OffsetDirection.UpLeft;
                return;
            case OffsetDirection.DownLeft:
                this.offsetDirection = OffsetDirection.DownRight;
                return;
            case OffsetDirection.DownRight:
                this.offsetDirection = OffsetDirection.DownLeft;
                return;
            default:
                return;
        }
    }
}
