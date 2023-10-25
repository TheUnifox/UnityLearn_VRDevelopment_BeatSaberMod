// Decompiled with JetBrains decompiler
// Type: ColorGraphicStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Transitions/Color Graphic Transition")]
[RequireComponent(typeof (Graphic))]
public class ColorGraphicStateTransition : BaseStateTransition<Graphic>
{
  [Space]
  [SerializeField]
  protected ColorTransitionSO _transition;
  protected ColorTween _colorTween;

  protected override BaseTransitionSO transition => (BaseTransitionSO) this._transition;

  protected override void TransitionToNormalState() => this.StartTween(this._transition.normalColor);

  protected override void TransitionToHighlightedState() => this.StartTween(this._transition.highlightedColor);

  protected override void TransitionToPressedState() => this.StartTween(this._transition.pressedColor);

  protected override void TransitionToDisabledState() => this.StartTween(this._transition.disabledColor);

  protected override void TransitionToSelectedState() => this.StartTween(this._transition.selectedColor);

  protected override void TransitionToSelectedAndHighlightedState() => this.StartTween(this._transition.selectedAndHighlightedColor);

  protected override void SetNormalState() => this._component.color = this._transition.normalColor;

  protected override void SetHighlightedState() => this._component.color = this._transition.highlightedColor;

  protected override void SetPressedState() => this._component.color = this._transition.pressedColor;

  protected override void SetDisabledState() => this._component.color = this._transition.disabledColor;

  protected override void SetSelectedState() => this._component.color = this._transition.selectedColor;

  protected override void SetSelectedAndHighlightedState() => this._component.color = this._transition.selectedAndHighlightedColor;

  public virtual void StartTween(Color endColor)
  {
    if (this._colorTween != null)
    {
      ColorTween.Pool.Despawn(this._colorTween);
      this._colorTween = (ColorTween) null;
    }
    Color color1 = this._component.color;
    this._colorTween = ColorTween.Pool.Spawn(color1, endColor, (System.Action<Color>) (color => this._component.color = color), this._transition.easeDuration, this._transition.easeType, 0.0f);
    this._colorTween.onCompleted = (System.Action) (() =>
    {
      ColorTween.Pool.Despawn(this._colorTween);
      this._colorTween = (ColorTween) null;
    });
    this.tweeningManager.RestartTween((Tween) this._colorTween, (object) this);
  }

  [CompilerGenerated]
  public virtual void m_CStartTweenm_Eb__16_0(Color color) => this._component.color = color;

  [CompilerGenerated]
  public virtual void m_CStartTweenm_Eb__16_1()
  {
    ColorTween.Pool.Despawn(this._colorTween);
    this._colorTween = (ColorTween) null;
  }
}
