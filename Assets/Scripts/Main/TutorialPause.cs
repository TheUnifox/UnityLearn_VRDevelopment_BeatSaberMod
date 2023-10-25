// Decompiled with JetBrains decompiler
// Type: TutorialPause
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class TutorialPause : IGamePause
{
  [Inject]
  protected readonly TutorialSongController _tutorialSongController;
  [Inject]
  protected readonly SaberManager _saberManager;
  [Inject]
  protected readonly AudioListenerController _audioListenerController;
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
    this._audioListenerController.Pause();
    this._tutorialSongController.PauseSong();
    System.Action didPauseEvent = this.didPauseEvent;
    if (didPauseEvent == null)
      return;
    didPauseEvent();
  }

  public virtual void WillResume()
  {
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
    this._audioListenerController.Resume();
    this._tutorialSongController.ResumeSong();
    System.Action didResumeEvent = this.didResumeEvent;
    if (didResumeEvent == null)
      return;
    didResumeEvent();
  }
}
