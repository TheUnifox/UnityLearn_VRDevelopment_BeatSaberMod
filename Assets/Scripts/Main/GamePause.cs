// Decompiled with JetBrains decompiler
// Type: GamePause
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class GamePause : IGamePause
{
  [Inject]
  protected GameEnergyCounter _gameEnergyCounter;
  [Inject]
  protected PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
  [Inject]
  protected IScoreController _scoreController;
  [Inject]
  protected BeatmapObjectExecutionRatingsRecorder _beatmapObjectExecutionRatingsRecorder;
  [Inject]
  protected SongController _songController;
  [Inject]
  protected SaberManager _saberManager;
  [Inject]
  protected AudioListenerController _audioListenerController;
  protected bool _pause;

  public bool isPaused => this._pause;

  public event System.Action didPauseEvent;

  public event System.Action willResumeEvent;

  public event System.Action didResumeEvent;

  public virtual void Pause()
  {
    if (this._pause)
      return;
    this._pause = true;
    this._saberManager.disableSabers = true;
    this._gameEnergyCounter.enabled = false;
    this._playerHeadAndObstacleInteraction.enabled = false;
    this._scoreController.SetEnabled(false);
    this._beatmapObjectExecutionRatingsRecorder.enabled = false;
    this._audioListenerController.Pause();
    this._songController.PauseSong();
    System.Action didPauseEvent = this.didPauseEvent;
    if (didPauseEvent == null)
      return;
    didPauseEvent();
  }

  public virtual void WillResume()
  {
    if (!this._pause)
      return;
    System.Action willResumeEvent = this.willResumeEvent;
    if (willResumeEvent == null)
      return;
    willResumeEvent();
  }

  public virtual void Resume()
  {
    if (!this._pause)
      return;
    this._pause = false;
    this._saberManager.disableSabers = false;
    this._gameEnergyCounter.enabled = true;
    this._playerHeadAndObstacleInteraction.enabled = true;
    this._scoreController.SetEnabled(true);
    this._beatmapObjectExecutionRatingsRecorder.enabled = true;
    this._audioListenerController.Resume();
    this._songController.ResumeSong();
    System.Action didResumeEvent = this.didResumeEvent;
    if (didResumeEvent == null)
      return;
    didResumeEvent();
  }
}
