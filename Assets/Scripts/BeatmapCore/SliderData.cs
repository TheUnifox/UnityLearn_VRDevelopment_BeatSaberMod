using System;

// Token: 0x0200002F RID: 47
public class SliderData : BeatmapObjectData
{
    // Token: 0x17000026 RID: 38
    // (get) Token: 0x060000C2 RID: 194 RVA: 0x0000297D File Offset: 0x00000B7D
    public override int subtypeGroupIdentifier
    {
        get
        {
            return 0;
        }
    }

    // Token: 0x17000027 RID: 39
    // (get) Token: 0x060000C3 RID: 195 RVA: 0x000039AE File Offset: 0x00001BAE
    // (set) Token: 0x060000C4 RID: 196 RVA: 0x000039B6 File Offset: 0x00001BB6
    public ColorType colorType { get; private set; }

    // Token: 0x17000028 RID: 40
    // (get) Token: 0x060000C5 RID: 197 RVA: 0x000039BF File Offset: 0x00001BBF
    // (set) Token: 0x060000C6 RID: 198 RVA: 0x000039C7 File Offset: 0x00001BC7
    public SliderData.Type sliderType { get; private set; }

    // Token: 0x17000029 RID: 41
    // (get) Token: 0x060000C7 RID: 199 RVA: 0x000039D0 File Offset: 0x00001BD0
    // (set) Token: 0x060000C8 RID: 200 RVA: 0x000039D8 File Offset: 0x00001BD8
    public bool hasHeadNote { get; private set; }

    // Token: 0x1700002A RID: 42
    // (get) Token: 0x060000C9 RID: 201 RVA: 0x000039E1 File Offset: 0x00001BE1
    // (set) Token: 0x060000CA RID: 202 RVA: 0x000039E9 File Offset: 0x00001BE9
    public float headControlPointLengthMultiplier { get; private set; }

    // Token: 0x1700002B RID: 43
    // (get) Token: 0x060000CB RID: 203 RVA: 0x000039F2 File Offset: 0x00001BF2
    // (set) Token: 0x060000CC RID: 204 RVA: 0x000039FA File Offset: 0x00001BFA
    public int headLineIndex { get; private set; }

    // Token: 0x1700002C RID: 44
    // (get) Token: 0x060000CD RID: 205 RVA: 0x00003A03 File Offset: 0x00001C03
    // (set) Token: 0x060000CE RID: 206 RVA: 0x00003A0B File Offset: 0x00001C0B
    public NoteLineLayer headLineLayer { get; private set; }

    // Token: 0x1700002D RID: 45
    // (get) Token: 0x060000CF RID: 207 RVA: 0x00003A14 File Offset: 0x00001C14
    // (set) Token: 0x060000D0 RID: 208 RVA: 0x00003A1C File Offset: 0x00001C1C
    public NoteLineLayer headBeforeJumpLineLayer { get; private set; }

    // Token: 0x1700002E RID: 46
    // (get) Token: 0x060000D1 RID: 209 RVA: 0x00003A25 File Offset: 0x00001C25
    // (set) Token: 0x060000D2 RID: 210 RVA: 0x00003A2D File Offset: 0x00001C2D
    public NoteCutDirection headCutDirection { get; private set; }

    // Token: 0x1700002F RID: 47
    // (get) Token: 0x060000D3 RID: 211 RVA: 0x00003A36 File Offset: 0x00001C36
    // (set) Token: 0x060000D4 RID: 212 RVA: 0x00003A3E File Offset: 0x00001C3E
    public float headCutDirectionAngleOffset { get; private set; }

    // Token: 0x17000030 RID: 48
    // (get) Token: 0x060000D5 RID: 213 RVA: 0x00003A47 File Offset: 0x00001C47
    // (set) Token: 0x060000D6 RID: 214 RVA: 0x00003A4F File Offset: 0x00001C4F
    public bool hasTailNote { get; private set; }

    // Token: 0x17000031 RID: 49
    // (get) Token: 0x060000D7 RID: 215 RVA: 0x00003A58 File Offset: 0x00001C58
    // (set) Token: 0x060000D8 RID: 216 RVA: 0x00003A60 File Offset: 0x00001C60
    public float tailTime { get; private set; }

    // Token: 0x17000032 RID: 50
    // (get) Token: 0x060000D9 RID: 217 RVA: 0x00003A69 File Offset: 0x00001C69
    // (set) Token: 0x060000DA RID: 218 RVA: 0x00003A71 File Offset: 0x00001C71
    public int tailLineIndex { get; private set; }

    // Token: 0x17000033 RID: 51
    // (get) Token: 0x060000DB RID: 219 RVA: 0x00003A7A File Offset: 0x00001C7A
    // (set) Token: 0x060000DC RID: 220 RVA: 0x00003A82 File Offset: 0x00001C82
    public float tailControlPointLengthMultiplier { get; private set; }

    // Token: 0x17000034 RID: 52
    // (get) Token: 0x060000DD RID: 221 RVA: 0x00003A8B File Offset: 0x00001C8B
    // (set) Token: 0x060000DE RID: 222 RVA: 0x00003A93 File Offset: 0x00001C93
    public NoteLineLayer tailLineLayer { get; private set; }

    // Token: 0x17000035 RID: 53
    // (get) Token: 0x060000DF RID: 223 RVA: 0x00003A9C File Offset: 0x00001C9C
    // (set) Token: 0x060000E0 RID: 224 RVA: 0x00003AA4 File Offset: 0x00001CA4
    public NoteLineLayer tailBeforeJumpLineLayer { get; private set; }

    // Token: 0x17000036 RID: 54
    // (get) Token: 0x060000E1 RID: 225 RVA: 0x00003AAD File Offset: 0x00001CAD
    // (set) Token: 0x060000E2 RID: 226 RVA: 0x00003AB5 File Offset: 0x00001CB5
    public NoteCutDirection tailCutDirection { get; private set; }

    // Token: 0x17000037 RID: 55
    // (get) Token: 0x060000E3 RID: 227 RVA: 0x00003ABE File Offset: 0x00001CBE
    // (set) Token: 0x060000E4 RID: 228 RVA: 0x00003AC6 File Offset: 0x00001CC6
    public float tailCutDirectionAngleOffset { get; private set; }

    // Token: 0x17000038 RID: 56
    // (get) Token: 0x060000E5 RID: 229 RVA: 0x00003ACF File Offset: 0x00001CCF
    // (set) Token: 0x060000E6 RID: 230 RVA: 0x00003AD7 File Offset: 0x00001CD7
    public SliderMidAnchorMode midAnchorMode { get; private set; }

    // Token: 0x17000039 RID: 57
    // (get) Token: 0x060000E7 RID: 231 RVA: 0x00003AE0 File Offset: 0x00001CE0
    // (set) Token: 0x060000E8 RID: 232 RVA: 0x00003AE8 File Offset: 0x00001CE8
    public int sliceCount { get; private set; }

    // Token: 0x1700003A RID: 58
    // (get) Token: 0x060000E9 RID: 233 RVA: 0x00003AF1 File Offset: 0x00001CF1
    // (set) Token: 0x060000EA RID: 234 RVA: 0x00003AF9 File Offset: 0x00001CF9
    public float squishAmount { get; private set; }

    // Token: 0x060000EB RID: 235 RVA: 0x00003B04 File Offset: 0x00001D04
    public override BeatmapDataItem GetCopy()
    {
        SliderData.Type sliderType = this.sliderType;
        ColorType colorType = this.colorType;
        bool hasHeadNote = this.hasHeadNote;
        float time = base.time;
        int headLineIndex = this.headLineIndex;
        NoteLineLayer headLineLayer = this.headLineLayer;
        NoteLineLayer headBeforeJumpLineLayer = this.headBeforeJumpLineLayer;
        NoteCutDirection headCutDirection = this.headCutDirection;
        float headCutDirectionAngleOffset = this.headCutDirectionAngleOffset;
        return new SliderData(sliderType, colorType, hasHeadNote, time, headLineIndex, headLineLayer, headBeforeJumpLineLayer, this.headControlPointLengthMultiplier, headCutDirection, headCutDirectionAngleOffset, this.hasTailNote, this.tailTime, this.tailLineIndex, this.tailLineLayer, this.tailBeforeJumpLineLayer, this.tailControlPointLengthMultiplier, this.tailCutDirection, this.tailCutDirectionAngleOffset, this.midAnchorMode, this.sliceCount, this.squishAmount);
    }

    // Token: 0x060000EC RID: 236 RVA: 0x00003B98 File Offset: 0x00001D98
    public SliderData(SliderData.Type sliderType, ColorType colorType, bool hasHeadNote, float headTime, int headLineIndex, NoteLineLayer headLineLayer, NoteLineLayer headBeforeJumpLineLayer, float headControlPointLengthMultiplier, NoteCutDirection headCutDirection, float headCutDirectionAngleOffset, bool hasTailNote, float tailTime, int tailLineIndex, NoteLineLayer tailLineLayer, NoteLineLayer tailBeforeJumpLineLayer, float tailControlPointLengthMultiplier, NoteCutDirection tailCutDirection, float tailCutDirectionAngleOffset, SliderMidAnchorMode midAnchorMode, int sliceCount, float squishAmount) : base(headTime, SliderData.SubtypeIdentifier(colorType))
    {
        this.sliderType = sliderType;
        this.colorType = colorType;
        this.hasHeadNote = hasHeadNote;
        this.headLineIndex = headLineIndex;
        this.headLineLayer = headLineLayer;
        this.headBeforeJumpLineLayer = headBeforeJumpLineLayer;
        this.headControlPointLengthMultiplier = headControlPointLengthMultiplier;
        this.headCutDirection = headCutDirection;
        this.headCutDirectionAngleOffset = headCutDirectionAngleOffset;
        this.hasTailNote = hasTailNote;
        this.tailTime = tailTime;
        this.tailLineIndex = tailLineIndex;
        this.tailLineLayer = tailLineLayer;
        this.tailBeforeJumpLineLayer = tailBeforeJumpLineLayer;
        this.tailControlPointLengthMultiplier = tailControlPointLengthMultiplier;
        this.tailCutDirection = tailCutDirection;
        this.tailCutDirectionAngleOffset = tailCutDirectionAngleOffset;
        this.midAnchorMode = midAnchorMode;
        this.sliceCount = sliceCount;
        this.squishAmount = ((squishAmount < 0.001f) ? 1f : squishAmount);
    }

    // Token: 0x060000ED RID: 237 RVA: 0x00003C60 File Offset: 0x00001E60
    public static SliderData CreateSliderData(ColorType colorType, float headTime, int headLineIndex, NoteLineLayer headLineLayer, NoteLineLayer headBeforeJumpLineLayer, float headControlPointLengthMultiplier, NoteCutDirection headCutDirection, float tailTime, int tailLineIndex, NoteLineLayer tailLineLayer, NoteLineLayer tailBeforeJumpLineLayer, float tailControlPointLengthMultiplier, NoteCutDirection tailCutDirection, SliderMidAnchorMode midAnchorMode)
    {
        return new SliderData(SliderData.Type.Normal, colorType, false, headTime, headLineIndex, headLineLayer, headBeforeJumpLineLayer, headControlPointLengthMultiplier, headCutDirection, 0f, false, tailTime, tailLineIndex, tailLineLayer, tailBeforeJumpLineLayer, tailControlPointLengthMultiplier, tailCutDirection, 0f, midAnchorMode, 0, 1f);
    }

    // Token: 0x060000EE RID: 238 RVA: 0x00003CA0 File Offset: 0x00001EA0
    public static SliderData CreateBurstSliderData(ColorType colorType, float headTime, int headLineIndex, NoteLineLayer headLineLayer, NoteLineLayer headBeforeJumpLineLayer, NoteCutDirection headCutDirection, float tailTime, int tailLineIndex, NoteLineLayer tailLineLayer, NoteLineLayer tailBeforeJumpLineLayer, NoteCutDirection tailCutDirection, int sliceCount, float squishAmount)
    {
        return new SliderData(SliderData.Type.Burst, colorType, false, headTime, headLineIndex, headLineLayer, headBeforeJumpLineLayer, 0f, headCutDirection, 0f, false, tailTime, tailLineIndex, tailLineLayer, tailBeforeJumpLineLayer, 0f, tailCutDirection, 0f, SliderMidAnchorMode.Straight, sliceCount, squishAmount);
    }

    // Token: 0x060000EF RID: 239 RVA: 0x00003CE0 File Offset: 0x00001EE0
    public override void Mirror(int lineCount)
    {
        this.headLineIndex = lineCount - 1 - this.headLineIndex;
        this.tailLineIndex = lineCount - 1 - this.tailLineIndex;
        this.headCutDirection = this.headCutDirection.Mirrored();
        this.tailCutDirection = this.tailCutDirection.Mirrored();
        this.midAnchorMode = this.midAnchorMode.OppositeDirection();
        this.colorType = this.colorType.Opposite();
    }

    // Token: 0x060000F0 RID: 240 RVA: 0x00003D51 File Offset: 0x00001F51
    public virtual void SetHasHeadNote(bool hasHeadNote)
    {
        this.hasHeadNote = hasHeadNote;
    }

    // Token: 0x060000F1 RID: 241 RVA: 0x00003D5A File Offset: 0x00001F5A
    public virtual void SetHasTailNote(bool hasTailNote)
    {
        this.hasTailNote = hasTailNote;
    }

    // Token: 0x060000F2 RID: 242 RVA: 0x00003D63 File Offset: 0x00001F63
    public virtual void SetHeadBeforeJumpLineLayer(NoteLineLayer lineLayer)
    {
        this.headBeforeJumpLineLayer = lineLayer;
    }

    // Token: 0x060000F3 RID: 243 RVA: 0x00003D6C File Offset: 0x00001F6C
    public virtual void SetTailBeforeJumpLineLayer(NoteLineLayer lineLayer)
    {
        this.tailBeforeJumpLineLayer = lineLayer;
    }

    // Token: 0x060000F4 RID: 244 RVA: 0x00003D75 File Offset: 0x00001F75
    public virtual void SetCutDirectionAngleOffset(float headCutDirectionAngleOffset, float tailCutDirectionAngleOffset)
    {
        this.headCutDirectionAngleOffset = headCutDirectionAngleOffset;
        this.tailCutDirectionAngleOffset = tailCutDirectionAngleOffset;
    }

    // Token: 0x060000F5 RID: 245 RVA: 0x0000236C File Offset: 0x0000056C
    public static int SubtypeIdentifier(ColorType colorType)
    {
        return (int)colorType;
    }

    // Token: 0x02000030 RID: 48
    public enum Type
    {
        // Token: 0x040000E3 RID: 227
        Normal,
        // Token: 0x040000E4 RID: 228
        Burst
    }
}
