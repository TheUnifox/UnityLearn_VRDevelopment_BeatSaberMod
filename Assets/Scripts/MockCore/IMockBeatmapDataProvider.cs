using System;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000003 RID: 3
public interface IMockBeatmapDataProvider : IDisposable
{
    // Token: 0x06000003 RID: 3
    Task<MockBeatmapData> GetBeatmapData(BeatmapIdentifierNetSerializable beatmap, CancellationToken cancellationToken);
}
