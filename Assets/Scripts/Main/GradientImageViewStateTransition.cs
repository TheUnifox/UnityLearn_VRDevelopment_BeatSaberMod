// Decompiled with JetBrains decompiler
// Type: GradientImageViewStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;

[AddComponentMenu("Transitions/Gradient State Transition")]
[RequireComponent(typeof (ImageView))]
public class GradientImageViewStateTransition : BaseStateTransition<ImageView>
{
  [Space]
  [SerializeField]
  protected GradientTransitionSO _transition;
  protected ColorTween _colorTweenA;
  protected ColorTween _colorTweenB;

  protected override BaseTransitionSO transition => (BaseTransitionSO) this._transition;

  protected override void TransitionToNormalState() => this.StartTweens(this._transition.normalColor1, this._transition.normalColor2);

  protected override void TransitionToHighlightedState() => this.StartTweens(this._transition.highlightColor1, this._transition.highlightColor2);

  protected override void TransitionToPressedState() => this.StartTweens(this._transition.pressedColor1, this._transition.pressedColor2);

  protected override void TransitionToDisabledState() => this.StartTweens(this._transition.disabledColor1, this._transition.disabledColor2);

  protected override void TransitionToSelectedState() => this.StartTweens(this._transition.selectedColor1, this._transition.selectedColor2);

  protected override void TransitionToSelectedAndHighlightedState() => this.StartTweens(this._transition.selectedAndHighlightedColor1, this._transition.selectedAndHighlightedColor2);

  protected override void SetNormalState() => this.SetColors(this._transition.normalColor1, this._transition.normalColor2);

  protected override void SetHighlightedState() => this.SetColors(this._transition.highlightColor1, this._transition.highlightColor2);

  protected override void SetPressedState() => this.SetColors(this._transition.pressedColor1, this._transition.pressedColor2);

  protected override void SetDisabledState() => this.SetColors(this._transition.disabledColor1, this._transition.disabledColor2);

  protected override void SetSelectedState() => this.SetColors(this._transition.selectedColor1, this._transition.selectedColor2);

  protected override void SetSelectedAndHighlightedState() => this.SetColors(this._transition.selectedAndHighlightedColor1, this._transition.selectedAndHighlightedColor2);

  public virtual void StartTweens(Color endColor1, Color endColor2)
  {
    Color color0 = this._component.color0;
    Color color1 = this._component.color1;
    this.StartTween(color0, endColor1, (System.Action<Color>) (color => this._component.color0 = color), (System.Action) (() =>
    {
      ColorTween.Pool.Despawn(this._colorTweenA);
      this._colorTweenA = (ColorTween) null;
    }), ref this._colorTweenA);
    this.StartTween(color1, endColor2, (System.Action<Color>) (color => this._component.color1 = color), (System.Action) (() =>
    {
      ColorTween.Pool.Despawn(this._colorTweenB);
      this._colorTweenB = (ColorTween) null;
    }), ref this._colorTweenB);
  }

  public virtual void StartTween(
    Color startColor,
    Color endColor,
    System.Action<Color> tweenAction,
    System.Action onCompleteAction,
    ref ColorTween colorTween)
  {
    if (colorTween != null)
    {
      ColorTween.Pool.Despawn(colorTween);
      colorTween = (ColorTween) null;
    }
    colorTween = ColorTween.Pool.Spawn(startColor, endColor, tweenAction, this._transition.easeDuration, this._transition.easeType, 0.0f);
    colorTween.onCompleted = onCompleteAction;
    this.tweeningManager.RestartTween((Tween) colorTween, (object) this);
  }

  public virtual void SetColors(Color startColor, Color endColor)
  {
    this._component.color0 = startColor;
    this._component.color1 = endColor;
  }

  [CompilerGenerated]
  public virtual void m_CStartTweensm_Eg__Color1CompleteAction()
  {
    ColorTween.Pool.Despawn(this._colorTweenA);
    this._colorTweenA = (ColorTween) null;
  }

  [CompilerGenerated]
  public virtual void m_CStartTweensm_Eg__Color2CompleteAction()
  {
    ColorTween.Pool.Despawn(this._colorTweenB);
    this._colorTweenB = (ColorTween) null;
  }

  [CompilerGenerated]
  public virtual void m_CStartTweensm_Eb__17_2(Color color) => this._component.color0 = color;

  [CompilerGenerated]
  public virtual void m_CStartTweensm_Eb__17_3(Color color) => this._component.color1 = color;
}
