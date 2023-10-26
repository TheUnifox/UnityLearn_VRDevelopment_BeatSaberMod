// Decompiled with JetBrains decompiler
// Type: HMUI.Screen
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections;
using UnityEngine;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class Screen : MonoBehaviour
  {
    protected ViewController _rootViewController;
    protected bool _isBeingDestroyed;

    public bool isBeingDestroyed => this._isBeingDestroyed;

    public virtual void SetRootViewController(
      ViewController newRootViewController,
      ViewController.AnimationType animationType)
    {
      if ((Object) newRootViewController == (Object) this._rootViewController)
        return;
      this.gameObject.SetActive(true);
      this.StopAllCoroutines();
      this.StartCoroutine(this.TransitionCoroutine(newRootViewController, animationType));
    }

    public virtual IEnumerator TransitionCoroutine(
      ViewController newRootViewController,
      ViewController.AnimationType animationType)
    {
      Screen screen = this;
      ViewController oldRootViewController = screen._rootViewController;
      screen._rootViewController = newRootViewController;
      if ((Object) newRootViewController != (Object) null)
      {
        if (!newRootViewController.gameObject.activeSelf)
          newRootViewController.gameObject.SetActive(true);
        newRootViewController.__Init(screen, (ViewController) null, (ContainerViewController) null);
        newRootViewController.__Activate(true, false);
        newRootViewController.transform.localPosition = Vector3.zero;
        newRootViewController.canvasGroup.alpha = 0.0f;
      }
      if ((Object) oldRootViewController != (Object) null)
        oldRootViewController.__Deactivate(true, false, false);
      if (animationType != ViewController.AnimationType.None)
      {
        yield return (object) null;
        float oldRootViewControllerStartAlpha = 1f;
        if ((Object) oldRootViewController != (Object) null)
          oldRootViewControllerStartAlpha = oldRootViewController.canvasGroup.alpha;
        float elapsedTime = 0.0f;
        while ((double) elapsedTime < 0.40000000596046448)
        {
          float t = Easing.OutQuart(elapsedTime / 0.4f);
          if ((Object) newRootViewController != (Object) null)
            newRootViewController.canvasGroup.alpha = t;
          if ((Object) oldRootViewController != (Object) null)
            oldRootViewController.canvasGroup.alpha = Mathf.Lerp(oldRootViewControllerStartAlpha, 0.0f, t);
          elapsedTime += Time.deltaTime;
          yield return (object) null;
        }
      }
      if ((Object) newRootViewController != (Object) null)
      {
        newRootViewController.canvasGroup.alpha = 1f;
        newRootViewController.transform.localPosition = Vector3.zero;
      }
      if ((Object) oldRootViewController != (Object) null)
      {
        oldRootViewController.canvasGroup.alpha = 0.0f;
        oldRootViewController.transform.localPosition = Vector3.zero;
        oldRootViewController.DeactivateGameObject();
        oldRootViewController.__ResetViewController();
      }
      if ((Object) screen._rootViewController == (Object) null)
        screen.gameObject.SetActive(false);
    }

    public virtual void OnDestroy() => this._isBeingDestroyed = true;
  }
}
