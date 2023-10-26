// Decompiled with JetBrains decompiler
// Type: HMUI.TextSlider
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class TextSlider : 
    Selectable,
    IBeginDragHandler,
    IEventSystemHandler,
    IDragHandler,
    IInitializePotentialDragHandler,
    ICanvasElement
  {
    [SerializeField]
    protected TextMeshProUGUI _valueText;
    [SerializeField]
    protected RectTransform _handleRect;
    [Space]
    [SerializeField]
    protected bool _enableDragging = true;
    [SerializeField]
    protected float _handleSize = 2f;
    [SerializeField]
    protected float _valueSize = 10f;
    [SerializeField]
    protected float _separatorSize = 1.5f;
    [SerializeField]
    protected int _numberOfSteps;
    [Space]
    [Range(0.0f, 1f)]
    [SerializeField]
    protected float _normalizedValue;
    protected RectTransform _containerRect;
    protected Graphic _handleGraphic;
    protected DrivenRectTransformTracker _tracker;

    public Color valueTextColor
    {
      set => this._valueText.color = value;
    }

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

    public float handleSize
    {
      get => this._handleSize;
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this._handleSize, value))
          return;
        this.UpdateVisuals();
      }
    }

    public float valueSize
    {
      get => this._valueSize;
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this._valueSize, value))
          return;
        this.UpdateVisuals();
      }
    }

    public float separatorSize
    {
      get => this._separatorSize;
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this._separatorSize, value))
          return;
        this.UpdateVisuals();
      }
    }

    public int numberOfSteps
    {
      get => this._numberOfSteps;
      set
      {
        if (!SetPropertyUtility.SetStruct<int>(ref this._numberOfSteps, value))
          return;
        this.SetNormalizedValue(this._normalizedValue);
        this.UpdateVisuals();
      }
    }

    public float normalizedValue
    {
      get
      {
        float normalizedValue = this._normalizedValue;
        if (this._numberOfSteps > 1)
          normalizedValue = Mathf.Round(normalizedValue * (float) (this._numberOfSteps - 1)) / (float) (this._numberOfSteps - 1);
        return normalizedValue;
      }
      set => this.SetNormalizedValue(value, false);
    }

    public event Action<TextSlider, float> normalizedValueDidChangeEvent;

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

    public virtual void SetNormalizedValue(float input) => this.SetNormalizedValue(input, true);

    public virtual void SetNormalizedValue(float input, bool sendCallback)
    {
      double normalizedValue1 = (double) this._normalizedValue;
      this._normalizedValue = Mathf.Clamp01(input);
      double normalizedValue2 = (double) this.normalizedValue;
      if (normalizedValue1 == normalizedValue2)
        return;
      this.UpdateVisuals();
      if (!sendCallback)
        return;
      Action<TextSlider, float> valueDidChangeEvent = this.normalizedValueDidChangeEvent;
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
      if (!((UnityEngine.Object) this._containerRect != (UnityEngine.Object) null) || !((UnityEngine.Object) this._valueText != (UnityEngine.Object) null))
        return;
      float width = this._containerRect.rect.width;
      Vector2 vector2_1 = new Vector2(0.0f, 0.0f);
      Vector2 vector2_2 = new Vector2(0.0f, 1f);
      float x = this.normalizedValue * (width - this.handleSize);
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.AnchorMax);
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.AnchorMin);
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.SizeDelta);
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.Pivot);
      this._tracker.Add((UnityEngine.Object) this, this._handleRect, DrivenTransformProperties.AnchoredPosition);
      this._handleRect.anchorMin = vector2_1;
      this._handleRect.anchorMax = vector2_2;
      this._handleRect.sizeDelta = new Vector2(this.handleSize, 0.0f);
      this._handleRect.pivot = new Vector2(0.0f, 0.5f);
      this._handleRect.anchoredPosition = new Vector2(x, 0.0f);
      RectTransform transform = (RectTransform) this._valueText.transform;
      this._tracker.Add((UnityEngine.Object) this, transform, DrivenTransformProperties.AnchorMax);
      this._tracker.Add((UnityEngine.Object) this, transform, DrivenTransformProperties.AnchorMin);
      this._tracker.Add((UnityEngine.Object) this, transform, DrivenTransformProperties.SizeDelta);
      this._tracker.Add((UnityEngine.Object) this, transform, DrivenTransformProperties.Pivot);
      this._tracker.Add((UnityEngine.Object) this, transform, DrivenTransformProperties.AnchoredPosition);
      transform.anchorMin = vector2_1;
      transform.anchorMax = vector2_2;
      transform.sizeDelta = new Vector2(this.valueSize, 0.0f);
      if ((double) x + (double) this.separatorSize + (double) this.valueSize >= (double) this._containerRect.rect.width)
      {
        transform.pivot = new Vector2(1f, 0.5f);
        transform.anchoredPosition = new Vector2(x - this.separatorSize, 0.0f);
        this._valueText.alignment = TextAlignmentOptions.CaplineRight;
      }
      else
      {
        transform.pivot = new Vector2(0.0f, 0.5f);
        transform.anchoredPosition = new Vector2(x + this.handleSize + this.separatorSize, 0.0f);
        this._valueText.alignment = TextAlignmentOptions.CaplineLeft;
      }
      this._valueText.text = this.TextForNormalizedValue(this.normalizedValue);
    }

    public virtual void UpdateDrag(PointerEventData eventData)
    {
      Vector2 localPoint;
      if (eventData.button != PointerEventData.InputButton.Left || (UnityEngine.Object) this._containerRect == (UnityEngine.Object) null || eventData.hovered.Count == 0 || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this._containerRect, eventData.position, eventData.pressEventCamera, out localPoint) || float.IsNaN(localPoint.x) || float.IsNaN(localPoint.y))
        return;
      Vector2 vector2_1 = localPoint - this._containerRect.rect.position;
      Rect rect = this._handleRect.rect;
      Vector2 vector2_2 = new Vector2(rect.width * 0.5f, 0.0f);
      Vector2 vector2_3 = vector2_1 - vector2_2;
      rect = this._handleRect.rect;
      Vector2 vector2_4 = (rect.size - this._handleRect.sizeDelta) * 0.5f;
      Vector2 vector2_5 = vector2_3 - vector2_4;
      rect = this._containerRect.rect;
      double width1 = (double) rect.width;
      double handleSize = (double) this.handleSize;
      rect = this._containerRect.rect;
      double width2 = (double) rect.width;
      double num1 = 1.0 - handleSize / width2;
      float num2 = (float) (width1 * num1);
      if ((double) num2 <= 0.0)
        return;
      this.SetNormalizedValue(vector2_5.x / num2);
    }

    public virtual bool MayDrag(PointerEventData eventData) => this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData) || !this._enableDragging)
        return;
      int num = (UnityEngine.Object) this._containerRect == (UnityEngine.Object) null ? 1 : 0;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData) || !this._enableDragging || !((UnityEngine.Object) this._containerRect != (UnityEngine.Object) null))
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

    public virtual void OnInitializePotentialDrag(PointerEventData eventData) => eventData.useDragThreshold = false;

    protected virtual string TextForNormalizedValue(float normalizedValue) => normalizedValue.ToString();
  }
}
