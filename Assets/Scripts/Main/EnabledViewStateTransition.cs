// Decompiled with JetBrains decompiler
// Type: EnabledViewStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[AddComponentMenu("Transitions/Enabled State Transition")]
public class EnabledViewStateTransition : BaseStateTransition<Behaviour>
{
  [Space]
  [SerializeField]
  protected EnabledTransitionSO _transition;

  protected override BaseTransitionSO transition => (BaseTransitionSO) this._transition;

  protected override void TransitionToNormalState() => this.SetNormalState();

  protected override void TransitionToHighlightedState() => this.SetHighlightedState();

  protected override void TransitionToPressedState() => this.SetPressedState();

  protected override void TransitionToDisabledState() => this.SetDisabledState();

  protected override void TransitionToSelectedState() => this.SetSelectedState();

  protected override void TransitionToSelectedAndHighlightedState() => this.SetSelectedAndHighlightedState();

  protected override void SetNormalState() => this._component.enabled = this._transition.normalState;

  protected override void SetHighlightedState() => this._component.enabled = this._transition.highlightedState;

  protected override void SetPressedState() => this._component.enabled = this._transition.pressedState;

  protected override void SetDisabledState() => this._component.enabled = this._transition.disabledState;

  protected override void SetSelectedState() => this._component.enabled = this._transition.selectedState;

  protected override void SetSelectedAndHighlightedState() => this._component.enabled = this._transition.selectedAndHighlightedState;
}
