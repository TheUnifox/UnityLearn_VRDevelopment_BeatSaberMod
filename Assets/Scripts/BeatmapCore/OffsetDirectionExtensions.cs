using System;

// Token: 0x0200002E RID: 46
public static class OffsetDirectionExtensions
{
    // Token: 0x060000C1 RID: 193 RVA: 0x00003202 File Offset: 0x00001402
    public static OffsetDirection OppositeDirection(this OffsetDirection offsetDirection)
    {
        switch (offsetDirection)
        {
            case OffsetDirection.Left:
                return OffsetDirection.Right;
            case OffsetDirection.Right:
                return OffsetDirection.Left;
            case OffsetDirection.UpLeft:
                return OffsetDirection.UpRight;
            case OffsetDirection.UpRight:
                return OffsetDirection.UpLeft;
            case OffsetDirection.DownLeft:
                return OffsetDirection.DownRight;
            case OffsetDirection.DownRight:
                return OffsetDirection.DownLeft;
            default:
                return offsetDirection;
        }
    }
}
