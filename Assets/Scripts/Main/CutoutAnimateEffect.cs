// Decompiled with JetBrains decompiler
// Type: CutoutAnimateEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CutoutAnimateEffect : MonoBehaviour
{
  [SerializeField]
  protected CutoutEffect[] _cuttoutEffects;
  [SerializeField]
  protected AnimationCurve _transitionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  [CompilerGenerated]
  protected bool m_Canimating;

  public bool animating
  {
    get => this.m_Canimating;
    private set => this.m_Canimating = value;
  }

  public virtual void Start() => this.SetCutout(0.0f);

  public virtual IEnumerator AnimateToCutoutCoroutine(
    float cutoutStart,
    float cutoutEnd,
    float duration)
  {
    this.animating = true;
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < (double) duration)
    {
      float time = elapsedTime / duration;
      this.SetCutout(Mathf.Lerp(cutoutStart, cutoutEnd, this._transitionCurve.Evaluate(time)));
      elapsedTime += Time.deltaTime;
      yield return (object) null;
    }
    this.SetCutout(cutoutEnd);
    this.animating = false;
  }

  public virtual void SetCutout(float cutout)
  {
    foreach (CutoutEffect cuttoutEffect in this._cuttoutEffects)
      cuttoutEffect.SetCutout(cutout);
  }

  public virtual void ResetEffect()
  {
    this.animating = false;
    this.StopAllCoroutines();
    this.SetCutout(0.0f);
  }

  public virtual void AnimateCutout(float cutoutStart, float cutoutEnd, float duration)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.AnimateToCutoutCoroutine(cutoutStart, cutoutEnd, duration));
  }
}
