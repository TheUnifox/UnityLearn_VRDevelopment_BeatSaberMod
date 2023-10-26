// Decompiled with JetBrains decompiler
// Type: HMUI.InputFieldViewStaticAnimations
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class InputFieldViewStaticAnimations : MonoBehaviour
  {
    [SerializeField]
    protected InputFieldView _inputFieldView;
    [Space]
    [SerializeField]
    protected AnimationClip _normalClip;
    [SerializeField]
    protected AnimationClip _highlightedClip;
    [SerializeField]
    protected AnimationClip _pressedClip;
    [SerializeField]
    protected AnimationClip _disabledClip;
    [SerializeField]
    protected AnimationClip _selectedClip;
    protected bool _didStart;

    public virtual void Awake() => this._inputFieldView.selectionStateDidChangeEvent += new Action<InputFieldView.SelectionState>(this.HandleInputFieldViewSelectionStateDidChange);

    public virtual void Start()
    {
      this._didStart = true;
      this.HandleInputFieldViewSelectionStateDidChange(this._inputFieldView.selectionState);
    }

    public virtual void OnEnable() => this.HandleInputFieldViewSelectionStateDidChange(this._inputFieldView.selectionState);

    public virtual void OnDestroy()
    {
      if (!((UnityEngine.Object) this._inputFieldView != (UnityEngine.Object) null))
        return;
      this._inputFieldView.selectionStateDidChangeEvent -= new Action<InputFieldView.SelectionState>(this.HandleInputFieldViewSelectionStateDidChange);
    }

    public virtual void HandleInputFieldViewSelectionStateDidChange(
      InputFieldView.SelectionState state)
    {
      if (!this._didStart || !this.isActiveAndEnabled)
        return;
      AnimationClip animationClip = (AnimationClip) null;
      switch (state)
      {
        case InputFieldView.SelectionState.Normal:
          animationClip = this._normalClip;
          break;
        case InputFieldView.SelectionState.Highlighted:
          animationClip = this._highlightedClip;
          break;
        case InputFieldView.SelectionState.Pressed:
          animationClip = this._pressedClip;
          break;
        case InputFieldView.SelectionState.Disabled:
          animationClip = this._disabledClip;
          break;
        case InputFieldView.SelectionState.Selected:
          animationClip = this._selectedClip;
          break;
      }
      if (!((UnityEngine.Object) animationClip != (UnityEngine.Object) null))
        return;
      animationClip.SampleAnimation(this.gameObject, 0.0f);
    }
  }
}
