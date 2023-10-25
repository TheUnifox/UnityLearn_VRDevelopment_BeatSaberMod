// Decompiled with JetBrains decompiler
// Type: Tweening.ColorTween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

namespace Tweening
{
  public class ColorTween : Tween<Color>
  {
    [DoesNotRequireDomainReloadInit]
    public static readonly StaticMemoryPool<Color, Color, System.Action<Color>, float, EaseType, float, ColorTween> Pool = new StaticMemoryPool<Color, Color, System.Action<Color>, float, EaseType, float, ColorTween>(new Action<Color, Color, System.Action<Color>, float, EaseType, float, ColorTween>(Tween<Color>.OnSpawned), new System.Action<ColorTween>(Tween<Color>.OnDespawned));

    public ColorTween()
    {
    }

    public ColorTween(
      Color fromValue,
      Color toValue,
      System.Action<Color> onUpdate,
      float duration,
      EaseType easeType,
      float delay = 0.0f)
      : base(fromValue, toValue, onUpdate, duration, easeType, delay)
    {
    }

    public override Color GetValue(float t) => this.fromValue + (this.toValue - this.fromValue) * Interpolation.Interpolate(t, this._easeType);
  }
}
