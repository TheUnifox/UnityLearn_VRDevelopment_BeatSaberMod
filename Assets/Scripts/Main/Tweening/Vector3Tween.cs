// Decompiled with JetBrains decompiler
// Type: Tweening.Vector3Tween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

namespace Tweening
{
  public class Vector3Tween : Tween<Vector3>
  {
    [DoesNotRequireDomainReloadInit]
    public static readonly StaticMemoryPool<Vector3, Vector3, System.Action<Vector3>, float, EaseType, float, Vector3Tween> Pool = new StaticMemoryPool<Vector3, Vector3, System.Action<Vector3>, float, EaseType, float, Vector3Tween>(new Action<Vector3, Vector3, System.Action<Vector3>, float, EaseType, float, Vector3Tween>(Tween<Vector3>.OnSpawned), new System.Action<Vector3Tween>(Tween<Vector3>.OnDespawned));

    public Vector3Tween()
    {
    }

    public Vector3Tween(
      Vector3 fromValue,
      Vector3 toValue,
      System.Action<Vector3> onUpdate,
      float duration,
      EaseType easeType,
      float delay = 0.0f)
      : base(fromValue, toValue, onUpdate, duration, easeType, delay)
    {
    }

    public override Vector3 GetValue(float t) => this.fromValue + (this.toValue - this.fromValue) * Interpolation.Interpolate(t, this._easeType);
  }
}
