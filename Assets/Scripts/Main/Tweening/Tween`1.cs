// Decompiled with JetBrains decompiler
// Type: Tweening.Tween`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

namespace Tweening
{
  public class Tween<T> : Tween
  {
    public T fromValue;
    public T toValue;
    public System.Action<T> onUpdate;

    protected static void OnSpawned(
      T fromValue,
      T toValue,
      System.Action<T> onUpdate,
      float duration,
      EaseType easeType,
      float delay,
      Tween<T> t)
    {
      t.Init(fromValue, toValue, onUpdate, duration, easeType, delay);
    }

    protected static void OnDespawned(Tween<T> t)
    {
      t.onStart = (System.Action) null;
      t.onUpdate = (System.Action<T>) null;
      t.onCompleted = (System.Action) null;
      t.onKilled = (System.Action) null;
      t.Kill();
    }

    protected Tween() => this.Init(default (T), default (T), (System.Action<T>) null, 0.0f, EaseType.None);

    protected Tween(
      T fromValue,
      T toValue,
      System.Action<T> onUpdate,
      float duration,
      EaseType easeType,
      float delay)
    {
      this.Init(fromValue, toValue, onUpdate, duration, easeType, delay);
    }

    public virtual void Init(
      T fromValue,
      T toValue,
      System.Action<T> onUpdate,
      float duration,
      EaseType easeType,
      float delay = 0.0f)
    {
      this.fromValue = fromValue;
      this.toValue = toValue;
      this.onUpdate = onUpdate;
      this._duration = duration;
      this._easeType = easeType;
      this._delay = delay;
      this._loop = false;
    }

    public override void Update(float currentTime)
    {
      float a = currentTime - this._startTime;
      float num = !this._loop || (double) a <= (double) this._duration + (double) this._delay ? Mathf.Min(a, (float) ((double) this._duration + (double) this._delay + 1.0 / 1000.0)) : (a - this._delay) % this._duration + this._delay;
      if (!this._isStarted && (double) num > (double) this._delay)
      {
        this._isStarted = true;
        System.Action onStart = this.onStart;
        if (onStart != null)
          onStart();
      }
      this._progress = (double) this._duration > 0.0 ? Mathf.Max(0.0f, num - this._delay) / this._duration : 1f;
      System.Action<T> onUpdate = this.onUpdate;
      if (onUpdate == null)
        return;
      onUpdate(this.GetValue(this._progress));
    }

    public override void Sample(float t)
    {
      System.Action<T> onUpdate = this.onUpdate;
      if (onUpdate == null)
        return;
      onUpdate(this.GetValue(t));
    }

    public virtual void ForceOnUpdate()
    {
      System.Action<T> onUpdate = this.onUpdate;
      if (onUpdate == null)
        return;
      onUpdate(this.GetValue(this._progress));
    }

    public virtual T GetValue(float t) => default (T);
  }
}
