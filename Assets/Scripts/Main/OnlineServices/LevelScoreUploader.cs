// Decompiled with JetBrains decompiler
// Type: OnlineServices.LevelScoreUploader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineServices
{
  public class LevelScoreUploader
  {
    protected const int kMaxUploadAttempts = 3;
    protected CancellationTokenSource _cancellationTokenSource;
    protected HTTPLeaderboardsModel _leaderboardsModel;
    protected List<LevelScoreUploader.LevelScoreResultsDataUploadInfo> _unsuccessfullySentLevelScoreResultsDataUploadInfos = new List<LevelScoreUploader.LevelScoreResultsDataUploadInfo>();
    protected List<LevelScoreUploader.LevelScoreResultsDataUploadInfo> _levelScoreResultsDataUploadInfos = new List<LevelScoreUploader.LevelScoreResultsDataUploadInfo>();
    protected PlatformOnlineServicesAvailabilityModel _platformOnlineServicesAvailabilityModel;

    public event System.Action<string> scoreForLeaderboardDidUploadEvent;

    public LevelScoreUploader(
      HTTPLeaderboardsModel leaderboardsModel,
      PlatformOnlineServicesAvailabilityModel platformOnlineServicesAvailabilityModel)
    {
      this._leaderboardsModel = leaderboardsModel;
      this._platformOnlineServicesAvailabilityModel = platformOnlineServicesAvailabilityModel;
    }

    public virtual void SendLevelScoreResult(LevelScoreResultsData levelScoreResultsData)
    {
      LevelScoreUploader.LevelScoreResultsDataUploadInfo resultsDataUploadInfo = new LevelScoreUploader.LevelScoreResultsDataUploadInfo()
      {
        levelScoreResultsData = levelScoreResultsData
      };
      if (this._cancellationTokenSource != null && this._levelScoreResultsDataUploadInfos.Count > 0)
      {
        this._levelScoreResultsDataUploadInfos.Insert(1, resultsDataUploadInfo);
      }
      else
      {
        this._levelScoreResultsDataUploadInfos.Insert(0, resultsDataUploadInfo);
        this.SendLevelScoreResultAsync();
      }
    }

    public virtual void TrySendPreviouslyUnsuccessfullySentResults()
    {
      this.AddUnsuccessfullySentResults();
      this.SendLevelScoreResultAsync();
    }

    public virtual async void SendLevelScoreResultAsync()
    {
      if (this._cancellationTokenSource != null)
        return;
      this._cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = this._cancellationTokenSource.Token;
      try
      {
        if ((await this._platformOnlineServicesAvailabilityModel.GetPlatformServicesAvailabilityInfo(cancellationToken)).availability != PlatformServicesAvailabilityInfo.OnlineServicesAvailability.Available)
          return;
        while (this._levelScoreResultsDataUploadInfos.Count > 0)
        {
          LevelScoreUploader.LevelScoreResultsDataUploadInfo levelScoreResultsDataUploadInfo = this._levelScoreResultsDataUploadInfos[0];
          this._levelScoreResultsDataUploadInfos.RemoveAt(0);
          LevelScoreResultsData levelScoreResultToUpload = levelScoreResultsDataUploadInfo.levelScoreResultsData;
          if (await this._leaderboardsModel.SendLevelScoreResultAsync(levelScoreResultToUpload, this._cancellationTokenSource.Token) == SendLeaderboardEntryResult.OK)
          {
            System.Action<string> leaderboardDidUploadEvent = this.scoreForLeaderboardDidUploadEvent;
            if (leaderboardDidUploadEvent != null)
              leaderboardDidUploadEvent(this._leaderboardsModel.GetLeaderboardId(levelScoreResultToUpload.difficultyBeatmap));
            this.AddUnsuccessfullySentResults();
          }
          else
          {
            --levelScoreResultsDataUploadInfo.uploadAttemptCountLeft;
            if (levelScoreResultsDataUploadInfo.uploadAttemptCountLeft > 0)
            {
              this._levelScoreResultsDataUploadInfos.Add(levelScoreResultsDataUploadInfo);
              await Task.Delay(Math.Max(1, 3 - levelScoreResultsDataUploadInfo.uploadAttemptCountLeft + 1) * 10 * 1000);
            }
            else
            {
              this._unsuccessfullySentLevelScoreResultsDataUploadInfos.Add(levelScoreResultsDataUploadInfo);
              levelScoreResultsDataUploadInfo.uploadAttemptCountLeft = 1;
            }
          }
          cancellationToken.ThrowIfCancellationRequested();
          levelScoreResultsDataUploadInfo = (LevelScoreUploader.LevelScoreResultsDataUploadInfo) null;
          levelScoreResultToUpload = new LevelScoreResultsData();
        }
      }
      catch (OperationCanceledException ex)
      {
      }
      finally
      {
        this._cancellationTokenSource = (CancellationTokenSource) null;
      }
    }

    public virtual void AddUnsuccessfullySentResults()
    {
      this._levelScoreResultsDataUploadInfos.AddRange((IEnumerable<LevelScoreUploader.LevelScoreResultsDataUploadInfo>) this._unsuccessfullySentLevelScoreResultsDataUploadInfos);
      this._unsuccessfullySentLevelScoreResultsDataUploadInfos.Clear();
    }

    public virtual void OnDestroy() => this._cancellationTokenSource?.Cancel();

    public class LevelScoreResultsDataUploadInfo
    {
      public LevelScoreResultsData levelScoreResultsData;
      public int uploadAttemptCountLeft = 3;
    }
  }
}
