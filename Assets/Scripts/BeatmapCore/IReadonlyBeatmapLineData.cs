using System;
using System.Collections.Generic;

// Token: 0x02000020 RID: 32
public interface IReadonlyBeatmapLineData
{
    // Token: 0x17000011 RID: 17
    // (get) Token: 0x06000074 RID: 116
    IReadOnlyList<BeatmapObjectData> beatmapObjectsData { get; }
}
