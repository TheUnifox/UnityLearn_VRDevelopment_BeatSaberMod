// Decompiled with JetBrains decompiler
// Type: HMUI.ButtonStaticAnimations
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class ButtonStaticAnimations : MonoBehaviour
  {
    [SerializeField]
    protected NoTransitionsButton _button;
    [Space]
    [SerializeField]
    protected AnimationClip _normalClip;
    [SerializeField]
    protected AnimationClip _highlightedClip;
    [SerializeField]
    protected AnimationClip _pressedClip;
    [SerializeField]
    protected AnimationClip _disabledClip;
    protected bool _didStart;

    public virtual void Awake() => this._button.selectionStateDidChangeEvent += new Action<NoTransitionsButton.SelectionState>(this.HandleButtonSelectionStateDidChange);

    public virtual void Start()
    {
      this._didStart = true;
      this.HandleButtonSelectionStateDidChange(this._button.selectionState);
    }

    public virtual void OnEnable() => this.HandleButtonSelectionStateDidChange(this._button.selectionState);

    public virtual void OnDestroy()
    {
      if (!((UnityEngine.Object) this._button != (UnityEngine.Object) null))
        return;
      this._button.selectionStateDidChangeEvent -= new Action<NoTransitionsButton.SelectionState>(this.HandleButtonSelectionStateDidChange);
    }

    public virtual void HandleButtonSelectionStateDidChange(NoTransitionsButton.SelectionState state)
    {
      if (!this._didStart || !this.isActiveAndEnabled)
        return;
      AnimationClip animationClip = (AnimationClip) null;
      switch (state)
      {
        case NoTransitionsButton.SelectionState.Normal:
          animationClip = this._normalClip;
          break;
        case NoTransitionsButton.SelectionState.Highlighted:
          animationClip = this._highlightedClip;
          break;
        case NoTransitionsButton.SelectionState.Pressed:
          animationClip = this._pressedClip;
          break;
        case NoTransitionsButton.SelectionState.Disabled:
          animationClip = this._disabledClip;
          break;
      }
      if (!((UnityEngine.Object) animationClip != (UnityEngine.Object) null))
        return;
      animationClip.SampleAnimation(this.gameObject, 0.0f);
    }
  }
}
