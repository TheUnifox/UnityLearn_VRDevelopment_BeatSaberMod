// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorFlowCoordinator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorFlowCoordinator : MonoBehaviour
  {
    private BeatmapEditorScreenSystem _screenSystem;
    private BeatmapEditorFlowCoordinator _parentFlowCoordinator;
    private BeatmapEditorFlowCoordinator _childFlowCoordinator;
    private BeatmapEditorViewController _mainViewController;
    private BeatmapEditorViewController _bottomViewController;
    private BeatmapEditorViewController _navbarViewController;
    private BeatmapEditorViewController _dialogViewController;
    private BeatmapEditorViewController _providedMainViewController;
    private BeatmapEditorViewController _providedNavbarViewController;
    private BeatmapEditorViewController _providedDialogViewController;
    private readonly List<BeatmapEditorViewController> _mainScreenViewControllers = new List<BeatmapEditorViewController>();
    private bool _wasActivatedBefore;
    private bool _viewControllerWasProvided;
    private bool _isInDidActivatePhase;
    private bool _isActivated;
    private bool _isInTransition;
    private bool _showBackButton;
    private bool _isBeingDestroyed;

    protected BeatmapEditorViewController topViewController => this._mainScreenViewControllers.Count <= 0 ? (BeatmapEditorViewController) null : this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1];

    protected bool showBackButton
    {
      get => this._showBackButton;
      set => this._showBackButton = value;
    }

    protected void OnDestroy() => this._isBeingDestroyed = true;

    public void __StartOnScreenSystem(BeatmapEditorScreenSystem screenSystem)
    {
      this._screenSystem = screenSystem;
      bool wasActivatedBefore = this._wasActivatedBefore;
      this._wasActivatedBefore = true;
      this._viewControllerWasProvided = false;
      this._isInDidActivatePhase = true;
      this.Activate(!wasActivatedBefore, true, false);
      this._isInDidActivatePhase = false;
      this.SetNavbarScreenViewController(this._providedNavbarViewController);
      this.SetDialogScreenViewController(this._providedDialogViewController);
      this.PresentViewController(this._providedMainViewController);
      this.InitialViewControllerWasPresented();
    }

    protected void PresentFlowCoordinator(
      BeatmapEditorFlowCoordinator flowCoordinator,
      bool replaceTopViewController)
    {
      if (replaceTopViewController)
        this.TopViewControllerWillChange(this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1], this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 2]);
      this._childFlowCoordinator = flowCoordinator;
      flowCoordinator._parentFlowCoordinator = this;
      flowCoordinator._screenSystem = this._screenSystem;
      bool wasActivatedBefore = flowCoordinator._wasActivatedBefore;
      flowCoordinator._wasActivatedBefore = true;
      this.Deactivate(false, false);
      flowCoordinator._viewControllerWasProvided = false;
      this._isInDidActivatePhase = true;
      flowCoordinator.Activate(!wasActivatedBefore, true, false);
      this._isInDidActivatePhase = false;
      flowCoordinator.SetNavbarScreenViewController(flowCoordinator._providedNavbarViewController);
      flowCoordinator.SetDialogScreenViewController(flowCoordinator._providedDialogViewController);
      if (replaceTopViewController)
      {
        BeatmapEditorViewController screenViewController = this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1];
        flowCoordinator.TopViewControllerWillChange(screenViewController, flowCoordinator._providedMainViewController);
        this._screenSystem.SetBackButton(flowCoordinator.showBackButton);
        flowCoordinator.ReplaceTopViewController(flowCoordinator._providedMainViewController);
        flowCoordinator.InitialViewControllerWasPresented();
      }
      else
      {
        flowCoordinator.PresentViewController(flowCoordinator._providedMainViewController);
        flowCoordinator.InitialViewControllerWasPresented();
      }
    }

    protected void DismissFlowCoordinator(BeatmapEditorFlowCoordinator flowCoordinator)
    {
      flowCoordinator._parentFlowCoordinator = (BeatmapEditorFlowCoordinator) null;
      this._childFlowCoordinator = (BeatmapEditorFlowCoordinator) null;
      BeatmapEditorViewController screenViewController = flowCoordinator._mainScreenViewControllers[0];
      flowCoordinator._mainScreenViewControllers.Remove(screenViewController);
      screenViewController.__DismissViewController();
      flowCoordinator.Deactivate(true, false);
      flowCoordinator._screenSystem = (BeatmapEditorScreenSystem) null;
      this.SetNavbarScreenViewController(this._navbarViewController);
      this.SetDialogScreenViewController(this._dialogViewController);
      this._viewControllerWasProvided = false;
      this._isInDidActivatePhase = true;
      this.Activate(false, false, false);
      this._isInDidActivatePhase = false;
      this._screenSystem.SetBackButton(this.showBackButton);
    }

    protected void ReplaceChildFlowCoordinator(BeatmapEditorFlowCoordinator flowCoordinator)
    {
      BeatmapEditorFlowCoordinator childFlowCoordinator = this._childFlowCoordinator;
      this._childFlowCoordinator = flowCoordinator;
      flowCoordinator._parentFlowCoordinator = this;
      flowCoordinator._screenSystem = this._screenSystem;
      bool wasActivatedBefore = flowCoordinator._wasActivatedBefore;
      flowCoordinator._wasActivatedBefore = true;
      childFlowCoordinator.Deactivate(true, false);
      childFlowCoordinator._parentFlowCoordinator = (BeatmapEditorFlowCoordinator) null;
      childFlowCoordinator._screenSystem = (BeatmapEditorScreenSystem) null;
      flowCoordinator._viewControllerWasProvided = false;
      this._isInDidActivatePhase = true;
      flowCoordinator.Activate(!wasActivatedBefore, true, false);
      this._isInDidActivatePhase = false;
      flowCoordinator.SetNavbarScreenViewController(flowCoordinator._providedNavbarViewController);
      flowCoordinator.SetDialogScreenViewController(flowCoordinator._providedDialogViewController);
      BeatmapEditorViewController screenViewController = childFlowCoordinator._mainScreenViewControllers[childFlowCoordinator._mainScreenViewControllers.Count - 1];
      flowCoordinator.TopViewControllerWillChange(screenViewController, flowCoordinator._providedMainViewController);
      this._screenSystem.SetBackButton(flowCoordinator.showBackButton);
      flowCoordinator.ReplaceTopViewController(flowCoordinator._providedMainViewController);
      flowCoordinator.InitialViewControllerWasPresented();
    }

    protected void PresentViewController(BeatmapEditorViewController viewController)
    {
      if (this._mainScreenViewControllers.Count > 0)
        this.TopViewControllerWillChange(this._mainScreenViewControllers[0], viewController);
      this._screenSystem.SetBackButton(this.showBackButton);
      if (this._mainScreenViewControllers.Count == 0 && (UnityEngine.Object) this._parentFlowCoordinator == (UnityEngine.Object) null)
      {
        this._mainScreenViewControllers.Add(viewController);
        this._screenSystem.mainScreen.SetRootViewController(viewController);
      }
      else
      {
        BeatmapEditorViewController editorViewController = this._mainScreenViewControllers.Count == 0 ? this._parentFlowCoordinator._mainScreenViewControllers[this._parentFlowCoordinator._mainScreenViewControllers.Count - 1] : this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1];
        this._mainScreenViewControllers.Add(viewController);
        BeatmapEditorViewController newViewController = viewController;
        editorViewController.__PresentViewController(newViewController);
      }
    }

    protected void DismissViewController(BeatmapEditorViewController viewController)
    {
      BeatmapEditorViewController screenViewController = this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 2];
      this.TopViewControllerWillChange(viewController, screenViewController);
      this._screenSystem.SetBackButton(this.showBackButton);
      this._mainScreenViewControllers.Remove(viewController);
      viewController.__DismissViewController();
    }

    protected void ReplaceTopViewController(BeatmapEditorViewController viewController)
    {
      this.TopViewControllerWillChange(this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1], viewController);
      this._screenSystem.SetBackButton(this.showBackButton);
      BeatmapEditorViewController screenViewController = this._mainScreenViewControllers[this._mainScreenViewControllers.Count - 1];
      this._mainScreenViewControllers.Remove(screenViewController);
      this._mainScreenViewControllers.Add(viewController);
      screenViewController.__ReplaceViewController(viewController);
    }

    protected void SetNavbarScreenViewController(BeatmapEditorViewController viewController)
    {
      this._navbarViewController = viewController;
      if ((UnityEngine.Object) this._screenSystem == (UnityEngine.Object) null || (UnityEngine.Object) this._childFlowCoordinator != (UnityEngine.Object) null)
        return;
      this._screenSystem.navbarScreen.SetRootViewController(this._navbarViewController);
    }

    protected void SetDialogScreenViewController(BeatmapEditorViewController dialogViewController)
    {
      this._dialogViewController = dialogViewController;
      if ((UnityEngine.Object) this._screenSystem == (UnityEngine.Object) null || (UnityEngine.Object) this._childFlowCoordinator != (UnityEngine.Object) null)
        return;
      this._screenSystem.dialogScreen.SetRootViewController(this._dialogViewController);
    }

    protected void SetBackButtonIconType(BackButtonView.BackButtonType type) => this._screenSystem.SetBackButtonIconType(type);

    protected void SetBackButtonIsDirtyNotification(bool isDirty) => this._screenSystem.SetBackButtonIsDirtyNotification(isDirty);

    protected void ProvideInitialViewControllers(
      BeatmapEditorViewController mainScreenViewController,
      BeatmapEditorViewController navbarScreenViewController = null,
      BeatmapEditorViewController dialogScreenViewController = null)
    {
      this._providedMainViewController = mainScreenViewController;
      this._providedNavbarViewController = navbarScreenViewController;
      this._providedDialogViewController = dialogScreenViewController;
      this._viewControllerWasProvided = true;
    }

    private void Activate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
      this._screenSystem.backButtonWasPressedEvent += new Action(this.HandleScreenSystemBackButtonWasPressed);
      this._isActivated = true;
      this.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
    }

    private void Deactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (this._isBeingDestroyed)
        return;
      this._screenSystem.backButtonWasPressedEvent -= new Action(this.HandleScreenSystemBackButtonWasPressed);
      this._isActivated = false;
      this.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
    }

    private void HandleScreenSystemBackButtonWasPressed() => this.BackButtonWasPressed(this.topViewController);

    protected virtual void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
    }

    protected virtual void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
    }

    protected virtual void TopViewControllerWillChange(
      BeatmapEditorViewController oldViewController,
      BeatmapEditorViewController newViewController)
    {
    }

    protected virtual void InitialViewControllerWasPresented()
    {
    }

    protected virtual void BackButtonWasPressed(BeatmapEditorViewController topViewController)
    {
    }
  }
}
