// Decompiled with JetBrains decompiler
// Type: HMUI.StackedController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HMUI
{
  public class StackedController : ContainerViewController
  {
    public ViewController topStackedViewController => this.viewControllers.Count == 0 ? (ViewController) null : this.viewControllers[this.viewControllers.Count - 1];

    protected override void LayoutViewControllers(List<ViewController> viewControllers)
    {
      int count = viewControllers.Count;
      for (int index = 0; index < count; ++index)
      {
        StackedController.SetupViewControllerRect(viewControllers[index], index);
        viewControllers[index].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        viewControllers[index].enableUserInteractions = index == count - 1;
      }
    }

    public virtual void PushViewController(
      ViewController viewController,
      Action finishedCallback,
      bool immediately = false)
    {
      this.AddViewController(viewController, finishedCallback, (Action<float, ViewController[]>) ((t, viewControllers) =>
      {
        for (int index = 0; index < viewControllers.Length; ++index)
        {
          viewControllers[index].rectTransform.anchoredPosition3D = new Vector3(0.0f, 0.0f, 0.0f);
          viewControllers[index].enableUserInteractions = index == viewControllers.Length - 1;
        }
      }), immediately);
    }

    public virtual void PopViewController(Action finishedCallback = null, bool immediately = false) => this.PopViewControllers(1, finishedCallback, immediately);

    public virtual void PopViewControllers(
      int numberOfViewControllersToPop,
      Action finishedCallback = null,
      bool immediately = false)
    {
      ViewController[] viewControllers1 = new ViewController[numberOfViewControllersToPop];
      for (int index = 0; index < numberOfViewControllersToPop; ++index)
        viewControllers1[index] = this._viewControllers[this._viewControllers.Count - index - 1];
      this.RemoveViewControllers(viewControllers1, finishedCallback, (Action<float, ViewController[], HashSet<ViewController>>) ((t, viewControllers, removingViewControllers) =>
      {
        for (int index = 0; index < viewControllers.Length; ++index)
          viewControllers[index].enableUserInteractions = index == viewControllers.Length - numberOfViewControllersToPop - 1;
      }), immediately);
    }

    private static void SetupViewControllerRect(ViewController viewController, int index) => viewController.rectTransform.pivot = new Vector2(0.5f, 0.5f);
  }
}
