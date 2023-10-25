// Decompiled with JetBrains decompiler
// Type: BeatEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BeatEffect : MonoBehaviour
{
  [SerializeField]
  protected SpriteRenderer _spriteRenderer;
  [SerializeField]
  protected Transform _spriteTransform;
  [SerializeField]
  protected TubeBloomPrePassLight _tubeBloomPrePassLight;
  [Space]
  [SerializeField]
  protected AnimationCurve _lightIntensityCurve;
  [SerializeField]
  protected AnimationCurve _spriteXScaleCurve;
  [SerializeField]
  protected AnimationCurve _spriteYScaleCurve;
  [SerializeField]
  protected AnimationCurve _transparencyCurve;
  protected readonly LazyCopyHashSet<IBeatEffectDidFinishEvent> _didFinishEvent = new LazyCopyHashSet<IBeatEffectDidFinishEvent>();
  protected float _animationDuration;
  protected float _elapsedTime;
  protected Color _color;

  public ILazyCopyHashSet<IBeatEffectDidFinishEvent> didFinishEvent => (ILazyCopyHashSet<IBeatEffectDidFinishEvent>) this._didFinishEvent;

  public virtual void Init(Color color, float animationDuration, Quaternion rotation)
  {
    this._elapsedTime = 0.0f;
    this._animationDuration = animationDuration;
    this._color = color.ColorWithAlpha(1f);
    this._spriteRenderer.color = color.ColorWithAlpha(0.0f);
    this._tubeBloomPrePassLight.color = color.ColorWithAlpha(0.0f);
    this.transform.rotation = rotation;
  }

  public virtual void ManualUpdate(float deltaTime)
  {
    this._elapsedTime += deltaTime;
    float time = Mathf.Clamp01(this._elapsedTime / this._animationDuration);
    this._tubeBloomPrePassLight.color = this._color.ColorWithAlpha(this._lightIntensityCurve.Evaluate(time));
    this._spriteRenderer.color = this._color.ColorWithAlpha(this._transparencyCurve.Evaluate(time));
    this._spriteTransform.localScale = new Vector3(this._spriteXScaleCurve.Evaluate(time), this._spriteYScaleCurve.Evaluate(time), 1f);
    if ((double) this._elapsedTime <= (double) this._animationDuration)
      return;
    foreach (IBeatEffectDidFinishEvent effectDidFinishEvent in this._didFinishEvent.items)
      effectDidFinishEvent.HandleBeatEffectDidFinish(this);
  }

  public class Pool : MonoMemoryPool<BeatEffect>
  {
  }
}
