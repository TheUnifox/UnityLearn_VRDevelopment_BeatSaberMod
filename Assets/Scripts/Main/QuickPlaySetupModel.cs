// Decompiled with JetBrains decompiler
// Type: QuickPlaySetupModel
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

public class QuickPlaySetupModel : IQuickPlaySetupModel
{
  [Inject]
  protected readonly INetworkConfig _networkConfig;
  protected readonly HttpClient _client = new HttpClient();
  protected const int kRequestCacheTimeoutMinutes = 5;
  protected const int kRequestTimeoutSeconds = 60;
  protected Task<QuickPlaySetupData> _request;
  protected DateTime _lastRequestTime;

  public virtual Task<QuickPlaySetupData> GetQuickPlaySetupAsync(CancellationToken cancellationToken)
  {
    if (!this.IsQuickPlaySetupTaskValid())
      this.StartRequest();
    return this._request.WithCancellation<QuickPlaySetupData>(cancellationToken);
  }

  [Inject]
  public virtual void Init() => this.StartRequest();

  public virtual void StartRequest()
  {
    this._request = this.GetQuickPlaySetupInternal();
    this._lastRequestTime = DateTime.Now;
  }

  public virtual async Task<QuickPlaySetupData> GetQuickPlaySetupInternal()
  {
    if (!this.IsUrlValid(this._networkConfig.quickPlaySetupUrl))
      return new QuickPlaySetupData();
    this._client.Timeout = TimeSpan.FromSeconds(60.0);
    UriBuilder uriBuilder = new UriBuilder(this._networkConfig.quickPlaySetupUrl);
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
      QuickPlaySetupModel.QuickPlaySetupDataFB quickPlaySetupDataFb = JsonConvert.DeserializeObject<QuickPlaySetupModel.QuickPlaySetupDataFB>(stringAsync);
      return quickPlaySetupDataFb.data == null || quickPlaySetupDataFb.data.Length == 0 ? JsonConvert.DeserializeObject<QuickPlaySetupData>(stringAsync) : quickPlaySetupDataFb.data[0];
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) string.Format("Exception in QuickPlaySetupModel json loading:\n{0}", (object) ex));
      return new QuickPlaySetupData();
    }
  }

  public virtual bool IsQuickPlaySetupTaskValid() => this._request != null && this._lastRequestTime >= DateTime.UtcNow - TimeSpan.FromMinutes(5.0) && !this._request.IsCanceled && !this._request.IsFaulted;

  public virtual bool IsUrlValid(string url) => Uri.IsWellFormedUriString(url, UriKind.Absolute);

  [Preserve]
  [Serializable]
  public class QuickPlaySetupDataFB
  {
    public QuickPlaySetupData[] data;
  }
}
