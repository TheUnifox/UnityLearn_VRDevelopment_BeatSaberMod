// Decompiled with JetBrains decompiler
// Type: NoPauseGamePause
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class NoPauseGamePause : IGamePause
{
  protected bool _pause;

  public bool isPaused => this._pause;

  public event System.Action didPauseEvent;

  public event System.Action willResumeEvent;

  public event System.Action didResumeEvent;

  public virtual void Pause()
  {
    this._pause = true;
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
    this._pause = false;
    System.Action didResumeEvent = this.didResumeEvent;
    if (didResumeEvent == null)
      return;
    didResumeEvent();
  }
}
