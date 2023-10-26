// Decompiled with JetBrains decompiler
// Type: HMUI.NavigationController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HMUI
{
  public class NavigationController : ContainerViewController
  {
    [SerializeField]
    protected NavigationController.Orientation _orientation;
    [SerializeField]
    protected bool _reversedStacking;
    [SerializeField]
    protected NavigationController.Alignment _alignment = NavigationController.Alignment.Middle;
    [SerializeField]
    protected float _edgeSize;
    [SerializeField]
    protected float _viewControllersSeparator;

    protected override void LayoutViewControllers(List<ViewController> viewControllers)
    {
      float[] forViewControllers = this.GetNewPositionsForViewControllers(viewControllers);
      int count = viewControllers.Count;
      for (int index = 0; index < count; ++index)
      {
        this.SetupViewControllerRect(viewControllers[index]);
        viewControllers[index].transform.localPosition = this.PositionVector(forViewControllers[index]);
      }
    }

    public virtual void PushViewController(
      ViewController viewController,
      Action finishedCallback,
      bool immediately = false)
    {
      float[] startPositions = (float[]) null;
      float[] endPositions = (float[]) null;
      this.AddViewController(viewController, finishedCallback, (Action<float, ViewController[]>) ((t, viewControllers) =>
      {
        if (startPositions == null)
        {
          this.SetupViewControllerRect(viewController);
          startPositions = new float[viewControllers.Length];
          startPositions[viewControllers.Length - 1] = this._orientation == NavigationController.Orientation.Horizontal ? 4f / this.transform.lossyScale.x : 4f / this.transform.lossyScale.y;
          for (int index = 0; index < viewControllers.Length - 1; ++index)
            startPositions[index] = this._orientation == NavigationController.Orientation.Horizontal ? this._viewControllers[index].transform.localPosition.x : this._viewControllers[index].transform.localPosition.y;
          endPositions = this.GetNewPositionsForViewControllers(this._viewControllers);
        }
        float t1 = Easing.OutQuart(t);
        for (int index = 0; index < viewControllers.Length; ++index)
        {
          float pos = Mathf.Lerp(startPositions[index], endPositions[index], t1);
          this._viewControllers[index].transform.localPosition = this.PositionVector(pos);
        }
      }), immediately);
    }

    public virtual void PopViewController(Action finishedCallback, bool immediately) => this.PopViewControllers(1, finishedCallback, immediately);

    public virtual void PopViewControllers(
      int numberOfViewControllersToPop,
      Action finishedCallback,
      bool immediately)
    {
      ViewController[] viewControllers1 = new ViewController[numberOfViewControllersToPop];
      for (int index = 0; index < numberOfViewControllersToPop; ++index)
        viewControllers1[index] = this._viewControllers[this._viewControllers.Count - index - 1];
      float[] startPositions = (float[]) null;
      float[] endPositions = (float[]) null;
      float moveOffset = this._orientation == NavigationController.Orientation.Horizontal ? 4f / this.transform.lossyScale.x : 4f / this.transform.lossyScale.y;
      this.RemoveViewControllers(viewControllers1, finishedCallback, (Action<float, ViewController[], HashSet<ViewController>>) ((t, viewControllers, removingViewControllers) =>
      {
        if (startPositions == null)
        {
          startPositions = new float[viewControllers.Length];
          for (int index = 0; index < viewControllers.Length; ++index)
            startPositions[index] = this._orientation == NavigationController.Orientation.Horizontal ? this._viewControllers[index].transform.localPosition.x : this._viewControllers[index].transform.localPosition.y;
          endPositions = this.GetNewPositionsForViewControllers(this._viewControllers, removingViewControllers, moveOffset);
        }
        float num1 = 1f - Easing.OutQuart(1f - t);
        float num2 = Easing.OutQuart(t);
        for (int index = 0; index < viewControllers.Length; ++index)
        {
          ViewController viewController = viewControllers[index];
          float pos = Mathf.Lerp(startPositions[index], endPositions[index], removingViewControllers.Contains(viewController) ? num2 : num1);
          viewController.transform.localPosition = this.PositionVector(pos);
        }
      }), immediately);
    }

    public virtual Vector3 PositionVector(float pos) => this._orientation != NavigationController.Orientation.Horizontal ? new Vector3(0.0f, pos, 0.0f) : new Vector3(pos, 0.0f, 0.0f);

    public virtual void SetupViewControllerRect(ViewController viewController) => viewController.rectTransform.pivot = new Vector2(0.5f, 0.5f);

    public virtual float[] GetNewPositionsForViewControllers(
      List<ViewController> viewControllers,
      HashSet<ViewController> fixedViewControllers = null,
      float fixedEndPos = 0.0f)
    {
      int count = viewControllers.Count;
      float[] forViewControllers = new float[viewControllers.Count];
      float num1 = 0.0f;
      foreach (ViewController viewController in viewControllers)
      {
        if (fixedViewControllers == null || !fixedViewControllers.Contains(viewController))
        {
          RectTransform rectTransform = viewController.rectTransform;
          num1 += this._orientation == NavigationController.Orientation.Horizontal ? rectTransform.rect.width * rectTransform.localScale.x : rectTransform.rect.height * rectTransform.localScale.y;
        }
      }
      float num2 = this._orientation == NavigationController.Orientation.Horizontal ? this.controllersContainer.rect.size.x : this.controllersContainer.rect.size.y;
      float num3 = 0.0f;
      switch (this._alignment)
      {
        case NavigationController.Alignment.Beginning:
          num3 = (float) (-(double) num2 * 0.5) + this._edgeSize;
          break;
        case NavigationController.Alignment.Middle:
          num3 = (float) (-((double) num1 + (double) this._viewControllersSeparator * (double) (viewControllers.Count - 1)) * 0.5);
          break;
        case NavigationController.Alignment.End:
          num3 = (float) ((double) num2 * 0.5 - (double) this._edgeSize - ((double) num1 + (double) this._viewControllersSeparator * (double) (viewControllers.Count - 1)));
          break;
      }
      float num4 = this._reversedStacking ? -1f : 1f;
      if (this._orientation == NavigationController.Orientation.Vertical)
        num3 = -num4 * num3;
      for (int index = 0; index < viewControllers.Count; ++index)
      {
        ViewController viewController = viewControllers[index];
        if (fixedViewControllers != null && fixedViewControllers.Contains(viewController))
        {
          forViewControllers[index] = fixedEndPos;
        }
        else
        {
          RectTransform component = viewControllers[index].GetComponent<RectTransform>();
          float num5 = this._orientation == NavigationController.Orientation.Horizontal ? component.rect.width * component.localScale.x : component.rect.height * component.localScale.y;
          if (this._orientation == NavigationController.Orientation.Horizontal)
          {
            forViewControllers[index] = num3 + (float) ((double) num4 * (double) num5 * 0.5);
            num3 += num4 * (num5 + this._viewControllersSeparator);
          }
          else
          {
            forViewControllers[index] = num3 - (float) ((double) num4 * (double) num5 * 0.5);
            num3 -= num4 * (num5 + this._viewControllersSeparator);
          }
        }
      }
      return forViewControllers;
    }

    public enum Orientation
    {
      Horizontal,
      Vertical,
    }

    public enum Alignment
    {
      Beginning,
      Middle,
      End,
    }
  }
}
