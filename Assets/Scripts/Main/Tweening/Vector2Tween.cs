// Decompiled with JetBrains decompiler
// Type: Tweening.Vector2Tween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

namespace Tweening
{
  public class Vector2Tween : Tween<Vector2>
  {
    [DoesNotRequireDomainReloadInit]
    public static readonly StaticMemoryPool<Vector2, Vector2, System.Action<Vector2>, float, EaseType, float, Vector2Tween> Pool = new StaticMemoryPool<Vector2, Vector2, System.Action<Vector2>, float, EaseType, float, Vector2Tween>(new Action<Vector2, Vector2, System.Action<Vector2>, float, EaseType, float, Vector2Tween>(Tween<Vector2>.OnSpawned), new System.Action<Vector2Tween>(Tween<Vector2>.OnDespawned));

    public Vector2Tween()
    {
    }

    public Vector2Tween(
      Vector2 fromValue,
      Vector2 toValue,
      System.Action<Vector2> onUpdate,
      float duration,
      EaseType easeType,
      float delay = 0.0f)
      : base(fromValue, toValue, onUpdate, duration, easeType, delay)
    {
    }

    public override Vector2 GetValue(float t) => this.fromValue + (this.toValue - this.fromValue) * Interpolation.Interpolate(t, this._easeType);
  }
}
