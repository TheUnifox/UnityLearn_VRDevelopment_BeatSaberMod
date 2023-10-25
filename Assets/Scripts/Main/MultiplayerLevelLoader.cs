// Decompiled with JetBrains decompiler
// Type: MultiplayerLevelLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class MultiplayerLevelLoader : ITickable
{
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly BeatmapLevelsModel _beatmapLevelsModel;
  protected MultiplayerLevelLoader.MultiplayerBeatmapLoaderState _loaderState;
  protected CancellationTokenSource _getBeatmapCancellationTokenSource;
  protected Task<BeatmapLevelsModel.GetBeatmapLevelResult> _getBeatmapLevelResultTask;
  protected ILevelGameplaySetupData _gameplaySetupData;
  protected IDifficultyBeatmap _difficultyBeatmap;
  protected float _startTime;
  protected bool _stillDownloadingCalled;

  public event System.Action stillDownloadingSongEvent;

  public event System.Action<ILevelGameplaySetupData, IDifficultyBeatmap> countdownFinishedEvent;

  public virtual void LoadLevel(ILevelGameplaySetupData gameplaySetupData, float initialStartTime)
  {
    if (this._loaderState != MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.NotLoading)
      return;
    this._loaderState = MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.LoadingBeatmap;
    this._gameplaySetupData = gameplaySetupData;
    this._startTime = initialStartTime;
    this._getBeatmapCancellationTokenSource = new CancellationTokenSource();
    this._getBeatmapLevelResultTask = this._beatmapLevelsModel.GetBeatmapLevelAsync(gameplaySetupData.beatmapLevel.beatmapLevel.levelID, this._getBeatmapCancellationTokenSource.Token);
  }

  public virtual void SetNewStartTime(float newStartTime) => this._startTime = newStartTime;

  public virtual void ClearLoading()
  {
    this._loaderState = MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.NotLoading;
    this._gameplaySetupData = (ILevelGameplaySetupData) null;
    this._difficultyBeatmap = (IDifficultyBeatmap) null;
    this._getBeatmapLevelResultTask = (Task<BeatmapLevelsModel.GetBeatmapLevelResult>) null;
    this._stillDownloadingCalled = false;
    if (this._getBeatmapCancellationTokenSource == null)
      return;
    this._getBeatmapCancellationTokenSource.Cancel();
    this._getBeatmapCancellationTokenSource = (CancellationTokenSource) null;
  }

  public virtual void Tick()
  {
    if (this._loaderState == MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.NotLoading)
      return;
    switch (this._loaderState)
    {
      case MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.LoadingBeatmap:
        if (this._getBeatmapLevelResultTask.IsCompleted)
        {
          BeatmapLevelsModel.GetBeatmapLevelResult result = this._getBeatmapLevelResultTask.Result;
          if (!result.isError && result.beatmapLevel != null)
            this._difficultyBeatmap = result.beatmapLevel.beatmapLevelData.GetDifficultyBeatmap(this._gameplaySetupData.beatmapLevel);
          this._loaderState = MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.WaitingForCountdown;
          break;
        }
        if ((double) this._multiplayerSessionManager.syncTime < (double) this._startTime || this._stillDownloadingCalled)
          break;
        System.Action downloadingSongEvent = this.stillDownloadingSongEvent;
        if (downloadingSongEvent != null)
          downloadingSongEvent();
        this._stillDownloadingCalled = true;
        break;
      case MultiplayerLevelLoader.MultiplayerBeatmapLoaderState.WaitingForCountdown:
        if ((double) this._multiplayerSessionManager.syncTime < (double) this._startTime)
          break;
        System.Action<ILevelGameplaySetupData, IDifficultyBeatmap> countdownFinishedEvent = this.countdownFinishedEvent;
        if (countdownFinishedEvent != null)
          countdownFinishedEvent(this._gameplaySetupData, this._difficultyBeatmap);
        this.ClearLoading();
        break;
    }
  }

  public enum MultiplayerBeatmapLoaderState
  {
    NotLoading,
    LoadingBeatmap,
    WaitingForCountdown,
  }
}
