// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Views;
using HMUI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BeatmapEditor3D
{
  public class BeatmapEditorViewController : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorViewRelationData _viewRelationData;
    private BeatmapEditorScreen _screen;
    private BeatmapEditorViewController _parentViewController;
    private BeatmapEditorViewController _childViewController;
    private RectTransform _rectTransform;
    private BaseRaycaster _graphicRaycaster;
    private bool _wasActivatedBefore;
    private bool _isActivated;
    private bool _isInTransition;

    public bool active
    {
      get => this.gameObject.activeSelf;
      set => this.gameObject.SetActive(value);
    }

    public bool isInViewControllerHierarchy => (Object) this._screen != (Object) null;

    public event BeatmapEditorViewController.DidActivateDelegate didActivateEvent;

    public event BeatmapEditorViewController.DidDeactivateDelegate didDeactivateEvent;

    public RectTransform rectTransform
    {
      get
      {
        if ((Object) this._rectTransform == (Object) null)
          this._rectTransform = (RectTransform) this.transform;
        return this._rectTransform;
      }
    }

    protected ButtonBinder buttonBinder { get; private set; }

    public void AddChildView(BeatmapEditorView view) => this._viewRelationData.AddChildIfNotExists(view);

    public void RemoveChildView(BeatmapEditorView view) => this._viewRelationData.RemoveChild(view);

    public virtual void __Init(
      BeatmapEditorScreen screen,
      BeatmapEditorViewController parentViewController)
    {
      this._screen = screen;
      this._parentViewController = parentViewController;
      if (this.buttonBinder == null)
        this.buttonBinder = new ButtonBinder();
      this.transform.SetParent(screen.transform, false);
    }

    public void __PresentViewController(BeatmapEditorViewController newViewController)
    {
      Assert.IsNull((object) this._childViewController, "Cannot present new view controller. This view controller is already presenting one.");
      Assert.IsNotNull((object) newViewController, "Can not present view controller.");
      Assert.IsFalse(this.IsViewControllerInHierarchy(newViewController), "Cannot present new view controller, because it is already in hierarchy");
      this.__Deactivate(false, false, false);
      if (!newViewController.active)
        newViewController.active = true;
      this._childViewController = newViewController;
      newViewController.__Init(this._screen, this);
      this.gameObject.SetActive(false);
      newViewController.__Activate(true, false);
      newViewController.__ResetRectTransformPosition();
      this.__ResetRectTransformPosition();
      this.DeactivateGameObject();
    }

    public void __ReplaceViewController(BeatmapEditorViewController newViewController)
    {
      Assert.IsFalse(this.IsViewControllerInHierarchy(newViewController), "Can not replace with new view controller, because it is in hierarchy already.");
      Assert.IsNull((object) this._childViewController, "Can not replace view controller, because it is presenting another view controller.");
      Assert.IsNotNull((object) newViewController, "Can not present null view controller.");
      this.__Deactivate(true, false, false);
      if (!newViewController.active)
        newViewController.active = true;
      if ((Object) this._parentViewController != (Object) null)
        this._parentViewController._childViewController = newViewController;
      newViewController.__Init(this._screen, this._parentViewController);
      newViewController.__Activate(true, false);
      newViewController.__ResetRectTransformPosition();
      this.__ResetRectTransformPosition();
      this.DeactivateGameObject();
      this.__ResetViewController();
    }

    public void __DismissViewController()
    {
      Assert.IsNotNull((object) this._parentViewController, "This view controller can not be dismissed, because it does not have any parent.");
      BeatmapEditorViewController parentViewController = this._parentViewController;
      this.__Deactivate(true, false, false);
      parentViewController._childViewController = (BeatmapEditorViewController) null;
      parentViewController.__Activate(false, false);
      parentViewController.__ResetRectTransformPosition();
      this.__ResetRectTransformPosition();
      this.DeactivateGameObject();
      this.__ResetViewController();
    }

    public virtual void __Activate(bool addedToHierarchy, bool screenSystemEnabling)
    {
      Assert.IsFalse(this._isActivated, "Cannot activate already active view controller (" + this.name + ")");
      this._isActivated = true;
      if (!this.active)
        this.active = true;
      bool wasActivatedBefore = this._wasActivatedBefore;
      this._wasActivatedBefore = true;
      this._viewRelationData.WillActivateChildren();
      this.DidActivate(!wasActivatedBefore, addedToHierarchy, screenSystemEnabling);
      this._viewRelationData.ActivateChildren();
      BeatmapEditorViewController.DidActivateDelegate didActivateEvent = this.didActivateEvent;
      if (didActivateEvent == null)
        return;
      didActivateEvent(!wasActivatedBefore, addedToHierarchy, screenSystemEnabling);
    }

    public virtual void __Deactivate(
      bool removedFromHierarchy,
      bool deactivateGameObject,
      bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        Assert.IsTrue(this._isActivated, "Cannot deactivate already non active view controller (" + this.name + ")");
      else
        Assert.IsTrue(this.isInViewControllerHierarchy, "Cannot deactivate view controller (" + this.name + "), because it is not in view controller hierarchy");
      this._isActivated = false;
      if (this.active & deactivateGameObject)
        this.active = false;
      this._viewRelationData.WillDeactivateChildren();
      this.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
      this._viewRelationData.DeactivateChildren();
      BeatmapEditorViewController.DidDeactivateDelegate didDeactivateEvent = this.didDeactivateEvent;
      if (didDeactivateEvent == null)
        return;
      didDeactivateEvent(removedFromHierarchy, screenSystemDisabling);
    }

    public virtual void __ResetViewController()
    {
      if ((Object) this._screen != (Object) null && !this._screen.isBeingDestroyed)
        this.transform.SetParent(this._screen.transform, false);
      this._screen = (BeatmapEditorScreen) null;
      this._parentViewController = (BeatmapEditorViewController) null;
      this._childViewController = (BeatmapEditorViewController) null;
    }

    public virtual void __ResetRectTransformPosition()
    {
      this.rectTransform.anchoredPosition = Vector2.zero;
      this.rectTransform.anchorMin = Vector2.zero;
      this.rectTransform.anchorMax = Vector2.one;
    }

    public virtual void DeactivateGameObject()
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
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

    private bool IsViewControllerInHierarchy(BeatmapEditorViewController viewController)
    {
      for (BeatmapEditorViewController parentViewController = viewController._parentViewController; (Object) parentViewController != (Object) null; parentViewController = parentViewController._parentViewController)
      {
        if (parentViewController == viewController)
          return true;
      }
      for (BeatmapEditorViewController childViewController = viewController._childViewController; (Object) childViewController != (Object) null; childViewController = childViewController._childViewController)
      {
        if (childViewController == viewController)
          return true;
      }
      return false;
    }

    public delegate void DidActivateDelegate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling);

    public delegate void DidDeactivateDelegate(
      bool removedFromHierarchy,
      bool screenSystemDisabling);
  }
}
