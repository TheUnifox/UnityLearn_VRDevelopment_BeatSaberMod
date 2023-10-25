using System;
using System.Threading.Tasks;

// Token: 0x0200004E RID: 78
public interface IServerBeatmapProviderManager : IDisposable
{
    // Token: 0x060002F6 RID: 758
    IServerBeatmapProvider GetServerBeatmapProvider();

    // Token: 0x060002F7 RID: 759
    Task RefreshAsync();
}
