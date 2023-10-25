using System;

// Token: 0x0200001A RID: 26
public interface ISaberMovementData
{
    // Token: 0x17000020 RID: 32
    // (get) Token: 0x0600008E RID: 142
    BladeMovementDataElement lastAddedData { get; }

    // Token: 0x0600008F RID: 143
    void AddDataProcessor(ISaberMovementDataProcessor dataProcessor);

    // Token: 0x06000090 RID: 144
    void RemoveDataProcessor(ISaberMovementDataProcessor dataProcessor);

    // Token: 0x06000091 RID: 145
    void RequestLastDataProcessing(ISaberMovementDataProcessor dataProcessor);

    // Token: 0x06000092 RID: 146
    float ComputeSwingRating(float overrideSegmentAngle);

    // Token: 0x06000093 RID: 147
    float ComputeSwingRating();
}
