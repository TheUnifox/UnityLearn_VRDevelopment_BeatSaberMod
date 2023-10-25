// Decompiled with JetBrains decompiler
// Type: FlyingSpriteEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FlyingSpriteEffect : FlyingObjectEffect
{
  [SerializeField]
  protected SpriteRenderer _spriteRenderer;
  [SerializeField]
  protected AnimationCurve _fadeAnimationCurve = AnimationCurve.Linear(0.0f, 1f, 1f, 0.0f);
  protected Color _color;

  public virtual void InitAndPresent(
    float duration,
    Vector3 targetPos,
    Quaternion rotation,
    Sprite sprite,
    Material material,
    Color color,
    bool shake)
  {
    this._spriteRenderer.sprite = sprite;
    this._spriteRenderer.material = material;
    this._color = color;
    this.InitAndPresent(duration, targetPos, rotation, shake);
  }

  protected override void ManualUpdate(float t) => this._spriteRenderer.color = this._color.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t));

  public class Pool : MonoMemoryPool<FlyingSpriteEffect>
  {
  }
}
