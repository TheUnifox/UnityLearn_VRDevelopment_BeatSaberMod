// Decompiled with JetBrains decompiler
// Type: CanvasGroupStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;

[AddComponentMenu("Transitions/Canvas Group Transition")]
[RequireComponent(typeof (CanvasGroup))]
public class CanvasGroupStateTransition : BaseStateTransition<CanvasGroup>
{
  [Space]
  [SerializeField]
  protected CanvasGroupTransitionSO _transition;
  protected FloatTween _floatTween;

  protected override BaseTransitionSO transition => (BaseTransitionSO) this._transition;

  protected override void TransitionToNormalState() => this.StartTween(this._transition.normalAlpha);

  protected override void TransitionToHighlightedState() => this.StartTween(this._transition.highlightedAlpha);

  protected override void TransitionToPressedState() => this.StartTween(this._transition.pressedAlpha);

  protected override void TransitionToDisabledState() => this.StartTween(this._transition.disabledAlpha);

  protected override void TransitionToSelectedState() => this.StartTween(this._transition.selectedAlpha);

  protected override void TransitionToSelectedAndHighlightedState() => this.StartTween(this._transition.selectedAndHighlightedAlpha);

  protected override void SetNormalState() => this._component.alpha = this._transition.normalAlpha;

  protected override void SetHighlightedState() => this._component.alpha = this._transition.highlightedAlpha;

  protected override void SetPressedState() => this._component.alpha = this._transition.pressedAlpha;

  protected override void SetDisabledState() => this._component.alpha = this._transition.disabledAlpha;

  protected override void SetSelectedState() => this._component.alpha = this._transition.selectedAlpha;

  protected override void SetSelectedAndHighlightedState() => this._component.alpha = this._transition.selectedAndHighlightedAlpha;

  public virtual void StartTween(float endAlpha)
  {
    if (this._floatTween != null)
    {
      FloatTween.Pool.Despawn(this._floatTween);
      this._floatTween = (FloatTween) null;
    }
    float alpha1 = this._component.alpha;
    this._floatTween = FloatTween.Pool.Spawn(alpha1, endAlpha, (System.Action<float>) (alpha => this._component.alpha = alpha), this._transition.easeDuration, this._transition.easeType, 0.0f);
    this._floatTween.onCompleted = (System.Action) (() =>
    {
      FloatTween.Pool.Despawn(this._floatTween);
      this._floatTween = (FloatTween) null;
    });
    this.tweeningManager.RestartTween((Tween) this._floatTween, (object) this);
  }

  [CompilerGenerated]
  public virtual void m_CStartTweenm_Eb__16_0(float alpha) => this._component.alpha = alpha;

  [CompilerGenerated]
  public virtual void m_CStartTweenm_Eb__16_1()
  {
    FloatTween.Pool.Despawn(this._floatTween);
    this._floatTween = (FloatTween) null;
  }
}
