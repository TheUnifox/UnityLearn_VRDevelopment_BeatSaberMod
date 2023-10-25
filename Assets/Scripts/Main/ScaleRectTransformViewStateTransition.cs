// Decompiled with JetBrains decompiler
// Type: ScaleRectTransformViewStateTransition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;

[AddComponentMenu("Transitions/Scale Rect Transform Transition")]
public class ScaleRectTransformViewStateTransition : BaseStateTransition<RectTransform>
{
  [Space]
  [SerializeField]
  protected Vector3TransitionSO _transition;
  protected Vector3Tween _vectorTween;

  protected override BaseTransitionSO transition => (BaseTransitionSO) this._transition;

  protected override void TransitionToNormalState() => this.StartTween(this._transition.normalState);

  protected override void TransitionToHighlightedState() => this.StartTween(this._transition.highlightedState);

  protected override void TransitionToPressedState() => this.StartTween(this._transition.pressedState);

  protected override void TransitionToDisabledState() => this.StartTween(this._transition.disabledState);

  protected override void TransitionToSelectedState() => this.StartTween(this._transition.selectedState);

  protected override void TransitionToSelectedAndHighlightedState() => this.StartTween(this._transition.selectedAndHighlightedState);

  protected override void SetNormalState() => this._component.localScale = this._transition.normalState;

  protected override void SetHighlightedState() => this._component.localScale = this._transition.highlightedState;

  protected override void SetPressedState() => this._component.localScale = this._transition.pressedState;

  protected override void SetDisabledState() => this._component.localScale = this._transition.disabledState;

  protected override void SetSelectedState() => this._component.localScale = this._transition.selectedState;

  protected override void SetSelectedAndHighlightedState() => this._component.localScale = this._transition.selectedAndHighlightedState;

  public virtual void StartTween(Vector3 endScale)
  {
    if (this._vectorTween != null)
    {
      Vector3Tween.Pool.Despawn(this._vectorTween);
      this._vectorTween = (Vector3Tween) null;
    }
    Vector3 localScale = this._component.localScale;
    this._vectorTween = Vector3Tween.Pool.Spawn(localScale, endScale, (System.Action<Vector3>) (pos => this._component.localScale = pos), this._transition.easeDuration, this._transition.easeType, 0.0f);
    this._vectorTween.onCompleted = (System.Action) (() =>
    {
      Vector3Tween.Pool.Despawn(this._vectorTween);
      this._vectorTween = (Vector3Tween) null;
    });
    this.tweeningManager.RestartTween((Tween) this._vectorTween, (object) this);
  }

  [CompilerGenerated]
  public virtual void m_CStartTweenm_Eb__16_0(Vector3 pos) => this._component.localScale = pos;

  [CompilerGenerated]
  public virtual void m_CStartTweenm_Eb__16_1()
  {
    Vector3Tween.Pool.Despawn(this._vectorTween);
    this._vectorTween = (Vector3Tween) null;
  }
}
