using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public abstract class StaticBeatmapObjectSpawnMovementData
{
    // Token: 0x06000138 RID: 312 RVA: 0x00004E0F File Offset: 0x0000300F
    public static Vector2 Get2DNoteOffset(int noteLineIndex, int noteLinesCount, NoteLineLayer noteLineLayer)
    {
        return new Vector2(((float)(-(float)(noteLinesCount - 1)) * 0.5f + (float)noteLineIndex) * 0.6f, StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer));
    }

    // Token: 0x06000139 RID: 313 RVA: 0x00004E30 File Offset: 0x00003030
    public static float LineYPosForLineLayer(NoteLineLayer lineLayer)
    {
        if (lineLayer == NoteLineLayer.Base)
        {
            return 0.25f;
        }
        if (lineLayer == NoteLineLayer.Upper)
        {
            return 0.85f;
        }
        return 1.45f;
    }

    // Token: 0x17000043 RID: 67
    // (get) Token: 0x0600013A RID: 314 RVA: 0x00004E4A File Offset: 0x0000304A
    public static float layerHeight
    {
        get
        {
            return 0.6f;
        }
    }

    // Token: 0x04000104 RID: 260
    public const float kNoteLinesDistance = 0.6f;

    // Token: 0x04000105 RID: 261
    public const float kBaseLinesYPos = 0.25f;

    // Token: 0x04000106 RID: 262
    public const float kUpperLinesYPos = 0.85f;

    // Token: 0x04000107 RID: 263
    public const float kTopLinesYPos = 1.45f;

    // Token: 0x04000108 RID: 264
    public const float kObstacleVerticalOffset = -0.15f;
}
