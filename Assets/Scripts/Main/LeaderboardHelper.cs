// Decompiled with JetBrains decompiler
// Type: LeaderboardHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;

public class LeaderboardHelper
{
  protected List<string> _leaderboardIds = new List<string>(1000);

  public virtual void CreateOculusLeaderboards(
    LeaderboardIdsModelSO leaderboardIdsModel,
    BeatmapLevelCollectionSO _levelCollection,
    bool debug,
    string appToken)
  {
    if ((Object) _levelCollection != (Object) null)
    {
      foreach (IBeatmapLevel beatmapLevel in (IEnumerable<IPreviewBeatmapLevel>) _levelCollection.beatmapLevels)
      {
        foreach (IDifficultyBeatmapSet difficultyBeatmapSet in (IEnumerable<IDifficultyBeatmapSet>) beatmapLevel.beatmapLevelData.difficultyBeatmapSets)
        {
          foreach (IDifficultyBeatmap difficultyBeatmap in (IEnumerable<IDifficultyBeatmap>) difficultyBeatmapSet.difficultyBeatmaps)
          {
            string platformLeaderboardId;
            if (!leaderboardIdsModel.TryGetPlatformLeaderboardId(difficultyBeatmap, out platformLeaderboardId))
              Debug.LogError((object) ("Leaderboard id for " + difficultyBeatmap.SerializedName() + " not found"));
            else
              this._leaderboardIds.Add(platformLeaderboardId);
          }
        }
      }
    }
    if (debug)
    {
      Debug.Log((object) "LEADERBOARDS \n ----------------------");
      for (int index = this._leaderboardIds.Count - 1; index >= 0; --index)
        Debug.Log((object) (this._leaderboardIds[index] + ": POST https://graph.oculus.com/1304877726278670/leaderboards access_token=" + appToken + " api_name=" + this._leaderboardIds[index] + " sort_order=HIGHER_IS_BETTER entry_write_policy=CLIENT_AUTHORITATIVE"));
    }
    else
    {
      for (int index = this._leaderboardIds.Count - 1; index >= 0; --index)
        this.CreateOculusLeaderboard(this._leaderboardIds[index], appToken);
    }
  }

  public virtual async void CreateOculusLeaderboard(string leaderboardID, string appToken)
  {
    using (HttpClient httpClient = new HttpClient())
    {
      Debug.Log((object) ("Creating leaderboard with id: " + leaderboardID));
      using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "https://graph.oculus.com/1304877726278670/leaderboards"))
      {
        List<string> contentList = new List<string>();
        contentList.Add("access_token=" + appToken);
        contentList.Add("api_name=" + leaderboardID);
        contentList.Add("sort_order=HIGHER_IS_BETTER");
        contentList.Add("entry_write_policy=CLIENT_AUTHORITATIVE");
        request.Content = (HttpContent) new StringContent(string.Join("&", (IEnumerable<string>) contentList));
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request);
        Debug.Log((object) (leaderboardID + " has response " + (object) httpResponseMessage + "content list : " + (object) contentList));
        contentList = (List<string>) null;
      }
    }
  }
}
