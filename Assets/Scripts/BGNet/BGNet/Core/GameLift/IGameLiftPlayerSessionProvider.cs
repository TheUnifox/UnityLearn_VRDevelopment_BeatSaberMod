using System;
using System.Threading;
using System.Threading.Tasks;

namespace BGNet.Core.GameLift
{
    // Token: 0x020000C6 RID: 198
    public interface IGameLiftPlayerSessionProvider : IPollable
    {
        // Token: 0x06000701 RID: 1793
        Task<PlayerSessionInfo> GetGameLiftPlayerSessionInfo(IAuthenticationTokenProvider authenticationTokenProvider, string userId, BeatmapLevelSelectionMask beatmapLevelSelectionMask, GameplayServerConfiguration gameplayServerConfiguration, string secret, string code, CancellationToken cancellationToken);
    }
}
