// Decompiled with JetBrains decompiler
// Type: HMUI.AnimatedSwitchView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace HMUI
{
  [DisallowMultipleComponent]
  [RequireComponent(typeof (ToggleWithCallbacks))]
  public class AnimatedSwitchView : MonoBehaviour
  {
    [SerializeField]
    protected RectTransform _knobRectTransform;
    [Space]
    [SerializeField]
    protected ImageView _backgroundImage;
    [SerializeField]
    protected ImageView _knobImage;
    [SerializeField]
    protected TextMeshProUGUI _onText;
    [SerializeField]
    protected TextMeshProUGUI _offText;
    [Space]
    [SerializeField]
    protected float _switchAnimationSmooth = 16f;
    [SerializeField]
    protected float _disableAnimationDuration = 0.3f;
    [SerializeField]
    protected float _highlightAnimationDuration = 0.3f;
    [Space]
    [SerializeField]
    protected float _horizontalStretchAmount = 0.8f;
    [SerializeField]
    protected float _verticalStretchAmount = 0.8f;
    [Space]
    [SerializeField]
    protected AnimatedSwitchView.ColorBlock _onColors;
    [SerializeField]
    protected AnimatedSwitchView.ColorBlock _offColors;
    [SerializeField]
    protected AnimatedSwitchView.ColorBlock _onHighlightedColors;
    [SerializeField]
    protected AnimatedSwitchView.ColorBlock _offHighlightedColors;
    [SerializeField]
    protected AnimatedSwitchView.ColorBlock _disabledColors;
    protected AnimatedSwitchView.AnimationState _animationState;
    protected float _switchAmount;
    protected float _highlightAmount;
    protected float _disabledAmount;
    protected float _originalKnobWidth;
    protected float _originalKnobHeight;
    protected ToggleWithCallbacks _toggle;

    public virtual void Awake() => this._toggle = this.GetComponent<ToggleWithCallbacks>();

    public virtual void Start()
    {
      this._switchAmount = this._toggle.isOn ? 1f : 0.0f;
      this._highlightAmount = 0.0f;
      this._disabledAmount = this._toggle.IsInteractable() ? 0.0f : 1f;
      this._animationState = AnimatedSwitchView.AnimationState.Idle;
      this._originalKnobWidth = this._knobRectTransform.sizeDelta.x;
      this._originalKnobHeight = this._knobRectTransform.sizeDelta.y;
      this.LerpColors(this._switchAmount, this._highlightAmount, this._disabledAmount);
      this.LerpPosition(this._switchAmount);
      this._toggle.stateDidChangeEvent += new Action<ToggleWithCallbacks.SelectionState>(this.HandleStateDidChange);
      this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleOnValueChanged));
    }

    public virtual void OnDestroy()
    {
      if (!((UnityEngine.Object) this._toggle != (UnityEngine.Object) null))
        return;
      this._toggle.stateDidChangeEvent -= new Action<ToggleWithCallbacks.SelectionState>(this.HandleStateDidChange);
      this._toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleOnValueChanged));
    }

    public virtual void Update()
    {
      if (this._animationState == AnimatedSwitchView.AnimationState.Idle)
      {
        this.enabled = false;
      }
      else
      {
        if (this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.SwitchingOn))
        {
          this._switchAmount = Mathf.Lerp(this._switchAmount, 1f, Time.deltaTime * this._switchAnimationSmooth);
          if ((double) this._switchAmount >= 0.99000000953674316)
          {
            this._switchAmount = 1f;
            this._animationState &= ~AnimatedSwitchView.AnimationState.SwitchingOn;
          }
        }
        else if (this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.SwitchingOff))
        {
          this._switchAmount = Mathf.Lerp(this._switchAmount, 0.0f, Time.deltaTime * this._switchAnimationSmooth);
          if ((double) this._switchAmount <= 0.0099999997764825821)
          {
            this._switchAmount = 0.0f;
            this._animationState &= ~AnimatedSwitchView.AnimationState.SwitchingOff;
          }
        }
        if (this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.HighlightingOn) && (double) this._disabledAmount <= 0.0)
        {
          this._highlightAmount += Time.deltaTime / this._highlightAnimationDuration;
          if ((double) this._highlightAmount >= 1.0)
          {
            this._highlightAmount = 1f;
            this._animationState &= ~AnimatedSwitchView.AnimationState.HighlightingOn;
          }
        }
        else if (this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.HighlightingOff))
        {
          this._highlightAmount -= Time.deltaTime / this._highlightAnimationDuration;
          if ((double) this._highlightAmount <= 0.0)
          {
            this._highlightAmount = 0.0f;
            this._animationState &= ~AnimatedSwitchView.AnimationState.HighlightingOff;
          }
        }
        if (this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.DisablingOn))
        {
          this._disabledAmount += Time.deltaTime / this._disableAnimationDuration;
          if ((double) this._disabledAmount >= 1.0)
          {
            this._disabledAmount = 1f;
            this._animationState &= ~AnimatedSwitchView.AnimationState.DisablingOn;
          }
        }
        else if (this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.DisablingOff))
        {
          this._disabledAmount -= Time.deltaTime / this._disableAnimationDuration;
          if ((double) this._disabledAmount <= 0.0)
          {
            this._disabledAmount = 0.0f;
            this._animationState &= ~AnimatedSwitchView.AnimationState.DisablingOff;
          }
        }
        this.LerpColors(this._switchAmount, this._highlightAmount, this._disabledAmount);
        this.LerpPosition(this._switchAmount);
        if (Mathf.Approximately(0.0f, this._horizontalStretchAmount))
          return;
        this.LerpStretch(this._switchAmount);
      }
    }

        public virtual void LerpPosition(float switchAmount)
        {
            Vector2 anchorMin = this._knobRectTransform.anchorMin;
            anchorMin.x = switchAmount;
            this._knobRectTransform.anchorMin = anchorMin;
            Vector2 anchorMax = this._knobRectTransform.anchorMax;
            anchorMax.x = switchAmount;
            this._knobRectTransform.anchorMax = anchorMax;
        }

        public virtual void LerpStretch(float switchAmount)
        {
            float num = 1f - Mathf.Abs(switchAmount - 0.5f) * 2f;
            float x = this._originalKnobWidth * (1f + this._horizontalStretchAmount * num);
            float y = this._originalKnobHeight * (1f - this._verticalStretchAmount * num);
            Vector2 sizeDelta = this._knobRectTransform.sizeDelta;
            sizeDelta.x = x;
            sizeDelta.y = y;
            this._knobRectTransform.sizeDelta = sizeDelta;
        }

        public virtual void LerpColors(float switchAmount, float highlightAmount, float disabledAmount)
    {
      this._backgroundImage.color = this.LerpColor(switchAmount, highlightAmount, disabledAmount, (AnimatedSwitchView.GetColorDelegate) (colorBlock => colorBlock.backgroundColor));
      if (this._backgroundImage.gradient)
      {
        this._backgroundImage.color0 = this.LerpColor(switchAmount, highlightAmount, disabledAmount, (AnimatedSwitchView.GetColorDelegate) (colorBlock => colorBlock.backgroundColor0));
        this._backgroundImage.color1 = this.LerpColor(switchAmount, highlightAmount, disabledAmount, (AnimatedSwitchView.GetColorDelegate) (colorBlock => colorBlock.backgroundColor1));
      }
      this._knobImage.color = this.LerpColor(switchAmount, highlightAmount, disabledAmount, (AnimatedSwitchView.GetColorDelegate) (colorBlock => colorBlock.knobColor));
      if (this._knobImage.gradient)
      {
        this._knobImage.color0 = this.LerpColor(switchAmount, highlightAmount, disabledAmount, (AnimatedSwitchView.GetColorDelegate) (colorBlock => colorBlock.knobColor0));
        this._knobImage.color1 = this.LerpColor(switchAmount, highlightAmount, disabledAmount, (AnimatedSwitchView.GetColorDelegate) (colorBlock => colorBlock.knobColor1));
      }
      this._onText.alpha = switchAmount;
      this._offText.alpha = (float) ((1.0 - (double) switchAmount) * 0.5);
    }

    public virtual Color LerpColor(
      float switchAmount,
      float highlightAmount,
      float disabledAmount,
      AnimatedSwitchView.GetColorDelegate getColorDelegate)
    {
      return Color.Lerp(Color.Lerp(Color.Lerp(getColorDelegate(this._offColors), getColorDelegate(this._onColors), switchAmount), Color.Lerp(getColorDelegate(this._offHighlightedColors), getColorDelegate(this._onHighlightedColors), switchAmount), highlightAmount), getColorDelegate(this._disabledColors), disabledAmount);
    }

    public virtual void HandleOnValueChanged(bool value)
    {
      if (value)
      {
        this._animationState |= AnimatedSwitchView.AnimationState.SwitchingOn;
        this._animationState &= ~AnimatedSwitchView.AnimationState.SwitchingOff;
      }
      else
      {
        this._animationState |= AnimatedSwitchView.AnimationState.SwitchingOff;
        this._animationState &= ~AnimatedSwitchView.AnimationState.SwitchingOn;
      }
      this.enabled = true;
    }

    public virtual void HandleStateDidChange(ToggleWithCallbacks.SelectionState selectionState)
    {
      if (selectionState == ToggleWithCallbacks.SelectionState.Disabled)
      {
        this._animationState |= AnimatedSwitchView.AnimationState.DisablingOn;
        this._animationState &= ~AnimatedSwitchView.AnimationState.DisablingOff;
      }
      else
      {
        this._animationState |= AnimatedSwitchView.AnimationState.DisablingOff;
        this._animationState &= ~AnimatedSwitchView.AnimationState.DisablingOn;
      }
      if (selectionState == ToggleWithCallbacks.SelectionState.Highlighted || selectionState == ToggleWithCallbacks.SelectionState.Pressed || selectionState == ToggleWithCallbacks.SelectionState.Selected)
      {
        if ((double) this._disabledAmount <= 0.0 || this._animationState.HasFlag((Enum) AnimatedSwitchView.AnimationState.DisablingOff))
        {
          this._animationState |= AnimatedSwitchView.AnimationState.HighlightingOn;
          this._animationState &= ~AnimatedSwitchView.AnimationState.HighlightingOff;
        }
      }
      else
      {
        this._animationState |= AnimatedSwitchView.AnimationState.HighlightingOff;
        this._animationState &= ~AnimatedSwitchView.AnimationState.HighlightingOn;
      }
      this.enabled = true;
    }

    [Serializable]
    public class ColorBlock
    {
      public Color knobColor = Color.white;
      public Color knobColor0 = Color.white;
      public Color knobColor1 = Color.white;
      [Space]
      public Color backgroundColor = Color.white;
      public Color backgroundColor0 = Color.white;
      public Color backgroundColor1 = Color.white;
    }

    [Flags]
    public enum AnimationState
    {
      Idle = 0,
      SwitchingOn = 1,
      SwitchingOff = 2,
      HighlightingOn = 4,
      HighlightingOff = 8,
      DisablingOn = 16, // 0x00000010
      DisablingOff = 32, // 0x00000020
    }

    public delegate Color GetColorDelegate(AnimatedSwitchView.ColorBlock colorBlock);
  }
}
