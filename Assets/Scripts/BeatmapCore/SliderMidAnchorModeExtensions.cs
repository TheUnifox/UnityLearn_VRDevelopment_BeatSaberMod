using System;

// Token: 0x02000032 RID: 50
public static class SliderMidAnchorModeExtensions
{
    // Token: 0x060000F6 RID: 246 RVA: 0x00003D85 File Offset: 0x00001F85
    public static SliderMidAnchorMode OppositeDirection(this SliderMidAnchorMode sliderMidAnchorMode)
    {
        if (sliderMidAnchorMode == SliderMidAnchorMode.Clockwise)
        {
            return SliderMidAnchorMode.CounterClockwise;
        }
        if (sliderMidAnchorMode != SliderMidAnchorMode.CounterClockwise)
        {
            return sliderMidAnchorMode;
        }
        return SliderMidAnchorMode.Clockwise;
    }
}
