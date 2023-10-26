// Decompiled with JetBrains decompiler
// Type: HMUI.Slider2D
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class Slider2D : 
    Selectable,
    IBeginDragHandler,
    IEventSystemHandler,
    IDragHandler,
    IInitializePotentialDragHandler,
    ICanvasElement
  {
    [SerializeField]
    protected RectTransform _handleRect;
    [Space]
    [SerializeField]
    protected Vector2 _normalizedValue;
    protected RectTransform _containerRect;
    protected Graphic _handleGraphic;
    protected DrivenRectTransformTracker _tracker;

    public RectTransform handleRect
    {
      get => this._handleRect;
      set
      {
        if (!SetPropertyUtility.SetClass<RectTransform>(ref this._handleRect, value))
          return;
        this.UpdateCachedReferences();
        this.UpdateVisuals();
      }
    }

    public Color handleColor
    {
      set
      {
        if (!((UnityEngine.Object) this._handleGraphic != (UnityEngine.Object) null))
          return;
        this._handleGraphic.color = value;
      }
    }

    public Vector2 normalizedValue
    {
      get => this._normalizedValue;
      set => this.SetNormalizedValue(value, false);
    }

    public event Action<Slider2D, Vector2> normalizedValueDidChangeEvent;

    public virtual void Rebuild(CanvasUpdate executing)
    {
    }

    public virtual void LayoutComplete()
    {
    }

    public virtual void GraphicUpdateComplete()
    {
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.UpdateCachedReferences();
      this.SetNormalizedValue(this._normalizedValue, false);
      this.UpdateVisuals();
    }

    protected override void OnDisable()
    {
      this._tracker.Clear();
      base.OnDisable();
    }

    public virtual void UpdateCachedReferences()
    {
      this._containerRect = !(bool) (UnityEngine.Object) this._handleRect || !((UnityEngine.Object) this._handleRect.parent != (UnityEngine.Object) null) ? (RectTransform) null : this._handleRect.parent.GetComponent<RectTransform>();
      if (!(bool) (UnityEngine.Object) this._handleRect)
        return;
      this._handleGraphic = this._handleRect.gameObject.GetComponent<Graphic>();
    }

    public virtual void SetNormalizedValue(Vector2 input) => this.SetNormalizedValue(input, true);

    public virtual void SetNormalizedValue(Vector2 input, bool sendCallback)
    {
      Vector2 normalizedValue1 = this._normalizedValue;
      this._normalizedValue.x = Mathf.Clamp01(input.x);
      this._normalizedValue.y = Mathf.Clamp01(input.y);
      Vector2 normalizedValue2 = this.normalizedValue;
      if (normalizedValue1 == normalizedValue2)
        return;
      this.UpdateVisuals();
      if (!sendCallback)
        return;
      Action<Slider2D, Vector2> valueDidChangeEvent = this.normalizedValueDidChangeEvent;
      if (valueDidChangeEvent == null)
        return;
      valueDidChangeEvent(this, this.normalizedValue);
    }

    protected override void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      if (!this.IsActive())
        return;
      this.UpdateVisuals();
    }

    protected virtual void UpdateVisuals()
    {
      this._tracker.Clear();
      if (!((UnityEngine.Object) this._containerRect != (UnityEngine.Object) null))
        return;
      float width = this._containerRect.rect.width;
      float height = this._containerRect.rect.height;
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.Pivot);
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.AnchoredPosition);
      this._handleRect.pivot = new Vector2(0.5f, 0.5f);
      this._handleRect.anchoredPosition = new Vector2((this._normalizedValue.x - this._containerRect.pivot.x) * width, (this._normalizedValue.y - this._containerRect.pivot.y) * height);
    }

    public virtual void UpdateDrag(PointerEventData eventData)
    {
      Vector2 localPoint;
      if (eventData.button != PointerEventData.InputButton.Left || (UnityEngine.Object) this._containerRect == (UnityEngine.Object) null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this._containerRect, eventData.position, eventData.pressEventCamera, out localPoint) || float.IsNaN(localPoint.x) || float.IsNaN(localPoint.y))
        return;
      float width = this._containerRect.rect.width;
      float height = this._containerRect.rect.height;
      this.SetNormalizedValue(new Vector2(localPoint.x / width + this._containerRect.pivot.x, localPoint.y / height + this._containerRect.pivot.y));
    }

    public virtual bool MayDrag(PointerEventData eventData) => this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      int num = (UnityEngine.Object) this._containerRect == (UnityEngine.Object) null ? 1 : 0;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData) || !((UnityEngine.Object) this._containerRect != (UnityEngine.Object) null))
        return;
      this.UpdateDrag(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      base.OnPointerDown(eventData);
      if (!((UnityEngine.Object) this._containerRect != (UnityEngine.Object) null))
        return;
      this.UpdateDrag(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData) => base.OnPointerEnter(eventData);

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant) => base.DoStateTransition(state, instant);

    public virtual void OnInitializePotentialDrag(PointerEventData eventData) => eventData.useDragThreshold = false;
  }
}
