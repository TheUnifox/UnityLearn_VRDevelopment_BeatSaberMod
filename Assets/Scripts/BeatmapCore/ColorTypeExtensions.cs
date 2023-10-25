using System;

// Token: 0x02000024 RID: 36
public static class ColorTypeExtensions
{
    // Token: 0x0600007B RID: 123 RVA: 0x00003034 File Offset: 0x00001234
    public static ColorType Opposite(this ColorType colorType)
    {
        if (colorType == ColorType.ColorA)
        {
            return ColorType.ColorB;
        }
        if (colorType != ColorType.ColorB)
        {
            return colorType;
        }
        return ColorType.ColorA;
    }
}
