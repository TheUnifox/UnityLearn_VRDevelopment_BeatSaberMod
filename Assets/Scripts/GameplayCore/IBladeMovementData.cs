using System;

// Token: 0x02000018 RID: 24
public interface IBladeMovementData
{
    // Token: 0x1700001D RID: 29
    // (get) Token: 0x06000089 RID: 137
    float bladeSpeed { get; }

    // Token: 0x1700001E RID: 30
    // (get) Token: 0x0600008A RID: 138
    BladeMovementDataElement lastAddedData { get; }

    // Token: 0x1700001F RID: 31
    // (get) Token: 0x0600008B RID: 139
    BladeMovementDataElement prevAddedData { get; }
}
