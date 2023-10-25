// Decompiled with JetBrains decompiler
// Type: OnlineServices.ServerManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace OnlineServices
{
  public class ServerManager : MonoBehaviour
  {
    [Inject]
    protected readonly HTTPLeaderboardsModel _leaderboardsModel;
    protected bool _initialized;
    protected LevelScoreUploader _levelScoreUploader;
    protected PlatformOnlineServicesAvailabilityModel _platformOnlineServicesAvailabilityModel;

    public event System.Action<PlatformServicesAvailabilityInfo> platformServicesAvailabilityInfoChangedEvent;

    public event System.Action<string> scoreForLeaderboardDidUploadEvent;

    public bool initialized => this._initialized;

    [Inject]
    public virtual void Init()
    {
      this._platformOnlineServicesAvailabilityModel = new PlatformOnlineServicesAvailabilityModel();
      this._platformOnlineServicesAvailabilityModel.platformServicesAvailabilityInfoChangedEvent += (System.Action<PlatformServicesAvailabilityInfo>) (availabilityInfo =>
      {
        System.Action<PlatformServicesAvailabilityInfo> infoChangedEvent = this.platformServicesAvailabilityInfoChangedEvent;
        if (infoChangedEvent == null)
          return;
        infoChangedEvent(availabilityInfo);
      });
      this._levelScoreUploader = new LevelScoreUploader(this._leaderboardsModel, this._platformOnlineServicesAvailabilityModel);
      this._levelScoreUploader.scoreForLeaderboardDidUploadEvent += (System.Action<string>) (leaderboardId =>
      {
        System.Action<string> leaderboardDidUploadEvent = this.scoreForLeaderboardDidUploadEvent;
        if (leaderboardDidUploadEvent == null)
          return;
        leaderboardDidUploadEvent(leaderboardId);
      });
      this._initialized = true;
    }

    public virtual string GetLeaderboardId(IDifficultyBeatmap difficultyBeatmap) => !this._initialized ? (string) null : this._leaderboardsModel.GetLeaderboardId(difficultyBeatmap);

    public virtual async Task<LeaderboardEntriesResult> GetLeaderboardEntriesAsync(
      GetLeaderboardFilterData leaderboardFilterData,
      CancellationToken cancellationToken)
    {
      if (!this._initialized)
        return LeaderboardEntriesResult.notInicializedError;
      if ((await this._platformOnlineServicesAvailabilityModel.GetPlatformServicesAvailabilityInfo(cancellationToken)).availability != PlatformServicesAvailabilityInfo.OnlineServicesAvailability.Available)
        return LeaderboardEntriesResult.onlineServicesUnavailableError;
      GetLeaderboardEntriesResult leaderboardEntriesAsync = await this._leaderboardsModel.GetLeaderboardEntriesAsync(leaderboardFilterData, cancellationToken);
      if (leaderboardEntriesAsync.isError)
        return LeaderboardEntriesResult.somethingWentWrongError;
      this._levelScoreUploader.TrySendPreviouslyUnsuccessfullySentResults();
      return LeaderboardEntriesResult.FromGetLeaderboardEntriesResult(leaderboardEntriesAsync);
    }

    public virtual void SendLevelScoreResult(LevelScoreResultsData levelScoreResultsData)
    {
      if (!this._initialized)
        return;
      this._levelScoreUploader.SendLevelScoreResult(levelScoreResultsData);
    }

    [CompilerGenerated]
    public virtual void m_CInitm_Eb__12_0(PlatformServicesAvailabilityInfo availabilityInfo)
    {
      System.Action<PlatformServicesAvailabilityInfo> infoChangedEvent = this.platformServicesAvailabilityInfoChangedEvent;
      if (infoChangedEvent == null)
        return;
      infoChangedEvent(availabilityInfo);
    }

    [CompilerGenerated]
    public virtual void m_CInitm_Eb__12_1(string leaderboardId)
    {
      System.Action<string> leaderboardDidUploadEvent = this.scoreForLeaderboardDidUploadEvent;
      if (leaderboardDidUploadEvent == null)
        return;
      leaderboardDidUploadEvent(leaderboardId);
    }
  }
}
