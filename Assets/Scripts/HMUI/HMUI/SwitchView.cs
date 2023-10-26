// Decompiled with JetBrains decompiler
// Type: HMUI.SwitchView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace HMUI
{
  [DisallowMultipleComponent]
  [RequireComponent(typeof (ToggleWithCallbacks))]
  public class SwitchView : MonoBehaviour
  {
    [SerializeField]
    protected SwitchView.AnimationType _animationType;
    [SerializeField]
    protected AnimationClip _normalAnimationClip;
    [SerializeField]
    protected AnimationClip _highlightedAnimationClip;
    [SerializeField]
    protected AnimationClip _pressedAnimationClip;
    [SerializeField]
    protected AnimationClip _disabledAnimationClip;
    [Space]
    [DrawIf("_animationType", SwitchView.AnimationType.OnOff, DrawIfAttribute.DisablingType.DontDraw)]
    [NullAllowed("_animationType", SwitchView.AnimationType.OnOff)]
    [SerializeField]
    protected AnimationClip _onAnimationClip;
    [DrawIf("_animationType", SwitchView.AnimationType.OnOff, DrawIfAttribute.DisablingType.DontDraw)]
    [NullAllowed("_animationType", SwitchView.AnimationType.OnOff)]
    [SerializeField]
    protected AnimationClip _offAnimationClip;
    [DrawIf("_animationType", SwitchView.AnimationType.SelectedState, DrawIfAttribute.DisablingType.DontDraw)]
    [NullAllowed("_animationType", SwitchView.AnimationType.SelectedState)]
    [SerializeField]
    protected AnimationClip _selectedAnimationClip;
    [DrawIf("_animationType", SwitchView.AnimationType.SelectedState, DrawIfAttribute.DisablingType.DontDraw)]
    [NullAllowed("_animationType", SwitchView.AnimationType.SelectedState)]
    [SerializeField]
    protected AnimationClip _selectedAndHighlightedAnimationClip;
    protected ToggleWithCallbacks _toggle;

    public virtual void Awake() => this._toggle = this.GetComponent<ToggleWithCallbacks>();

    public virtual void Start()
    {
      this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleOnValueChanged));
      this._toggle.stateDidChangeEvent += new Action<ToggleWithCallbacks.SelectionState>(this.HandleStateDidChange);
      this.RefreshVisuals();
    }

    public virtual void OnDestroy()
    {
      this._toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleOnValueChanged));
      this._toggle.stateDidChangeEvent -= new Action<ToggleWithCallbacks.SelectionState>(this.HandleStateDidChange);
    }

    public virtual void HandleOnValueChanged(bool value) => this.RefreshVisuals();

    public virtual void HandleStateDidChange(ToggleWithCallbacks.SelectionState value) => this.RefreshVisuals();

    public virtual void RefreshVisuals()
    {
      if (this._animationType == SwitchView.AnimationType.OnOff)
      {
        if (this._toggle.isOn)
          this._onAnimationClip.SampleAnimation(this.gameObject, 0.0f);
        else
          this._offAnimationClip.SampleAnimation(this.gameObject, 0.0f);
      }
      switch (this._toggle.selectionState)
      {
        case ToggleWithCallbacks.SelectionState.Normal:
          if (this._animationType == SwitchView.AnimationType.SelectedState && this._toggle.isOn)
          {
            this._selectedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
            break;
          }
          this._normalAnimationClip.SampleAnimation(this.gameObject, 0.0f);
          break;
        case ToggleWithCallbacks.SelectionState.Highlighted:
          if (this._animationType == SwitchView.AnimationType.SelectedState && this._toggle.isOn)
          {
            this._selectedAndHighlightedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
            break;
          }
          this._highlightedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
          break;
        case ToggleWithCallbacks.SelectionState.Pressed:
          this._pressedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
          break;
        case ToggleWithCallbacks.SelectionState.Disabled:
          this._disabledAnimationClip.SampleAnimation(this.gameObject, 0.0f);
          break;
      }
    }

    public enum AnimationType
    {
      OnOff,
      SelectedState,
    }
  }
}
