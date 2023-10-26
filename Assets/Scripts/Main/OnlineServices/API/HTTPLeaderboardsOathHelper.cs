// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.HTTPLeaderboardsOathHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BeatSaberAPI.DataTransferObjects;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace OnlineServices.API
{
  public class HTTPLeaderboardsOathHelper
  {
    [Inject]
    protected readonly IUserLoginDtoDataSource _userLoginDataSource;
    protected const string kLoginPath = "/v1/User/Register";
    protected const string kLogoutPath = "/api/v1/Account/LogOut";
    protected readonly UriBuilder _uriBuilder = new UriBuilder("https", "localhost", 5001);
    protected AccessToken _accessToken;

    public virtual async Task<string> SendWebRequestWithOathAsync(
      string path,
      string method,
      object objectToSendAsJson,
      CancellationToken cancellationToken)
    {
      await this.LoginIfNeededAsync(cancellationToken);
      if (!this.IsUserLoggedIn())
        return (string) null;
      this._uriBuilder.Path = path;
      this._uriBuilder.Query = "";
      string uri = this._uriBuilder.Uri.ToString();
      string bodyData = objectToSendAsJson == null ? (string) null : JsonUtility.ToJson(objectToSendAsJson);
      string str = await this.SendWebRequestAsync(uri, method, bodyData, this._accessToken.token);
      if (str == null && this._accessToken == null)
      {
        await this.LoginIfNeededAsync(cancellationToken);
        if (!this.IsUserLoggedIn())
          return (string) null;
        str = await this.SendWebRequestAsync(uri, method, bodyData, this._accessToken.token);
      }
      return str;
    }

    public virtual async Task LogOut()
    {
      string str = await this.SendWebRequestWithOathAsync("/api/v1/Account/LogOut", "POST", (object) null, new CancellationTokenSource().Token);
    }

    public virtual async Task LoginIfNeededAsync(CancellationToken cancellationToken)
    {
      if (this._accessToken != null)
        return;
      this._accessToken = await this.GetAccessTokenAsync(cancellationToken);
    }

    public virtual bool IsUserLoggedIn() => this._accessToken != null;

    public virtual async Task<AccessToken> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
      this._uriBuilder.Path = "/v1/User/Register";
      this._uriBuilder.Query = "";
      string uri = this._uriBuilder.Uri.ToString();
      UserAuthenticationData authenticationData = await this._userLoginDataSource.UserAuthenticationDataAsync(cancellationToken);
      if (authenticationData == null)
        return (AccessToken) null;
      string json = JsonUtility.ToJson((object) authenticationData);
      try
      {
        return JsonUtility.FromJson<UserAuthenticationResult>(await this.SendWebRequestAsync(uri, "POST", json, (string) null)).accessToken;
      }
      catch (NullReferenceException)
      {
        return (AccessToken) null;
      }
    }

    public virtual async Task<string> SendWebRequestAsync(
      string uri,
      string method,
      string bodyData,
      string bearerToken)
    {
      HTTPLeaderboardsOathHelper leaderboardsOathHelper = this;
      string text;
      using (UnityWebRequest webRequest = new UnityWebRequest(uri, method))
      {
        if (bearerToken != null)
          webRequest.SetRequestHeader("Authorization", "Bearer " + bearerToken);
        if (bodyData != null)
        {
          webRequest.uploadHandler = (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData));
          webRequest.SetRequestHeader("Content-Type", "application/json");
        }
        webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        UnityWebRequestAsyncOperation requestAsyncOperation = webRequest.SendWebRequest();
        TaskCompletionSource<bool> taskComplitionSource = new TaskCompletionSource<bool>();
        Action<AsyncOperation> action = (Action<AsyncOperation>) (asyncOperation2 =>
        {
          if ((webRequest.result == UnityWebRequest.Result.ConnectionError) || (webRequest.result == UnityWebRequest.Result.ProtocolError))
          {
            if (webRequest.responseCode == 401L)
              this._accessToken = (AccessToken) null;
            taskComplitionSource.TrySetResult(false);
          }
          taskComplitionSource.TrySetResult(true);
        });
        requestAsyncOperation.completed += action;
        text = await taskComplitionSource.Task ? webRequest.downloadHandler.text : (string) null;
      }
      return text;
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
        await Task.Delay(1000);
      }
    }

    [Conditional("HTTPLeaderboardsOathHelperLog")]
    public static void Log(string message) => UnityEngine.Debug.Log((object) message);
  }
}
