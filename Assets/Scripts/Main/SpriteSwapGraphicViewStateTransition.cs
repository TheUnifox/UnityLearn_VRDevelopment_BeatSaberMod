// Decompiled with JetBrains decompiler
// Type: SpriteSwapGraphicViewStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Transitions/Sprite Swap Transition")]
public class SpriteSwapGraphicViewStateTransition : BaseStateTransition<Image>
{
  [Space]
  [SerializeField]
  protected SpriteSwapTransitionSO _transition;

  protected override BaseTransitionSO transition => (BaseTransitionSO) this._transition;

  protected override void TransitionToNormalState() => this.SetNormalState();

  protected override void TransitionToHighlightedState() => this.SetHighlightedState();

  protected override void TransitionToPressedState() => this.SetPressedState();

  protected override void TransitionToDisabledState() => this.SetDisabledState();

  protected override void TransitionToSelectedState() => this.SetSelectedState();

  protected override void TransitionToSelectedAndHighlightedState() => this.SetSelectedAndHighlightedState();

  protected override void SetNormalState() => this._component.sprite = this._transition.normalSprite;

  protected override void SetHighlightedState() => this._component.sprite = this._transition.highlightedSprite;

  protected override void SetPressedState() => this._component.sprite = this._transition.pressedSprite;

  protected override void SetDisabledState() => this._component.sprite = this._transition.disabledSprite;

  protected override void SetSelectedState() => this._component.sprite = this._transition.selectedSprite;

  protected override void SetSelectedAndHighlightedState() => this._component.sprite = this._transition.selectedAndHighlightedSprite;
}
