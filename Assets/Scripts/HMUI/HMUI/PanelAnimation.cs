// Decompiled with JetBrains decompiler
// Type: HMUI.PanelAnimation
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections;
using UnityEngine;

namespace HMUI
{
  public class PanelAnimation : MonoBehaviour
  {
    public virtual void StartAnimation(
      CanvasGroup canvasGroup,
      CanvasGroup parentCanvasGroup,
      float duration,
      AnimationCurve scaleXAnimationCurve,
      AnimationCurve scaleYAnimationCurve,
      AnimationCurve alphaAnimationCurve,
      AnimationCurve parentAlphaAnimationCurve,
      Action finishedCallback)
    {
      if (!this.gameObject.activeInHierarchy)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
        if (finishedCallback == null)
          return;
        finishedCallback();
      }
      else
      {
        this.StopAllCoroutines();
        this.StartCoroutine(this.AnimationCoroutine(duration, canvasGroup, parentCanvasGroup, scaleXAnimationCurve, scaleYAnimationCurve, alphaAnimationCurve, parentAlphaAnimationCurve, finishedCallback));
      }
    }

    public virtual IEnumerator AnimationCoroutine(
      float duration,
      CanvasGroup canvasGroup,
      CanvasGroup parentCanvasGroup,
      AnimationCurve scaleXAnimationCurve,
      AnimationCurve scaleYAnimationCurve,
      AnimationCurve alphaAnimationCurve,
      AnimationCurve parentAlphaAnimationCurve,
      Action finishedCallback)
    {
      PanelAnimation panelAnimation = this;
      Transform canvasTransform = canvasGroup.transform;
      float elapsedTime = 0.0f;
      while ((double) elapsedTime < (double) duration)
      {
        elapsedTime += Time.deltaTime;
        float time = elapsedTime / duration;
        if ((UnityEngine.Object) parentCanvasGroup != (UnityEngine.Object) null)
          parentCanvasGroup.alpha = parentAlphaAnimationCurve.Evaluate(time);
        canvasGroup.alpha = alphaAnimationCurve.Evaluate(time);
        canvasTransform.localScale = new Vector3(scaleXAnimationCurve.Evaluate(time), scaleYAnimationCurve.Evaluate(time), 1f);
        yield return (object) null;
      }
      if ((UnityEngine.Object) parentCanvasGroup != (UnityEngine.Object) null)
        parentCanvasGroup.alpha = parentAlphaAnimationCurve.Evaluate(1f);
      canvasGroup.alpha = alphaAnimationCurve.Evaluate(1f);
      canvasTransform.localScale = new Vector3(scaleXAnimationCurve.Evaluate(1f), scaleYAnimationCurve.Evaluate(1f), 1f);
      UnityEngine.Object.Destroy((UnityEngine.Object) panelAnimation);
      Action action = finishedCallback;
      if (action != null)
        action();
    }
  }
}
