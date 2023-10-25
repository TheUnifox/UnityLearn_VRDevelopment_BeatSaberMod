using System;
using System.Collections.Generic;

// Token: 0x02000021 RID: 33
public class BeatmapLineData : IReadonlyBeatmapLineData
{
    // Token: 0x17000012 RID: 18
    // (get) Token: 0x06000075 RID: 117 RVA: 0x00002FEE File Offset: 0x000011EE
    public IReadOnlyList<BeatmapObjectData> beatmapObjectsData
    {
        get
        {
            return this._beatmapObjectsData;
        }
    }

    // Token: 0x06000076 RID: 118 RVA: 0x00002FF6 File Offset: 0x000011F6
    public BeatmapLineData(int initialCapacity)
    {
        this._beatmapObjectsData = new List<BeatmapObjectData>(initialCapacity);
    }

    // Token: 0x06000077 RID: 119 RVA: 0x0000300A File Offset: 0x0000120A
    public BeatmapLineData(List<BeatmapObjectData> beatmapObjectData)
    {
        this._beatmapObjectsData = beatmapObjectData;
    }

    // Token: 0x06000078 RID: 120 RVA: 0x00003019 File Offset: 0x00001219
    public virtual void AddBeatmapObjectData(BeatmapObjectData beatmapObjectData)
    {
        this._beatmapObjectsData.Add(beatmapObjectData);
    }

    // Token: 0x0400008B RID: 139
    protected readonly List<BeatmapObjectData> _beatmapObjectsData;
}
