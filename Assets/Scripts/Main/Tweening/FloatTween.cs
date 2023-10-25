// Decompiled with JetBrains decompiler
// Type: Tweening.FloatTween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using Zenject;

namespace Tweening
{
  public class FloatTween : Tween<float>
  {
    [DoesNotRequireDomainReloadInit]
    public static readonly StaticMemoryPool<float, float, System.Action<float>, float, EaseType, float, FloatTween> Pool = new StaticMemoryPool<float, float, System.Action<float>, float, EaseType, float, FloatTween>(new Action<float, float, System.Action<float>, float, EaseType, float, FloatTween>(Tween<float>.OnSpawned), new System.Action<FloatTween>(Tween<float>.OnDespawned));

    public FloatTween()
    {
    }

    public FloatTween(
      float fromValue,
      float toValue,
      System.Action<float> onUpdate,
      float duration,
      EaseType easeType,
      float delay = 0.0f)
      : base(fromValue, toValue, onUpdate, duration, easeType, delay)
    {
    }

    public override float GetValue(float t) => this.fromValue + (this.toValue - this.fromValue) * Interpolation.Interpolate(t, this._easeType);
  }
}
