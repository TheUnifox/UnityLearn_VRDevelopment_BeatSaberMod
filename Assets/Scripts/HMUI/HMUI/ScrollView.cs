// Decompiled with JetBrains decompiler
// Type: HMUI.ScrollView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace HMUI
{
  [RequireComponent(typeof (EventSystemListener))]
  public class ScrollView : MonoBehaviour
  {
    [SerializeField]
    protected RectTransform _viewport;
    [SerializeField]
    protected RectTransform _contentRectTransform;
    [Space]
    [SerializeField]
    protected ScrollView.ScrollViewDirection _scrollViewDirection;
    [Space]
    [SerializeField]
    [NullAllowed]
    protected Button _pageUpButton;
    [SerializeField]
    [NullAllowed]
    protected Button _pageDownButton;
    [SerializeField]
    [NullAllowed]
    protected VerticalScrollIndicator _verticalScrollIndicator;
    [Space]
    [SerializeField]
    protected float _smooth = 8f;
    [SerializeField]
    protected float _joystickScrollSpeed = 60f;
    [SerializeField]
    protected float _joystickQuickSnapMaxTime = 0.7f;
    [Space]
    public ScrollView.ScrollType _scrollType;
    [Space]
    [DrawIf("_scrollType", ScrollView.ScrollType.FixedCellSize, DrawIfAttribute.DisablingType.DontDraw)]
    public float _fixedCellSize = 10f;
    [DrawIf("_scrollType", ScrollView.ScrollType.FocusItems, DrawIfAttribute.DisablingType.DontDraw)]
    public float _scrollItemRelativeThresholdPosition = 0.4f;
    [DrawIf("_scrollType", ScrollView.ScrollType.FocusItems, ScrollView.ScrollType.PageSize, DrawIfAttribute.DisablingType.DontDraw)]
    public float _pageStepNormalizedSize = 0.7f;
    [Inject]
    protected readonly IVRPlatformHelper _platformHelper;
    protected ButtonBinder _buttonBinder;
    protected float _destinationPos;
    protected float[] _scrollFocusPositions;
    protected EventSystemListener _eventSystemListener;
    protected ScrollView.ScrollDirection _lastJoystickScrollDirection;
    protected float _joystickScrollStartTime;

    public event Action<float> scrollPositionChangedEvent;

    public RectTransform viewportTransform => this._viewport;

    public RectTransform contentTransform => this._contentRectTransform;

    public float position => this._scrollViewDirection != ScrollView.ScrollViewDirection.Vertical ? this._contentRectTransform.anchoredPosition.x : this._contentRectTransform.anchoredPosition.y;

    public float scrollableSize => Mathf.Max(this.contentSize - this.scrollPageSize, 0.0f);

    private float scrollPageSize => this._scrollViewDirection != ScrollView.ScrollViewDirection.Vertical ? this._viewport.rect.width : this._viewport.rect.height;

    private float contentSize => this._scrollViewDirection != ScrollView.ScrollViewDirection.Vertical ? this._contentRectTransform.rect.width : this._contentRectTransform.rect.height;

    public virtual void Awake()
    {
      this._eventSystemListener = this.GetComponent<EventSystemListener>();
      this._eventSystemListener.pointerDidEnterEvent += new Action<PointerEventData>(this.HandlePointerDidEnter);
      this._eventSystemListener.pointerDidExitEvent += new Action<PointerEventData>(this.HandlePointerDidExit);
      this._buttonBinder = new ButtonBinder();
      if ((UnityEngine.Object) this._pageUpButton != (UnityEngine.Object) null)
        this._buttonBinder.AddBinding(this._pageUpButton, new Action(this.PageUpButtonPressed));
      if ((UnityEngine.Object) this._pageDownButton != (UnityEngine.Object) null)
        this._buttonBinder.AddBinding(this._pageDownButton, new Action(this.PageDownButtonPressed));
      this.UpdateContentSize();
      if (this._scrollType == ScrollView.ScrollType.FocusItems)
      {
        ItemForFocussedScrolling[] componentsInChildren = this.GetComponentsInChildren<ItemForFocussedScrolling>(true);
        switch (this._scrollViewDirection)
        {
          case ScrollView.ScrollViewDirection.Vertical:
            this._scrollFocusPositions = ((IEnumerable<ItemForFocussedScrolling>) componentsInChildren).Select<ItemForFocussedScrolling, float>((Func<ItemForFocussedScrolling, float>) (item => this.WorldPositionToScrollViewPosition(item.transform.position).y)).OrderBy<float, float>((Func<float, float>) (i => i)).ToArray<float>();
            break;
          case ScrollView.ScrollViewDirection.Horizontal:
            this._scrollFocusPositions = ((IEnumerable<ItemForFocussedScrolling>) componentsInChildren).Select<ItemForFocussedScrolling, float>((Func<ItemForFocussedScrolling, float>) (item => this.WorldPositionToScrollViewPosition(item.transform.position).x)).OrderBy<float, float>((Func<float, float>) (i => i)).ToArray<float>();
            break;
        }
      }
      this.RefreshButtons();
      this.enabled = false;
    }

    public virtual void OnDestroy()
    {
      this._buttonBinder?.ClearBindings();
      if (this._platformHelper != null)
        this._platformHelper.joystickWasNotCenteredThisFrameEvent -= new Action<Vector2>(this.HandleJoystickWasNotCenteredThisFrame);
      if (!((UnityEngine.Object) this._eventSystemListener != (UnityEngine.Object) null))
        return;
      this._eventSystemListener.pointerDidEnterEvent -= new Action<PointerEventData>(this.HandlePointerDidEnter);
      this._eventSystemListener.pointerDidExitEvent -= new Action<PointerEventData>(this.HandlePointerDidExit);
    }

    public virtual void Update()
    {
      Vector2 anchoredPosition = this._contentRectTransform.anchoredPosition;
      double a = this._scrollViewDirection == ScrollView.ScrollViewDirection.Vertical ? (double) anchoredPosition.y : (double) anchoredPosition.x;
      float b = this._scrollViewDirection == ScrollView.ScrollViewDirection.Vertical ? this._destinationPos : -this._destinationPos;
      float num = Mathf.Lerp((float) a, b, Time.deltaTime * this._smooth);
      if ((double) Mathf.Abs((float) a - this._destinationPos) < 0.0099999997764825821)
      {
        num = b;
        this.enabled = false;
      }
      this._contentRectTransform.anchoredPosition = this._scrollViewDirection == ScrollView.ScrollViewDirection.Vertical ? new Vector2(0.0f, num) : new Vector2(num, 0.0f);
      Action<float> positionChangedEvent = this.scrollPositionChangedEvent;
      if (positionChangedEvent != null)
        positionChangedEvent(num);
      this.UpdateVerticalScrollIndicator(Mathf.Abs(num));
    }

    public virtual void SetContentSize(float contentSize)
    {
      bool flag = (double) contentSize > (double) this.scrollPageSize + 0.0099999997764825821;
      if (this._scrollViewDirection == ScrollView.ScrollViewDirection.Vertical)
        this._contentRectTransform.sizeDelta = new Vector2(this._contentRectTransform.sizeDelta.x, contentSize);
      else if (this._scrollViewDirection == ScrollView.ScrollViewDirection.Horizontal)
        this._contentRectTransform.sizeDelta = new Vector2(contentSize, this._contentRectTransform.sizeDelta.y);
      if ((UnityEngine.Object) this._verticalScrollIndicator != (UnityEngine.Object) null)
      {
        this._verticalScrollIndicator.gameObject.SetActive(flag);
        Rect rect = this._viewport.rect;
        this._verticalScrollIndicator.normalizedPageHeight = this._scrollViewDirection == ScrollView.ScrollViewDirection.Vertical ? rect.height / contentSize : rect.width / contentSize;
      }
      if ((UnityEngine.Object) this._pageUpButton != (UnityEngine.Object) null)
        this._pageUpButton.gameObject.SetActive(flag);
      if (!((UnityEngine.Object) this._pageDownButton != (UnityEngine.Object) null))
        return;
      this._pageDownButton.gameObject.SetActive(flag);
    }

    public virtual void UpdateContentSize()
    {
      switch (this._scrollViewDirection)
      {
        case ScrollView.ScrollViewDirection.Vertical:
          this.SetContentSize(this._contentRectTransform.rect.height);
          break;
        case ScrollView.ScrollViewDirection.Horizontal:
          this.SetContentSize(this._contentRectTransform.rect.width);
          break;
      }
      this.ScrollTo(0.0f, false);
    }

    public virtual void ScrollToEnd(bool animated) => this.ScrollTo(this.contentSize - this.scrollPageSize, animated);

    public virtual void ScrollToWorldPosition(
      Vector3 worldPosition,
      float pageRelativePosition,
      bool animated)
    {
      this.ScrollTo(this.WorldPositionToScrollViewPosition(worldPosition).y - pageRelativePosition * this.scrollPageSize, animated);
    }

    public virtual void ScrollToWorldPositionIfOutsideArea(
      Vector3 worldPosition,
      float pageRelativePosition,
      float relativeBoundaryStart,
      float relativeBoundaryEnd,
      bool animated)
    {
      float y = this.WorldPositionToScrollViewPosition(worldPosition).y;
      float num1 = this._destinationPos + relativeBoundaryStart * this.scrollPageSize;
      float num2 = this._destinationPos + relativeBoundaryEnd * this.scrollPageSize;
      if ((double) y > (double) num1 && (double) y < (double) num2)
        return;
      this.ScrollTo(y - pageRelativePosition * this.scrollPageSize, animated);
    }

    public virtual void ScrollTo(float destinationPos, bool animated)
    {
      this.SetDestinationPos(destinationPos);
      if (!animated)
      {
        switch (this._scrollViewDirection)
        {
          case ScrollView.ScrollViewDirection.Vertical:
            this._contentRectTransform.anchoredPosition = new Vector2(0.0f, this._destinationPos);
            Action<float> positionChangedEvent1 = this.scrollPositionChangedEvent;
            if (positionChangedEvent1 != null)
            {
              positionChangedEvent1(this._destinationPos);
              break;
            }
            break;
          case ScrollView.ScrollViewDirection.Horizontal:
            this._contentRectTransform.anchoredPosition = new Vector2(-this._destinationPos, 0.0f);
            Action<float> positionChangedEvent2 = this.scrollPositionChangedEvent;
            if (positionChangedEvent2 != null)
            {
              positionChangedEvent2(-this._destinationPos);
              break;
            }
            break;
        }
      }
      this.RefreshButtons();
      this.enabled = true;
    }

    public virtual Vector2 WorldPositionToScrollViewPosition(Vector3 worldPosition) => (Vector2) this._viewport.transform.InverseTransformPoint(this._contentRectTransform.position) - (Vector2) this._viewport.transform.InverseTransformPoint(worldPosition);

    public virtual void SetDestinationPos(float value)
    {
      float max = this.contentSize - this.scrollPageSize;
      if ((double) max < 0.0)
        this._destinationPos = 0.0f;
      else
        this._destinationPos = Mathf.Clamp(value, 0.0f, max);
    }

    public virtual void UpdateVerticalScrollIndicator(float posY)
    {
      if (!((UnityEngine.Object) this._verticalScrollIndicator != (UnityEngine.Object) null))
        return;
      this._verticalScrollIndicator.progress = posY / (this.contentSize - this.scrollPageSize);
    }

    public virtual void PageUpButtonPressed()
    {
      float num = this._destinationPos;
      switch (this._scrollType)
      {
        case ScrollView.ScrollType.PageSize:
          num -= this._pageStepNormalizedSize * this.scrollPageSize;
          break;
        case ScrollView.ScrollType.FixedCellSize:
          num = (float) Mathf.FloorToInt((num - this._fixedCellSize * (float) (Mathf.RoundToInt(this.scrollPageSize / this._fixedCellSize) - 1)) / this._fixedCellSize) * this._fixedCellSize;
          break;
        case ScrollView.ScrollType.FocusItems:
          float threshold = this._destinationPos + this._scrollItemRelativeThresholdPosition * this.scrollPageSize;
          num = ((IEnumerable<float>) this._scrollFocusPositions).Where<float>((Func<float, bool>) (pos => (double) pos < (double) threshold)).DefaultIfEmpty<float>(this._destinationPos).Max() - this._pageStepNormalizedSize * this.scrollPageSize;
          break;
      }
      this.SetDestinationPos(num);
      this.RefreshButtons();
      this.enabled = true;
    }

    public virtual void PageDownButtonPressed()
    {
      float num = this._destinationPos;
      switch (this._scrollType)
      {
        case ScrollView.ScrollType.PageSize:
          num += this._pageStepNormalizedSize * this.scrollPageSize;
          break;
        case ScrollView.ScrollType.FixedCellSize:
          num = (float) Mathf.CeilToInt((num + this._fixedCellSize * (float) (Mathf.RoundToInt(this.scrollPageSize / this._fixedCellSize) - 1)) / this._fixedCellSize) * this._fixedCellSize;
          break;
        case ScrollView.ScrollType.FocusItems:
          float threshold = this._destinationPos + (1f - this._scrollItemRelativeThresholdPosition) * this.scrollPageSize;
          num = ((IEnumerable<float>) this._scrollFocusPositions).Where<float>((Func<float, bool>) (pos => (double) pos > (double) threshold)).DefaultIfEmpty<float>(this._destinationPos + this.scrollPageSize).Min() - (1f - this._pageStepNormalizedSize) * this.scrollPageSize;
          break;
      }
      this.SetDestinationPos(num);
      this.RefreshButtons();
      this.enabled = true;
    }

    public virtual void RefreshButtons()
    {
      if ((UnityEngine.Object) this._pageUpButton != (UnityEngine.Object) null)
        this._pageUpButton.interactable = (double) this._destinationPos > 1.0 / 1000.0;
      if (!((UnityEngine.Object) this._pageDownButton != (UnityEngine.Object) null))
        return;
      this._pageDownButton.interactable = (double) this._destinationPos < (double) this.contentSize - (double) this.scrollPageSize - 1.0 / 1000.0;
    }

    public virtual void HandlePointerDidEnter(PointerEventData eventData)
    {
      if (this._platformHelper == null)
        return;
      this._platformHelper.joystickWasNotCenteredThisFrameEvent += new Action<Vector2>(this.HandleJoystickWasNotCenteredThisFrame);
      this._platformHelper.joystickWasCenteredThisFrameEvent += new Action(this.HandleJoystickWasCenteredThisFrame);
    }

    public virtual void HandlePointerDidExit(PointerEventData eventData)
    {
      if (this._platformHelper == null)
        return;
      this._platformHelper.joystickWasNotCenteredThisFrameEvent -= new Action<Vector2>(this.HandleJoystickWasNotCenteredThisFrame);
      this._platformHelper.joystickWasCenteredThisFrameEvent -= new Action(this.HandleJoystickWasCenteredThisFrame);
    }

    public virtual void HandleJoystickWasNotCenteredThisFrame(Vector2 deltaPos)
    {
      if (this._lastJoystickScrollDirection == ScrollView.ScrollDirection.None)
        this._joystickScrollStartTime = Time.time;
      this._lastJoystickScrollDirection = this.ResolveScrollDirection(deltaPos);
      float destinationPos = this._destinationPos;
      switch (this._scrollViewDirection)
      {
        case ScrollView.ScrollViewDirection.Vertical:
          destinationPos -= deltaPos.y * Time.deltaTime * this._joystickScrollSpeed;
          break;
        case ScrollView.ScrollViewDirection.Horizontal:
          destinationPos += deltaPos.x * Time.deltaTime * this._joystickScrollSpeed;
          break;
      }
      this.SetDestinationPos(destinationPos);
      this.RefreshButtons();
      this.enabled = true;
    }

    public virtual void HandleJoystickWasCenteredThisFrame()
    {
      float num1 = this._destinationPos;
      float num2 = Time.time - this._joystickScrollStartTime;
      switch (this._scrollType)
      {
        case ScrollView.ScrollType.PageSize:
          float num3 = 0.0f;
          float num4 = 1f;
          if ((double) num2 < (double) this._joystickQuickSnapMaxTime)
          {
            if (this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Up || this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Left)
              num3 = -1f;
            else if (this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Down || this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Right)
              num3 = 1f;
            num4 = this.scrollPageSize * this._pageStepNormalizedSize;
          }
          num1 = (float) Mathf.RoundToInt(num1 / num4 + num3) * num4;
          break;
        case ScrollView.ScrollType.FixedCellSize:
          float num5 = this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Up || this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Left ? -1f : 0.0f;
          num1 = (float) Mathf.CeilToInt(num1 / this._fixedCellSize + num5) * this._fixedCellSize;
          break;
        case ScrollView.ScrollType.FocusItems:
          if (this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Up || this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Right)
          {
            float threshold = this._destinationPos + this._scrollItemRelativeThresholdPosition * this.scrollPageSize;
            num1 = ((IEnumerable<float>) this._scrollFocusPositions).Where<float>((Func<float, bool>) (pos => (double) pos < (double) threshold)).DefaultIfEmpty<float>(this._destinationPos).Max();
            if ((double) num2 < (double) this._joystickQuickSnapMaxTime)
            {
              num1 -= this._pageStepNormalizedSize * this.scrollPageSize;
              break;
            }
            break;
          }
          if (this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Down || this._lastJoystickScrollDirection == ScrollView.ScrollDirection.Left)
          {
            float threshold = this._destinationPos + (1f - this._scrollItemRelativeThresholdPosition) * this.scrollPageSize;
            num1 = ((IEnumerable<float>) this._scrollFocusPositions).Where<float>((Func<float, bool>) (pos => (double) pos > (double) threshold)).DefaultIfEmpty<float>(this._destinationPos + this.scrollPageSize).Min();
            if ((double) num2 < (double) this._joystickQuickSnapMaxTime)
            {
              num1 -= (1f - this._pageStepNormalizedSize) * this.scrollPageSize;
              break;
            }
            break;
          }
          break;
      }
      this._lastJoystickScrollDirection = ScrollView.ScrollDirection.None;
      this.SetDestinationPos(num1);
      this.RefreshButtons();
      this.enabled = true;
    }

    public virtual ScrollView.ScrollDirection ResolveScrollDirection(Vector2 deltaPos)
    {
      switch (this._scrollViewDirection)
      {
        case ScrollView.ScrollViewDirection.Vertical:
          if ((double) deltaPos.y > 0.0)
            return ScrollView.ScrollDirection.Up;
          if ((double) deltaPos.y < 0.0)
            return ScrollView.ScrollDirection.Down;
          break;
        case ScrollView.ScrollViewDirection.Horizontal:
          if ((double) deltaPos.x > 0.0)
            return ScrollView.ScrollDirection.Right;
          if ((double) deltaPos.x < 0.0)
            return ScrollView.ScrollDirection.Left;
          break;
      }
      return ScrollView.ScrollDirection.None;
    }

    [CompilerGenerated]
    public virtual float Awake_b__38_0(ItemForFocussedScrolling item) => this.WorldPositionToScrollViewPosition(item.transform.position).y;

    [CompilerGenerated]
    public virtual float Awake_b__38_2(ItemForFocussedScrolling item) => this.WorldPositionToScrollViewPosition(item.transform.position).x;

    public enum ScrollType
    {
      PageSize,
      FixedCellSize,
      FocusItems,
    }

    public enum ScrollDirection
    {
      None,
      Up,
      Down,
      Left,
      Right,
    }

    public enum ScrollViewDirection
    {
      Vertical,
      Horizontal,
    }
  }
}
