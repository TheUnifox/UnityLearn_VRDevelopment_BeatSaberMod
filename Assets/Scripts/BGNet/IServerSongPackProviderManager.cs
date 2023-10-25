using System;
using System.Threading.Tasks;

// Token: 0x02000050 RID: 80
public interface IServerSongPackProviderManager : IDisposable
{
    // Token: 0x060002F9 RID: 761
    IServerSongPackProvider GetServerSongPackProvider();

    // Token: 0x060002FA RID: 762
    Task RefreshAsync();
}
