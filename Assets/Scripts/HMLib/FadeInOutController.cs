// Decompiled with JetBrains decompiler
// Type: FadeInOutController
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{
  [SerializeField]
  protected FloatSO _easeValue;
  [SerializeField]
  protected AnimationCurve _fadeInCurve;
  [SerializeField]
  protected AnimationCurve _fadeOutCurve;
  [SerializeField]
  protected float _fadeInStartDelay = 0.1f;
  [SerializeField]
  protected float _defaultFadeOutDuration = 1.3f;
  [SerializeField]
  protected float _defaultFadeInDuration = 1f;

  public virtual void FadeOutInstant() => this.FadeOut(0.0f, (System.Action) null);

  public virtual void FadeIn() => this.FadeIn(this._defaultFadeInDuration, (System.Action) null);

  public virtual void FadeOut() => this.FadeOut(this._defaultFadeOutDuration, (System.Action) null);

  public virtual void FadeIn(float duration) => this.FadeIn(duration, (System.Action) null);

  public virtual void FadeOut(float duration) => this.FadeOut(duration, (System.Action) null);

  public virtual void FadeIn(System.Action fadeInCallback) => this.FadeIn(this._defaultFadeInDuration, fadeInCallback);

  public virtual void FadeOut(System.Action fadeOutCallback) => this.FadeOut(this._defaultFadeInDuration, fadeOutCallback);

  public virtual void FadeIn(float duration, System.Action fadeInFinishedCallback)
  {
    this.StopAllCoroutines();
    if ((double) duration == 0.0)
    {
      this._easeValue.value = 1f;
    }
    else
    {
      this._easeValue.value = 0.0f;
      this.StartCoroutine(this.Fade(0.0f, 1f, duration, this._fadeInStartDelay, this._fadeInCurve, fadeInFinishedCallback));
    }
  }

  public virtual void FadeOut(float duration, System.Action fadeOutFinishedCallback)
  {
    this.StopAllCoroutines();
    if ((double) duration == 0.0)
      this._easeValue.value = 0.0f;
    else
      this.StartCoroutine(this.Fade(this._easeValue.value, 0.0f, duration, 0.0f, this._fadeOutCurve, fadeOutFinishedCallback));
  }

  public virtual IEnumerator Fade(
    float fromValue,
    float toValue,
    float duration,
    float startDelay,
    AnimationCurve curve,
    System.Action fadeFinishedCallback)
  {
    if ((double) startDelay > 0.0)
      yield return (object) new WaitForSeconds(startDelay);
    for (float elapsedTime = 0.0f; (double) elapsedTime < (double) duration; elapsedTime += Time.deltaTime)
    {
      this._easeValue.value = Mathf.Lerp(fromValue, toValue, curve.Evaluate(elapsedTime / duration));
      yield return (object) null;
    }
    this._easeValue.value = toValue;
    System.Action action = fadeFinishedCallback;
    if (action != null)
      action();
  }
}
