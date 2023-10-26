// Decompiled with JetBrains decompiler
// Type: HMUI.FlowCoordinator
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace HMUI
{
  public abstract class FlowCoordinator : MonoBehaviour
  {
    [Inject]
    private readonly BaseInputModule _baseInputModule;
    private ScreenSystem _screenSystem;
    private FlowCoordinator _parentFlowCoordinator;
    private FlowCoordinator _childFlowCoordinator;
    private List<ViewController> _mainScreenViewControllers = new List<ViewController>();
    private ViewController _leftScreenViewController;
    private ViewController _rightScreenViewController;
    private ViewController _bottomScreenViewController;
    private ViewController _topScreenViewController;
    private bool _wasActivatedBefore;
    private string _title;
    private ViewController _providedMainViewController;
    private ViewController _providedLeftScreenViewController;
    private ViewController _providedRightScreenViewController;
    private ViewController _providedBottomScreenViewController;
    private ViewController _providedTopScreenViewController;
    private bool _viewControllersWereProvided;
    private bool _isInDidActivatePhase;
    private bool _isActivated;
    private bool _isInTransition;
    private bool _showBackButton;
    private EventSystem _prevEventSystem;

    public ViewController topViewController => this._mainScreenViewControllers.Count <= 0 ? (ViewController) null : this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1];

    public FlowCoordinator childFlowCoordinator => this._childFlowCoordinator;

    public bool isActivated => this._isActivated;

    protected string title => this._title;

    protected void SetTitle(string value, ViewController.AnimationType animationType = ViewController.AnimationType.In)
    {
      this._title = value;
      if (!this.IsFlowCoordinatorInHierarchy(this) || this._isInDidActivatePhase)
        return;
      this.PresentTitle(value, animationType);
    }

    protected bool showBackButton
    {
      set => this._showBackButton = value;
      get => this._showBackButton;
    }

    public void __StartOnScreenSystem(ScreenSystem screenSystem)
    {
      this._screenSystem = screenSystem;
      bool wasActivatedBefore = this._wasActivatedBefore;
      this._wasActivatedBefore = true;
      this._viewControllersWereProvided = false;
      this._isInDidActivatePhase = true;
      this.Activate(!wasActivatedBefore, true, false);
      this._isInDidActivatePhase = false;
      this.SetLeftScreenViewController(this._providedLeftScreenViewController, ViewController.AnimationType.None);
      this.SetRightScreenViewController(this._providedRightScreenViewController, ViewController.AnimationType.None);
      this.SetBottomScreenViewController(this._providedBottomScreenViewController, ViewController.AnimationType.None);
      if ((UnityEngine.Object) this._providedTopScreenViewController != (UnityEngine.Object) null)
        this.SetTopScreenViewController(this._providedTopScreenViewController, ViewController.AnimationType.None);
      else
        this.PresentTitle(this._title, ViewController.AnimationType.None);
      this.PresentViewController(this._providedMainViewController, new Action(this.InitialViewControllerWasPresented), immediately: true);
    }

    protected void PresentFlowCoordinator(
      FlowCoordinator flowCoordinator,
      Action finishedCallback = null,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      bool immediately = false,
      bool replaceTopViewController = false)
    {
      if (replaceTopViewController)
        this.TopViewControllerWillChange(this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1], this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 2], immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      this._childFlowCoordinator = flowCoordinator;
      flowCoordinator._parentFlowCoordinator = this;
      flowCoordinator._screenSystem = this._screenSystem;
      bool wasActivatedBefore = flowCoordinator._wasActivatedBefore;
      flowCoordinator._wasActivatedBefore = true;
      this.Deactivate(false, false);
      flowCoordinator._viewControllersWereProvided = false;
      this._isInDidActivatePhase = true;
      flowCoordinator.Activate(!wasActivatedBefore, true, false);
      this._isInDidActivatePhase = false;
      flowCoordinator.SetLeftScreenViewController(flowCoordinator._providedLeftScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      flowCoordinator.SetRightScreenViewController(flowCoordinator._providedRightScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      flowCoordinator.SetBottomScreenViewController(flowCoordinator._providedBottomScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      if ((UnityEngine.Object) flowCoordinator._providedTopScreenViewController != (UnityEngine.Object) null)
        flowCoordinator.SetTopScreenViewController(flowCoordinator._providedTopScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      else
        flowCoordinator.PresentTitle(flowCoordinator._title, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      if (replaceTopViewController)
      {
        ViewController screenViewController = this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1];
        flowCoordinator.TopViewControllerWillChange(screenViewController, flowCoordinator._providedMainViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
        this._screenSystem.SetBackButton(flowCoordinator.showBackButton, !immediately);
        if (!flowCoordinator._providedMainViewController.isInTransition && !this._isInTransition)
          this.TransitionDidStart();
        flowCoordinator.ReplaceTopViewController(flowCoordinator._providedMainViewController, this, flowCoordinator, (Action) (() =>
        {
          Action action = finishedCallback;
          if (action != null)
            action();
          flowCoordinator.InitialViewControllerWasPresented();
          if (flowCoordinator._providedMainViewController.isInTransition || !this._isInTransition)
            return;
          this.TransitionDidFinish();
        }), immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In, animationDirection);
      }
      else
      {
        if (!flowCoordinator._providedMainViewController.isInTransition && !this._isInTransition)
          this.TransitionDidStart();
        flowCoordinator.PresentViewController(flowCoordinator._providedMainViewController, (Action) (() =>
        {
          Action action = finishedCallback;
          if (action != null)
            action();
          flowCoordinator.InitialViewControllerWasPresented();
          if (flowCoordinator._providedMainViewController.isInTransition || !this._isInTransition)
            return;
          this.TransitionDidFinish();
        }), animationDirection, immediately);
      }
    }

    protected void DismissFlowCoordinator(
      FlowCoordinator flowCoordinator,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      Action finishedCallback = null,
      bool immediately = false)
    {
      flowCoordinator._parentFlowCoordinator = (FlowCoordinator) null;
      this._childFlowCoordinator = (FlowCoordinator) null;
      ViewController viewController = flowCoordinator._mainScreenViewControllers[0];
      flowCoordinator._mainScreenViewControllers.Remove(viewController);
      if (!immediately)
        this.SetGlobalUserInteraction(false);
      if (!viewController.isInTransition && !this._isInTransition)
      {
        flowCoordinator.TransitionDidStart();
        this.TransitionDidStart();
      }
      viewController.__DismissViewController((Action) (() =>
      {
        if (immediately)
          return;
        this.SetGlobalUserInteraction(true);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (viewController.isInTransition || !this._isInTransition)
          return;
        flowCoordinator.TransitionDidFinish();
        this.TransitionDidFinish();
      }), animationDirection, immediately);
      flowCoordinator.Deactivate(true, false);
      flowCoordinator._screenSystem = (ScreenSystem) null;
      this.SetLeftScreenViewController(this._leftScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.Out);
      this.SetRightScreenViewController(this._rightScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.Out);
      this.SetBottomScreenViewController(this._bottomScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.Out);
      this.SetTopScreenViewController(this._topScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.Out);
      this._viewControllersWereProvided = false;
      this._isInDidActivatePhase = true;
      this.Activate(false, false, false);
      this._isInDidActivatePhase = false;
      this.PresentTitle(this._title, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.Out);
      this._screenSystem.SetBackButton(this.showBackButton, !immediately);
      if (!immediately)
        return;
      Action action1 = finishedCallback;
      if (action1 != null)
        action1();
      if (viewController.isInTransition || !this._isInTransition)
        return;
      this.TransitionDidFinish();
      flowCoordinator.TransitionDidFinish();
    }

    protected void ReplaceChildFlowCoordinator(
      FlowCoordinator flowCoordinator,
      Action finishedCallback = null,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      bool immediately = false)
    {
      FlowCoordinator flowCoordinatorToReplace = this.childFlowCoordinator;
      this._childFlowCoordinator = flowCoordinator;
      flowCoordinator._parentFlowCoordinator = this;
      flowCoordinator._screenSystem = this._screenSystem;
      bool wasActivatedBefore = flowCoordinator._wasActivatedBefore;
      flowCoordinator._wasActivatedBefore = true;
      flowCoordinatorToReplace.Deactivate(true, false);
      flowCoordinatorToReplace._parentFlowCoordinator = (FlowCoordinator) null;
      flowCoordinatorToReplace._screenSystem = (ScreenSystem) null;
      flowCoordinator._viewControllersWereProvided = false;
      this._isInDidActivatePhase = true;
      flowCoordinator.Activate(!wasActivatedBefore, true, false);
      this._isInDidActivatePhase = false;
      flowCoordinator.SetLeftScreenViewController(flowCoordinator._providedLeftScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      flowCoordinator.SetRightScreenViewController(flowCoordinator._providedRightScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      flowCoordinator.SetBottomScreenViewController(flowCoordinator._providedBottomScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      if ((UnityEngine.Object) flowCoordinator._providedTopScreenViewController != (UnityEngine.Object) null)
        flowCoordinator.SetTopScreenViewController(flowCoordinator._providedTopScreenViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      else
        flowCoordinator.PresentTitle(flowCoordinator._title, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      ViewController screenViewController = flowCoordinatorToReplace._mainScreenViewControllers[flowCoordinatorToReplace._mainScreenViewControllers.Count - 1];
      flowCoordinator.TopViewControllerWillChange(screenViewController, flowCoordinator._providedMainViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      this._screenSystem.SetBackButton(flowCoordinator.showBackButton, !immediately);
      if (!flowCoordinator._providedMainViewController.isInTransition && !this._isInTransition)
      {
        this.TransitionDidStart();
        flowCoordinatorToReplace.TransitionDidStart();
        flowCoordinator.TransitionDidStart();
      }
      flowCoordinator.ReplaceTopViewController(flowCoordinator._providedMainViewController, flowCoordinatorToReplace, flowCoordinator, (Action) (() =>
      {
        Action action = finishedCallback;
        if (action != null)
          action();
        flowCoordinator.InitialViewControllerWasPresented();
        if (flowCoordinator._providedMainViewController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
        flowCoordinatorToReplace.TransitionDidFinish();
        flowCoordinator.TransitionDidFinish();
      }), immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In, animationDirection);
    }

    protected void PresentViewController(
      ViewController viewController,
      Action finishedCallback = null,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      bool immediately = false)
    {
      if (this._mainScreenViewControllers.Count > 0)
        this.TopViewControllerWillChange(this._mainScreenViewControllers[0], viewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
      this._screenSystem.SetBackButton(this.showBackButton, !immediately);
      if (this._mainScreenViewControllers.Count == 0 && (UnityEngine.Object) this._parentFlowCoordinator == (UnityEngine.Object) null)
      {
        this._mainScreenViewControllers.Add(viewController);
        if (!viewController.isInTransition && !this._isInTransition)
          this.TransitionDidStart();
        this._screenSystem.mainScreen.SetRootViewController(viewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (viewController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
      }
      else
      {
        ViewController topViewController = this._mainScreenViewControllers.Count != 0 ? this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1] : this._parentFlowCoordinator._mainScreenViewControllers[this._parentFlowCoordinator._mainScreenViewControllers.Count - 1];
        this._mainScreenViewControllers.Add(viewController);
        if (!immediately)
          this.SetGlobalUserInteraction(false);
        if (!topViewController.isInTransition && !this._isInTransition)
          this.TransitionDidStart();
        topViewController.__PresentViewController(viewController, (Action) (() =>
        {
          if (!immediately)
            this.SetGlobalUserInteraction(true);
          Action action = finishedCallback;
          if (action != null)
            action();
          if (topViewController.isInTransition || !this._isInTransition)
            return;
          this.TransitionDidFinish();
        }), animationDirection, immediately);
      }
    }

    protected void DismissViewController(
      ViewController viewController,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal,
      Action finishedCallback = null,
      bool immediately = false)
    {
      ViewController newTopViewController = this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 2];
      this.TopViewControllerWillChange(viewController, newTopViewController, immediately ? ViewController.AnimationType.None : ViewController.AnimationType.Out);
      this._screenSystem.SetBackButton(this.showBackButton, !immediately);
      this._mainScreenViewControllers.Remove(viewController);
      if (!immediately)
        this.SetGlobalUserInteraction(false);
      if (!newTopViewController.isInTransition && !this._isInTransition)
        this.TransitionDidStart();
      viewController.__DismissViewController((Action) (() =>
      {
        if (!immediately)
          this.SetGlobalUserInteraction(true);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (newTopViewController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
      }), animationDirection, immediately);
    }

    protected void ReplaceTopViewController(
      ViewController viewController,
      Action finishedCallback = null,
      ViewController.AnimationType animationType = ViewController.AnimationType.In,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal)
    {
      this.TopViewControllerWillChange(this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1], viewController, animationType);
      this._screenSystem.SetBackButton(this.showBackButton, animationType != 0);
      this.ReplaceTopViewController(viewController, this, this, finishedCallback, animationType, animationDirection);
    }

    private void ReplaceTopViewController(
      ViewController viewController,
      FlowCoordinator originalOwnerFlowCoordinator,
      FlowCoordinator newOwnerFlowCoordinator,
      Action finishedCallback = null,
      ViewController.AnimationType animationType = ViewController.AnimationType.In,
      ViewController.AnimationDirection animationDirection = ViewController.AnimationDirection.Horizontal)
    {
      ViewController originalTopViewController = originalOwnerFlowCoordinator._mainScreenViewControllers[originalOwnerFlowCoordinator._mainScreenViewControllers.Count - 1];
      originalOwnerFlowCoordinator._mainScreenViewControllers.Remove(originalTopViewController);
      newOwnerFlowCoordinator._mainScreenViewControllers.Add(viewController);
      if (animationType != ViewController.AnimationType.None)
        this.SetGlobalUserInteraction(false);
      if (!originalTopViewController.isInTransition && !this._isInTransition)
        this.TransitionDidStart();
      originalTopViewController.__ReplaceViewController(viewController, (Action) (() =>
      {
        if (animationType != ViewController.AnimationType.None)
          this.SetGlobalUserInteraction(true);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (originalTopViewController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
      }), animationType, animationDirection);
    }

    protected void PushViewControllerToNavigationController(
      NavigationController navigationController,
      ViewController viewController,
      Action finishedCallback = null,
      bool immediately = false)
    {
      if (!immediately)
        this.SetGlobalUserInteraction(false);
      if (!viewController.isInTransition && !this._isInTransition)
        this.TransitionDidStart();
      navigationController.PushViewController(viewController, (Action) (() =>
      {
        if (!immediately)
          this.SetGlobalUserInteraction(true);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (viewController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
      }), immediately);
    }

    protected void SetViewControllersToNavigationController(
      NavigationController navigationController,
      params ViewController[] viewControllers)
    {
      navigationController.SetChildViewControllers(viewControllers);
    }

    protected void SetViewControllerToNavigationController(
      NavigationController navigationController,
      ViewController viewController)
    {
      navigationController.SetChildViewController(viewController);
    }

    protected void PopViewControllerFromNavigationController(
      NavigationController navigationController,
      Action finishedCallback = null,
      bool immediately = false)
    {
      if (!immediately)
        this.SetGlobalUserInteraction(false);
      if (!navigationController.isInTransition && !this._isInTransition)
        this.TransitionDidStart();
      navigationController.PopViewController((Action) (() =>
      {
        if (!immediately)
          this.SetGlobalUserInteraction(true);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (navigationController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
      }), immediately);
    }

    protected void PopViewControllersFromNavigationController(
      NavigationController navigationController,
      int numberOfControllers,
      Action finishedCallback = null,
      bool immediately = false)
    {
      if (!immediately)
        this.SetGlobalUserInteraction(false);
      if (!navigationController.isInTransition && !this._isInTransition)
        this.TransitionDidStart();
      navigationController.PopViewControllers(numberOfControllers, (Action) (() =>
      {
        if (!immediately)
          this.SetGlobalUserInteraction(true);
        Action action = finishedCallback;
        if (action != null)
          action();
        if (navigationController.isInTransition || !this._isInTransition)
          return;
        this.TransitionDidFinish();
      }), immediately);
    }

    protected void SetLeftScreenViewController(
      ViewController viewController,
      ViewController.AnimationType animationType)
    {
      if ((UnityEngine.Object) this._screenSystem == (UnityEngine.Object) null || (UnityEngine.Object) this._childFlowCoordinator != (UnityEngine.Object) null)
      {
        this._leftScreenViewController = viewController;
      }
      else
      {
        this._leftScreenViewController = viewController;
        this._screenSystem.leftScreen.SetRootViewController(viewController, animationType);
      }
    }

    protected void SetRightScreenViewController(
      ViewController viewController,
      ViewController.AnimationType animationType)
    {
      if ((UnityEngine.Object) this._screenSystem == (UnityEngine.Object) null || (UnityEngine.Object) this._childFlowCoordinator != (UnityEngine.Object) null)
      {
        this._rightScreenViewController = viewController;
      }
      else
      {
        this._rightScreenViewController = viewController;
        this._screenSystem.rightScreen.SetRootViewController(viewController, animationType);
      }
    }

    protected void SetBottomScreenViewController(
      ViewController viewController,
      ViewController.AnimationType animationType)
    {
      if ((UnityEngine.Object) this._screenSystem == (UnityEngine.Object) null || (UnityEngine.Object) this._childFlowCoordinator != (UnityEngine.Object) null)
      {
        this._bottomScreenViewController = viewController;
      }
      else
      {
        this._bottomScreenViewController = viewController;
        this._screenSystem.bottomScreen.SetRootViewController(viewController, animationType);
      }
    }

    protected void SetTopScreenViewController(
      ViewController viewController,
      ViewController.AnimationType animationType)
    {
      if ((UnityEngine.Object) this._screenSystem == (UnityEngine.Object) null || (UnityEngine.Object) this._childFlowCoordinator != (UnityEngine.Object) null)
      {
        this._topScreenViewController = viewController;
      }
      else
      {
        this._topScreenViewController = viewController;
        this._screenSystem.topScreen.SetRootViewController(viewController, animationType);
      }
    }

    private void PresentTitle(string title, ViewController.AnimationType animationType)
    {
      if (string.IsNullOrEmpty(title))
      {
        this.SetTopScreenViewController((ViewController) null, animationType);
      }
      else
      {
        this._screenSystem.titleViewController.SetText(title);
        this.SetTopScreenViewController((ViewController) this._screenSystem.titleViewController, animationType);
      }
    }

    public bool IsFlowCoordinatorInHierarchy(FlowCoordinator flowCoordinator) => (UnityEngine.Object) flowCoordinator._parentFlowCoordinator != (UnityEngine.Object) null || (UnityEngine.Object) flowCoordinator._screenSystem != (UnityEngine.Object) null;

    public FlowCoordinator YoungestChildFlowCoordinatorOrSelf()
    {
      FlowCoordinator flowCoordinator = this;
      while ((UnityEngine.Object) flowCoordinator.childFlowCoordinator != (UnityEngine.Object) null)
        flowCoordinator = flowCoordinator.childFlowCoordinator;
      return flowCoordinator;
    }

    protected void ProvideInitialViewControllers(
      ViewController mainViewController,
      ViewController leftScreenViewController = null,
      ViewController rightScreenViewController = null,
      ViewController bottomScreenViewController = null,
      ViewController topScreenViewController = null)
    {
      this._providedLeftScreenViewController = leftScreenViewController;
      this._providedRightScreenViewController = rightScreenViewController;
      this._providedBottomScreenViewController = bottomScreenViewController;
      this._providedTopScreenViewController = topScreenViewController;
      this._providedMainViewController = mainViewController;
      this._viewControllersWereProvided = true;
    }

    private void Activate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
      this._screenSystem.backButtonWasPressedEvent += new Action(this.HandleScreenSystemBackButtonWasPressed);
      this._isActivated = true;
      this.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    }

    private void Deactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      this._screenSystem.backButtonWasPressedEvent -= new Action(this.HandleScreenSystemBackButtonWasPressed);
      this._isActivated = false;
      this.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
    }

    public void __ExternalActivate()
    {
      this.Activate(false, false, true);
      if ((UnityEngine.Object) this.topViewController != (UnityEngine.Object) null && !this.topViewController.isActivated)
        this.topViewController.__Activate(false, true);
      if ((UnityEngine.Object) this._leftScreenViewController != (UnityEngine.Object) null && !this._leftScreenViewController.isActivated)
        this._leftScreenViewController.__Activate(false, true);
      if ((UnityEngine.Object) this._rightScreenViewController != (UnityEngine.Object) null && !this._rightScreenViewController.isActivated)
        this._rightScreenViewController.__Activate(false, true);
      if ((UnityEngine.Object) this._bottomScreenViewController != (UnityEngine.Object) null && !this._bottomScreenViewController.isActivated)
        this._bottomScreenViewController.__Activate(false, true);
      if (!((UnityEngine.Object) this._topScreenViewController != (UnityEngine.Object) null) || this._topScreenViewController.isActivated)
        return;
      this._topScreenViewController.__Activate(false, true);
    }

    public void __ExternalDeactivate()
    {
      this.Deactivate(false, true);
      if ((UnityEngine.Object) this.topViewController != (UnityEngine.Object) null && this.topViewController.isActivated)
        this.topViewController.__Deactivate(false, false, true);
      if ((UnityEngine.Object) this._leftScreenViewController != (UnityEngine.Object) null && this._leftScreenViewController.isActivated)
        this._leftScreenViewController.__Deactivate(false, false, true);
      if ((UnityEngine.Object) this._rightScreenViewController != (UnityEngine.Object) null && this._rightScreenViewController.isActivated)
        this._rightScreenViewController.__Deactivate(false, false, true);
      if ((UnityEngine.Object) this._bottomScreenViewController != (UnityEngine.Object) null && this._bottomScreenViewController.isActivated)
        this._bottomScreenViewController.__Deactivate(false, false, true);
      if (!((UnityEngine.Object) this._topScreenViewController != (UnityEngine.Object) null) || !this._topScreenViewController.isActivated)
        return;
      this._topScreenViewController.__Deactivate(false, false, true);
    }

    private void HandleScreenSystemBackButtonWasPressed() => this.BackButtonWasPressed(this.topViewController);

    protected void SetGlobalUserInteraction(bool value) => this._baseInputModule.enabled = value;

    protected abstract void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling);

    protected virtual void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
    }

    protected virtual void TransitionDidStart() => this._isInTransition = true;

    protected virtual void TransitionDidFinish() => this._isInTransition = false;

    protected virtual void TopViewControllerWillChange(
      ViewController oldViewController,
      ViewController newViewController,
      ViewController.AnimationType animationType)
    {
    }

    protected virtual void InitialViewControllerWasPresented()
    {
    }

    protected virtual void BackButtonWasPressed(ViewController topViewController)
    {
    }

    [Conditional("FlowCoordinatorLog")]
    private static void Log(string message) => UnityEngine.Debug.Log((object) message);
  }
}
