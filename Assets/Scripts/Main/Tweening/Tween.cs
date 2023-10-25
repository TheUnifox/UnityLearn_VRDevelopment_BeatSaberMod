// Decompiled with JetBrains decompiler
// Type: Tweening.Tween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace Tweening
{
  public abstract class Tween
  {
    public System.Action onStart;
    public System.Action onCompleted;
    public System.Action onKilled;
    protected float _progress;
    protected float _startTime;
    protected float _duration;
    protected bool _loop;
    protected float _delay;
    protected bool _isStarted;
    protected bool _isKilled;
    protected EaseType _easeType = EaseType.Linear;
    protected const float kEpsilon = 0.001f;

    public bool isStarted => this._isStarted;

    public bool isActive => !this._isKilled && !this.isComplete && this.isStarted;

    public bool isComplete => (double) this._progress >= 1.0;

    public bool isKilled => this._isKilled;

    public float progress => this._progress;

    public float startTime => this._startTime;

    public float duration
    {
      get => this._duration;
      set => this._duration = value;
    }

    public bool loop
    {
      get => this._loop;
      set => this._loop = value;
    }

    public float delay
    {
      get => this._delay;
      set => this._delay = value;
    }

    public EaseType easeType
    {
      get => this._easeType;
      set => this._easeType = value;
    }

    public void Kill() => this._isKilled = true;

    public void Restart(float startTime)
    {
      this._isStarted = false;
      this._isKilled = false;
      this._progress = 0.0f;
      this._startTime = startTime;
    }

    public void Resume()
    {
      this._isStarted = false;
      this._isKilled = false;
    }

    public void SetStartTimeAndEndTime(float startTime, float endTime)
    {
      this._startTime = startTime;
      this._duration = endTime - startTime;
    }

    public abstract void Update(float currentTime);

    public abstract void Sample(float t);
  }
}
