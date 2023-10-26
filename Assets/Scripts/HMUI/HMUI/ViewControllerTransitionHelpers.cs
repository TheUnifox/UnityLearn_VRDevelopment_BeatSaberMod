// Decompiled with JetBrains decompiler
// Type: HMUI.ViewControllerTransitionHelpers
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections;
using UnityEngine;

namespace HMUI
{
  public class ViewControllerTransitionHelpers
  {
    protected const float kTransitionDuration = 0.4f;
    protected const float kHorizontalTransitionMoveOffset = 2f;
    protected const float kVerticalTransitionMoveOffset = 0.5f;

    public static IEnumerator DoPresentTransition(
      ViewController toPresentViewController,
      ViewController toDismissViewController,
      ViewController.AnimationDirection animationDirection,
      float moveOffsetMultiplier)
    {
      switch (animationDirection)
      {
        case ViewController.AnimationDirection.Horizontal:
          yield return (object) ViewControllerTransitionHelpers.DoHorizontalTransition(toPresentViewController, toDismissViewController, moveOffsetMultiplier);
          break;
        case ViewController.AnimationDirection.Vertical:
          yield return (object) ViewControllerTransitionHelpers.DoVerticalTransition(toPresentViewController, toDismissViewController, moveOffsetMultiplier * -1f);
          break;
      }
    }

    public static IEnumerator DoDismissTransition(
      ViewController toPresentViewController,
      ViewController toDismissViewController,
      ViewController.AnimationDirection animationDirection,
      float moveOffsetMultiplier)
    {
      switch (animationDirection)
      {
        case ViewController.AnimationDirection.Horizontal:
          yield return (object) ViewControllerTransitionHelpers.DoHorizontalTransition(toPresentViewController, toDismissViewController, moveOffsetMultiplier * -1f);
          break;
        case ViewController.AnimationDirection.Vertical:
          yield return (object) ViewControllerTransitionHelpers.DoVerticalTransition(toPresentViewController, toDismissViewController, moveOffsetMultiplier);
          break;
      }
    }

    private static IEnumerator DoHorizontalTransition(
      ViewController toPresentViewController,
      ViewController toDismissViewController,
      float moveOffsetMultiplier)
    {
      toPresentViewController.canvasGroup.alpha = 0.0f;
      float baseCanvasGroupAlpha = toDismissViewController.canvasGroup.alpha;
      float moveOffset = 2f * moveOffsetMultiplier;
      yield return (object) ViewControllerTransitionHelpers.AnimationCoroutine((Action<float>) (t =>
      {
        toPresentViewController.canvasGroup.alpha = t;
        toPresentViewController.transform.localPosition = new Vector3(moveOffset * (1f - t), 0.0f, 0.0f);
        toDismissViewController.canvasGroup.alpha = Mathf.Lerp(baseCanvasGroupAlpha, 0.0f, t);
        toDismissViewController.transform.localPosition = new Vector3(-moveOffset * t, 0.0f, 0.0f);
      }));
      ViewControllerTransitionHelpers.ImmediateTransition(toPresentViewController, toDismissViewController);
    }

    private static IEnumerator DoVerticalTransition(
      ViewController toPresentViewController,
      ViewController toDismissViewController,
      float moveOffsetMultiplier)
    {
      toPresentViewController.canvasGroup.alpha = 0.0f;
      float baseCanvasGroupAlpha = toDismissViewController.canvasGroup.alpha;
      float moveOffset = 0.5f * moveOffsetMultiplier;
      yield return (object) ViewControllerTransitionHelpers.AnimationCoroutine((Action<float>) (t =>
      {
        toPresentViewController.canvasGroup.alpha = t;
        toPresentViewController.transform.localPosition = new Vector3(0.0f, moveOffset * (1f - t), 0.0f);
        float num = Mathf.Lerp(baseCanvasGroupAlpha, 0.0f, t);
        toDismissViewController.canvasGroup.alpha = num * num;
        toDismissViewController.transform.localPosition = new Vector3(0.0f, (float) (-(double) moveOffset * (double) t * 0.5), 0.0f);
      }));
      ViewControllerTransitionHelpers.ImmediateTransition(toPresentViewController, toDismissViewController);
    }

    public static void ImmediateTransition(
      ViewController toPresentViewController,
      ViewController toDismissViewController)
    {
      toPresentViewController.canvasGroup.alpha = 1f;
      toDismissViewController.canvasGroup.alpha = 0.0f;
      toPresentViewController.transform.localPosition = Vector3.zero;
      toDismissViewController.transform.localPosition = Vector3.zero;
    }

    private static IEnumerator AnimationCoroutine(Action<float> transitionAnimation)
    {
      yield return (object) null;
      float elapsedTime = 0.0f;
      while ((double) elapsedTime < 0.40000000596046448)
      {
        float num = Easing.OutQuart(elapsedTime / 0.4f);
        Action<float> action = transitionAnimation;
        if (action != null)
          action(num);
        elapsedTime += Time.deltaTime;
        yield return (object) null;
      }
      Action<float> action1 = transitionAnimation;
      if (action1 != null)
        action1(1f);
    }
  }
}
