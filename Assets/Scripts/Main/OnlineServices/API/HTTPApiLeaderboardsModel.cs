// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.HTTPApiLeaderboardsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using LeaderboardsDTO;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace OnlineServices.API
{
  public class HTTPApiLeaderboardsModel : IApiLeaderboardsModel
  {
    [Inject]
    protected readonly HTTPLeaderboardsOathHelper _httpLeaderboardsOathHelper;
    protected const string kSendLevelScoreResultPath = "/v1/Leaderboard/AddEntry";
    protected const string kGetLeaderboardEntriesPath = "/v1/Leaderboard/Filter";

    public virtual async void LogoutAsync() => await this._httpLeaderboardsOathHelper.LogOut();

    public virtual async Task<ApiResponse<LeaderboardEntries>> GetLeaderboardEntriesAsync(
      LeaderboardQuery leaderboardQueryDTO,
      CancellationToken cancellationToken)
    {
      try
      {
        LeaderboardEntries responseDto = JsonUtility.FromJson<LeaderboardEntries>(await this._httpLeaderboardsOathHelper.SendWebRequestWithOathAsync("/v1/Leaderboard/Filter", "POST", (object) leaderboardQueryDTO, cancellationToken));
        return new ApiResponse<LeaderboardEntries>(responseDto != null ? Response.Success : Response.UnknownError, responseDto);
      }
      catch (NullReferenceException ex)
      {
        Debug.Log((object) ("GetLeaderboardEntriesAsync exception: " + ex.Message));
        return new ApiResponse<LeaderboardEntries>(Response.UnknownError, (LeaderboardEntries) null);
      }
    }

    public virtual async Task<Response> SendLevelScoreResultAsync(
      LevelScoreResult levelScoreResult,
      CancellationToken cancellationToken)
    {
      try
      {
        return JsonUtility.FromJson<LeaderboardEntryDTO>(await this._httpLeaderboardsOathHelper.SendWebRequestWithOathAsync("/v1/Leaderboard/AddEntry", "POST", (object) levelScoreResult, cancellationToken)) != null ? Response.Success : Response.UnknownError;
      }
      catch (NullReferenceException ex)
      {
        return Response.UnknownError;
      }
    }
  }
}
