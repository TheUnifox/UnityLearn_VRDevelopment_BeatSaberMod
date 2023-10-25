// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.HTTPAdminLeaderboardsHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace OnlineServices.API
{
  public class HTTPAdminLeaderboardsHelper
  {
    protected string _secret;
    protected UriBuilder _uriBuilder;

    public HTTPAdminLeaderboardsHelper(string uri, string secret)
    {
      this._uriBuilder = new UriBuilder(uri);
      this._secret = secret;
    }

    public virtual async Task<string> ServerStatus(CancellationToken cancellationToken)
    {
      this._uriBuilder.Path = "/Admin/ServerStatus";
      this._uriBuilder.Query = "?Secret=" + UnityWebRequest.EscapeURL(this._secret);
      return await this.SendWebRequestAsync(this._uriBuilder.Uri.ToString(), "GET", (string) null, cancellationToken);
    }

    public virtual async Task<HTTPAdminLeaderboardsHelper.LeaderboardsInfoResultDto> LeaderboardsExist(
      string[] leaderboardIds,
      CancellationToken cancellationToken)
    {
      this._uriBuilder.Path = "/Admin/LeaderboardsExist";
      this._uriBuilder.Query = "?Secret=" + UnityWebRequest.EscapeURL(this._secret);
      return JsonUtility.FromJson<HTTPAdminLeaderboardsHelper.LeaderboardsInfoResultDto>(await this.SendWebRequestAsync(this._uriBuilder.Uri.ToString(), "POST", JsonUtility.ToJson((object) new HTTPAdminLeaderboardsHelper.LeaderboardsIdsDto()
      {
        leaderboardsIds = leaderboardIds
      }), cancellationToken));
    }

    public virtual async Task<bool> CreateOrUpdateLeaderboards(
      string[] leaderboardIds,
      CancellationToken cancellationToken)
    {
      this._uriBuilder.Path = "/Admin/CreateOrUpdateLeaderboards";
      this._uriBuilder.Query = "?Secret=" + UnityWebRequest.EscapeURL(this._secret);
      string str = this._uriBuilder.Uri.ToString();
      Debug.Log((object) str);
      string json = JsonUtility.ToJson((object) new HTTPAdminLeaderboardsHelper.LeaderboardsIdsDto()
      {
        leaderboardsIds = leaderboardIds
      });
      Debug.Log((object) json);
      string message = await this.SendWebRequestAsync(str, "POST", json, cancellationToken);
      bool result = false;
      Debug.Log((object) message);
      bool.TryParse(message, out result);
      return result;
    }

    public virtual async Task<string> SendWebRequestAsync(
      string uri,
      string method,
      string bodyData,
      CancellationToken cancellationToken)
    {
      using (UnityWebRequest webRequest = new UnityWebRequest(uri, method))
      {
        if (bodyData != null)
        {
          webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData));
          webRequest.SetRequestHeader("Content-Type", "application/json");
        }
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        await this.SendAndWaitAsync(webRequest, cancellationToken);
        if (!webRequest.isNetworkError && !webRequest.isHttpError)
          return webRequest.downloadHandler.text;
        if (webRequest.responseCode == 401L)
          Debug.Log((object) "Unauthorized");
        Debug.Log((object) webRequest.downloadHandler.text);
        Debug.Log((object) webRequest.error);
        return (string) null;
      }
    }

    public virtual async Task SendAndWaitAsync(
      UnityWebRequest webRequest,
      CancellationToken cancellationToken)
    {
      AsyncOperation asyncOperation = (AsyncOperation) webRequest.SendWebRequest();
      while (!asyncOperation.isDone)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          webRequest.Abort();
          throw new OperationCanceledException(cancellationToken);
        }
        await Task.Delay(100);
      }
    }

    [Serializable]
    public class LeaderboardsIdsDto
    {
      public string[] leaderboardsIds;
    }

    [Serializable]
    public class ServerStatusResultDto
    {
      public bool everythingOK;
    }

    [Serializable]
    public class LeaderboardsInfoDto
    {
      public bool exist;
    }

    [Serializable]
    public class LeaderboardsInfoResultDto
    {
      public HTTPAdminLeaderboardsHelper.LeaderboardsInfoDto[] leaderboardsInfos;
    }
  }
}
