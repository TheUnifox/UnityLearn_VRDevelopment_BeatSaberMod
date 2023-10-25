using System;

// Token: 0x0200001B RID: 27
public interface ISaberMovementDataProcessor
{
    // Token: 0x06000094 RID: 148
    void ProcessNewData(BladeMovementDataElement newData, BladeMovementDataElement prevData, bool prevDataAreValid);
}
