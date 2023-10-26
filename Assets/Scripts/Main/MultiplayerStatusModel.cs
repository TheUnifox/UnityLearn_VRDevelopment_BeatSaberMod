// Decompiled with JetBrains decompiler
// Type: MultiplayerStatusModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

public class MultiplayerStatusModel : IMultiplayerStatusModel
{
  [Inject]
  protected readonly INetworkConfig _networkConfig;
  protected HttpClient _client = new HttpClient();
  protected const int kRequestTimeoutSeconds = 60;
  protected Task<MultiplayerStatusData> _request;

  [Inject]
  public virtual void Init() => this.StartRequest();

  public virtual Task<MultiplayerStatusData> GetMultiplayerStatusAsync(
    CancellationToken cancellationToken)
  {
    if (!this.IsAvailabilityTaskValid())
      this.StartRequest();
    return this._request.WithCancellation<MultiplayerStatusData>(cancellationToken);
  }

  public virtual bool IsAvailabilityTaskValid() => this._request != null && !this._request.IsCompleted && !this._request.IsCanceled && !this._request.IsFaulted;

  public virtual void StartRequest() => this._request = this.GetMultiplayerStatusAsyncInternal();

  public virtual async Task<MultiplayerStatusData> GetMultiplayerStatusAsyncInternal()
  {
    this._client.Timeout = TimeSpan.FromSeconds(60.0);
    UriBuilder uriBuilder = new UriBuilder(this._networkConfig.multiplayerStatusUrl);
    uriBuilder.Query = string.Format("access_token={0}&service_environment={1}", (object) this._networkConfig.graphAccessToken, (object) this._networkConfig.serviceEnvironment);
    string stringAsync;
    try
    {
      stringAsync = await this._client.GetStringAsync(uriBuilder.Uri);
    }
    catch (TaskCanceledException)
    {
      throw new TimeoutException(string.Format("GET request to {0} timed out", (object) uriBuilder.Uri));
    }
    try
    {
      MultiplayerStatusModel.MultiplayerStatusDataFB multiplayerStatusDataFb = JsonConvert.DeserializeObject<MultiplayerStatusModel.MultiplayerStatusDataFB>(stringAsync);
      MultiplayerStatusData statusAsyncInternal = multiplayerStatusDataFb.data == null || multiplayerStatusDataFb.data.Length == 0 ? JsonConvert.DeserializeObject<MultiplayerStatusData>(stringAsync) : multiplayerStatusDataFb.data[0];
      if (this._networkConfig.forceGameLift)
        statusAsyncInternal.useGamelift = true;
      return statusAsyncInternal;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) string.Format("Exception in MultiplayerStatusModel json loading:\n{0}", (object) ex));
      return new MultiplayerStatusData()
      {
        useGamelift = this._networkConfig.forceGameLift
      };
    }
  }

  [Preserve]
  [Serializable]
  public class MultiplayerStatusDataFB
  {
    public MultiplayerStatusData[] data;
  }
}
