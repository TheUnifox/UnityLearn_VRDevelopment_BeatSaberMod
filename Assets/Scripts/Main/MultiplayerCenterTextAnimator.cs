// Decompiled with JetBrains decompiler
// Type: MultiplayerCenterTextAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using TMPro;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerCenterTextAnimator : MonoBehaviour
{
  [Space]
  [SerializeField]
  protected TextMeshPro _text;
  [SerializeField]
  protected Transform _scalingTarget;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected Tween<float> _fadeInTween;
  protected Tween<float> _fadeOutTween;
  protected Tween<float> _fontSizeTween;
  protected Tween<Color> _colorTween;
  protected Tween<Vector3> _offsetTween;

  public virtual void Awake()
  {
    this._scalingTarget.localScale = Vector3.zero;
    this._fadeInTween = (Tween<float>) new Tweening.FloatTween(0.0f, 1f, (System.Action<float>) (val => this._scalingTarget.localScale = Vector3.one * val), 0.0f, EaseType.Linear);
    this._fadeOutTween = (Tween<float>) new Tweening.FloatTween(1f, 0.0f, (System.Action<float>) (val => this._scalingTarget.localScale = Vector3.one * val), 0.0f, EaseType.Linear);
    Tween<float> fadeOutTween = this._fadeOutTween;
    fadeOutTween.onCompleted = fadeOutTween.onCompleted + (System.Action) (() => this._scalingTarget.gameObject.SetActive(false));
    this._fontSizeTween = (Tween<float>) new Tweening.FloatTween(0.0f, 1f, (System.Action<float>) (val => this._text.fontSize = val), 0.0f, EaseType.Linear);
    this._colorTween = (Tween<Color>) new Tweening.ColorTween(Color.white, Color.white, (System.Action<Color>) (val => this._text.color = val), 0.0f, EaseType.Linear);
    this._offsetTween = (Tween<Vector3>) new Vector3Tween(Vector3.zero, Vector3.zero, (System.Action<Vector3>) (val => this._scalingTarget.localPosition = val), 0.0f, EaseType.Linear);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void AnimateTextColor(Color color, float duration, EaseType easeType)
  {
    this._colorTween.duration = duration;
    this._colorTween.fromValue = this._text.color;
    this._colorTween.toValue = color;
    this._colorTween.easeType = easeType;
    this._tweeningManager.RestartTween((Tween) this._colorTween, (object) this);
  }

  public virtual void AnimateFontSize(float fontSize, float duration, EaseType easeType)
  {
    this._fontSizeTween.duration = duration;
    this._fontSizeTween.fromValue = this._text.fontSize;
    this._fontSizeTween.toValue = fontSize;
    this._fontSizeTween.easeType = easeType;
    this._tweeningManager.RestartTween((Tween) this._fontSizeTween, (object) this);
  }

  public virtual void AnimatePositionOffsetSize(Vector3 offset, float duration, EaseType easeType)
  {
    this._offsetTween.duration = duration;
    this._offsetTween.fromValue = this._scalingTarget.localPosition;
    this._offsetTween.toValue = offset;
    this._offsetTween.easeType = easeType;
    this._tweeningManager.RestartTween((Tween) this._offsetTween, (object) this);
  }

  public virtual void AnimateEnabled(bool isEnabled, float duration, EaseType easeType)
  {
    this._fadeInTween.Kill();
    this._fadeOutTween.Kill();
    if ((double) duration > 0.0)
    {
      if (isEnabled)
      {
        this._scalingTarget.gameObject.SetActive(true);
        if ((double) this._scalingTarget.localScale.x >= 1.0)
          return;
        this._fadeInTween.easeType = easeType;
        this._fadeInTween.duration = duration;
        this._fadeInTween.fromValue = this._scalingTarget.localScale.x;
        this._tweeningManager.RestartTween((Tween) this._fadeInTween, (object) this);
      }
      else
      {
        if ((double) this._scalingTarget.localScale.x <= 0.0)
          return;
        this._fadeOutTween.easeType = easeType;
        this._fadeOutTween.duration = duration;
        this._fadeOutTween.fromValue = this._scalingTarget.localScale.x;
        this._tweeningManager.RestartTween((Tween) this._fadeOutTween, (object) this);
      }
    }
    else
      this._scalingTarget.gameObject.SetActive(isEnabled);
  }

  public virtual void SetText(string text) => this._text.text = text;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__8_0(float val) => this._scalingTarget.localScale = Vector3.one * val;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__8_1(float val) => this._scalingTarget.localScale = Vector3.one * val;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__8_5() => this._scalingTarget.gameObject.SetActive(false);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__8_2(float val) => this._text.fontSize = val;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__8_3(Color val) => this._text.color = val;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__8_4(Vector3 val) => this._scalingTarget.localPosition = val;
}
