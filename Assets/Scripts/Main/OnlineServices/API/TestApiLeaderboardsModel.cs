// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.TestApiLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace OnlineServices.API
{
  public class TestApiLeaderboardsModel : IApiLeaderboardsModel
  {
    public virtual async void LogoutAsync() => await Task.Delay(10);

    public virtual async Task<ApiResponse<LeaderboardEntries>> GetLeaderboardEntriesAsync(
      LeaderboardQuery leaderboardQueryDTO,
      CancellationToken cancellationToken)
    {
            BeatSaberAPI.DataTransferObjects.LeaderboardEntry[] leaderboardEntryArray = new BeatSaberAPI.DataTransferObjects.LeaderboardEntry[10];
      int num = Random.Range(0, 10);
      for (int index = 0; index < leaderboardEntryArray.Length; ++index)
      {
        if (num != index)
        {
          string str = " P " + (object) Random.Range(100000, 999999);
        }
        leaderboardEntryArray[index] = new BeatSaberAPI.DataTransferObjects.LeaderboardEntry()
        {
          id = index
        };
      }
      return await Task.FromResult<ApiResponse<LeaderboardEntries>>(new ApiResponse<LeaderboardEntries>(Response.Success, new LeaderboardEntries()
      {
        entries = leaderboardEntryArray
      }));
    }

    public virtual async Task<Response> SendLevelScoreResultAsync(
      LevelScoreResult levelScoreResult,
      CancellationToken cancellationToken)
    {
      return await Task.FromResult<Response>(Response.UnknownError);
    }
  }
}
