// Decompiled with JetBrains decompiler
// Type: HMUI.ModalView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace HMUI
{
  public class ModalView : MonoBehaviour
  {
    [SerializeField]
    protected PanelAnimationSO _presentPanelAnimations;
    [SerializeField]
    protected PanelAnimationSO _dismissPanelAnimation;
    [SerializeField]
    protected bool _animateParentCanvas = true;
    [Inject]
    protected readonly DiContainer _container;
    protected Transform _previousParent;
    protected bool _isShown;
    protected bool _viewIsValid;
    protected Canvas _mainCanvas;
    protected CanvasGroup _parentCanvasGroup;
    protected GameObject _blockerGO;
    protected int _test;

    public event Action blockerClickedEvent;

    public virtual void OnDisable() => this.Hide(false);

    public virtual void OnDestroy()
    {
      if (!(bool) (UnityEngine.Object) this._blockerGO)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this._blockerGO);
    }

    public virtual void SetupView(Transform screenTransform)
    {
      if (this._viewIsValid)
        return;
      this.gameObject.SetActive(true);
      this._mainCanvas = EssentialHelpers.GetOrAddComponent<Canvas>(this.gameObject);
      if ((UnityEngine.Object) screenTransform != (UnityEngine.Object) null)
      {
        this._parentCanvasGroup = EssentialHelpers.GetOrAddComponent<CanvasGroup>(screenTransform.gameObject);
        foreach (object component in screenTransform.GetComponents<BaseRaycaster>())
        {
          System.Type type = component.GetType();
          if ((UnityEngine.Object) this.gameObject.GetComponent(type) == (UnityEngine.Object) null)
            this._container.InstantiateComponent(type, this.gameObject);
        }
      }
      else
        EssentialHelpers.GetOrAddComponent<GraphicRaycaster>(this.gameObject);
      EssentialHelpers.GetOrAddComponent<CanvasGroup>(this.gameObject).ignoreParentGroups = true;
      this.gameObject.transform.SetParent(screenTransform, true);
      this.gameObject.SetActive(false);
      this._viewIsValid = true;
    }

    public virtual void Hide(bool animated, Action finishedCallback = null)
    {
      if (!this._isShown)
        return;
      ViewController componentInParent = this.transform.GetComponentInParent<ViewController>();
      if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
        componentInParent.didDeactivateEvent -= new ViewController.DidDeactivateDelegate(this.HandleParentViewControllerDidDeactivate);
      UnityEngine.Object.Destroy((UnityEngine.Object) this._blockerGO);
      this._isShown = false;
      this._dismissPanelAnimation.ExecuteAnimation(this.gameObject, this._animateParentCanvas ? this._parentCanvasGroup : (CanvasGroup) null, !animated, (Action) (() =>
      {
        this.transform.SetParent(this._previousParent, true);
        this.gameObject.SetActive(false);
        Action action = finishedCallback;
        if (action == null)
          return;
        action();
      }));
    }

    public virtual void Show(bool animated, bool moveToCenter = false, Action finishedCallback = null)
    {
      if (this._isShown)
        return;
      Canvas canvas;
      ViewController viewController;
      Transform modalRootTransform = ModalView.GetModalRootTransform(this.transform.parent, out canvas, out viewController);
      if ((UnityEngine.Object) viewController != (UnityEngine.Object) null)
        viewController.didDeactivateEvent += new ViewController.DidDeactivateDelegate(this.HandleParentViewControllerDidDeactivate);
      this._previousParent = this.transform.parent;
      if (!this._viewIsValid)
        this.SetupView(modalRootTransform);
      this.gameObject.SetActive(true);
      this.gameObject.GetComponent<Canvas>().sortingLayerID = canvas.sortingLayerID;
      if (moveToCenter)
      {
        this.transform.SetParent(modalRootTransform, false);
        RectTransform transform = (RectTransform) this.transform;
        transform.pivot = new Vector2(0.5f, 0.5f);
        Vector2 center = ((RectTransform) modalRootTransform).rect.center;
        transform.localPosition = new Vector3(center.x, center.y, transform.localPosition.z);
      }
      else
      {
        this.transform.SetParent(modalRootTransform, true);
        RectTransform rectTransform = (RectTransform) modalRootTransform;
        RectTransform transform = (RectTransform) this.transform;
        Vector3 localPosition = transform.localPosition;
        float num = (float) ((double) localPosition.y - (double) transform.rect.height * 0.5 + (double) rectTransform.rect.height * 0.5);
        if ((double) num < 0.0)
        {
          localPosition.y -= num;
          transform.localPosition = localPosition;
        }
      }
      this._blockerGO = this.CreateBlocker();
      this._isShown = true;
      this._presentPanelAnimations.ExecuteAnimation(this.gameObject, this._animateParentCanvas ? this._parentCanvasGroup : (CanvasGroup) null, !animated, finishedCallback);
    }

    public virtual GameObject CreateBlocker()
    {
      GameObject blocker = new GameObject("Blocker");
      RectTransform rectTransform = blocker.AddComponent<RectTransform>();
      Canvas canvas = (Canvas) null;
      for (Transform parent = this._mainCanvas.transform.parent; (UnityEngine.Object) parent != (UnityEngine.Object) null; parent = parent.parent)
      {
        canvas = parent.GetComponent<Canvas>();
        if ((UnityEngine.Object) canvas != (UnityEngine.Object) null)
          break;
      }
      rectTransform.SetParent(this._mainCanvas.transform.parent, false);
      rectTransform.SetSiblingIndex(this._mainCanvas.transform.GetSiblingIndex());
      rectTransform.anchorMin = (Vector2) Vector3.zero;
      rectTransform.anchorMax = (Vector2) Vector3.one;
      rectTransform.sizeDelta = Vector2.zero;
      if ((UnityEngine.Object) canvas != (UnityEngine.Object) null)
      {
        foreach (object component in canvas.GetComponents<BaseRaycaster>())
        {
          System.Type type = component.GetType();
          if ((UnityEngine.Object) blocker.GetComponent(type) == (UnityEngine.Object) null)
            this._container.InstantiateComponent(type, blocker);
        }
      }
      else
        EssentialHelpers.GetOrAddComponent<GraphicRaycaster>(blocker);
      blocker.AddComponent<Touchable>();
      blocker.AddComponent<Button>().onClick.AddListener(new UnityAction(this.HandleBlockerButtonClicked));
      return blocker;
    }

    public virtual void HandleBlockerButtonClicked()
    {
      Action blockerClickedEvent = this.blockerClickedEvent;
      if (blockerClickedEvent == null)
        return;
      blockerClickedEvent();
    }

    public virtual void HandleParentViewControllerDidDeactivate(
      bool removedFromHierarchy,
      bool screenSystemDisabling)
    {
      bool prevAnimateParentCanvas = this._animateParentCanvas;
      this._animateParentCanvas = screenSystemDisabling;
      this.Hide(!screenSystemDisabling, (Action) (() => this._animateParentCanvas = prevAnimateParentCanvas));
    }

    private static Transform GetModalRootTransform(
      Transform transform,
      out Canvas canvas,
      out ViewController viewController)
    {
      Screen componentInParent = transform.GetComponentInParent<Screen>();
      canvas = componentInParent.GetComponentInChildren<Canvas>();
      viewController = componentInParent.GetComponentInChildren<ViewController>();
      return (UnityEngine.Object) viewController != (UnityEngine.Object) null ? viewController.transform : canvas.transform;
    }
  }
}
