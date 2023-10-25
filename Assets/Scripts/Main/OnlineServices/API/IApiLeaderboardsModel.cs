// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.IApiLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineServices.API
{
  public interface IApiLeaderboardsModel
  {
    Task<ApiResponse<LeaderboardEntries>> GetLeaderboardEntriesAsync(
      LeaderboardQuery leaderboardQueryDTO,
      CancellationToken cancellationToken);

    Task<Response> SendLevelScoreResultAsync(
      LevelScoreResult levelScoreResultDto,
      CancellationToken cancellationToken);

    void LogoutAsync();
  }
}
