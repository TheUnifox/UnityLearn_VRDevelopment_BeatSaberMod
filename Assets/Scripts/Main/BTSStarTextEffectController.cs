// Decompiled with JetBrains decompiler
// Type: BTSStarTextEffectController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BTSStarTextEffectController : MonoBehaviour
{
  [SerializeField]
  protected Transform _transform;
  [SerializeField]
  protected SpriteRenderer _spriteRenderer;
  [SerializeField]
  protected Animation _animation;
  protected const string kAnimationName = "StarTextShort";
  protected float _currentAnimationDuration;

  public float animationDuration => this._currentAnimationDuration;

  public virtual void Reinitialize(
    Sprite sprite,
    Transform parentTransform,
    float desiredAnimationLength)
  {
    this._spriteRenderer.sprite = sprite;
    this._transform.SetParent(parentTransform, false);
    this._animation["StarTextShort"].speed = 1f / desiredAnimationLength;
    this._currentAnimationDuration = this._animation.clip.length * desiredAnimationLength;
    this._animation.Rewind("StarTextShort");
    this._animation.Sample();
    this._animation.Play();
  }

  public class Pool : MonoMemoryPool<Sprite, Transform, float, BTSStarTextEffectController>
  {
    protected override void Reinitialize(
      Sprite sprite,
      Transform transform,
      float desiredAnimationLength,
      BTSStarTextEffectController starTextEffectController)
    {
      starTextEffectController.Reinitialize(sprite, transform, desiredAnimationLength);
    }
  }
}
