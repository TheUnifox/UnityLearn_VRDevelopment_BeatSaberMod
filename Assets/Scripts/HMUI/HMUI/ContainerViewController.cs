// Decompiled with JetBrains decompiler
// Type: HMUI.ContainerViewController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  public abstract class ContainerViewController : ViewController
  {
    [SerializeField]
    [NullAllowed]
    private RectTransform _controllersContainer;
    protected List<ViewController> _viewControllers = new List<ViewController>();

    public RectTransform controllersContainer => !((UnityEngine.Object) this._controllersContainer != (UnityEngine.Object) null) ? (RectTransform) this.transform : this._controllersContainer;

    public List<ViewController> viewControllers => this._viewControllers;

    public override void __Init(
      Screen screen,
      ViewController parentViewController,
      ContainerViewController containerViewController)
    {
      base.__Init(screen, parentViewController, containerViewController);
      int count = this._viewControllers.Count;
      for (int index = 0; index < count; ++index)
        this._viewControllers[index].__Init(screen, parentViewController, this);
    }

    protected abstract void LayoutViewControllers(List<ViewController> viewControllers);

    public override void __Activate(bool addedToHierarchy, bool screenSystemEnabling)
    {
      this.LayoutViewControllers(this._viewControllers);
      foreach (ViewController viewController in this._viewControllers)
        viewController.__Activate(addedToHierarchy, screenSystemEnabling);
      base.__Activate(addedToHierarchy, screenSystemEnabling);
    }

    public override void __Deactivate(
      bool removedFromHierarchy,
      bool deactivateGameObject,
      bool screenSystemDisabling)
    {
      base.__Deactivate(removedFromHierarchy, deactivateGameObject, screenSystemDisabling);
      for (int index = this._viewControllers.Count - 1; index >= 0; --index)
        this._viewControllers[index].__Deactivate(removedFromHierarchy, deactivateGameObject, screenSystemDisabling);
    }

    public override void DeactivateGameObject()
    {
      base.DeactivateGameObject();
      int count = this._viewControllers.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this._viewControllers[index].gameObject.activeSelf)
          this._viewControllers[index].gameObject.SetActive(false);
      }
    }

    public void ClearChildViewControllers()
    {
      int count = this._viewControllers.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this._viewControllers[index].isActivated)
          this._viewControllers[index].__Deactivate(true, true, false);
        this._viewControllers[index].__ResetViewController();
      }
      this._viewControllers.Clear();
    }

    public void SetChildViewController(ViewController viewController) => this.SetChildViewControllers(viewController);

    public void SetChildViewControllers(params ViewController[] viewControllers)
    {
      this.ClearChildViewControllers();
      this._viewControllers = new List<ViewController>((IEnumerable<ViewController>) viewControllers);
      int count = this._viewControllers.Count;
      for (int index = 0; index < count; ++index)
        this._viewControllers[index].__Init(this.screen, this.parentViewController, this);
      if (this.isActivated)
      {
        for (int index = 0; index < count; ++index)
          this._viewControllers[index].__Activate(true, false);
      }
      this.LayoutViewControllers(this._viewControllers);
    }

    protected void AddViewController(
      ViewController viewController,
      Action finishedCallback,
      Action<float, ViewController[]> animationLayouter,
      bool immediately = false)
    {
      this.StopAllCoroutines();
      if (this.isActivated)
        this.StartCoroutine(this.AddViewControllerCoroutine(viewController, finishedCallback, animationLayouter, immediately));
      else
        this.SetChildViewControllers(new List<ViewController>((IEnumerable<ViewController>) this._viewControllers)
        {
          viewController
        }.ToArray());
    }

    private IEnumerator AddViewControllerCoroutine(
      ViewController newViewController,
      Action finishedCallback,
      Action<float, ViewController[]> animationLayouter,
      bool immediately)
    {
      ContainerViewController containerViewController = this;
      if (!containerViewController.isInTransition)
      {
        containerViewController.isInTransition = true;
        newViewController.isInTransition = true;
        containerViewController._viewControllers.Add(newViewController);
        newViewController.__Init(containerViewController.screen, containerViewController.parentViewController, containerViewController);
        newViewController.__Activate(true, false);
        ViewController[] viewControllers = containerViewController._viewControllers.ToArray();
        if (!immediately)
        {
          float transitionDuration = 0.4f;
          float elapsedTime = 0.0f;
          while ((double) elapsedTime < (double) transitionDuration)
          {
            animationLayouter(elapsedTime / transitionDuration, viewControllers);
            elapsedTime += Time.deltaTime;
            yield return (object) null;
          }
        }
        animationLayouter(1f, viewControllers);
        containerViewController.isInTransition = false;
        newViewController.isInTransition = false;
        Action action = finishedCallback;
        if (action != null)
          action();
      }
    }

    protected void RemoveViewControllers(
      ViewController[] viewControllers,
      Action finishedCallback,
      Action<float, ViewController[], HashSet<ViewController>> animationLayouter,
      bool immediately)
    {
      foreach (ViewController viewController in viewControllers)
        ;
      this.StartCoroutine(this.RemoveViewControllersCoroutine(viewControllers, finishedCallback, animationLayouter, immediately));
    }

    private IEnumerator RemoveViewControllersCoroutine(
      ViewController[] viewControllersToRemove,
      Action finishedCallback,
      Action<float, ViewController[], HashSet<ViewController>> animationLayouter,
      bool immediately)
    {
      ContainerViewController containerViewController = this;
      if (!containerViewController.isInTransition)
      {
        containerViewController.isInTransition = true;
        foreach (ViewController viewController in viewControllersToRemove)
          viewController.isInTransition = true;
        EventSystem.current.SetSelectedGameObject((GameObject) null);
        ViewController[] viewControllers = containerViewController._viewControllers.ToArray();
        HashSet<ViewController> viewControllersToRemoveSet = new HashSet<ViewController>((IEnumerable<ViewController>) viewControllersToRemove);
        if (!immediately)
        {
          float transitionDuration = 0.4f;
          float elapsedTime = 0.0f;
          while ((double) elapsedTime < (double) transitionDuration)
          {
            animationLayouter(elapsedTime / transitionDuration, viewControllers, viewControllersToRemoveSet);
            elapsedTime += Time.deltaTime;
            yield return (object) null;
          }
        }
        animationLayouter(1f, viewControllers, viewControllersToRemoveSet);
        foreach (ViewController viewController in viewControllersToRemove)
        {
          viewController.__Deactivate(true, true, false);
          viewController.__ResetViewController();
        }
        containerViewController._viewControllers = containerViewController._viewControllers.Except<ViewController>((IEnumerable<ViewController>) viewControllersToRemove).ToList<ViewController>();
        containerViewController.isInTransition = false;
        foreach (ViewController viewController in viewControllersToRemove)
          viewController.isInTransition = false;
        Action action = finishedCallback;
        if (action != null)
          action();
      }
    }

    private float[] GetNewXPositionsForViewControllers(
      List<ViewController> viewControllers,
      int exludeFromEndCount)
    {
      int length = viewControllers.Count - exludeFromEndCount;
      float[] forViewControllers = new float[length];
      float num1 = 0.0f;
      Rect rect;
      for (int index = 0; index < length; ++index)
      {
        RectTransform component = viewControllers[index].GetComponent<RectTransform>();
        double num2 = (double) num1;
        rect = component.rect;
        double num3 = (double) rect.width * (double) component.localScale.x;
        num1 = (float) (num2 + num3);
      }
      float num4 = 0.05f;
      float num5 = (float) (-((double) num1 + (double) num4 * (double) (length - 1)) * 0.5);
      for (int index1 = 0; index1 < length; ++index1)
      {
        RectTransform component = viewControllers[index1].GetComponent<RectTransform>();
        float[] numArray = forViewControllers;
        int index2 = index1;
        double num6 = (double) num5;
        rect = component.rect;
        double num7 = (double) rect.width * (double) component.localScale.x * 0.5;
        double num8 = num6 + num7;
        numArray[index2] = (float) num8;
        double num9 = (double) num5;
        rect = component.rect;
        double num10 = (double) rect.width * (double) component.localScale.x + (double) num4;
        num5 = (float) (num9 + num10);
      }
      return forViewControllers;
    }
  }
}
