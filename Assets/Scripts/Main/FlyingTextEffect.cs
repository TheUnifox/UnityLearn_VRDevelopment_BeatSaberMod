// Decompiled with JetBrains decompiler
// Type: FlyingTextEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using Zenject;

public class FlyingTextEffect : FlyingObjectEffect
{
  [SerializeField]
  protected TextMeshPro _text;
  [SerializeField]
  protected AnimationCurve _fadeAnimationCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 0.0f);
  protected Color _color;

  public virtual void InitAndPresent(
    string text,
    float duration,
    Vector3 targetPos,
    Quaternion rotation,
    Color color,
    float fontSize,
    bool shake)
  {
    this.InitAndPresent(duration, targetPos, rotation, shake);
    this._color = color;
    this._text.text = text;
    this._text.fontSize = fontSize;
  }

  protected override void ManualUpdate(float t) => this._text.color = this._color.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t));

  public class Pool : MonoMemoryPool<FlyingTextEffect>
  {
  }
}
