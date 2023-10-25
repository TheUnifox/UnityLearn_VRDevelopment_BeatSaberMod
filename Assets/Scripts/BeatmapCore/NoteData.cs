using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class NoteData : BeatmapObjectData
{
    // Token: 0x17000013 RID: 19
    // (get) Token: 0x06000085 RID: 133 RVA: 0x0000297D File Offset: 0x00000B7D
    public override int subtypeGroupIdentifier
    {
        get
        {
            return 0;
        }
    }

    // Token: 0x17000014 RID: 20
    // (get) Token: 0x06000086 RID: 134 RVA: 0x0000335C File Offset: 0x0000155C
    // (set) Token: 0x06000087 RID: 135 RVA: 0x00003364 File Offset: 0x00001564
    public NoteData.GameplayType gameplayType { get; private set; }

    // Token: 0x17000015 RID: 21
    // (get) Token: 0x06000088 RID: 136 RVA: 0x0000336D File Offset: 0x0000156D
    // (set) Token: 0x06000089 RID: 137 RVA: 0x00003375 File Offset: 0x00001575
    public NoteData.ScoringType scoringType { get; private set; }

    // Token: 0x17000016 RID: 22
    // (get) Token: 0x0600008A RID: 138 RVA: 0x0000337E File Offset: 0x0000157E
    // (set) Token: 0x0600008B RID: 139 RVA: 0x00003386 File Offset: 0x00001586
    public ColorType colorType { get; private set; }

    // Token: 0x17000017 RID: 23
    // (get) Token: 0x0600008C RID: 140 RVA: 0x0000338F File Offset: 0x0000158F
    // (set) Token: 0x0600008D RID: 141 RVA: 0x00003397 File Offset: 0x00001597
    public NoteCutDirection cutDirection { get; private set; }

    // Token: 0x17000018 RID: 24
    // (get) Token: 0x0600008E RID: 142 RVA: 0x000033A0 File Offset: 0x000015A0
    // (set) Token: 0x0600008F RID: 143 RVA: 0x000033A8 File Offset: 0x000015A8
    public float timeToNextColorNote { get; set; }

    // Token: 0x17000019 RID: 25
    // (get) Token: 0x06000090 RID: 144 RVA: 0x000033B1 File Offset: 0x000015B1
    // (set) Token: 0x06000091 RID: 145 RVA: 0x000033B9 File Offset: 0x000015B9
    public float timeToPrevColorNote { get; set; }

    // Token: 0x1700001A RID: 26
    // (get) Token: 0x06000092 RID: 146 RVA: 0x000033C2 File Offset: 0x000015C2
    // (set) Token: 0x06000093 RID: 147 RVA: 0x000033CA File Offset: 0x000015CA
    public int lineIndex { get; private set; }

    // Token: 0x1700001B RID: 27
    // (get) Token: 0x06000094 RID: 148 RVA: 0x000033D3 File Offset: 0x000015D3
    // (set) Token: 0x06000095 RID: 149 RVA: 0x000033DB File Offset: 0x000015DB
    public NoteLineLayer noteLineLayer { get; private set; }

    // Token: 0x1700001C RID: 28
    // (get) Token: 0x06000096 RID: 150 RVA: 0x000033E4 File Offset: 0x000015E4
    // (set) Token: 0x06000097 RID: 151 RVA: 0x000033EC File Offset: 0x000015EC
    public NoteLineLayer beforeJumpNoteLineLayer { get; private set; }

    // Token: 0x1700001D RID: 29
    // (get) Token: 0x06000098 RID: 152 RVA: 0x000033F5 File Offset: 0x000015F5
    // (set) Token: 0x06000099 RID: 153 RVA: 0x000033FD File Offset: 0x000015FD
    public int flipLineIndex { get; private set; }

    // Token: 0x1700001E RID: 30
    // (get) Token: 0x0600009A RID: 154 RVA: 0x00003406 File Offset: 0x00001606
    // (set) Token: 0x0600009B RID: 155 RVA: 0x0000340E File Offset: 0x0000160E
    public float flipYSide { get; private set; }

    // Token: 0x1700001F RID: 31
    // (get) Token: 0x0600009C RID: 156 RVA: 0x00003417 File Offset: 0x00001617
    // (set) Token: 0x0600009D RID: 157 RVA: 0x0000341F File Offset: 0x0000161F
    public float cutDirectionAngleOffset { get; private set; }

    // Token: 0x17000020 RID: 32
    // (get) Token: 0x0600009E RID: 158 RVA: 0x00003428 File Offset: 0x00001628
    // (set) Token: 0x0600009F RID: 159 RVA: 0x00003430 File Offset: 0x00001630
    public float cutSfxVolumeMultiplier { get; private set; }

    // Token: 0x060000A0 RID: 160 RVA: 0x0000343C File Offset: 0x0000163C
    public override BeatmapDataItem GetCopy()
    {
        return new NoteData(base.time, this.lineIndex, this.noteLineLayer, this.beforeJumpNoteLineLayer, this.gameplayType, this.scoringType, this.colorType, this.cutDirection, this.timeToNextColorNote, this.timeToPrevColorNote, this.flipLineIndex, this.flipYSide, this.cutDirectionAngleOffset, this.cutSfxVolumeMultiplier);
    }

    // Token: 0x060000A1 RID: 161 RVA: 0x000034A4 File Offset: 0x000016A4
    protected NoteData(float time, int lineIndex, NoteLineLayer noteLineLayer, NoteLineLayer beforeJumpNoteLineLayer, NoteData.GameplayType gameplayType, NoteData.ScoringType scoringType, ColorType colorType, NoteCutDirection cutDirection, float timeToNextColorNote, float timeToPrevColorNote, int flipLineIndex, float flipYSide, float cutDirectionAngleOffset, float cutSfxVolumeMultiplier) : base(time, NoteData.SubtypeIdentifier(colorType))
    {
        this.gameplayType = gameplayType;
        this.scoringType = scoringType;
        this.colorType = colorType;
        this.cutDirection = cutDirection;
        this.timeToNextColorNote = timeToNextColorNote;
        this.timeToPrevColorNote = timeToPrevColorNote;
        this.lineIndex = lineIndex;
        this.noteLineLayer = noteLineLayer;
        this.beforeJumpNoteLineLayer = beforeJumpNoteLineLayer;
        this.flipLineIndex = flipLineIndex;
        this.flipYSide = flipYSide;
        this.cutDirectionAngleOffset = cutDirectionAngleOffset;
        this.cutSfxVolumeMultiplier = cutSfxVolumeMultiplier;
    }

    // Token: 0x060000A2 RID: 162 RVA: 0x00003528 File Offset: 0x00001728
    public static NoteData CreateBombNoteData(float time, int lineIndex, NoteLineLayer noteLineLayer)
    {
        return new NoteData(time, lineIndex, noteLineLayer, noteLineLayer, NoteData.GameplayType.Bomb, NoteData.ScoringType.NoScore, ColorType.None, NoteCutDirection.None, 0f, 0f, lineIndex, 0f, 0f, 1f);
    }

    // Token: 0x060000A3 RID: 163 RVA: 0x00003560 File Offset: 0x00001760
    public static NoteData CreateBasicNoteData(float time, int lineIndex, NoteLineLayer noteLineLayer, ColorType colorType, NoteCutDirection cutDirection)
    {
        return new NoteData(time, lineIndex, noteLineLayer, noteLineLayer, NoteData.GameplayType.Normal, NoteData.ScoringType.Normal, colorType, cutDirection, 0f, 0f, lineIndex, 0f, 0f, 1f);
    }

    // Token: 0x060000A4 RID: 164 RVA: 0x00003598 File Offset: 0x00001798
    public static NoteData CreateBurstSliderNoteData(float time, int lineIndex, NoteLineLayer noteLineLayer, NoteLineLayer beforeJumpNoteLineLayer, ColorType colorType, NoteCutDirection cutDirection, float cutSfxVolumeMultiplier)
    {
        return new NoteData(time, lineIndex, noteLineLayer, beforeJumpNoteLineLayer, NoteData.GameplayType.BurstSliderElement, NoteData.ScoringType.BurstSliderElement, colorType, cutDirection, 0f, 0f, lineIndex, 0f, 0f, cutSfxVolumeMultiplier);
    }

    // Token: 0x060000A5 RID: 165 RVA: 0x000035CC File Offset: 0x000017CC
    public virtual NoteData CopyWith(float? time = null, int? lineIndex = null, NoteLineLayer? noteLineLayer = null, NoteLineLayer? beforeJumpNoteLineLayer = null, NoteData.GameplayType? gameplayType = null, NoteData.ScoringType? scoringType = null, ColorType? colorType = null, NoteCutDirection? cutDirection = null, float? timeToNextColorNote = null, float? timeToPrevColorNote = null, int? flipLineIndex = null, float? flipYSide = null, float? cutDirectionAngleOffset = null, float? cutSfxVolumeMultiplier = null)
    {
        return new NoteData(time ?? base.time, lineIndex ?? this.lineIndex, noteLineLayer ?? this.noteLineLayer, beforeJumpNoteLineLayer ?? this.beforeJumpNoteLineLayer, gameplayType ?? this.gameplayType, scoringType ?? this.scoringType, colorType ?? this.colorType, cutDirection ?? this.cutDirection, timeToNextColorNote ?? this.timeToNextColorNote, timeToPrevColorNote ?? this.timeToPrevColorNote, flipLineIndex ?? this.flipLineIndex, flipYSide ?? this.flipYSide, cutDirectionAngleOffset ?? this.cutDirectionAngleOffset, cutSfxVolumeMultiplier ?? this.cutSfxVolumeMultiplier);
    }

    // Token: 0x060000A6 RID: 166 RVA: 0x00003758 File Offset: 0x00001958
    public virtual void SetBeforeJumpNoteLineLayer(NoteLineLayer lineLayer)
    {
        this.beforeJumpNoteLineLayer = lineLayer;
    }

    // Token: 0x060000A7 RID: 167 RVA: 0x00003761 File Offset: 0x00001961
    public virtual void ChangeToBurstSliderHead()
    {
        this.scoringType = NoteData.ScoringType.BurstSliderHead;
        this.gameplayType = NoteData.GameplayType.BurstSliderHead;
    }

    // Token: 0x060000A8 RID: 168 RVA: 0x00003771 File Offset: 0x00001971
    public virtual void ChangeToGameNote()
    {
        this.scoringType = NoteData.ScoringType.Normal;
        this.gameplayType = NoteData.GameplayType.Normal;
    }

    // Token: 0x060000A9 RID: 169 RVA: 0x00003781 File Offset: 0x00001981
    public virtual void ChangeToSliderHead()
    {
        this.scoringType = NoteData.ScoringType.SliderHead;
    }

    // Token: 0x060000AA RID: 170 RVA: 0x0000378A File Offset: 0x0000198A
    public virtual void ChangeToSliderTail()
    {
        this.scoringType = NoteData.ScoringType.SliderTail;
    }

    // Token: 0x060000AB RID: 171 RVA: 0x00003794 File Offset: 0x00001994
    public virtual void SetNoteFlipToNote(NoteData targetNote)
    {
        this.flipLineIndex = targetNote.lineIndex;
        this.flipYSide = (float)((this.lineIndex > targetNote.lineIndex) ? 1 : -1);
        if ((this.lineIndex > targetNote.lineIndex && this.noteLineLayer < targetNote.noteLineLayer) || (this.lineIndex < targetNote.lineIndex && this.noteLineLayer > targetNote.noteLineLayer))
        {
            this.flipYSide *= -1f;
        }
    }

    // Token: 0x060000AC RID: 172 RVA: 0x00003810 File Offset: 0x00001A10
    public virtual void SetCutDirectionAngleOffset(float cutDirectionAngleOffset)
    {
        this.cutDirectionAngleOffset = cutDirectionAngleOffset;
    }

    // Token: 0x060000AD RID: 173 RVA: 0x00003819 File Offset: 0x00001A19
    public virtual void ResetNoteFlip()
    {
        this.flipLineIndex = this.lineIndex;
        this.flipYSide = 0f;
    }

    // Token: 0x060000AE RID: 174 RVA: 0x00003834 File Offset: 0x00001A34
    public override void Mirror(int lineCount)
    {
        this.lineIndex = lineCount - 1 - this.lineIndex;
        this.flipLineIndex = lineCount - 1 - this.flipLineIndex;
        this.colorType = this.colorType.Opposite();
        this.cutDirection = this.cutDirection.Mirrored();
        this.cutDirectionAngleOffset = -this.cutDirectionAngleOffset;
    }

    // Token: 0x060000AF RID: 175 RVA: 0x00003890 File Offset: 0x00001A90
    public virtual void SetNoteToAnyCutDirection()
    {
        this.cutDirection = NoteCutDirection.Any;
    }

    // Token: 0x060000B0 RID: 176 RVA: 0x00003899 File Offset: 0x00001A99
    public virtual void ChangeNoteCutDirection(NoteCutDirection newCutDirection)
    {
        this.cutDirection = newCutDirection;
    }

    // Token: 0x060000B1 RID: 177 RVA: 0x000038A2 File Offset: 0x00001AA2
    public virtual void TransformNoteAOrBToRandomType()
    {
        if (this.colorType != ColorType.ColorA && this.colorType != ColorType.ColorB)
        {
            return;
        }
        if (UnityEngine.Random.Range(0f, 1f) > 0.6f)
        {
            this.colorType = ((this.colorType == ColorType.ColorA) ? ColorType.ColorB : ColorType.ColorA);
        }
    }

    // Token: 0x060000B2 RID: 178 RVA: 0x0000236C File Offset: 0x0000056C
    public static int SubtypeIdentifier(ColorType colorType)
    {
        return (int)colorType;
    }

    // Token: 0x02000029 RID: 41
    public enum GameplayType
    {
        // Token: 0x040000AE RID: 174
        Normal,
        // Token: 0x040000AF RID: 175
        Bomb,
        // Token: 0x040000B0 RID: 176
        BurstSliderHead,
        // Token: 0x040000B1 RID: 177
        BurstSliderElement,
        // Token: 0x040000B2 RID: 178
        BurstSliderElementFill
    }

    // Token: 0x0200002A RID: 42
    public enum ScoringType
    {
        // Token: 0x040000B4 RID: 180
        Ignore = -1,
        // Token: 0x040000B5 RID: 181
        NoScore,
        // Token: 0x040000B6 RID: 182
        Normal,
        // Token: 0x040000B7 RID: 183
        SliderHead,
        // Token: 0x040000B8 RID: 184
        SliderTail,
        // Token: 0x040000B9 RID: 185
        BurstSliderHead,
        // Token: 0x040000BA RID: 186
        BurstSliderElement
    }
}
