// Decompiled with JetBrains decompiler
// Type: HMUI.ViewController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (BaseRaycaster))]
  [RequireComponent(typeof (CanvasGroup))]
  public class ViewController : MonoBehaviour
  {
    public const float kTransitionDuration = 0.4f;
    protected const float kTransitionMoveOffset = 2f;
    [CompilerGenerated]
    protected ButtonBinder buttonBinder_k__BackingField;
    protected ContainerViewController _containerViewController;
    protected ViewController _parentViewController;
    protected ViewController _childViewController;
    protected Screen _screen;
    protected RectTransform _rectTransform;
    protected CanvasGroup _canvasGroup;
    protected bool _wasActivatedBefore;
    protected bool _isActivated;
    protected bool _isInTransition;
    protected BaseRaycaster _graphicRaycaster;

    public ContainerViewController containerViewController => this._containerViewController;

    public Screen screen => this._screen;

    public ViewController parentViewController => this._parentViewController;

    public ViewController childViewController => this._childViewController;

    public bool isInViewControllerHierarchy => (UnityEngine.Object) this._screen != (UnityEngine.Object) null;

    public bool isActivated => this._isActivated;

    public bool wasActivatedBefore => this._wasActivatedBefore;

    public bool isInTransition
    {
      get => this._isInTransition;
      set => this._isInTransition = value;
    }

    public bool enableUserInteractions
    {
      get => this.graphicRaycaster.enabled;
      set => this.graphicRaycaster.enabled = value;
    }

    public event ViewController.DidActivateDelegate didActivateEvent;

    public event ViewController.DidDeactivateDelegate didDeactivateEvent;

    protected ButtonBinder buttonBinder
    {
      get => this.buttonBinder_k__BackingField;
      private set => this.buttonBinder_k__BackingField = value;
    }

    public RectTransform rectTransform
    {
      get
      {
        if ((UnityEngine.Object) this._rectTransform == (UnityEngine.Object) null)
          this._rectTransform = this.GetComponent<RectTransform>();
        return this._rectTransform;
      }
    }

    public CanvasGroup canvasGroup
    {
      get
      {
        if ((UnityEngine.Object) this._canvasGroup == (UnityEngine.Object) null)
          this._canvasGroup = this.GetComponent<CanvasGroup>();
        return this._canvasGroup;
      }
    }

    private BaseRaycaster graphicRaycaster
    {
      get
      {
        if ((UnityEngine.Object) this._graphicRaycaster == (UnityEngine.Object) null)
          this._graphicRaycaster = this.GetComponent<BaseRaycaster>();
        return this._graphicRaycaster;
      }
    }

    protected virtual void OnDestroy()
    {
      this.buttonBinder?.ClearBindings();
      if (!this.isActivated)
        return;
      this.__Deactivate(true, false, false);
    }

    protected virtual void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
    }

    protected virtual void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
    }

    public virtual void __Init(
      Screen screen,
      ViewController parentViewController,
      ContainerViewController containerViewController)
    {
      this._screen = screen;
      this._parentViewController = parentViewController;
      this._containerViewController = containerViewController;
      if (this.buttonBinder == null)
        this.buttonBinder = new ButtonBinder();
      if ((UnityEngine.Object) this._containerViewController != (UnityEngine.Object) null)
        this.transform.SetParent((Transform) containerViewController.controllersContainer, false);
      else
        this.transform.SetParent(screen.transform, false);
    }

    public virtual void __ResetViewController()
    {
      if ((UnityEngine.Object) this._screen != (UnityEngine.Object) null && !this._screen.isBeingDestroyed)
        this.transform.SetParent(this._screen.transform, false);
      this._screen = (Screen) null;
      this._parentViewController = (ViewController) null;
      this._containerViewController = (ContainerViewController) null;
      this._childViewController = (ViewController) null;
    }

    public virtual void __PresentViewController(
      ViewController viewController,
      Action finishedCallback,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      bool immediately = false)
    {
      this.StartCoroutine(this.PresentViewControllerCoroutine(viewController, finishedCallback, animationDirection, immediately));
    }

    public virtual IEnumerator PresentViewControllerCoroutine(
      ViewController newViewController,
      Action finishedCallback,
      ViewController.AnimationDirection animationDirection,
      bool immediately)
    {
      ViewController viewController = this;
      if (!viewController._isInTransition && !newViewController._isInTransition)
      {
        viewController._isInTransition = true;
        newViewController._isInTransition = true;
        EventSystem.current?.SetSelectedGameObject((GameObject) null);
        viewController.__Deactivate(false, false, false);
        if (!newViewController.gameObject.activeSelf)
          newViewController.gameObject.SetActive(true);
        viewController._childViewController = newViewController;
        newViewController.__Init(viewController._screen, viewController, (ContainerViewController) null);
        if (immediately)
          viewController.gameObject.SetActive(false);
        newViewController.__Activate(true, false);
        if (!immediately)
        {
          float moveOffsetMultiplier = 1f / viewController.transform.lossyScale.z;
          yield return (object) ViewControllerTransitionHelpers.DoPresentTransition(newViewController, viewController, animationDirection, moveOffsetMultiplier);
        }
        else
          ViewControllerTransitionHelpers.ImmediateTransition(newViewController, viewController);
        viewController.DeactivateGameObject();
        viewController._isInTransition = false;
        newViewController._isInTransition = false;
        Action action = finishedCallback;
        if (action != null)
          action();
      }
    }

    public virtual void __ReplaceViewController(
      ViewController viewController,
      Action finishedCallback,
      ViewController.AnimationType animationType,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal)
    {
      this.StartCoroutine(this.ReplaceViewControllerCoroutine(viewController, finishedCallback, animationType, animationDirection));
    }

    public virtual IEnumerator ReplaceViewControllerCoroutine(
      ViewController newViewController,
      Action finishedCallback,
      ViewController.AnimationType animationType,
      ViewController.AnimationDirection animationDirection)
    {
      ViewController toDismissViewController = this;
      if (!toDismissViewController._isInTransition && !newViewController._isInTransition)
      {
        toDismissViewController._isInTransition = true;
        newViewController._isInTransition = true;
        EventSystem.current?.SetSelectedGameObject((GameObject) null);
        toDismissViewController.__Deactivate(true, false, false);
        if (!newViewController.gameObject.activeSelf)
          newViewController.gameObject.SetActive(true);
        if ((UnityEngine.Object) toDismissViewController.parentViewController != (UnityEngine.Object) null)
          toDismissViewController.parentViewController._childViewController = newViewController;
        newViewController.__Init(toDismissViewController._screen, toDismissViewController.parentViewController, (ContainerViewController) null);
        newViewController.__Activate(true, false);
        if (animationType != ViewController.AnimationType.None)
        {
          float moveOffsetMultiplier = (float) (1.0 / (double) toDismissViewController.transform.lossyScale.z * (animationType == ViewController.AnimationType.In ? 1.0 : -1.0));
          yield return (object) ViewControllerTransitionHelpers.DoPresentTransition(newViewController, toDismissViewController, animationDirection, moveOffsetMultiplier);
        }
        else
          ViewControllerTransitionHelpers.ImmediateTransition(newViewController, toDismissViewController);
        toDismissViewController.DeactivateGameObject();
        toDismissViewController.__ResetViewController();
        toDismissViewController._isInTransition = false;
        newViewController._isInTransition = false;
        Action action = finishedCallback;
        if (action != null)
          action();
      }
    }

    public virtual void __DismissViewController(
      Action finishedCallback,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      bool immediately = false)
    {
      this.StartCoroutine(this.DismissViewControllerCoroutine(finishedCallback, animationDirection, immediately));
    }

    public virtual IEnumerator DismissViewControllerCoroutine(
      Action finishedCallback,
      ViewController.AnimationDirection animationDirection,
      bool immediately)
    {
      ViewController toDismissViewController = this;
      ViewController movingInViewController = toDismissViewController._parentViewController;
      if (!toDismissViewController._isInTransition && !movingInViewController._isInTransition)
      {
        toDismissViewController._isInTransition = true;
        movingInViewController._isInTransition = true;
        EventSystem.current?.SetSelectedGameObject((GameObject) null);
        toDismissViewController.__Deactivate(true, false, false);
        movingInViewController._childViewController = (ViewController) null;
        movingInViewController.__Activate(false, true);
        if (!immediately)
        {
          float moveOffsetMultiplier = 1f / toDismissViewController.transform.lossyScale.z;
          yield return (object) ViewControllerTransitionHelpers.DoDismissTransition(movingInViewController, toDismissViewController, animationDirection, moveOffsetMultiplier);
        }
        else
          ViewControllerTransitionHelpers.ImmediateTransition(movingInViewController, toDismissViewController);
        toDismissViewController.DeactivateGameObject();
        toDismissViewController.__ResetViewController();
        toDismissViewController._isInTransition = false;
        movingInViewController._isInTransition = false;
        Action action = finishedCallback;
        if (action != null)
          action();
      }
    }

    public virtual void __Activate(bool addedToHierarchy, bool screenSystemEnabling)
    {
      this._isActivated = true;
      if (!this.gameObject.activeSelf)
        this.gameObject.SetActive(true);
      bool wasActivatedBefore = this._wasActivatedBefore;
      this._wasActivatedBefore = true;
      this.DidActivate(!wasActivatedBefore, addedToHierarchy, screenSystemEnabling);
      ViewController.DidActivateDelegate didActivateEvent = this.didActivateEvent;
      if (didActivateEvent == null)
        return;
      didActivateEvent(!wasActivatedBefore, addedToHierarchy, screenSystemEnabling);
    }

    public virtual void __Deactivate(
      bool removedFromHierarchy,
      bool deactivateGameObject,
      bool screenSystemDisabling)
    {
      int num = removedFromHierarchy ? 1 : 0;
      this._isActivated = false;
      if (this.gameObject.activeSelf & deactivateGameObject)
        this.gameObject.SetActive(false);
      this.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
      ViewController.DidDeactivateDelegate didDeactivateEvent = this.didDeactivateEvent;
      if (didDeactivateEvent == null)
        return;
      didDeactivateEvent(removedFromHierarchy, screenSystemDisabling);
    }

    public virtual void DeactivateGameObject()
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
    }

    public virtual bool IsViewControllerInHierarchy(ViewController viewController)
    {
      for (ViewController parentViewController = viewController._parentViewController; (UnityEngine.Object) parentViewController != (UnityEngine.Object) null; parentViewController = parentViewController._parentViewController)
      {
        if (parentViewController == viewController || parentViewController is ContainerViewController && ((ContainerViewController) parentViewController).viewControllers.Contains(viewController))
          return true;
      }
      for (ViewController childViewController = viewController._childViewController; (UnityEngine.Object) childViewController != (UnityEngine.Object) null; childViewController = childViewController._childViewController)
      {
        if (childViewController == viewController || childViewController is ContainerViewController && ((ContainerViewController) childViewController).viewControllers.Contains(viewController))
          return true;
      }
      return false;
    }

    [Conditional("ViewControllerLog")]
    public static void Log(string message) => UnityEngine.Debug.Log((object) message);

    public delegate void DidActivateDelegate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling);

    public delegate void DidDeactivateDelegate(
      bool removedFromHierarchy,
      bool screenSystemDisabling);

    public enum AnimationType
    {
      None,
      In,
      Out,
    }

    public enum AnimationDirection
    {
      Horizontal,
      Vertical,
    }
  }
}
