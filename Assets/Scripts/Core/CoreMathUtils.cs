using System;

// Token: 0x02000002 RID: 2
public class CoreMathUtils
{
    // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    public static float CalculateHalfJumpDurationInBeats(float startHalfJumpDurationInBeats, float maxHalfJumpDistance, float noteJumpMovementSpeed, float oneBeatDuration, float noteJumpStartBeatOffset)
    {
        float num = startHalfJumpDurationInBeats;
        float num2 = noteJumpMovementSpeed * oneBeatDuration;
        float num3 = num2 * num;
        maxHalfJumpDistance -= 0.001f;
        while (num3 > maxHalfJumpDistance)
        {
            num /= 2f;
            num3 = num2 * num;
        }
        num += noteJumpStartBeatOffset;
        if (num < 0.25f)
        {
            num = 0.25f;
        }
        return num;
    }

    // Token: 0x06000002 RID: 2 RVA: 0x00002098 File Offset: 0x00000298
    [Obsolete]
    public static float __CalculateHalfJumpDurationInBeatsV2(float startHalfJumpDurationInBeats, float maxHalfJumpDistance, float noteJumpMovementSpeed, float oneBeatDuration, float noteJumpStartBeatOffset)
    {
        float num = startHalfJumpDurationInBeats;
        while (noteJumpMovementSpeed * oneBeatDuration * num > maxHalfJumpDistance)
        {
            num /= 2f;
        }
        num += noteJumpStartBeatOffset;
        if (num < 1f)
        {
            num = 1f;
        }
        return num;
    }

    // Token: 0x06000003 RID: 3 RVA: 0x000020D0 File Offset: 0x000002D0
    [Obsolete]
    public static int __CalculateHalfJumpDurationInBeatsV1(float startHalfJumpDurationInBeats, float maxHalfJumpDistance, float noteJumpMovementSpeed, float oneBeatDuration, float minHalfJumpDistance)
    {
        int num = (int)startHalfJumpDurationInBeats;
        while (noteJumpMovementSpeed * oneBeatDuration * (float)num > maxHalfJumpDistance)
        {
            num--;
        }
        while (noteJumpMovementSpeed * oneBeatDuration * (float)num < minHalfJumpDistance)
        {
            num++;
        }
        return num;
    }

    // Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
    public CoreMathUtils()
    {
    }

    // Token: 0x04000001 RID: 1
    protected const float kHalfJumpDistanceEpsilon = 0.001f;
}
